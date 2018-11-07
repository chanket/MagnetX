using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MagnetX.Searcher.WebSearcher
{
    [SearcherEnabled]
    class BtcatWebSearcher : SimpleWebSearcher
	{
		protected override async Task<string> GetURLAsync(string word, int page)
		{
			string text = Uri.EscapeUriString(word);
			return "https://btcat.org/search/" + text + "/rela-" + page + ".html";
		}

        public BtcatWebSearcher()
            : base("btcat.org", "<div class=\"item-title\">",
                  new Regex("<a title=\"(.+?)\"", RegexOptions.Compiled),
                  new Regex("/btinfo-(.+?)\\.html", RegexOptions.Compiled),
                  new Regex("cpill yellow-pill.+?>(.+?)<", RegexOptions.Compiled))
        {

        }
    }
}
