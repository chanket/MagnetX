using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagnetX.Searcher
{
    /// <summary>
    /// 搜索结果。
    /// </summary>
    public class Result
    {
        public string Name { get; set; }
        public string Magnet { get; set; }
        public string Size { get; set; }
        public string From { get; set; }
    }

    /// <summary>
    /// <see cref="Searcher"/>的状态。
    /// </summary>
    public enum TestResults
    {
        OK,
        Timeout,
        ServerError,
        FormatError,
        UnknownError,
    }

    /// <summary>
    /// 标记<see cref="Searcher"/>为启用状态的属性。
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class SearcherEnabledAttribute : System.Attribute
    {

    }

    /// <summary>
    /// <see cref="Searcher"/>返回搜索结果的事件委托。
    /// </summary>
    /// <param name="sender">返回搜索结果的<see cref="Searcher"/>对象。</param>
    /// <param name="results">搜索结果的列表。</param>
    /// <returns>指示是否继续搜索。</returns>
    delegate bool SearcherResultsDelegate(Searcher sender, List<Result> results);

    /// <summary>
    /// 数据源的抽象类。
    /// </summary>
    abstract class Searcher
    {
        public SearcherResultsDelegate OnResults = null;

        public object Tag { get; set; } = null;
        public abstract string Name { get; }
        public abstract void SearchAsync(string word);
        public abstract Task<TestResults> TestAsync();
    }
}
