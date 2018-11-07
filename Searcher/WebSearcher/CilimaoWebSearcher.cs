using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MagnetX.Searcher.WebSearcher
{
    [SearcherEnabled]
    class CilimaoWebSearcher : SimpleWebSearcher
    {
        protected override async Task<string> GetURLAsync(string word, int page)
        {
            string name = Uri.EscapeUriString(word);
            return "https://www.cilimao.me/search?word=" + name + "&sortProperties=download_count&page=" + page;
        }

        public CilimaoWebSearcher()
            : base("cilimao.me", "<div class=\"Search__result__",
                  new Regex("<a.+?>(.+?)</a>", RegexOptions.Compiled),
                  new Regex("href=\"/information/(.+?)\\?", RegexOptions.Compiled),
                  new Regex("文件大小.+?-->(.+?)<!--", RegexOptions.Compiled))
        {

        }

        protected override async Task<Result> GetResultAsync(string part)
        {
            Result result = await base.GetResultAsync(part).ConfigureAwait(false);
            if (result == null) return null;

            result.Name = result.Name.Replace("<em>", "").Replace("</em>", "");
            return result;
        }
    }
}
