using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MagnetX.Searcher.WebSearcher
{
    [SearcherEnabled]
    class BtsoWebSearcher : SimpleWebSearcher
	{
		protected override async Task<string> GetURLAsync(string word, int page)
		{
			string text = Uri.EscapeUriString(word);
			return "https://btso.pw/search/" + text + "/page/" + page;
		}

        public BtsoWebSearcher()
            : base("btso.pw", "<div class=\"row\">",
                  new Regex("title=\"(.+?)\"", RegexOptions.Compiled),
                  new Regex("/hash/(.+?)\"", RegexOptions.Compiled),
                  new Regex("size\">(.+?)<", RegexOptions.Compiled))
        {

        }

        protected override HttpClient CreateHttpClient()
        {
            HttpClient httpClient = base.CreateHttpClient();
            httpClient.DefaultRequestHeaders.Add("Accept-Language", "zh-CN,zh;q=0.9,en;q=0.8");
            return httpClient;
        }
    }
}
