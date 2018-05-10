using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraEditors.Repository;
using Chaint.Common.Core;
using Chaint.Common.Core.Utils;
using Chaint.Common.Core.EventArgs;
using Chaint.Instock.ServiceHelper;
using Chaint.Common.Entity.Utils;
using Chaint.Instock.Core;
using DevExpress.DataAccess.Sql;
using Chaint.Common.ServiceHelper;
using System.Text;
using DevExpress.XtraEditors;
using System.Data.SqlClient;

namespace Chaint.Instock.Business.PlugIns
{
    partial class DistributionPlugIn
    {
        //记录仓库-库区对应关系
        Dictionary<string, List<Tuple<string, string>>> stockDic =
               new Dictionary<string, List<Tuple<string, string>>>();
        Dictionary<string, string> stocks = new Dictionary<string, string>();
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
                if (stockDic.Keys.Contains(stockNumber))
                {
                    stockDic[stockNumber].Add(areaItem);
                }
                else
                {
                    stockDic.Add(stockNumber, new List<Tuple<string, string>>() { areaItem });
                }
            }
            DataTable dtSource = new DataTable();
            dtSource.Columns.Add("FNAME", typeof(string));
            dtSource.Columns.Add("FNUMBER", typeof(string));
            dtSource.Columns["FNAME"].Caption = "名称";
            dtSource.Columns["FNUMBER"].Caption = "编码";
            foreach (var stock in stocks.Keys)
            {
                DataRow dRow = dtSource.NewRow();
                dRow["FNAME"] = stock;
                dRow["FNUMBER"] = stocks[stock];
                dtSource.Rows.Add(dRow);
            }
            dtSource.AcceptChanges();
            var field = this.View.GetControl<CheckedComboBoxEdit>(Const_Distribution.Query_FStock);
            DataSource ds = new DataSource();
            ds.DisplayTitle = "仓库";
            ds.DisplayMember = "FNAME";
            ds.ValueMember = "FNUMBER";
            ds.Data = dtSource;
            field.Bind(ds);
        }
        private void BindStockArea()
        {
            string[] stocks = this.View.GetValue<string>(Const_StockAreaDataQuery.Head_Field_FStock).Split(',');
            if (stocks.Length < 1) return;
            List<Tuple<string, string>> stockAreas = new List<Tuple<string, string>>();
            foreach (string stock in stocks)
            {
                List<Tuple<string, string>> items;
                if (!stockDic.TryGetValue(stock.Trim(), out items)) continue;
                if (items.IsEmpty()) continue;
                stockAreas.AddRange(items);
            }
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
            var field = this.View.GetControl<CheckedComboBoxEdit>(Const_Distribution.Query_FStockArea);
            DataSource ds = new DataSource();
            ds.DisplayMember = "FNAME";
            ds.ValueMember = "FNUMBER";
            ds.Data = dtSource;
            field.Bind(ds);
        }
        private void BindProduct()
        {
            OperateResult result =
                StockAreaDataServiceHelper.GetProductInfo(this.Context);
            if (!result.IsSuccess) return;
            if (result.ResultDataTable.IsEmpty()) return;
            result.ResultDataTable.Columns["WLMC"].Caption = "名称";
            result.ResultDataTable.Columns["WLBH"].Caption = "编码";
            DataSource ds = new DataSource();
            ds.DisplayMember = "WLMC";
            ds.ValueMember = "WLBH";
            ds.Data = result.ResultDataTable;
            var field = this.View.GetControl<SearchLookUpEdit>(Const_Distribution.Query_FProduct);
            field.Bind(ds);
        }
        private void BindDataPlan()
        {

            string sqlFormat = @" SELECT DISTINCT SAD.FNAME FROM T_AUTOSCAN_STOCKAREADATAENTRY SAD
                    WHERE FSTATUS=1 AND SAD.FDATE BETWEEN @FBEGINDATE AND @FENDDATE ";
            StringBuilder sbFilter = new StringBuilder();
            List<SqlParameter> param = new List<SqlParameter>();
            SqlParameter item1 = new SqlParameter("@FBEGINDATE",DbType.DateTime);
            item1.Value = DateTime.Now.AddDays(-7).Date;
            SqlParameter item2 = new SqlParameter("@FENDDATE", DbType.DateTime);
            item2.Value = DateTime.Now.AddDays(2).Date;
            param.Add(item1);
            param.Add(item2);
            string sql = string.Format(sqlFormat, sbFilter.ToString());
            string[] table = new string[] { "T1"};
            OperateResults result = DBAccessServiceHelper.ExcuteQuery(this.Context, sql, table, param.ToArray());
            if (!result.IsSuccess) return;
            result.ResultData.Tables["T1"].Columns["FNAME"].Caption = "名称";
            DataSource ds = new DataSource();
            ds.DisplayMember = "FNAME";
            ds.ValueMember = "FNAME";
            ds.Data = result.ResultData.Tables["T1"];
            var planFiled = this.View.GetControl<RepositoryItemSearchLookUpEdit>(Const_Distribution.Entry_Control_StockAreaPlan);
            planFiled.Bind(ds);
        }
    }
}
