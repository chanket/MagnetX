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

        public override string Text
        {
            get
            {
                return "bt177.org";
            }
        }

        protected override IEnumerable<string> GetParts(string content)
        {
            string[] parts = content.Split(new string[] { "<li><div class=\"T1\">" }, StringSplitOptions.None);
            for (int i = 1; i < parts.Length; i++)
            {
                yield return parts[i];
            }
        }

        protected override string GetURL(string word, int page)
        {
            string name = Uri.EscapeUriString(word);
            return "http://www.bt177.org/word/" + word + "_" + page + ".html";
        }

        protected Regex regName = new Regex("title\\=\"(.+?)\"\\>", RegexOptions.Compiled);
        protected Regex regMagnet = new Regex("\\<div\\s+class\\=\"dInfo\"\\>.*([a-zA-Z0-9]{40}).*\\<\\/div\\>", RegexOptions.Compiled);
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
                r.Magnet = regMagnet.Match(part).Groups[1].Value;
                r.Magnet = "magnet:?xt=urn:btih:" + r.Magnet;
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
