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
                                    foreach (string part in GetParts(await resp.Content.ReadAsStringAsync()))
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

        public override async Task<TestResults> TestAsync()
        {
            string url = GetURL("电影", 1);
            HttpClient hc = new HttpClient();
            hc.Timeout = TimeSpan.FromMilliseconds(5000);
            try
            {
                var resp = await hc.GetAsync(url);
                if (resp.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    foreach (string part in GetParts(await resp.Content.ReadAsStringAsync()))
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

        protected abstract string GetURL(string word, int page);

        protected abstract IEnumerable<string> GetParts(string content);

        protected abstract Result ReadPart(string part);
    }
}
