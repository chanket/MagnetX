using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MagnetX.Searcher.WebSearcher
{
    class ZhizhubWebSearcher : WebSearcher
    {
        public override string Name
        {
            get
            {
                return "zhizhub.com";
            }
        }

        protected override string GetURL(string word, int page)
        {
            string name = Uri.EscapeUriString(word);
            return "http://www.zhizhub.com/so/" + name + "-first-asc-" + page;
        }

        protected override IEnumerable<string> PrepareParts(string content)
        {
            string[] parts = content.Split(new string[] { "/static/img/torrent.gif" }, StringSplitOptions.None);
            for (int i = 1; i < parts.Length; i++)
            {
                yield return parts[i];
            }
        }

        protected Regex regName = new Regex(".html\">(.+?)</a>", RegexOptions.Compiled);
        protected Regex regMagnet = new Regex("<a href=\"/bt/(.{40})", RegexOptions.Compiled);
        protected Regex regSize = new Regex("\"dsize\">(.+?)<", RegexOptions.Compiled);

        protected override Result ReadPart(string part)
        {
            Result r = new Result() { From = this.Name };
            try
            {
                if (!regName.IsMatch(part)) return null;
                if (!regMagnet.IsMatch(part)) return null;
                if (!regSize.IsMatch(part)) return null;
                r.Name = regName.Match(part).Groups[1].Value;
                r.Name = r.Name.Replace("<span class=\"highlight\">", "");
                r.Name = r.Name.Replace("</span>", "");
                r.Magnet = regMagnet.Match(part).Groups[1].Value;
                r.Magnet = "magnet:?xt=urn:btih:" + r.Magnet;
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
