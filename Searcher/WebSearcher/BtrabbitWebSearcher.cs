using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MagnetX.Searcher.WebSearcher
{
    class BtrabbitWebSearcher : WebSearcher
    {
        public override string Name
        {
            get
            {
                return "btrabbit.net";
            }
        }

        protected override string GetURL(string word, int page)
        {
            string name = Uri.EscapeUriString(word);
            return "http://www.btrabbit.net/search/" + name + "/default-" + page + ".html";
        }

        protected override IEnumerable<string> GetParts(string content)
        {
            string[] parts = content.Split(new string[] { "<div class=\"search-item" }, StringSplitOptions.None);
            for (int i = 1; i < parts.Length; i++)
            {
                yield return parts[i];
            }
        }

        protected Regex regName = new Regex("<a title=\"(.+?)\"", RegexOptions.Compiled);
        protected Regex regMagnet = new Regex("href=\"\\/wiki\\/([^\\.]+)\\.", RegexOptions.Compiled);
        protected Regex regSize = new Regex("文件大小(?:.+?)>(.+?)<\\/b>", RegexOptions.Compiled);

        protected override Result ReadPart(string part)
        {
            Result r = new Result();
            try
            {
                if (!regName.IsMatch(part)) return null;
                if (!regMagnet.IsMatch(part)) return null;
                if (!regSize.IsMatch(part)) return null;
                //if (!regHotness.IsMatch(part)) return null;
                r.Name = regName.Match(part).Groups[1].Value;;
                r.Magnet = regMagnet.Match(part).Groups[1].Value;
                r.Magnet = "magnet:?xt=urn:btih:" + r.Magnet;
                r.Size = regSize.Match(part).Groups[1].Value;
                r.Size = r.Size.Replace(" ", " ");
                //r.Hotness = int.Parse(regHotness.Match(part).Groups[1].Value);

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
