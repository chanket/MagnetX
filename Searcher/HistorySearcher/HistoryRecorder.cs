using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MagnetX.Searcher.HistorySearcher
{
    /// <summary>
    /// 历史记录的记录类。
    /// </summary>
    class HistoryRecorder
    {
        /// <summary>
        /// 记录的文件流。
        /// </summary>
        protected BufferedStream Stream { get; } = new BufferedStream(Utils.HistoryStreamForWrite);

        /// <summary>
        /// 判断重复记录的哈希表。
        /// </summary>
        protected HashSet<string> Set { get; } = new HashSet<string>();

        /// <summary>
        /// 单线程插入的信号量。
        /// </summary>
        protected SemaphoreSlim Lock { get; } = new SemaphoreSlim(0);

        protected async void Prepare()
        {
            using (Stream stream = new BufferedStream(Utils.HistoryStreamForRead))
            {
                Result result = await Utils.ReadResult(stream).ConfigureAwait(false);
                while (result != null)
                {
                    Set.Add(result.Magnet);

                    result = await Utils.ReadResult(stream).ConfigureAwait(false);
                }
            }
            
            Lock.Release();
        }

        public HistoryRecorder()
        {
            Prepare();
        }

        /// <summary>
        /// 插入一组历史纪录。
        /// </summary>
        public async Task Insert(IEnumerable<Result> results)
        {
            await Lock.WaitAsync().ConfigureAwait(false);
            {
                int count = 0;
                Stream.Seek(0, SeekOrigin.End);
                foreach (Result result in results)
                {
                    if (!Set.Contains(result.Magnet))
                    {
                        await Utils.WriteResult(Stream, result).ConfigureAwait(false);
                        count++;
                    }
                }
                if (count != 0) await Stream.FlushAsync().ConfigureAwait(false);
            }
            Lock.Release();
        }
    }
}
