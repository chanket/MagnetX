using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MagnetX.Searcher.WebSearcher
{
    class DhtseakWebSearcher : WebSearcher
    {
        public override string Name
        {
            get
            {
                return "dhtseak.date";
            }
        }

        protected override async Task<string> GetURL(string word, int page)
        {
            string name = Uri.EscapeUriString(word);
            return "http://www.dhtseak.date/search/" + name + "/" + page + "-3.html";
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
        protected Regex regMagnet = new Regex("urn:btih:([a-zA-Z0-9]{40})", RegexOptions.Compiled);
        protected Regex regSize = new Regex("文件大小.+?>(.+?)<", RegexOptions.Compiled);

        protected override async Task<Result> ReadPart(string part)
        {
            Result r = new Result() { From = this.Name };
            try
            {
                var matchName = regName.Match(part);
                var matchMagnet = regMagnet.Match(part);
                var matchSize = regSize.Match(part);
                if (!matchName.Success || !matchMagnet.Success || !matchSize.Success) return null;

                r.Name = matchName.Groups[1].Value.Replace("<b>", "").Replace("</b>", "");
                r.Magnet = "magnet:?xt=urn:btih:" + matchMagnet.Groups[1].Value;
                r.Size = matchSize.Groups[1].Value;
                return r;
            }
            catch
            {
                return null;
            }
        }
    }
}
