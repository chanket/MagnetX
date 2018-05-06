using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MagnetX.Searcher.WebSearcher
{
    class BtwhatWebSearcher : WebSearcher
    {
        public override string Name
        {
            get
            {
                return "btwhat.info";
            }
        }

        protected override string GetURL(string word, int page)
        {
            string name = Convert.ToBase64String(Encoding.UTF8.GetBytes(word));
            return "http://www.btwhat.info/search/b-" + name + "/" + page + "-3.html";
        }

        protected override IEnumerable<string> PrepareParts(string content)
        {
            string[] parts = content.Split(new string[] { "<div class=\"search-item\">" }, StringSplitOptions.None);
            for (int i = 1; i < parts.Length; i++)
            {
                yield return parts[i];
            }
        }

        protected Regex regName = new Regex("document\\.write\\(decodeURIComponent\\(([^\\)]+)\\)", RegexOptions.Compiled);
        protected Regex regMagnet = new Regex("<a href=\"\\/wiki\\/([^\\.]+)\\.", RegexOptions.Compiled);
        protected Regex regSize = new Regex("File Size[\\D]+\\>\\s*(\\d[^\\<]+)", RegexOptions.Compiled);

        protected override Result ReadPart(string part)
        {
            Result r = new Result() { From = this.Name };
            try
            {
                if (!regName.IsMatch(part)) return null;
                if (!regMagnet.IsMatch(part)) return null;
                if (!regSize.IsMatch(part)) return null;
                r.Name = regName.Match(part).Groups[1].Value;
                r.Name = Uri.UnescapeDataString(r.Name.Replace("\"", "").Replace("+", ""));
                r.Name = r.Name.Replace("<b>", "").Replace("</b>", "");
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
