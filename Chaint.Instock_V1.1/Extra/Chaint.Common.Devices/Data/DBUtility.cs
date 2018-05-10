using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Text.RegularExpressions;


/*
 ˵��:���ܹ���ͨ�����ݿ������Ļ����Ϸ�װ�˶����ݿ�ĳ�������
 (1) SqlServer�����ַ�����ʽ: [Server=.;Database=TYP24;UID=sa;PWD=chaint]
 (2) MySql: [@"Server=localhost;Database=crawldb;Uid=root;Pwd=]
 (3) SQLite: [@"Data Source=D:\VS2008\NetworkTime\CrawlApplication\CrawlApplication.db3"]
 (4) Access: ["Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\\DatabasePath\\MmDatabase.mdb;User Id=admin;Password=;"]
 (5) Oracle: ["Provider=MSDAORA;Data Source=MyOracleDB;User Id=myUsername;Password=myPassword;"]
 (6) DB2:    ["Provider=DB2OLEDB;Network Transport Library=TCPIP;
                Network Address=130.120.110.001;
                Initial Catalog=MyCatalog;Package Collection=MyPackageCollection;
                Default Schema=MySchema;User ID=MyUsername;Password=MyPassword;"]
 ���ߣ�Hychong
 ���ڣ�2011-07-18
 ���͵�ַ��http://blog.csdn.net/zhoufoxcn ��http://zhoufoxcn.blog.51cto.com
*/
namespace Chaint.Common.Devices.Data
{
    public sealed class DbUtility
    {
        //public string ConnectionString { get; private set; }
        public string ConnectionString = "";
        private DbProviderFactory providerFactory;

        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="connectionString">���ݿ������ַ���</param>
        /// <param name="providerType">���ݿ�����ö�٣��μ�<paramref name="providerType"/></param>
        public DbUtility(string connectionString, DbProviderType providerType)
        {
            ConnectionString = connectionString;
            providerFactory = ProviderFactory.GetDbProviderFactory(providerType);
            if (providerFactory == null)
            {
                //throw new ArgumentException("Can't load DbProviderFactory for given value of providerType");
                throw new ArgumentException("�������������ṩ�����Ͳ��ܼ��� DbProviderFactory!");
            }
        }

        /// <summary>   
        /// �����ݿ�ִ����ɾ�Ĳ�����������Ӱ���������   
        /// </summary>   
        /// <param name="sql">Ҫִ�е���ɾ�ĵ�SQL���</param>   
        /// <param name="parameters">ִ����ɾ���������Ҫ�Ĳ���</param>
        /// <returns></returns>  
        public int ExecuteNonQuery(string sql, IList<DbParameter> parameters)
        {
            return ExecuteNonQuery(sql, parameters, CommandType.Text);
        }

        /// <summary>   
        /// �����ݿ�ִ����ɾ�Ĳ�����������Ӱ���������   
        /// </summary>   
        /// <param name="sql">Ҫִ�е���ɾ�ĵ�SQL���</param>   
        /// <param name="parameters">ִ����ɾ���������Ҫ�Ĳ���</param>
        /// <param name="commandType">ִ�е�SQL��������</param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sql, IList<DbParameter> parameters, CommandType commandType)
        {
            using (DbCommand command = CreateDbCommand(sql, parameters, commandType))
            {
                command.Connection.Open();
                int affectedRows = command.ExecuteNonQuery();
                command.Connection.Close();
                return affectedRows;
            }
        }

        /// <summary>   
        /// ִ��һ����ѯ��䣬����һ��������DataReaderʵ��   
        /// </summary>   
        /// <param name="sql">Ҫִ�еĲ�ѯ���</param>   
        /// <param name="parameters">ִ��SQL��ѯ�������Ҫ�Ĳ���</param>
        /// <returns></returns> 
        public DbDataReader ExecuteReader(string sql, IList<DbParameter> parameters)
        {
            return ExecuteReader(sql, parameters, CommandType.Text);
        }

        /// <summary>   
        /// ִ��һ����ѯ��䣬����һ��������DataReaderʵ��   
        /// </summary>   
        /// <param name="sql">Ҫִ�еĲ�ѯ���</param>   
        /// <param name="parameters">ִ��SQL��ѯ�������Ҫ�Ĳ���</param>
        /// <param name="commandType">ִ�е�SQL��������</param>
        /// <returns></returns> 
        public DbDataReader ExecuteReader(string sql, IList<DbParameter> parameters, CommandType commandType)
        {
            DbCommand command = CreateDbCommand(sql, parameters, commandType);
            command.Connection.Open();
            return command.ExecuteReader(CommandBehavior.CloseConnection);
        }

        /// <summary>   
        /// ִ��һ����ѯ��䣬����һ��������ѯ�����DataTable   
        /// </summary>   
        /// <param name="sql">Ҫִ�еĲ�ѯ���</param>   
        /// <param name="parameters">ִ��SQL��ѯ�������Ҫ�Ĳ���</param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string sql, IList<DbParameter> parameters)
        {
            return ExecuteDataTable(sql, parameters, CommandType.Text);
        }

        /// <summary>   
        /// ִ��һ����ѯ��䣬����һ��������ѯ�����DataTable   
        /// </summary>   
        /// <param name="sql">Ҫִ�еĲ�ѯ���</param>   
        /// <param name="parameters">ִ��SQL��ѯ�������Ҫ�Ĳ���</param>
        /// <param name="commandType">ִ�е�SQL��������</param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string sql, IList<DbParameter> parameters, CommandType commandType)
        {
            using (DbCommand command = CreateDbCommand(sql, parameters, commandType))
            {
                using (DbDataAdapter adapter = providerFactory.CreateDataAdapter())
                {
                    adapter.SelectCommand = command;
                    DataTable data = new DataTable();
                    adapter.Fill(data);
                    return data;
                }
            }
        }

        /// <summary>   
        /// ִ��һ����ѯ��䣬���ز�ѯ����ĵ�һ�е�һ��   
        /// </summary>   
        /// <param name="sql">Ҫִ�еĲ�ѯ���</param>   
        /// <param name="parameters">ִ��SQL��ѯ�������Ҫ�Ĳ���</param>   
        /// <returns></returns>   
        public Object ExecuteScalar(string sql, IList<DbParameter> parameters)
        {
            return ExecuteScalar(sql, parameters, CommandType.Text);
        }

        /// <summary>   
        /// ִ��һ����ѯ��䣬���ز�ѯ����ĵ�һ�е�һ��   
        /// </summary>   
        /// <param name="sql">Ҫִ�еĲ�ѯ���</param>   
        /// <param name="parameters">ִ��SQL��ѯ�������Ҫ�Ĳ���</param>   
        /// <param name="commandType">ִ�е�SQL��������</param>
        /// <returns></returns>   
        public Object ExecuteScalar(string sql, IList<DbParameter> parameters, CommandType commandType)
        {
            using (DbCommand command = CreateDbCommand(sql, parameters, commandType))
            {
                command.Connection.Open();
                object result = command.ExecuteScalar();
                command.Connection.Close();
                return result;
            }
        }

        /// <summary>
        /// ����һ��DbCommand����
        /// </summary>
        /// <param name="sql">Ҫִ�еĲ�ѯ���</param>   
        /// <param name="parameters">ִ��SQL��ѯ�������Ҫ�Ĳ���</param>
        /// <param name="commandType">ִ�е�SQL��������</param>
        /// <returns></returns>
        private DbCommand CreateDbCommand(string sql, IList<DbParameter> parameters, CommandType commandType)
        {
            DbConnection connection = providerFactory.CreateConnection();
            DbCommand command = providerFactory.CreateCommand();
            connection.ConnectionString = ConnectionString;
            command.CommandText = sql;
            command.CommandType = commandType;
            command.Connection = connection;
            if (!(parameters == null || parameters.Count == 0))
            {
                foreach (DbParameter parameter in parameters)
                {
                    command.Parameters.Add(parameter);
                }
            }
            return command;
        }

        private DbCommand CreateDbCommand(string sql, CommandType commandType)
        {
            DbConnection connection = providerFactory.CreateConnection();
            DbCommand command = providerFactory.CreateCommand();
            connection.ConnectionString = ConnectionString;
            command.CommandText = sql;
            command.CommandType = commandType;
            command.Connection = connection;
            return command;
        }

        public void ExeSql(string strSql, ref string retMsg)
        {
            try
            {
                using (DbCommand command = CreateDbCommand(strSql, CommandType.Text))
                {
                    command.Connection.Open();
                    int affectedRows = command.ExecuteNonQuery();
                    command.Connection.Close();
                    retMsg = "success";
                }
            }
            catch (System.Exception ex)
            {
                retMsg = ex.Message;
            }
        }


        public void ExeSql(string strSql, ref DataSet dsData, ref string retMsg)
        {
            try
            {
                using (DbCommand command = CreateDbCommand(strSql, CommandType.Text))
                {
                    using (DbDataAdapter adapter = providerFactory.CreateDataAdapter())
                    {
                        adapter.SelectCommand = command;
                        dsData = new DataSet();
                        adapter.Fill(dsData);
                    }
                    retMsg = "success";
                    if (command.Connection.State != ConnectionState.Closed) command.Connection.Close();
                }
            }
            catch (System.Exception ex)
            {
                retMsg = ex.Message;
            }
        }

        public void ExeSql(string strSql, ref DataTable dtData, ref string retMsg)
        {
            try
            {
                using (DbCommand command = CreateDbCommand(strSql, CommandType.Text))
                {
                    using (DbDataAdapter adapter = providerFactory.CreateDataAdapter())
                    {
                        adapter.SelectCommand = command;
                        dtData = new DataTable();
                        adapter.Fill(dtData);
                    }
                    retMsg = "success";
                    if (command.Connection.State != ConnectionState.Closed) command.Connection.Close();
                }
            }
            catch (System.Exception ex)
            {
                retMsg = ex.Message;
            }
        }

        public void ExeSql(string strSql, string strParamName, object objParamValue, ref string retMsg)
        {
            try
            {
                DbParameter myParams = providerFactory.CreateParameter();
                IList<DbParameter> lstparams = new List<DbParameter>(); //��ʼ��ʱ��Ҫע��IList�ǽӿ�
                myParams.ParameterName = strParamName;
                myParams.Value = objParamValue;
                lstparams.Add(myParams);
                using (DbCommand command = CreateDbCommand(strSql, lstparams, CommandType.StoredProcedure))
                {
                    command.Connection.Open();
                    int result = command.ExecuteNonQuery();
                    command.Connection.Close();
                    retMsg = "success";
                    if (command.Connection.State != ConnectionState.Closed) command.Connection.Close();
                }
            }
            catch (System.Exception ex)
            {
                retMsg = ex.Message;
            }
        }

        public void ExeSql(string strSql, string[] strParamNames, object[] objParamValues, ref string retMsg)
        {
            try
            {
                if (strParamNames == null || objParamValues == null || strParamNames.Length != objParamValues.Length)
                {
                    retMsg = "�洢���̴����������!";
                    return;
                }

                DbParameter myParams = null;
                IList<DbParameter> lstparams = new List<DbParameter>(); //��ʼ��ʱ��Ҫע��IList�ǽӿ�
                for (int i = 0; i < strParamNames.Length; i++)
                {
                    myParams = providerFactory.CreateParameter();
                    myParams.ParameterName = strParamNames[i];
                    myParams.Value = objParamValues[i];
                    lstparams.Add(myParams);
                }
                using (DbCommand command = CreateDbCommand(strSql, lstparams, CommandType.StoredProcedure))
                {
                    command.Connection.Open();
                    int result = command.ExecuteNonQuery();
                    command.Connection.Close();
                    retMsg = "success";
                    if (command.Connection.State != ConnectionState.Closed) command.Connection.Close();
                }
            }
            catch (System.Exception ex)
            {
                retMsg = ex.Message;
            }
        }

        public void ExeSql(string strSql, string strParamName, object objParamValue, ref DataTable dtData, ref string retMsg)
        {
            try
            {
                DbParameter myParams = providerFactory.CreateParameter();
                myParams.ParameterName = strParamName;
                myParams.Value = objParamValue;

                IList<DbParameter> lstParams = new List<DbParameter>();
                lstParams.Add(myParams);

                using (DbCommand command = CreateDbCommand(strSql, lstParams, CommandType.StoredProcedure))
                {
                    using (DbDataAdapter adapter = providerFactory.CreateDataAdapter())
                    {
                        adapter.SelectCommand = command;
                        dtData = new DataTable();
                        adapter.Fill(dtData);
                    }
                    retMsg = "success";
                    if (command.Connection.State != ConnectionState.Closed) command.Connection.Close();
                }
            }
            catch (System.Exception ex)
            {
                retMsg = ex.Message;
            }
        }

        public void ExeSql(string strSql, string[] strParamNames, object[] objParamValues, ref DataTable dtData, ref string retMsg)
        {
            try
            {
                DbParameter myParams = null;
                IList<DbParameter> lstParams = new List<DbParameter>();
                for (int i = 0; i < strParamNames.Length; i++)
                {
                    myParams = providerFactory.CreateParameter();
                    myParams.ParameterName = strParamNames[i];
                    myParams.Value = objParamValues[i];
                    lstParams.Add(myParams);
                }
                using (DbCommand command = CreateDbCommand(strSql, lstParams, CommandType.StoredProcedure))
                {
                    using (DbDataAdapter adapter = providerFactory.CreateDataAdapter())
                    {
                        adapter.SelectCommand = command;
                        dtData = new DataTable();
                        adapter.Fill(dtData);
                    }
                    retMsg = "success";
                    if (command.Connection.State != ConnectionState.Closed) command.Connection.Close();
                }
            }
            catch (System.Exception ex)
            {
                retMsg = ex.Message;
            }
        }

    }
    /// <summary>
    /// ���ݿ�����ö��
    /// </summary>
    public enum DbProviderType : byte
    {
        SqlServer,
        MySql,
        SQLite,
        Oracle,
        ODBC,
        OleDb,
        Firebird,
        PostgreSql,
        DB2,
        Informix,
        SqlServerCe
    }
    /// <summary>
    /// DbProviderFactory������
    /// </summary>
    public class ProviderFactory
    {
        private static Dictionary<DbProviderType, string> providerInvariantNames = new Dictionary<DbProviderType, string>();
        private static Dictionary<DbProviderType, DbProviderFactory> providerFactoies = new Dictionary<DbProviderType, DbProviderFactory>(20);
        static ProviderFactory()
        {
            //������֪�����ݿ������ĳ���
            providerInvariantNames.Add(DbProviderType.SqlServer, "System.Data.SqlClient");
            providerInvariantNames.Add(DbProviderType.OleDb, "System.Data.OleDb");
            providerInvariantNames.Add(DbProviderType.ODBC, "System.Data.ODBC");
            providerInvariantNames.Add(DbProviderType.Oracle, "Oracle.DataAccess.Client");
            providerInvariantNames.Add(DbProviderType.MySql, "MySql.Data.MySqlClient");
            providerInvariantNames.Add(DbProviderType.SQLite, "System.Data.SQLite");
            providerInvariantNames.Add(DbProviderType.Firebird, "FirebirdSql.Data.Firebird");
            providerInvariantNames.Add(DbProviderType.PostgreSql, "Npgsql");
            providerInvariantNames.Add(DbProviderType.DB2, "IBM.Data.DB2.iSeries");
            providerInvariantNames.Add(DbProviderType.Informix, "IBM.Data.Informix");
            providerInvariantNames.Add(DbProviderType.SqlServerCe, "System.Data.SqlServerCe");
        }
        /// <summary>
        /// ��ȡָ�����ݿ����Ͷ�Ӧ�ĳ�������
        /// </summary>
        /// <param name="providerType">���ݿ�����ö��</param>
        /// <returns></returns>
        public static string GetProviderInvariantName(DbProviderType providerType)
        {
            return providerInvariantNames[providerType];
        }
        /// <summary>
        /// ��ȡָ�����͵����ݿ��Ӧ��DbProviderFactory
        /// </summary>
        /// <param name="providerType">���ݿ�����ö��</param>
        /// <returns></returns>
        public static DbProviderFactory GetDbProviderFactory(DbProviderType providerType)
        {
            //�����û�м��أ�����ظ�DbProviderFactory
            if (!providerFactoies.ContainsKey(providerType))
            {
                providerFactoies.Add(providerType, ImportDbProviderFactory(providerType));
            }
            return providerFactoies[providerType];
        }
        /// <summary>
        /// ����ָ�����ݿ����͵�DbProviderFactory
        /// </summary>
        /// <param name="providerType">���ݿ�����ö��</param>
        /// <returns></returns>
        private static DbProviderFactory ImportDbProviderFactory(DbProviderType providerType)
        {
            string providerName = providerInvariantNames[providerType];
            DbProviderFactory factory = null;
            try
            {
                //��ȫ�ֳ����в���
                factory = DbProviderFactories.GetFactory(providerName);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
                factory = null;
            }
            return factory;
        }
    }
}
