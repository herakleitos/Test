using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Text.RegularExpressions;


/*
 说明:在周公的通用数据库访问类的基础上封装了对数据库的常见操作
 (1) SqlServer连接字符串形式: [Server=.;Database=TYP24;UID=sa;PWD=chaint]
 (2) MySql: [@"Server=localhost;Database=crawldb;Uid=root;Pwd=]
 (3) SQLite: [@"Data Source=D:\VS2008\NetworkTime\CrawlApplication\CrawlApplication.db3"]
 (4) Access: ["Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\\DatabasePath\\MmDatabase.mdb;User Id=admin;Password=;"]
 (5) Oracle: ["Provider=MSDAORA;Data Source=MyOracleDB;User Id=myUsername;Password=myPassword;"]
 (6) DB2:    ["Provider=DB2OLEDB;Network Transport Library=TCPIP;
                Network Address=130.120.110.001;
                Initial Catalog=MyCatalog;Package Collection=MyPackageCollection;
                Default Schema=MySchema;User ID=MyUsername;Password=MyPassword;"]
 作者：Hychong
 日期：2011-07-18
 博客地址：http://blog.csdn.net/zhoufoxcn 或http://zhoufoxcn.blog.51cto.com
*/
namespace Chaint.Common.Devices.Data
{
    public sealed class DbUtility
    {
        //public string ConnectionString { get; private set; }
        public string ConnectionString = "";
        private DbProviderFactory providerFactory;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="providerType">数据库类型枚举，参见<paramref name="providerType"/></param>
        public DbUtility(string connectionString, DbProviderType providerType)
        {
            ConnectionString = connectionString;
            providerFactory = ProviderFactory.GetDbProviderFactory(providerType);
            if (providerFactory == null)
            {
                //throw new ArgumentException("Can't load DbProviderFactory for given value of providerType");
                throw new ArgumentException("根据数据连接提供者类型不能加载 DbProviderFactory!");
            }
        }

        /// <summary>   
        /// 对数据库执行增删改操作，返回受影响的行数。   
        /// </summary>   
        /// <param name="sql">要执行的增删改的SQL语句</param>   
        /// <param name="parameters">执行增删改语句所需要的参数</param>
        /// <returns></returns>  
        public int ExecuteNonQuery(string sql, IList<DbParameter> parameters)
        {
            return ExecuteNonQuery(sql, parameters, CommandType.Text);
        }

        /// <summary>   
        /// 对数据库执行增删改操作，返回受影响的行数。   
        /// </summary>   
        /// <param name="sql">要执行的增删改的SQL语句</param>   
        /// <param name="parameters">执行增删改语句所需要的参数</param>
        /// <param name="commandType">执行的SQL语句的类型</param>
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
        /// 执行一个查询语句，返回一个关联的DataReader实例   
        /// </summary>   
        /// <param name="sql">要执行的查询语句</param>   
        /// <param name="parameters">执行SQL查询语句所需要的参数</param>
        /// <returns></returns> 
        public DbDataReader ExecuteReader(string sql, IList<DbParameter> parameters)
        {
            return ExecuteReader(sql, parameters, CommandType.Text);
        }

        /// <summary>   
        /// 执行一个查询语句，返回一个关联的DataReader实例   
        /// </summary>   
        /// <param name="sql">要执行的查询语句</param>   
        /// <param name="parameters">执行SQL查询语句所需要的参数</param>
        /// <param name="commandType">执行的SQL语句的类型</param>
        /// <returns></returns> 
        public DbDataReader ExecuteReader(string sql, IList<DbParameter> parameters, CommandType commandType)
        {
            DbCommand command = CreateDbCommand(sql, parameters, commandType);
            command.Connection.Open();
            return command.ExecuteReader(CommandBehavior.CloseConnection);
        }

        /// <summary>   
        /// 执行一个查询语句，返回一个包含查询结果的DataTable   
        /// </summary>   
        /// <param name="sql">要执行的查询语句</param>   
        /// <param name="parameters">执行SQL查询语句所需要的参数</param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string sql, IList<DbParameter> parameters)
        {
            return ExecuteDataTable(sql, parameters, CommandType.Text);
        }

        /// <summary>   
        /// 执行一个查询语句，返回一个包含查询结果的DataTable   
        /// </summary>   
        /// <param name="sql">要执行的查询语句</param>   
        /// <param name="parameters">执行SQL查询语句所需要的参数</param>
        /// <param name="commandType">执行的SQL语句的类型</param>
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
        /// 执行一个查询语句，返回查询结果的第一行第一列   
        /// </summary>   
        /// <param name="sql">要执行的查询语句</param>   
        /// <param name="parameters">执行SQL查询语句所需要的参数</param>   
        /// <returns></returns>   
        public Object ExecuteScalar(string sql, IList<DbParameter> parameters)
        {
            return ExecuteScalar(sql, parameters, CommandType.Text);
        }

        /// <summary>   
        /// 执行一个查询语句，返回查询结果的第一行第一列   
        /// </summary>   
        /// <param name="sql">要执行的查询语句</param>   
        /// <param name="parameters">执行SQL查询语句所需要的参数</param>   
        /// <param name="commandType">执行的SQL语句的类型</param>
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
        /// 创建一个DbCommand对象
        /// </summary>
        /// <param name="sql">要执行的查询语句</param>   
        /// <param name="parameters">执行SQL查询语句所需要的参数</param>
        /// <param name="commandType">执行的SQL语句的类型</param>
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
                IList<DbParameter> lstparams = new List<DbParameter>(); //初始化时需要注意IList是接口
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
                    retMsg = "存储过程传输参数错误!";
                    return;
                }

                DbParameter myParams = null;
                IList<DbParameter> lstparams = new List<DbParameter>(); //初始化时需要注意IList是接口
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
    /// 数据库类型枚举
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
    /// DbProviderFactory工厂类
    /// </summary>
    public class ProviderFactory
    {
        private static Dictionary<DbProviderType, string> providerInvariantNames = new Dictionary<DbProviderType, string>();
        private static Dictionary<DbProviderType, DbProviderFactory> providerFactoies = new Dictionary<DbProviderType, DbProviderFactory>(20);
        static ProviderFactory()
        {
            //加载已知的数据库访问类的程序集
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
        /// 获取指定数据库类型对应的程序集名称
        /// </summary>
        /// <param name="providerType">数据库类型枚举</param>
        /// <returns></returns>
        public static string GetProviderInvariantName(DbProviderType providerType)
        {
            return providerInvariantNames[providerType];
        }
        /// <summary>
        /// 获取指定类型的数据库对应的DbProviderFactory
        /// </summary>
        /// <param name="providerType">数据库类型枚举</param>
        /// <returns></returns>
        public static DbProviderFactory GetDbProviderFactory(DbProviderType providerType)
        {
            //如果还没有加载，则加载该DbProviderFactory
            if (!providerFactoies.ContainsKey(providerType))
            {
                providerFactoies.Add(providerType, ImportDbProviderFactory(providerType));
            }
            return providerFactoies[providerType];
        }
        /// <summary>
        /// 加载指定数据库类型的DbProviderFactory
        /// </summary>
        /// <param name="providerType">数据库类型枚举</param>
        /// <returns></returns>
        private static DbProviderFactory ImportDbProviderFactory(DbProviderType providerType)
        {
            string providerName = providerInvariantNames[providerType];
            DbProviderFactory factory = null;
            try
            {
                //从全局程序集中查找
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
