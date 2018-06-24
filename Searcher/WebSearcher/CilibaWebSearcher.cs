using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MagnetX.Searcher.WebSearcher
{
	internal class CilibaWebSearcher : WebSearcher
	{
		protected Regex regName = new Regex("target=\"_blank\">(.+?)<", RegexOptions.Compiled);

		protected Regex regMagnet = new Regex("/btread/(.+?)\\.", RegexOptions.Compiled);

		protected Regex regSize = new Regex("大小:\\s+(.+?)\\s\\s", RegexOptions.Compiled);

		public override string Name => "ciliba.net";

		protected override async Task<string> GetURL(string word, int page)
		{
			string text = Uri.EscapeUriString(word);
			return "http://www.ciliba.net/word/" + text + "_" + page + ".html";
		}

		protected override IEnumerable<string> PrepareParts(string content)
		{
			string[] parts = content.Split(new string[1]
			{
				"<div class=\"T1\">"
			}, StringSplitOptions.None);
			for (int i = 2; i < parts.Length; i++)
			{
				yield return parts[i];
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

                result.Name = matchName.Groups[1].Value.Replace("<em>", "").Replace("</em>", "");
				result.Magnet = "magnet:?xt=urn:btih:" + regMagnet.Match(part).Groups[1].Value;
				result.Size = matchMagnet.Groups[1].Value;
				return result;
			}
			catch
			{
				return null;
			}
		}
	}
}
