using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Threading;
using System.Windows.Forms;

namespace MagnetX.Searcher.HistorySearcher
{
    class HistorySearcher : Searcher
    {
        private static string connStr = @"Provider = Microsoft.Ace.OLEDB.12.0; Data Source = Cache.accdb; Jet OLEDB:Database Password=MAGNETX";
        private static string searchPrefixStr = @"SELECT `ID`,`Promote`,`Size` FROM `Data`";
        private static string insertStr = @"INSERT INTO `Data` (`ID`, `Promote`, `Size`) VALUES (?, ?, ?)";
        private static OleDbCommand BuildSearch(string[] words)
        {
            string baseCmd = searchPrefixStr;
            for (int i = 0; i < words.Length; i++)
            {
                if (i == 0) baseCmd += " WHERE `Promote` LIKE ?";
                else baseCmd += " AND `Promote` LIKE ?";
            }

            OleDbCommand cmd = new OleDbCommand(baseCmd);
            for (int i = 0; i < words.Length; i++)
            {
                cmd.Parameters.AddWithValue("?", "%" + words[i] + "%");
            }

            return cmd;
        }
        private static OleDbCommand BuildInsert(Result result)
        {
            string promote;
            if (result.Name.Length > 255) promote = result.Name.Substring(0, 255);
            else promote = result.Name;

            OleDbCommand cmd = new OleDbCommand(insertStr);
            cmd.Parameters.AddWithValue("?", result.Magnet.Substring(20, 40));
            cmd.Parameters.AddWithValue("?", result.Name);
            cmd.Parameters.AddWithValue("?", result.Size);
            return cmd;
        }
        private static async void Insert(object _instance)
        {
            HistorySearcher instance = _instance as HistorySearcher;
            List<Result> insertList = new List<Result>();

            while (true)
            {
                try
                {
                    using (var conn = new OleDbConnection(connStr))
                    {
                        await conn.OpenAsync().ConfigureAwait(false);

                        while (true)
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
                                    catch
                                    {
                                    }
                                }
                            }
                        }
                    }
                }
                catch
                {
                    //Connection Timeout
                }
            }
        }

        private List<Result> InsertList { get; } = new List<Result>();

        private Thread ThreadInsert { get; } = new Thread(new ParameterizedThreadStart(Insert));

        public HistorySearcher()
        {
            ThreadInsert.Start(this);
        }

        public override string Name
        {
            get
            {
                return "本地记录";
            }
        }

        public override async Task SearchAsync(string word)
        {
            string[] words = word.Split(' ', '\t');
            using (var conn = new OleDbConnection(connStr))
            {
                try
                {
                    await conn.OpenAsync().ConfigureAwait(false);
                    var cmd = BuildSearch(words);
                    cmd.Connection = conn;
                    var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false);

                    List<Result> results = new List<Result>();
                    while (await reader.ReadAsync().ConfigureAwait(false))
                    {
                        string magnet = await reader.GetFieldValueAsync<string>(0).ConfigureAwait(false);
                        string promote = await reader.GetFieldValueAsync<string>(1).ConfigureAwait(false);
                        string size = await reader.GetFieldValueAsync<string>(2).ConfigureAwait(false);
                        results.Add(new Result() {
                            Magnet = "magnet:?xt=urn:btih:" + magnet,
                            Name = promote,
                            From = Name,
                            Size = size,
                        });

                        if (results.Count == 100)
                        {
                            if (OnResults == null) break;
                            else if (!OnResults.Invoke(this, results)) break;
                            results.Clear();
                        }
                    }
                    if (results.Count != 0) OnResults?.Invoke(this, results);
                }
                catch { }
            }
        }

        public override async Task<TestResults> TestAsync()
        {
            using (var conn = new OleDbConnection(connStr))
            {
                try
                {
                    await conn.OpenAsync();
                    return TestResults.OK;
                }
                catch (Exception ex)
                {
                    return TestResults.Unusable;
                }
            }
        }

        public void InsertAsync(IEnumerable<Result> results)
        {
            lock (InsertList)
            {
                InsertList.AddRange(results);
            }
        }

    }
}
