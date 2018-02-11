using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MagnetX.Searcher.WebSearcher
{
    class Bt177WebSearcher : WebSearcher
    {
        public override string Name
        {
            get
            {
                return "bt177.info";
            }
        }

        protected override string GetURL(string word, int page)
        {
            string name = Uri.EscapeUriString(word);
            return "http://www.bt177.info/word/" + word + "_" + page + ".html";
        }

        protected override IEnumerable<string> GetParts(string content)
        {
            string[] parts = content.Split(new string[] { "<li><a href=" }, StringSplitOptions.None);
            for (int i = 1; i < parts.Length; i++)
            {
                if (parts[i].Length > 60 && parts[i].Length < 1000)
                    yield return parts[i];
            }
        }

        protected Regex regName = new Regex("title=\"(.+?)\"\\>", RegexOptions.Compiled);
        protected Regex regMagnet = new Regex("\".+?\\/read\\/(.+?)\\.html\"", RegexOptions.Compiled);
        protected Regex regSize = new Regex("<span>文件大小：(.+?)<\\/span>", RegexOptions.Compiled);

        protected override Result ReadPart(string part)
        {
            Result r = new Result();
            try
            {
                if (!regName.IsMatch(part)) return null;
                if (!regMagnet.IsMatch(part)) return null;
                if (!regSize.IsMatch(part)) return null;
                r.Name = regName.Match(part).Groups[1].Value;
                r.Magnet = regMagnet.Match(part).Groups[1].Value;
                r.Magnet = "magnet:?xt=urn:btih:" + r.Magnet;
                r.Size = regSize.Match(part).Groups[1].Value;
                r.Size = r.Size.Replace(' ', ' ');

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
