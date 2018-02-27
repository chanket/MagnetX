using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagnetX.List.Comparers
{
    public class SizeComparer : System.Collections.IComparer
    {
        public bool Asc { get; }

        public SizeComparer(bool asc)
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
            else if (size.EndsWith("byte", StringComparison.OrdinalIgnoreCase))
            {
                size = size.Substring(0, size.Length - 4);
                double.TryParse(size, out retval);
            }
            else if (size.EndsWith("b", StringComparison.OrdinalIgnoreCase))
            {
                size = size.Substring(0, size.Length - 1);
                double.TryParse(size, out retval);
            }
            else
            {
                double.TryParse(size, out retval);
            }

            return retval;
        }

        public int Compare(object x, object y)
        {
            ListViewItem xx = x as ListViewItem;
            ListViewItem yy = y as ListViewItem;
            double xSize = ParseSize(xx.Result.Size);
            double ySize = ParseSize(yy.Result.Size);

            if (xSize < ySize) return (Asc ? 1 : -1) * 1;
            else if (xSize > ySize) return (Asc ? 1 : -1) * -1;
            else return 0;
        }
    }
}
