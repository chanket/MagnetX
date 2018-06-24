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
            using (var conn = Utils.CreateConnection())
            {
                try
                {
                    await conn.OpenAsync().ConfigureAwait(false);
                    var cmd = Utils.BuildSearch(conn, words);
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
            using (var conn = Utils.CreateConnection())
            {
                try
                {
                    await conn.OpenAsync();
                    return TestResults.OK;
                }
                catch (Exception ex)
                {
                    return TestResults.ServerError;
                }
            }
        }
    }
}
