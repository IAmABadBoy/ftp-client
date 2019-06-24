using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
namespace FTP_Winform
{
    public partial class Form1 : Form
    {
        private String user; /*账号*/
        private String pass; /*密码*/
        private ImageList imagelist; /*图标集*/
        private List<DataModel> currentDirList = new List<DataModel>(); /*当前目录下的文件以及文件夹*/
        private List<DataModel> currentFileList = new List<DataModel>();
        private static String RootUrl = ""; /*默认的根目录*/
        private static String LocalSavePath = "./FtpDownLoad"; /*默认的保存路径*/
        private String currentUrl = ""; /*当前路径*/
        private bool USESSL = false; /*是否启用ssl*/
        private int LastSelectedIndex = -1; /*上次选择的用户名-账号组合*/
        private String LastLoginIP = "";

        private Thread downloads; /*下载线程*/
        private Thread uploads; /*上传线程*/

        private delegate void ChangeProgress(int va); /*progressBar的委托*/
        private ChangeProgress changep1, changep2;

        private delegate String GetListView(); /*listview的委托*/
        private GetListView getName, getType;

        private List<A_PS> FtpData; /*账号-密码对模型*/

        private double tick1 = 0;
        private double tick2 = 0;

        private Download download;
        private Upload upload;
        private Recorder recorder;
        private int listmode = -1;
        public Form1()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 窗体加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            //调用初始化
            Initil();
        }

        #region---初始化操作---
        /// <summary>
        /// 初始化
        /// </summary>
        private void Initil()
        {
            //添加图标
            imagelist = new ImageList();
            imagelist.ImageSize = new Size(32, 32);
            imagelist.Images.Add(Properties.Resources.direct);
            imagelist.Images.Add(Properties.Resources.document);
            imagelist.Images.Add(Properties.Resources.download);
            imagelist.Images.Add(Properties.Resources.upload);


            imagelist.ColorDepth = ColorDepth.Depth32Bit;
            listView1.LargeImageList = imagelist;

            //加密方式选择
            comboEncode.SelectedIndex = 1;

            //初始化线程
            downloads = new Thread(DownloadFile);
            uploads = new Thread(UploadFile);

            //初始化委托
            changep1 = new ChangeProgress(ChangeP1);
            changep2 = new ChangeProgress(ChangeP2);

            //listview操作
            getName = new GetListView(GetListName);
            getType = new GetListView(GetListType);

            //
            btnDisCon.Enabled = false;

            //账号密码操作
            FtpData = new List<A_PS>();
            String temp = XmlHelper.getLastChoice();
            if (!temp.Equals(""))
            {
                LastLoginIP = temp.Split(';')[0];
                LastSelectedIndex = Convert.ToInt32(temp.Split(';')[1]);

                XmlHelper.ReadAllDatas(LastLoginIP, ref FtpData);
                if (FtpData.Count > 0)
                {
                    FillCombAcc();
                    setAcctInfo();
                }
            }

            //获取保存路径
            LocalSavePath = XmlHelper.getSavePath();
            if (LocalSavePath.Equals(""))
                LocalSavePath = "./FtpDownLoad";
            label7.Text = LocalSavePath;
            comboEncode.SelectedIndex = 0;
        }

        /// <summary>
        /// 界面部分可否编辑控制
        /// </summary>
        private void EnableOrDis()
        {
            btnDisCon.Enabled = !btnDisCon.Enabled;
            btnCon.Enabled = !btnCon.Enabled;
            comboAcct.Enabled = btnCon.Enabled;
            textURL.Enabled = btnCon.Enabled;
            textPassword.Enabled = btnCon.Enabled;
            comboEncode.Enabled = btnCon.Enabled;
            listView1.Enabled = !btnCon.Enabled;
        }

        #endregion

        #region ---文件浏览器的操作---
        /// <summary>
        /// 获取文件信息
        /// </summary>
        /// <param name="line">读取的一条文件信息</param>
        private void _split(String line)
        {
            line = line.Replace("    ", " ");
            List<String> results = line.Split(' ').ToList();
            //去掉空项
            results.RemoveAll(n => n == "");
            DataModel mod = new DataModel();
            if (results[2].Contains("DIR"))
            {
                mod.Type = 1;
                mod.Name = results[results.Count - 1];
                //mod.Size = Math.Ceiling(double.Parse(re[4]) / 1024);
                currentDirList.Add(mod);
            }
            else
            {
                mod.Type = 0;
                mod.Name = results[results.Count - 1];
                mod.Size = Math.Ceiling(double.Parse(results[2]));
                currentFileList.Add(mod);
            }
        }

        /// <summary>
        /// 向列表添加信息
        /// </summary>
        public void _PrintcurrentFileListAndcurrentDirList()
        {
            listView1.Items.Clear();
            if (!currentUrl.Equals(RootUrl + "/"))
            {
                //此处的”\\..“修改时也要同时修改进入文件夹操作的
                listView1.Items.Add("\\..", 0);
            }
            foreach (DataModel dm in currentDirList)
            {
                ListViewItem itm = new ListViewItem();
                itm.Text = dm.Name;
                itm.SubItems.Add("");
                itm.SubItems.Add("文件夹");
                itm.ImageIndex = 0;
                listView1.Items.Add(itm);
            }
            foreach (DataModel dm in currentFileList)
            {
                ListViewItem itm = new ListViewItem();
                itm.Text = dm.Name;
                itm.SubItems.Add(dm.Size + dm.Unit);
                itm.SubItems.Add("文件");
                itm.ImageIndex = 1;
                listView1.Items.Add(itm);
            }
        }

        /// <summary>
        /// 变换当前路径
        /// </summary>
        /// <param name="str">当前路径</param>
        private void Change_currentPath(String str)
        {
            currentUrl = str;
            currentPath.Text = "当前路径：" + str;
        }

        /// <summary>
        /// 进入文件夹
        /// </summary>
        /// <param name="path">路径名</param>
        private void enterADirectury(String path)
        {
            currentDirList.Clear();
            currentFileList.Clear();
            String url = path;
            FtpWebResponse FWRes = upload.get_reader(url, false, WebRequestMethods.Ftp.ListDirectoryDetails);
            StreamReader reader = new StreamReader(FWRes.GetResponseStream(), Encoding.UTF8);
            String line = reader.ReadLine();
            //更改当前路径
            Change_currentPath(FWRes.ResponseUri.AbsoluteUri);
            while (line != null)
            {
                _split(line);
                line = reader.ReadLine();
            }
            _PrintcurrentFileListAndcurrentDirList();
            reader.Close();
            FWRes.Close();
        }

        #endregion                              

        #region ---窗体界面操作---
        /// <summary>
        /// 打开连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(comboAcct.Text.ToString())
                || string.IsNullOrWhiteSpace(textPassword.Text)
                || string.IsNullOrWhiteSpace(textURL.Text)
                )
            {
                MessageBox.Show("输入完整信息！");
                return;
            }
            try
            {
                String url = "ftp://" + textURL.Text.Trim();
                RootUrl = url;
                user = comboAcct.Text.ToString();
                pass = textPassword.Text.ToString();
                recorder = new Recorder();
                download = new Download(user, pass, 20,recorder);
                upload = new Upload(user, pass, 20,recorder);
                enterADirectury(url);
                EnableOrDis();

                //存储数据
                saveOneData();
                listflash.Enabled = true;
                listflash.Interval = 1000;
                listmode = 0;
            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        /// <summary>
        /// 模拟文件浏览器操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                String name = listView1.Items[listView1.SelectedIndices[0]].SubItems[0].Text;
                if (name.Equals("\\.."))
                {
                    String uu = currentUrl.Substring(0, currentUrl.LastIndexOf('/'));
                    enterADirectury(uu);
                    return;
                }
                String type = listView1.Items[listView1.SelectedIndices[0]].SubItems[2].Text;
                if (type.Trim().Equals("文件夹"))
                {
                    if (currentUrl.ElementAt(currentUrl.Length - 1) == '/')
                    {
                        enterADirectury(currentUrl + name);
                    }
                    else
                    {
                        enterADirectury(currentUrl + "/" + name);
                    }
                    return;
                }
                else
                {
                    DialogResult result = MessageBox.Show("你选中了文件：" + name + "\n是否要下载？", "下载提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);
                    if (result == System.Windows.Forms.DialogResult.Yes)
                    {
                        if (downloads.IsAlive)
                        {
                            MessageBox.Show("请等待当前文件下载完！");
                            return;
                        }
                        else
                        {
                            downloads = new Thread(DownloadFile);
                            downloads.Start();
                        }
                        return;
                    }
                    else
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 假装退出程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                EnableOrDis();
                if (uploads.IsAlive)
                {
                    uploads.Abort();
                }
                if (downloads.IsAlive)
                {
                    downloads.Abort();
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 修改progressBar1的值（下载）
        /// </summary>
        /// <param name="va">要修改的值</param>
        private void ChangeP1(int va)
        {
            progressBar1.Value = va;
            label1.Text = "下载进度：" + va + "%";
        }

        /// <summary>
        /// 修改progressBar2的值(上传)
        /// </summary>
        /// <param name="va">要修改的值</param>
        private void ChangeP2(int va)
        {
            progressBar2.Value = va;
            label6.Text = "上传进度：" + va + "%";
        }

        /// <summary>
        /// 针对ListView1的访问
        /// </summary>
        /// <returns></returns>
        private String GetListName()
        {

              return listView1.Items[listView1.SelectedIndices[0]].SubItems[0].Text;                  
        }

        private String GetListType()
        {
            try
            {
                return listView1.Items[listView1.SelectedIndices[0]].SubItems[2].Text;
            }
            catch (Exception e)
            {
                return "";
            }

        }
        #endregion                  

        #region ---右键菜单操作---        
        /// <summary>
        /// 上传文件的方法
        /// </summary>
        private void UploadFile()
        {
            bool issuccess = true;
            try
            {
                openFileDialog1 = new OpenFileDialog();
                String UpFilePath = "";
                String name = "";
                String realPath;

                //if (!Directory.Exists(@"D:/MyFTP/Download/"))
                //{
                //    //创建路径
                //    Directory.CreateDirectory(@"D:/MyFTP/Download/");
                //}
                //openFileDialog1.InitialDirectory = @"D:/MyFTP/Download/";
                openFileDialog1.Title = "选择要上传的文件";
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    UpFilePath = openFileDialog1.FileName;
                    name = UpFilePath.Substring(UpFilePath.LastIndexOf('\\') + 1);
                    //路径拼接
                    if (currentUrl.ElementAt(currentUrl.Length - 1) == '/')
                        realPath = currentUrl + name;
                    else
                        realPath = currentUrl + "/" + name;

                    upload.start_upload(realPath, USESSL, UpFilePath);

                    //TODO 上传完之前执行了
                    enterADirectury(currentUrl);
                }


            }
            catch(IOException e)
            {
                issuccess = false;
                MessageBox.Show("上传错误"+e.Message);
            }
            finally
            {
                if(issuccess)
                    MessageBox.Show("任务创建成功");
            }
            
        }

        /// <summary>
        /// 下载文件的方法
        /// </summary>
        private void DownloadFile()
        {
            bool issuccess = true;
            try
            {
                String fileName = this.Invoke(getName).ToString();
                String type = this.Invoke(getType).ToString();
                String realPath;
                if (!type.Equals("文件"))
                {
                    MessageBox.Show("请选择要下载的文件！");
                    return;
                }

                //判断是否有文件夹
                if (!Directory.Exists(@LocalSavePath))
                {
                    Directory.CreateDirectory(@LocalSavePath);
                }

                //路径拼接
                if (currentUrl.ElementAt(currentUrl.Length - 1) == '/')
                    realPath = currentUrl + fileName;
                else
                    realPath = currentUrl + "/" + fileName;


                download.start_download(realPath, USESSL, @LocalSavePath + "/" + fileName);
            }
            catch (Exception ex)
            {
                issuccess = false;
                MessageBox.Show("下载失败！\n\t请检查是否有权限在当前目录下执行下载操作！");
            }
            finally
            {
                //清除
                if(issuccess)
                    MessageBox.Show("任务创建成功");
            }
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolUpload_Click(object sender, EventArgs e)
        {
            timer_upload.Enabled = true;
            timer_upload.Interval = 1000;
            UploadFile();
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolDownload_Click(object sender, EventArgs e)
        {
            timer_download.Enabled = true;
            timer_download.Interval = 1000;
            DownloadFile();
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolDelete_Click(object sender, EventArgs e)
        {
            try
            {
                String fileName = this.Invoke(getName).ToString();
                String type = this.Invoke(getType).ToString();
                String realPath;
                //路径拼接
                if (currentUrl.ElementAt(currentUrl.Length - 1) == '/')
                    realPath = currentUrl + fileName;
                else
                    realPath = currentUrl + "/" + fileName;

                upload.deleteFile(realPath, USESSL);

                //刷新当前文件夹
                enterADirectury(currentUrl);
            }
            catch (Exception ex)
            {
                //MessageBox.Show("删除失败！\n\t请检查是否有权限在当前目录下执行删除操作！");
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region ---其他操作---
        

        /// <summary>
        /// 是否启用ssl
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboEncode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboEncode.SelectedIndex == 1)
                USESSL = true;
            else
                USESSL = false;
        }

        /// <summary>
        /// 切换显示方式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click_1(object sender, EventArgs e)
        {
            listView1.View = (listView1.View == View.Details) ? View.LargeIcon : View.Details;
            button1.Text = (listView1.View == View.Details) ? "列表显示" : "图标显示";
        }

        /// <summary>
        /// 设置保存路径
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1 = new FolderBrowserDialog();
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                //MessageBox.Show(folderBrowserDialog1.SelectedPath);
                LocalSavePath = folderBrowserDialog1.SelectedPath.Replace('\\', '/');
                XmlHelper.setSavePath(LocalSavePath);

                label7.Text = LocalSavePath;
            }
        }

        /// <summary>
        /// 计时器下载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_download_Tick(object sender, EventArgs e)
        {
            if (tick1 >= 1024)
            {
                tick1 /= 1024;
                label8.Text = String.Format("当前下载速度：{0:##.#}K/s", tick1);
            }
            if (tick1 >= 1024)
            {
                tick1 /= 1024;
                label8.Text = String.Format("当前下载速度：{0:##.#}M/s", tick1);
            }
            tick1 = 0;
        }

        /// <summary>
        /// 计时器上传
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_upload_Tick(object sender, EventArgs e)
        {
            if (tick2 >= 1024)
            {
                tick2 /= 1024;
                label9.Text = String.Format("当前上传速度：{0:##.#}K/s", tick2);
            }
            if (tick2 >= 1024)
            {
                tick2 /= 1024;
                label9.Text = String.Format("当前上传速度：{0:##.#}M/s", tick2);
            }
            tick2 = 0;
        }

        private void list_flash(object sender,EventArgs e)
        {
            sender = null;
            if (listmode == -1)
            {
                return;
            } else if (listmode == 0)
            {

            }
            else if(listmode == 1)
            {
                downloading_Click(sender, e);
            }
            else if(listmode == 2)
            {
                conplete_Click(sender, e);
            }
        }
        #endregion

        #region ---账号密码操作---
        /// <summary>
        /// 填充数据
        /// </summary>
        private void setAcctInfo()
        {
            textURL.Text = LastLoginIP;
            //触发 comboAcct_SelectedIndexChanged
            comboAcct.SelectedIndex = LastSelectedIndex;
        }

        /// <summary>
        /// 账号下拉列表填充
        /// </summary>
        private void FillCombAcc()
        {
            //清空
            comboAcct.Items.Clear();
            foreach (A_PS ap in FtpData)
            {
                comboAcct.Items.Add(ap.ACCT);
            }
        }

        /// <summary>
        /// 填充密码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboAcct_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboAcct.Text = comboAcct.SelectedItem.ToString();
            textPassword.Text = FtpData[comboAcct.SelectedIndex].PASS;
        }

        private void textURL_TextChanged(object sender, EventArgs e)
        {

        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void downloading_Click(object sender, EventArgs e)
        {
            if (upload == null)
            {
                MessageBox.Show("请先连接");
                return;
            }
            if (listmode != 1)
            {
                listmode = 1;
                listView1.Clear();
                listView1.Columns.Add("id", 60, HorizontalAlignment.Center);
                listView1.Columns.Add("类型", 50, HorizontalAlignment.Center);
                listView1.Columns.Add("已传输大小", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("总大小", 50, HorizontalAlignment.Center);
                listView1.Columns.Add("进度", 50, HorizontalAlignment.Center);
                listView1.Columns.Add("本机路径", 250, HorizontalAlignment.Center);
                listView1.Columns.Add("server路径", 250, HorizontalAlignment.Center);
                foreach (string id in recorder.downtasks_running.Keys)
                {
                    object[] o = recorder.downtasks_running[id];
                    ListViewItem lvi = new ListViewItem();
                    lvi.ImageIndex = 2;
                    lvi.Text = id;
                    lvi.SubItems.Add("下载");
                    for (int i = 0; i < 5; i++)
                        lvi.SubItems.Add(o[i].ToString());

                    listView1.Items.Add(lvi);

                }
                foreach (string id in recorder.uptasks_running.Keys)
                {
                    object[] o = recorder.uptasks_running[id];
                    ListViewItem lvi = new ListViewItem();
                    lvi.ImageIndex = 3;
                    lvi.Text = id;
                    lvi.SubItems.Add("上传");
                    for (int i = 0; i < 5; i++)
                        lvi.SubItems.Add(o[i].ToString());
                    listView1.Items.Add(lvi);

                }
            }
            
            if (false)
            {
                List<string> s1 = new List<string>();//s1为需要修改的id
                List<string> s2 = new List<string>();//s2为需要删去的id
                List<string> s3 = new List<string>();//s3为需要增加的id
                List<string> o1 = new List<string>();
                List<string> o2 = new List<string>();
                for(int i =0; i< listView1.Items.Count; i++)
                {
                    o1.Add(listView1.Items[i].Text);
                }
                foreach(string id in recorder.downtasks_running.Keys)
                {
                    o2.Add(id);
                }
                foreach(string id in recorder.uptasks_running.Keys)
                {
                    o2.Add(id);
                }
                foreach(string id in o1)
                {
                    if (o2.Contains(id))
                    {
                        s1.Add(id);
                    }
                    else
                    {
                        s2.Add(id);
                    }
                }
                foreach(string id in o2)
                {
                    if (o1.Contains(id))
                    {
                        s3.Add(id);
                    }
                }
                foreach(string id in s1)
                {
                    int i = listView1.Items.IndexOfKey(id);
                    if (listView1.Items[i].SubItems[0].Text == "下载")
                    {
                        listView1.Items[i].SubItems[1].Text = recorder.downtasks_running[id][0].ToString() ;
                        listView1.Items[i].SubItems[3].Text = recorder.downtasks_running[id][2].ToString();

                    }
                    else
                    {
                        listView1.Items[i].SubItems[1].Text = recorder.uptasks_running[id][0].ToString();
                        listView1.Items[i].SubItems[3].Text = recorder.uptasks_running[id][2].ToString();
                    }
                                    }
            }
            

        }

        private void serverlist_Click(object sender, EventArgs e)
        {
            listmode = 0;
            if(upload==null)
            {
                MessageBox.Show("请先连接");
                return;
            }
            listView1.Clear();
            listView1.Columns.Add("名称", 250, HorizontalAlignment.Center);
            listView1.Columns.Add("大小", 100, HorizontalAlignment.Center);
            listView1.Columns.Add("类型", 150, HorizontalAlignment.Center);
            enterADirectury(currentUrl);
        }

        private void conplete_Click(object sender, EventArgs e)
        {
            listmode = 2;
            if (upload == null)
            {
                MessageBox.Show("请先连接");
                return;
            }
            listView1.Clear();
            listView1.Columns.Add("id", 60, HorizontalAlignment.Center);
            listView1.Columns.Add("类型", 50, HorizontalAlignment.Center);
            listView1.Columns.Add("总大小", 50, HorizontalAlignment.Center);
            listView1.Columns.Add("进度", 50, HorizontalAlignment.Center);
            listView1.Columns.Add("本机路径", 250, HorizontalAlignment.Center);
            listView1.Columns.Add("server路径", 250, HorizontalAlignment.Center);
            if (upload == null)
            {
                MessageBox.Show("请先连接");
                return;
            }
            foreach (string id in recorder.downtasks_complete.Keys)
            {
                object[] o = recorder.downtasks_complete[id];
                ListViewItem lvi = new ListViewItem();
                lvi.ImageIndex = 2;
                lvi.Text = id;
                lvi.SubItems.Add("下载");
                o[2] = 100;
                for (int i = 1; i < 5; i++)
                    lvi.SubItems.Add(o[i].ToString());
                listView1.Items.Add(lvi);
            }
            foreach (string id in recorder.uptasks_complete.Keys)
            {
                object[] o = recorder.uptasks_complete[id];
                ListViewItem lvi = new ListViewItem();
                lvi.ImageIndex = 3;
                lvi.Text = id;
                lvi.SubItems.Add("上传");
                o[2] = 100;
                for (int i = 1; i < 5; i++)
                    lvi.SubItems.Add(o[i].ToString());
                listView1.Items.Add(lvi);

            }
        }

        /// <summary>
        /// 存储一条数据
        /// </summary>
        private void saveOneData()
        {
            A_PS temp = new A_PS() { ACCT = comboAcct.Text, PASS = textPassword.Text };
            XmlHelper.AddOneData(textURL.Text, temp.ACCT, temp.PASS);

            if (textURL.Text.Equals(LastLoginIP))
            {
                if (!FtpData.Contains(temp))
                {
                    comboAcct.Items.Add(temp.ACCT);
                    FtpData.Add(temp);
                }
            }
            else
            {
                XmlHelper.ReadAllDatas(textURL.Text, ref FtpData);
                FillCombAcc();
                LastSelectedIndex = FtpData.Count - 1;
                LastLoginIP = textURL.Text;
                XmlHelper.setLastChoice(LastLoginIP, LastSelectedIndex);

                //填充数据
                //setAcctInfo();
            }
        }

        /// <summary>
        /// 输入完后自动填充密码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textURL_Leave(object sender, EventArgs e)
        {
            XmlHelper.ReadAllDatas(textURL.Text, ref FtpData);
            if (FtpData.Count > 0)
            {
                LastLoginIP = textURL.Text;
                FillCombAcc();
                setAcctInfo();
            }
        }
        #endregion

    }

    public class baseclass
    {
        private String user;
        private String pass;
        public baseclass(String user,String pass){
            this.user = user;
            this.pass = pass;
        }
        public static bool ValidateServerCertificate
         (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        public FtpWebResponse get_reader(String realPath,bool USESSL,String mode)
        {
            FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(@realPath);
            ftpRequest.UseBinary = true;
            ftpRequest.Timeout = 5000;
            ftpRequest.Credentials = new NetworkCredential(user, pass);
            if (USESSL)
            {
                ftpRequest.EnableSsl = true;
                ServicePointManager.ServerCertificateValidationCallback =
                    new RemoteCertificateValidationCallback(ValidateServerCertificate);
            }
            ftpRequest.Method = mode;
            FtpWebResponse ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
            return ftpResponse;
        }

        public FtpWebRequest get_request(String realPath, bool USESSL, String mode)
        {
            FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(@realPath);
            ftpRequest.UseBinary = true;
            ftpRequest.Timeout = 5000;
            ftpRequest.Credentials = new NetworkCredential(user, pass);
            if (USESSL)
            {
                ftpRequest.EnableSsl = true;
                ServicePointManager.ServerCertificateValidationCallback =
                    new RemoteCertificateValidationCallback(ValidateServerCertificate);
            }
            ftpRequest.Method = mode;
            return ftpRequest;
        }

        /// <summary>
        /// 获取文件大小
        /// </summary>
        /// <param name="str">文件路径</param>
        /// <returns></returns>
        public double GetFileSize(String str,bool USESSL)
        {

            FtpWebResponse res = get_reader(str, USESSL, WebRequestMethods.Ftp.GetFileSize);
            double length = res.ContentLength;
            res.Close();
            res.Dispose();
            return length;
        }

        public void deleteFile(String realPath,bool USESSL)
        {
            FtpWebResponse f = get_reader(realPath, USESSL, WebRequestMethods.Ftp.DeleteFile);
            f.Close();
            f.Dispose();
        }
    }

    /// <summary>
    /// 负责下载操作的类
    /// </summary>
    public class Download:baseclass
    {
        private int totalnum;
        private int maxthread;
        private int id = 100000;
        String _id;
        String realPath;
        bool USESSL;
        String targetPath;
        double total;
        public Dictionary<string, object[]> list;
        private Dictionary<string,Thread> downloads;
        private Recorder recorder;
        #region --初始化函数--
        /// <summary>
        /// 函数初始化操作。
        /// </summary>
        public Download(String user, String pass,int maxthread,Recorder recorder):base(user,pass)
        {
            totalnum = 0;
            this.maxthread = maxthread;
            list = new Dictionary<string, object[]>();
            downloads = new Dictionary<string, Thread>();
            this.recorder = recorder;
        }
        #endregion

        #region --下载函数--
        /// <summary>
        /// 下载操作
        /// 返回值为 下载id 文件大小
        /// </summary>
        /// <returns></returns>
        public Object[] start_download(String realPath,bool USESSL,String targetPath)
        {
            if (totalnum >= maxthread)
            {
                Object[] res = { false, false };
                return res;
            }
            Object[] info = new Object[2];
            info[0] = (object)id.ToString();
            id++;
            _id = info[0].ToString();
            info[1] = (object)GetFileSize(realPath, USESSL);
            object [] v = { 0, (double)info[1],realPath,targetPath };
            list.Add(info[0].ToString(),v);
            this.USESSL = USESSL;
            this.realPath = realPath;
            this.targetPath = targetPath;
            Thread temp = new Thread(this._download);
            downloads.Add(info[0].ToString(), temp);
            temp.Start();
            recorder.down_task_upload(info[0].ToString(),list);
            totalnum++;
            return info;
        }

        private void _download()
        {
            try
            {
                String idd = _id;
                FtpWebResponse response = get_reader(realPath, USESSL, WebRequestMethods.Ftp.DownloadFile);
                Stream reader = response.GetResponseStream();
                FileStream output = new FileStream(targetPath, FileMode.OpenOrCreate);
                byte[] buffer = new byte[2048];
                int leng = 0;
                double current = 0;
                //开始计时*************************************************

                while ((leng = reader.Read(buffer, 0, buffer.Length)) > 0)
                {
                    current += leng;
                    //*****************************************************
                    output.Write(buffer, 0, leng);
                    double real = (current / total) * 100;
                    int prog = (int)Math.Ceiling(real);
                    list[idd][0] = current;
                    //不加这句，label将会来不及显示
                    System.Windows.Forms.Application.DoEvents();
                }
                output.Close();
                output.Dispose();
                reader.Close();
                reader.Dispose();
                response.Close();
                response.Dispose();
                totalnum--;
            }catch(Exception e)
            {
                MessageBox.Show("下载出错。"+e.Message);
            }
            
        }
        #endregion
    }

    /// <summary>
    /// 负责上传操作的类
    /// </summary>
    public class Upload:baseclass
    {
        private int totalnum;
        private int maxthread;
        private int id = 100000;
        String _id;
        String realPath;
        bool USESSL;
        String UpFilePath;
        double total;
        public Dictionary<string, object[]> list;
        private Dictionary<string, Thread> uploads;
        private Recorder recorder;
        #region --初始化函数--
        public Upload(String user, String pass, int maxthread,Recorder recorder):base(user,pass)
        {
            totalnum = 0;
            this.maxthread = maxthread;
            list = new Dictionary<string, object[]>();
            uploads = new Dictionary<string, Thread>();
            this.recorder = recorder;
        }
        #endregion

        #region --上传函数--
        public Object[] start_upload(String realPath, bool USESSL, String UpFilePath)
        {
            if (totalnum >= maxthread)
            {
                Object[] res = { false, false };
                return res;
            }
            Object[] info = new Object[2];
            info[0] = (object)id.ToString();
            id++;
            _id = info[0].ToString();
            FileInfo fileInfo = new FileInfo(@UpFilePath);
            info[1] = (object) fileInfo.Length;
            object[] v = { 0, Double.Parse(info[1].ToString()),realPath,UpFilePath };
            list.Add(info[0].ToString(), v);
            this.USESSL = USESSL;
            this.realPath = realPath;
            this.UpFilePath = UpFilePath;
            Thread temp = new Thread(this._upload);
            uploads.Add(info[0].ToString(), temp);
            temp.Start();
            recorder.up_task_upload(_id, list);
            totalnum++;
            return info;
        }
        public void _upload()
        {
           
                String idd = _id;
                FtpWebRequest response = get_request(realPath, USESSL, WebRequestMethods.Ftp.UploadFile);
                double current = 0;
                Stream writer = response.GetRequestStream();
                FileStream input = new FileStream(@UpFilePath, FileMode.Open);
                byte[] buffer = new byte[2048];
                int leng = 0;
                //**************************************************

                while ((leng = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    current += leng;

                    //*******************************
                    writer.Write(buffer, 0, leng);
                    double real = (current / total) * 100;
                    int prog = (int)Math.Ceiling(real);
                    list[idd][0] = current;
                    System.Windows.Forms.Application.DoEvents();
                }
                input.Close();
                input.Dispose();
                writer.Close();
                writer.Dispose();
                totalnum--;
           
            

        }
        #endregion
    }

    /// <summary>
    /// 负责记录的类
    /// </summary>
    public class Recorder {
        public Dictionary<string, object[]> uptasks_running;
        public Dictionary<string, object[]> uptasks_complete;
        public Dictionary<string, object[]> downtasks_running;
        public Dictionary<string, object[]> downtasks_complete;
        public String id;
        public int mode;
        public Dictionary<string, object[]> dict;

        public Recorder()
        {
            uptasks_running = new Dictionary<string, object[]>();
            uptasks_complete = new Dictionary<string, object[]>();
            downtasks_running = new Dictionary<string, object[]>();
            downtasks_complete = new Dictionary<string, object[]>();
        }

        public void down_task_upload(String id, Dictionary<String, object[]> dict)
        {
            this.id = id;
            this.mode = 2;
            this.dict = dict;
            Thread t = new Thread(get_info);
            t.Start();
        }
        public void up_task_upload(String id, Dictionary<String, object[]> dict)
        {
            this.id = id;
            this.mode = 1;
            this.dict = dict;
            Thread t = new Thread(get_info);
            t.Start();
        }

        public void get_info()
        {
            String _id = id;
            Dictionary<String, object[]> _dict = dict;
            int _mode = mode;
            object[] temp = new object[5];
            if(_mode == 1)
            {
                temp[0] = _dict[_id][0];
                temp[1] = _dict[_id][1];
                temp[2] = double.Parse(_dict[_id][0].ToString()) * 100 / double.Parse( _dict[_id][1].ToString());
                temp[3] = _dict[_id][2];
                temp[4] = _dict[_id][3];
                uptasks_running.Add(_id,temp);
                while (true)
                {
                    if (double.Parse(_dict[_id][0].ToString()) >= double.Parse(_dict[_id][1].ToString()))
                    {
                        uptasks_complete.Add(_id, temp);
                        uptasks_running.Remove(_id);
                        return;
                    }
                    else
                    {
                        uptasks_running[id][0] = _dict[_id][0]; 
                        uptasks_running[_id][2] = double.Parse(_dict[_id][0].ToString()) * 100 / double.Parse(_dict[_id][1].ToString());
                    }
                    Thread.Sleep(1000);
                }
            }
            else if(_mode ==2)
            {
                temp[0] = _dict[_id][0];
                temp[1] = _dict[_id][1];
                temp[2] = double.Parse(_dict[_id][0].ToString()) * 100 / double.Parse(_dict[_id][1].ToString());
                temp[3] = _dict[_id][2];
                temp[4] = _dict[_id][3];
                downtasks_running.Add(_id, temp);
                while (true)
                {
                    if (double.Parse(_dict[_id][0].ToString()) >= double.Parse(_dict[_id][1].ToString()))
                    {
                        downtasks_complete.Add(_id, downtasks_running[_id]);
                        downtasks_running.Remove(_id);
                        return;
                    }
                    else
                    {
                        downtasks_running[_id][0] = _dict[_id][0];
                        downtasks_running[_id][2] = double.Parse(_dict[_id][0].ToString()) * 100 / double.Parse(_dict[_id][1].ToString());
                    }
                    Thread.Sleep(1000);
                }
            }

        }
    }
}
