using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MagnetX.Searcher.WebSearcher
{
    class BtceriseWebSearcher : WebSearcher
    {
        public override string Name
        {
            get
            {
                return "btcerise.me";
            }
        }

        protected override string GetURL(string word, int page)
        {
            string name = Uri.EscapeUriString(word);
            return "http://www.btcerise.me/search?keyword=" + name + "&p=" + page;
        }

        protected override IEnumerable<string> GetParts(string content)
        {
            string[] parts = content.Split(new string[] { "<div class=\"r\">" }, StringSplitOptions.None);
            for (int i = 1; i < parts.Length; i++)
            {
                yield return parts[i];
            }
        }

        protected Regex regName = new Regex("\\<h5 class\\=\"h\"\\>(.+?)\\<\\/h5\\>", RegexOptions.Compiled);
        protected Regex regMagnet = new Regex("href=\"(magnet[^\\&^\"]+)", RegexOptions.Compiled);
        protected Regex regSize = new Regex("大小[\\D]+\\>\\s*(\\d[^\\<]+)", RegexOptions.Compiled);

        protected override Result ReadPart(string part)
        {
            Result r = new Result();
            try
            {
                if (!regName.IsMatch(part)) return null;
                if (!regMagnet.IsMatch(part)) return null;
                if (!regSize.IsMatch(part)) return null;
                r.Name = regName.Match(part).Groups[1].Value;
                r.Name = r.Name.Replace("<span class='highlight'>", "").Replace("</span>", "");
                r.Magnet = regMagnet.Match(part).Groups[1].Value;
                r.Size = regSize.Match(part).Groups[1].Value;
                //r.Hotness = -1;

                if (r.Name.IndexOf("email") >= 0) return null;
                r.From = this.Name;
                return r;
            }
            catch
            {
                return null;
            }
        }
    }
}
