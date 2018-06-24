using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Text.RegularExpressions;

namespace MagnetX.Searcher.WebSearcher
{
	internal class CnbtkittyWebSearcher : WebSearcher
	{
		protected Regex regName = new Regex("target=\"_blank\">(.+?)<\\/a>", RegexOptions.Compiled);

		protected Regex regMagnet = new Regex("'http://go.gotourls.bid/golxyb2/(.+?)\\.", RegexOptions.Compiled);

		protected Regex regSize = new Regex("Size(?:.+?)<b>(.+?)<\\/b>", RegexOptions.Compiled);

		public override string Name => "cnbtkitty.org";

		protected override string GetURL(string word, int page)
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
				return "http://cnbtkitty.org/search/" + text + "/" + page + "/4/0.html";
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

		protected override Result ReadPart(string part)
		{
			Result result = new Result();
			try
			{
				if (!regName.IsMatch(part))
				{
					return null;
				}
				if (!regMagnet.IsMatch(part))
				{
					return null;
				}
				if (!regSize.IsMatch(part))
				{
					return null;
				}
				result.Name = regName.Match(part).Groups[1].Value;
				result.Name = result.Name.Replace("<b>", "");
				result.Name = result.Name.Replace("</b>", "");
				if (result.Name.EndsWith(".torrent"))
				{
					result.Name = result.Name.Substring(0, result.Name.Length - ".torrent".Length);
				}
				result.Magnet = "magnet:?xt=urn:btih:" + regMagnet.Match(part).Groups[1].Value;
				result.Size = regSize.Match(part).Groups[1].Value;
				result.From = Name;
				return result;
			}
			catch
			{
				return null;
			}
		}
	}
}
