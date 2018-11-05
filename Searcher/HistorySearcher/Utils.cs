using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;

namespace MagnetX.Searcher.HistorySearcher
{
    /// <summary>
    /// 历史纪录的工具类。
    /// 提供了对历史文件流的操作。
    /// </summary>
    static class Utils
    {
        private static string History { get; } = "History.bin";
        private static Encoding Encoding { get; } = Encoding.UTF8;
        public static FileStream HistoryStreamForRead { get { return new FileStream(History, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite); } }
        public static FileStream HistoryStreamForWrite { get { return new FileStream(History, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read); } }

        public static async Task<Result> ReadResult(Stream stream)
        {
            try
            {
                int size = (stream.ReadByte() << 24)
                    + (stream.ReadByte() << 16)
                    + (stream.ReadByte() << 8)
                    + stream.ReadByte();
                byte[] buffer = new byte[size];

                await stream.ReadAsync(buffer, 0, size).ConfigureAwait(false);
                using (BinaryReader br = new BinaryReader(new DeflateStream(new MemoryStream(buffer), CompressionMode.Decompress), Encoding))
                {
                    Result result = new Result();
                    result.Name = br.ReadString();
                    result.Magnet = br.ReadString();
                    result.Size = br.ReadString();

                    return result;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static async Task<bool> WriteResult(Stream stream, Result result)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    using (BinaryWriter bw = new BinaryWriter(new DeflateStream(ms, CompressionMode.Compress, true), Encoding))
                    {
                        bw.Write(result.Name);
                        bw.Write(result.Magnet);
                        bw.Write(result.Size);
                    }

                    ms.Seek(0, SeekOrigin.Begin);
                    int size = (int)ms.Length;
                    await stream.WriteAsync(new byte[] {
                        (byte)(size >> 24),
                        (byte)(size >> 16),
                        (byte)(size >> 8),
                        (byte)(size),
                    }, 0, 4).ConfigureAwait(false);
                    await ms.CopyToAsync(stream).ConfigureAwait(false);

                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
