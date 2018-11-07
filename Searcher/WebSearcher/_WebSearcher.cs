using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace MagnetX.Searcher.WebSearcher
{
    /// <summary>
    /// 描述来自网页的数据源的抽象类。
    /// </summary>
    abstract class WebSearcher : Searcher
    {
        protected static Regex regCfEmail1 = new Regex("<span class=\"__cf_email__\".+?>", RegexOptions.Compiled);
        protected string HandleCfEmail(string name)
        {
            return regCfEmail1.Replace(name, "").Replace("</span>", "");
        }

        /// <summary>
        /// 创建HttpClient。
        /// 子类可以重载此方法，来实现自定义请求头。
        /// </summary>
        /// <returns></returns>
        protected virtual HttpClient CreateHttpClient()
        {
            HttpClient httpClient = new HttpClient(new HttpClientHandler()
            {
                AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate,
            }, true);

            httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (compatible; Baiduspider/2.0; +http://www.baidu.com/search/spider.html)");
            return httpClient;
        }

        /// <summary>
        /// 搜索指定关键词的实现。
        /// </summary>
        /// <param name="word">关键词</param>
        /// <returns></returns>
        public sealed override async void SearchAsync(string word)
        {
            for (int page = 1; page < 20; page++)
            {
                try
                {
                    string url = await GetURLAsync(word, page).ConfigureAwait(false);
                    List<Result> list = new List<Result>();
                    for (int ntry = 0; ntry < 5; ntry++)
                    {
                        using (HttpClient hc = CreateHttpClient())
                        {
                            hc.Timeout = TimeSpan.FromMilliseconds(8000 + ntry * 2000);

                            try
                            {
                                var resp = await hc.GetAsync(url).ConfigureAwait(false);
                                if (resp.StatusCode == System.Net.HttpStatusCode.OK)
                                {
                                    string html = await hc.GetStringAsync(url).ConfigureAwait(false);
                                    var results = await GetResultsAsync(html).ConfigureAwait(false);

                                    if (results != null)
                                    {
                                        foreach (var result in results)
                                        {
                                            if (result != null)
                                            {
                                                result.Name = HandleCfEmail(result.Name);
                                                list.Add(result);
                                            }
                                        }

                                        if (results.FirstOrDefault() != null) break;
                                    }
                                }
                                else
                                {
                                    await Task.Delay(250).ConfigureAwait(false);
                                }
                            }
                            catch
                            {
                                await Task.Delay(250).ConfigureAwait(false);
                            }
                        }
                    }

                    if (list.Count == 0) break;
                    if (OnResults == null) break;
                    if (!OnResults.Invoke(this, list)) break;
                    list.Clear();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        /// <summary>
        /// <see cref="TestAsync"/>方法使用的关键词。
        /// </summary>
        protected const string TestKeyword = "电影";

        /// <summary>
        /// <see cref="TestAsync"/>中Http请求的超时时间。
        /// </summary>
        protected const int TestTimeout = 15000;

        /// <summary>
        /// 测试该源是否可用的方法。
        /// </summary>
        /// <returns></returns>
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
                        var results = await GetResultsAsync(html).ConfigureAwait(false);

                        if (results != null && results.FirstOrDefault() != null)
                        {
                            return TestResults.OK;
                        }
                        else
                        {
                            return TestResults.FormatError;
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

        /// <summary>
        /// 抽象方法，用于获取指定关键字指定页号的资源所在的URL。
        /// </summary>
        /// <param name="word">关键字</param>
        /// <param name="page">页号</param>
        /// <returns></returns>
        protected abstract Task<string> GetURLAsync(string word, int page);

        /// <summary>
        /// 抽象方法，用于解析从GetURL方法获得的分段，返回Result类型的结果。
        /// </summary>
        /// <param name="part">分段</param>
        /// <returns></returns>
        protected abstract Task<IEnumerable<Result>> GetResultsAsync(string html);
    }
}
