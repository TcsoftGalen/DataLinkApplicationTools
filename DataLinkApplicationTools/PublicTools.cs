using System;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.Win32;
using System.Net.NetworkInformation;
using System.IO;
using System.Threading;

namespace DataLinkApplicationTools
{
    public class PublicTools
    {

        //OS产品名称
        private string os_ProductName;
        //OS 当前内部版本号 6.1 08  6.3 12
        private string os_CurrentVersion;
       
        //计算机网卡信息
        private string net_Info;

        //测试用
        public string sql_Result;
        public string sql_Result1;

        //数据库连接字串
        protected static string SqlConnectionString;
        /// <summary>
        /// 类构造函数,在构造时初始化 ConfigurationManager
        /// 并取得数据库连接字符。
        /// 无返回值;
        /// </summary>
        public PublicTools()
        {
            Configuration appSetting = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            SqlConnectionString = appSetting.AppSettings.Settings["connectString"].Value.ToString();         
        }
        /// <summary>
        /// 取得操作系统信息，并在Main中显示。
        /// 用于未来对Server 2012 的支持。
        /// 目前阶段没有对版本进行处理。
        /// </summary>
        public string GetSystemInfo()
        {   //regedit系统信息路径
            RegistryKey regKey = Registry.LocalMachine.OpenSubKey(Properties.Resources.REG_OSInfoPath);
            //取得系统名称
            os_ProductName = regKey.GetValue("ProductName").ToString();
            //取得内部版本号  08 6.1   12 6.3
            os_CurrentVersion = regKey.GetValue("CurrentVersion").ToString();         
            regKey.Close();
            return "当前操作系统：" + os_ProductName + " " + os_CurrentVersion;
        }

        /// <summary>
        /// 线程测试
        /// </summary>
        public void ABC()
        { int i;
            for ( i = 0; i < 500; i++)
            {
                Thread.Sleep(100);
            }
            sql_Result = "1号数据库还原完毕";
            

        }
        /// <summary>
        /// 线程测试。
        /// </summary>
        public void DEF()
        {
            int j;
            for (j= 0; j < 100; j++)
            {
                Thread.Sleep(10);
            }
            sql_Result1 = "2号数据库还原完毕";

        }
        /// <summary>
        /// 取得计算机网络连接信息。
        /// 待整理。
        /// </summary>
        private void GetNetWorkInfo()
        {
            NetworkInterface[] ins = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in ins)
            {
                if (adapter.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                {
                    IPInterfaceProperties ip = adapter.GetIPProperties();
                    UnicastIPAddressInformationCollection ipcon = ip.UnicastAddresses;
                    foreach (UnicastIPAddressInformation ipadd in ipcon)
                    {
                        if (ipadd.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            string c = adapter.OperationalStatus.ToString();
                            if (c == "Up") { c = "启用"; } else { c = "禁用"; }
                            net_Info = "网卡名称：" + adapter.Name.ToString() + "\n网络地址：" + ipadd.Address.ToString() + "\n连接状态：" + c;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 取得SqlConnection对象。
        /// </summary>
        /// <returns>返回已个SqlConnection对象，该连接未Open，在引用是需要手动Open</returns>
        private SqlConnection GetSqlConnection()
        {
            //创建连接对象
            SqlConnection sqlConnection;
            if (null != SqlConnectionString)
            {
                sqlConnection = new SqlConnection(SqlConnectionString);
            }
            else
            {
                sqlConnection = null;
            }
            return sqlConnection;
        }

        /// <summary>
        /// 根据数据库代码来还原数据库。
        /// 此方法将在Main中的Thread调用。
        /// 每次调用该方法时将对sql_Result的值进行清空，以防在线程执行完毕后反馈UI信息有误。
        /// </summary>
        /// <param name="DataBase">待还原的数据库代码 1=DL，2=CDB ，3=Log ，4=QC</param>
        public void SwitchDataBase(int DataBase)
        {
            sql_Result = "";
            switch (DataBase)
            {
                case 1:
                    RestoreDataLinkDB("DataLink", Properties.Resources.RESROTE_DATALINK);
                    break;
                case 2:
                    RestoreDataLinkDB("DataLinkCDB", Properties.Resources.RESROTE_DATALINKCDB);
                    break;
                case 3:
                    RestoreDataLinkDB("DataLinkLog", Properties.Resources.RESROTE_DATALINKLOG);
                    break;
                case 4:
                    RestoreDataLinkDB("DataLinkQC", Properties.Resources.RESROTE_DATALINKQC);
                    break;
            }
        }
        /// <summary>
        /// 执行还原数据库
        /// </summary>
        /// <param name="DataBaseName">待还原的数据库名称</param>
        /// <param name="RestoreString">还原数据的脚本语句，此语句已固话在Resoures的引用中</param>
       private  void RestoreDataLinkDB(string DataBaseName,string RestoreString)
        {
            if (File.Exists(@"C:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\"+ DataBaseName + ".mdf"))
            {
                //数据库存在
               sql_Result = "数据库 [ "+ DataBaseName + " ] 已存在！"; ;
            }
            else
            {
                //数据库不存在
                SqlConnection sqlcon = this.GetSqlConnection();
                sqlcon.Open();
                try
                {
                    SqlCommand sqlcmd = new SqlCommand()
                    {
                        Connection = sqlcon,
                        CommandText = RestoreString
                    };
                    if (sqlcmd.ExecuteNonQuery() == -1)
                    {
                        sql_Result = "数据库 [ " + DataBaseName + " ] 还原完毕！";
                    }

                    sqlcmd.Dispose();
                }
                catch (SqlException ex)
                {
                    sql_Result = ex.ToString();
                }
                finally
                {
                    sqlcon.Close();
                    sqlcon.Dispose();
                }
                
            }

        }    
        /// <summary>
        /// 创建DataLink程序所对应的文件夹。
        /// 并移动相关文件到正确的位置。
        /// 施工ing
        /// </summary>
        public void CreateDir()
        {
            if (!Directory.Exists(@"D:\DataLink")) { Directory.CreateDirectory(@"D:\DataLink"); }
            if (!Directory.Exists(@"D:\Backup")) { Directory.CreateDirectory(@"D:\Backup"); }
            if (!Directory.Exists(@"D:\Update")) { Directory.CreateDirectory(@"D:\Update"); }
            Directory.CreateDirectory(@"D:\Backup\DbBack");
            Directory.CreateDirectory(@"D:\Backup\DbBack\1Monday");
            Directory.CreateDirectory(@"D:\Backup\DbBack\2Tuesday");
            Directory.CreateDirectory(@"D:\Backup\DbBack\3Wednesday");
            Directory.CreateDirectory(@"D:\Backup\DbBack\4Thursday");
            Directory.CreateDirectory(@"D:\Backup\DbBack\5Friday");
            Directory.CreateDirectory(@"D:\Backup\DbBack\6Saturday");
            Directory.CreateDirectory(@"D:\Backup\DbBack\7Sunday ");


        }
    }
}
