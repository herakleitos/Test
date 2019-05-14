using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSync
{
    public abstract class DataAccessor
    {
        protected string m_ConnectString = "";
        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionString
        {
            get { return m_ConnectString; }
        }
        public DataAccessor(string strConnectString)
        {
            m_ConnectString = strConnectString;

        }
        public abstract DbParameter CreateParameter();
        public abstract DbParameter CreateParameter(string name,object value);
        public abstract int ExecuteNonQuery(string sql, params DbParameter[] parameters);

        public abstract DataSet ExecuteQuery(string sql, string[] tableNames, params DbParameter[] parameters);
    }
}
