using System;
using System.Data;
using DevExpress.XtraEditors;
using Chaint.Common.Core;
using Chaint.Common.Core.Utils;
using Chaint.Instock.Core;
using Chaint.Instock.ServiceHelper;
using Chaint.Common.Entity.Utils;
using System.Collections.Generic;
using System.Linq;
using DevExpress.XtraGrid;
using Chaint.Common.ServiceHelper;
using System.Data.SqlClient;
using System.Text;
using DevExpress.XtraGrid.Views.Grid;

namespace Chaint.Instock.Business.PlugIns
{
    partial class StockAreaDataQueryPlugIn
    {
        private void GetData()
        {
            if (Validate())
            {
                DateTime beginDate = this.View.GetValue<DateTime>(Const_StockAreaDataQuery.Head_Field_FBeginDate);
                DateTime endDate = this.View.GetValue<DateTime>(Const_StockAreaDataQuery.Head_Field_FEndDate);
                string[] stocks = this.View.GetValue<string>(Const_StockAreaDataQuery.Head_Field_FStock).Split(',');
                string[] stockList = stocks.Where(w => !w.IsNullOrEmptyOrWhiteSpace()).ToArray();
                string[] stockAreas = this.View.GetValue<string>(Const_StockAreaDataQuery.Head_Field_FStockArea).Split(',');
                string[] stockAreaList = stockAreas.Where(w => !w.IsNullOrEmptyOrWhiteSpace()).ToArray();
                string sqlFormt = @"SELECT 
                    MTRL.WLMC AS FPRODUCT,
                    STOCK.FNAME AS FSTOCK,
                    TEMP0.FSTOCKAREANAME AS FSTOCKAREA,
                    CASE SADE.FPAPERTYPE WHEN '1' THEN '卷筒纸' WHEN '2' THEN '平板纸' END AS FPAPERTYPE ,
                    SADE.FSPECIFICATION,
                    SADE.FPAPERGRADE,
                    SADE.FCERTIFICATION,
                    SADE.FTRANSPORTTYPE,
                    SADE.FCOREDIAMETERORREAM,
                    SADE.FDIAMETERORSLIDES,
                    SADE.FLENGTH,
                    SADE.FTRADEMARKSTYLE,
                    SADE.FSPECCUSTNAME,
                    SADE.FSPECPRODNAME,
                    SADE.FPACKTYPE,
                    SADE.FDELIVERDATE,
                    SADE.FWEIGHTMODE,
                    SADE.FCOLOR,
                    SADE.FPONUMBER,
                    SADE.FMEMO,
                    SADE.FSTOCKDATE,
                    SADE.FAMOUNT,
                    SADE.FWEIGHT,
                    TEMP0.FTOTALCAPACITY,
                    SADE.FDATE,
                    ISNULL(TEMP0.FTOTALCAPACITY,0)- SADE.FAMOUNT AS FUSEDCAPACITY           
                    FROM T_AUTOSCAN_STOCKAREADATAENTRY SADE
                    INNER JOIN T_AUTOSCAN_STOCK STOCK ON SADE.FSTOCK = STOCK.FNUMBER
                    LEFT JOIN T_AUTOSCAN_STOCKAREAPLAN SAP ON SAP.FNUMBER = SADE.FSTOCKAREAPLAN 
                    LEFT JOIN [CT_WLZD] MTRL ON SADE.FPRODUCT = MTRL.WLBH
                    INNER JOIN (SELECT SA.FSTOCK,SAE.FSTOCKAREANUMBER,SAE.FSTOCKAREANAME,SAE.FTOTALCAPACITY
					    FROM T_AUTOSCAN_STOCKAREAENTRY SAE INNER JOIN T_AUTOSCAN_STOCKAREA SA ON SAE.FHEADID
					=SA.FID) TEMP0 ON SADE.FSTOCK= TEMP0.FSTOCK AND SADE.FSTOCKAREA= TEMP0.FSTOCKAREANUMBER
                    WHERE SADE.FSTATUS=3 AND SADE.FAMOUNT>0 {0} ORDER BY SADE.FPRODUCT,SADE.FSPECIFICATION,SADE.FSTOCKDATE DESC";
                StringBuilder sbFilter = new StringBuilder();
                List<SqlParameter> parameters = new List<SqlParameter>();
                if (beginDate != null && beginDate != DateTime.MinValue&&beginDate!=DateTime.Parse("1753/1/1 12:00:00"))
                {
                    sbFilter.Append(" AND SADE.FDATE >= @FBEGINDATE ");
                    SqlParameter para = new SqlParameter("@FBEGINDATE", DbType.DateTime);
                    para.Value = beginDate;
                    parameters.Add(para);
                }
                if (endDate != null && endDate != DateTime.MinValue && beginDate != DateTime.Parse("1753/1/1 12:00:00"))
                {
                    sbFilter.Append(" AND SADE.FDATE <= @FENDDATE ");
                    SqlParameter para = new SqlParameter("@FENDDATE", DbType.DateTime);
                    para.Value = endDate;
                    parameters.Add(para);
                }

                if (stockList.Length == 1)
                {
                    sbFilter.Append(" AND SADE.FSTOCK= @FSTOCK ");
                    SqlParameter item = new SqlParameter("@FSTOCK", DbType.String);
                    item.Value = stocks[0];
                    parameters.Add(item);
                }
                else if (stockList.Length > 1)
                {
                    string format = " AND SADE.FSTOCK IN ({0})";
                    StringBuilder sbIn = new StringBuilder();
                    int i = 0;
                    foreach (string stock in stocks)
                    {
                        i++;
                        string paraName = string.Format("@FSTOCK{0}", i);
                        sbIn.Append(paraName);
                        sbIn.Append(',');
                        SqlParameter item = new SqlParameter(paraName, DbType.String);
                        item.Value = stock;
                        parameters.Add(item);
                    }
                    sbFilter.AppendFormat(format, sbIn.ToString().TrimEnd(','));
                }
                if (stockAreaList.Length == 1)
                {
                    sbFilter.Append(" AND SADE.FSTOCKAREA= @FSTOCKAREA ");
                    SqlParameter item = new SqlParameter("@FSTOCKAREA", DbType.String);
                    item.Value = stockAreas[0];
                    parameters.Add(item);
                }
                else if (stockAreaList.Length > 1)
                {
                    string format = " AND SADE.FSTOCKAREA IN ({0}) ";
                    StringBuilder sbIn = new StringBuilder();
                    int i = 0;
                    foreach (string stockarea in stockAreas)
                    {
                        i++;
                        string paraName = string.Format("@FSTOCKAREA{0}", i);
                        sbIn.Append(paraName);
                        sbIn.Append(',');
                        SqlParameter item = new SqlParameter(paraName, DbType.String);
                        item.Value = stockarea.Trim();
                        parameters.Add(item);
                    }
                    sbFilter.AppendFormat(format, sbIn.ToString().TrimEnd(','));
                }
                string product = this.View.GetValue<string>(Const_StockAreaDataQuery.Head_Field_FProduct);
                if (!product.IsNullOrEmptyOrWhiteSpace())
                {
                    SqlParameter item = new SqlParameter("@FPRODUCT", DbType.String);
                    item.Value = product;
                    parameters.Add(item);
                    sbFilter.AppendFormat(" AND SADE.FPRODUCT = @FPRODUCT ");
                }
                int paperType = this.View.GetValue<int>(Const_StockAreaDataQuery.Head_Field_FPaperType);
                if (paperType > 0)
                {
                    SqlParameter item = new SqlParameter("@FPAPERTYPE", DbType.Int32);
                    item.Value = paperType;
                    parameters.Add(item);
                    sbFilter.AppendFormat(" AND SADE.FPAPERTYPE = @FPAPERTYPE ");
                }
                string specification = this.View.GetValue<string>(Const_StockAreaDataQuery.Head_Field_FSpecification);
                if (!specification.IsNullOrEmptyOrWhiteSpace())
                {
                    SqlParameter item = new SqlParameter("@FSPECIFICATION", DbType.String);
                    item.Value = specification;
                    parameters.Add(item);
                    sbFilter.AppendFormat(" AND SADE.FSPECIFICATION = @FSPECIFICATION ");
                }
                string paperGrade = this.View.GetValue<string>(Const_StockAreaDataQuery.Head_Field_FPaperGrade);
                if (!paperGrade.IsNullOrEmptyOrWhiteSpace())
                {
                    SqlParameter item = new SqlParameter("@FPAPERGRADE", DbType.String);
                    item.Value = paperGrade;
                    parameters.Add(item);
                    sbFilter.AppendFormat(" AND SADE.FPAPERGRADE = @FPAPERGRADE ");
                }
                string certification = this.View.GetValue<string>(Const_StockAreaDataQuery.Head_Field_FCertification);
                if (!certification.IsNullOrEmptyOrWhiteSpace())
                {
                    SqlParameter item = new SqlParameter("@FCERTIFICATION", DbType.String);
                    item.Value = certification;
                    parameters.Add(item);
                    sbFilter.AppendFormat(" AND SADE.FCERTIFICATION = @FCERTIFICATION ");
                }
                string transportType = this.View.GetValue<string>(Const_StockAreaDataQuery.Head_Field_FTransportType);
                if (!transportType.IsNullOrEmptyOrWhiteSpace())
                {
                    SqlParameter item = new SqlParameter("@FTRANSPORTTYPE", DbType.String);
                    item.Value = transportType;
                    parameters.Add(item);
                    sbFilter.AppendFormat(" AND SADE.FTRANSPORTTYPE = @FTRANSPORTTYPE ");
                }
                decimal coreDiameterOrReam = this.View.GetValue<decimal>(Const_StockAreaDataQuery.Head_Field_FCoreDiameterOrReam);
                if (coreDiameterOrReam > 0)
                {
                    SqlParameter item = new SqlParameter("@FCOREDIAMETERORREAM", DbType.Decimal);
                    item.Value = coreDiameterOrReam;
                    parameters.Add(item);
                    sbFilter.AppendFormat(" AND SADE.FCOREDIAMETERORREAM = @FCOREDIAMETERORREAM ");
                }
                decimal diameterOrReam = this.View.GetValue<decimal>(Const_StockAreaDataQuery.Head_Field_FDiameterOrSlides);
                if (diameterOrReam > 0)
                {
                    SqlParameter item = new SqlParameter("@FDIAMETERORSLIDES", DbType.Decimal);
                    item.Value = diameterOrReam;
                    parameters.Add(item);
                    sbFilter.AppendFormat(" AND SADE.FDIAMETERORSLIDES = @FDIAMETERORSLIDES ");
                }
                decimal length = this.View.GetValue<decimal>(Const_StockAreaDataQuery.Head_Field_FLength);
                if (length > 0)
                {
                    SqlParameter item = new SqlParameter("@FLENGTH", DbType.Decimal);
                    item.Value = length;
                    parameters.Add(item);
                    sbFilter.AppendFormat(" AND SADE.FLENGTH = @FLENGTH ");
                }
                string trademarkStyle = this.View.GetValue<string>(Const_StockAreaDataQuery.Head_Field_FTrademarkStyle);
                if (!trademarkStyle.IsNullOrEmptyOrWhiteSpace())
                {
                    SqlParameter item = new SqlParameter("@FTRADEMARKSTYLE", DbType.String);
                    item.Value = trademarkStyle;
                    parameters.Add(item);
                    sbFilter.AppendFormat(" AND SADE.FTRADEMARKSTYLE = @FTRADEMARKSTYLE ");
                }
                string specCustName = this.View.GetValue<string>(Const_StockAreaDataQuery.Head_Field_FSpecCustName);
                if (!specCustName.IsNullOrEmptyOrWhiteSpace())
                {
                    SqlParameter item = new SqlParameter("@FSPECCUSTNAME", DbType.String);
                    item.Value = specCustName;
                    parameters.Add(item);
                    sbFilter.AppendFormat(" AND SADE.FSPECCUSTNAME = @FSPECCUSTNAME ");
                }
                string specProdName = this.View.GetValue<string>(Const_StockAreaDataQuery.Head_Field_FSpecProdName);
                if (!specProdName.IsNullOrEmptyOrWhiteSpace())
                {
                    SqlParameter item = new SqlParameter("@FSPECPRODNAME", DbType.String);
                    item.Value = specProdName;
                    parameters.Add(item);
                    sbFilter.AppendFormat(" AND SADE.FSPECPRODNAME = @FSPECPRODNAME ");
                }
                string packType = this.View.GetValue<string>(Const_StockAreaDataQuery.Head_Field_FPackType);
                if (!packType.IsNullOrEmptyOrWhiteSpace())
                {
                    SqlParameter item = new SqlParameter("@FPACKTYPE", DbType.String);
                    item.Value = packType;
                    parameters.Add(item);
                    sbFilter.AppendFormat(" AND SADE.FPACKTYPE = @FPACKTYPE ");
                }
                DateTime deliverDate = this.View.GetValue<DateTime>(Const_StockAreaDataQuery.Head_Field_FDeliverDate);
                if (deliverDate != null && deliverDate != DateTime.MinValue && deliverDate != DateTime.Parse("1/1/1753 12:00:00"))
                {
                    SqlParameter bItem = new SqlParameter("@FBDELIVERDATE", DbType.DateTime);
                    bItem.Value = deliverDate.Date;
                    parameters.Add(bItem);
                    SqlParameter eItem = new SqlParameter("@FEDELIVERDATE", DbType.DateTime);
                    eItem.Value = deliverDate.Date.AddDays(1).AddSeconds(-1);
                    parameters.Add(eItem);
                    sbFilter.AppendFormat(" AND SADE.FDELIVERDATE BETWEEN @FBDELIVERDATE AND @FEDELIVERDATE ");
                }
                string weightMode = this.View.GetValue<string>(Const_StockAreaDataQuery.Head_Field_FWeightMode);
                if (!weightMode.IsNullOrEmptyOrWhiteSpace())
                {
                    SqlParameter item = new SqlParameter("@FWEIGHTMODE", DbType.String);
                    item.Value = weightMode;
                    parameters.Add(item);
                    sbFilter.AppendFormat(" AND SADE.FWEIGHTMODE = @FWEIGHTMODE ");
                }
                string color = this.View.GetValue<string>(Const_StockAreaDataQuery.Head_Field_FColor);
                if (!color.IsNullOrEmptyOrWhiteSpace())
                {
                    SqlParameter item = new SqlParameter("@FCOLOR", DbType.String);
                    item.Value = color;
                    parameters.Add(item);
                    sbFilter.AppendFormat(" AND SADE.FCOLOR = @FCOLOR ");
                }
                string poNumber = this.View.GetValue<string>(Const_StockAreaDataQuery.Head_Field_FPoNumber);
                if (!poNumber.IsNullOrEmptyOrWhiteSpace())
                {
                    SqlParameter item = new SqlParameter("@FPONUMBER", DbType.String);
                    item.Value = poNumber;
                    parameters.Add(item);
                    sbFilter.AppendFormat(" AND SADE.FPONUMBER = @FPONUMBER ");
                }
                string memo = this.View.GetValue<string>(Const_StockAreaDataQuery.Head_Field_FMemo);
                if (!memo.IsNullOrEmptyOrWhiteSpace())
                {
                    SqlParameter item = new SqlParameter("@FMEMO", DbType.String);
                    item.Value = memo;
                    parameters.Add(item);
                    sbFilter.AppendFormat(" AND SADE.FMEMO = @FMEMO ");
                }
                string spCustomer = this.View.GetValue<string>(Const_StockAreaDataQuery.Head_Field_FSpCustomer);
                if (!spCustomer.IsNullOrEmptyOrWhiteSpace())
                {
                    SqlParameter item = new SqlParameter("@FSPCUSTOMER", DbType.String);
                    item.Value = spCustomer;
                    parameters.Add(item);
                    sbFilter.AppendFormat(" AND SADE.FSPCUSTOMER = @FSPCUSTOMER ");
                }
                string sql = string.Format(sqlFormt, sbFilter.ToString());
                string[] tableName = new string[] { "T1" };
                OperateResults result = DBAccessServiceHelper.ExcuteQuery(this.Context, sql, tableName, parameters.ToArray());
                if (result.IsSuccess)
                {
                    GridControl gvMain = this.View.GetControl<GridControl>(Const_StockAreaDataQuery.Entry_Control);
                    gvMain.DataSource = result.ResultData.Tables["T1"];
                    this.View.GetControl<GridView>(Const_StockAreaDataQuery.Entry_Entry).BestFitColumns();
                }
            }
        }
        private bool Validate()
        {
            //DateTime beginDate = this.View.GetValue<DateTime>(Const_StockAreaDataQuery.Head_Field_FBeginDate);
            //DateTime endDate = this.View.GetValue<DateTime>(Const_StockAreaDataQuery.Head_Field_FEndDate);
            //if (beginDate == null || beginDate == DateTime.MinValue)
            //{
            //    ChaintMessageBox.Show("请选择查询开始日期！");
            //    return false;
            //}
            //if (endDate == null || endDate == DateTime.MinValue)
            //{
            //    ChaintMessageBox.Show("请选择查询结束日期！");
            //    return false;
            //}
            //string stock = this.View.GetValue<string>(Const_StockAreaDataQuery.Head_Field_FStock);
            //if (stock.IsNullOrEmptyOrWhiteSpace())
            //{
            //    ChaintMessageBox.Show("请选择查询仓库！");
            //    return false;
            //}
            //string stockArea = this.View.GetValue<string>(Const_StockAreaDataQuery.Head_Field_FStockArea);
            //if (stockArea.IsNullOrEmptyOrWhiteSpace())
            //{
            //    ChaintMessageBox.Show("请选择查询库区！");
            //    return false;
            //}
            return true;
        }
    }
}
