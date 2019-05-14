using DataSync.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSync
{
    public abstract class DataOperator
    {
        private DataAccessor _accessor;
        public DataOperator(DataAccessor dataAccessor)
        {
            _accessor = dataAccessor;
        }
        public DataAccessor dataAccessor {
            get { return _accessor; }
        }
        public virtual int WriteData(Entity data)
        {
            return 1;
        }
        public virtual int InsertData(Entity data)
        {
            return 1;
        }
        public virtual int UpdateData(Entity data)
        {
            return 1;
        }
        public virtual bool IsExist(string tableName, string id)
        {
            string sql = @"SELECT 1 FROM  {0} WHERE DFSJZJ=@DFSJZJ;";
            List<DbParameter> parameters = new List<DbParameter>();
            parameters.Add(dataAccessor.CreateParameter("@DFSJZJ", id));
            DataSet result = dataAccessor.ExecuteQuery(string.Format(sql, tableName), new string[] { "T1" }, parameters.ToArray());
            if (result == null || result.Tables == null || result.Tables.Count <= 0 || result.Tables["T1"].Rows.Count <= 0) return false;
            return true;
        }
    }
}
