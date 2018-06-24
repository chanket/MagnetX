using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;

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
			httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.87 Safari/537.36");
			httpClient.DefaultRequestHeaders.Add("Accept-Language", "zh-CN,zh;q=0.9,en;q=0.8");
			return httpClient;
		}

		protected override string GetURL(string word, int page)
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

		protected override Result ReadPart(string part)
		{
			Result result = new Result
			{
				From = Name
			};
			try
			{
				if (!regName.IsMatch(part))
				{
					return null;
				}
				if (!regMagnet.IsMatch(part))
				{
					return null;
				}
				if (!regSize.IsMatch(part))
				{
					return null;
				}
				result.Name = regName.Match(part).Groups[1].Value;
				result.Magnet = regMagnet.Match(part).Groups[1].Value;
				result.Magnet = "magnet:?xt=urn:btih:" + result.Magnet;
				result.Size = regSize.Match(part).Groups[1].Value;
				return result;
			}
			catch
			{
				return null;
			}
		}
	}
}
