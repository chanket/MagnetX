using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MagnetX.Searcher.WebSearcher
{
	internal class BtsoWebSearcher : WebSearcher
	{
		protected Regex regName = new Regex("title=\"(.+?)\"", RegexOptions.Compiled);

		protected Regex regMagnet = new Regex("/hash/(.+?)\"", RegexOptions.Compiled);

		protected Regex regSize = new Regex("size\">(.+?)<", RegexOptions.Compiled);

		public override string Name => "btso.pw";

		protected override HttpClient CreateHttpClient()
		{
			HttpClient httpClient = base.CreateHttpClient();
			httpClient.DefaultRequestHeaders.Add("Accept-Language", "zh-CN,zh;q=0.9,en;q=0.8");
			return httpClient;
		}

		protected override async Task<string> GetURL(string word, int page)
		{
			string text = Uri.EscapeUriString(word);
			return "https://btso.pw/search/" + text + "/page/" + page;
		}

		protected override IEnumerable<string> PrepareParts(string content)
		{
			string[] parts = content.Split(new string[1]
			{
				"<div class=\"row\">"
			}, StringSplitOptions.None);
			for (int i = 1; i < parts.Length - 1; i++)
			{
				if (parts[i].Length > 60 && parts[i].Length < 1000)
				{
					yield return parts[i];
				}
			}
		}

		protected override async Task<Result> ReadPart(string part)
		{
			Result result = new Result
			{
				From = Name
			};
			try
            {
                var matchName = regName.Match(part);
                var matchMagnet = regMagnet.Match(part);
                var matchSize = regSize.Match(part);
                if (!matchName.Success || !matchMagnet.Success || !matchSize.Success) return null;

                result.Name = matchName.Groups[1].Value;
				result.Magnet = "magnet:?xt=urn:btih:" + matchMagnet.Groups[1].Value;
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
