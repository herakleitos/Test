
using System.Collections.Generic;
using System.Text;
using DevExpress.DataAccess.Sql;
using Chaint.Common.Core.Utils;
using Chaint.Common.Core;
using Chaint.Common.Data;
using Chaint.Common.Interface.Business;
namespace Chaint.Instock.Service
{
    public class StockInAutoScanService : IStockInAutoScan
    {
        public OperateResult GetDefaultDisplay(Context ctx, string dispType)
        {
            OperateResult result = new OperateResult();
            StringBuilder sb = new StringBuilder();
            sb.Append(@" SELECT DEFINE1 AS FUSER,DEFINE2 AS FSHIFT , DEFINE3 AS FSHIFTTIME, DEFINE4 AS FDATE ,
                            DEFINE5 AS FBUSINESS, DEFINE6 AS FORG, DEFINE7 AS FREMARK
                            FROM T_DEFAULTDISPLAY ");
            List<QueryParameter> SqlParams = new List<QueryParameter>();
            if (!dispType.IsNullOrEmptyOrWhiteSpace())
            {
                sb.AppendLine("WHERE DISPTYPE = @DISPTYPE ORDER BY DISPTYPE ");
                QueryParameter param = new QueryParameter("@DISPTYPE", typeof(string), dispType);
                SqlParams.Add(param);
            }
            result.ResultTable = DBService.ExecuteQuery(ctx, sb.ToString(), SqlParams.ToArray());
            result.IsSuccess = true;
            return result;
        }
        public OperateResult GetFactoryOrg(Context ctx, bool isChoose, bool isLocal)
        {
            OperateResult result = new OperateResult();
            StringBuilder sb = new StringBuilder();
            if (isChoose)
            {
                sb.Append(@" SELECT FACTORYNAME,MACHINEID,FACTORYABBR,FACTORYNAME,FACTORYPHONE,
                            ONLYID,ISCHOOSE,ISLOCAL FROM T_FACTORY WHERE isChoose='1' ");
            }
            else if (isLocal)
            {
                sb.Append(@" SELECT FACTORYNAME,MACHINEID,FACTORYABBR,FACTORYNAME,FACTORYPHONE,
                            ONLYID,ISCHOOSE,ISLOCAL FROM T_FACTORY WHERE ISLOCAL ='1' ");
            }
            result.ResultTable = DBService.ExecuteQuery(ctx, sb.ToString());
            result.IsSuccess = true;
            return result;
        }
        public OperateResult GetBusinessType(Context ctx, string businessCode)
        {
            OperateResult result = new OperateResult();
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT ONLYID,BUSINESSCODE,BUSINESSNAME,BUSINESSTYPE  FROM T_BUSINESS_TYPE ");
            List<QueryParameter> SqlParams = new List<QueryParameter>();
            if (!businessCode.IsNullOrEmptyOrWhiteSpace())
            {
                sb.Append(@" WHERE BUSINESSTYPE = @BUSINESSTYPE ");
                QueryParameter param = new QueryParameter("@BUSINESSTYPE", typeof(string), businessCode);
                SqlParams.Add(param);
            }
            result.ResultTable = DBService.ExecuteQuery(ctx, sb.ToString(), SqlParams.ToArray());
            result.IsSuccess = true;
            return result;
        }
        public OperateResult GetShiftInfo(Context ctx, string shiftName)
        {
            OperateResult result = new OperateResult();
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT SHIFTCODE,SHIFTNAME FROM T_SHIFT ");
            List<QueryParameter> SqlParams = new List<QueryParameter>();
            if (!shiftName.IsNullOrEmptyOrWhiteSpace())
            {
                sb.Append(@" WHERE SHIFTNAME = @SHIFTNAME ");
                QueryParameter param = new QueryParameter("@SHIFTNAME", typeof(string), shiftName);
                SqlParams.Add(param);
            }
            sb.Append(@" ORDER BY SHIFTCODE ");
            result.ResultTable = DBService.ExecuteQuery(ctx, sb.ToString(), SqlParams.ToArray());
            result.IsSuccess = true;
            return result;
        }
    }
}
