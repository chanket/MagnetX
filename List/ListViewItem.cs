using MagnetX.Searcher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagnetX.List
{
    class ListViewItem : System.Windows.Forms.ListViewItem
    {
        public Result Result { get; }

        public ListViewItem(Result r)
        {
            Result = r;
            this.SubItems[0] = new ListViewSubItem(this, r.Name);
            this.SubItems.Add(r.Size);
            this.SubItems.Add(r.From);
        }
    }
}
