using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MagnetX.Searcher.WebSearcher
{
    [SearcherEnabled]
    class ZhizhucWebSearcher : SimpleWebSearcher
    {
        protected override async Task<string> GetURLAsync(string word, int page)
        {
            string text = Uri.EscapeUriString(word);
            return "http://www.zhizhuc.com/so/" + text + "-first-asc-" + page;
        }

        public ZhizhucWebSearcher()
            : base("zhizhuc.com", "<img src=\"/static/img/torrent.gif\"",
                  new Regex("<a.+?>(.+?)</a>", RegexOptions.Compiled),
                  new Regex("href=\"/bt/(.+?)\\.html", RegexOptions.Compiled),
                  new Regex("dsize.+?>(.+?)<", RegexOptions.Compiled))
        {

        }

        protected override async Task<Result> GetResultAsync(string part)
        {
            Result result = await base.GetResultAsync(part).ConfigureAwait(false);
            if (result == null) return null;

            result.Name = result.Name.Replace("<span class=\"highlight\">", "").Replace("</span>", "");
            return result;
        }
    }
}
