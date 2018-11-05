namespace MagnetX
{
    partial class FormMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.textBoxWord = new System.Windows.Forms.TextBox();
            this.buttonSearch = new System.Windows.Forms.Button();
            this.contextMenuStripResult = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.复制ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.资源名ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.功能ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.数据源ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.githubToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.保存纪录ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listViewResults = new MagnetX.List.ListView();
            this.columnHeaderName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderFrom = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStripResult.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxWord
            // 
            this.textBoxWord.Location = new System.Drawing.Point(12, 28);
            this.textBoxWord.Name = "textBoxWord";
            this.textBoxWord.Size = new System.Drawing.Size(778, 21);
            this.textBoxWord.TabIndex = 0;
            this.textBoxWord.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxWord_KeyDown);
            // 
            // buttonSearch
            // 
            this.buttonSearch.Location = new System.Drawing.Point(796, 28);
            this.buttonSearch.Name = "buttonSearch";
            this.buttonSearch.Size = new System.Drawing.Size(79, 21);
            this.buttonSearch.TabIndex = 1;
            this.buttonSearch.Text = "搜索";
            this.buttonSearch.UseVisualStyleBackColor = true;
            this.buttonSearch.Click += new System.EventHandler(this.buttonSearch_Click);
            // 
            // contextMenuStripResult
            // 
            this.contextMenuStripResult.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.复制ToolStripMenuItem,
            this.toolStripSeparator1,
            this.资源名ToolStripMenuItem});
            this.contextMenuStripResult.Name = "contextMenuStripResult";
            this.contextMenuStripResult.Size = new System.Drawing.Size(125, 54);
            // 
            // 复制ToolStripMenuItem
            // 
            this.复制ToolStripMenuItem.Name = "复制ToolStripMenuItem";
            this.复制ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.复制ToolStripMenuItem.Text = "复制链接";
            this.复制ToolStripMenuItem.Click += new System.EventHandler(this.复制ToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(121, 6);
            // 
            // 资源名ToolStripMenuItem
            // 
            this.资源名ToolStripMenuItem.Enabled = false;
            this.资源名ToolStripMenuItem.Name = "资源名ToolStripMenuItem";
            this.资源名ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.资源名ToolStripMenuItem.Text = "资源名";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.功能ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(885, 25);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.MenuActivate += new System.EventHandler(this.menuStrip1_MenuActivate);
            // 
            // 功能ToolStripMenuItem
            // 
            this.功能ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.数据源ToolStripMenuItem,
            this.githubToolStripMenuItem,
            this.toolStripSeparator2,
            this.保存纪录ToolStripMenuItem});
            this.功能ToolStripMenuItem.Name = "功能ToolStripMenuItem";
            this.功能ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.功能ToolStripMenuItem.Text = "菜单";
            // 
            // 数据源ToolStripMenuItem
            // 
            this.数据源ToolStripMenuItem.Name = "数据源ToolStripMenuItem";
            this.数据源ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.数据源ToolStripMenuItem.Text = "数据源";
            this.数据源ToolStripMenuItem.Click += new System.EventHandler(this.数据源ToolStripMenuItem_Click);
            // 
            // githubToolStripMenuItem
            // 
            this.githubToolStripMenuItem.Name = "githubToolStripMenuItem";
            this.githubToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.githubToolStripMenuItem.Text = "检查更新";
            this.githubToolStripMenuItem.Click += new System.EventHandler(this.githubToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(177, 6);
            // 
            // 保存纪录ToolStripMenuItem
            // 
            this.保存纪录ToolStripMenuItem.Checked = true;
            this.保存纪录ToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.保存纪录ToolStripMenuItem.Name = "保存纪录ToolStripMenuItem";
            this.保存纪录ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.保存纪录ToolStripMenuItem.Text = "保存记录";
            this.保存纪录ToolStripMenuItem.Click += new System.EventHandler(this.保存纪录ToolStripMenuItem_Click);
            // 
            // listViewResults
            // 
            this.listViewResults.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderName,
            this.columnHeaderSize,
            this.columnHeaderFrom});
            this.listViewResults.FullRowSelect = true;
            this.listViewResults.ListViewSortType = MagnetX.List.ListView.ListViewSortTypes.None;
            this.listViewResults.Location = new System.Drawing.Point(12, 55);
            this.listViewResults.Name = "listViewResults";
            this.listViewResults.Size = new System.Drawing.Size(861, 401);
            this.listViewResults.TabIndex = 2;
            this.listViewResults.UseCompatibleStateImageBehavior = false;
            this.listViewResults.View = System.Windows.Forms.View.Details;
            this.listViewResults.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listViewResults_ColumnClick);
            this.listViewResults.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listViewResults_MouseDoubleClick);
            this.listViewResults.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listViewResults_MouseDown);
            // 
            // columnHeaderName
            // 
            this.columnHeaderName.Text = "资源名";
            this.columnHeaderName.Width = 652;
            // 
            // columnHeaderSize
            // 
            this.columnHeaderSize.Text = "大小";
            this.columnHeaderSize.Width = 81;
            // 
            // columnHeaderFrom
            // 
            this.columnHeaderFrom.Text = "来源";
            this.columnHeaderFrom.Width = 105;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(885, 468);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.listViewResults);
            this.Controls.Add(this.buttonSearch);
            this.Controls.Add(this.textBoxWord);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormMain";
            this.Text = "MagnetX";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.SizeChanged += new System.EventHandler(this.FormMain_SizeChanged);
            this.contextMenuStripResult.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxWord;
        private System.Windows.Forms.Button buttonSearch;
        private System.Windows.Forms.ColumnHeader columnHeaderName;
        private System.Windows.Forms.ColumnHeader columnHeaderSize;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripResult;
        private System.Windows.Forms.ToolStripMenuItem 复制ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem 资源名ToolStripMenuItem;
        private MagnetX.List.ListView listViewResults;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 功能ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 数据源ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem githubToolStripMenuItem;
        private System.Windows.Forms.ColumnHeader columnHeaderFrom;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem 保存纪录ToolStripMenuItem;
    }
}

