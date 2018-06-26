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
    class HistoryRecorder
    {
        protected BufferedStream Stream { get; } = new BufferedStream(Utils.HistoryFileStreamWrite);

        protected HashSet<string> Set { get; } = new HashSet<string>();

        protected SemaphoreSlim Lock { get; } = new SemaphoreSlim(0);

        protected async void Prepare()
        {
            using (Stream stream = new BufferedStream(Utils.HistoryFileStreamRead))
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
