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

        protected override async Task<string> GetURL(string word, int page)
        {
            string name = Uri.EscapeUriString(word);
            return "http://www.bt177.info/word/" + word + "_" + page + ".html";
        }

        protected override IEnumerable<string> PrepareParts(string content)
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

        protected override async Task<Result> ReadPart(string part)
        {
            Result result = new Result() { From = this.Name };
            try
            {
                var matchName = regName.Match(part);
                var matchMagnet = regMagnet.Match(part);
                var matchSize = regSize.Match(part);
                if (!matchName.Success || !matchMagnet.Success || !matchSize.Success) return null;

                result.Name = matchName.Groups[1].Value;
                result.Magnet = "magnet:?xt=urn:btih:" + matchMagnet.Groups[1].Value;
                result.Size = matchSize.Groups[1].Value.Replace(' ', ' ');
                return result;
            }
            catch
            {
                return null;
            }
        }
    }
}
