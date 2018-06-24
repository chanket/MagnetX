using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MagnetX.Searcher.WebSearcher
{
    class CililianxWebSearcher : WebSearcher
    {
        public override string Name
        {
            get
            {
                return "cililianx.com";
            }
        }

        protected override async Task<string> GetURL(string word, int page)
        {
            string name = Uri.EscapeUriString(word);
            return "http://cililianx.com/list/" + name + "/" + page + ".html";
        }

        protected override IEnumerable<string> PrepareParts(string content)
        {
            string[] parts = content.Split(new string[] { "<div class=\"T1\">" }, StringSplitOptions.None);
            for (int i = 1; i < parts.Length; i++)
            {
                yield return parts[i];
            }
        }

        protected Regex regName = new Regex("<a name='file_title'.+?>(.+?)</a>", RegexOptions.Compiled);
		protected Regex regMagnet = new Regex("/xiangxi/(.+?)\"", RegexOptions.Compiled);
        protected Regex regSize = new Regex("种子大小[\\D]+\\>\\s*(\\d[^\\<]+)", RegexOptions.Compiled);

        protected override async Task<Result> ReadPart(string part)
        {
            Result result = new Result() { From = this.Name };
            try
            {
                var matchName = regName.Match(part);
                var matchMagnet = regMagnet.Match(part);
                var matchSize = regSize.Match(part);
                if (!matchName.Success || !matchMagnet.Success || !matchSize.Success) return null;

                result.Name = matchName.Groups[1].Value.Replace("<span class=\"mhl\">", "").Replace("</span>", "");
                result.Magnet = "magnet:?xt=urn:btih:" + matchMagnet.Groups[1].Value;
                result.Size = matchSize.Groups[1].Value;
                return result;
            }
            catch
            {
                return null;
            }
        }
    }
}
