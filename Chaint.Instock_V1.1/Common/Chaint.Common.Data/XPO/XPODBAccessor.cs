using System.Linq;
using Chaint.Common.Core.Enums;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.DataAccess.Sql;
using DevExpress.DataAccess.Sql.DataApi;
using DevExpress.DataAccess.ConnectionParameters;
namespace Chaint.Common.Data.XPO
{
    public class XPODBAccessor
    {
        private string m_ConnectString = "";
        private bool m_IsConnected = false;
        private Enums_DBType m_DBType;

        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionString
        {
            get { return m_ConnectString; }
        }
        /// <summary>
        /// 是否已连接
        /// </summary>
        public bool IsConnected
        {
            get { return m_IsConnected; }
        }
        /// <summary>
        /// 当前会话层
        /// </summary>
        public Session DBSession
        {
            get
            {
                return XPOUtils.CreateUnitOfWork(this.ConnectionString);
            }
        }
        /// <summary>
        /// 当前DB类型
        /// </summary>
        public Enums_DBType DBType
        {
            get { return m_DBType; }
        }
        public XPODBAccessor(Enums_DBType dbType, string strConnectString)
        {
            m_DBType = dbType;
            m_ConnectString = strConnectString;
        }
        /// <summary>
        /// 测试连接性
        /// </summary>
        /// <returns></returns>
        public bool TestConnection()
        {
            IDataLayer dataLayer = XPOUtils.CreateDataLayer(this.ConnectionString);
            if (dataLayer == null)
            {
                m_IsConnected = false;
                return false;
            }
            try
            {
                using (UnitOfWork session = new UnitOfWork(dataLayer))
                {
                    if (session != null)
                    {
                        m_IsConnected = true;
                    }
                    else
                    {
                        m_IsConnected = false;
                    }
                }
            }
            catch
            {
                m_IsConnected = false;
            }
            if (dataLayer != null) dataLayer.Dispose();
            return m_IsConnected;
        }

        /// <summary>
        /// 根据自定义的查询语句数组，获取一个结果集 与DataSet类似
        /// </summary>
        /// <param name="SqlQuerys">自定义的查询语句数组,可以包含查询参数</param>
        /// <returns></returns>
        public IResultSet ExecuteQuery(params CustomSqlQuery[] SqlQuerys)
        {
            IResultSet ret = null;
            using (SqlDataSource ds = new SqlDataSource(new CustomStringConnectionParameters(this.ConnectionString)))
            {
                ds.Queries.AddRange(SqlQuerys);
                ds.Fill();
                if (ds.Result != null)
                {
                    ret = ds.Result;
                }
            }
            return ret;
        }
        /// <summary>
        /// 根据Sql及参数查询,获取数据存储结果,一般和XPDataView联合使用，这需要提前建立XPDataView,并手动建立例
        /// 如：
        ///  XPDataView xpdv = new XPDataView();
        ///   xpdvProduct.Properties.AddRange(new DataViewProperty[] {
        ///                 new DataViewProperty ("ProductOID",typeof(int)),
        ///                 new DataViewProperty ("ActualWidth",typeof(decimal)),
        ///                 
        /// xpdv.LoadData(selectedData对象)
        ///  
        /// </summary>
        /// <param name="strSQL"></param>
        /// <param name="parameterNames"></param>
        /// <param name="parameterValues"></param>
        /// <returns></returns>
        public SelectedData ExecuteQuery(string strSQL, string[] parameterNames, object[] parameterValues)
        {
            SelectedData retData = null;
            using (UnitOfWork session = XPOUtils.CreateUnitOfWork(this.ConnectionString))
            {
                retData = session.ExecuteQuery(strSQL, parameterNames, parameterValues);
                session.DataLayer.Dispose();
                session.Disconnect();
            }
            return retData;
        }

        /// <summary>
        /// 根据Sql及参数查询,获取数据存储结果,一般可和XPDataView联合使用，这需要提前建立XPDataView,并手动建立例
        /// 如：
        ///  XPDataView xpdv = new XPDataView();
        ///   xpdvProduct.Properties.AddRange(new DataViewProperty[] {
        ///                 new DataViewProperty ("ProductOID",typeof(int)),
        ///                 new DataViewProperty ("ActualWidth",typeof(decimal)),
        ///                 
        /// xpdv.LoadData(selectedData对象)
        ///  
        /// </summary>
        /// <param name="strSQL"></param>
        /// <param name="parameterNames"></param>
        /// <param name="parameterValues"></param>
        /// <returns></returns>
        public SelectedData ExecuteQuery(string strSQL)
        {
            SelectedData retData = null;
            using (UnitOfWork session = XPOUtils.CreateUnitOfWork(this.ConnectionString))
            {
                retData = session.ExecuteQuery(strSQL);
                session.DataLayer.Dispose();
                session.Disconnect();
                session.Dispose();
            }
            return retData;
        }
        /// <summary>
        /// 根据Sql查询语句返回一个查询结果 (单个数据表的结果)
        /// 
        ///   
        /// </summary>
        /// <param name="strSQL"></param>
        /// <param name="Parameters">查询参数,类似于存储过程中定义的变量,如:
        ///  如： new QueryParameter("@FromTime", typeof(DateTime), FromTime),
        /// </param>
        /// <returns></returns>
        public ITable ExecuteQuery(string strSQL, params QueryParameter[] Parameters)
        {
            ITable ret = null;
            CustomSqlQuery query = new CustomSqlQuery("query", strSQL);
            if (Parameters != null)
            {
                query.Parameters.AddRange(Parameters);
            }
            using (SqlDataSource ds = new SqlDataSource(new CustomStringConnectionParameters(this.ConnectionString)))
            {
                ds.Queries.Add(query);
                ds.Fill();
                if (ds.Result != null && ds.Result.Count() > 0)
                {
                    ret = ds.Result[0];
                }
            }
            return ret;
        }
        /// <summary>
        /// 执行SQL,返回第一行第一列的值 
        /// </summary>
        /// <param name="strSQL"></param>
        /// <returns></returns>
        public object ExecuteScalar(string strSQL)
        {
            object ret = null;
            using (UnitOfWork session = XPOUtils.CreateUnitOfWork(this.ConnectionString))
            {
                ret = session.ExecuteScalar(strSQL);
                session.DataLayer.Dispose();
                session.Disconnect();
                session.Dispose();
            }
            return ret;
        }

        /// <summary>
        /// 返回第一行一列的值 
        /// </summary>
        /// <param name="strSQL"></param>
        /// <param name="parameterNames"></param>
        /// <param name="parameterValues"></param>
        /// <returns></returns>
        public object ExecuteScalar(string strSQL, string[] parameterNames, object[] parameterValues)
        {
            object ret = null;
            using (UnitOfWork session = XPOUtils.CreateUnitOfWork(this.ConnectionString))
            {
                ret = session.ExecuteScalar(strSQL, parameterNames, parameterValues);
                session.DataLayer.Dispose();
                session.Disconnect();
                session.Dispose();
            }
            return ret;
        }

        /// <summary>
        /// 执行sql语句后，返回数据表影响的行数
        /// </summary>
        /// <param name="strSQL"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string strSQL)
        {
            int ret = 0;
            using (UnitOfWork session = XPOUtils.CreateUnitOfWork(this.ConnectionString))
            {
                ret = session.ExecuteNonQuery(strSQL);
                session.DataLayer.Dispose();
                session.Disconnect();
                session.Dispose();
            }
            return ret;
        }

        /// <summary>
        /// 执行sql语句后，返回数据表影响的行数
        /// </summary>
        /// <param name="strSQL"></param>
        /// <param name="parameterNames"></param>
        /// <param name="parameterValues"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string strSQL, QueryParameter[] parameters)
        {
            int ret = 0;
            using (UnitOfWork session = XPOUtils.CreateUnitOfWork(this.ConnectionString))
            {
                int length = parameters.Length;
                string[] parameterNames = new string[length];
                object[] parameterValues = new object[length];
                for (int i = 0; i < length; i++)
                {
                    parameterNames[i] = parameters[i].Name;
                    parameterValues[i] = parameters[i].Value;
                }
                ret = session.ExecuteNonQuery(strSQL, parameterNames, parameterValues);
                session.DataLayer.Dispose();
                session.Disconnect();
                session.Dispose();
            }
            return ret;
        }
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="strProcName"></param>
        /// <returns></returns>
        public SelectedData ExecuteSproc(string strProcName)
        {
            SelectedData retData = null;
            using (UnitOfWork session = XPOUtils.CreateUnitOfWork(this.ConnectionString))
            {
                retData = session.ExecuteSproc(strProcName);
                session.DataLayer.Dispose();
                session.Disconnect();
                session.Dispose();
            }

            return retData;
        }


        /// <summary>
        /// 执行带参数的存储过程
        /// 
        /// </summary>
        /// <param name="strProcName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public SelectedData ExecuteSproc(string strProcName,
            params DevExpress.Data.Filtering.OperandValue[] parameters)
        {
            SelectedData retData = null;
            using (UnitOfWork session = XPOUtils.CreateUnitOfWork(this.ConnectionString))
            {
                retData = session.ExecuteSproc(strProcName, parameters);
                session.DataLayer.Dispose();
                session.Disconnect();
                session.Dispose();
            }
            return retData;
        }

        /// <summary>
        ///  执行带参数的存储过程
        /// </summary>
        /// <param name="strProcName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public SelectedData ExecuteSprocParametrized(string strProcName, params SprocParameter[] parameters)
        {
            SelectedData retData = null;
            using (UnitOfWork session = XPOUtils.CreateUnitOfWork(this.ConnectionString))
            {
                retData = session.ExecuteSprocParametrized(strProcName, parameters);
                session.DataLayer.Dispose();
                session.Disconnect();
                session.Dispose();
            }
            return retData;
        }
        /// <summary>
        /// 创建当前 UnitOfWork
        /// </summary>
        /// <returns></returns>
        public UnitOfWork CreateCurrentUnitOfWork()
        {
            return XPOUtils.CreateUnitOfWork(this.ConnectionString);
        }

        /// <summary>
        /// 从某个XPO类中查找某个字段值是否存在
        /// 
        /// TestDM.User aa= session.FindObject<TestDM.User>(CriteriaOperator.Parse("UserCode='CH'"));
        /// </summary>
        /// <typeparam name="ClassType"> XPO类类型</typeparam>
        /// <param name="criterial">DevExpress.Data.Filtering.CriteriaOperator.Parse(strFilterString)</param>
        /// <returns></returns>
        public ClassType FindObject<ClassType>(DevExpress.Data.Filtering.CriteriaOperator criterial)
        {
            ClassType ct;

            using (UnitOfWork session = XPOUtils.CreateUnitOfWork(this.ConnectionString))
            {
                ct = session.FindObject<ClassType>(criterial);
            }

            return ct;
        }

        /// <summary>
        /// 从某个XPO类中查找某个字段值是否存在
        /// 
        /// TestDM.User aa= session.FindObject<TestDM.User>(CriteriaOperator.Parse("UserCode='CH'"));
        /// </summary>
        /// <typeparam name="ClassType"> XPO类类型</typeparam>
        /// <param name="strFilterString">如：UserCode='CH'</param>
        /// <returns></returns>
        public ClassType FindObject<ClassType>(string strFilterString)
        {
            ClassType ct;

            using (UnitOfWork session = XPOUtils.CreateUnitOfWork(this.ConnectionString))
            {
                ct = session.FindObject<ClassType>(DevExpress.Data.Filtering.CriteriaOperator.Parse(strFilterString));
            }

            return ct;
        }

        public ClassType FindObject<ClassType>(UnitOfWork session, string strFilterString)
        {
            return session.FindObject<ClassType>(DevExpress.Data.Filtering.CriteriaOperator.Parse(strFilterString));
        }

        public ClassType FindObject<ClassType>(UnitOfWork session, DevExpress.Data.Filtering.CriteriaOperator criterial)
        {
            return session.FindObject<ClassType>(criterial);
        }
        /// <summary>
        /// 将XPDataView转换为DataTable
        /// </summary>
        /// <param name="xpDataView">可按行与列名进行访问 dView[dView.Count-1]["UserName"]</param>
        /// <returns></returns>
        public System.Data.DataTable XPDataViewToDataTable(DevExpress.Xpo.XPDataView xpDataView)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            System.Data.DataColumn colunm = null;
            System.Data.DataRow row;
            //添加列属性
            foreach (DevExpress.Xpo.DataViewProperty item in xpDataView.Properties)
            {
                colunm = new System.Data.DataColumn(item.Name, item.ValueType);
                colunm.AllowDBNull = true;
                dt.Columns.Add(colunm);
            }
            //添加行值
            foreach (DevExpress.Xpo.DataViewRecord dataRow in xpDataView)
            {
                row = dt.NewRow();
                foreach (DevExpress.Xpo.DataViewProperty dataColunm in xpDataView.Properties)
                {
                    if (dataRow[dataColunm.Name] == null)
                    {
                        row[dataColunm.Name] = System.DBNull.Value;
                    }
                    else
                    {
                        row[dataColunm.Name] = dataRow[dataColunm.Name];
                    }
                }
                dt.Rows.Add(row);
            }
            return dt;
        }
    }
}
