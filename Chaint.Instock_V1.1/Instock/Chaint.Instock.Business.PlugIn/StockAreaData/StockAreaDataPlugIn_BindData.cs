using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraEditors.Repository;
using Chaint.Common.Core;
using Chaint.Common.Core.Utils;
using Chaint.Instock.ServiceHelper;
using Chaint.Common.Entity.Utils;
using Chaint.Instock.Core;
using DevExpress.XtraGrid.Views.Grid;

namespace Chaint.Instock.Business.PlugIns
{
    partial class StockAreaDataPlugIn
    {
        //记录库区-仓库对应关系
        private Dictionary<string, string> stockAreaToStockDic =
               new Dictionary<string, string>();
        //记录库区-库区总容量对应关系
        private Dictionary<string, int> stockAreaDic =
               new Dictionary<string, int>();
        //记录仓库信息
        private Dictionary<string, string> stockInfo =
               new Dictionary<string, string>();
        //记录产品信息
        private Dictionary<string, string> prdInfo =
               new Dictionary<string, string>();
        //记录库区数量信息
        private Dictionary<string, Tuple<int, decimal>> amountInfo = 
            new Dictionary<string, Tuple<int, decimal>>();
        //记录数量改变的分录
        private Dictionary<string,int> alterInfo = new Dictionary<string, int>();
        //卷筒包装方式
        private DataTable rollPackType = null;
        //平板包装方式
        private DataTable sheetPackType = null;
        private Dictionary<string,DataTable> stockDic =new Dictionary<string, DataTable>();

        private void InitData()
        {
            if (this.Context.CompanyCode != "2")
            {
                BindSpCustomer();
                BindStockAreaPlan();
                BindPaperGrade();
                BindCertification();
                BindTransportType();
                BindColor();
                BindSpecProductName();
                BindTrademarkStyle();
                BindWeightMode();
            }
            BindSpecCustomerName();
            BindStatus();
            BindProduct();
            BindStock();
            BindPaperType();
            string stocks = string.Join(",",stockInfo.Keys);
            string paperType = appConfiger.GetValue("PAPERTYPE", "Type", "2");
            this.View.SetValue(Const_StockAreaData.Head_Field_FHPapertype, paperType);
            BindInStockArea(stocks);
        }
        private void BindStatus()
        {
            DataTable dtSource = new DataTable();
            dtSource.Columns.Add("FNAME", typeof(string));
            dtSource.Columns.Add("FNUMBER", typeof(int));
            dtSource.Columns["FNAME"].Caption = "名称";
            dtSource.Columns["FNUMBER"].Caption = "编码";
            DataRow dRow1 = dtSource.NewRow();
            dRow1["FNAME"] = "初始化数据";
            dRow1["FNUMBER"] = 0;
            DataRow dRow2 = dtSource.NewRow();
            dRow2["FNAME"] = "计划数据";
            dRow2["FNUMBER"] = 1;
            DataRow dRow3 = dtSource.NewRow();
            dRow3["FNAME"] = "计划完成";
            dRow3["FNUMBER"] = 2;
            if (type != 2)
            {
                dtSource.Rows.Add(dRow1);
            }
            dtSource.Rows.Add(dRow2);
            dtSource.Rows.Add(dRow3);
            dtSource.AcceptChanges();
            var field = this.View.GetControl<RepositoryItemSearchLookUpEdit>(Const_StockAreaData.Entry_Control_Status);
            var headField = this.View.GetControl<CheckedComboBoxEdit>(Const_StockAreaData.Head_Field_FStatus);
            DataSource ds = new DataSource();
            ds.DisplayMember = "FNAME";
            ds.ValueMember = "FNUMBER";
            ds.Data = dtSource;
            field.Bind(ds);
            headField.Bind(ds);
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
            var field = this.View.GetControl<RepositoryItemSearchLookUpEdit>(Const_StockAreaData.Entry_Control_SpCustomer);
            field.Bind(ds);
        }
        private void BindStockAreaPlan()
        {
            OperateResult result =
                StockAreaServiceHelper.GetStockAreaPlan(this.Context);
            if (!result.IsSuccess) return;
            if (result.ResultDataTable.IsEmpty()) return;
            result.ResultDataTable.Columns["FNAME"].Caption = "名称";
            result.ResultDataTable.Columns["FNUMBER"].Caption = "编码";
            var field = this.View.GetControl<RepositoryItemSearchLookUpEdit>(Const_StockAreaData.Entry_Control_FStockAreaPlan);
            DataSource ds = new DataSource();
            ds.DisplayTitle = "库区计划";
            ds.DisplayMember = "FNAME";
            ds.ValueMember = "FNUMBER";
            ds.Data = result.ResultDataTable;
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
            prdInfo.Clear();
            foreach (DataRow row in result.ResultDataTable.Rows)
            {
                string productId = Convert.ToString(row["WLBH"]);
                string productName = Convert.ToString(row["WLMC"]);
                if (!prdInfo.Keys.Contains(productId))
                {
                    prdInfo.Add(productId,productName);
                }
            }
            DataSource ds = new DataSource();
            ds.DisplayMember = "WLMC";
            ds.ValueMember = "WLBH";
            ds.Data = result.ResultDataTable;
            var field = this.View.GetControl<RepositoryItemSearchLookUpEdit>(Const_StockAreaData.Entry_Control_FProduct);
            var headField = this.View.GetControl<SearchLookUpEdit>(Const_StockAreaData.Head_Field_FHProduct);
            field.Bind(ds);
            headField.Bind(ds);
        }
        private void BindStock()
        {
            stockDic.Clear();
            stockAreaDic.Clear();
            stockInfo.Clear();
            stockAreaToStockDic.Clear();
            OperateResult result =
              StockAreaDataServiceHelper.GetStockAreaInfo(this.Context);
            if (!result.IsSuccess)
            {
                ChaintMessageBox.Show("发生错误，请查看日志！");
                return;
            }
            DataTable dtStock = new DataTable();
            dtStock.Columns.Add("FNAME", typeof(string));
            dtStock.Columns.Add("FNUMBER", typeof(string));
            dtStock.Columns["FNAME"].Caption = "名称";
            dtStock.Columns["FNUMBER"].Caption = "编码";
            DataTable dtArea = new DataTable();
            dtArea.Columns.Add("FNAME", typeof(string));
            dtArea.Columns.Add("FNUMBER", typeof(string));
            dtArea.Columns["FNAME"].Caption = "名称";
            dtArea.Columns["FNUMBER"].Caption = "编号";
            List<string> areaAdded = new List<string>();
            List<string> stockAdded = new List<string>();
            foreach (var item in result.ResultTable)
            {
                string stockName = Convert.ToString(item["FSTOCKNAME"]);
                string stockNumber = Convert.ToString(item["FSTOCK"]);
                string stockAreaNumber = Convert.ToString(item[Const_StockArea.Entry_Column_FStockAreaNumber]);
                string stockAreaName = Convert.ToString(item[Const_StockArea.Entry_Column_FStockAreaName]);
                int totalCapacity = Convert.ToInt32(item[Const_StockArea.Entry_Column_FTotalCapacity]);
                if (!stockAreaToStockDic.Keys.Contains(stockAreaNumber))
                {
                    stockAreaToStockDic.Add(stockAreaNumber, stockNumber);
                }
                if (!stockInfo.Keys.Contains(stockNumber))
                {
                    stockInfo.Add(stockNumber, stockName);
                }
                if (!areaAdded.Contains(stockAreaNumber))
                {
                    areaAdded.Add(stockAreaNumber);
                    DataRow dRow = dtArea.NewRow();
                    dRow["FNUMBER"] = stockAreaNumber;
                    dRow["FNAME"] = stockAreaName;
                    dtArea.Rows.Add(dRow);
                    areaAdded.Add(stockAreaNumber);
                    if (stockDic.Keys.Contains(stockNumber))
                    {
                        DataRow item1 = stockDic[stockNumber].NewRow();
                        item1["FNUMBER"] = stockAreaNumber;
                        item1["FNAME"] = stockAreaName;
                        stockDic[stockNumber].Rows.Add(item1);
                        stockDic[stockNumber].AcceptChanges();
                    }
                    else
                    {
                        DataTable dtItem = new DataTable();
                        dtItem.Columns.Add("FNAME", typeof(string));
                        dtItem.Columns.Add("FNUMBER", typeof(string));
                        dtItem.Columns["FNAME"].Caption = "名称";
                        dtItem.Columns["FNUMBER"].Caption = "编号";
                        DataRow item2 = dtItem.NewRow();
                        item2["FNUMBER"] = stockAreaNumber;
                        item2["FNAME"] = stockAreaName;
                        dtItem.Rows.Add(item2);
                        dtItem.AcceptChanges();
                        stockDic.Add(stockNumber, dtItem);
                    }
                }
                if (!stockAdded.Contains(stockNumber))
                {
                    stockAdded.Add(stockNumber);
                    DataRow dRow = dtStock.NewRow();
                    dRow["FNUMBER"] = stockNumber;
                    dRow["FNAME"] = stockName;
                    dtStock.Rows.Add(dRow);
                }
                if (!stockAreaDic.Keys.Contains(stockAreaNumber))
                {
                    stockAreaDic.Add(stockAreaNumber, totalCapacity);
                }
            }
            dtStock.AcceptChanges();
            dtArea.AcceptChanges();
            var field = this.View.GetControl<RepositoryItemSearchLookUpEdit>(Const_StockAreaData.Entry_Control_Stock);
            var headStock = this.View.GetControl<CheckedComboBoxEdit>(Const_StockAreaData.Head_Field_FHStock);
            DataSource ds = new DataSource();
            ds.DisplayTitle = "仓库";
            ds.DisplayMember = "FNAME";
            ds.ValueMember = "FNUMBER";
            ds.Data = dtStock;
            field.Bind(ds);
            headStock.Bind(ds);

            var inStockArea = this.View.GetControl<RepositoryItemSearchLookUpEdit>(Const_StockAreaData.Entry_Control_FInStockArea);
            var stockArea = this.View.GetControl<RepositoryItemSearchLookUpEdit>(Const_StockAreaData.Entry_Control_StockArea);
            DataSource dsArea = new DataSource();
            dsArea.DisplayMember = "FNAME";
            dsArea.ValueMember = "FNUMBER";
            dsArea.Data = dtArea;
            inStockArea.Bind(ds);
            stockArea.Bind(ds);
        }
        private void BindStockArea(string strStock)
        {
            string[] stocks = strStock.Split(',');
            if (stocks.Length < 1) return;
            DataTable dtSource = new DataTable();
            dtSource.Columns.Add("FNAME", typeof(string));
            dtSource.Columns.Add("FNUMBER", typeof(string));
            dtSource.Columns["FNAME"].Caption = "名称";
            dtSource.Columns["FNUMBER"].Caption = "编号";
            foreach (string item in stocks)
            {
                DataTable areaItems;
                if (!stockDic.TryGetValue(item.Trim(), out areaItems)) continue;
                if (areaItems.IsEmpty()) continue;
                foreach (DataRow row in areaItems.Rows)
                {
                    DataRow newRow = dtSource.NewRow();
                    newRow.ItemArray  = row.ItemArray.Clone<object[]>();
                    dtSource.Rows.Add(newRow);
                }
            }
            dtSource.AcceptChanges();
            var headStockArea = this.View.GetControl<CheckedComboBoxEdit>(Const_StockAreaData.Head_Field_FHStockArea);
            DataSource ds = new DataSource();
            ds.DisplayMember = "FNAME";
            ds.ValueMember = "FNUMBER";
            ds.Data = dtSource;
            headStockArea.Bind(ds);
        }
        private void BindInStockArea(string strStock)
        {
            string[] stocks = strStock.Split(',');
            if (stocks.Length < 1) return;
            DataTable dtSource = new DataTable();
            dtSource.Columns.Add("FNAME", typeof(string));
            dtSource.Columns.Add("FNUMBER", typeof(string));
            dtSource.Columns["FNAME"].Caption = "名称";
            dtSource.Columns["FNUMBER"].Caption = "编号";
            foreach (string item in stocks)
            {
                DataTable areaItems;
                if (!stockDic.TryGetValue(item.Trim(), out areaItems)) continue;
                if (areaItems.IsEmpty()) continue;
                foreach (DataRow row in areaItems.Rows)
                {
                    DataRow newRow = dtSource.NewRow();
                    newRow.ItemArray = row.ItemArray.Clone<object[]>();
                    dtSource.Rows.Add(newRow);
                }
            }
            dtSource.AcceptChanges();
            var inStockArea = this.View.GetControl<RepositoryItemSearchLookUpEdit>(Const_StockAreaData.Entry_Control_FInStockArea);
            var stockArea = this.View.GetControl<RepositoryItemSearchLookUpEdit>(Const_StockAreaData.Entry_Control_StockArea);
            DataSource ds = new DataSource();
            ds.DisplayMember = "FNAME";
            ds.ValueMember = "FNUMBER";
            ds.Data = dtSource;
            inStockArea.Bind(ds);
            stockArea.Bind(ds);
        }
        private void SetData()
        {
            if (!QueryValidate()) return;
            string[] status = this.View.GetValue<string>(Const_StockAreaData.Head_Field_FStatus).Split(',');
            int[] statusList = status.Where(w => !w.IsNullOrEmptyOrWhiteSpace()).
                Select(s => Convert.ToInt32(s)).ToArray();
            string[] stocks = this.View.GetValue<string>(Const_StockAreaData.Head_Field_FHStock).Split(',');
            string[] stockAreas = this.View.GetValue<string>(Const_StockAreaData.Head_Field_FHStockArea).Split(',');
            string[] stockList = stocks.Where(w => !w.IsNullOrEmptyOrWhiteSpace()).
                Select(s => Convert.ToString(s)).ToArray();
            string[] stockAreaList = stockAreas.Where(w => !w.IsNullOrEmptyOrWhiteSpace()).
                Select(s => Convert.ToString(s)).ToArray();
            if (type == 3|| type == 4)
            {
                statusList = new int[] { 3 };
            }
            if (type == -1)
            {
                statusList = new int[] { -1,0 };
            }
            if (statusList.IsEmpty())
            {
                ChaintMessageBox.Show("请选择状态!");
                return;
            }
            DateTime beginDate = this.View.GetValue<DateTime>(Const_StockAreaData.Head_Field_FBeginDate);
            DateTime endDate = this.View.GetValue<DateTime>(Const_StockAreaData.Head_Field_FEndDate);
            string product = this.View.GetValue<string>(Const_StockAreaData.Head_Field_FHProduct);
            string specification = this.View.GetValue<string>(Const_StockAreaData.Head_Field_FHSpecification);
            string paperType = this.View.GetValue<string>(Const_StockAreaData.Head_Field_FHPapertype);
            OperateResults result =
                StockAreaDataServiceHelper.GetStockDataInfo(this.Context, product, paperType, specification, stockList, stockAreaList, beginDate, endDate, statusList);
            if (!result.IsSuccess)
            {
                ChaintMessageBox.Show("发生错误，请查看日志!");
                return;
            }
            if (type == 3)
            {
                amountInfo.Clear();
                foreach (DataRow row in result.ResultData.Tables["T1"].Rows)
                {
                    string entryId = row.GetValue<string>(Const_StockAreaData.Entry_Column_FEntryId);
                    int amount = row.GetValue<int>(Const_StockAreaData.Entry_Column_FAmount);
                    decimal weight = row.GetValue<decimal>(Const_StockAreaData.Entry_Column_FWeight);
                    if (amountInfo.Keys.Contains(entryId)) continue;
                    amountInfo.Add(entryId, new Tuple<int, decimal>(amount, weight));
                }
            }
            GridControl gc_Data
                = this.View.GetControl<GridControl>(Const_StockAreaData.Entry_Control);
            gc_Data.DataSource = result.ResultData.Tables["T1"];
            this.View.GetControl<GridView>(Const_StockAreaData.Entry_Entry).BestFitColumns();
            this.View.Model.BindEntryData(Const_StockAreaData.Entry_TableName, result.ResultData.Tables["T1"]);
        }
        private bool QueryValidate()
        {
            //DateTime beginDate = this.View.GetValue<DateTime>(Const_StockAreaData.Head_Field_FBeginDate);
            //DateTime endDate = this.View.GetValue<DateTime>(Const_StockAreaData.Head_Field_FEndDate);
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
            if (type != 3 && type != 4) return true;
            string specification = this.View.GetValue<string>(Const_StockAreaData.Head_Field_FHSpecification);
            if (specification.IsNullOrEmptyOrWhiteSpace())
            {
                ChaintMessageBox.Show("请输入规格！");
                return false;
            }
            //string[] stocks = this.View.GetValue<string>(Const_StockAreaData.Head_Field_FHStock).Split(',');
            //string[] stockList = stocks.Where(w => !w.IsNullOrEmptyOrWhiteSpace()).ToArray();
            //if (stockList == null || stockList.Length < 1)
            //{
            //    ChaintMessageBox.Show("请选择仓库！");
            //    return false;
            //}
            //string[] stockAreas = this.View.GetValue<string>(Const_StockAreaData.Head_Field_FHStockArea).Split(',');
            //string[] stockAreaList = stockAreas.Where(w => !w.IsNullOrEmptyOrWhiteSpace()).ToArray();
            //if (stockAreaList == null || stockAreaList.Length < 1)
            //{
            //    ChaintMessageBox.Show("请选择库区！");
            //    return false;
            //}
            return true;
        }
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
            var field = this.View.GetControl<RepositoryItemSearchLookUpEdit>(Const_StockAreaData.Entry_Control_FPaperType);
            var hField = this.View.GetControl<SearchLookUpEdit>(Const_StockAreaData.Head_Field_FHPapertype);
            field.Bind(ds);
            hField.Bind(ds);
        }
        private void BindCoreDiameter()
        {
            OperateResult result =
                         StockAreaPlanServiceHelper.GetCoreDiameter(this.Context);
            if (result.ResultDataTable.IsEmpty()) return;
            result.ResultDataTable.Columns["COREDIAMETER"].Caption = "名称";
            DataSource ds = new DataSource();
            ds.DisplayMember = "COREDIAMETER";
            ds.Data = result.ResultDataTable;
            var field = this.View.GetControl<RepositoryItemComboBox>(Const_StockAreaData.Entry_Control_FCoreDiameterOrReam);
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
            var field = this.View.GetControl<RepositoryItemSearchLookUpEdit>(Const_StockAreaData.Entry_Control_FPaperGrade);
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
            var field = this.View.GetControl<RepositoryItemSearchLookUpEdit>(Const_StockAreaData.Entry_Control_FCertification);
            field.Bind(ds);
        }
        /// <summary>
        /// 绑定包装方式
        /// </summary>
        private void BindPackType(string paperType)
        {
            DataTable dt = null;
            if (paperType == "1")
            {
                dt = rollPackType;
            }
            else
            {
                dt =  sheetPackType;
            }
            if (dt == null)
            {
                OperateResult result =
                            StockAreaPlanServiceHelper.GetPackType(this.Context, paperType);
                if (result.ResultDataTable.IsEmpty()) return;
                result.ResultDataTable.Columns["PACKTYPE"].Caption = "名称";
                dt = result.ResultDataTable;
                if (paperType == "1")
                {
                    rollPackType = result.ResultDataTable;
                }
                else
                {
                    sheetPackType = result.ResultDataTable;
                }
            }
            DataSource ds = new DataSource();
            ds.DisplayMember = "PACKTYPE";
            ds.ValueMember = "PACKTYPE";
            ds.Data = dt;
            var field = this.View.GetControl<RepositoryItemSearchLookUpEdit>(Const_StockAreaData.Entry_Control_FPackType);
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
            var field = this.View.GetControl<RepositoryItemSearchLookUpEdit>(Const_StockAreaData.Entry_Control_FTransportType);
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
            var field = this.View.GetControl<RepositoryItemSearchLookUpEdit>(Const_StockAreaData.Entry_Control_FColor);
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
            var field = this.View.GetControl<RepositoryItemSearchLookUpEdit>(Const_StockAreaData.Entry_Control_FSpecCustName);
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
            var field = this.View.GetControl<RepositoryItemSearchLookUpEdit>(Const_StockAreaData.Entry_Control_FSpecProdName);
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
            var field = this.View.GetControl<RepositoryItemSearchLookUpEdit>(Const_StockAreaData.Entry_Control_FTrademarkStyle);
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
            var field = this.View.GetControl<RepositoryItemSearchLookUpEdit>(Const_StockAreaData.Entry_Control_FWeightMode);
            field.Bind(ds);
        }
    }
}
