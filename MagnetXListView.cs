using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MagnetX
{
    class MagnetXListView : ListView
    {
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
