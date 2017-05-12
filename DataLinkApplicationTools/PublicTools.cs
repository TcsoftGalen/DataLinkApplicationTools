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
        //IIS 当前版本号
       // private string iis_VersionString;
        //OS 当前内部版本号 6.1 08  6.3 12
        private string os_CurrentVersion;
        //OS产品名称
        private string os_ProductName;
        //计算机网卡信息
        private string net_Info;
        public string sql_Result;
        public string sql_Result1;

        //数据库连接字串
        protected static string SqlConnectionString;
        /// <summary>
        /// 类构造函数,在构造时初始化 ConfigurationManager
        /// 无返回值;
        /// </summary>
        public PublicTools()
        {
            Configuration appSetting = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            SqlConnectionString = appSetting.AppSettings.Settings["connectString"].Value.ToString();         
        }
        /// <summary>
        /// 取得系统信息   是 Svr08 或 Svr12
        /// </summary>
        public string GetSystemInfo()
        {   //regedit系统信息路径
            RegistryKey regKey = Registry.LocalMachine.OpenSubKey(Properties.Resources.REG_OSInfoPath);
            //取得系统名称
            os_ProductName = regKey.GetValue("ProductName").ToString();
            os_CurrentVersion = regKey.GetValue("CurrentVersion").ToString();         
            regKey.Close();

            return "当前操作系统：" + os_ProductName + " " + os_CurrentVersion;
        }


        public void ABC()
        { int i;
            for ( i = 0; i < 500; i++)
            {
                Thread.Sleep(100);
            }
            sql_Result = "1号数据库还原完毕";
            

        }
        public void DEF()
        {
            int j;
            for (j= 0; j < 100; j++)
            {
                Thread.Sleep(10);
            }
            sql_Result1 = "2号数据库还原完毕";

        }

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
        /// 取得SQL连接。
        /// </summary>
        /// <returns></returns>
        private SqlConnection GetSqlConnection()
        {
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
       private  void RestoreDataLinkDB(string DbName,string RestoreString)
        {
            if (File.Exists(@"C:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\"+ DbName + ".mdf"))
            {
                //数据库存在
               sql_Result = "数据库 [ "+ DbName + " ] 已存在！"; ;
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
                    sql_Result = sqlcmd.ExecuteNonQuery().ToString();
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
                sql_Result = "数据库 [ "+ DbName + " ] 还原完毕！"; 
            }

        }    
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
