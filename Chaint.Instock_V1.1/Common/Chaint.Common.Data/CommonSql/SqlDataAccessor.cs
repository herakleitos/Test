using System;
using System.Data;
using System.Data.SqlClient;
using Chaint.Common.Core.Enums;
using Chaint.Common.Core.Sql;

namespace Chaint.Common.Data
{
    public class SqlDataAccessor
    {
        private string m_ConnectString = "";
        private Enums_DBType m_DBType;
        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionString
        {
            get { return m_ConnectString; }
        }
        /// <summary>
        /// 当前DB类型
        /// </summary>
        public Enums_DBType DBType
        {
            get { return m_DBType; }
        }
        public SqlDataAccessor(Enums_DBType dbType, string strConnectString)
        {
            m_DBType = dbType;
            m_ConnectString = strConnectString;
        }
        public void BulkInsert(BatchInsertParam parameter)
        {
            using (SqlConnection connection = new SqlConnection(this.ConnectionString))
            {
                connection.Open();
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                {
                    bulkCopy.DestinationTableName = parameter.TableName;
                    bulkCopy.BatchSize = parameter.Data.Rows.Count;
                    foreach (string dataField in parameter.FieldMapping.Keys)
                    {
                        string DBField = parameter.FieldMapping[dataField];
                        bulkCopy.ColumnMappings.Add(dataField, DBField);
                    }
                    bulkCopy.WriteToServer(parameter.Data);
                }
            }
        }
        public int ExcuteNoQuery(string Sql,SqlParameter[] parameters)
        {
            using (SqlConnection connection = new SqlConnection(this.ConnectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = Sql;
                cmd.CommandType = CommandType.Text;
                foreach (SqlParameter p in parameters)
                {
                    if (p != null)
                    {
                        if ((p.Direction == ParameterDirection.InputOutput ||
                          p.Direction == ParameterDirection.Input) &&
                          (p.Value == null))
                        {
                            p.Value = DBNull.Value;
                        }
                        cmd.Parameters.Add(p);
                    }
                }
                int i = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                connection.Close();
                return i;
            }
        }
        public DataSet ExecuteQuery(string sql, string[] tableNames, params SqlParameter[] parameters)
        {
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(this.ConnectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;
                foreach (SqlParameter p in parameters)
                {
                    if (p != null)
                    {
                        if ((p.Direction == ParameterDirection.InputOutput ||
                          p.Direction == ParameterDirection.Input) &&
                          (p.Value == null))
                        {
                            p.Value = DBNull.Value;
                        }
                        cmd.Parameters.Add(p);
                    }
                }
                using (SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd))
                {
                    if (tableNames != null && tableNames.Length > 0)
                    {
                        string tableName = "Table";
                        for (int index = 0; index < tableNames.Length; index++)
                        {
                            if (tableNames[index] == null || tableNames[index].Length == 0)
                                throw new ArgumentException("传入的表名为空");
                            dataAdapter.TableMappings.Add((index > 0) ? (tableName + index.ToString())
                                : tableName, tableNames[index]);
                        }
                    }
                    dataAdapter.Fill(ds);
                    cmd.Parameters.Clear();
                }
                connection.Close();
            }
            return ds;
        }
        public DataSet ExecuteSproc(string procName,string[] tableNames, params SqlParameter[] parameters)
        {
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(this.ConnectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandText= procName;
                cmd.CommandType = CommandType.StoredProcedure;
                foreach (SqlParameter p in parameters)
                {
                    if (p != null)
                    {
                        if ((p.Direction == ParameterDirection.InputOutput ||
                          p.Direction == ParameterDirection.Input) &&
                          (p.Value == null))
                        {
                            p.Value = DBNull.Value;
                        }
                        cmd.Parameters.Add(p);
                    }
                }
                using (SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd))
                {
                    if (tableNames != null && tableNames.Length > 0)
                    {
                        string tableName = "Table";
                        for (int index = 0; index < tableNames.Length; index++)
                        {
                            if (tableNames[index] == null || tableNames[index].Length == 0)
                                throw new ArgumentException("传入的表名为空");
                            dataAdapter.TableMappings.Add((index > 0) ? (tableName + index.ToString()) : tableName, tableNames[index]);
                        }
                    }
                    dataAdapter.Fill(ds);
                    cmd.Parameters.Clear();
                }
                connection.Close();
            }
            return ds;
        }
    }
}
