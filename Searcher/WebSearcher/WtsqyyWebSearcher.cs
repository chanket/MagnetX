using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MagnetX.Searcher.WebSearcher
{
    [SearcherEnabled]
    class WtsqyyWebSearcher : WebSearcher
	{
        public override string Name { get; } = "wtsqyy.com";

		protected override async Task<string> GetURL(string word, int page)
		{
			string name = Uri.EscapeUriString(word);
            return "http://www.wtsqyy.com/search/" + name + "/?p=" + page;
        }

		protected override IEnumerable<string> PrepareParts(string content)
		{
			string[] parts = content.Split(new string[1]
			{
                "<h3 class=\"T1\">"
            }, StringSplitOptions.None);
			for (int i = 1; i < parts.Length; i++)
            {
                yield return parts[i];
            }
		}

        protected Regex regName = new Regex("title=\"(.+?)\\.torrent\"", RegexOptions.Compiled);

        protected Regex regMagnet = new Regex("class=\"item-list\">([a-zA-Z0-9]{40})<", RegexOptions.Compiled);

        protected Regex regSize = new Regex("文件大小.+?>(.+?)<", RegexOptions.Compiled);

        protected override async Task<Result> ReadPart(string part)
        {
            Result result = new Result { From = Name };
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
