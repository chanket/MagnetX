using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MagnetX.Searcher.WebSearcher
{
    [SearcherEnabled]
    class CilibaWebSearcher : WebSearcher
    {
        public override string Name => "ciliba.biz";

        protected override async Task<string> GetURL(string word, int page)
        {
            string name = Uri.EscapeUriString(word);
            return "https://www.ciliba.biz/s/" + name + "_rel_" + page + ".html";
        }

        protected override IEnumerable<string> PrepareParts(string content)
        {
            string[] parts = content.Split(new string[] { "<div class=\"search-item\">" }, StringSplitOptions.None);
            for (int i = 1; i < parts.Length; i++)
            {
                yield return parts[i];
            }
        }

        protected Regex regName = new Regex("target=\"_blank\">(.+?)</a>", RegexOptions.Compiled);
        protected Regex regMagnet = new Regex("<a href=\".+?/detail/(.+?)\\.html", RegexOptions.Compiled);
        protected Regex regSize = new Regex("文件大小.+?>(.+?)<", RegexOptions.Compiled);

        protected override async Task<Result> ReadPart(string part)
        {
            Result result = new Result() { From = Name, };
            try
            {
                var matchName = regName.Match(part);
                var matchMagnet = regMagnet.Match(part);
                var matchSize = regSize.Match(part);
                if (!matchName.Success || !matchMagnet.Success || !matchSize.Success) return null;

                result.Name = matchName.Groups[1].Value.Replace("<em>", "").Replace("</em>", "");
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
