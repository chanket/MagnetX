using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MagnetX.Searcher.WebSearcher;
using MagnetX.Searcher.HistorySearcher;

namespace MagnetX
{
    static class Utils
    {
        private static Settings Settings { get; set; }

        public static HistoryRecorder HistoryRecorder { get; set; } 

        public static IEnumerable<Searcher.Searcher> GetAllSearchers()
        {
            yield return new HistorySearcher();
            yield return new Bt177WebSearcher();
            yield return new CilisharexWebSearcher();
            yield return new BtmuleWebSearcher();
            yield return new ZhongzisoWebSearcher();
            yield return new CnbtkittyWebSearcher();
            yield return new BtcatWebSearcher();
            yield return new IdopeWebSearcher();
            yield return new BaocaibtWebSearcher();
            yield return new CilibaWebSearcher();
            yield return new BtsoWebSearcher();
            yield return new DhtseakWebSearcher();
            yield return new SomagnetWebSearcher();
            yield return new WtsqyyWebSearcher();
            yield return new CililianxWebSearcher();
        }

        public static void Init()
        {
            Searcher.HistorySearcher.Utils.Init();
            HistoryRecorder = new HistoryRecorder();

            Settings = Settings.Load();
        }

        public static bool GetSearcherEnabled(Searcher.Searcher s)
        {
            if (s == null)
            {
                return false;
            }
            string[] ignoredSearcher = Settings.IgnoredSearcher;
            foreach (string b in ignoredSearcher)
            {
                if (s.Name == b)
                {
                    return false;
                }
            }
            return true;
        }

        public static void SetSearcherEnabled(Searcher.Searcher s, bool b)
        {
            if (s != null)
            {
                HashSet<string> hashSet = new HashSet<string>(Settings.IgnoredSearcher);
                try
                {
                    if (b)
                    {
                        hashSet.Remove(s.Name);
                    }
                    else
                    {
                        hashSet.Add(s.Name);
                    }
                }
                catch
                {
                }
                Settings.IgnoredSearcher = hashSet.ToArray();
                Settings.Save(Settings);
            }
        }

        public static bool RecordHistory
        {
            get
            {
                return Settings.RecordHistory;
            }
            set
            {
                Settings.RecordHistory = value;
                Settings.Save(Settings);
            }
        }
    }
}
