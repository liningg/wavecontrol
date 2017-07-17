namespace WaveControl
{
    partial class UserControl1
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

        #region 组件设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.添加标注ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.清除记录ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // hScrollBar1
            // 
            this.hScrollBar1.Location = new System.Drawing.Point(0, 323);
            this.hScrollBar1.Name = "hScrollBar1";
            this.hScrollBar1.Size = new System.Drawing.Size(560, 17);
            this.hScrollBar1.SmallChange = 5;
            this.hScrollBar1.TabIndex = 0;
            this.hScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBar1_Scroll);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.添加标注ToolStripMenuItem,
            this.清除记录ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(153, 70);
            // 
            // 添加标注ToolStripMenuItem
            // 
            this.添加标注ToolStripMenuItem.Name = "添加标注ToolStripMenuItem";
            this.添加标注ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.添加标注ToolStripMenuItem.Text = "添加标注";
            this.添加标注ToolStripMenuItem.Click += new System.EventHandler(this.添加标注ToolStripMenuItem_Click);
            // 
            // 清除记录ToolStripMenuItem
            // 
            this.清除记录ToolStripMenuItem.Name = "清除记录ToolStripMenuItem";
            this.清除记录ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.清除记录ToolStripMenuItem.Text = "清除记录";
            this.清除记录ToolStripMenuItem.Click += new System.EventHandler(this.清除记录ToolStripMenuItem_Click);
            // 
            // UserControl1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.hScrollBar1);
            this.Name = "UserControl1";
            this.Size = new System.Drawing.Size(560, 340);
            this.Load += new System.EventHandler(this.UserControl1_Load);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.UserControl1_MouseClick);
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.UserControl1_MouseDoubleClick);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.HScrollBar hScrollBar1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 添加标注ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 清除记录ToolStripMenuItem;
    }
}
