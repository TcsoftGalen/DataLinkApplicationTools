using System;
using System.Data.SqlClient;
//using MySql.Data.MySqlClient;
using System.DirectoryServices;
using Microsoft.Win32;
using System.Net.NetworkInformation;


namespace DataLinkApplicationTools
{
    class ComputerInformation
    {
        //IIS 程序全称
        private string iis_ProductString;
        //IIS 当前版本号
        private string iis_VersionString;
        //OS 当前内部版本号 6.1 08  6.3 12
        private string os_CurrentVersion;
        //OS产品名称
        private string os_ProductName;
        //OS的使用类型
        private string os_InstallationType;
        //Server系统独有的 系统版本标识
        private string os_CSDVersion;
        //OS 当前版本编号
        private string os_CurrentBuildNumber;
        //计算机网卡信息
        private string net_Info;

        public string IIS_ProductString { get => iis_ProductString; set => iis_ProductString = value; }
        public string IIS_VersionString { get => iis_VersionString; set => iis_VersionString = value; }
        public string OS_CurrentVersion { get => os_CurrentVersion; set => os_CurrentVersion = value; }
        public string OS_ProductName { get => os_ProductName; set => os_ProductName = value; }
        public string OS_InstallationType { get => os_InstallationType; set => os_InstallationType = value; }
        public string OS_CSDVersion { get => os_CSDVersion; set => os_CSDVersion = value; }
        public string OS_CurrentBuildNumber { get => os_CurrentBuildNumber; set => os_CurrentBuildNumber = value; }
        public string Net_Info { get => net_Info; set => net_Info = value; }

        public ComputerInformation()
        {
            GetSystemInfo();
            GetNetWorkInfo();
        }

        private void GetSystemInfo()
        {
            RegistryKey regKey = Registry.LocalMachine.OpenSubKey(Properties.Resources.REG_OSInfoPath);
            os_ProductName = regKey.GetValue("ProductName").ToString();
            os_CurrentVersion = regKey.GetValue("CurrentVersion").ToString();
            os_CurrentBuildNumber = regKey.GetValue("CurrentBuildNumber").ToString();
            os_InstallationType = regKey.GetValue("InstallationType").ToString();
            if (os_InstallationType == "Server")
            {
                os_CSDVersion = regKey.GetValue("CSDVersion").ToString();
            }
            regKey = Registry.LocalMachine.OpenSubKey(Properties.Resources.REG_IISInfoPath);
            iis_ProductString = regKey.GetValue("ProductString").ToString();
            iis_VersionString = regKey.GetValue("VersionString").ToString();

            regKey.Close();
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
        /// 获取当前数据库的版本信息。 2008，2012，2014，Mysql
        /// 数据库类型 1=SqlServer 0=MySql
        /// </summary>
        /// <param name="SqlType">数据库类型 1=SqlServer 0=MySql</param>
        /// <returns>当前已安装的数据库版本信息</returns>
        //public static string GetSqlServerVersion(int SqlType)
        //{
        //    if (SqlType == 1)
        //    {
        //        //SqlServer连接
        //        SqlConnection sqlServerCon = new SqlConnection(SqlServerConntectString);

        //        try
        //        {
        //            sqlServerCon.Open();

        //            SqlVersion = sqlServerCon.ServerVersion.ToString();
        //        }
        //        catch (Exception ex)
        //        {                 
        //            SqlVersion = "获取数据库版本失败：\n"+ ex.Message.ToString();
        //        }
        //        finally
        //        {

        //            sqlServerCon.Close();
        //        }

        //    }
        //    else
        //    {
        //        //MySQL连接
        //        MySqlConnection myConn = new MySqlConnection(MySqlConntectString);

        //        try
        //        {
        //            myConn.Open();

        //            SqlVersion = myConn.ServerVersion.ToString();
        //        }
        //        catch (Exception ex)
        //        {
        //            SqlVersion = "获取数据库版本失败：\n" + ex.Message.ToString();
        //        }
        //        finally
        //        {
        //             myConn.Close();
        //        }
        //    }
        //    return SqlVersion;
        //}

        //public static string GetIisServerVersion()
        //{
        //    try
        //    {
        //        DirectoryEntry de = new DirectoryEntry("IIS://localhost/W3SVC/INFO");
        //        iisVersion = de.Properties["MajorIISVersionNumber"].Value.ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        iisVersion = ex.Message.ToString();
        //    }
        //    return iisVersion;
        //}
    }
}
