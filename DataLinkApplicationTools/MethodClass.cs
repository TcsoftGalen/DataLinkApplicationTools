using System;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.DirectoryServices;


namespace DataLinkApplicationTools
{
    class MethodClass
    {
        private const string WindowsServer2008 = "6";
        private const string WindowsServer2008R2 = "6.1";
        private const string WindowsServer2012 = "6.2";
        private const string WindowsServer2012R2 = "6.3*";
        private const string SqlServerConntectString = "Data Source=127.0.0.1; Initial Catalog=master; User ID=sa; Password=Tcsoft123; Connect Timeout=3;";
        private const string MySqlConntectString = "Data Source=127.0.0.1; Initial Catalog=master; User ID=sa; Password=Tcsoft123; Connect Timeout=3;";
        private static string OSystemVersion;
        private static string SqlVersion;
        private static string iisVersion;

        /// <summary>
        /// 获取当前操作系统的版本信息 2008，2008r2，2012，2012r2
        /// </summary>
        /// <returns>当前系统的版本字符串</returns>
        public static string GetSystemVersion()
        {
            switch (Environment.OSVersion.Version.Major + "." + Environment.OSVersion.Version.Minor)
            {
                case WindowsServer2008:
                    OSystemVersion = "Windows Server 2008";
                    break;
                case WindowsServer2008R2:
                    OSystemVersion = "Windows Server 2008 R2 Sp1";
                    break;
                case WindowsServer2012:
                    OSystemVersion = "Windows Server 2012";
                    break;
                case WindowsServer2012R2:
                    OSystemVersion = "Windows Server 2012 R2";
                    break;
                default:
                    OSystemVersion = "Any Other System";
                    break;

            }
            return OSystemVersion;
        }
        /// <summary>
        /// 获取当前数据库的版本信息。 2008，2012，2014，Mysql
        /// 数据库类型 1=SqlServer 0=MySql
        /// </summary>
        /// <param name="SqlType">数据库类型 1=SqlServer 0=MySql</param>
        /// <returns>当前已安装的数据库版本信息</returns>
        public static string GetSqlServerVersion(int SqlType)
        {
            if (SqlType == 1)
            {
                //SqlServer连接
                SqlConnection sqlServerCon = new SqlConnection(SqlServerConntectString);

                try
                {
                    sqlServerCon.Open();

                    SqlVersion = sqlServerCon.ServerVersion.ToString();
                }
                catch (Exception ex)
                {                 
                    SqlVersion = "获取数据库版本失败：\n"+ ex.Message.ToString();
                }
                finally
                {
                    
                    sqlServerCon.Close();
                }

            }
            else
            {
                //MySQL连接
                MySqlConnection myConn = new MySqlConnection(MySqlConntectString);

                try
                {
                    myConn.Open();

                    SqlVersion = myConn.ServerVersion.ToString();
                }
                catch (Exception ex)
                {
                    SqlVersion = "获取数据库版本失败：\n" + ex.Message.ToString();
                }
                finally
                {
                     myConn.Close();
                }
            }
            return SqlVersion;
        }
       
        public static string GetIisServerVersion()
        {
            try
            {
                DirectoryEntry de = new DirectoryEntry("IIS://localhost/W3SVC/INFO");
                iisVersion = de.Properties["MajorIISVersionNumber"].Value.ToString();
            }
            catch (Exception ex)
            {
                iisVersion = ex.Message.ToString();
            }
            return iisVersion;
        }
    }
}
