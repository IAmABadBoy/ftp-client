using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FTP_Winform
{
    class XmlHelper
    {
        //全局文件路径
        static String xmlPath = "ftpData.xml";


        /// <summary>
        /// 创建文件
        /// </summary>
        public static void Create()
        {
            if (!File.Exists(@xmlPath))
            {
                //File.Create() 不会自动关闭，所以要在末尾加上Close()
                File.Create(@xmlPath).Close();

                XmlDocument doc = new XmlDocument();
                XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                doc.AppendChild(dec);

                XmlElement root = doc.CreateElement("AllConnections");
                doc.AppendChild(root);

                doc.Save(@xmlPath);
            }
        }

        /// <summary>
        /// 添加一条记录
        /// </summary>
        /// <param name="ip">所属ip</param>
        /// <param name="nacc">账号</param>
        /// <param name="npass">密码</param>
        public static void AddOneData(String ip, String nacc, String npass)
        {
            //先判断是否存在文件
            Create();

            //装载文件
            XmlDocument doc = new XmlDocument();
            doc.Load(@xmlPath);
            XmlElement root = doc.DocumentElement;
            var nodes = root.GetElementsByTagName("Connection");
            foreach (XmlNode node in nodes)
            {
                if (node.Attributes[1].Value.Equals(ip))  /*如果连接信息存在*/
                {
                    if (!SearchOrUpdate(ip, nacc)) /*如果账号不存在*/
                    {
                        XmlElement temp = doc.CreateElement("Account");
                        temp.SetAttribute("name", nacc);
                        temp.InnerText = npass;
                        node.AppendChild(temp);

                        doc.Save(@xmlPath);
                        return; /*添加了一条记录*/
                    }
                    return; /*信息已经存在*/
                }
            }
            //新增连接、账号信息
            XmlElement newCon = doc.CreateElement("Connection");
            newCon.SetAttribute("id", (nodes.Count + 1).ToString());
            newCon.SetAttribute("name", ip);

            XmlElement temp1 = doc.CreateElement("Account");
            temp1.SetAttribute("name", nacc);
            temp1.InnerText = npass;

            newCon.AppendChild(temp1);
            root.AppendChild(newCon);

            doc.Save(@xmlPath);
        }

        /// <summary>
        /// 查找或者更新数据
        /// </summary>
        /// <param name="ip">所属ip</param>
        /// <param name="acc">账号</param>
        /// <param name="newpass">新密码</param>
        /// <returns></returns>
        public static bool SearchOrUpdate(String ip, String acc = "", String newpass = "")
        {
            //先判断是否存在文件
            Create();

            //装载文件
            XmlDocument doc = new XmlDocument();
            doc.Load(@xmlPath);
            XmlElement root = doc.DocumentElement;
            var nodes = root.GetElementsByTagName("Connection");
            foreach (XmlNode node in nodes)
            {
                if (node.Attributes[1].Value.Equals(ip))
                {
                    if (!acc.Equals(""))
                    {
                        foreach (XmlNode accts in node.ChildNodes)
                        {
                            if (accts.Attributes[0].Value.Equals(acc))
                            {
                                if (!newpass.Equals("")) /*是否更新密码*/
                                {
                                    //更新密码
                                    accts.InnerText = newpass;
                                    doc.Save(@xmlPath);
                                }
                                //查找的账号存在
                                return true;
                            }
                        }
                        //查找的账号不存在
                        return false;
                    }
                    //查找的IP存在
                    return true;
                }
            }
            //查找的IP不存在
            return false;
        }

        /// <summary>
        /// 获取、填充指定连接下的所有账号信息
        /// </summary>
        /// <param name="ip">指定的连接地址</param>
        public static void ReadAllDatas(String ip, ref List<A_PS> datas)
        {
            //先判断是否存在文件
            Create();

            //装载文件
            XmlDocument doc = new XmlDocument();
            doc.Load(@xmlPath);
            XmlElement root = doc.DocumentElement;
            var nodes = root.GetElementsByTagName("Connection");
            if (nodes.Count > 0)
            {
                //外层是所有的连接
                foreach (XmlNode node in nodes)
                {
                    //Console.WriteLine("--->连接名：" + node.Attributes[1].Value);
                    if (node.Attributes[1].Value.Equals(ip))
                    {
                        //先清空
                        datas.Clear();

                        //内层是连接下的所有账号信息
                        foreach (XmlNode accts in node.ChildNodes)
                        {
                            //Console.WriteLine("\t账号：{0}，密码{1}", accts.Attributes[0].Value, accts.InnerText);
                            datas.Add(new A_PS() { ACCT = accts.Attributes[0].Value, PASS = accts.InnerText});
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 获取上次的登陆账户
        /// </summary>
        public static String getLastChoice()
        {
            //先判断是否存在文件
            Create();

            //装载文件
            XmlDocument doc = new XmlDocument();
            doc.Load(@xmlPath);
            XmlElement root = doc.DocumentElement;
            var nodes = root.GetElementsByTagName("LastSelectedIndex");
            String result = "";
            if (nodes.Count > 0)
            {
                String ip = nodes[0].Attributes[0].Value;
                int index = Convert.ToInt32(nodes[0].InnerText);
                result = string.Format("{0};{1}", ip, index);
            }
            return result;
        }

        /// <summary>
        /// 设置最近的登录账户
        /// </summary>
        /// <param name="ip">登录ip</param>
        /// <param name="index">选择的账号索引</param>
        public static void setLastChoice(String ip, int index)
        {
            //先判断是否存在文件
            Create();

            //装载文件
            XmlDocument doc = new XmlDocument();
            doc.Load(@xmlPath);
            XmlElement root = doc.DocumentElement;
            var nodes = root.GetElementsByTagName("LastSelectedIndex");
            if (nodes.Count > 0)
            {
                nodes[0].Attributes[0].Value = ip;
                nodes[0].InnerText = index.ToString();
            }
            else
            {
                XmlElement temp = doc.CreateElement("LastSelectedIndex");
                temp.SetAttribute("IP", ip);
                temp.InnerText = index.ToString();
                root.AppendChild(temp);
            }

            //保存更改
            doc.Save(@xmlPath);
        }

        /// <summary>
        /// 获取文件保存路径
        /// </summary>
        public static String getSavePath()
        {
            //先判断是否存在文件
            Create();

            //装载文件
            XmlDocument doc = new XmlDocument();
            doc.Load(@xmlPath);
            XmlElement root = doc.DocumentElement;
            var nodes = root.GetElementsByTagName("SavePath");
            String result = "";
            if (nodes.Count > 0)
            {
                result = nodes[0].InnerText;
            }
            return result;
        }

        /// <summary>
        /// 设置保存路径
        /// </summary>
        /// <param name="path"></param>
        public static void setSavePath(String path)
        {
            //先判断是否存在文件
            Create();

            //装载文件
            XmlDocument doc = new XmlDocument();
            doc.Load(@xmlPath);
            XmlElement root = doc.DocumentElement;
            var nodes = root.GetElementsByTagName("SavePath");
            if (nodes.Count > 0)
            {
                nodes[0].InnerText = path;
            }
            else
            {
                XmlElement temp = doc.CreateElement("SavePath");               
                temp.InnerText = path;
                root.AppendChild(temp);
            }

            //保存更改
            doc.Save(@xmlPath);
        }
    }
}
