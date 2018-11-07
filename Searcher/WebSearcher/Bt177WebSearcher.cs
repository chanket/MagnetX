using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MagnetX.Searcher.WebSearcher
{
    [SearcherEnabled]
    class Bt177WebSearcher : SimpleWebSearcher
    {
        protected override async Task<string> GetURLAsync(string word, int page)
        {
            string text = Uri.EscapeUriString(word);
            return "http://www.bt177.biz/word/" + text + "_" + page + ".html";
        }

        public Bt177WebSearcher()
            : base("bt177.biz", "<li><div class=\"T1\">", 
                  new Regex("title=\"(.+?)\"", RegexOptions.Compiled),
                  new Regex("href=\"/read/(.+?)\\.html", RegexOptions.Compiled),
                  new Regex("大小.+?>(.+?)</span>", RegexOptions.Compiled))
        {

        }
    }
}
