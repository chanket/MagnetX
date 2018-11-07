using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MagnetX.Searcher.WebSearcher
{
    [SearcherEnabled]
    class CnbtkittyWebSearcher : SimpleWebSearcher
	{
		protected override async Task<string> GetURLAsync(string word, int page)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (DeflateStream deflateStream = new DeflateStream(memoryStream, CompressionMode.Compress, true))
				{
					byte[] bytes = Encoding.UTF8.GetBytes(word);
					deflateStream.Write(bytes, 0, bytes.Length);
				}
				string text = Convert.ToBase64String(memoryStream.ToArray());
				text = text.Replace('/', '_');
				text = text.Replace('+', '-');
				text = text.TrimEnd('=');
				return "https://cnbtkitty.me/search/" + text + "/" + page + "/4/0.html";
			}
		}

        public CnbtkittyWebSearcher()
            : base("cnbtkitty.me", "<dl class='list-con'>",
          new Regex("target=\"_blank\">(.+?)<\\/a>", RegexOptions.Compiled),
          new Regex("<a href=\".+?/t/(.+?)\\.html", RegexOptions.Compiled),
          new Regex("Size(?:.+?)<b>(.+?)<\\/b>", RegexOptions.Compiled))
        {

        }

        protected override async Task<Result> GetResultAsync(string part)
        {
            Result result = await base.GetResultAsync(part).ConfigureAwait(false);
            if (result == null) return null;

            string encoded = result.Magnet;
            encoded = encoded.Replace('-', '+');
            encoded = encoded.Replace('_', '/');
            while (encoded.Length % 4 != 0) encoded += '=';
            DeflateStream ds = new DeflateStream(new MemoryStream(Convert.FromBase64String(encoded)), CompressionMode.Decompress);
            byte[] decoded = new byte[40];
            int count = ds.Read(decoded, 0, 40);

            if (count == 40)
            {
                result.Name = result.Name.Replace("<b>", "").Replace("</b>", "");
                if (result.Name.EndsWith(".torrent")) result.Name = result.Name.Substring(0, result.Name.Length - ".torrent".Length);
                result.Magnet = "magnet:?xt=urn:btih:" + Encoding.ASCII.GetString(decoded);
                return result;
            }
            else
            {
                return null;
            }
        }
    }
}
