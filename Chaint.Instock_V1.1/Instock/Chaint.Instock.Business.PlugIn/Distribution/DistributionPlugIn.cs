using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.DataAccess.Sql;
using DevExpress.XtraBars;
using Chaint.Common.ServiceHelper;
using Chaint.Common.BasePlugIn;
using Chaint.Common.Core.EventArgs;
using Chaint.Common.Core;
using Chaint.Common.Core.Sql;
using Chaint.Common.Core.Utils;
using Chaint.Instock.Core;
using Chaint.Instock.Business.View;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Columns;

namespace Chaint.Instock.Business.PlugIns
{
    public partial class DistributionPlugIn : AbstractBillPlugIn
    {
        XtraForm form;
        public DistributionPlugIn(DistributionView view) : base(view)
        {
            form = this.View.GetControl<XtraForm>(Const_Distribution.Base_Form);
        }
        public override void OnSetBusinessInfo()
        {
            base.OnSetBusinessInfo();
            InitView();
        }

        public override void OnBind()
        {
            base.OnBind();
            BindProduct();
            BindStock();
            BindDataPlan();
            DateTime beginDate = DateTime.Now;
            DateTime endDate = DateTime.Now;
            this.View.SetValue(Const_Distribution.Query_FBeginDate, beginDate);
            this.View.SetValue(Const_Distribution.Query_FEndDate, endDate);
        }
        public override void AfterBind()
        {
            base.AfterBind();
        }
        public override void ButtonClick(ButtonClickEventArgs e)
        {
            base.ButtonClick(e);
            switch (e.Sender)
            {
                case Const_Distribution.Query_Control_ButtonQuery:
                    QueryData();
                    break;
                case Const_Distribution.Query_Control_ButtonConfirm:
                    DoConfirmOrSave(false);
                    break;
                case Const_Distribution.Query_Control_ButtonSave:
                    DoConfirmOrSave(true);
                    break;
                case Const_Distribution.Query_Control_ButtonBatchFill:
                    BatchFill();
                    break;
            }
        }

        public override void DataChanged(DataChangedEventArgs e)
        {
            base.DataChanged(e);
            if (e.Sender.EqualIgnorCase(Const_Distribution.Entry_Control_FAllCheck))
            {
                GridControl gcData =
                    this.View.GetControl<GridControl>(Const_Distribution.Main_GridControl);
                if (gcData.DataSource == null) return;
                bool isCheck = this.View.GetValue<bool>(Const_Distribution.Entry_Control_FAllCheck);
                int checkValue = 0;
                if (isCheck)
                {
                    checkValue = 1;
                }
                else
                {
                    checkValue = 0;
                }
                DataTable dt = (DataTable)gcData.DataSource;
                foreach (DataRow row in dt.Rows)
                {
                    row["FCHECK"] = checkValue;
                }
                gcData.DataSource = dt;
                gcData.Refresh();
                this.View.Model.BindEntryData(Const_Distribution.Entry_TableName, dt);
            }
            if (e.Sender == Const_Distribution.Query_FStock)
            {
                BindStockArea();
            }
            if (e.Sender == Const_Distribution.Entry_Field_FStockAreaPlan)
            {
                GridControl gcData =
                   this.View.GetControl<GridControl>(Const_Distribution.Main_GridControl);
                DataTable dt = (DataTable)gcData.DataSource;
                string planName = dt.Rows[e.Row].GetValue<string>(Const_Distribution.Entry_Column_FStockAreaPlan);
                if (planName.IsNullOrEmptyOrWhiteSpace()) return;
                DataRow data = GetPlanInfo(planName);
                if (data == null) return;
                FillRow(data, dt.Rows[e.Row]);
                gcData.DataSource = dt;
                gcData.RefreshDataSource();
            }
        }
        private void InitView()
        {
            if (this.Context.CompanyCode == "2")
            {
                var entry = this.View.GetControl<GridView>(Const_Distribution.Main_GridView);
                foreach (GridColumn item in entry.Columns)
                {
                    if (item.Name.EqualIgnorCase(Const_Distribution.Entry_Column_FSpecification)
                        || item.Name.EqualIgnorCase(Const_Distribution.Entry_Column_FStockAreaPlan)
                        || item.Name.EqualIgnorCase(Const_Distribution.Entry_Column_FPaperGrade)
                        || item.Name.EqualIgnorCase(Const_Distribution.Entry_Column_FPackType)
                        || item.Name.EqualIgnorCase(Const_Distribution.Entry_Column_FCertification)
                        || item.Name.EqualIgnorCase(Const_Distribution.Entry_Column_FColor)
                        || item.Name.EqualIgnorCase(Const_Distribution.Entry_Column_FCoreDiameterOrReam)
                        || item.Name.EqualIgnorCase(Const_Distribution.Entry_Column_FDeliverDate)
                        || item.Name.EqualIgnorCase(Const_Distribution.Entry_Column_FDiameterOrSlides)
                        || item.Name.EqualIgnorCase(Const_Distribution.Entry_Column_FLength)
                        || item.Name.EqualIgnorCase(Const_Distribution.Entry_Column_FSpCustomer)
                        || item.Name.EqualIgnorCase(Const_StockAreaData.Entry_Column_FPoNumber)

                        || item.Name.EqualIgnorCase(Const_Distribution.Entry_Column_FSpecProdName)
                        || item.Name.EqualIgnorCase(Const_Distribution.Entry_Column_FSpecProdName)
                        || item.Name.EqualIgnorCase(Const_Distribution.Entry_Column_FTrademarkStyle)
                        || item.Name.EqualIgnorCase(Const_Distribution.Entry_Column_FTransportType)
                        || item.Name.EqualIgnorCase(Const_Distribution.Entry_Column_FWeightMode))
                    {
                        item.Visible = false;
                        continue;
                    }
                    if (item.Name.EqualIgnorCase(Const_Distribution.Entry_Column_FSpecCustName))
                    {
                        item.Caption = "客户";
                    }
                }
            }
        }
        private void DoConfirmOrSave(bool isSave)
        {
            //执行确认操作，首先将用户选择的分录更新至库区分配表中（因为用户可以手动修改库区等信息，所以需要更新）
            //更新之后，将数据同步到库区数据表中
            GridControl gcData =
                this.View.GetControl<GridControl>(Const_Distribution.Main_GridControl);
            gcData.RefreshDataSource();
            DataTable dt = (DataTable)gcData.DataSource;
            DataRow[] rows = dt.Select("FCHECK= 1 AND FSTATUS =0 ");
            if (rows.Count() <= 0)
            {
                ChaintMessageBox.Show("请至少选择一条未确认的数据进行操作!");
                return;
            }
            if (!InputValidate(rows)) return;
            OperationResult result = this.View.Model.Save();
            if (!result.IsSuccess)
            {
                ChaintMessageBox.Show("发生错误，请重试!");
                return;
            }
            if (isSave)
            {
                ChaintMessageBox.Show("保存成功!");
                return;
            }
            //校验未通过，直接返回
            if (!ConfirmValidate(rows)) return;

            ExcuteObject obj1 = new ExcuteObject();
            ExcuteObject obj2 = new ExcuteObject();
            ExcuteObject obj3 = new ExcuteObject();
            ExcuteObject obj4 = new ExcuteObject();
            //更新库区计划
            string sql1 = @"MERGE INTO T_AUTOSCAN_STOCKAREADATAENTRY SADE
                    USING (SELECT SADIS.FPRODUCT,SADIS.FSTOCKAREAPLAN,SUM(SADIS.FAMOUNT) AS FAMOUNT,
					ISNULL(SUM(SADIS.FWEIGHT),0) AS FWEIGHT,
                    SADIS.FSTOCK,SADIS.FSTOCKAREA
                    FROM T_AUTOSCAN_STOCKAREADISTRIBUTION SADIS
                    WHERE SADIS.FSTATUS=0 AND SADIS.FID IN ({0}) 
					GROUP BY SADIS.FPRODUCT,SADIS.FSTOCKAREAPLAN,SADIS.FSTOCK,SADIS.FSTOCKAREA) TEMP
                    ON (SADE.FPRODUCT =TEMP.FPRODUCT AND SADE.FNAME =TEMP.FSTOCKAREAPLAN AND 
                    SADE.FSTOCK=TEMP.FSTOCK AND
					SADE.FSTOCKAREA = TEMP.FSTOCKAREA
                    AND SADE.FSTATUS = 1)
                    WHEN MATCHED THEN
                    UPDATE SET SADE.FAMOUNT +=  TEMP.FAMOUNT,SADE.FWEIGHT +=  TEMP.FWEIGHT;";
            //更新库区数据
            string sql2 = @"MERGE INTO T_AUTOSCAN_STOCKAREADATAENTRY SADE
                    USING (SELECT 
					SUM(SADIS.FAMOUNT) AS FAMOUNT,ISNULL(SUM(SADIS.FWEIGHT),0) AS FWEIGHT,
                    SADIS.FPRODUCT,SADIS.FSTOCK,SADIS.FSTOCKAREA,
					SADIS.FPAPERTYPE,SADIS.FSPECIFICATION,SADIS.FPAPERGRADE,SADIS.FCERTIFICATION,SADIS.FTRANSPORTTYPE,
					SADIS.FCOREDIAMETERORREAM,SADIS.FDIAMETERORSLIDES,SADIS.FLENGTH,SADIS.FTRADEMARKSTYLE,
					SADIS.FSPECCUSTNAME,SADIS.FSPECPRODNAME,SADIS.FPACKTYPE,SADIS.FDELIVERDATE,
                    SADIS.FWEIGHTMODE,
					SADIS.FCOLOR,SADIS.FPONUMBER,SADIS.FMEMO,SADIS.FCUSTOMER,SADIS.FSTOCKDATE
                    FROM T_AUTOSCAN_STOCKAREADISTRIBUTION SADIS
                    WHERE SADIS.FSTATUS=0 AND SADIS.FID IN ({0}) 
					GROUP BY SADIS.FPRODUCT,SADIS.FSTOCK,SADIS.FSTOCKAREA,
					SADIS.FPAPERTYPE,SADIS.FSPECIFICATION,SADIS.FPAPERGRADE,SADIS.FCERTIFICATION,SADIS.FTRANSPORTTYPE,
					SADIS.FCOREDIAMETERORREAM,SADIS.FDIAMETERORSLIDES,SADIS.FLENGTH,SADIS.FTRADEMARKSTYLE,
					SADIS.FSPECCUSTNAME,SADIS.FSPECPRODNAME,SADIS.FPACKTYPE,SADIS.FDELIVERDATE,SADIS.FWEIGHTMODE,
					SADIS.FCOLOR,SADIS.FPONUMBER,SADIS.FMEMO,SADIS.FCUSTOMER,SADIS.FSTOCKDATE) TEMP
                    ON (SADE.FPRODUCT =TEMP.FPRODUCT AND SADE.FSTOCK=TEMP.FSTOCK AND SADE.FSTOCKAREA = TEMP.FSTOCKAREA
					AND  TEMP.FPAPERTYPE = SADE.FPAPERTYPE AND  TEMP.FSPECIFICATION =SADE.FSPECIFICATION
					AND TEMP.FPAPERGRADE = SADE.FPAPERGRADE AND TEMP.FCERTIFICATION = SADE.FCERTIFICATION
					AND TEMP.FTRANSPORTTYPE = SADE.FTRANSPORTTYPE AND TEMP.FCOREDIAMETERORREAM =  SADE.FCOREDIAMETERORREAM 
					AND TEMP.FDIAMETERORSLIDES= SADE.FDIAMETERORSLIDES AND TEMP.FLENGTH =  SADE.FLENGTH 
					AND TEMP.FTRADEMARKSTYLE= SADE.FTRADEMARKSTYLE AND TEMP.FSPECCUSTNAME=  SADE.FSPECCUSTNAME 
					AND TEMP.FSPECPRODNAME = SADE.FSPECPRODNAME AND TEMP.FPACKTYPE = SADE.FPACKTYPE
				    AND ISNULL(TEMP.FDELIVERDATE,'2017-10-26') = ISNULL(SADE.FDELIVERDATE,'2017-10-26') 
					AND TEMP.FWEIGHTMODE= SADE.FWEIGHTMODE
					AND TEMP.FCOLOR= SADE.FCOLOR AND TEMP.FPONUMBER= SADE.FPONUMBER
					AND TEMP.FMEMO= SADE.FMEMO AND TEMP.FCUSTOMER= SADE.FCUSTOMER 
					AND TEMP.FSTOCKDATE= SADE.FSTOCKDATE
                    AND SADE.FSTATUS = 3)
                    WHEN MATCHED THEN
                    UPDATE SET SADE.FAMOUNT +=  TEMP.FAMOUNT,SADE.FWEIGHT +=  TEMP.FWEIGHT,SADE.FDATE=GETDATE()
					WHEN NOT MATCHED THEN
						INSERT (FENTRYID,FPRODUCT,FAMOUNT,FWEIGHT,FPLANAMOUNT,FSTATUS,FDATE,FSTOCK,FSTOCKAREA,
						FPAPERTYPE,FSPECIFICATION,FPAPERGRADE,FCERTIFICATION,FTRANSPORTTYPE,FCOREDIAMETERORREAM,FDIAMETERORSLIDES,
						FLENGTH,FTRADEMARKSTYLE,FSPECCUSTNAME,FSPECPRODNAME,FPACKTYPE,FDELIVERDATE,FWEIGHTMODE,
						FCOLOR,FPONUMBER,FMEMO,FCUSTOMER,FSTOCKDATE)
                    VALUES(newid(),TEMP.FPRODUCT,TEMP.FAMOUNT,TEMP.FWEIGHT,0,3,GETDATE(),TEMP.FSTOCK,TEMP.FSTOCKAREA,
					TEMP.FPAPERTYPE,TEMP.FSPECIFICATION,TEMP.FPAPERGRADE,TEMP.FCERTIFICATION,TEMP.FTRANSPORTTYPE,
					TEMP.FCOREDIAMETERORREAM,TEMP.FDIAMETERORSLIDES,TEMP.FLENGTH,TEMP.FTRADEMARKSTYLE,
					TEMP.FSPECCUSTNAME,TEMP.FSPECPRODNAME,TEMP.FPACKTYPE,TEMP.FDELIVERDATE,TEMP.FWEIGHTMODE,
					TEMP.FCOLOR,TEMP.FPONUMBER,TEMP.FMEMO,TEMP.FCUSTOMER,TEMP.FSTOCKDATE);";
            //如果计划数量=数量，将计划状态改为计划完成状态
            string sql3 = @"UPDATE T_AUTOSCAN_STOCKAREADATAENTRY SET FSTATUS=2 WHERE FPLANAMOUNT = FAMOUNT AND FSTATUS =1";
            //更新库区分配表的状态和确认日期
            string sql4 = @"UPDATE T_AUTOSCAN_STOCKAREADISTRIBUTION  SET FSTATUS = 1,FCONFIRMDATE=GETDATE() WHERE 
                    FSTATUS=0 AND FID IN ({0})";
            List<SqlParameter> parameters = new List<SqlParameter>();
            string filter = GetFilterStringAndParameter(rows, parameters);
            obj1.Parameters.AddRange(parameters);
            obj2.Parameters.AddRange(parameters);
            obj4.Parameters.AddRange(parameters);
            obj1.Sql = string.Format(sql1, filter);
            obj2.Sql = string.Format(sql2, filter);
            obj3.Sql = sql3;
            obj4.Sql = string.Format(sql4, filter);
            List<ExcuteObject> excuteObjs = new List<ExcuteObject>();
            excuteObjs.Add(obj1);
            excuteObjs.Add(obj2);
            excuteObjs.Add(obj3);
            excuteObjs.Add(obj4);
            OperationResult opResult = DBAccessServiceHelper.ExcuteWithTransaction(this.Context, excuteObjs);
            if (opResult.IsSuccess)
            {
                ChaintMessageBox.Show("确认成功!");
                QueryData();
            }
            else
            {
                ChaintMessageBox.Show("确认失败，请检查是否存在可用的库区数据计划!");
            }
        }

        private bool InputValidate(DataRow[] rows)
        {
            foreach (DataRow row in rows)
            {
                string stockAreaPlan = row.GetValue<string>(Const_Distribution.Entry_Column_FStockAreaPlan);
                if (stockAreaPlan.IsNullOrEmptyOrWhiteSpace())
                {
                    ChaintMessageBox.Show("计划名称不允许为空!");
                    return false;
                }
                string stock = row.GetValue<string>(Const_Distribution.Entry_Column_FStock);
                if (stock.IsNullOrEmptyOrWhiteSpace())
                {
                    ChaintMessageBox.Show("仓库不允许为空!");
                    return false;
                }
                string stockArea = row.GetValue<string>(Const_Distribution.Entry_Column_FStockArea);
                if (stockArea.IsNullOrEmptyOrWhiteSpace())
                {
                    ChaintMessageBox.Show("库区不允许为空!");
                    return false;
                }
            }
            return true;
        }

        private bool ConfirmValidate(DataRow[] rows)
        {
            bool result = true;
            //判斷庫區數據是否初始化
            string sqlFormat = @"SELECT SADIS.FSTOCK,SADIS.FSTOCKAREA,WL.WLMC AS FPRODUCTNAME,SADE.FENTRYID,TEMP.FSTOCKAREANAME
                    ,SADE.FPLANAMOUNT - SADE.FAMOUNT AS FLEFTAMOUNT,SADIS.FAMOUNT
                    FROM T_AUTOSCAN_STOCKAREADISTRIBUTION SADIS
                    LEFT JOIN T_AUTOSCAN_STOCKAREADATAENTRY SADE ON SADIS.FPRODUCT = SADE.FPRODUCT AND 
					SADIS.FSTOCKAREAPLAN = SADE.FNAME AND SADE.FSTATUS=1
					AND SADIS.FSTOCK = SADE.FSTOCK AND SADIS.FSTOCKAREA = SADE.FSTOCKAREA 
                    LEFT JOIN (SELECT SAE.FSTOCKAREANAME,SAE.FSTOCKAREANUMBER,SA.FSTOCK 
                    FROM T_AUTOSCAN_STOCKAREAENTRY SAE
					INNER JOIN T_AUTOSCAN_STOCKAREA SA ON SA.FID= SAE.FHEADID) TEMP ON 
					SADIS.FSTOCK = TEMP.FSTOCK AND SADIS.FSTOCKAREA = TEMP.FSTOCKAREANUMBER
				    LEFT JOIN CT_WLZD WL ON WL.WLBH = SADIS.FPRODUCT
                    WHERE SADIS.FSTATUS=0 AND 
                     SADIS.FID IN ({0}) ";
            List<QueryParameter> param = new List<QueryParameter>();
            string filter = GetFilterStringAndParameter(rows, null, param);
            string sql = string.Format(sqlFormat, filter);
            OperateResult validateResult = DBAccessServiceHelper.ExcuteQuery(this.Context, sql, param.ToArray());
            if (!validateResult.IsSuccess)
            {
                ChaintMessageBox.Show("发生错误，请重试!");
                result = false;
            }
            else
            {
                if (validateResult.ResultTable.Count() > 0)
                {
                    bool isValPlan = true;
                    bool isValAmount = true;
                    string stock = string.Empty;
                    string stockArea = string.Empty;
                    string productName = string.Empty;
                    Dictionary<string, int> leftAmountDic = new Dictionary<string, int>();
                    foreach (var item in validateResult.ResultTable)
                    {
                        string fentryid = Convert.ToString(item["FENTRYID"]);
                        int leftAmount = Convert.ToInt32(item["FLEFTAMOUNT"]);
                        int amount = Convert.ToInt32(item["FAMOUNT"]);
                        stock = Convert.ToString(item["FSTOCK"]);
                        stockArea = Convert.ToString(item["FSTOCKAREANAME"]);
                        productName = Convert.ToString(item["FPRODUCTNAME"]);
                        if (fentryid.IsNullOrEmptyOrWhiteSpace())
                        {
                            isValPlan = false;
                            break;
                        }
                        if (leftAmount - amount < 0)
                        {
                            isValAmount = false;
                            break;
                        }
                        if (leftAmountDic.Keys.Contains(fentryid))
                        {
                            leftAmountDic[fentryid] -= amount;
                            if (leftAmountDic[fentryid] < 0)
                            {
                                isValAmount = false;
                                break;
                            }
                        }
                        else
                        {
                            leftAmountDic.Add(fentryid, leftAmount - amount);
                        }
                    }
                    if (!isValPlan)
                    {
                        ChaintMessageBox.Show(string.Format("仓库【{0}】，库区【{1}】没有产品【{2}】的有效数据计划，请检查！", stock, stockArea, productName));
                        result = false;
                    }
                    else if (!isValAmount)
                    {
                        ChaintMessageBox.Show(string.Format("仓库【{0}】，库区【{1}】,产品【{2}】对应的数据计划剩余数量不足，请检查！", stock, stockArea, productName));
                        result = false;
                    }
                }
            }
            return result;
        }
        private string GetFilterStringAndParameter(DataRow[] rows, List<SqlParameter> sqlParameters = null,
            List<QueryParameter> queryParameters = null)
        {
            StringBuilder filterSb = new StringBuilder();
            List<QueryParameter> param = new List<QueryParameter>();
            int i = 0;
            char seperator = ',';
            foreach (DataRow row in rows)
            {
                i++;
                string fid = row.GetValue<string>(Const_Distribution.Entry_Column_FID);
                string parameterName = string.Format("@{0}{1}", Const_Distribution.Entry_Column_FID, i);
                filterSb.Append(parameterName);
                filterSb.Append(seperator);
                SqlParameter para = new SqlParameter(parameterName, DbType.String);
                para.Value = fid;
                if (sqlParameters != null)
                {
                    sqlParameters.Add(para);
                }
                if (queryParameters != null)
                {
                    queryParameters.Add(new QueryParameter(parameterName, typeof(string), fid));
                }
            }
            return filterSb.ToString().TrimEnd(seperator);
        }
        private void QueryData()
        {
            if (Validate())
            {
                BindDataPlan();
                DateTime beginDate = this.View.GetValue<DateTime>(Const_Distribution.Query_FBeginDate);
                DateTime endDate = this.View.GetValue<DateTime>(Const_Distribution.Query_FEndDate);
                string oper = this.View.GetValue<string>(Const_Distribution.Query_FOper);
                bool isCheck = this.View.GetValue<bool>(Const_Distribution.Query_FIsConfirm);
                string[] stocks = this.View.GetValue<string>(Const_Distribution.Query_FStock).Split(',');
                string[] stockList = stocks.Where(w => !w.IsNullOrEmptyOrWhiteSpace()).ToArray();
                string[] stockAreas = this.View.GetValue<string>(Const_Distribution.Query_FStockArea).Split(',');
                string[] stockAreaList = stockAreas.Where(w => !w.IsNullOrEmptyOrWhiteSpace()).ToArray();
                string product = this.View.GetValue<string>(Const_Distribution.Query_FProduct);
                string sql = @"SELECT 
                0 AS FCHECK,
                SAD.FID,
                FBARCODE,
                FOPERATOR,
                SAD.FPRODUCT,   
                WL.WLMC AS FPRODUCTNAME,
                SAD.FSTOCKAREAPLAN,
                SAD.FSTOCK,
				STOCK.FNAME AS FSTOCKNAME,
			    SAE.FSTOCKAREANAME AS FSTOCKAREANAME,
                SAD.FSTOCKAREA,
                SAD.FSTATUS,
                CASE SAD.FSTATUS WHEN 1 THEN '已确认' WHEN 0 THEN '未确认' END AS FSTATUSNAME,
                SAD.FAMOUNT,
                SAD.FDATE,
                SAD.FCONFIRMDATE,
                SAD.FPAPERTYPE,
                CASE SAD.FPAPERTYPE WHEN '1' THEN '卷筒纸' WHEN '2' THEN '平板纸' END AS FPAPERTYPENAME ,
                SAD.FPAPERGRADE,
                SAD.FCERTIFICATION,
                SAD.FPACKTYPE,
                SAD.FTRANSPORTTYPE,
                SAD.FCOLOR,
                SAD.FCOREDIAMETERORREAM,
                SAD.FSPCUSTOMER,
                SAD.FDIAMETERORSLIDES,
                SAD.FLENGTH,
                SAD.FTRADEMARKSTYLE,
                SAD.FSPECPRODNAME,
                SAD.FSPECCUSTNAME,
                SAD.FWEIGHTMODE,
                SAD.FSPECIFICATION,
                SAD.FMEMO,
                SAD.FPONUMBER,
                SAD.FDELIVERDATE,
                SAD.FCUSTOMER,
                SAD.FSTOCKDATE,
                SAD.FWEIGHT,
				SAD.FSTOCKAREAPLAN AS FPLANNAME,
                SAD.FPRODUCTINFO
                FROM T_AUTOSCAN_STOCKAREADISTRIBUTION SAD
				LEFT JOIN [dbo].[CT_WLZD] WL ON SAD.FPRODUCT = WL.WLBH
                LEFT JOIN T_AUTOSCAN_STOCK STOCK ON SAD.FSTOCK = STOCK.FNUMBER
                LEFT JOIN T_AUTOSCAN_STOCKAREAENTRY SAE ON SAD.FSTOCKAREA = SAE.FSTOCKAREANUMBER
                WHERE SAD.FSTATUS=@FSTATUS {0} ORDER BY FDATE DESC";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@FSTATUS", DbType.Int32));
                parameters[0].Value = isCheck ? 1 : 0;
                StringBuilder sbFilter = new StringBuilder();
                if (beginDate != null && beginDate != DateTime.MinValue && beginDate != DateTime.Parse("1753/1/1 12:00:00"))
                {
                    sbFilter.Append(" AND SAD.FDATE >= @FBEGINDATE ");
                    SqlParameter para = new SqlParameter("@FBEGINDATE", DbType.DateTime);
                    para.Value = beginDate;
                    parameters.Add(para);
                }
                if (endDate != null && endDate != DateTime.MinValue && beginDate != DateTime.Parse("1753/1/1 12:00:00"))
                {
                    sbFilter.Append(" AND SAD.FDATE <= @FENDDATE ");
                    SqlParameter para = new SqlParameter("@FENDDATE", DbType.DateTime);
                    para.Value = endDate;
                    parameters.Add(para);
                }
                if (!oper.IsNullOrEmptyOrWhiteSpace())
                {
                    sbFilter.Append(" AND SAD.FOPERATOR = @FOPERATOR ");
                    parameters.Add(new SqlParameter("@FOPERATOR", DbType.String));
                    parameters[3].Value = oper;
                }
                if (stockList.Length == 1)
                {
                    sbFilter.Append(" AND SAD.FSTOCK= @FSTOCK ");
                    SqlParameter item = new SqlParameter("@FSTOCK", DbType.String);
                    item.Value = stocks[0];
                    parameters.Add(item);
                }
                else if (stockList.Length > 1)
                {
                    string format = " AND SAD.FSTOCK IN ({0})";
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
                    sbFilter.Append(" AND SAD.FSTOCKAREA= @FSTOCKAREA ");
                    SqlParameter item = new SqlParameter("@FSTOCKAREA", DbType.String);
                    item.Value = stockAreas[0];
                    parameters.Add(item);
                }
                else if (stockAreaList.Length > 1)
                {
                    string format = " AND SAD.FSTOCKAREA IN ({0}) ";
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
                if (!product.IsNullOrEmptyOrWhiteSpace())
                {
                    SqlParameter item = new SqlParameter("@FPRODUCT", DbType.String);
                    item.Value = product;
                    parameters.Add(item);
                    sbFilter.AppendFormat(" AND SAD.FPRODUCT = @FPRODUCT ");
                }
                string specification = this.View.GetValue<string>(Const_Distribution.Query_FSpecification);
                if (!specification.IsNullOrEmptyOrWhiteSpace())
                {
                    SqlParameter spItem = new SqlParameter("@FSPECIFICATION", DbType.String);
                    spItem.Value = specification;
                    parameters.Add(spItem);
                    sbFilter.AppendFormat(" AND SAD.FSPECIFICATION = @FSPECIFICATION ");
                }
                sql = string.Format(sql, sbFilter.ToString());
                string[] tableName = new string[] { "T1" };
                OperateResults result = DBAccessServiceHelper.ExcuteQuery(this.Context, sql, tableName, parameters.ToArray());
                if (result.IsSuccess)
                {
                    GridControl gvMain = this.View.GetControl<GridControl>(Const_Distribution.Main_GridControl);
                    gvMain.DataSource = result.ResultData.Tables["T1"];
                    this.View.GetControl<GridView>(Const_Distribution.Main_GridView).BestFitColumns();
                    this.View.Model.BindEntryData(Const_Distribution.Entry_TableName, result.ResultData.Tables["T1"]);
                    var gridView =
                        this.View.GetControl<GridView>(Const_Distribution.Main_GridView);
                    var saveControl =
                        this.View.GetControl<BarButtonItem>(Const_Distribution.Query_Control_ButtonSave);
                    var confirmControl =
                       this.View.GetControl<BarButtonItem>(Const_Distribution.Query_Control_ButtonConfirm);
                    GridColumn plan =
                        this.View.GetControl<GridColumn>(Const_Distribution.Entry_Column_FStockAreaPlan);
                    GridColumn planName =
                        this.View.GetControl<GridColumn>(Const_Distribution.Entry_Column_FPlanName);
                    if (isCheck)
                    {
                        plan.Visible = false;
                        planName.Visible = true;
                        saveControl.Enabled = false;
                        confirmControl.Enabled = false;
                        gridView.OptionsBehavior.ReadOnly = true;
                    }
                    else
                    {
                        plan.Visible = true;
                        planName.Visible = false;
                        saveControl.Enabled = true;
                        confirmControl.Enabled = true;
                        gridView.OptionsBehavior.ReadOnly = false;
                    }
                }
                else
                {
                    ChaintMessageBox.Show("查询失败，请重试!");
                }
            }
        }
        private bool Validate()
        {
            DateTime beginDate = this.View.GetValue<DateTime>(Const_Distribution.Query_FBeginDate);
            DateTime endDate = this.View.GetValue<DateTime>(Const_Distribution.Query_FEndDate);
            if (beginDate == null || beginDate == DateTime.MinValue)
            {
                ChaintMessageBox.Show("请选择查询开始日期！");
                return false;
            }
            if (endDate == null || endDate == DateTime.MinValue)
            {
                ChaintMessageBox.Show("请选择查询结束日期！");
                return false;
            }
            //string[] stocks = this.View.GetValue<string>(Const_Distribution.Query_FStock).Split(',');
            //string[] stockList = stocks.Where(w => !w.IsNullOrEmptyOrWhiteSpace()).ToArray();
            //if (stockList == null || stockList.Length<1)
            //{
            //    ChaintMessageBox.Show("请选择仓库！");
            //    return false;
            //}
            //string[] stockAreas = this.View.GetValue<string>(Const_Distribution.Query_FStockArea).Split(',');
            //string[] stockAreaList = stockAreas.Where(w => !w.IsNullOrEmptyOrWhiteSpace()).ToArray();
            //if (stockAreaList == null || stockAreaList.Length < 1)
            //{
            //    ChaintMessageBox.Show("请选择库区！");
            //    return false;
            //}
            return true;
        }

        private void BatchFill()
        {
            int selRow = this.View.GetSelectedRowIndex(Const_Distribution.Main_GridView);
            GridControl gcData =
                  this.View.GetControl<GridControl>(Const_Distribution.Main_GridControl);
            DataTable dt = (DataTable)gcData.DataSource;
            string planName = dt.Rows[selRow].GetValue<string>(Const_Distribution.Entry_Column_FStockAreaPlan);
            if (planName.IsNullOrEmptyOrWhiteSpace()) return;
            DataRow data = GetPlanInfo(planName);
            if (data == null) return;
            foreach (DataRow row in dt.Rows)
            {
                int isCheck = row.GetValue<int>(Const_Distribution.Entry_Column_FCheck);
                if (isCheck==1)
                {
                    row[Const_Distribution.Entry_Column_FStockAreaPlan] = planName;
                    FillRow(data, row);
                }
            }
            gcData.DataSource = dt;
            gcData.RefreshDataSource();
        }
        private DataRow GetPlanInfo(string planName)
        {
            string sql = @" SELECT *,STOCK.FNAME AS FSTOCKNAME,SAE.FSTOCKAREANAME AS FSTOCKAREANAME 
                FROM T_AUTOSCAN_STOCKAREADATAENTRY SDE
                LEFT JOIN T_AUTOSCAN_STOCK STOCK ON SDE.FSTOCK = STOCK.FNUMBER
                LEFT JOIN T_AutoScan_StockAreaEntry SAE ON SDE.FSTOCKAREA = SAE.FSTOCKAREANUMBER
                WHERE SDE.FNAME =@FNAME ";
            SqlParameter para = new SqlParameter("@FNAME", DbType.String);
            para.Value = planName;
            SqlParameter[] parameters =
                new SqlParameter[] { para };
            OperateResults result
                = DBAccessServiceHelper.ExcuteQuery(this.Context, sql, new string[] { "T1" }, parameters);
            if (!result.IsSuccess || result.ResultData.Tables["T1"].Rows.Count <= 0) return null;
            return result.ResultData.Tables["T1"].Rows[0];
        }
        private void FillRow(DataRow data , DataRow row)
        {
            row[Const_Distribution.Entry_Column_FStock] = data[Const_StockAreaData.Entry_Column_FStock];
            row[Const_Distribution.Entry_Column_FStockArea] = data[Const_StockAreaData.Entry_Column_FStockArea];
            row[Const_Distribution.Entry_Column_FStockName] = data["FSTOCKNAME"];
            row[Const_Distribution.Entry_Column_FStockAreaName] = data["FSTOCKAREANAME"];
            row[Const_Distribution.Entry_Column_FPaperType] = data[Const_StockAreaData.Entry_Column_FPaperType];
            row[Const_Distribution.Entry_Column_FSpecification] = data[Const_StockAreaData.Entry_Column_FSpecification];
            row[Const_Distribution.Entry_Column_FPaperGrade] = data[Const_StockAreaData.Entry_Column_FPaperGrade];
            row[Const_Distribution.Entry_Column_FCertification] = data[Const_StockAreaData.Entry_Column_FCertification];
            row[Const_Distribution.Entry_Column_FTransportType] = data[Const_StockAreaData.Entry_Column_FTransportType];

            row[Const_Distribution.Entry_Column_FCoreDiameterOrReam] =
                data.GetValue<decimal>(Const_StockAreaData.Entry_Column_FCoreDiameterOrReam);
            row[Const_Distribution.Entry_Column_FDiameterOrSlides]
                = data.GetValue<decimal>(Const_StockAreaData.Entry_Column_FDiameterOrSlides);
            row[Const_Distribution.Entry_Column_FLength] =
                data.GetValue<decimal>(Const_StockAreaData.Entry_Column_FLength);

            row[Const_Distribution.Entry_Column_FTrademarkStyle] = data[Const_StockAreaData.Entry_Column_FTrademarkStyle];
            row[Const_Distribution.Entry_Column_FSpecCustName] = data[Const_StockAreaData.Entry_Column_FSpecCustName];
            row[Const_Distribution.Entry_Column_FSpecProdName] = data[Const_StockAreaData.Entry_Column_FSpecProdName];

            row[Const_Distribution.Entry_Column_FPackType] = data[Const_StockAreaData.Entry_Column_FPackType];
            row[Const_Distribution.Entry_Column_FDeliverDate] = data[Const_StockAreaData.Entry_Column_FDeliverDate];
            row[Const_Distribution.Entry_Column_FMemo] = data[Const_StockAreaData.Entry_Column_FMemo];
            row[Const_Distribution.Entry_Column_FWeightMode] = data[Const_StockAreaData.Entry_Column_FWeightMode];
            row[Const_Distribution.Entry_Column_FColor] = data[Const_StockAreaData.Entry_Column_FColor];
            row[Const_Distribution.Entry_Column_FPoNumber] = data[Const_StockAreaData.Entry_Column_FPoNumber];
            row[Const_Distribution.Entry_Column_FSpCustomer] = data[Const_StockAreaData.Entry_Column_FSpCustomer];
        }
    }
}
