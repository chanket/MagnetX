using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MagnetX.Searcher.WebSearcher
{
    class ZhongzisoWebSearcher : WebSearcher
    {
        public override string Name
        {
            get
            {
                return "zhongziso.com";
            }
        }

        protected override string GetURL(string word, int page)
        {
            string name = Uri.EscapeUriString(word);

            return "http://m.zhongziso.com/list_ctime/" + name + "/" + page;
        }

        protected override IEnumerable<string> GetParts(string content)
        {
            string[] parts = content.Split(new string[] { "<li class=\"list-group-item title" }, StringSplitOptions.None);
            for (int i = 1; i < parts.Length; i++)
            {
                yield return parts[i];
            }
        }

        protected Regex regName = new Regex("class=\"text-success\">(.+?)<\\/a>", RegexOptions.Compiled);
        protected Regex regMagnet = new Regex("\"(magnet.+?)[\"&]", RegexOptions.Compiled);
        protected Regex regSize = new Regex("class=\"text-size\">(.+?)<\\/dd>", RegexOptions.Compiled);

        protected override Result ReadPart(string part)
        {
            Result r = new Result();
            try
            {
                if (!regName.IsMatch(part)) return null;
                if (!regMagnet.IsMatch(part)) return null;
                if (!regSize.IsMatch(part)) return null;
                r.Name = regName.Match(part).Groups[1].Value;
                r.Name = r.Name.Replace("<span class=\"highlight\">", "");
                r.Name = r.Name.Replace("</span>", "");
                r.Magnet = regMagnet.Match(part).Groups[1].Value;
                r.Size = regSize.Match(part).Groups[1].Value;

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
