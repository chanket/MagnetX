using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MagnetX.Searcher.WebSearcher
{
    [SearcherEnabled]
    class CilibaWebSearcher : SimpleWebSearcher
    {
        protected override async Task<string> GetURLAsync(string word, int page)
        {
            string name = Uri.EscapeUriString(word);
            return "https://www.ciliba.biz/s/" + name + "_rel_" + page + ".html";
        }

        public CilibaWebSearcher()
            : base("ciliba.biz", "<div class=\"search-item\">",
                  new Regex("target=\"_blank\">(.+?)</a>", RegexOptions.Compiled),
                  new Regex("<a href=\".+?/detail/(.+?)\\.html", RegexOptions.Compiled),
                  new Regex("文件大小.+?>(.+?)<", RegexOptions.Compiled))
        {

        }

        protected override async Task<Result> GetResultAsync(string part)
        {
            Result result = await base.GetResultAsync(part).ConfigureAwait(false);
            if (result == null) return null;

            result.Name = result.Name.Replace("<em>", "").Replace("</em>", "");
            return result;
        }
    }
}
