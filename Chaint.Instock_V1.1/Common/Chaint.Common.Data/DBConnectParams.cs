using Chaint.Common.Core.Enums;
namespace Chaint.Common.Data
{
    public class DBConnectParams
    {
        public Enums_DBType DBType { get; set; }

        /// <summary>
        /// 服务器名称或者对应的IP地址
        /// </summary>
        public string Server { get; set; }
        /// <summary>
        /// 数据库名称 或者数据库所在文件的全路径
        /// </summary>
        public string DataBase { get; set; }
        /// <summary>
        /// 连接数据库的用户ID
        /// </summary>
        public string UserID { get; set; }
        /// <summary>
        /// 连接数据库的用户密码
        /// </summary>
        public string Password { get; set; }
        public DBConnectParams(Enums_DBType dbType, string strServer, string strDatabase, 
            string strUserID, string strPassword)
        {
            this.DBType = DBType;
            this.Server = strServer;
            this.DataBase = strDatabase;
            this.UserID = strUserID;
            this.Password = strPassword;
        }
        public DBConnectParams()
        {
        }
    }
}
