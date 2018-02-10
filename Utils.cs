using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MagnetX.Searcher.WebSearcher;

namespace MagnetX
{
    static class Utils
    {
        private static Dictionary<Type, bool> SearcherEnabled { get; } = new Dictionary<Type, bool>();
        public static bool GetSearcherEnabled(Searcher.Searcher s)
        {
            if (s == null) return false;

            if (!SearcherEnabled.ContainsKey(s.GetType()))
            {
                SearcherEnabled.Add(s.GetType(), true);
                return true;
            }
            else
            {
                return SearcherEnabled[s.GetType()];
            }
        }
        public static void SetSearcherEnabled(Searcher.Searcher s, bool b)
        {
            if (s == null) return;

            if (!SearcherEnabled.ContainsKey(s.GetType()))
            {
                SearcherEnabled.Add(s.GetType(), b);
            }
            else
            {
                SearcherEnabled[s.GetType()] = b;
            }
        }
        public static IEnumerable<Searcher.Searcher> GetAllSearchers()
        {
            yield return new Bt177WebSearcher();
            yield return new CilisharexWebSearcher();
            yield return new BtwhatWebSearcher();
            yield return new BtmuleWebSearcher();
            yield return new BtceriseWebSearcher();
            yield return new CnbtkittyWebSearcher();
        }
    }
}
