using MagnetX.Searcher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MagnetX
{
    class MagnetXListViewItem : ListViewItem
    {
        public Result Result { get; }

        public MagnetXListViewItem(Result r)
        {
            Result = r;
            this.SubItems[0] = new ListViewSubItem(this, r.Name);
            this.SubItems.Add(r.Size);
            this.SubItems.Add(r.From);
        }
    }

    class MagnetXListView : ListView
    {
        public class MagnetXListViewNameComparer : System.Collections.IComparer
        {
            public bool Asc { get; }

            public MagnetXListViewNameComparer(bool asc)
            {
                Asc = asc;
            }

            public int Compare(object x, object y)
            {
                MagnetXListViewItem xx = x as MagnetXListViewItem;
                MagnetXListViewItem yy = y as MagnetXListViewItem;
                return (Asc ? 1 : -1) * string.Compare(xx.Result.Name, yy.Result.Name);
            }
        }

        public class MagnetXListViewSizeComparer : System.Collections.IComparer
        {
            public bool Asc { get; }

            public MagnetXListViewSizeComparer(bool asc)
            {
                Asc = asc;
            }

            private double ParseSize(string size)
            {
                double retval = 0;
                size = size.Trim();
                if (size.EndsWith("kb", StringComparison.OrdinalIgnoreCase))
                {
                    size = size.Substring(0, size.Length - 2);
                    double.TryParse(size, out retval);
                    retval *= 1024;
                }
                else if (size.EndsWith("mb", StringComparison.OrdinalIgnoreCase))
                {
                    size = size.Substring(0, size.Length - 2);
                    double.TryParse(size, out retval);
                    retval *= 1024;
                    retval *= 1024;
                }
                else if (size.EndsWith("gb", StringComparison.OrdinalIgnoreCase))
                {
                    size = size.Substring(0, size.Length - 2);
                    double.TryParse(size, out retval);
                    retval *= 1024;
                    retval *= 1024;
                    retval *= 1024;
                }
                else if (size.EndsWith("tb", StringComparison.OrdinalIgnoreCase))
                {
                    size = size.Substring(0, size.Length - 2);
                    double.TryParse(size, out retval);
                    retval *= 1024;
                    retval *= 1024;
                    retval *= 1024;
                    retval *= 1024;
                }
                else
                {
                    size = size.Substring(0, size.Length - 2);
                    double.TryParse(size, out retval);
                }

                return retval;
            }

            public int Compare(object x, object y)
            {
                MagnetXListViewItem xx = x as MagnetXListViewItem;
                MagnetXListViewItem yy = y as MagnetXListViewItem;
                double xSize = ParseSize(xx.Result.Size);
                double ySize = ParseSize(yy.Result.Size);

                if (xSize < ySize) return (Asc ? 1 : -1) * 1;
                else if (xSize > ySize) return (Asc ? 1 : -1) * -1;
                else return 0;
            }
        }

        public class MagnetXListViewFromComparer : System.Collections.IComparer
        {
            public bool Asc { get; }

            public MagnetXListViewFromComparer(bool asc)
            {
                Asc = asc;
            }

            public int Compare(object x, object y)
            {
                MagnetXListViewItem xx = x as MagnetXListViewItem;
                MagnetXListViewItem yy = y as MagnetXListViewItem;
                return (Asc ? 1 : -1) * string.Compare(xx.Result.From, yy.Result.From);
            }
        }

        public MagnetXListView()
        {
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            UpdateStyles();
        }

        protected HashSet<string> Hash { get; set; } = new HashSet<string>();
        public void UniqueItemClear()
        {
            Items.Clear();
            Hash.Clear();
        }
        public bool UniqueItemAdd(ListViewItem item, string hash)
        {
            bool a = Hash.Add(hash);
            if (a) Items.Add(item);
            return a;
        }
    }
}
