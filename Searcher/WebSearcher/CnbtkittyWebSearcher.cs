using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MagnetX.Searcher.WebSearcher
{
    [SearcherEnabled]
    class CnbtkittyWebSearcher : WebSearcher
	{
		public override string Name => "cnbtkitty.me";

		protected override async Task<string> GetURL(string word, int page)
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

		protected override IEnumerable<string> PrepareParts(string content)
		{
			string[] parts = content.Split(new string[1]
			{
				"<dl class='list-con'>"
			}, StringSplitOptions.None);
			for (int i = 1; i < parts.Length; i++)
			{
				yield return parts[i];
			}
        }

        protected Regex regName = new Regex("target=\"_blank\">(.+?)<\\/a>", RegexOptions.Compiled);
        protected Regex regMagnet = new Regex("<a href=\".+?/t/(.+?)\\.html", RegexOptions.Compiled);
        protected Regex regSize = new Regex("Size(?:.+?)<b>(.+?)<\\/b>", RegexOptions.Compiled);

        protected override async Task<Result> ReadPart(string part)
        {
            Result result = new Result() { From = this.Name };
            try
            {
                var matchName = regName.Match(part);
                var matchMagnet = regMagnet.Match(part);
                var matchSize = regSize.Match(part);

                if (!matchName.Success || !matchMagnet.Success || !matchSize.Success) return null;

                string encoded = matchMagnet.Groups[1].Value;
                encoded = encoded.Replace('-', '+');
                encoded = encoded.Replace('_', '/');
                while (encoded.Length % 4 != 0) encoded += '=';
                DeflateStream ds = new DeflateStream(new MemoryStream(Convert.FromBase64String(encoded)), CompressionMode.Decompress);
                byte[] decoded = new byte[40];
                int count = ds.Read(decoded, 0, 40);
                if (count != 40) return null;

                result.Name = matchName.Groups[1].Value.Replace("<b>", "").Replace("</b>", "");
				if (result.Name.EndsWith(".torrent")) result.Name = result.Name.Substring(0, result.Name.Length - ".torrent".Length);
                result.Magnet = "magnet:?xt=urn:btih:" + Encoding.ASCII.GetString(decoded);
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
