using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MagnetX.Searcher.WebSearcher
{
    abstract class WebSearcher : Searcher
    {
        /// <summary>
        /// 创建HttpClient。
        /// 子类可以重载此方法，来实现自定义请求头。
        /// </summary>
        /// <returns></returns>
        protected virtual HttpClient CreateHttpClient()
        {
            HttpClient httpClient = new HttpClient();
            return httpClient;
        }

        /// <summary>
        /// 搜索指定关键词的实现。
        /// 对于不同的源，不需要修改这个方法，只需要继承本类，然后实现本类的抽象方法即可。
        /// </summary>
        /// <param name="word">关键词</param>
        /// <returns></returns>
        public override async Task SearchAsync(string word)
        {
            for (int page = 1; page < 100; page++)
            {
                try
                {
                    string url = GetURL(word, page);
                    List<Result> list = new List<Result>();
                    for (int ntry = 0; ntry < 8; ntry++)
                    {
                        HttpClient hc = CreateHttpClient();
                        hc.Timeout = TimeSpan.FromMilliseconds(5000 + ntry * 250);
                        try
                        {
                            var resp = await hc.GetAsync(url).ConfigureAwait(false);
                            if (resp.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                foreach (string part in PrepareParts(await resp.Content.ReadAsStringAsync().ConfigureAwait(false)))
                                {
                                    var result = ReadPart(part);
                                    if (result != null) list.Add(ReadPart(part));
                                }
                                break;
                            }
                            else
                            {
                                await Task.Delay(250).ConfigureAwait(false);
                            }
                        }
                        catch (TaskCanceledException)
                        {
                            await Task.Delay(250).ConfigureAwait(false);
                        }
                    }

                    if (list.Count == 0) break;
                    if (OnResults == null) break;
                    if (!OnResults.Invoke(this, list)) break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        /// <summary>
        /// 测试该源是否可用的方法。
        /// </summary>
        /// <returns></returns>
        public override async Task<TestResults> TestAsync()
        {
            string url = GetURL("电影", 1);
            HttpClient hc = CreateHttpClient();
            hc.Timeout = TimeSpan.FromMilliseconds(10000);
            try
            {
                var resp = await hc.GetAsync(url).ConfigureAwait(false);
                if (resp.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    foreach (string part in PrepareParts(await resp.Content.ReadAsStringAsync().ConfigureAwait(false)))
                    {
                        var result = ReadPart(part);
                        if (result != null) return TestResults.OK;
                    }
                    return TestResults.Unusable;
                }
                else
                {
                    return TestResults.Unusable;
                }
            }
            catch (TaskCanceledException)
            {
                return TestResults.Timeout;
            }
            catch (Exception)
            {
                return TestResults.Unusable;
            }
        }

        protected virtual Encoding DefaultEncoding { get; set; } = Encoding.UTF8;

        /// <summary>
        /// 抽象方法，用于获取指定关键字制定页号的搜索URL。
        /// </summary>
        /// <param name="word">关键字</param>
        /// <param name="page">页号</param>
        /// <returns></returns>
        protected abstract string GetURL(string word, int page);

        /// <summary>
        /// 抽象方法，对网页的原始内容进行预处理，返回若干个不同的分段，各个分段包含一个完整的结果。
        /// </summary>
        /// <param name="content">原始内容</param>
        /// <returns></returns>
        protected abstract IEnumerable<string> PrepareParts(string content);

        /// <summary>
        /// 抽象方法，用于解析从GetURL方法获得的分段，返回Result类型的结果。
        /// </summary>
        /// <param name="part">分段</param>
        /// <returns></returns>
        protected abstract Result ReadPart(string part);
    }
}
