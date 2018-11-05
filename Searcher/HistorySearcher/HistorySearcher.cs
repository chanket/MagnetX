using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.IO.Compression;

namespace MagnetX.Searcher.HistorySearcher
{
    /// <summary>
    /// 历史记录的搜索类。
    /// </summary>
    class HistorySearcher : Searcher
    {
        protected bool Matches(string[] words, string name)
        {
            foreach(string word in words)
            {
                if (name.IndexOf(word) == -1) return false;
            }
            return true;
        }

        public override string Name
        {
            get
            {
                return "本地记录";
            }
        }

        public override async void SearchAsync(string word)
        {
            string[] words = word.Split(' ', '\t');
            List<Result> list = new List<Result>();
            try
            {
                using (BufferedStream fs = new BufferedStream(Utils.HistoryStreamForRead))
                {
                    while (true)
                    {
                        Result result = await Utils.ReadResult(fs).ConfigureAwait(false);
                        if (result == null) break;

                        result.From = Name;
                        if (Matches(words,result.Name))
                        {
                            list.Add(result);
                        }
                        if (list.Count == 100)
                        {
                            if (OnResults == null) break;
                            if (!OnResults.Invoke(this, list)) break;
                            list.Clear();
                        }
                    }
                }
            }
            catch { }

            try
            {
                if (list.Count != 0)
                {
                    if (OnResults != null) OnResults.Invoke(this, list);
                    list.Clear();
                }
            }
            catch { }
        }

        public override async Task<TestResults> TestAsync()
        {
            return TestResults.OK;
        }
    }
}
