
using System.Collections.Generic;
using DevExpress.DataAccess.Sql;
using Chaint.Common.Interface.Business;
using Chaint.Common.Core;
using Chaint.Common.Data;
using System.Data.SqlClient;

namespace Chaint.Instock.Service
{
    public class StockAreaPlanService : IStockAreaPlanService
    {
        /// <summary>
        /// 获取产品备注(特殊客户)
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public OperateResult GetSpCustomer(Context ctx)
        {
            OperateResult result = new OperateResult();
            string strSql = @"SELECT NAME FROM CT_ZDYZD WHERE ZDLX='TSKH'";
            result.ResultDataTable = DBService.ExecuteQueryWithDataTable(ctx, strSql);
            result.IsSuccess = true;
            return result;
        }
        /// <summary>
        /// 获取纸芯数据
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public OperateResult GetCoreDiameter(Context ctx)
        {
            OperateResult result = new OperateResult();
            string strSql = @"SELECT COREDIAMETER FROM PAPER_COREDIAMETER ORDER BY COREDIAMETER";
            result.ResultDataTable = DBService.ExecuteQueryWithDataTable(ctx, strSql);
            result.IsSuccess = true;
            return result;
        }
        /// <summary>
        /// 获取产品等级
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public OperateResult GetPaperGrade(Context ctx)
        {
            OperateResult result = new OperateResult();
            string strSql = @"SELECT GRADEDESC FROM PAPER_GRADE ORDER BY GRADE";
            result.ResultDataTable = DBService.ExecuteQueryWithDataTable(ctx, strSql);
            result.IsSuccess = true;
            return result;
        }
        /// <summary>
        /// 获取产品等级
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public OperateResult GetPaperCertification(Context ctx)
        {
            OperateResult result = new OperateResult();
            string strSql = @"SELECT PAPERCERT FROM PAPER_CERTIFICATION ORDER BY ONLYID";
            result.ResultDataTable = DBService.ExecuteQueryWithDataTable(ctx, strSql);
            result.IsSuccess = true;
            return result;
        }
        /// <summary>
        /// 获取包装方式
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="PaperType"></param>
        /// <returns></returns>
        public OperateResult GetPackType(Context ctx, string PaperType)
        {
            OperateResult result = new OperateResult();
            string strSql = @"SELECT PACKTYPE FROM PAPER_PACKTYPE 
                               WHERE TYPE =@PAPERTYPE ORDER BY ONLYID";
            List<SqlParameter> SqlParams = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@PAPERTYPE", System.Data.SqlDbType.VarChar);
            param.Value = PaperType;
            SqlParams.Add(param);
            result.ResultDataTable = DBService.ExecuteQueryWithDataTable(ctx, strSql, SqlParams.ToArray());
            result.IsSuccess = true;
            return result;
        }
        /// <summary>
        /// 获取夹板包装
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public OperateResult GetTransportType(Context ctx)
        {
            OperateResult result = new OperateResult();
            string strSql = @"SELECT PLYWOOKDPACK AS TRANSPORTTYPE FROM PAPER_PLYWOOKDPACK ORDER BY ONLYID";
            result.ResultDataTable = DBService.ExecuteQueryWithDataTable(ctx, strSql);
            result.IsSuccess = true;
            return result;
        }
        /// <summary>
        /// 获取令数
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public OperateResult GetReam(Context ctx)
        {
            OperateResult result = new OperateResult();
            string strSql = @"SELECT DISTINCT TOP 30 MAX(TEMP.ONLYID) AS ONLYID, TEMP.SHEETREAMS 
                                FROM (SELECT ONLYID,SP.SHEETREAMS FROM SHEET_PRODUCT SP) TEMP 
                                GROUP BY  TEMP.SHEETREAMS  ORDER BY ONLYID DESC";
            result.ResultDataTable = DBService.ExecuteQueryWithDataTable(ctx, strSql);
            result.IsSuccess = true;
            return result;
        }
        /// <summary>
        /// 获取卷筒直径
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public OperateResult GetRollDiameter(Context ctx)
        {
            OperateResult result = new OperateResult();
            string strSql = @"SELECT DISTINCT TOP 30 MAX(TEMP.ONLYID) AS ONLYID, TEMP.DIAMETER 
                                FROM (SELECT ONLYID,RP.DIAMETER FROM ROLL_PRODUCT RP) TEMP 
                                GROUP BY  TEMP.DIAMETER  ORDER BY ONLYID DESC";
            result.ResultDataTable = DBService.ExecuteQueryWithDataTable(ctx, strSql);
            result.IsSuccess = true;
            return result;
        }
        /// <summary>
        /// 获取卷筒线长
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public OperateResult GetRollLength(Context ctx)
        {
            OperateResult result = new OperateResult();
            string strSql = @"SELECT DISTINCT TOP 30 MAX(TEMP.ONLYID) AS ONLYID, TEMP.LENGTH 
                                FROM (SELECT ONLYID,RP.LENGTH FROM ROLL_PRODUCT RP) TEMP 
                                GROUP BY  TEMP.LENGTH  ORDER BY ONLYID DESC";
            result.ResultDataTable = DBService.ExecuteQueryWithDataTable(ctx, strSql);
            result.IsSuccess = true;
            return result;
        }
        /// <summary>
        /// 获取产品色相
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public OperateResult GetPaperColor(Context ctx)
        {
            OperateResult result = new OperateResult();
            string strSql = @"SELECT PC.COLORDESC FROM [DBO].[PAPER_COLOR] PC";
            result.ResultDataTable = DBService.ExecuteQueryWithDataTable(ctx, strSql);
            result.IsSuccess = true;
            return result;
        }
        /// <summary>
        /// 获取规格
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public OperateResult GetSpecification(Context ctx,string paperType)
        {
            OperateResult result = new OperateResult();
            string strSql = string.Empty;
            if (paperType == "1")
            {
                strSql = @"SELECT DISTINCT TOP 30 MAX(TEMP.ONLYID) AS ONLYID, TEMP.SHEETWIDTH AS FSPECIFICATION
FROM (SELECT ONLYID,DBO.TRIMZERO(RP.WIDTHLABEL )AS SHEETWIDTH 
FROM ROLL_PRODUCT RP) TEMP 
GROUP BY  TEMP.SHEETWIDTH  ORDER BY ONLYID DESC";
            }
            else if (paperType == "2")
            {
                strSql = @"SELECT DISTINCT TOP 30 MAX(TEMP.ONLYID) AS ONLYID, TEMP.SHEETWIDTH+'-'+TEMP.SHEETLENGTH AS FSPECIFICATION
FROM (SELECT ONLYID,DBO.TRIMZERO(SP.WIDTHLABEL )AS SHEETWIDTH,
DBO.TRIMZERO(SP.LENGHTLABEL ) AS SHEETLENGTH FROM SHEET_PRODUCT SP WHERE  
SP.WIDTHLABEL IS NOT NULL AND SP.LENGHTLABEL IS NOT NULL)TEMP 
GROUP BY  TEMP.SHEETWIDTH,TEMP.SHEETLENGTH  ORDER BY ONLYID DESC";
            }
            result.ResultDataTable = DBService.ExecuteQueryWithDataTable(ctx, strSql);
            result.IsSuccess = true;
            return result;
        }
        /// <summary>
        /// 获取专用客户
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public OperateResult GetSpecCustomerName(Context ctx)
        {
            OperateResult result = new OperateResult();
            string strSql = @"SELECT SPECCUSTNAME FROM PAPER_SPECCUSTNAME ORDER BY ONLYID";
            result.ResultDataTable = DBService.ExecuteQueryWithDataTable(ctx, strSql);
            result.IsSuccess = true;
            return result;
        }
        /// <summary>
        /// 获取专用产品
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public OperateResult GetSpecProductName(Context ctx)
        {
            OperateResult result = new OperateResult();
            string strSql = @"SELECT SPECPRODNAME FROM PAPER_SPECPRODNAME ORDER BY ONLYID";
            result.ResultDataTable = DBService.ExecuteQueryWithDataTable(ctx, strSql);
            result.IsSuccess = true;
            return result;
        }
        /// <summary>
        /// 获取合格证样式
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public OperateResult GetTrademarkStyle(Context ctx)
        {
            OperateResult result = new OperateResult();
            string strSql = @"SELECT TRADEMARKSTYLE FROM PAPER_TRADEMARKSTYLE";
            result.ResultDataTable = DBService.ExecuteQueryWithDataTable(ctx, strSql);
            result.IsSuccess = true;
            return result;
        }
        /// <summary>
        /// 获取计重方式
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public OperateResult GetWeightMode(Context ctx)
        {
            OperateResult result = new OperateResult();
            string strSql = @"SELECT WEIGHTMODE,WEIGHTMODEDESC FROM PAPER_WEIGHTMODE";
            result.ResultDataTable = DBService.ExecuteQueryWithDataTable(ctx, strSql);
            result.IsSuccess = true;
            return result;
        }
        /// <summary>
        /// 获取令张数
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public OperateResult GetSlidesOfReam(Context ctx)
        {
            OperateResult result = new OperateResult();
            string strSql = @"SELECT DISTINCT TOP 30 MAX(TEMP.ONLYID) AS ONLYID, TEMP.SLIDESOFREAM 
                                FROM (SELECT ONLYID,SP.SLIDESOFREAM FROM SHEET_PRODUCT SP) TEMP 
                                GROUP BY  TEMP.SLIDESOFREAM  ORDER BY ONLYID DESC";
            result.ResultDataTable = DBService.ExecuteQueryWithDataTable(ctx, strSql);
            result.IsSuccess = true;
            return result;
        }
    }
}
