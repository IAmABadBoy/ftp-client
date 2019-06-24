using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTP_Winform
{    
    /// <summary>
    /// 文件-文件夹模型
    /// </summary>
    /// Type：文件夹 -- 1；文件 -- 0
    class DataModel
    {
        public int Type { set; get; }
        public String Name { set; get; }
        public String createTime { set; get; }
        public double size;
        public String Unit { set; get; } 

        public DataModel()
        {
            Name = createTime = "";
            Type = -1;
            Size = -1.0;
            Unit = "B";
        }

        /// <summary>
        /// 设置文件大小并调整合适的单位
        /// </summary>
        public double Size
        {
            set
            {
                size = value;
                if (size >= 1024 && Unit.Equals("B"))
                {
                    size = Math.Ceiling(size / 1024);
                    Unit = "KB";
                }
                if (size >= 1024 && Unit.Equals("KB"))
                {
                    size = Math.Ceiling(size / 1024);
                    Unit = "MB";
                }
                if (size >= 1024 && Unit.Equals("MB"))
                {
                    size = Math.Ceiling(size / 1024);
                    Unit = "GB";
                }
                
            }
            get { return size; }
        }
    }

    /// <summary>
    /// 密码-账号模型
    /// </summary>
    class A_PS
    {
        public String ACCT { set; get; }
        public String PASS { set; get; }
        public String LogTime { set; get; }

        /// <summary>
        /// 重写了Equals方法
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            A_PS temp = (A_PS)obj;
            return (this.ACCT.Equals(temp.ACCT));
        }
    }
}
