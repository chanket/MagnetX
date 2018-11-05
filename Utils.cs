using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MagnetX.Searcher;
using MagnetX.Searcher.HistorySearcher;
using System.Reflection;

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

        /// <summary>
        /// 获取所有可用的<see cref="Searcher.Searcher"/>的实现类的实例。
        /// 任何一个类需要满足下列所有条件：具有<see cref="SearcherEnabledAttribute"/>属性；具有无参数构造函数。
        /// </summary>
        public static IEnumerable<Searcher.Searcher> GetAllSearchers()
        {
            foreach (Type t in Assembly.GetExecutingAssembly().GetTypes())
            {
                SearcherEnabledAttribute attr = t.GetCustomAttribute(typeof(SearcherEnabledAttribute)) as SearcherEnabledAttribute;
                if (attr != null && t.IsSubclassOf(typeof(Searcher.Searcher)))
                {
                    ConstructorInfo constructor = t.GetConstructor(new Type[0]);
                    if (constructor != null)
                    {
                        yield return constructor.Invoke(null) as Searcher.Searcher;
                    }
                }
            }
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
