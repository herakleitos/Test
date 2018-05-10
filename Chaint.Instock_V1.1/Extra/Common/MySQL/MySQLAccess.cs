using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using DataModel;

namespace Common.MySQL
{
    public  class MySQLAccess
    {
        private string ConnctionString = "";

        private const string jetsettingsInsert_PRO = "jetsettingsinsert";    

  
         /// <summary>
        /// Initializes a new instance of the SQLAccess class.
        /// </summary>
        public MySQLAccess()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
            ConnctionString = Utils.MySqlConnectionString;
            //ConnctionString = "Server=192.168.56.101;Port=3306;Database=test;Uid=root;Pwd=100200;CharSet=big5//assi  utf8;";

        }

        public bool _IsConect = false;
        public bool IsConnect
        {
            get { return _IsConect; }
        }

        public delegate void SqlStateEventHandler(object sender,
               SqlStateEventArgs e);


        public event SqlStateEventHandler SqlStateChange;


        protected virtual void OnSqlStateChange(SqlStateEventArgs e)
        {
            if (SqlStateChange != null)
            {

                _IsConect = e.IsConnect;
                SqlStateChange(this, e);//Raise the event
            }
        }



        public bool TestConnection()
        {
            if (ConnctionString == "")
                return false;

            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = ConnctionString;
            try
            {
                conn.Open();
                _IsConect = true;
            }

            catch
            {
                _IsConect = false;

            }
            finally
            {
                conn.Close();
            }

            return _IsConect;

        }

        public DataSet ServerInfoQuery()
        {
            DataSet ds = new DataSet();
            using (MySqlConnection connection = new MySqlConnection(ConnctionString))
            {
                try
                {
                        MySqlHelper.FillDataset(connection,
                        CommandType.Text,
                        "select  now() as ServerDate",
                        ds,
                        new string[] { "ServerInfo" }
                        );
                    OnSqlStateChange(new SqlStateEventArgs(true, "ServerInfo读取成功"));
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));
                }
            }
            return ds;
        }

         



        public DataSet testAA()
        {
            DataSet ds = new DataSet();
            using (MySqlConnection connection = new MySqlConnection(ConnctionString))
            {
                string sqltext = "select MAKTX from  zph01 where CHARG = '15R7000002'";
                try
                {
                    MySqlHelper.FillDataset(connection,
                        CommandType.Text,
                        sqltext,
                        ds,
                         new string[] { "Roll_ProductAndZPH01" }
                        );

                  
                }
                catch (Exception ex)
                {
                    OnSqlStateChange(new SqlStateEventArgs(false, ex.ToString()));
                }
            }
            return ds;
        }
    
    
    }
}
