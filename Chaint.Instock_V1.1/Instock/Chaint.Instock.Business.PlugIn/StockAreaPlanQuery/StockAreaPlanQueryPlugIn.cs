using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using Chaint.Common.ServiceHelper;
using Chaint.Common.BasePlugIn;
using Chaint.Common.Core.EventArgs;
using Chaint.Common.Core;
using Chaint.Instock.ServiceHelper;
using Chaint.Common.Core.Utils;
using Chaint.Common.Entity.Utils;
using Chaint.Instock.Core;
using Chaint.Instock.Business.View;
using DevExpress.XtraGrid.Views.Grid;

namespace Chaint.Instock.Business.PlugIns
{
    public class StockAreaPlanQueryPlugIn : AbstractBillPlugIn
    {
        //记录仓库-库区对应关系
        Dictionary<string, List<Tuple<string, string>>> stockDic =
               new Dictionary<string, List<Tuple<string, string>>>();
        Dictionary<string, string> stocks = new Dictionary<string, string>();
        public StockAreaPlanQueryPlugIn(StockAreaPlanQueryView view) : base(view)
        {

        }
        public override void OnSetBusinessInfo()
        {
            base.OnSetBusinessInfo();
            BindStock();
        }
        public override void OnBind()
        {
            base.OnBind();
            DateTime beginDate = DateTime.Now;
            DateTime endDate = DateTime.Now;
            this.View.SetValue(Const_StockAreaPlanQuery.Query_FBeginDate, beginDate);
            this.View.SetValue(Const_StockAreaPlanQuery.Query_FEndDate, endDate);
        }
        public override void DataChanged(DataChangedEventArgs e)
        {
            base.DataChanged(e);
            switch (e.Sender)
            {
                case Const_StockAreaPlanQuery.Query_FStock:
                    BindStockArea();
                    break;
            }
        }
        public override void ButtonClick(ButtonClickEventArgs e)
        {
            base.ButtonClick(e);
            switch (e.Sender)
            {
                case Const_StockAreaPlanQuery.Query_Control_ButtonQuery:
                    GetData();
                    break;
            }
        }

        private void GetData()
        {
            if (Validate())
            {
                DateTime beginDate = this.View.GetValue<DateTime>(Const_StockAreaPlanQuery.Query_FBeginDate);
                DateTime endDate = this.View.GetValue<DateTime>(Const_StockAreaPlanQuery.Query_FEndDate);
                bool isComplete = this.View.GetValue<bool>(Const_StockAreaPlanQuery.Query_FIsComplete);
                string[] stocksNames = this.View.GetValue<string>(Const_StockAreaPlanQuery.Query_FStock).Split(',');
                string[] stockAreas = this.View.GetValue<string>(Const_StockAreaPlanQuery.Query_FStockArea).Split(',');
                string[] stocksList = stocksNames.Where(w => !w.IsNullOrEmptyOrWhiteSpace()).ToArray();
                string[] stockAreaList = stockAreas.Where(w => !w.IsNullOrEmptyOrWhiteSpace()).ToArray();
                string sql = string.Empty;
                if (isComplete == true)
                {
                    sql = @"SELECT MTRL.WLMC AS FPRODUCT,SADE.FNAME AS FSTOCKAREAPLAN,STOCK.FNAME AS FSTOCK,
                    TEMP0.FSTOCKAREANAME AS FSTOCKAREA,TEMP0.FTOTALCAPACITY,
                    SADE.FUSEDCAPACITY,SADE.FPLANAMOUNT,SADE.FAMOUNT,
                    0 AS FWAMOUNT
                    FROM T_AUTOSCAN_STOCKAREADATAENTRY SADE
                    LEFT JOIN T_AUTOSCAN_STOCK STOCK ON SADE.FSTOCK = STOCK.FNUMBER
                    LEFT JOIN [CT_WLZD] MTRL ON SADE.FPRODUCT = MTRL.WLBH
                    LEFT JOIN (SELECT SA.FSTOCK,SAE.FSTOCKAREANUMBER,SAE.FSTOCKAREANAME,SAE.FTOTALCAPACITY
					 FROM T_AUTOSCAN_STOCKAREAENTRY SAE INNER JOIN T_AUTOSCAN_STOCKAREA SA ON SAE.FHEADID
					=SA.FID) TEMP0 ON SADE.FSTOCK= TEMP0.FSTOCK AND SADE.FSTOCKAREA= TEMP0.FSTOCKAREANUMBER
                    WHERE SADE.FDATE BETWEEN @FBEGINDATE  AND @FENDDATE AND SADE.FSTATUS=2
                    {0}";
                }
                else
                {
                    sql = @" SELECT MTRL.WLMC AS FPRODUCT,SADE.FNAME AS FSTOCKAREAPLAN,STOCK.FNAME AS FSTOCK,
                    TEMP0.FSTOCKAREANAME AS FSTOCKAREA,SADE.FSTOCKAREA,TEMP0.FTOTALCAPACITY,
                    SADE.FUSEDCAPACITY,SADE.FPLANAMOUNT,SADE.FAMOUNT,
                    ISNULL(TEMP.FAMOUNT,0) AS FWAMOUNT
                    FROM T_AUTOSCAN_STOCKAREADATAENTRY SADE
                    LEFT JOIN T_AUTOSCAN_STOCK STOCK ON SADE.FSTOCK = STOCK.FNUMBER
                    LEFT JOIN T_AUTOSCAN_STOCKAREAPLAN SAP ON SAP.FNUMBER = SADE.FSTOCKAREAPLAN
                    LEFT JOIN [CT_WLZD] MTRL ON SADE.FPRODUCT = MTRL.WLBH
                    LEFT JOIN (SELECT SA.FSTOCK,SAE.FSTOCKAREANUMBER,SAE.FSTOCKAREANAME,SAE.FTOTALCAPACITY
					 FROM T_AUTOSCAN_STOCKAREAENTRY SAE INNER JOIN T_AUTOSCAN_STOCKAREA SA ON SAE.FHEADID
					=SA.FID) TEMP0 ON SADE.FSTOCK= TEMP0.FSTOCK AND SADE.FSTOCKAREA= TEMP0.FSTOCKAREANUMBER
                    LEFT JOIN
                    (SELECT SADIS.FSTOCK, SADIS.FSTOCKAREA, SADIS.FPRODUCT, SADIS.FSTOCKAREAPLAN, 
                    SUM(SADIS.FAMOUNT) AS  FAMOUNT
                    FROM T_AUTOSCAN_STOCKAREADISTRIBUTION SADIS WHERE SADIS.FSTATUS = 0 AND
					SADIS.FDATE BETWEEN @FBEGINDATE  AND @FENDDATE GROUP BY
                    SADIS.FSTOCK, SADIS.FSTOCKAREA, SADIS.FPRODUCT, SADIS.FSTOCKAREAPLAN) TEMP
                       ON SADE.FSTOCK = TEMP.FSTOCK AND SADE.FSTOCKAREA = TEMP.FSTOCKAREA 
                    AND SADE.FPRODUCT = TEMP.FPRODUCT
                    AND SADE.FNAME = TEMP.FSTOCKAREAPLAN 
                    WHERE SADE.FDATE BETWEEN @FBEGINDATE  AND @FENDDATE AND SADE.FSTATUS = 1
                     {0}";
                }
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@FBEGINDATE", DbType.DateTime));
                parameters.Add(new SqlParameter("@FENDDATE", DbType.DateTime));
                parameters[0].Value = beginDate;
                parameters[1].Value = endDate;
                StringBuilder sbFilter = new StringBuilder();
                if (stocksList.Length == 1)
                {
                    string stock;
                    if (!stocks.TryGetValue(stocksList[0].Trim(), out stock)) return;
                    sbFilter.Append(" AND SADE.FSTOCK= @FSTOCK AND ");
                    SqlParameter item = new SqlParameter("@FSTOCK", DbType.String);
                    item.Value = stock;
                    parameters.Add(item);
                }
                else if (stocksList.Length > 1)
                {
                    string format = " AND SADE.FSTOCK IN ({0}) AND";
                    StringBuilder sbIn = new StringBuilder();
                    int i = 0;
                    foreach (string stockName in stocksList)
                    {
                        string stock;
                        if (!stocks.TryGetValue(stockName.Trim(), out stock)) continue;
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
                    item.Value = stockAreaList[0];
                    parameters.Add(item);
                }
                else if (stockAreaList.Length > 1)
                {
                    string format = " AND SADE.FSTOCKAREA IN ({0}) ";
                    StringBuilder sbIn = new StringBuilder();
                    int i = 0;
                    foreach (string stockarea in stockAreaList)
                    {
                        i++;
                        string paraName = string.Format("@FSTOCKAREA{0}",i);
                        sbIn.Append(paraName);
                        sbIn.Append(',');
                        SqlParameter item = new SqlParameter(paraName,DbType.String);
                        item.Value = stockarea.Trim();
                        parameters.Add(item);
                    }
                    sbFilter.AppendFormat(format, sbIn.ToString().TrimEnd(','));
                }
                sql = string.Format(sql, sbFilter.ToString());
                string[] tableName = new string[] { "T1" };
                OperateResults result = DBAccessServiceHelper.ExcuteQuery(this.Context, sql, tableName, parameters.ToArray());
                if (result.IsSuccess)
                {
                    GridControl gvMain = this.View.GetControl<GridControl>(Const_StockAreaPlanQuery.Main_GridControl);
                    gvMain.DataSource = result.ResultData.Tables["T1"];
                    this.View.GetControl<GridView>(Const_StockAreaPlanQuery.Main_GridView).BestFitColumns();
                }
            }
        }
        private bool Validate()
        {
            DateTime beginDate = this.View.GetValue<DateTime>(Const_StockAreaPlanQuery.Query_FBeginDate);
            DateTime endDate = this.View.GetValue<DateTime>(Const_StockAreaPlanQuery.Query_FEndDate);
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
            //string stock = this.View.GetValue<string>(Const_StockAreaPlanQuery.Query_FStock);
            //if (stock.IsNullOrEmptyOrWhiteSpace())
            //{
            //    ChaintMessageBox.Show("请选择查询仓库！");
            //    return false;
            //}
            //string stockArea = this.View.GetValue<string>(Const_StockAreaPlanQuery.Query_FStockArea);
            //if (stockArea.IsNullOrEmptyOrWhiteSpace())
            //{
            //    ChaintMessageBox.Show("请选择查询库区！");
            //    return false;
            //}
            return true;
        }
        private void BindStockArea()
        {
            string[] stocks = this.View.GetValue<string>(Const_StockAreaPlanQuery.Query_FStock).Split(',');
            if (stocks.Length < 1) return;
            List<Tuple<string, string>> stockAreas =new  List<Tuple<string, string>>();
            foreach (string stock in stocks)
            {
                List<Tuple<string, string>> items;
                if (!stockDic.TryGetValue(stock.Trim(), out items)) continue;
                if (items.IsEmpty()) continue;
                stockAreas.AddRange(items);
            }
            if (stockAreas.IsEmpty()) return;
            DataTable dtSource = new DataTable();
            dtSource.Columns.Add("FNAME", typeof(string));
            dtSource.Columns.Add("FNUMBER", typeof(string));
            dtSource.Columns["FNAME"].Caption = "名称";
            dtSource.Columns["FNUMBER"].Caption = "编号";
            foreach (var item in stockAreas)
            {
                DataRow dRow = dtSource.NewRow();
                dRow["FNUMBER"] = item.Item1;
                dRow["FNAME"] = item.Item2;
                dtSource.Rows.Add(dRow);
            }
            dtSource.AcceptChanges();
            var field = this.View.GetControl<CheckedComboBoxEdit>(Const_StockAreaPlanQuery.Query_FStockArea);
            DataSource ds = new DataSource();
            ds.DisplayMember = "FNAME";
            ds.ValueMember = "FNUMBER";
            ds.Data = dtSource;
            field.Bind(ds);
        }

        private void BindStock()
        {
            OperateResult result =
              StockAreaDataServiceHelper.GetStockAreaInfo(this.Context);
            if (!result.IsSuccess)
            {
                ChaintMessageBox.Show("发生错误，请查看日志");
                return;
            }
            stockDic.Clear();
            stocks.Clear();
            foreach (var item in result.ResultTable)
            {
                string stockName = Convert.ToString(item["FSTOCKNAME"]);
                string stockNumber = Convert.ToString(item["FSTOCK"]);
                string stockAreaNumber = Convert.ToString(item[Const_StockArea.Entry_Column_FStockAreaNumber]);
                string stockAreaName = Convert.ToString(item[Const_StockArea.Entry_Column_FStockAreaName]);
                Tuple<string, string> areaItem = new Tuple<string, string>(stockAreaNumber, stockAreaName);
                if (!stocks.Keys.Contains(stockName))
                {
                    stocks.Add(stockName, stockNumber);
                }
                if (stockDic.Keys.Contains(stockName))
                {
                    stockDic[stockName].Add(areaItem);
                }
                else
                {
                    stockDic.Add(stockName, new List<Tuple<string, string>>() { areaItem });
                }
            }
            DataTable dtSource = new DataTable();
            dtSource.Columns.Add("FNAME", typeof(string));
            dtSource.Columns["FNAME"].Caption = "名称";
            foreach (var stock in stocks.Keys)
            {
                DataRow dRow = dtSource.NewRow();
                dRow["FNAME"] = stock;
                dtSource.Rows.Add(dRow);
            }
            dtSource.AcceptChanges();
            var field = this.View.GetControl<CheckedComboBoxEdit>(Const_StockAreaPlanQuery.Query_FStock);
            DataSource ds = new DataSource();
            ds.DisplayTitle = "仓库";
            ds.DisplayMember = "FNAME";
            ds.ValueMember = "FNAME";
            ds.Data = dtSource;
            field.Bind(ds);
        }
    }
}
