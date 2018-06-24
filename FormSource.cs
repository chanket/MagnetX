using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MagnetX
{
    public partial class FormSource : Form
    {
        public FormSource()
        {
            InitializeComponent();
        }

        private void FormSource_Load(object sender, EventArgs e)
        {
            foreach (var s in Utils.GetAllSearchers())
            {
                ListViewItem lvi = new ListViewItem(s.Name);
                lvi.SubItems.Add("");
                lvi.Tag = s;
                listView1.Items.Add(lvi);
                if (Utils.GetSearcherEnabled(s)) lvi.Checked = true;
            }
        }

        private void FormSource_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (ListViewItem lvi in listView1.Items)
            {
                Utils.SetSearcherEnabled(lvi.Tag as Searcher.Searcher, lvi.Checked);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;

            int count = listView1.Items.Count;
            foreach (ListViewItem lvi in listView1.Items)
            {
                Searcher.Searcher s = lvi.Tag as Searcher.Searcher;
                if (s != null)
                {
                    Task.Run(async () =>
                    {
                        var result = await s.TestAsync();
                        this.Invoke(new MethodInvoker(() => {
                            switch (result)
                            {
                                case Searcher.TestResults.OK:
                                    lvi.SubItems[1].Text = "可用";
                                    break;
                                case Searcher.TestResults.Unusable:
                                    lvi.SubItems[1].Text = "错误";
                                    break;
                                case Searcher.TestResults.Timeout:
                                    lvi.SubItems[1].Text = "超时";
                                    break;
                            }

                            if (--count == 0)
                            {
                                button1.Enabled = true;
                            }
                        }));
                    });
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem lvi in listView1.Items)
            {
                lvi.Checked = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem lvi in listView1.Items)
            {
                lvi.Checked = false;
            }
        }
    }
}
