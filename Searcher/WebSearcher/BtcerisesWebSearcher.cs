using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MagnetX.Searcher.WebSearcher
{
    class BtcerisesWebSearcher : WebSearcher
    {
        public override string Name
        {
            get
            {
                return "btcerises.com";
            }
        }

        protected override string GetURL(string word, int page)
        {
            string name = Uri.EscapeUriString(word);
            return "http://www.btcerises.com/search?keyword=" + name + "&p=" + page;
        }

        protected override IEnumerable<string> PrepareParts(string content)
        {
            string[] parts = content.Split(new string[] { "<div class=\"r\">" }, StringSplitOptions.None);
            for (int i = 1; i < parts.Length; i++)
            {
                yield return parts[i];
            }
        }

        protected Regex regName = new Regex("class=\"h\">(.+?)<\\/h5>", RegexOptions.Compiled);
        protected Regex regMagnet = new Regex("href=\"(magnet.+?)[&\"]", RegexOptions.Compiled);
        protected Regex regSize = new Regex("大小：<span.+?>(.+?)</span>", RegexOptions.Compiled | RegexOptions.Singleline);

        protected override Result ReadPart(string part)
        {
            Result r = new Result() { From = this.Name };
            try
            {
                if (!regName.IsMatch(part)) return null;
                if (!regMagnet.IsMatch(part)) return null;
                if (!regSize.IsMatch(part)) return null;
                r.Name = regName.Match(part).Groups[1].Value;
                r.Name = r.Name.Replace("<span class='highlight'>", "");
                r.Name = r.Name.Replace("</span>", "");
                r.Magnet = regMagnet.Match(part).Groups[1].Value;
                r.Size = regSize.Match(part).Groups[1].Value;
                r.Size = r.Size.Replace(' ', ' ');
                return r;
            }
            catch
            {
                return null;
            }
        }
    }
}
