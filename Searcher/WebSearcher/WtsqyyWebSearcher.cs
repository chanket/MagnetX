using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MagnetX.Searcher.WebSearcher
{
    [SearcherEnabled]
    class WtsqyyWebSearcher : SimpleWebSearcher
	{
		protected override async Task<string> GetURLAsync(string word, int page)
		{
			string name = Uri.EscapeUriString(word);
            return "http://www.wtsqyy.com/search/" + name + "/?p=" + page;
        }

        public WtsqyyWebSearcher()
            : base("wtsqyy.com", "<h3 class=\"T1\">",
                  new Regex("title=\"(.+?)\\.torrent\"", RegexOptions.Compiled),
                  new Regex("class=\"item-list\">([a-zA-Z0-9]{40})<", RegexOptions.Compiled),
                  new Regex("文件大小.+?>(.+?)<", RegexOptions.Compiled))
        {

        }
	}
}
