namespace FTP_Winform
{
    partial class Form1
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.btnCon = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.listView1 = new System.Windows.Forms.ListView();
            this.header1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.header2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolUpload = new System.Windows.Forms.ToolStripMenuItem();
            this.toolDownload = new System.Windows.Forms.ToolStripMenuItem();
            this.toolDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.btnDisCon = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.currentPath = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.serverlist = new System.Windows.Forms.Button();
            this.downloading = new System.Windows.Forms.Button();
            this.conplete = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.progressBar2 = new System.Windows.Forms.ProgressBar();
            this.label6 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textURL = new System.Windows.Forms.TextBox();
            this.textPassword = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.comboEncode = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.comboAcct = new System.Windows.Forms.ComboBox();
            this.button2 = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.label7 = new System.Windows.Forms.Label();
            this.timer_download = new System.Windows.Forms.Timer(this.components);
            this.timer_upload = new System.Windows.Forms.Timer(this.components);
            this.listflash = new System.Windows.Forms.Timer(this.components);
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCon
            // 
            this.btnCon.Location = new System.Drawing.Point(262, 3);
            this.btnCon.Margin = new System.Windows.Forms.Padding(2);
            this.btnCon.Name = "btnCon";
            this.btnCon.Size = new System.Drawing.Size(80, 26);
            this.btnCon.TabIndex = 1;
            this.btnCon.Text = "连接";
            this.btnCon.UseVisualStyleBackColor = true;
            this.btnCon.Click += new System.EventHandler(this.button1_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(2, 22);
            this.progressBar1.Margin = new System.Windows.Forms.Padding(2);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(599, 18);
            this.progressBar1.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 7);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 7;
            this.label1.Text = "下载进度：";
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.header1,
            this.header2,
            this.columnHeader1});
            this.listView1.ContextMenuStrip = this.contextMenuStrip1;
            this.listView1.FullRowSelect = true;
            this.listView1.Location = new System.Drawing.Point(108, 2);
            this.listView1.Margin = new System.Windows.Forms.Padding(2);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(494, 257);
            this.listView1.TabIndex = 8;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            this.listView1.DoubleClick += new System.EventHandler(this.listView1_DoubleClick);
            // 
            // header1
            // 
            this.header1.Text = "名称";
            this.header1.Width = 150;
            // 
            // header2
            // 
            this.header2.Text = "大小";
            this.header2.Width = 167;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "类型";
            this.columnHeader1.Width = 202;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolUpload,
            this.toolDownload,
            this.toolDelete});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(105, 82);
            // 
            // toolUpload
            // 
            this.toolUpload.Image = global::FTP_Winform.Properties.Resources.upload;
            this.toolUpload.Name = "toolUpload";
            this.toolUpload.Size = new System.Drawing.Size(104, 26);
            this.toolUpload.Text = "上传";
            this.toolUpload.Click += new System.EventHandler(this.toolUpload_Click);
            // 
            // toolDownload
            // 
            this.toolDownload.Image = global::FTP_Winform.Properties.Resources.download;
            this.toolDownload.Name = "toolDownload";
            this.toolDownload.Size = new System.Drawing.Size(104, 26);
            this.toolDownload.Text = "下载";
            this.toolDownload.Click += new System.EventHandler(this.toolDownload_Click);
            // 
            // toolDelete
            // 
            this.toolDelete.Image = global::FTP_Winform.Properties.Resources.delete;
            this.toolDelete.Name = "toolDelete";
            this.toolDelete.Size = new System.Drawing.Size(104, 26);
            this.toolDelete.Text = "删除";
            this.toolDelete.Click += new System.EventHandler(this.toolDelete_Click);
            // 
            // btnDisCon
            // 
            this.btnDisCon.Location = new System.Drawing.Point(364, 3);
            this.btnDisCon.Margin = new System.Windows.Forms.Padding(2);
            this.btnDisCon.Name = "btnDisCon";
            this.btnDisCon.Size = new System.Drawing.Size(79, 26);
            this.btnDisCon.TabIndex = 9;
            this.btnDisCon.Text = "断开";
            this.btnDisCon.UseVisualStyleBackColor = true;
            this.btnDisCon.Click += new System.EventHandler(this.button4_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // currentPath
            // 
            this.currentPath.AutoSize = true;
            this.currentPath.Location = new System.Drawing.Point(4, 129);
            this.currentPath.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.currentPath.Name = "currentPath";
            this.currentPath.Size = new System.Drawing.Size(65, 12);
            this.currentPath.TabIndex = 10;
            this.currentPath.Text = "当前路径：";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(2, 150);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(2);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.flowLayoutPanel1);
            this.splitContainer1.Panel1.Controls.Add(this.listView1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.label9);
            this.splitContainer1.Panel2.Controls.Add(this.label8);
            this.splitContainer1.Panel2.Controls.Add(this.progressBar2);
            this.splitContainer1.Panel2.Controls.Add(this.label6);
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            this.splitContainer1.Panel2.Controls.Add(this.progressBar1);
            this.splitContainer1.Size = new System.Drawing.Size(604, 354);
            this.splitContainer1.SplitterDistance = 261;
            this.splitContainer1.SplitterWidth = 3;
            this.splitContainer1.TabIndex = 11;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.serverlist);
            this.flowLayoutPanel1.Controls.Add(this.downloading);
            this.flowLayoutPanel1.Controls.Add(this.conplete);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(103, 255);
            this.flowLayoutPanel1.TabIndex = 9;
            this.flowLayoutPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.flowLayoutPanel1_Paint);
            // 
            // serverlist
            // 
            this.serverlist.Location = new System.Drawing.Point(0, 0);
            this.serverlist.Margin = new System.Windows.Forms.Padding(0);
            this.serverlist.Name = "serverlist";
            this.serverlist.Size = new System.Drawing.Size(100, 38);
            this.serverlist.TabIndex = 0;
            this.serverlist.Text = "服务器文件列表";
            this.serverlist.UseVisualStyleBackColor = true;
            this.serverlist.Click += new System.EventHandler(this.serverlist_Click);
            // 
            // downloading
            // 
            this.downloading.Location = new System.Drawing.Point(0, 38);
            this.downloading.Margin = new System.Windows.Forms.Padding(0);
            this.downloading.Name = "downloading";
            this.downloading.Size = new System.Drawing.Size(100, 38);
            this.downloading.TabIndex = 1;
            this.downloading.Text = "正在下载";
            this.downloading.UseVisualStyleBackColor = true;
            this.downloading.Click += new System.EventHandler(this.downloading_Click);
            // 
            // conplete
            // 
            this.conplete.Location = new System.Drawing.Point(0, 76);
            this.conplete.Margin = new System.Windows.Forms.Padding(0);
            this.conplete.Name = "conplete";
            this.conplete.Size = new System.Drawing.Size(100, 38);
            this.conplete.TabIndex = 2;
            this.conplete.Text = "下载完成";
            this.conplete.UseVisualStyleBackColor = true;
            this.conplete.Click += new System.EventHandler(this.conplete_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(402, 50);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(89, 12);
            this.label9.TabIndex = 25;
            this.label9.Text = "当前上传速度：";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(400, 7);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(89, 12);
            this.label8.TabIndex = 24;
            this.label8.Text = "当前下载速度：";
            // 
            // progressBar2
            // 
            this.progressBar2.Location = new System.Drawing.Point(2, 66);
            this.progressBar2.Margin = new System.Windows.Forms.Padding(2);
            this.progressBar2.Name = "progressBar2";
            this.progressBar2.Size = new System.Drawing.Size(599, 18);
            this.progressBar2.TabIndex = 9;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(0, 51);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 8;
            this.label6.Text = "上传进度：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 47);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 12;
            this.label2.Text = "用户名";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 82);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 13;
            this.label3.Text = "密  码";
            // 
            // textURL
            // 
            this.textURL.Location = new System.Drawing.Point(64, 18);
            this.textURL.Margin = new System.Windows.Forms.Padding(2);
            this.textURL.Name = "textURL";
            this.textURL.Size = new System.Drawing.Size(182, 21);
            this.textURL.TabIndex = 14;
            this.textURL.Text = "100.64.238.118:21";
            this.textURL.TextChanged += new System.EventHandler(this.textURL_TextChanged);
            this.textURL.Leave += new System.EventHandler(this.textURL_Leave);
            // 
            // textPassword
            // 
            this.textPassword.Location = new System.Drawing.Point(64, 79);
            this.textPassword.Margin = new System.Windows.Forms.Padding(2);
            this.textPassword.Name = "textPassword";
            this.textPassword.Size = new System.Drawing.Size(182, 21);
            this.textPassword.TabIndex = 15;
            this.textPassword.Text = "123";
            this.textPassword.UseSystemPasswordChar = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 18);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 16;
            this.label4.Text = "地  址";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(276, 50);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 17;
            this.label5.Text = "加密方式";
            // 
            // comboEncode
            // 
            this.comboEncode.FormattingEnabled = true;
            this.comboEncode.Items.AddRange(new object[] {
            "无",
            "TSL/SSL加密"});
            this.comboEncode.Location = new System.Drawing.Point(331, 46);
            this.comboEncode.Margin = new System.Windows.Forms.Padding(2);
            this.comboEncode.Name = "comboEncode";
            this.comboEncode.Size = new System.Drawing.Size(121, 20);
            this.comboEncode.TabIndex = 18;
            this.comboEncode.Tag = "0";
            this.comboEncode.SelectedIndexChanged += new System.EventHandler(this.comboEncode_SelectedIndexChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(510, 119);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(87, 26);
            this.button1.TabIndex = 20;
            this.button1.Text = "图标显示";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // comboAcct
            // 
            this.comboAcct.FormattingEnabled = true;
            this.comboAcct.Location = new System.Drawing.Point(64, 47);
            this.comboAcct.Margin = new System.Windows.Forms.Padding(2);
            this.comboAcct.Name = "comboAcct";
            this.comboAcct.Size = new System.Drawing.Size(182, 20);
            this.comboAcct.TabIndex = 21;
            this.comboAcct.Text = "admin";
            this.comboAcct.SelectedIndexChanged += new System.EventHandler(this.comboAcct_SelectedIndexChanged);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(461, 3);
            this.button2.Margin = new System.Windows.Forms.Padding(2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(87, 28);
            this.button2.TabIndex = 22;
            this.button2.Text = "更改保存路径";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label7
            // 
            this.label7.AutoEllipsis = true;
            this.label7.AutoSize = true;
            this.label7.Enabled = false;
            this.label7.Location = new System.Drawing.Point(459, 46);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.MaximumSize = new System.Drawing.Size(146, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 12);
            this.label7.TabIndex = 23;
            this.label7.Text = "label7";
            // 
            // timer_download
            // 
            this.timer_download.Tick += new System.EventHandler(this.timer_download_Tick);
            // 
            // timer_upload
            // 
            this.timer_upload.Tick += new System.EventHandler(this.timer_upload_Tick);
            //
            this.listflash.Tick += new System.EventHandler(this.list_flash);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(606, 522);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.comboAcct);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.comboEncode);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textPassword);
            this.Controls.Add(this.textURL);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.currentPath);
            this.Controls.Add(this.btnDisCon);
            this.Controls.Add(this.btnCon);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "SimpleFTPClient";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCon;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader header1;
        private System.Windows.Forms.ColumnHeader header2;
        private System.Windows.Forms.Button btnDisCon;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolDownload;
        private System.Windows.Forms.ToolStripMenuItem toolDelete;
        private System.Windows.Forms.ToolStripMenuItem toolUpload;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label currentPath;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textURL;
        private System.Windows.Forms.TextBox textPassword;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboEncode;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ProgressBar progressBar2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox comboAcct;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Timer timer_download;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Timer timer_upload;
        private System.Windows.Forms.Timer listflash;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button serverlist;
        private System.Windows.Forms.Button downloading;
        private System.Windows.Forms.Button conplete;
    }
}

