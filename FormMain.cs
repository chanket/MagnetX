using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MagnetX.Searcher;
using MagnetX.Searcher.HistorySearcher;

namespace MagnetX
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
            this.MinimumSize = new Size(300, 200);
        }

        private void FormMain_SizeChanged(object sender, EventArgs e)
        {
            textBoxWord.Width = this.Width - 125;
            buttonSearch.Left = textBoxWord.Left + textBoxWord.Width + 6;
            listViewResults.Width = this.Width - 40;
            listViewResults.Height = this.Height - 106;
            if (columnHeaderName.Width + columnHeaderSize.Width != listViewResults.Width - 24)
            {
                this.columnHeaderName.Width = Math.Max(75, listViewResults.Width - 24 - (columnHeaderSize.Width + columnHeaderFrom.Width));
            }
        }


        private void buttonSearch_Click(object sender, EventArgs e)
        {
            Search(textBoxWord.Text);
        }

        private void textBoxWord_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Search(textBoxWord.Text);
            }
        }

        private bool searcher_Results(Searcher.Searcher sender, List<Result> results)
        {
            bool retval = false;
            if (!(sender is HistorySearcher) && Utils.Cache && results.Count != 0)
            {
                Utils.HistorySearcher.InsertAsync(results);
            }

            this.Invoke(new MethodInvoker(() =>
            {
                int gen = (int)sender.Tag;
                if (gen != searchGeneration)
                {
                    retval = false;
                }
                else
                {
                    listViewResults.BeginUpdate();
                    foreach (Result r in results)
                    {
                        listViewResults.UniqueItemAdd(new List.ListViewItem(r), r.Magnet.ToLower());
                    }
                    listViewResults.EndUpdate();

                    this.Text = "MagnetX (" + listViewResults.Items.Count + "个结果)";

                    retval = true;
                }
            }));
            return retval;
        }

        private void listViewResults_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (listViewResults.SelectedItems.Count != 0)
                {
                    var r = (listViewResults.SelectedItems[0] as List.ListViewItem).Result;
                    contextMenuStripResult.Items[2].Text = r.Name;
                    toCopy = r.Magnet;
                    contextMenuStripResult.Show(listViewResults, new Point(e.X, e.Y));
                }
            }
        }

        private void listViewResults_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (listViewResults.SelectedItems.Count != 0)
                {
                    var r = (listViewResults.SelectedItems[0] as List.ListViewItem).Result;
                    Process.Start(r.Magnet);
                }
            }
        }

        private string toCopy = "";
        private void 复制ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.Clear();
            Clipboard.SetText(toCopy);
        }


        private int searchGeneration = 0;
        private void Search(string word)
        {
            searchGeneration++;
            listViewResults.UniqueItemClear();
            if (!string.IsNullOrEmpty(word))
            {
                foreach (var s in Utils.GetAllSearchers())
                {
                    if (Utils.GetSearcherEnabled(s))
                    {
                        s.Tag = searchGeneration;
                        s.OnResults += searcher_Results;
                        s.SearchAsync(word);
                    }
                }
            }
            this.Text = "MagnetX";
        }

        private void 数据源ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FormSource().ShowDialog();
        }

        private void listViewResults_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            switch (e.Column)
            {
                case 0:
                    {
                        if (listViewResults.ListViewItemSorter is List.Comparers.NameComparer)
                        {
                            listViewResults.ListViewItemSorter = new List.Comparers.NameComparer(!(listViewResults.ListViewItemSorter as List.Comparers.NameComparer).Asc);
                        }
                        else
                        {
                            listViewResults.ListViewItemSorter = new List.Comparers.NameComparer(true);
                        }

                        listViewResults.Sort();
                    }break;

                case 1:
                    {
                        if (listViewResults.ListViewItemSorter is List.Comparers.SizeComparer)
                        {
                            listViewResults.ListViewItemSorter = new List.Comparers.SizeComparer(!(listViewResults.ListViewItemSorter as List.Comparers.SizeComparer).Asc);
                        }
                        else
                        {
                            listViewResults.ListViewItemSorter = new List.Comparers.SizeComparer(true);
                        }

                        listViewResults.Sort();
                    }
                    break;

                case 2:
                    {
                        if (listViewResults.ListViewItemSorter is List.Comparers.FromComparer)
                        {
                            listViewResults.ListViewItemSorter = new List.Comparers.FromComparer(!(listViewResults.ListViewItemSorter as List.Comparers.FromComparer).Asc);
                        }
                        else
                        {
                            listViewResults.ListViewItemSorter = new List.Comparers.FromComparer(true);
                        }

                        listViewResults.Sort();
                    }
                    break;
            }
        }

        private void FormMain_Load(object sender, EventArgs e)
        {

        }
    }
}
