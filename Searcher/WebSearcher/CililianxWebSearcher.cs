using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MagnetX.Searcher.WebSearcher
{
    [SearcherEnabled]
    class CililianxWebSearcher : SimpleWebSearcher
    {
        protected override async Task<string> GetURLAsync(string word, int page)
        {
            string name = Uri.EscapeUriString(word);
            return "http://cililianx.com/list/" + name + "/" + page + ".html";
        }

        public CililianxWebSearcher()
            : base("cililianx.com", "<div class=\"T1\">",
                  new Regex("<a name='file_title'.+?>(.+?)</a>", RegexOptions.Compiled),
                  new Regex("/xiangxi/(.+?)\"", RegexOptions.Compiled),
                  new Regex("种子大小[\\D]+\\>\\s*(\\d[^\\<]+)", RegexOptions.Compiled))
        {

        }

        protected override async Task<Result> GetResultAsync(string part)
        {
            Result result = await base.GetResultAsync(part).ConfigureAwait(false);
            if (result == null) return null;

            result.Name = result.Name.Replace("<span class=\"mhl\">", "").Replace("</span>", "");
            return result;
        }
    }
}
