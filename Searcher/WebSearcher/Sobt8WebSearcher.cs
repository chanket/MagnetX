using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MagnetX.Searcher.WebSearcher
{
    class Sobt8WebSearcher : WebSearcher
    {
        public override string Name
        {
            get
            {
                return "sobt8.com";
            }
        }

        protected override IEnumerable<string> GetParts(string content)
        {
            string[] parts = content.Split(new string[] { "<div class=\"item-title\">" }, StringSplitOptions.None);
            for (int i = 1; i < parts.Length; i++)
            {
                yield return parts[i];
            }
        }

        protected override string GetURL(string word, int page)
        {
            return "http://www.sobt8.com/q/" + word + "_rel_" + page + ".html";
        }

        protected Regex regName = new Regex("target\\=\"_blank\"\\>(.+?)\\<\\/a\\>", RegexOptions.Compiled);
        protected Regex regMagnet = new Regex("\\/torrent\\/([a-zA-Z0-9]{40})\\.html", RegexOptions.Compiled);
        protected Regex regSize = new Regex("文件大小[\\D]+\\>\\s*(\\d[^\\<]+)", RegexOptions.Compiled);

        protected override Result ReadPart(string part)
        {
            Result r = new Result();
            try
            {
                if (!regName.IsMatch(part)) return null;
                if (!regMagnet.IsMatch(part)) return null;
                if (!regSize.IsMatch(part)) return null;
                r.Name = regName.Match(part).Groups[1].Value;
                r.Name = r.Name.Replace("<em>", "").Replace("</em>", "");
                r.Magnet = "magnet:?xt=urn:btih:" + regMagnet.Match(part).Groups[1].Value;
                r.Size = regSize.Match(part).Groups[1].Value;
                //r.Hotness = -1;

                if (r.Name.IndexOf("email") >= 0) return null;
                return r;
            }
            catch
            {
                return null;
            }
        }
    }
}
