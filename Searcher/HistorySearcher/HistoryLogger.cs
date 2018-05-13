using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MagnetX.Searcher.HistorySearcher
{
    class HistoryLogger
    {
        private static string connStr = @"Provider = Microsoft.Ace.OLEDB.12.0; Data Source = Cache.accdb; Jet OLEDB:Database Password=MAGNETX";
        private static string insertStr = @"INSERT INTO `Data` (`ID`, `Promote`, `Size`) VALUES (?, ?, ?)";
        private static string deleteStr = @"DELETE FROM `Data`";
        private static OleDbCommand BuildInsert(Result result)
        {
            OleDbCommand cmd = new OleDbCommand(insertStr);
            cmd.Parameters.AddWithValue("?", result.Magnet.Substring(20, 40));
            cmd.Parameters.AddWithValue("?", result.Name);
            cmd.Parameters.AddWithValue("?", result.Size);
            return cmd;
        }
        private static OleDbCommand BuildDelete()
        {
            OleDbCommand cmd = new OleDbCommand(deleteStr);
            return cmd;
        }
        private static async void Insert(object _instance)
        {
            HistoryLogger instance = _instance as HistoryLogger;
            List<Result> insertList = new List<Result>();

            while (instance.Running)
            {
                try
                {
                    using (var conn = new OleDbConnection(connStr))
                    {
                        await conn.OpenAsync().ConfigureAwait(false);

                        while (instance.Running)
                        {
                            await Task.Delay(1000).ConfigureAwait(false);

                            insertList.Clear();
                            lock (instance.InsertList)
                            {
                                insertList.AddRange(instance.InsertList);
                                instance.InsertList.Clear();
                            }

                            if (insertList.Count > 0)
                            {
                                foreach (var result in insertList)
                                {
                                    var cmd = BuildInsert(result);
                                    cmd.Connection = conn;

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
                catch {  }
            }
        }

        private Thread ThreadInsert { get; } = new Thread(new ParameterizedThreadStart(Insert));
        private bool Running { get; set; } = true;
        private List<Result> InsertList { get; } = new List<Result>();

        public HistoryLogger()
        {
            ThreadInsert.Start(this);
        }

        ~HistoryLogger()
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
                using (var conn = new OleDbConnection(connStr))
                {
                    await conn.OpenAsync().ConfigureAwait(false);
                    var cmd = BuildDelete();
                    cmd.Connection = conn;
                    await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
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
