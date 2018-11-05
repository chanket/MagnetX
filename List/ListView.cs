using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MagnetX.List
{
    /// <summary>
    /// 继承自<see cref="System.Windows.Forms.ListView"/>，
    /// 使用<see cref="MagnetX.List.ListViewItem"/>作为基本元素，
    /// 并且提供了<see cref="MagnetX.List.ListView.UniqueItemAdd(ListViewItem, string)"/>
    /// 和<see cref="MagnetX.List.ListView.UniqueItemClear" />方法，
    /// 用于添加hash值唯一的元素。
    /// </summary>
    class ListView : System.Windows.Forms.ListView
    {
        public ListView()
        {
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            UpdateStyles();
        }

        /// <summary>
        /// 清空所有元素，并清空判断唯一元素的哈希表。
        /// </summary>
        public void UniqueItemClear()
        {
            base.Items.Clear();
            Hash.Clear();
        }

        /// <summary>
        /// 添加唯一元素。
        /// </summary>
        /// <param name="item"></param>
        /// <param name="hash"></param>
        /// <returns></returns>
        public bool UniqueItemAdd(ListViewItem item)
        {
            if (Hash.Add(item.Result.Magnet))
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
