using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagnetX.List.Comparers
{
    public class FromComparer : System.Collections.IComparer
    {
        public bool Asc { get; }

        public FromComparer(bool asc)
        {
            Asc = asc;
        }

        public int Compare(object x, object y)
        {
            ListViewItem xx = x as ListViewItem;
            ListViewItem yy = y as ListViewItem;
            return (Asc ? 1 : -1) * string.Compare(xx.Result.From, yy.Result.From);
        }
    }
}
