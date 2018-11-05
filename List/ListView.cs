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

        #region UniqueItem
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
        #endregion

        #region Sort
        /// <summary>
        /// 描述<see cref="ListView"/>的排序方法。
        /// </summary>
        public enum ListViewSortTypes
        {
            /// <summary>
            /// 不排序。
            /// </summary>
            None = 0,

            /// <summary>
            /// 资源名升序。
            /// </summary>
            NameAsc = Name | Asc,

            /// <summary>
            /// 资源名降序。
            /// </summary>
            NameDesc = Name | Desc,

            /// <summary>
            /// 大小升序。
            /// </summary>
            SizeAsc = Size | Asc,

            /// <summary>
            /// 大小降序。
            /// </summary>
            SizeDesc = Size | Desc,

            /// <summary>
            /// 源升序。
            /// </summary>
            FromAsc = From | Asc,

            /// <summary>
            /// 源降序。
            /// </summary>
            FromDesc = From | Desc,

            Asc = 1,
            Desc = 2,
            Name = 4,
            Size = 8,
            From = 16,
        }

        private ListViewSortTypes listViewSortType = ListViewSortTypes.None;

        /// <summary>
        /// 获取或设置<see cref="ListView"/>的排序方法。
        /// </summary>
        public ListViewSortTypes ListViewSortType
        {
            get
            {
                return listViewSortType;
            }
            set
            {
                listViewSortType = value;

                if (listViewSortType.HasFlag(ListViewSortTypes.Name))
                {
                    StringComparer stringComparer = StringComparer.CurrentCulture;
                    if (listViewSortType.HasFlag(ListViewSortTypes.Asc))
                    {
                        base.ListViewItemSorter = Comparer<ListViewItem>.Create(new Comparison<ListViewItem>((ListViewItem a, ListViewItem b) => {
                            return stringComparer.Compare(a.Result.Name, b.Result.Name);
                        }));
                    }
                    else if (listViewSortType.HasFlag(ListViewSortTypes.Desc))
                    {
                        base.ListViewItemSorter = Comparer<ListViewItem>.Create(new Comparison<ListViewItem>((ListViewItem a, ListViewItem b) => {
                            return -stringComparer.Compare(a.Result.Name, b.Result.Name);
                        }));
                    }
                    else
                    {
                        base.ListViewItemSorter = null;
                    }
                }
                else if (listViewSortType.HasFlag(ListViewSortTypes.Size))
                {
                    StringComparer stringComparer = StringComparer.CurrentCulture;
                    if (listViewSortType.HasFlag(ListViewSortTypes.Asc))
                    {
                        base.ListViewItemSorter = Comparer<ListViewItem>.Create(new Comparison<ListViewItem>((ListViewItem a, ListViewItem b) => {
                            return stringComparer.Compare(a.Result.Size, b.Result.Size);
                        }));
                    }
                    else if (listViewSortType.HasFlag(ListViewSortTypes.Desc))
                    {
                        base.ListViewItemSorter = Comparer<ListViewItem>.Create(new Comparison<ListViewItem>((ListViewItem a, ListViewItem b) => {
                            return -stringComparer.Compare(a.Result.Size, b.Result.Size);
                        }));
                    }
                    else
                    {
                        base.ListViewItemSorter = null;
                    }
                }
                else if (listViewSortType.HasFlag(ListViewSortTypes.From))
                {
                    StringComparer stringComparer = StringComparer.CurrentCulture;
                    if (listViewSortType.HasFlag(ListViewSortTypes.Asc))
                    {
                        base.ListViewItemSorter = Comparer<ListViewItem>.Create(new Comparison<ListViewItem>((ListViewItem a, ListViewItem b) => {
                            return stringComparer.Compare(a.Result.From, b.Result.From);
                        }));
                    }
                    else if (listViewSortType.HasFlag(ListViewSortTypes.Desc))
                    {
                        base.ListViewItemSorter = Comparer<ListViewItem>.Create(new Comparison<ListViewItem>((ListViewItem a, ListViewItem b) => {
                            return -stringComparer.Compare(a.Result.From, b.Result.From);
                        }));
                    }
                    else
                    {
                        base.ListViewItemSorter = null;
                    }
                }
                else
                {
                    base.ListViewItemSorter = null;
                }

                this.Sort();
            }
        }
        #endregion
    }
}
