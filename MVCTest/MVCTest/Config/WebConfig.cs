using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace MVCTest.Config
{
    public class WebConfig
    {

        public static string DataBaseConnectionString()
        {
            string DataSource = ConfigurationManager.AppSettings["Server"].ToString();
            string DataBase = ConfigurationManager.AppSettings["DataBase"].ToString();
            string UserId = ConfigurationManager.AppSettings["User"].ToString();
            string PassWord = ConfigurationManager.AppSettings["Password"].ToString();
            return ConnectionString(DataSource, DataBase, UserId, PassWord);
        }
        public static string ConnectionString(string DataSource, string DataBase, string UserId, string PassWord)
        {
            //return string.Concat("metadata=res://*/ClientData.csdl|res://*/ClientData.ssdl|res://*/ClientData.msl;provider=System.Data.SqlClient;provider connection string=\"",
            //    "Data Source=" + DataSource + ";Initial Catalog=" + DataBase + ";User ID=" + UserId + ";Password=" + PassWord + ";MultipleActiveResultSets=True;\"");
            return string.Format("Data Source={0};Initial Catalog={1};User ID={2};Password={3}", DataSource, DataBase, UserId, PassWord);
        }
    }
}