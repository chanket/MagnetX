using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MagnetX.Searcher.WebSearcher
{
    [SearcherEnabled]
    class SomagnetWebSearcher : SimpleWebSearcher
    {
        protected override async Task<string> GetURLAsync(string word, int page)
		{
			string name = Uri.EscapeUriString(word);
            return "https://www.somagnet.com/so/" + name + "/page/" + page;
        }

        public SomagnetWebSearcher()
            : base("somagnet.com", "<dl class='item'>",
          new Regex("target='_blank'>(.+?)</dt>", RegexOptions.Compiled),
          new Regex("<dt><a href='(.+?)'", RegexOptions.Compiled),
          new Regex("文件大小.+?>(.+?)<", RegexOptions.Compiled))
        {

        }

        protected Regex RegexMagnetReal = new Regex("种子哈希.+?>(.+?)<", RegexOptions.Compiled);

        protected override async Task<Result> GetResultAsync(string part)
        {
            Result result = await base.GetResultAsync(part).ConfigureAwait(false);
            if (result == null) return null;

            result.Name = result.Name.Replace("<span class=\"highlight\">", "").Replace("</span>", "");
            try
            {
                using (HttpClient hc = CreateHttpClient())
                {
                    string htmlMagnet = await hc.GetStringAsync("https://www.somagnet.com/" + result.Magnet);
                    var matchMagnet = RegexMagnetReal.Match(htmlMagnet);

                    if (matchMagnet.Success)
                    {
                        result.Magnet = "magnet:?xt=urn:btih:" + matchMagnet.Groups[1].Value;
                        return result;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch
            {
                return null;
            }
        }
	}
}
