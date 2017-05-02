using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace DataLinkApplicationTools
{
     class SqlOperation
    {

        /// <summary>
        /// 建立数据库连接.
        /// </summary>
        /// <param name="sql_ConnectionString">SQL库连接字串</param>
        /// <returns>返回SqlConnection对象</returns>
        public SqlConnection GetSqlServerConnection(string sql_ConnectionString)
        {
            SqlConnection mySqlConnection = new SqlConnection(sql_ConnectionString);
            return mySqlConnection;
        }
        ///<summary>
        ///执行SqlCommand
        ///<summary>
        ///<param name="sql_ExecuteSQL"</param>
        public void ExecuteSQLAction(string sql_ExecuteSQL)
        {
            SqlConnection conn = this.GetSqlServerConnection("123");
            conn.Open();
            try
            {
                SqlCommand sqlCmd = new SqlCommand(sql_ExecuteSQL,conn);
                int a  = int.Parse(sqlCmd.ExecuteNonQuery().ToString());
                sqlCmd.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally {
                conn.Close();
                           
            }

        }








    }
}
