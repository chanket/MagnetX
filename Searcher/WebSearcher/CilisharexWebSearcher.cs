using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MagnetX.Searcher.WebSearcher
{
    class CilisharexWebSearcher : WebSearcher
    {
        public override string Name
        {
            get
            {
                return "cilisharex.com";
            }
        }

        protected override string GetURL(string word, int page)
        {
            byte[] data = Encoding.UTF8.GetBytes(word);
            string name = "";
            for (int i = 0; i < data.Length; i++)
            {
                name += Convert.ToString(data[i], 16).ToLower();
            }
            return "http://www.cilisharex.com/search/" + name + "-" + page + "-d.html";
        }

        protected override IEnumerable<string> PrepareParts(string content)
        {
            string[] parts = content.Split(new string[] { "<div class=\"search-item\">" }, StringSplitOptions.None);
            for (int i = 1; i < parts.Length; i++)
            {
                yield return parts[i];
            }
        }

        protected Regex regName = new Regex("document\\.write\\(decodeURIComponent\\(\"(.+?)\"", RegexOptions.Compiled);
        protected Regex regMagnet = new Regex("<a href=\"(magnet[^\"]+)\"", RegexOptions.Compiled);
        protected Regex regSize = new Regex("文件大小[\\D]+\\>\\s*(\\d[^\\<]+)", RegexOptions.Compiled);

        protected override Result ReadPart(string part)
        {
            Result r = new Result() { From = this.Name };
            try
            {
                if (!regName.IsMatch(part)) return null;
                if (!regMagnet.IsMatch(part)) return null;
                if (!regSize.IsMatch(part)) return null;
                r.Name = regName.Match(part).Groups[1].Value;
                r.Name = Uri.UnescapeDataString(r.Name);
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
