using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagnetX.Searcher
{
    public class Result
    {
        public string Name { get; set; }
        public string Magnet { get; set; }
        public string Size { get; set; }
        public string From { get; set; }
    }

    public enum TestResults
    {
        OK,
        Timeout,
        ServerError,
        FormatError,
        UnknownError,
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class SearcherEnabledAttribute : System.Attribute
    {

    }

    abstract class Searcher
    {
        public delegate bool SearcherResultsDelegate(Searcher sender, List<Result> results);
        public SearcherResultsDelegate OnResults = null;

        public bool Break { get; set; } = false;
        public object Tag { get; set; } = null;
        public abstract string Name { get; }
        public abstract void SearchAsync(string word);
        public abstract Task<TestResults> TestAsync();
    }
}
