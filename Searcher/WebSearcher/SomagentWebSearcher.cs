using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MagnetX.Searcher.WebSearcher
{
    [SearcherEnabled]
    class SomagnetWebSearcher : WebSearcher
    {
        public override string Name { get; } = "somagnet.com";

        protected override async Task<string> GetURL(string word, int page)
		{
			string name = Uri.EscapeUriString(word);
            return "https://www.somagnet.com/so/" + name + "/page/" + page;
        }

		protected override IEnumerable<string> PrepareParts(string content)
		{
			string[] parts = content.Split(new string[1]
			{
				"<dl class='item'>"
			}, StringSplitOptions.None);
			for (int i = 1; i < parts.Length; i++)
			{
                yield return parts[i];
            }
		}

        protected Regex regName = new Regex("target='_blank'>(.+?)</dt>", RegexOptions.Compiled);

        protected Regex regMagnet = new Regex("种子哈希.+?>(.+?)<", RegexOptions.Compiled);

        protected Regex regSize = new Regex("文件大小.+?>(.+?)<", RegexOptions.Compiled);

        protected Regex regTemp = new Regex("<dt><a href='(.+?)'", RegexOptions.Compiled);

        protected override async Task<Result> ReadPart(string part)
		{
			Result result = new Result
			{
				From = Name
			};
			try
            {
                var matchName = regName.Match(part);
                var matchSize = regSize.Match(part);
                var matchTemp = regTemp.Match(part);
                if (!matchName.Success || !matchSize.Success || !matchTemp.Success) return null;

                string temp = matchTemp.Groups[1].Value;
                using (HttpClient hc = CreateHttpClient())
                {
                    string tempData = await hc.GetStringAsync("https://www.somagnet.com/" + temp);
                    var matchMagnet = regMagnet.Match(tempData);
                    if (!matchMagnet.Success) return null;

                    result.Magnet = "magnet:?xt=urn:btih:" + matchMagnet.Groups[1].Value;
                }
                
                result.Name = matchName.Groups[1].Value.Replace("<span class=\"highlight\">", "").Replace("</span>", "");
                result.Size = matchSize.Groups[1].Value;
				return result;
			}
			catch
			{
				return null;
			}
		}
	}
}
