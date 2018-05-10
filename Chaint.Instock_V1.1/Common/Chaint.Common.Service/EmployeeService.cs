using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.DataAccess.Sql.DataApi;
using DevExpress.DataAccess.Sql;
using Chaint.Common.Core.Utils;
using Chaint.Common.Core;
using Chaint.Common.Data;
using Chaint.Common.Interface;
namespace Chaint.Common.Service
{
    public class EmployeeService : IEmployeeService
    {
        public OperateResult GetUserInfo(Context ctx, string usercode)
        {
            OperateResult result = new OperateResult();
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT USERCODE,USERNAME,PASSWORD FROM T_USER ");
            List<QueryParameter> SqlParams = new List<QueryParameter>();
            if (!usercode.IsNullOrEmptyOrWhiteSpace())
            {
                sb.AppendLine(" WHERE USERCODE = @USERCODE ORDER BY USERCODE ");
                QueryParameter param = new QueryParameter("@USERCODE", typeof(string), usercode);
                SqlParams.Add(param);
            }
            result.ResultTable = DBService.ExecuteQuery(ctx, sb.ToString(), SqlParams.ToArray());
            result.IsSuccess = true;
            return result;
        }
        public bool ValidateUser(Context ctx, string usercode, string password)
        {
            if (usercode.IsNullOrEmptyOrWhiteSpace() || password.IsNullOrEmptyOrWhiteSpace())
                return false;
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT USERCODE,USERNAME,PASSWORD FROM T_USER ");
            sb.AppendLine(" WHERE USERCODE = @USERCODE ");
            sb.AppendLine(" AND PASSWORD = @PASSWORD ORDER BY USERCODE ");
            List<QueryParameter> SqlParams = new List<QueryParameter>();
            SqlParams.Add(new QueryParameter("@USERCODE", typeof(string), usercode));
            SqlParams.Add(new QueryParameter("@PASSWORD", typeof(string), password));
            ITable result = 
                DBService.ExecuteQuery(ctx, sb.ToString(), SqlParams.ToArray());
            if (result.Count() <= 0) return false;
            return true;
        }
    }
}
