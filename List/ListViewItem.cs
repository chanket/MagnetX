using MagnetX.Searcher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagnetX.List
{
    /// <summary>
    /// 表示<see cref="MagnetX.List.ListView"/>中的元素，描述一项搜索结果。
    /// </summary>
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
