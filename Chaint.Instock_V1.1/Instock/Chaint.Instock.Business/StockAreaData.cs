using System;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using Chaint.Common.Core;
using Chaint.Common.Core.Const;
using Chaint.Common.Core.EventArgs;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.Data;
using Chaint.Common.Core.Utils;
using Chaint.Instock.Core;
using Chaint.Instock.Business.Controler;
using Chaint.Instock.Business.View;
namespace Chaint.Instock.Business
{
    public partial class StockAreaData : XtraForm
    {
        private Context context;
        private StockAreaDataControler controler;
        private StockAreaDataView view;
        public StockAreaData(Context ctx)
        {
            context = ctx;
            InitializeComponent();
            RegisterEvent();
            InitView();
            InitControler();
        }
        private void InitControler()
        {
            controler = new StockAreaDataControler(view);
        }
        private void OnLoad(object sender, EventArgs e)
        {
            controler.Invoke(Const_Event.OnLoad);
        }
        private void InitView()
        {
            this.view = new StockAreaDataView(context);
            //控件
            this.view.AddControl(Const_StockAreaData.Base_Form, this);
            this.view.AddControl(Const_StockAreaData.Head_Menu_Save, bt_Save);
            this.view.AddControl(Const_StockAreaData.Head_Menu_New, bt_New);
            this.view.AddControl(Const_StockAreaData.Head_Menu_Query, btQuery);
            this.view.AddControl(Const_StockAreaData.Head_Field_FHProduct, FHProduct);
            this.view.AddControl(Const_StockAreaData.Head_Menu_Refresh, btRefresh);
            this.view.AddControl(Const_StockAreaData.Entry_Control_FCheckAll, FCheckAll);

            this.view.AddControl(Const_StockAreaData.Head_Container, this.lc_Head);
            this.view.AddControl(Const_StockAreaData.Entry_Control, gc_Data);
            this.view.AddControl(Const_StockAreaData.Entry_Entry, gv_Entry);
            this.view.AddControl(Const_StockAreaData.Entry_Menu_NewEntry, bt_NewEntry);
            this.view.AddControl(Const_StockAreaData.Entry_Menu_CopyEntry, bt_CopyEntry);
            this.view.AddControl(Const_StockAreaData.Entry_Menu_DeleteEntry, bt_DeleteEntry);
            this.view.AddControl(Const_StockAreaData.Entry_Menu_Complete,  btComplete);
            this.view.AddControl(Const_StockAreaData.Head_Menu_Print, btPrint);
            this.view.AddControl(Const_StockAreaData.Head_Menu_Print, btExport);

            this.view.AddControl(Const_StockAreaData.Head_Field_FBeginDate, FBeginDate);
            this.view.AddControl(Const_StockAreaData.Head_Field_FEndDate, FEndDate);
            this.view.AddControl(Const_StockAreaData.Head_Field_FStatus, FHStatus);
            this.view.AddControl(Const_StockAreaData.Head_Field_FHStock, FHStock);
            this.view.AddControl(Const_StockAreaData.Head_Field_FHStockArea, FHStockArea);
            this.view.AddControl(Const_StockAreaData.Head_Field_FHSpecification, FHSpecification);
            this.view.AddControl(Const_StockAreaData.Head_Field_FHPapertype, FHPaperType);
            this.view.AddControl(Const_StockAreaData.Entry_Field_FDate, FDate);
            this.view.AddControl(Const_StockAreaData.Entry_Field_FEntryId, FEntryID);

            this.view.AddControl(Const_StockAreaData.Entry_Control_FInStockArea, this.slu_InStockArea);
            this.view.AddControl(Const_StockAreaData.Entry_Control_FStockAreaPlan, this.slu_StockAreaPlan);
            this.view.AddControl(Const_StockAreaData.Entry_Control_FProduct, this.slu_Product);
            this.view.AddControl(Const_StockAreaData.Entry_Control_Status, this.slu_Status);
            this.view.AddControl(Const_StockAreaData.Entry_Control_Amount, this.cab_Amount);
            this.view.AddControl(Const_StockAreaData.Entry_Control_PlanAmount, this.cab_PlanAmount);

            this.view.AddControl(Const_StockAreaData.Entry_Field_FProduct, FProduct);
            this.view.AddControl(Const_StockAreaData.Entry_Field_FStockAreaPlan, FStockAreaPlan);
            this.view.AddControl(Const_StockAreaData.Entry_Field_FAmount ,FAmount);
            this.view.AddControl(Const_StockAreaData.Entry_Field_FPlanAmount, FPlanAmount);
            this.view.AddControl(Const_StockAreaData.Entry_Field_FStatus, FStatus);
            this.view.AddControl(Const_StockAreaData.Entry_Field_FStock, this.FStock);
            this.view.AddControl(Const_StockAreaData.Entry_Field_FStockArea, this.FStockArea);
            this.view.AddControl(Const_StockAreaData.Entry_Field_FName, this.FName);
            this.view.AddControl(Const_StockAreaData.Entry_Field_FCustomer, this.FCustomer);
            this.view.AddControl(Const_StockAreaData.Entry_Field_FSpCustomer, this.FSpCustomer);

            this.view.AddControl(Const_StockAreaData.Entry_Control_Stock, this.slu_Stock);
            this.view.AddControl(Const_StockAreaData.Entry_Control_StockArea, this.slu_StockArea);
            this.view.AddControl(Const_StockAreaData.Entry_Control_FPaperType, this.slu_PaperType);
            this.view.AddControl(Const_StockAreaData.Entry_Control_FCoreDiameterOrReam, this.cb_CoreDiameterOrReam);
            this.view.AddControl(Const_StockAreaData.Entry_Control_FDiameterOrSlides, this.cb_DiameterOrSlides);
            this.view.AddControl(Const_StockAreaData.Entry_Control_FSpecification, this.cb_Specification);
            this.view.AddControl(Const_StockAreaData.Entry_Control_FLength, this.cb_Length);
            this.view.AddControl(Const_StockAreaData.Entry_Control_FTransportType, this.slu_TransportType);
            this.view.AddControl(Const_StockAreaData.Entry_Control_FWeightMode, this.slu_WeightMode);
            this.view.AddControl(Const_StockAreaData.Entry_Control_FCertification, this.slu_Certification);
            this.view.AddControl(Const_StockAreaData.Entry_Control_FTrademarkStyle, this.slu_TrademarkStyle);
            this.view.AddControl(Const_StockAreaData.Entry_Control_FPackType, this.slu_PackType);
            this.view.AddControl(Const_StockAreaData.Entry_Control_FSpecCustName, this.slu_SpecCustName);
            this.view.AddControl(Const_StockAreaData.Entry_Control_FPaperGrade, this.slu_PaperGrade);
            this.view.AddControl(Const_StockAreaData.Entry_Control_FSpecProdName, this.slu_SpecProdName);
            this.view.AddControl(Const_StockAreaData.Entry_Control_FDeliverDate, this.date_DeliverDate);
            this.view.AddControl(Const_StockAreaData.Entry_Control_FColor, this.slu_Color);
            this.view.AddControl(Const_StockAreaData.Entry_Control_SpCustomer, this.slu_SpCustomer);

            this.view.AddControl(Const_StockAreaData.Entry_Field_FTotalCapacity, this.FTotalCapacity);
            this.view.AddControl(Const_StockAreaData.Entry_Field_FUsedCapacity, this.FUsedCapacity);

            this.view.AddControl(Const_StockAreaData.Head_Menu_Update, this.btUpdate);
            this.view.AddControl(Const_StockAreaData.Entry_Field_FWeight, this.FWeight);
            this.view.AddControl(Const_StockAreaData.Entry_Field_FOutWeight, this.FOutWeight);

            this.view.AddControl("lo_Status", this.lo_Status);
        }
        #region 注册控件事件
        private void RegisterEvent()
        {
            this.Disposed += new EventHandler(this.OnDisposed);
            this.Load+=new EventHandler(this.OnLoad);
            this.bt_Save.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bt_Save_ItemClick);
            this.slu_Stock.EditValueChanged += new System.EventHandler(this.FStock_EditValueChanged);
            this.bt_NewEntry.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bt_NewEntry_ItemClick);
            this.bt_DeleteEntry.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bt_DeleteEntry_ItemClick);
            this.gv_Entry.InitNewRow += new DevExpress.XtraGrid.Views.Grid.InitNewRowEventHandler(this.gv_Entry_InitNewRow);
            this.gv_Entry.RowDeleted += new DevExpress.Data.RowDeletedEventHandler(this.gv_Entry_RowDeleted);
            this.gv_Entry.RowDeleting += new DevExpress.Data.RowDeletingEventHandler(this.gv_Entry_RowDeleting);
            this.gv_Entry.CellValueChanged += 
                new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gv_Entry_CellValueChanged);
            this.gv_Entry.CustomRowCellEdit += new CustomRowCellEditEventHandler(gv_View_CustomRowCellEdit);
            this.FCheckAll.CheckedChanged += new System.EventHandler(this.FCheckAll_CheckedChanged);
            this.gv_Entry.CustomDrawRowIndicator += new RowIndicatorCustomDrawEventHandler(gv_Entry_CustomDrawRowIndicator);
        }

        private void OnDisposed(object sender, EventArgs e)
        {
            this.Load -= new EventHandler(this.OnLoad);
            this.bt_Save.ItemClick -= new DevExpress.XtraBars.ItemClickEventHandler(this.bt_Save_ItemClick);
            this.slu_Stock.EditValueChanged -= new System.EventHandler(this.FStock_EditValueChanged);
            this.bt_NewEntry.ItemClick -= new DevExpress.XtraBars.ItemClickEventHandler(this.bt_NewEntry_ItemClick);
            this.bt_DeleteEntry.ItemClick -= new DevExpress.XtraBars.ItemClickEventHandler(this.bt_DeleteEntry_ItemClick);
            this.gv_Entry.InitNewRow -= new DevExpress.XtraGrid.Views.Grid.InitNewRowEventHandler(this.gv_Entry_InitNewRow);
            this.gv_Entry.RowDeleted -= new DevExpress.Data.RowDeletedEventHandler(this.gv_Entry_RowDeleted);
            this.gv_Entry.RowDeleting -= new DevExpress.Data.RowDeletingEventHandler(this.gv_Entry_RowDeleting);
            this.gv_Entry.CellValueChanged -=
                new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gv_Entry_CellValueChanged);
            this.gv_Entry.CustomRowCellEdit -= new CustomRowCellEditEventHandler(gv_View_CustomRowCellEdit);
            this.FCheckAll.CheckedChanged -= new System.EventHandler(this.FCheckAll_CheckedChanged);
            this.gv_Entry.CustomDrawRowIndicator -= new RowIndicatorCustomDrawEventHandler(gv_Entry_CustomDrawRowIndicator);
            this.view.Dispose();
        }

        private void gv_Entry_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void gv_View_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            CustomEntryRowCellEventArgs args = new CustomEntryRowCellEventArgs();
            args.Sender = e.Column.Name;
            args.Row = e.RowHandle;
            args.Item = e.RepositoryItem;
            controler.Invoke(Const_Event.CustomEntryRowCell, args);
            e.RepositoryItem = args.Item;
        }

        private void GridViewEditorValueChanged(object sender, EventArgs e)
        {
            //if (!this.gv_Entry.IsNewItemRow(gv_Entry.FocusedRowHandle))
            //{
            //    gv_Entry.CloseEditor();
            //    gv_Entry.UpdateCurrentRow();
            //}
        }
        private void gv_Entry_RowDeleted(object sender, RowDeletedEventArgs e)
        {
            AfterDeleteEntryRowEventArgs args = new AfterDeleteEntryRowEventArgs();
            args.Sender = Const_StockAreaData.Entry_Entry;
            args.Parent = Const_StockAreaData.Entry_Control;
            args.Row = e.RowHandle;
            controler.Invoke(Const_Event.AfterDeleteEntryRow, args);
        }
        private void gv_Entry_RowDeleting(object sender, RowDeletingEventArgs e)
        {
            BeforeDeleteEntryRowEventArgs args = new BeforeDeleteEntryRowEventArgs();
            args.Sender = Const_StockAreaData.Entry_Entry;
            args.Parent = Const_StockAreaData.Entry_Control;
            args.Row = e.RowHandle;
            args.Cancel = e.Cancel;
            controler.Invoke(Const_Event.BeforeDeleteEntryRow, args);
        }
        #endregion

        private void bt_NewEntry_ItemClick(object sender, ItemClickEventArgs e)
        {
            string oldText = this.bt_NewEntry.Caption;
            this.bt_NewEntry.Caption = "请稍后";
            this.bt_NewEntry.Enabled = false;
            ButtonClickEventArgs args = new ButtonClickEventArgs();
            args.Sender = Const_StockAreaData.Entry_Menu_NewEntry;
            controler.Invoke(Const_Event.ButtonClick, args);
            this.bt_NewEntry.Caption = oldText;
            this.bt_NewEntry.Enabled = true;
        }
        private void gv_Entry_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            AfterCreateNewEntryRowEventArgs args = new AfterCreateNewEntryRowEventArgs();
            args.Sender = Const_StockAreaData.Entry_Entry;
            args.Row = e.RowHandle;
            args.PrimaryKey = Const_StockAreaData.Entry_Column_FEntryId;
            args.Parent = Const_StockAreaData.Entry_Control;
            controler.Invoke(Const_Event.AfterCreateNewEntryRow, args);
        }
        private void gv_Entry_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            //即时更新数据源
            if (!gv_Entry.IsNewItemRow(gv_Entry.FocusedRowHandle))
            {
                gv_Entry.CloseEditor();
                gv_Entry.UpdateCurrentRow();
            }
            DataChangedEventArgs args = new DataChangedEventArgs();
            args.Row = e.RowHandle;
            args.Parent = Const_StockAreaData.Entry_Entry;
            args.Sender = e.Column.Name;
            args.Value = e.Value;
            controler.Invoke(Const_Event.DataChanged, args);
        }
        private void bt_DeleteEntry_ItemClick(object sender, ItemClickEventArgs e)
        {
            string oldText = this.bt_DeleteEntry.Caption;
            this.bt_DeleteEntry.Caption = "请稍后";
            this.bt_DeleteEntry.Enabled = false;
            ButtonClickEventArgs args = new ButtonClickEventArgs();
            args.Sender = Const_StockAreaData.Entry_Menu_DeleteEntry;
            controler.Invoke(Const_Event.ButtonClick, args);
            this.bt_DeleteEntry.Caption = oldText;
            this.bt_DeleteEntry.Enabled = true;
        }

        private void bt_Save_ItemClick(object sender, ItemClickEventArgs e)
        {
            string oldText = this.bt_Save.Caption;
            this.bt_Save.Caption = "请稍后";
            this.bt_Save.Enabled = false;
            ButtonClickEventArgs args = new ButtonClickEventArgs();
            args.Sender = Const_StockAreaData.Head_Menu_Save;
            controler.Invoke(Const_Event.ButtonClick, args);
            this.bt_Save.Caption = oldText;
            this.bt_Save.Enabled = true;
        }

        private void FStock_EditValueChanged(object sender, EventArgs e)
        {
            DataChangedEventArgs args = new DataChangedEventArgs();
            args.Sender = Const_StockAreaData.Entry_Field_FStock;
            controler.Invoke(Const_Event.DataChanged, args);
        }

        private void FStockArea_EditValueChanged(object sender, EventArgs e)
        {
            DataChangedEventArgs args = new DataChangedEventArgs();
            args.Sender = Const_StockAreaData.Entry_Field_FStockArea;
            controler.Invoke(Const_Event.DataChanged, args);
        }
        private void btRefresh_ItemClick(object sender, ItemClickEventArgs e)
        {
            string oldText = this.btRefresh.Caption;
            this.btRefresh.Caption = "请稍后";
            this.btRefresh.Enabled = false;
            ButtonClickEventArgs args = new ButtonClickEventArgs();
            args.Sender = Const_StockAreaData.Head_Menu_Refresh;
            controler.Invoke(Const_Event.ButtonClick, args);
            this.btRefresh.Caption = oldText;
            this.btRefresh.Enabled = true;
        }
        private void btQuery_Click(object sender, EventArgs e)
        {
            string oldText = this.btQuery.Text;
            this.btQuery.Text = "请稍后";
            this.btQuery.Enabled = false;
            ButtonClickEventArgs args = new ButtonClickEventArgs();
            args.Sender = Const_StockAreaData.Head_Menu_Query;
            controler.Invoke(Const_Event.ButtonClick, args);
            this.btQuery.Text = oldText;
            this.btQuery.Enabled = true;
        }
        private void btUpdate_ItemClick(object sender, ItemClickEventArgs e)
        {
            string oldText = this.btUpdate.Caption;
            this.btUpdate.Caption = "请稍后";
            this.btUpdate.Enabled = false;
            ButtonClickEventArgs args = new ButtonClickEventArgs();
            args.Sender = Const_StockAreaData.Head_Menu_Update;
            controler.Invoke(Const_Event.ButtonClick, args);
            this.btUpdate.Caption = oldText;
            this.btUpdate.Enabled = true;
        }

        private void btComplete_ItemClick(object sender, ItemClickEventArgs e)
        {
            string oldText = this.btComplete.Caption;
            this.btComplete.Caption = "请稍后";
            this.btComplete.Enabled = false;
            ButtonClickEventArgs args = new ButtonClickEventArgs();
            args.Sender = Const_StockAreaData.Entry_Menu_Complete;
            controler.Invoke(Const_Event.ButtonClick, args);
            this.btComplete.Caption = oldText;
            this.btComplete.Enabled = true;
        }

        private void btPrint_ItemClick(object sender, ItemClickEventArgs e)
        {
            string oldText = this.btPrint.Caption;
            this.btPrint.Caption = "请稍后";
            this.btPrint.Enabled = false;
            ButtonClickEventArgs args = new ButtonClickEventArgs();
            args.Sender = Const_StockAreaData.Head_Menu_Print;
            controler.Invoke(Const_Event.ButtonClick, args);
            this.btPrint.Caption = oldText;
            this.btPrint.Enabled = true;
        }

        private void btPrintTemplet_ItemClick(object sender, ItemClickEventArgs e)
        {
            string oldText = this.btPrintTemplet.Caption;
            this.btPrintTemplet.Caption = "请稍后";
            this.btPrintTemplet.Enabled = false;
            ButtonClickEventArgs args = new ButtonClickEventArgs();
            args.Sender = Const_StockAreaData.Head_Menu_PrintTemplet;
            controler.Invoke(Const_Event.ButtonClick, args);
            this.btPrintTemplet.Caption = oldText;
            this.btPrintTemplet.Enabled = true;
        }

        private void FHStock_EditValueChanged(object sender, EventArgs e)
        {
            DataChangedEventArgs args = new DataChangedEventArgs();
            args.Row = -1;
            args.Sender = Const_StockAreaData.Head_Field_FHStock;
            controler.Invoke(Const_Event.DataChanged, args);
        }

        private void bt_CopyEntry_ItemClick(object sender, ItemClickEventArgs e)
        {
            string oldText = this.bt_CopyEntry.Caption;
            this.bt_CopyEntry.Caption = "请稍后";
            this.bt_CopyEntry.Enabled = false;
            ButtonClickEventArgs args = new ButtonClickEventArgs();
            args.Sender = Const_StockAreaData.Entry_Menu_CopyEntry;
            controler.Invoke(Const_Event.ButtonClick, args);
            this.bt_CopyEntry.Caption = oldText;
            this.bt_CopyEntry.Enabled = true;
        }
        private void btPrintPreview_ItemClick(object sender, ItemClickEventArgs e)
        {
            string oldText = this.btPrintPreview.Caption;
            this.btPrintPreview.Caption = "请稍后";
            this.btPrintPreview.Enabled = false;
            ButtonClickEventArgs args = new ButtonClickEventArgs();
            args.Sender = Const_StockAreaData.Head_Menu_PrintPreview;
            controler.Invoke(Const_Event.ButtonClick, args);
            this.btPrintPreview.Caption = oldText;
            this.btPrintPreview.Enabled = true;
        }

        private void btExport_ItemClick(object sender, ItemClickEventArgs e)
        {
            string oldText = this.btExport.Caption;
            this.btExport.Caption = "请稍后";
            this.btExport.Enabled = false;
            ButtonClickEventArgs args = new ButtonClickEventArgs();
            args.Sender = Const_StockAreaData.Head_Menu_Export;
            controler.Invoke(Const_Event.ButtonClick, args);
            this.btExport.Caption = oldText;
            this.btExport.Enabled = true;
        }
        private void FCheckAll_CheckedChanged(object sender, EventArgs e)
        {
            DataChangedEventArgs args = new DataChangedEventArgs();
            args.Sender = Const_StockAreaData.Entry_Control_FCheckAll;
            args.Value = this.FCheckAll.Checked;
            controler.Invoke(Const_Event.DataChanged, args);
        }

        private void FHPaperType_EditValueChanged(object sender, EventArgs e)
        {
            DataChangedEventArgs args = new DataChangedEventArgs();
            args.Sender = Const_StockAreaData.Head_Field_FHPapertype;
            controler.Invoke(Const_Event.DataChanged, args);
        }
    }
}
