using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MagnetX.Searcher.WebSearcher
{
    [SearcherEnabled]
    class BaocaibtWebSearcher : WebSearcher
	{
		public override string Name => "baocaibt.org";

		protected override async Task<string> GetURL(string word, int page)
		{
			string text = Uri.EscapeUriString(word);
			return "http://www.baocaibt.org/search/" + word + "/?c=&s=create_time&p=" + page;
		}

		protected override IEnumerable<string> PrepareParts(string content)
		{
			string[] parts = content.Split(new string[1]
			{
				"<td class=\"x-item\">"
			}, StringSplitOptions.None);
			for (int i = 1; i < parts.Length; i++)
			{
				if (parts[i].Length > 60 && parts[i].Length < 1000)
				{
					yield return parts[i];
				}
			}
		}

        protected Regex regName = new Regex("title=\"(.+?)\"", RegexOptions.Compiled);

        protected Regex regMagnet = new Regex("/hash/(.+?)\"", RegexOptions.Compiled);

        protected Regex regSize = new Regex("文件大小.+?>(.+?)<", RegexOptions.Compiled);

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
