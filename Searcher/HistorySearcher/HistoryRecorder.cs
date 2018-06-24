using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MagnetX.Searcher.HistorySearcher
{
    class HistoryRecorder
    {
        private bool Running { get; set; } = true;
        private List<Result> InsertList { get; } = new List<Result>();
        private async void InsertLoop()
        {
            List<Result> insertList = new List<Result>();

            while (Running)
            {
                try
                {
                    using (var conn = Utils.CreateConnection())
                    {
                        await conn.OpenAsync().ConfigureAwait(false);

                        while (Running)
                        {
                            await Task.Delay(5000).ConfigureAwait(false);

                            insertList.Clear();
                            lock (InsertList)
                            {
                                insertList.AddRange(InsertList);
                                InsertList.Clear();
                            }

                            if (insertList.Count > 0)
                            {
                                foreach (var result in insertList)
                                {
                                    var cmd = Utils.BuildInsert(conn, result);

                                    try
                                    {
                                        await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                                    }
                                    catch {  }
                                }
                            }
                        }
                    }
                }
                catch
                {
                    await Task.Delay(1000).ConfigureAwait(false);
                }
            }
        }

        public HistoryRecorder()
        {
            InsertLoop();
        }

        ~HistoryRecorder()
        {
            Running = false;
        }

        public void Insert(IEnumerable<Result> results)
        {
            lock (InsertList)
            {
                InsertList.AddRange(results);
            }
        }

        public async Task<bool> ClearAsync()
        {
            try
            {
                using (var conn = Utils.CreateConnection())
                {
                    await conn.OpenAsync();
                    var cmd = Utils.BuildDelete(conn);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
