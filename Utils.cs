using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MagnetX.Searcher.WebSearcher;
using MagnetX.Searcher.HistorySearcher;

namespace MagnetX
{
    /// <summary>
    /// 全局工具类。
    /// 提供<see cref="Searcher.Searcher"/>实例的获取；
    /// 提供<see cref="Searcher.HistorySearcher.HistoryRecorder"/>单例的获取；
    /// 封装了对<see cref="MagnetX.Settings"/>类数据的访问。
    /// </summary>
    static class Utils
    {
        public static HistoryRecorder HistoryRecorder { get; } = new HistoryRecorder();

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

        #region Settings

        private static Settings Settings { get; } = Settings.Load();

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

        #endregion
    }
}
