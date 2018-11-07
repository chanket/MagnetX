using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MagnetX.Searcher.WebSearcher
{
    [SearcherEnabled]
    class BturlWebSearcher : SimpleWebSearcher
    {
        protected override async Task<string> GetURLAsync(string word, int page)
        {
            string name = Uri.EscapeUriString(word);
            return "https://www.bturl.cc/search/" + name + "_click_" + page + ".html";
        }

        public BturlWebSearcher()
            : base("bturl.cc", "<h3 class=\"T1\">",
                  new Regex("<div class=\"item-list\">(.+)(</div>|\n)", RegexOptions.Compiled),
                  new Regex("href=\"/(.+?)\\.html", RegexOptions.Compiled),
                  new Regex("文件大小.+?>(.+?)</span>", RegexOptions.Compiled))
        {

        }
    }
}
