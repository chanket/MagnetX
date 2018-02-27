using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MagnetX.List
{
    class ListView : System.Windows.Forms.ListView
    {
        public ListView()
        {
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            UpdateStyles();
        }

        public void UniqueItemClear()
        {
            Items.Clear();
            Hash.Clear();
        }

        public bool UniqueItemAdd(ListViewItem item, string hash)
        {
            if (Hash.Add(hash))
            {
                Items.Add(item);
                return true;
            }
            else
            {
                return false;
            }
        }

        protected HashSet<string> Hash { get; set; } = new HashSet<string>();
    }
}
