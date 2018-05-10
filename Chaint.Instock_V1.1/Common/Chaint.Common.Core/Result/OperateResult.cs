using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DevExpress.DataAccess.Sql.DataApi;
namespace Chaint.Common.Core
{
    public class OperateResult
    {
        public ITable ResultTable { get; set; }
        public DataTable  ResultDataTable { get; set; }
        public int AffectRow { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
    public class OperateResults
    {
        public DataSet ResultData { get; set; }
        public int AffectRow { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}
