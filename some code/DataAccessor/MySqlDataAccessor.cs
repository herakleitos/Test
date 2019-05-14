using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;

namespace DataSync
{
    public class MySqlDataAccessor: DataAccessor
    {
        
        public MySqlDataAccessor(string strConnectString):base(strConnectString)
        {

        }
        public override DbParameter CreateParameter()
        {
            return new MySqlParameter();
        }
        public override DbParameter CreateParameter(string parameterName, object value)
        {
            return new MySqlParameter(parameterName, value);
        }
        public override int ExecuteNonQuery(string sql, params DbParameter[] parameters)
        {
            using (MySqlConnection conn = new MySqlConnection(this.m_ConnectString))
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                foreach (MySqlParameter p in parameters)
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
                cmd.Dispose();
                conn.Close();
                conn.Dispose();
                return i;
            }
        }
        public override DataSet ExecuteQuery(string sql,string[] tableNames, params DbParameter[] parameters)
        {
            DataSet ds = new DataSet();
            using (MySqlConnection conn = new MySqlConnection(this.m_ConnectString))
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                foreach (MySqlParameter p in parameters)
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
                using (MySqlDataAdapter dataAdapter = new MySqlDataAdapter(cmd))
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
                cmd.Dispose();
                conn.Close();
                conn.Dispose();
            }
            return ds;
        }
    }
}
