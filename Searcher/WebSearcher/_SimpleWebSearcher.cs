using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MagnetX.Searcher.WebSearcher
{
    /// <summary>
    /// 描述来自网页的数据源的抽象类。
    /// 相比于<see cref="WebSearcher"/>，
    /// 它使用<see cref="SplitToken"/>对每页的结果进行切割，
    /// 通过正则表达式在每个切割块内提取结果。
    /// </summary>
    abstract class SimpleWebSearcher : WebSearcher
    {
        public SimpleWebSearcher(string name, string splitToken, Regex regName, Regex regMagnet, Regex regSize)
        {
            this.Name = name;
            this.SplitToken = splitToken;
            this.RegexName = regName;
            this.RegexMagnet = regMagnet;
            this.RegexSize = regSize;
        }

        public sealed override string Name { get; }
        protected string SplitToken { get; }
        protected Regex RegexName { get; }
        protected Regex RegexMagnet { get; }
        protected Regex RegexSize { get; }

        /// <summary>
        /// 使用<see cref="SplitToken"/>切割每页原始数据，抛弃首个结果并返回后续结果。
        /// </summary>
        protected virtual IEnumerable<string> GetParts(string html)
        {
            string[] parts = html.Split(new string[] { SplitToken }, StringSplitOptions.None);
            for (int i = 1; i < parts.Length; i++)
            {
                yield return parts[i];
            }
        }

        /// <summary>
        /// 从一个切割部分，使用正则表达式<see cref="RegexName"/>、<see cref="RegexMagnet"/>和<see cref="RegexSize"/>匹配的结果返回<see cref="Result"/>对象。
        /// 匹配失败时将返回null。
        /// </summary>
        protected virtual async Task<Result> GetResultAsync(string part)
        {
            Result result = new Result() { From = Name, };
            var matchName = RegexName.Match(part);
            var matchMagnet = RegexMagnet.Match(part);
            var matchSize = RegexSize.Match(part);
            if (matchName.Success && matchMagnet.Success && matchSize.Success)
            {
                result.Name = matchName.Groups[1].Value;
                result.Magnet = "magnet:?xt=urn:btih:" + matchMagnet.Groups[1].Value;
                result.Size = matchSize.Groups[1].Value;
                return result;
            }
            else
            {
                return null;
            }
        }

        public override async Task<TestResults> TestAsync()
        {
            string url = await GetURLAsync(TestKeyword, 1).ConfigureAwait(false);
            using (HttpClient hc = CreateHttpClient())
            {
                hc.Timeout = TimeSpan.FromMilliseconds(TestTimeout);
                try
                {
                    var resp = await hc.GetAsync(url).ConfigureAwait(false);
                    if (resp.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string html = await hc.GetStringAsync(url).ConfigureAwait(false);
                        var parts = GetParts(html);

                        if (parts.FirstOrDefault() == null)
                        {
                            return TestResults.FormatError;
                        }
                        else
                        {
                            var result = GetResultAsync(parts.FirstOrDefault());
                            if (result == null)
                            {
                                return TestResults.FormatError;
                            }
                            else
                            {
                                return TestResults.OK;
                            }
                        }
                    }
                    else
                    {
                        return TestResults.ServerError;
                    }
                }
                catch (TaskCanceledException)
                {
                    return TestResults.Timeout;
                }
                catch (Exception)
                {
                    return TestResults.UnknownError;
                }
            }
        }

        protected sealed override async Task<IEnumerable<Result>> GetResultsAsync(string html)
        {
            List<Result> results = new List<Result>();
            foreach (string part in GetParts(html))
            {
                Result result = await GetResultAsync(part).ConfigureAwait(false);
                if (result != null) results.Add(result);
            }
            return results;
        }
    }
}
