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
        /// 搜索指定关键词的实现。
        /// 对于不同的源，不需要修改这个方法，只需要继承本类，然后实现本类的抽象方法即可。
        /// </summary>
        /// <param name="word">关键词</param>
        /// <returns></returns>
        public override async Task SearchAsync(string word)
        {
            bool isBreak = false;

            await Task.Run(() => {
                Parallel.For(1, 101, async (page) =>
                {
                    if (isBreak) return;

                    try
                    {
                        string url = GetURL(word, page);
                        List<Result> list = new List<Result>();
                        for (int ntry = 0; ntry < 8 && !isBreak; ntry++)
                        {
                            HttpClient hc = new HttpClient();
                            hc.Timeout = TimeSpan.FromMilliseconds(5000 + ntry * 250);
                            try
                            {
                                var resp = await hc.GetAsync(url);
                                if (resp.StatusCode == System.Net.HttpStatusCode.OK)
                                {
                                    foreach (string part in PrepareParts(await resp.Content.ReadAsStringAsync()))
                                    {
                                        var result = ReadPart(part);
                                        if (result != null) list.Add(ReadPart(part));
                                    }
                                    break;
                                }
                                else
                                {
                                    await Task.Delay(250);
                                }
                            }
                            catch (TaskCanceledException)
                            {
                                await Task.Delay(250);
                            }
                        }

                        if (Break)
                            isBreak = true;
                        if (OnResults == null)
                            isBreak = true;
                        if (list.Count == 0)
                            isBreak = true;
                        if (!OnResults.Invoke(this, list))
                            isBreak = true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                });
            });
        }

        /// <summary>
        /// 测试该源是否可用的方法。
        /// </summary>
        /// <returns></returns>
        public override async Task<TestResults> TestAsync()
        {
            string url = GetURL("电影", 1);
            HttpClient hc = new HttpClient();
            hc.Timeout = TimeSpan.FromMilliseconds(10000);
            try
            {
                var resp = await hc.GetAsync(url);
                if (resp.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    foreach (string part in PrepareParts(await resp.Content.ReadAsStringAsync()))
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
