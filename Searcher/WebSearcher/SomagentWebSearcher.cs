using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MagnetX.Searcher.WebSearcher
{
	internal class SomagnetWebSearcher : WebSearcher
    {
        public override string Name { get; } = "baocaibt.org";

        protected override string GetURL(string word, int page)
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

        protected Regex regName = new Regex("title=\"(.+?)\"", RegexOptions.Compiled);

        protected Regex regMagnet = new Regex("/hash/(.+?)\"", RegexOptions.Compiled);

        protected Regex regSize = new Regex("文件大小.+?>(.+?)<", RegexOptions.Compiled);

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
