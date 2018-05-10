using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Chaint.Common.Core;
using Chaint.Common.Data;
using Chaint.Common.App;
using Chaint.Common.Interface;
using Chaint.Common.Interface.Business;
using Chaint.Common.Core.Utils;

namespace Chaint.Instock.Service
{
    public class StockAreaDataService : IStockAreaDataService
    {
        public OperateResult GetStockAreaInfo(Context ctx)
        {
            OperateResult result = new OperateResult();
            StringBuilder sb = new StringBuilder();
            sb.Append(@" SELECT DISTINCT STK.FNUMBER AS FSTOCK, STK.FNAME AS FSTOCKNAME,SAE.FSTOCKAREANUMBER,SAE.FSTOCKAREANAME,SAE.FTOTALCAPACITY
                        FROM T_AUTOSCAN_STOCKAREA SA INNER JOIN  T_AUTOSCAN_STOCKAREAENTRY SAE 
                        ON SA.FID = SAE.FHEADID 
	                    LEFT JOIN T_AUTOSCAN_STOCK STK ON SA.FSTOCK =STK.FNUMBER ");
            result.ResultTable = DBService.ExecuteQuery(ctx, sb.ToString());
            result.IsSuccess = true;
            return result;
        }
        public OperateResult GetProductInfo(Context ctx)
        {
            OperateResult result = new OperateResult();
            StringBuilder sb = new StringBuilder();
            sb.Append(@" SELECT WLBH,WLMC FROM [DBO].[CT_WLZD] ");
            result.ResultDataTable = DBService.ExecuteQueryWithDataTable(ctx, sb.ToString());
            result.IsSuccess = true;
            return result;
        }
        public OperateResults GetStockDataInfo(Context ctx,string product,string paperType, string specification, string[] stock, string[] stockArea, DateTime beginDate,DateTime endDate
            , int[] statusArray)
        {
            string sqlFormat = @"
            SELECT 0 AS FCHECK,SADE.FENTRYID,SADE.FSTOCKAREAPLAN,SADE.FPRODUCT,SADE.FAMOUNT,SADE.FDATE,SADE.FPLANAMOUNT,SADE.FSTATUS,SADE.FSTOCK,
            SADE.FSTOCKAREA,SADE.FPAPERTYPE,SADE.FPAPERGRADE,SADE.FCERTIFICATION,SADE.FPACKTYPE,SADE.FTRANSPORTTYPE,SADE.FCOLOR,
            SADE.FCOREDIAMETERORREAM,SADE.FDIAMETERORSLIDES,SADE.FLENGTH,SADE.FTRADEMARKSTYLE,SADE.FSPECPRODNAME,
            SADE.FSPECCUSTNAME,SADE.FWEIGHTMODE,SADE.FSPECIFICATION,SADE.FMEMO,SADE.FPONUMBER,FDELIVERDATE,SADE.FUSEDCAPACITY,
            SADE.FNAME,SADE.FCUSTOMER,SADE.FSTOCKDATE,SADE.FINSTOCKAREA,SADE.FOUTAMOUNT,SADE.FWEIGHT,SADE.FOUTWEIGHT,
            SADE.FTOTALCAPACITY,SADE.FSPCUSTOMER
             FROM T_AUTOSCAN_STOCKAREADATAENTRY SADE
             LEFT JOIN 
             (SELECT SA.FSTOCK,SAE.FSTOCKAREANUMBER,SAE.FTOTALCAPACITY
              FROM T_AUTOSCAN_STOCKAREA SA INNER JOIN T_AUTOSCAN_STOCKAREAENTRY SAE ON SA.FID= SAE.FHEADID) TEMP
              ON SADE.FSTOCK = TEMP.FSTOCK AND SADE.FSTOCKAREA=TEMP.FSTOCKAREANUMBER
            WHERE  SADE.FPAPERTYPE = @FPAPERTYPE {0}
            ORDER BY SADE.FDATE DESC;";
            StringBuilder sbFilter = new StringBuilder();
            List<SqlParameter> parameters = new List<SqlParameter>();
            SqlParameter para1 = new SqlParameter("@FPAPERTYPE", DbType.String);
            para1.Value = paperType;
            parameters.Add(para1);
            if (!specification.IsNullOrEmptyOrWhiteSpace())
            {
                sbFilter.Append(" AND SADE.FSPECIFICATION = @FSPECIFICATION ");
                SqlParameter para = new SqlParameter("@FSPECIFICATION", DbType.String);
                para.Value = specification;
                parameters.Add(para);
            }
            if (beginDate != null && beginDate != DateTime.MinValue && beginDate != DateTime.Parse("1753/1/1 12:00:00"))
            {
                sbFilter.Append(" AND SADE.FDATE >= @FBEGINDATE ");
                SqlParameter para = new SqlParameter("@FBEGINDATE", DbType.DateTime);
                para.Value = beginDate;
                parameters.Add(para);
            }
            if (endDate != null && endDate != DateTime.MinValue && endDate != DateTime.Parse("1753/1/1 12:00:00"))
            {
                sbFilter.Append(" AND SADE.FDATE <= @FENDDATE ");
                SqlParameter para = new SqlParameter("@FENDDATE", DbType.DateTime);
                para.Value = endDate;
                parameters.Add(para);
            }
            if (statusArray.Length <= 1)
            {
                sbFilter.Append(" AND SADE.FSTATUS= @FSTATUS ");
                SqlParameter item = new SqlParameter("@FSTATUS", DbType.String);
                item.Value = statusArray[0];
                parameters.Add(item);
            }
            else
            {
                string format = " AND SADE.FSTATUS IN ({0})";
                StringBuilder sbIn = new StringBuilder();
                int i = 0;
                foreach (int status in statusArray)
                {
                    i++;
                    string paraName = string.Format("@FSTATUS{0}", i);
                    sbIn.Append(paraName);
                    sbIn.Append(',');
                    SqlParameter item = new SqlParameter(paraName, DbType.Int32);
                    item.Value = status;
                    parameters.Add(item);
                }
                sbFilter.AppendFormat(format, sbIn.ToString().TrimEnd(','));
            }
            if (statusArray.Length == 1 && statusArray[0] == 3)
            {
                sbFilter.Append(" AND SADE.FAMOUNT >0 ");
            }
            if (stock.Length == 1)
            {
                sbFilter.Append(" AND SADE.FSTOCK= @FSTOCK ");
                SqlParameter item = new SqlParameter("@FSTOCK", DbType.String);
                item.Value = stock[0];
                parameters.Add(item);
            }
            else if(stock.Length > 1)
            {
                string format = " AND SADE.FSTOCK IN ({0})";
                StringBuilder sbIn = new StringBuilder();
                int j = 0;
                foreach (string stockName in stock)
                {
                    j++;
                    string paraName = string.Format("@FSTOCK{0}", j);
                    sbIn.Append(paraName);
                    sbIn.Append(',');
                    SqlParameter item = new SqlParameter(paraName, DbType.String);
                    item.Value = stockName;
                    parameters.Add(item);
                }
                sbFilter.AppendFormat(format, sbIn.ToString().TrimEnd(','));
            }
            if (stockArea.Length == 1)
            {
                sbFilter.Append(" AND SADE.FSTOCKAREA= @FSTOCKAREA ");
                SqlParameter item = new SqlParameter("@FSTOCKAREA", DbType.String);
                item.Value = stockArea[0];
                parameters.Add(item);
            }
            else if(stockArea.Length > 1)
            {
                string format = " AND SADE.FSTOCKAREA IN ({0}) ";
                StringBuilder sbIn = new StringBuilder();
                int k = 0;
                foreach (string value in stockArea)
                {
                    k++;
                    string paraName = string.Format("@FSTOCKAREA{0}", k);
                    sbIn.Append(paraName);
                    sbIn.Append(',');
                    SqlParameter item = new SqlParameter(paraName, DbType.String);
                    item.Value = value.Trim();
                    parameters.Add(item);
                }
                sbFilter.AppendFormat(format, sbIn.ToString().TrimEnd(','));
            }
            if (!product.IsNullOrEmptyOrWhiteSpace())
            {
                SqlParameter item = new SqlParameter("@FPRODUCT", DbType.String);
                item.Value = product;
                parameters.Add(item);
                sbFilter.AppendFormat(" AND SADE.FPRODUCT = @FPRODUCT ");
            }
            string sql = string.Format(sqlFormat, sbFilter.ToString().TrimEnd(','));
            OperateResults result
                = AppServiceContext.GetService<IDBAccessService>(ctx).ExecuteQuery(ctx, sql, new string[] { "T1"}, parameters.ToArray());
            return result;
        }
    }
}
