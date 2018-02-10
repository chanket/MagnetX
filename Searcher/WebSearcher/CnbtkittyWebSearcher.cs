using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MagnetX.Searcher.WebSearcher
{
    class CnbtkittyWebSearcher : WebSearcher
    {
        public override string Name
        {
            get
            {
                return "cnbtkitty.com";
            }
        }

        protected Dictionary<string, string> Words = new Dictionary<string, string>();
        protected Regex RegWord = new Regex(@"/search/(.+?)/", RegexOptions.Compiled);

        protected override string GetURL(string word, int page)
        {
            //这个源的URL规则暂不知晓
            //只有模拟网页操作，POST一个包含关键字FORM
            //服务器将URL作为302返回
            //使用Words变量缓存结果

            if (Words.ContainsKey(word))
            {
                return "http://cnbtkitty.com/search/" + Words[word] + "/" + page + "/4/0.html";
            }
            else
            {
                var form = new List<KeyValuePair<string, string>>();
                form.Add(new KeyValuePair<string, string>("keyword", word));
                //form.Add(new KeyValuePair<string, string>("hidden", word));

                try
                {
                    var client = new HttpClient();
                    var content = new FormUrlEncodedContent(form);
                    var task = client.PostAsync("http://cnbtkitty.com", content);
                    task.Wait();

                    if (!task.IsCompleted)
                    {
                        throw new HttpRequestException();
                    }

                    string url = task.Result.RequestMessage.RequestUri.ToString();
                    var match = RegWord.Match(url);
                    if (!match.Success)
                    {
                        throw new InvalidOperationException();
                    }

                    string value = match.Groups[1].Value;
                    Words.Add(word, value);
                    return GetURL(word, page);
                }
                catch
                {
                    return null;
                }
            }
        }

        protected override IEnumerable<string> GetParts(string content)
        {
            string[] parts = content.Split(new string[] { "<dl class='list-con'>" }, StringSplitOptions.None);
            for (int i = 1; i < parts.Length; i++)
            {
                yield return parts[i];
            }
        }

        protected Regex regName = new Regex("target=\"_blank\">(.+?)<\\/a>", RegexOptions.Compiled);
        protected Regex regMagnet = new Regex("'(magnet.+)'", RegexOptions.Compiled);
        protected Regex regSize = new Regex("Size(?:.+?)<b>(.+?)<\\/b>", RegexOptions.Compiled);

        protected override Result ReadPart(string part)
        {
            Result r = new Result();
            try
            {
                if (!regName.IsMatch(part)) return null;
                if (!regMagnet.IsMatch(part)) return null;
                if (!regSize.IsMatch(part)) return null;
                r.Name = regName.Match(part).Groups[1].Value;
                r.Name = r.Name.Replace("<b>", "");
                r.Name = r.Name.Replace("</b>", "");
                if (r.Name.EndsWith(".torrent")) r.Name = r.Name.Substring(0, r.Name.Length - ".torrent".Length);
                r.Magnet = regMagnet.Match(part).Groups[1].Value;
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
