using System;
using System.Data;
using System.Data.SqlClient;
using DevExpress.XtraEditors;
using DevExpress.XtraBars;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraLayout;
using Chaint.Common.Core.Utils;
using Chaint.Common.BasePlugIn;
using Chaint.Common.Core.EventArgs;
using Chaint.Instock.Core;
using Chaint.Instock.Business.View;
using System.Collections.Generic;
using Chaint.Common.Core;
using Chaint.Common.Core.AppConfig;
using Chaint.Common.Entity.Utils;
using Chaint.Common.ServiceHelper;

namespace Chaint.Instock.Business.PlugIns
{
    public partial class StockAreaDataPlugIn : AbstractBillPlugIn
    {
        private XtraForm form;
        private int type = -1;
        private AppConfig_INI appConfiger;
        public StockAreaDataPlugIn(StockAreaDataView view) : base(view)
        {
            form = this.View.GetControl<XtraForm>(Const_StockAreaData.Base_Form);
            appConfiger = new AppConfig_INI(view.Context.DevicesConfigFilePath);
        }
        public override void OnSetBusinessInfo()
        {
            base.OnSetBusinessInfo();
            InitView();
            InitData();
        }
        public override void OnBind()
        {
            base.OnBind();
            this.View.SetValue(Const_StockAreaData.Head_Field_FBeginDate, DateTime.Now);
            this.View.SetValue(Const_StockAreaData.Head_Field_FEndDate, DateTime.Now.AddDays(1));
            if (type == 0)//初始化数据
            {
                this.View.SetValue(Const_StockAreaData.Head_Field_FStatus, "0");
            }
            if (type == 1)//新增数据计划
            {
                this.View.SetValue(Const_StockAreaData.Head_Field_FStatus, "1");
            }
            if (type == 2)//表示数据计划管理
            {
                this.View.SetValue(Const_StockAreaData.Head_Field_FStatus, "0,1");
            }
            BindEntry();
        }
        private void BindEntry()
        {
            DataTable dataSource = new DataTable();
            dataSource.Columns.Add(Const_StockAreaData.Entry_Column_FCheck, typeof(int));
            dataSource.Columns.Add(Const_StockAreaData.Entry_Column_FEntryId, typeof(string));
            dataSource.Columns.Add(Const_StockAreaData.Entry_Column_FName, typeof(string));
            dataSource.Columns.Add(Const_StockAreaData.Entry_Column_FProduct, typeof(string));
            dataSource.Columns.Add(Const_StockAreaData.Entry_Column_FStock, typeof(string));
            dataSource.Columns.Add(Const_StockAreaData.Entry_Column_FStockArea, typeof(string));
            dataSource.Columns.Add(Const_StockAreaData.Entry_Column_FStockAreaPlan, typeof(string));
            dataSource.Columns.Add(Const_StockAreaData.Entry_Column_FAmount, typeof(int));
            dataSource.Columns.Add(Const_StockAreaData.Entry_Column_FPlanAmount, typeof(int));
            dataSource.Columns.Add(Const_StockAreaData.Entry_Column_FStatus, typeof(int));
            dataSource.Columns.Add(Const_StockAreaData.Entry_Column_FDate, typeof(DateTime));

            dataSource.Columns.Add(Const_StockAreaData.Entry_Column_FPaperType, typeof(string));
            dataSource.Columns.Add(Const_StockAreaData.Entry_Column_FPaperGrade, typeof(string));
            dataSource.Columns.Add(Const_StockAreaData.Entry_Column_FSpecification, typeof(string));
            dataSource.Columns.Add(Const_StockAreaData.Entry_Column_FCertification, typeof(string));
            dataSource.Columns.Add(Const_StockAreaData.Entry_Column_FTransportType, typeof(string));
            dataSource.Columns.Add(Const_StockAreaData.Entry_Column_FTrademarkStyle, typeof(string));
            dataSource.Columns.Add(Const_StockAreaData.Entry_Column_FCoreDiameterOrReam, typeof(decimal));
            dataSource.Columns.Add(Const_StockAreaData.Entry_Column_FDiameterOrSlides, typeof(decimal));
            dataSource.Columns.Add(Const_StockAreaData.Entry_Column_FLength, typeof(decimal));
            dataSource.Columns.Add(Const_StockAreaData.Entry_Column_FSpecCustName, typeof(string));
            dataSource.Columns.Add(Const_StockAreaData.Entry_Column_FSpecProdName, typeof(string));
            dataSource.Columns.Add(Const_StockAreaData.Entry_Column_FPackType, typeof(string));
            dataSource.Columns.Add(Const_StockAreaData.Entry_Column_FDeliverDate, typeof(DateTime));
            dataSource.Columns.Add(Const_StockAreaData.Entry_Column_FWeightMode, typeof(string));
            dataSource.Columns.Add(Const_StockAreaData.Entry_Column_FColor, typeof(string));
            dataSource.Columns.Add(Const_StockAreaData.Entry_Column_FPoNumber, typeof(string));
            dataSource.Columns.Add(Const_StockAreaData.Entry_Column_FMemo, typeof(string));
            dataSource.Columns.Add(Const_StockAreaData.Entry_Column_FCustomer, typeof(int));
            dataSource.Columns.Add(Const_StockAreaData.Entry_Column_FStockDate, typeof(string));
            dataSource.Columns.Add(Const_StockAreaData.Entry_Column_FOutAmount, typeof(int));
            dataSource.Columns.Add(Const_StockAreaData.Entry_Column_FInStockArea, typeof(string));
            dataSource.Columns.Add(Const_StockAreaData.Entry_Column_FWeight, typeof(decimal));
            dataSource.Columns.Add(Const_StockAreaData.Entry_Column_FOutWeight, typeof(decimal));
            dataSource.Columns.Add(Const_StockAreaData.Entry_Column_FSpCustomer, typeof(string));
            dataSource.Columns.Add(Const_StockAreaData.Entry_Column_FTotalCapacity, typeof(int));
            dataSource.Columns.Add(Const_StockAreaData.Entry_Column_FUsedCapacity, typeof(int));
            this.View.GetControl<GridControl>("gc_Data").DataSource = dataSource;
            this.View.Model.BindEntryData(Const_StockAreaData.Entry_TableName, dataSource);
        }
        public override void ButtonClick(ButtonClickEventArgs e)
        {
            base.ButtonClick(e);
            switch (e.Sender)
            {
                case Const_StockAreaData.Entry_Menu_NewEntry:
                    var gridNew = this.View.GetControl<GridView>(Const_StockAreaData.Entry_Entry);
                    gridNew.AddNewRow();
                    break;
                case Const_StockAreaData.Entry_Menu_CopyEntry:
                    CopyToNewRow();
                    break;
                case Const_StockAreaData.Entry_Menu_DeleteEntry:
                    var gridDel = this.View.GetControl<GridView>(Const_StockAreaData.Entry_Entry);
                    int row = gridDel.FocusedRowHandle;
                    gridDel.DeleteRow(row);
                    gridDel.UpdateCurrentRow();
                    break;
                case Const_StockAreaData.Head_Menu_Save:
                    SaveToDB();
                    break;
                case Const_StockAreaData.Head_Menu_Update:
                    UpdateStockData();
                    break;
                case Const_StockAreaData.Head_Menu_Query:
                    SetData();
                    break;
                case Const_StockAreaData.Entry_Menu_Complete:
                    Complete();
                    break;
                case Const_StockAreaData.Head_Menu_Print:
                    Print();
                    break;
                case Const_StockAreaData.Head_Menu_PrintPreview:
                    PrintPreview();
                    break;
                case Const_StockAreaData.Head_Menu_PrintTemplet:
                    SetPrintTemplet();
                    break;
                case Const_StockAreaData.Head_Menu_Export:
                    ExportToExcel();
                    break;
            }
        }

        public override void DataChanged(DataChangedEventArgs e)
        {
            base.DataChanged(e);
            switch (e.Sender)
            {
                case Const_StockAreaData.Entry_Field_FStockArea:
                    string stockArea = Convert.ToString(e.Value);
                    SetAmount(e.Row, stockArea);
                    string stock = string.Empty;
                    stockAreaToStockDic.TryGetValue(stockArea,out stock);
                    this.View.SetValue(Const_StockAreaData.Entry_Entry,
                        Const_StockAreaData.Entry_Column_FStock, stock, e.Row);
                    break;
                case Const_StockAreaData.Entry_Field_FStockAreaPlan:
                    GetStockAreaPlan(e);
                    break;
                case Const_StockAreaData.Head_Field_FHStock:
                    string hStock = this.View.GetValue<string>(Const_StockAreaData.Head_Field_FHStock);
                    if (hStock.IsNullOrEmptyOrWhiteSpace()) return;
                    BindStockArea(hStock);
                    break;
                case Const_StockAreaData.Entry_Field_FOutAmount:
                    if (type == 3)
                    {
                        int amount = this.View.Model.GetValue<int>(Const_StockAreaData.Entry_Field_FAmount, e.Row);
                        int outAmount = this.View.Model.GetValue<int>(Const_StockAreaData.Entry_Field_FOutAmount, e.Row);
                        this.View.Model.SetValue(Const_StockAreaData.Entry_Field_FAmount, amount - outAmount, e.Row);
                        this.View.Model.SetValue(Const_StockAreaData.Entry_Field_FOutAmount, 0, e.Row);
                        string entryId = this.View.Model.GetValue<string>(Const_StockAreaData.Entry_Field_FEntryId,e.Row);
                        Tuple<int, decimal> oldAmount;
                        amountInfo.TryGetValue(entryId,out oldAmount);
                        if (oldAmount.Item1 != amount - outAmount)
                        {
                            if (!alterInfo.ContainsKey(entryId))
                            {
                                alterInfo.Add(entryId, e.Row);
                            }
                        }
                        else
                        {
                            if (alterInfo.ContainsKey(entryId))
                            {
                                alterInfo.Remove(entryId);
                            }
                        }
                    }
                    this.View.Model.SetValue(Const_StockAreaData.Entry_Field_FDate, DateTime.Now, e.Row);
                    break;
                case Const_StockAreaData.Entry_Field_FOutWeight:
                    if (type == 3)
                    {
                        decimal weight = this.View.Model.GetValue<decimal>(Const_StockAreaData.Entry_Field_FWeight, e.Row);
                        decimal outWeight = this.View.Model.GetValue<decimal>(Const_StockAreaData.Entry_Field_FOutWeight, e.Row);
                        this.View.Model.SetValue(Const_StockAreaData.Entry_Field_FWeight, weight - outWeight, e.Row);
                        this.View.Model.SetValue(Const_StockAreaData.Entry_Field_FOutWeight, 0, e.Row);
                        string entryId = this.View.Model.GetValue<string>(Const_StockAreaData.Entry_Field_FEntryId, e.Row);
                        Tuple<int, decimal> oldAmount;
                        amountInfo.TryGetValue(entryId, out oldAmount);
                        if (oldAmount.Item2 != weight - outWeight)
                        {
                            if (!alterInfo.ContainsKey(entryId))
                            {
                                alterInfo.Add(entryId, e.Row);
                            }
                        }
                        else
                        {
                            if (alterInfo.ContainsKey(entryId))
                            {
                                alterInfo.Remove(entryId);
                            }
                        }
                    }
                    this.View.Model.SetValue(Const_StockAreaData.Entry_Field_FDate, DateTime.Now, e.Row);
                    break;
                case Const_StockAreaData.Entry_Control_FCheckAll:
                    GridControl gcData =
                        this.View.GetControl<GridControl>(Const_StockAreaData.Entry_Control);
                    if (gcData.DataSource == null) return;
                    bool isCheck = this.View.GetValue<bool>(Const_StockAreaData.Entry_Control_FCheckAll);
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
                    this.View.Model.BindEntryData(Const_StockAreaData.Entry_TableName, dt);
                    break;
                case Const_StockAreaData.Head_Field_FHPapertype:
                    string paperType = this.View.GetValue<string>(Const_StockAreaData.Head_Field_FHPapertype);
                    if (paperType == "2")
                    {
                        BindCoreDiameter();
                    }
                    BindPackType(paperType);
                    break;
            }
        }
        public override void AfterDeleteEntryRow(AfterDeleteEntryRowEventArgs e)
        {
            base.AfterDeleteEntryRow(e);
            DataChangedEventArgs args = new DataChangedEventArgs();
            args.Row = e.Row;
            args.Value = 0;
        }
        public override void AfterCreateNewEntryRow(AfterCreateNewEntryRowEventArgs e)
        {
            base.AfterCreateNewEntryRow(e);
            this.View.SetValue(Const_StockAreaData.Entry_Entry, Const_StockAreaData.Entry_Column_FDate,
                DateTime.Now, e.Row);
            string paperType = this.View.GetValue<string>(Const_StockAreaData.Head_Field_FHPapertype);
            this.View.SetValue(Const_StockAreaData.Entry_Entry, Const_StockAreaData.Entry_Column_FPaperType, paperType, e.Row);
            this.View.SetValue(Const_StockAreaData.Entry_Entry, Const_StockAreaData.Entry_Column_FCheck,0, e.Row);
            if (type == 0)//历史数据
            {
                this.View.SetValue(Const_StockAreaData.Entry_Entry, Const_StockAreaData.Entry_Column_FName,
                string.Empty, e.Row);
                this.View.SetValue(Const_StockAreaData.Entry_Entry, Const_StockAreaData.Entry_Column_FStatus,
                0, e.Row);
            }
            else if (type == 1)//计划数据
            {
                this.View.SetValue(Const_StockAreaData.Entry_Entry, Const_StockAreaData.Entry_Column_FStatus,
                    1, e.Row);
            }
            //相关数量默认0,避免null
            this.View.SetValue(Const_StockAreaData.Entry_Entry, Const_StockAreaData.Entry_Column_FPlanAmount,
             0, e.Row);
            this.View.SetValue(Const_StockAreaData.Entry_Entry, Const_StockAreaData.Entry_Column_FAmount,
             0, e.Row);
            this.View.SetValue(Const_StockAreaData.Entry_Entry, Const_StockAreaData.Entry_Column_FOutAmount,
             0, e.Row);
            this.View.SetValue(Const_StockAreaData.Entry_Entry, Const_StockAreaData.Entry_Column_FWeight,
             0, e.Row);
            this.View.SetValue(Const_StockAreaData.Entry_Entry, Const_StockAreaData.Entry_Column_FOutWeight,
             0, e.Row);
        }
        /// <summary>
        /// 界面初始化
        /// </summary>
        private void InitView()
        {
            var entry = this.View.GetControl<GridView>(Const_StockAreaData.Entry_Entry);
            if (this.Context.CompanyCode == "2")
            {
                foreach (GridColumn item in entry.Columns)
                {
                    if (item.Name.EqualIgnorCase(Const_StockAreaData.Entry_Column_FSpecification)
                        || item.Name.EqualIgnorCase(Const_StockAreaData.Entry_Column_FStockAreaPlan)
                        || item.Name.EqualIgnorCase(Const_StockAreaData.Entry_Column_FPaperGrade)
                        || item.Name.EqualIgnorCase(Const_StockAreaData.Entry_Column_FPackType)
                        || item.Name.EqualIgnorCase(Const_StockAreaData.Entry_Column_FCertification)
                        || item.Name.EqualIgnorCase(Const_StockAreaData.Entry_Column_FColor)
                        || item.Name.EqualIgnorCase(Const_StockAreaData.Entry_Column_FCoreDiameterOrReam)
                        || item.Name.EqualIgnorCase(Const_StockAreaData.Entry_Column_FDeliverDate)
                        || item.Name.EqualIgnorCase(Const_StockAreaData.Entry_Column_FDiameterOrSlides)
                        || item.Name.EqualIgnorCase(Const_StockAreaData.Entry_Column_FLength)
                        || item.Name.EqualIgnorCase(Const_StockAreaData.Entry_Column_FSpCustomer)          
                  
                        || item.Name.EqualIgnorCase(Const_StockAreaData.Entry_Column_FPoNumber)
                        || item.Name.EqualIgnorCase(Const_StockAreaData.Entry_Column_FSpecProdName)
                        || item.Name.EqualIgnorCase(Const_StockAreaData.Entry_Column_FSpecProdName)
                        || item.Name.EqualIgnorCase(Const_StockAreaData.Entry_Column_FTrademarkStyle)
                        || item.Name.EqualIgnorCase(Const_StockAreaData.Entry_Column_FTransportType)
                        || item.Name.EqualIgnorCase(Const_StockAreaData.Entry_Column_FWeightMode))
                    {
                        item.Visible = false;
                        continue;
                    }
                    if (item.Name.EqualIgnorCase(Const_StockAreaData.Entry_Column_FSpecCustName))
                    {
                        item.Caption = "客户";
                    }
                }
            }
            object objType = this.Context.GetOption("Type");
            this.Context.RemoveOption("Type");
            if (objType != null) type = Convert.ToInt32(objType);
            if (type == -1)
            {
                form.Text = "初始化数据查询";
                entry.OptionsBehavior.ReadOnly = true;
                this.View.GetControl<CheckEdit>(Const_StockAreaData.Entry_Control_FCheckAll).Visible = false;
                this.View.GetControl<BarButtonItem>(Const_StockAreaData.Entry_Menu_CopyEntry).Visibility = BarItemVisibility.Never;
                this.View.GetControl<BarButtonItem>(Const_StockAreaData.Head_Menu_Save).Visibility = BarItemVisibility.Never;
                this.View.GetControl<BarButtonItem>(Const_StockAreaData.Entry_Menu_NewEntry).Visibility = BarItemVisibility.Never;
                this.View.GetControl<BarButtonItem>(Const_StockAreaData.Entry_Menu_DeleteEntry).Visibility = BarItemVisibility.Never;
                this.View.GetControl<LayoutControlItem>("lo_Status").Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                foreach (GridColumn item in entry.Columns)
                {
                    if (item.Name.EqualIgnorCase(Const_StockAreaData.Entry_Column_FName)
                        || item.Name.EqualIgnorCase(Const_StockAreaData.Entry_Column_FPlanAmount)
                        || item.Name.EqualIgnorCase(Const_StockAreaData.Entry_Column_FOutAmount)
                        || item.Name.EqualIgnorCase(Const_StockAreaData.Entry_Column_FOutWeight)
                        || item.Name.EqualIgnorCase(Const_StockAreaData.Entry_Column_FInStockArea)
                        || item.Name.EqualIgnorCase(Const_StockAreaData.Entry_Column_FCustomer)
                        || item.Name.EqualIgnorCase(Const_StockAreaData.Entry_Column_FCheck))
                    {
                        item.Visible = false;
                        continue;
                    }
                }
            }
            if (type == 0)
            {
                form.Text = "数据初始化";
                this.View.GetControl<CheckEdit>(Const_StockAreaData.Entry_Control_FCheckAll).Visible = false;
                this.View.GetControl<CheckedComboBoxEdit>(Const_StockAreaData.Head_Field_FStatus).Enabled = false;
                this.View.GetControl<BarButtonItem>(Const_StockAreaData.Head_Menu_Update).Visibility = BarItemVisibility.Always;
                foreach (GridColumn item in entry.Columns)
                {
                    if (item.Name.EqualIgnorCase(Const_StockAreaData.Entry_Column_FName)
                        || item.Name.EqualIgnorCase(Const_StockAreaData.Entry_Column_FPlanAmount)
                        || item.Name.EqualIgnorCase(Const_StockAreaData.Entry_Column_FOutAmount)
                        || item.Name.EqualIgnorCase(Const_StockAreaData.Entry_Column_FOutWeight)
                        || item.Name.EqualIgnorCase(Const_StockAreaData.Entry_Column_FInStockArea)
                        || item.Name.EqualIgnorCase(Const_StockAreaData.Entry_Column_FCustomer)
                        || item.Name.EqualIgnorCase(Const_StockAreaData.Entry_Column_FCheck)
                    )
                    {
                        item.Visible = false;
                        continue;
                    }
                }
            }
            else if (type == 1)
            {
                form.Text = "数据计划";
                foreach (GridColumn item in entry.Columns)
                {
                    if (item.Name.EqualIgnorCase(Const_StockAreaData.Entry_Control_Amount))
                    {
                        item.OptionsColumn.ReadOnly = true;
                        continue;
                    }
                    if (item.Name.EqualIgnorCase(Const_StockAreaData.Entry_Column_FInStockArea)
                        || item.Name.EqualIgnorCase(Const_StockAreaData.Entry_Column_FOutAmount)
                        || item.Name.EqualIgnorCase(Const_StockAreaData.Entry_Column_FOutWeight)
                        || item.Name.EqualIgnorCase(Const_StockAreaData.Entry_Column_FCheck)
                        )
                    {
                        item.Visible = false;
                        continue;
                    }
                }
                this.View.GetControl<CheckEdit>(Const_StockAreaData.Entry_Control_FCheckAll).Visible = false;
                this.View.GetControl<BarButtonItem>(Const_StockAreaData.Head_Menu_Print).Visibility = BarItemVisibility.Always;
                this.View.GetControl<CheckedComboBoxEdit>(Const_StockAreaData.Head_Field_FStatus).Enabled = false;
            }
            else if (type == 2)
            {
                form.Text = "计划管理";
                foreach (GridColumn item in entry.Columns)
                {
                    if (item.Name.EqualIgnorCase(Const_StockAreaData.Entry_Column_FAmount)
                        || item.Name.EqualIgnorCase(Const_StockAreaData.Entry_Column_FPlanAmount)
                        ||item.Name.EqualIgnorCase(Const_StockAreaData.Entry_Column_FCheck))
                        continue;
                    if (item.Name.EqualIgnorCase(Const_StockAreaData.Entry_Column_FInStockArea)
                        || item.Name.EqualIgnorCase(Const_StockAreaData.Entry_Column_FOutAmount)
                        || item.Name.EqualIgnorCase(Const_StockAreaData.Entry_Column_FOutWeight))
                    {
                        item.Visible = false;
                        continue;
                    }
                    item.OptionsColumn.ReadOnly = true;
                }
                this.View.GetControl<BarButtonItem>(Const_StockAreaData.Entry_Menu_Complete).Visibility = BarItemVisibility.Always;
                this.View.GetControl<BarButtonItem>(Const_StockAreaData.Entry_Menu_CopyEntry).Visibility = BarItemVisibility.Never;
                this.View.GetControl<BarButtonItem>(Const_StockAreaData.Entry_Menu_NewEntry).Visibility = BarItemVisibility.Never;
                this.View.GetControl<BarButtonItem>(Const_StockAreaData.Entry_Menu_DeleteEntry).Visibility = BarItemVisibility.Never;
                this.View.GetControl<BarButtonItem>(Const_StockAreaData.Head_Menu_Print).Visibility = BarItemVisibility.Always;
                this.View.GetControl<CheckedComboBoxEdit>(Const_StockAreaData.Head_Field_FStatus).Enabled = true;
            }
            else if (type == 3)
            {
                form.Text = "库区数据管理";
                foreach (GridColumn item in entry.Columns)
                {
                    if (item.Name.EqualIgnorCase(Const_StockAreaData.Entry_Column_FName)
                        || item.Name.EqualIgnorCase(Const_StockAreaData.Entry_Column_FStatus)
                        || item.Name.EqualIgnorCase(Const_StockAreaData.Entry_Column_FPlanAmount)
                        || item.Name.EqualIgnorCase(Const_StockAreaData.Entry_Column_FInStockArea)
                        || item.Name.EqualIgnorCase(Const_StockAreaData.Entry_Column_FCheck))
                    {
                        item.Visible = false;
                        continue;
                    }
                    if (item.Name.EqualIgnorCase(Const_StockAreaData.Entry_Column_FAmount)
                        || item.Name.EqualIgnorCase(Const_StockAreaData.Entry_Column_FOutAmount)
                        || item.Name.EqualIgnorCase(Const_StockAreaData.Entry_Column_FOutWeight)
                        || item.Name.EqualIgnorCase(Const_StockAreaData.Entry_Column_FMemo))
                        continue;
                    item.OptionsColumn.ReadOnly = true;
                }
                this.View.GetControl<CheckEdit>(Const_StockAreaData.Entry_Control_FCheckAll).Visible = false;
                this.View.GetControl<LayoutControlItem>("lo_Status").Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                this.View.GetControl<BarButtonItem>(Const_StockAreaData.Entry_Menu_CopyEntry).Visibility = BarItemVisibility.Never;
                this.View.GetControl<BarButtonItem>(Const_StockAreaData.Entry_Menu_NewEntry).Visibility = BarItemVisibility.Never;
                this.View.GetControl<BarButtonItem>(Const_StockAreaData.Entry_Menu_DeleteEntry).Visibility = BarItemVisibility.Never;
            }
            else if (type == 4)//移库操作
            {
                form.Text = "移库";
                foreach (GridColumn item in entry.Columns)
                {
                    if (item.Name.EqualIgnorCase(Const_StockAreaData.Entry_Column_FName)
                        || item.Name.EqualIgnorCase(Const_StockAreaData.Entry_Column_FPlanAmount)
                        || item.Name.EqualIgnorCase(Const_StockAreaData.Entry_Column_FStatus)
                         || item.Name.EqualIgnorCase(Const_StockAreaData.Entry_Column_FCheck))
                    {
                        item.Visible = false;
                        continue;
                    }
                    if (item.Name.EqualIgnorCase(Const_StockAreaData.Entry_Column_FInStockArea)
                        || item.Name.EqualIgnorCase(Const_StockAreaData.Entry_Column_FOutAmount)
                        || item.Name.EqualIgnorCase(Const_StockAreaData.Entry_Column_FOutWeight))
                    {
                        item.Visible = true;
                        continue;
                    }
                    item.OptionsColumn.ReadOnly = true;
                }
                this.View.GetControl<CheckEdit>(Const_StockAreaData.Entry_Control_FCheckAll).Visible = false;
                this.View.GetControl<LayoutControlItem>("lo_Status").Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                this.View.GetControl<BarButtonItem>(Const_StockAreaData.Entry_Menu_CopyEntry).Visibility = BarItemVisibility.Never;
                this.View.GetControl<BarButtonItem>(Const_StockAreaData.Entry_Menu_NewEntry).Visibility = BarItemVisibility.Never;
                this.View.GetControl<BarButtonItem>(Const_StockAreaData.Entry_Menu_DeleteEntry).Visibility = BarItemVisibility.Never;
            }
        }
        private void SetAmount(int row, string stockArea)
        {
            int amount;
            if (!stockAreaDic.TryGetValue(stockArea, out amount)) return;
            this.View.SetValue(Const_StockAreaData.Entry_Entry,
                Const_StockAreaData.Entry_Column_FTotalCapacity, amount, row);
        }
        private void GetStockAreaPlan(DataChangedEventArgs e)
        {
            string planNumber = this.View.GetValue<string>(Const_StockAreaData.Entry_Entry,
                Const_StockAreaData.Entry_Column_FStockAreaPlan, e.Row);
            if (planNumber.IsNullOrEmptyOrWhiteSpace()) return;
            string sql = @" SELECT * FROM T_AUTOSCAN_STOCKAREAPLAN WHERE FNumber =@FNUMBER ";
            SqlParameter para = new SqlParameter("@FNUMBER", DbType.String);
            para.Value = planNumber;
            SqlParameter[] parameters =
                new SqlParameter[] { para };
            OperateResults result
                = DBAccessServiceHelper.ExcuteQuery(this.Context, sql, new string[] { "T1" }, parameters);
            if (!result.IsSuccess || result.ResultData.Tables["T1"].Rows.Count <= 0) return;
            DataRow row = result.ResultData.Tables["T1"].Rows[0];
            this.View.SetValue(Const_StockAreaData.Entry_Entry, Const_StockAreaData.Entry_Column_FPaperType,
                row[Const_StockAreaPlan.Head_Column_FPaperType], e.Row);
            this.View.SetValue(Const_StockAreaData.Entry_Entry, Const_StockAreaData.Entry_Column_FSpecification,
                row[Const_StockAreaPlan.Head_Column_FSpecification], e.Row);
            this.View.SetValue(Const_StockAreaData.Entry_Entry, Const_StockAreaData.Entry_Column_FPaperGrade,
                row[Const_StockAreaPlan.Head_Column_FPaperGrade], e.Row);
            this.View.SetValue(Const_StockAreaData.Entry_Entry, Const_StockAreaData.Entry_Column_FCertification,
                row[Const_StockAreaPlan.Head_Column_FCertification], e.Row);
            this.View.SetValue(Const_StockAreaData.Entry_Entry, Const_StockAreaData.Entry_Column_FTransportType,
                row[Const_StockAreaPlan.Head_Column_FTransportType], e.Row);
            object cordiameter = row.GetValue<object>(Const_StockAreaPlan.Head_Column_FCoreDiameterOrReam);
            this.View.SetValue(Const_StockAreaData.Entry_Entry, Const_StockAreaData.Entry_Column_FCoreDiameterOrReam,
                ObjectUtils.TrimEndZero(cordiameter), e.Row);
            object diameter = row.GetValue<object>(Const_StockAreaPlan.Head_Column_FDiameterOrSlides);
            this.View.SetValue(Const_StockAreaData.Entry_Entry, Const_StockAreaData.Entry_Column_FDiameterOrSlides,
                ObjectUtils.TrimEndZero(diameter), e.Row);
            object length = row.GetValue<object>(Const_StockAreaPlan.Head_Column_FLength);
            this.View.SetValue(Const_StockAreaData.Entry_Entry, Const_StockAreaData.Entry_Column_FLength,
                ObjectUtils.TrimEndZero(length), e.Row);
            this.View.SetValue(Const_StockAreaData.Entry_Entry, Const_StockAreaData.Entry_Column_FTrademarkStyle,
            row[Const_StockAreaPlan.Head_Column_FTrademarkStyle], e.Row);
            this.View.SetValue(Const_StockAreaData.Entry_Entry, Const_StockAreaData.Entry_Column_FSpecCustName,
            row[Const_StockAreaPlan.Head_Column_FSpecCustName], e.Row);
            this.View.SetValue(Const_StockAreaData.Entry_Entry, Const_StockAreaData.Entry_Column_FSpecProdName,
            row[Const_StockAreaPlan.Head_Column_FSpecProdName], e.Row);
            this.View.SetValue(Const_StockAreaData.Entry_Entry, Const_StockAreaData.Entry_Column_FPackType,
            row[Const_StockAreaPlan.Head_Column_FPackType], e.Row);
            this.View.SetValue(Const_StockAreaData.Entry_Entry, Const_StockAreaData.Entry_Column_FDeliverDate,
            row[Const_StockAreaPlan.Head_Column_FDeliverDate], e.Row);
            this.View.SetValue(Const_StockAreaData.Entry_Entry, Const_StockAreaData.Entry_Column_FMemo,
            row[Const_StockAreaPlan.Head_Column_FMemo], e.Row);
            this.View.SetValue(Const_StockAreaData.Entry_Entry, Const_StockAreaData.Entry_Column_FWeightMode,
            row[Const_StockAreaPlan.Head_Column_FWeightMode], e.Row);
            this.View.SetValue(Const_StockAreaData.Entry_Entry, Const_StockAreaData.Entry_Column_FColor,
            row[Const_StockAreaPlan.Head_Column_FColor], e.Row);
            this.View.SetValue(Const_StockAreaData.Entry_Entry, Const_StockAreaData.Entry_Column_FPoNumber,
            row[Const_StockAreaPlan.Head_Column_FPoNumber], e.Row);
            this.View.SetValue(Const_StockAreaData.Entry_Entry, Const_StockAreaData.Entry_Column_FSpCustomer,
row[Const_StockAreaPlan.Head_Column_FSpCustomer], e.Row);
            this.View.GetControl<GridView>(Const_StockAreaData.Entry_Entry).RefreshRow(e.Row);
        }
    }
}
