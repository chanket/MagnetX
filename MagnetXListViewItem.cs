using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MagnetX.Searcher;

namespace MagnetX
{
    public class MagnetXListViewItem : ListViewItem
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
}
