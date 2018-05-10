using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Data;

//针对SQL Server/Access/Oracle/...
namespace Chaint.Common.Devices.Data
{
    public delegate void MessageEventHandler(bool blnConnected);   //主要用于DB连接出错时，可以重新连接
    public abstract class DBAccessFunc
    {
        public abstract string ConnectString { get;}

        public abstract IDbConnection Connection { get;}

        public abstract void Open(ref string retMsg);

        public abstract void TestDBConn();

        public abstract void Close(ref string retMsg);

        public abstract void ExeSql(string strSql, ref string retMsg);
        public abstract void ExeSql(string strSql, ref DataSet ds, ref string retMsg);

        public abstract void ExeSql(string strSql, string strParam, object strValue, ref string retMsg);
        public abstract void ExeSql(string strSql, string[] strParams, object[] strValues, ref string retMsg);

        public abstract void ExeSql(string strSql, string[] strParams, object[] strValues, ref  DataSet ds, ref string retMsg);
        public abstract void ExeSql(string strSql, string strParam, object strValue, ref  DataSet ds, ref string retMsg);

        public abstract void ExeSql(System.Collections.ArrayList SQLStringList, ref string retMsg);

        public abstract event MessageEventHandler OnDBConnectedChanged;

    }

    public class DBAccessorFactory
    {
        public static DBAccessFunc GetDBAccessor(string strConnection)
        {
            if (strConnection.ToLower().IndexOf("provider=") < 0) //SqlServer	
            {
                return new SQLAccessor(strConnection);
            }
            else     //other database	
            {
                return new SQLAccessor(strConnection);
            }
        }
    }

    //SQL Server数据库
    internal class SQLAccessor : DBAccessFunc
    {
        public override event MessageEventHandler OnDBConnectedChanged;
        private SqlConnection m_conn = null;
        public string m_connectionstring = "";
        public SQLAccessor(string connectionString)
        {
            m_conn = new SqlConnection(connectionString);
            m_connectionstring = connectionString;
        }

        public override string ConnectString
        {
            get { return this.m_connectionstring; }
        }
        public override IDbConnection Connection
        {
            get { return this.m_conn; }
        }

        public override void TestDBConn()
        {
            using (SqlConnection conn = new SqlConnection(m_connectionstring))
            {
                try
                {
                    conn.Open();
                    if (OnDBConnectedChanged != null) OnDBConnectedChanged(true);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    if (OnDBConnectedChanged != null) OnDBConnectedChanged(false);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        public override void Open(ref string retMsg)
        {
            retMsg = "fail";
            using (SqlConnection conn = new SqlConnection(m_connectionstring))
            {
                try
                {
                    conn.Open();
                    retMsg = "success";
                    if (OnDBConnectedChanged != null) OnDBConnectedChanged(true);
                    return;
                }
                catch (Exception ex)
                {
                    retMsg = string.Format("数据库打开失败,原因:{0}", ex.Message);
                    if (OnDBConnectedChanged != null) OnDBConnectedChanged(false);
                    return;
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        public override void Close(ref string retMsg)
        {
            retMsg = "fail";
            try
            {
                if (m_conn.State != ConnectionState.Closed)
                {
                    m_conn.Close();
                }
                retMsg = "success";
                if (OnDBConnectedChanged != null) OnDBConnectedChanged(false);
                return;
            }
            catch (System.Exception ex)
            {
                string.Format("数据库关闭错误,原因:{0}", ex.Message);
                return;
            }
        }

        public override void ExeSql(string strSql, ref string retMsg)
        {
            retMsg = "fail";
            using (SqlConnection conn = new SqlConnection(m_connectionstring))
            {
                try
                {
                    using( SqlCommand cmd = new SqlCommand(strSql, conn))
                    {
                        cmd.CommandTimeout = 30;
                        cmd.CommandType = CommandType.Text;
                        if (conn.State != ConnectionState.Open) conn.Open();
                        cmd.ExecuteScalar();
                        conn.Close();
                        retMsg = "success";
                        if (OnDBConnectedChanged != null) OnDBConnectedChanged(true);
                        return;
                    }
                }
                catch (System.Exception ex)
                {
                    if (conn.State != ConnectionState.Closed) conn.Close();
                    if (OnDBConnectedChanged != null) OnDBConnectedChanged(false);
                    retMsg = ex.Message;
                    return;
                }
            }
        }

        public override void ExeSql(string strSql, ref DataSet ds, ref string retMsg)
        {
            retMsg = "fail";
            ds = new DataSet();

            using (SqlConnection conn = new SqlConnection(m_connectionstring))
            {
                try
                {
                    using (SqlCommandBuilder thisBuilder = new SqlCommandBuilder())
                    {
                        if (conn.State != ConnectionState.Open) conn.Open();
                        thisBuilder.DataAdapter = new System.Data.SqlClient.SqlDataAdapter(strSql, conn);
                        thisBuilder.DataAdapter.Fill(ds);
                        retMsg = "success";
                        conn.Close();
                        if (OnDBConnectedChanged != null) OnDBConnectedChanged(true);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    if (conn.State != ConnectionState.Closed) conn.Close();
                    retMsg = ex.Message;
                    if (OnDBConnectedChanged != null) OnDBConnectedChanged(false);
                    return;
                }
            }
        }

        public override void ExeSql(string strSql, string strParam, object strValue, ref string retMsg)
        {
            retMsg = "fail";
            using (SqlConnection conn = new SqlConnection(m_connectionstring))
            {
                using(SqlCommand cmd = new SqlCommand(strSql, conn))
                {
                    try
                    {
                        conn.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@" + strParam, strValue);
                        cmd.ExecuteNonQuery();
                        retMsg = "success";
                        conn.Close();
                        cmd.Parameters.Clear();
                        if (OnDBConnectedChanged != null) OnDBConnectedChanged(true);
                    }
                    catch (System.Exception ex)
                    {
                        if (conn.State != ConnectionState.Closed) conn.Close();
                        cmd.Parameters.Clear();
                        retMsg = ex.Message;
                        if (OnDBConnectedChanged != null) OnDBConnectedChanged(false);
                    }
                }
            }
        }

        public override void ExeSql(string strSql, string strParam, object strValue, ref DataSet ds, ref string retMsg)
        {
            retMsg = "fail";
            ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(m_connectionstring))
            {
                using (SqlCommand cmd = new SqlCommand(strSql, conn))
                {
                    try
                    {
                        conn.Open();
                        cmd.CommandTimeout = 30;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@" + strParam, strValue);
                        SqlDataAdapter sda = new SqlDataAdapter(cmd);
                        sda.Fill(ds);

                        conn.Close();
                        cmd.Parameters.Clear();//这一句一定要加上，否则会出现parameters集合类资源不能及时释放，导致出错！;

                        retMsg = "success";
                        if (OnDBConnectedChanged != null) OnDBConnectedChanged(true);
                        return;
                    }
                    catch (System.Exception ex)
                    {
                        conn.Close();
                        cmd.Parameters.Clear();//这一句一定要加上，否则会出现parameters集合类资源不能及时释放，导致出错！;

                        retMsg = ex.Message;
                        if (OnDBConnectedChanged != null) OnDBConnectedChanged(false);
                        return;
                    }
               
                }
            }
         }

        public override void ExeSql(string strSql, string[] strParams, object[] strValues, ref string retMsg)
        {
            retMsg = "fail";
            if ((strParams != null) && (strParams.Length != strValues.Length))
            { retMsg = "查询参数和值长度不匹配!"; return; }
            using (SqlConnection conn = new SqlConnection(m_connectionstring))
            {
                using (SqlCommand cmd = new SqlCommand(strSql, conn))
                {
                    try
                    {
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        cmd.CommandTimeout = 30;
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (strParams != null)
                        {
                            for (int i = 0; i < strParams.Length; i++)
                                cmd.Parameters.AddWithValue("@" + strParams[i], strValues[i]);

                            cmd.ExecuteNonQuery();

                            if (conn.State != ConnectionState.Closed) conn.Close();
                            cmd.Parameters.Clear();

                            retMsg = "success";
                            if (OnDBConnectedChanged != null) OnDBConnectedChanged(true);
                        }
                        return;
                    }
                    catch (System.Exception ex)
                    {
                        if (conn.State != ConnectionState.Closed) conn.Close();
                        cmd.Parameters.Clear();

                        retMsg = ex.Message;
                        if (OnDBConnectedChanged != null) OnDBConnectedChanged(false);
                        return;
                    }
                }
            }
        }

        public override void ExeSql(string strSql, string[] strParams, object[] strValues, ref DataSet ds, ref string retMsg)
        {
            retMsg = "fail";
            if ((strParams != null) && (strParams.Length != strValues.Length))
            { retMsg = "查询参数和值长度不匹配!"; return; }
            
            ds = new DataSet();

            using (SqlConnection conn = new SqlConnection(m_connectionstring))
            {
                using (SqlCommand cmd = new SqlCommand(strSql, conn))
                {
                    try
                    {
                        if (conn.State != ConnectionState.Open) conn.Open();
                        cmd.CommandTimeout = 30;
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter sda = new SqlDataAdapter(cmd);
                        if (strParams != null)
                        {
                            for (int i = 0; i < strParams.Length; i++)
                                cmd.Parameters.AddWithValue("@" + strParams[i], strValues[i]);
                            sda.Fill(ds);

                            if (conn.State != ConnectionState.Closed) conn.Close();
                            cmd.Parameters.Clear();

                            retMsg = "success";
                            if (OnDBConnectedChanged != null) OnDBConnectedChanged(true);
                        }
                        return;
                    }
                    catch (System.Exception ex)
                    {
                        if (conn.State != ConnectionState.Closed) conn.Close();
                        cmd.Parameters.Clear();

                        retMsg = ex.Message;
                        if (OnDBConnectedChanged != null) OnDBConnectedChanged(false);
                        return;
                    }
                }
            }
        }

        /// <summary>    
        /// 执行多条SQL语句，实现数据库事务。    
        /// </summary>    
        /// <param name="SQLStringList">多条SQL语句</param>        
        public override void ExeSql(System.Collections.ArrayList SQLStringList, ref string retMsg)
        {
            retMsg = "fail";
            string strSql = "";
            if (SQLStringList.Count == 0)
                return;
            using (SqlConnection conn = new SqlConnection(m_connectionstring))
            {

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandTimeout = 30;
                    cmd.CommandType = CommandType.Text;
                    SqlTransaction sqlTrans = conn.BeginTransaction();
                    cmd.Transaction = sqlTrans;
                    try
                    {
                        for (int i = 0; i < SQLStringList.Count; i++)
                        {
                            strSql = SQLStringList[i].ToString();
                            if (strSql.Trim().Length > 1)
                            {
                                if (cmd.Connection.State == ConnectionState.Closed) cmd.Connection.Open();
                                cmd.CommandText = strSql;
                                cmd.ExecuteNonQuery();
                            }
                        }
                        sqlTrans.Commit();
                        retMsg = "success";
                        if (OnDBConnectedChanged != null) OnDBConnectedChanged(true);
                    }
                    catch (System.Exception ex)
                    {
                        sqlTrans.Rollback();
                        retMsg = ex.Message;
                        if (OnDBConnectedChanged != null) OnDBConnectedChanged(false);
                    }
                }
            } 
        }
    }

    //Access 数据库
    internal class AccessAccessor : DBAccessFunc
    {
        public override event MessageEventHandler OnDBConnectedChanged;
        private string m_connectionstring = "";
        private OleDbConnection m_conn = null;

        public override IDbConnection Connection
        {
            get { return this.m_conn; }
        }
        public override string ConnectString
        {
            get { return m_connectionstring; }
        }
        public AccessAccessor(string connectionString)
        {
            m_conn = new OleDbConnection(connectionString);
            m_connectionstring = connectionString;
        }

        public override void TestDBConn()
        {
            using (OleDbConnection conn = new OleDbConnection(m_connectionstring))
            {
                try
                {
                    conn.Open();
                    if (OnDBConnectedChanged != null) OnDBConnectedChanged(true);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    if (OnDBConnectedChanged != null) OnDBConnectedChanged(false);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        public override void Open(ref string retMsg)
        {
            retMsg = "fail";
            bool blnConnected = false;

            using (OleDbConnection conn = new OleDbConnection(m_connectionstring))
            {
                try
                {
                    conn.Open();
                    retMsg = "success";
                    blnConnected = true;
                    return;
                }
                catch (Exception ex)
                {
                    retMsg = string.Format("数据库打开失败,原因:{0}", ex.Message);
                    blnConnected = false;
                    return;
                }
                finally
                {
                    if (OnDBConnectedChanged != null) OnDBConnectedChanged(blnConnected);
                    m_conn.Close();
                }
            }


            
        }

        public override void Close(ref string retMsg)
        {
            retMsg = "fail";
            try
            {
                if (m_conn.State != ConnectionState.Closed)
                {
                    m_conn.Close();
                    m_conn.Dispose();
                    m_conn = null;
                }
                retMsg = "success";
                return;
            }
            catch (System.Exception ex)
            {
                retMsg = string.Format("数据库关闭错误,原因:{0}", ex.Message);
                return;
            }
        }

        public override void ExeSql(string strSql, ref string retMsg)
        {
            retMsg = "fail";
            using (OleDbConnection conn = new OleDbConnection(m_connectionstring))
            {
                using (OleDbCommand cmd = new OleDbCommand(strSql, conn))
                {
                    cmd.CommandTimeout = 30;
                    cmd.CommandType = CommandType.Text;
                    bool blnConnected = false;
                    try
                    {
                        conn.Open();
                        cmd.ExecuteScalar();

                        if (conn.State != ConnectionState.Closed) conn.Close();
                       
                        

                        retMsg = "success";
                        blnConnected = true;

                        if (OnDBConnectedChanged != null) OnDBConnectedChanged(blnConnected);
                        return;
                    }
                    catch (System.Exception ex)
                    {
                        blnConnected = false;
                        retMsg = ex.Message;

                        if (conn.State != ConnectionState.Closed) conn.Close();
                        if (OnDBConnectedChanged != null) OnDBConnectedChanged(blnConnected);
                        return;
                    }
                }
            }
        }

        public override void ExeSql(string strSql, ref DataSet ds, ref string retMsg)
        {
            retMsg = "fail";
            ds = new DataSet();
            bool blnConnected = false;
            using (OleDbConnection conn = new OleDbConnection(m_connectionstring))
            {
                using (OleDbCommandBuilder thisBuilder = new OleDbCommandBuilder())
                {
                    thisBuilder.DataAdapter = new System.Data.OleDb.OleDbDataAdapter(strSql, conn);
                    try
                    {
                        if (conn.State != ConnectionState.Open)  conn.Open();
                        thisBuilder.DataAdapter.Fill(ds);

                        if (conn.State != ConnectionState.Closed) conn.Close();
                        thisBuilder.Dispose();
                        
                        retMsg = "success";
                        blnConnected = true;

                        if (OnDBConnectedChanged != null) OnDBConnectedChanged(blnConnected);
                        return;
                    }
                    catch (Exception ex)
                    {
                        blnConnected = false;
                        retMsg = ex.Message;
                        if (conn.State != ConnectionState.Closed) conn.Close();
                        if (OnDBConnectedChanged != null) OnDBConnectedChanged(blnConnected);
                        return;
                    }
                }
            }
        }

        public override void ExeSql(string strSql, string strParam, object strValue, ref string retMsg)
        {
            retMsg = "fail";
            using (OleDbConnection conn = new OleDbConnection(m_connectionstring))
            {
                using (OleDbCommand cmd = new OleDbCommand(strSql, conn))
                {
                    bool blnConnected = false;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@" + strParam, strValue);
                    try
                    {
                        if (conn.State != ConnectionState.Open)conn.Open();
                        cmd.ExecuteNonQuery();

                        conn.Close();
                        cmd.Parameters.Clear();
                        retMsg = "success";
                        blnConnected = true;
                        if (OnDBConnectedChanged != null) OnDBConnectedChanged(blnConnected);
                        return;
                    }
                    catch (System.Exception ex)
                    {
                        blnConnected = false;
                        retMsg = ex.Message;

                        cmd.Parameters.Clear();
                        if (conn.State != ConnectionState.Closed) conn.Close();
                        if (OnDBConnectedChanged != null) OnDBConnectedChanged(blnConnected);
                        return;
                    }
                }
            }
        }

        public override void ExeSql(string strSql, string strParam, object strValue, ref DataSet ds, ref string retMsg)
        {
            retMsg = "fail";
            ds = new DataSet();
            bool blnConnected = false;

            using (OleDbConnection conn = new OleDbConnection(m_connectionstring))
            {
                using (OleDbCommand cmd = new OleDbCommand(strSql, conn))
                {
                    cmd.CommandTimeout = 30;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@" + strParam, strValue);
                    OleDbDataAdapter sda = new OleDbDataAdapter(cmd);
                    try
                    {
                        if (conn.State != ConnectionState.Open)conn.Open();
                        sda.Fill(ds);

                        if (conn.State != ConnectionState.Closed) conn.Close();

                        cmd.Parameters.Clear();//这一句一定要加上，否则会出现parameters集合类资源不能及时释放，导致出错！

                        blnConnected = true;
                        retMsg = "success";
                        if (OnDBConnectedChanged != null) OnDBConnectedChanged(blnConnected);
                        return;
                    }
                    catch (System.Exception ex)
                    {
                        cmd.Parameters.Clear();//这一句一定要加上，否则会出现parameters集合类资源不能及时释放，导致出错！
                        if (conn.State != ConnectionState.Closed) conn.Close();
                        blnConnected = false;
                        retMsg = ex.Message;
                        if (OnDBConnectedChanged != null) OnDBConnectedChanged(blnConnected);
                        return;
                    }
                }
            }
            
        }

        public override void ExeSql(string strSql, string[] strParams, object[] strValues, ref string retMsg)
        {
            retMsg = "fail";
            bool blnConnected = false;
            if ((strParams != null) && (strParams.Length != strValues.Length))
            { retMsg = "查询参数和值长度不匹配!"; return; }

            using (OleDbConnection conn = new OleDbConnection(m_connectionstring))
            {
                using (OleDbCommand cmd = new OleDbCommand(strSql, conn))
                {
                    cmd.CommandTimeout = 30;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        if (strParams != null)
                        {
                            for (int i = 0; i < strParams.Length; i++)
                                cmd.Parameters.AddWithValue("@" + strParams[i], strValues[i]);
                            if (conn.State != ConnectionState.Open) conn.Open();    
                            cmd.ExecuteNonQuery();

                            if (conn.State != ConnectionState.Closed) conn.Close();
                            cmd.Parameters.Clear();
                            retMsg = "success";
                            blnConnected = true;
                            if (OnDBConnectedChanged != null) OnDBConnectedChanged(blnConnected);
                        
                        }
                        return;
                    }
                    catch (System.Exception ex)
                    {
                        if (conn.State != ConnectionState.Closed) conn.Close();
                        cmd.Parameters.Clear();

                        blnConnected = false;
                        retMsg = ex.Message;
                        if (OnDBConnectedChanged != null) OnDBConnectedChanged(blnConnected);
                        
                        return;
                    }
                }
            }
        }

        public override void ExeSql(string strSql, string[] strParams, object[] strValues, ref DataSet ds, ref string retMsg)
        {
            retMsg = "fail";
            bool blnConnected = false;
            if ((strParams != null) && (strParams.Length != strValues.Length))
            { retMsg = "查询参数和值长度不匹配!"; return; }
            ds = new DataSet();

            using (OleDbConnection conn = new OleDbConnection(m_connectionstring))
            {
                using (OleDbCommand cmd = new OleDbCommand(strSql, conn))
                {
                    cmd.CommandTimeout = 30;
                    cmd.CommandType = CommandType.StoredProcedure;
                    OleDbDataAdapter sda = new OleDbDataAdapter(cmd);
                    try
                    {
                        if (strParams != null)
                        {
                            for (int i = 0; i < strParams.Length; i++)
                                cmd.Parameters.AddWithValue("@" + strParams[i], strValues[i]);
                            if (conn.State != ConnectionState.Open)conn.Open();
                            sda.Fill(ds);
                            blnConnected = true;
                            retMsg = "success";

                            if (conn.State != ConnectionState.Closed) conn.Close();
                            cmd.Parameters.Clear();
                            if (OnDBConnectedChanged != null) OnDBConnectedChanged(blnConnected);
                        }
                        return;
                    }
                    catch (System.Exception ex)
                    {
                        if (conn.State != ConnectionState.Closed) conn.Close();
                        cmd.Parameters.Clear();

                        blnConnected = false;
                        retMsg = ex.Message;

                        if (OnDBConnectedChanged != null) OnDBConnectedChanged(blnConnected);
                        return;
                    }
                }
            }   
        }

        /// <summary>    
        /// 执行多条SQL语句，实现数据库事务。    
        /// </summary>    
        /// <param name="SQLStringList">多条SQL语句</param>        
        public override void ExeSql(System.Collections.ArrayList SQLStringList, ref string retMsg)
        {
            retMsg = "fail";
            string strSql = "";
            bool blnConnected = false;
            if (SQLStringList.Count == 0)
                return;

            using (OleDbConnection conn = new OleDbConnection(m_connectionstring))
            {
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = conn;
                cmd.CommandTimeout = 30;
                cmd.CommandType = CommandType.Text;
                OleDbTransaction oleTrans = conn.BeginTransaction();
                cmd.Transaction = oleTrans;
                try
                {
                    for (int i = 0; i < SQLStringList.Count; i++)
                    {
                        strSql = SQLStringList[i].ToString();
                        if (strSql.Trim().Length > 1)
                        {
                            cmd.CommandText = strSql;
                            cmd.ExecuteNonQuery();
                        }
                    }
                    oleTrans.Commit();
                    blnConnected = true;
                    retMsg = "success";
                    
                }
                catch (System.Exception ex)
                {
                    blnConnected = false;
                    oleTrans.Rollback();
                    retMsg = ex.Message;
                }
                finally
                {
                    if (OnDBConnectedChanged != null) OnDBConnectedChanged(blnConnected);
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }
            
        }

    }
}
