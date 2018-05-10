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

namespace Chaint.Instock.Business.PlugIns
{
    partial class StockAreaDataQueryPlugIn
    {
        //记录仓库-库区对应关系
        Dictionary<string, List<Tuple<string, string>>> stockDic =
               new Dictionary<string, List<Tuple<string, string>>>();
        Dictionary<string, string> stocks = new Dictionary<string, string>();
        private void BindPaperType()
        {
            DataTable dtSource = new DataTable();
            dtSource.Columns.Add("TypeId", typeof(string));
            dtSource.Columns.Add("TypeName", typeof(string));
            dtSource.Columns["TypeId"].Caption = "编号";
            dtSource.Columns["TypeName"].Caption = "名称";
            DataRow dRow1 = dtSource.NewRow();
            dRow1["TypeId"] = "1";
            dRow1["TypeName"] = "卷筒纸";
            dtSource.Rows.Add(dRow1);
            DataRow dRow2 = dtSource.NewRow();
            dRow2["TypeId"] = "2";
            dRow2["TypeName"] = "平板纸";
            dtSource.Rows.Add(dRow2);
            dtSource.AcceptChanges();
            DataSource ds = new DataSource();
            ds.DisplayMember = "TypeName";
            ds.ValueMember = "TypeId";
            ds.Data = dtSource;
            var field = this.View.GetControl<SearchLookUpEdit>(Const_StockAreaDataQuery.Head_Field_FPaperType);
            field.Bind(ds);
        }
        private void BindSpCustomer()
        {
            OperateResult result =
                       StockAreaPlanServiceHelper.GetSpCustomer(this.Context);
            if (result.ResultDataTable.IsEmpty()) return;
            result.ResultDataTable.Columns["NAME"].Caption = "名称";
            DataSource ds = new DataSource();
            ds.DisplayMember = "NAME";
            ds.ValueMember = "NAME";
            ds.Data = result.ResultDataTable;
            var field = this.View.GetControl<SearchLookUpEdit>(Const_StockAreaDataQuery.Head_Field_FSpCustomer);
            field.Bind(ds);
        }
        /// <summary>
        /// 绑定纸芯数据
        /// </summary>
        private void BindCoreDiameter()
        {
            OperateResult result =
                          StockAreaPlanServiceHelper.GetCoreDiameter(this.Context);
            if (result.ResultDataTable.IsEmpty()) return;
            result.ResultDataTable.Columns["COREDIAMETER"].Caption = "名称";
            DataSource ds = new DataSource();
            ds.DisplayMember = "COREDIAMETER";
            ds.Data = result.ResultDataTable;
            var field = this.View.GetControl<ComboBoxEdit>(Const_StockAreaDataQuery.Head_Field_FCoreDiameterOrReam);
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
            var field = this.View.GetControl<CheckedComboBoxEdit>(Const_StockAreaDataQuery.Head_Field_FStock);
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
            var field = this.View.GetControl<CheckedComboBoxEdit>(Const_StockAreaDataQuery.Head_Field_FStockArea);
            DataSource ds = new DataSource();
            ds.DisplayMember = "FNAME";
            ds.ValueMember = "FNUMBER";
            ds.Data = dtSource;
            field.Bind(ds);
        }
        /// <summary>
        /// 绑定产品等级
        /// </summary>
        private void BindPaperGrade()
        {
            OperateResult result =
                       StockAreaPlanServiceHelper.GetPaperGrade(this.Context);
            if (result.ResultDataTable.IsEmpty()) return;
            result.ResultDataTable.Columns["GRADEDESC"].Caption = "名称";
            DataSource ds = new DataSource();
            ds.DisplayMember = "GRADEDESC";
            ds.ValueMember = "GRADEDESC";
            ds.Data = result.ResultDataTable;
            var field = this.View.GetControl<SearchLookUpEdit>(Const_StockAreaDataQuery.Head_Field_FPaperGrade);
            field.Bind(ds);
        }
        /// <summary>
        /// 绑定产品认证
        /// </summary>
        private void BindCertification()
        {
            OperateResult result =
                         StockAreaPlanServiceHelper.GetPaperCertification(this.Context);
            if (result.ResultDataTable.IsEmpty()) return;
            result.ResultDataTable.Columns["PAPERCERT"].Caption = "名称";
            DataSource ds = new DataSource();
            ds.DisplayMember = "PAPERCERT";
            ds.ValueMember = "PAPERCERT";
            ds.Data = result.ResultDataTable;
            var field = this.View.GetControl<SearchLookUpEdit>(Const_StockAreaDataQuery.Head_Field_FCertification);
            field.Bind(ds);
        }
        /// <summary>
        /// 绑定包装方式
        /// </summary>
        private void BindPackType(string paperType)
        {
            OperateResult result =
                        StockAreaPlanServiceHelper.GetPackType(this.Context, paperType);
            if (result.ResultDataTable.IsEmpty()) return;
            result.ResultDataTable.Columns["PACKTYPE"].Caption = "名称";
            DataSource ds = new DataSource();
            ds.DisplayMember = "PACKTYPE";
            ds.ValueMember = "PACKTYPE";
            ds.Data = result.ResultDataTable;
            var field = this.View.GetControl<SearchLookUpEdit>(Const_StockAreaDataQuery.Head_Field_FPackType);
            field.Bind(ds);
        }
        /// <summary>
        /// 绑定夹板包装
        /// </summary>
        private void BindTransportType()
        {
            OperateResult result =
                       StockAreaPlanServiceHelper.GetTransportType(this.Context);
            if (result.ResultDataTable.IsEmpty()) return;
            result.ResultDataTable.Columns["TRANSPORTTYPE"].Caption = "名称";

            DataSource ds = new DataSource();
            ds.DisplayMember = "TRANSPORTTYPE";
            ds.ValueMember = "TRANSPORTTYPE";
            ds.Data = result.ResultDataTable;
            var field = this.View.GetControl<SearchLookUpEdit>(Const_StockAreaDataQuery.Head_Field_FTransportType);
            field.Bind(ds);
        }
        /// <summary>
        /// 绑定产品色相
        /// </summary>
        private void BindColor()
        {
            OperateResult result =
                          StockAreaPlanServiceHelper.GetPaperColor(this.Context);
            if (result.ResultDataTable.IsEmpty()) return;
            result.ResultDataTable.Columns["COLORDESC"].Caption = "名称";

            DataSource ds = new DataSource();
            ds.DisplayMember = "COLORDESC";
            ds.ValueMember = "COLORDESC";
            ds.Data = result.ResultDataTable;
            var field = this.View.GetControl<SearchLookUpEdit>(Const_StockAreaDataQuery.Head_Field_FColor);
            field.Bind(ds);
        }
        /// <summary>
        /// 绑定客户专用
        /// </summary>
        private void BindSpecCustomerName()
        {
            OperateResult result =
                  StockAreaPlanServiceHelper.GetSpecCustomerName(this.Context);
            if (result.ResultDataTable.IsEmpty()) return;
            result.ResultDataTable.Columns["SPECCUSTNAME"].Caption = "名称";
            DataSource ds = new DataSource();
            ds.DisplayMember = "SPECCUSTNAME";
            ds.ValueMember = "SPECCUSTNAME";
            ds.Data = result.ResultDataTable;
            var field = this.View.GetControl<SearchLookUpEdit>(Const_StockAreaDataQuery.Head_Field_FSpecCustName);
            field.Bind(ds);
        }
        /// <summary>
        /// 绑定产品专用
        /// </summary>
        private void BindSpecProductName()
        {
            OperateResult result =
                       StockAreaPlanServiceHelper.GetSpecProductName(this.Context);
            if (result.ResultDataTable.IsEmpty()) return;
            result.ResultDataTable.Columns["SPECPRODNAME"].Caption = "名称";
            DataSource ds = new DataSource();
            ds.DisplayMember = "SPECPRODNAME";
            ds.ValueMember = "SPECPRODNAME";
            ds.Data = result.ResultDataTable;
            var field = this.View.GetControl<SearchLookUpEdit>(Const_StockAreaDataQuery.Head_Field_FSpecProdName);
            field.Bind(ds);
        }
        /// <summary>
        /// 绑定合格证样式
        /// </summary>
        private void BindTrademarkStyle()
        {
            DataTable dtSource = new DataTable();
            dtSource.Columns.Add("TrademarkStyle", typeof(string));
            dtSource.Columns["TrademarkStyle"].Caption = "名称";
            DataRow dRow1 = dtSource.NewRow();
            dRow1["TrademarkStyle"] = "白证";
            DataRow dRow2 = dtSource.NewRow();
            dRow2["TrademarkStyle"] = "出口证";
            dtSource.Rows.Add(dRow1);
            dtSource.Rows.Add(dRow2);
            dtSource.AcceptChanges();
            DataSource ds = new DataSource();
            ds.DisplayMember = "TrademarkStyle";
            ds.ValueMember = "TrademarkStyle";
            ds.Data = dtSource;
            var field = this.View.GetControl<SearchLookUpEdit>(Const_StockAreaDataQuery.Head_Field_FTrademarkStyle);
            field.Bind(ds);
        }
        /// <summary>
        /// 绑定计重方式
        /// </summary>
        private void BindWeightMode()
        {
            DataTable dtSource = new DataTable();
            dtSource.Columns.Add("WeightModeDesc", typeof(string));
            dtSource.Columns["WeightModeDesc"].Caption = "名称";
            DataRow dRow1 = dtSource.NewRow();
            dRow1["WeightModeDesc"] = "计重";
            DataRow dRow2 = dtSource.NewRow();
            dRow2["WeightModeDesc"] = "计米";
            DataRow dRow3 = dtSource.NewRow();
            dRow3["WeightModeDesc"] = "注米";
            DataRow dRow4 = dtSource.NewRow();
            dRow4["WeightModeDesc"] = "正丝";
            DataRow dRow5 = dtSource.NewRow();
            dRow5["WeightModeDesc"] = "反丝";
            dtSource.Rows.Add(dRow1);
            dtSource.Rows.Add(dRow2);
            dtSource.Rows.Add(dRow3);
            dtSource.Rows.Add(dRow4);
            dtSource.Rows.Add(dRow5);
            dtSource.AcceptChanges();
            DataSource ds = new DataSource();
            ds.DisplayMember = "WeightModeDesc";
            ds.ValueMember = "WeightModeDesc";
            ds.Data = dtSource;
            var field = this.View.GetControl<SearchLookUpEdit>(Const_StockAreaDataQuery.Head_Field_FWeightMode);
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
            var field = this.View.GetControl<SearchLookUpEdit>(Const_StockAreaDataQuery.Head_Field_FProduct);
            field.Bind(ds);
        }
    }
}
