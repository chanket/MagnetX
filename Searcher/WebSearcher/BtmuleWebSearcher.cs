using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MagnetX.Searcher.WebSearcher
{
    class BtmuleWebSearcher : WebSearcher
    {
        public override string Name
        {
            get
            {
                return "btmule.org";
            }
        }

        protected override async Task<string> GetURL(string word, int page)
        {
            string name = Uri.EscapeUriString(word);
            return "http://www.btmule.org/q/" + name + ".html?sort=hits&page=" + page;
        }

        protected override IEnumerable<string> PrepareParts(string content)
        {
            string[] parts = content.Split(new string[] { "<div class=\"item-title\">" }, StringSplitOptions.None);
            for (int i = 1; i < parts.Length; i++)
            {
                yield return parts[i];
            }
        }

        protected Regex regName = new Regex("target\\=\"_blank\"\\>(.+?)\\<\\/a\\>", RegexOptions.Compiled);
        protected Regex regMagnet = new Regex("\\/torrent\\/([a-zA-Z0-9]{40})\\.html", RegexOptions.Compiled);
        protected Regex regSize = new Regex("文件大小[\\D]+\\>\\s*(\\d[^\\<]+)", RegexOptions.Compiled);

        protected override async Task<Result> ReadPart(string part)
        {
            Result r = new Result() { From = this.Name };
            try
            {
                var matchName = regName.Match(part);
                var matchMagnet = regMagnet.Match(part);
                var matchSize = regSize.Match(part);
                if (!matchName.Success || !matchMagnet.Success || !matchSize.Success) return null;

                r.Name = regName.Match(part).Groups[1].Value.Replace("<em>", "").Replace("</em>", "");
                r.Magnet = "magnet:?xt=urn:btih:" + regMagnet.Match(part).Groups[1].Value;
                r.Size = regSize.Match(part).Groups[1].Value;
                return r;
            }
            catch
            {
                return null;
            }
        }
    }
}
