using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MagnetX.Searcher.WebSearcher
{
	internal class BtcatWebSearcher : WebSearcher
	{
		protected Regex regName = new Regex("<a title=\"(.+?)\"", RegexOptions.Compiled);

		protected Regex regMagnet = new Regex("/btinfo-(.+?)\\.html", RegexOptions.Compiled);

		protected Regex regSize = new Regex("cpill yellow-pill.+?>(.+?)<", RegexOptions.Compiled);

		public override string Name => "btcat.org";

		protected override string GetURL(string word, int page)
		{
			string text = Uri.EscapeUriString(word);
			return "https://btcat.org/search/" + text + "/rela-" + page + ".html";
		}

		protected override IEnumerable<string> PrepareParts(string content)
		{
			string[] parts = content.Split(new string[1]
			{
				"<div class=\"item-title\">"
			}, StringSplitOptions.None);
			for (int i = 1; i < parts.Length; i++)
			{
				yield return parts[i];
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
				result.Magnet = "magnet:?xt=urn:btih:" + regMagnet.Match(part).Groups[1].Value;
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
