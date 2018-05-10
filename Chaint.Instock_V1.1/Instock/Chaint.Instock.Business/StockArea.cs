using System;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using Chaint.Common.Core;
using Chaint.Common.Core.EventArgs;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.Data;
using Chaint.Common.Core.Utils;
using Chaint.Common.Core.Const;
using Chaint.Instock.Core;
using Chaint.Instock.Business.View;
using Chaint.Instock.Business.Controler;
namespace Chaint.Instock.Business
{
    public partial class StockArea:XtraForm
    {
        private Context context;
        private StockAreaControler controler;
        public StockAreaView view;
        public StockArea(Context ctx)
        {
            context = ctx;
            InitializeComponent();
            RegisterEvent();
            InitView();
            InitControler();
        }
        private void InitControler()
        {
            controler = new StockAreaControler(view);
        }
        private void OnLoad(object sender, EventArgs e)
        {
            controler.Invoke(Const_Event.OnLoad);
        }
        private void InitView()
        {
            this.view = new StockAreaView(context);
            //控件
            this.view.AddControl(Const_StockArea.Base_Form, this);
            this.view.AddControl(Const_StockArea.Head_Menu_Save, btSave);
            this.view.AddControl(Const_StockArea.Head_Menu_New, btNew);
            this.view.AddControl(Const_StockArea.Head_Menu_Delete, btDelete);
            this.view.AddControl(Const_StockArea.Entry_Control, gc_Data);
            this.view.AddControl(Const_StockArea.Entry_Entry, gv_Entry);

            //字段
            this.view.AddControl(Const_StockArea.Entry_Field_FID, FHeadID);
            this.view.AddControl(Const_StockArea.Entry_Field_FSeq, FSeq);
            this.view.AddControl(Const_StockArea.Entry_Field_FEntryId, FEntryID);

            this.view.AddControl(Const_StockArea.Entry_Field_FStockAreaNumber, FStockAreaNumber);
            this.view.AddControl(Const_StockArea.Entry_Field_FStockAreaName, FStockAreaName);
            this.view.AddControl(Const_StockArea.Entry_Field_FLocation ,FLocation);
            this.view.AddControl(Const_StockArea.Entry_Field_FTotalCapacity, FTotalCapacity);
            this.view.AddControl(Const_StockArea.Head_Field_FID, this.FID);
            this.view.AddControl(Const_StockArea.Head_Field_FStock, FStock);
            this.view.AddControl(Const_StockArea.Head_Field_FMemo, FMemo);
            this.view.AddControl(Const_StockArea.Head_Field_FCreateDate, this.FCreateDate);
            this.view.AddControl(Const_StockArea.Head_Field_FModifyDate, this.FModifyDate);
            this.view.SetValue(Const_StockArea.Head_Field_FID,SequenceGuid.NewGuid().ToString());
        }
        #region 注册控件事件
        private void RegisterEvent()
        {
            this.Disposed += new EventHandler(this.OnDisposed);
            this.Load+=new EventHandler(this.OnLoad);
            this.btSave.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bt_Save_ItemClick);
            this.FStock.EditValueChanged += new System.EventHandler(this.FStock_EditValueChanged);
            this.FMemo.EditValueChanged += new System.EventHandler(this.FMemo_EditValueChanged);

            this.bt_NewEntry.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bt_NewEntry_ItemClick);
            this.bt_DeleteEntry.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bt_DeleteEntry_ItemClick);
            this.gv_Entry.InitNewRow += new DevExpress.XtraGrid.Views.Grid.InitNewRowEventHandler(this.gv_Entry_InitNewRow);
            this.gv_Entry.RowDeleted += new DevExpress.Data.RowDeletedEventHandler(this.gv_Entry_RowDeleted);
            this.gv_Entry.RowDeleting += new DevExpress.Data.RowDeletingEventHandler(this.gv_Entry_RowDeleting);
            this.gv_Entry.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gv_Entry_CellValueChanged);
        }

        private void OnDisposed(object sender, EventArgs e)
        {
            this.Load -= new EventHandler(this.OnLoad);
            this.btSave.ItemClick -= new DevExpress.XtraBars.ItemClickEventHandler(this.bt_Save_ItemClick);
            this.FStock.EditValueChanged -= new System.EventHandler(this.FStock_EditValueChanged);
            this.FMemo.EditValueChanged -= new System.EventHandler(this.FMemo_EditValueChanged);

            this.bt_NewEntry.ItemClick -= new DevExpress.XtraBars.ItemClickEventHandler(this.bt_NewEntry_ItemClick);
            this.bt_DeleteEntry.ItemClick -= new DevExpress.XtraBars.ItemClickEventHandler(this.bt_DeleteEntry_ItemClick);
            this.gv_Entry.InitNewRow -= new DevExpress.XtraGrid.Views.Grid.InitNewRowEventHandler(this.gv_Entry_InitNewRow);
            this.gv_Entry.RowDeleted -= new DevExpress.Data.RowDeletedEventHandler(this.gv_Entry_RowDeleted);
            this.gv_Entry.RowDeleting -= new DevExpress.Data.RowDeletingEventHandler(this.gv_Entry_RowDeleting);
            this.gv_Entry.CellValueChanged -= new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gv_Entry_CellValueChanged);
            this.view.Dispose();
        }

        private void gv_Entry_RowDeleted(object sender, RowDeletedEventArgs e)
        {
            AfterDeleteEntryRowEventArgs args = new AfterDeleteEntryRowEventArgs();
            args.Sender = Const_StockArea.Entry_Entry;
            args.Parent = Const_StockArea.Entry_Control;
            args.Row = e.RowHandle;
            args.SeqName = Const_StockArea.Entry_Field_FSeq;
            controler.Invoke(Const_Event.AfterDeleteEntryRow, args);
        }
        private void gv_Entry_RowDeleting(object sender, RowDeletingEventArgs e)
        {
            BeforeDeleteEntryRowEventArgs args = new BeforeDeleteEntryRowEventArgs();
            args.Sender = Const_StockArea.Entry_Entry;
            args.Parent = Const_StockArea.Entry_Control;
            args.Row = e.RowHandle;
            args.SeqName = Const_StockArea.Entry_Field_FSeq;
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
            args.Sender = Const_StockArea.Entry_Menu_NewEntry;
            controler.Invoke(Const_Event.ButtonClick, args);
            this.bt_NewEntry.Caption = oldText;
            this.bt_NewEntry.Enabled = true;
        }
        private void gv_Entry_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            AfterCreateNewEntryRowEventArgs args = new AfterCreateNewEntryRowEventArgs();
            args.Sender = Const_StockArea.Entry_Entry;
            args.Row = e.RowHandle;
            args.PrimaryKey = Const_StockArea.Entry_Column_FEntryId;
            args.ParentContainer = Const_StockArea.Entry_Column_FID;
            args.ParentKey = Const_StockArea.Head_Column_FID;
            args.Parent = Const_StockArea.Entry_Control;
            args.SeqKey = Const_StockArea.Entry_Column_FSeq;
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
            this.view.Sync();
            DataChangedEventArgs args = new DataChangedEventArgs();
            args.Row = e.RowHandle;
            args.Parent = Const_StockArea.Entry_Entry;
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
            args.Sender = Const_StockArea.Entry_Menu_DeleteEntry;
            controler.Invoke(Const_Event.ButtonClick, args);
            this.bt_DeleteEntry.Caption = oldText;
            this.bt_DeleteEntry.Enabled = true;
        }

        private void bt_Save_ItemClick(object sender, ItemClickEventArgs e)
        {
            string oldText = this.btSave.Caption;
            this.btSave.Caption = "请稍后";
            this.btSave.Enabled = false;
            ButtonClickEventArgs args = new ButtonClickEventArgs();
            args.Sender = Const_StockArea.Head_Menu_Save;
            controler.Invoke(Const_Event.ButtonClick, args);
            this.btSave.Caption = oldText;
            this.btSave.Enabled = true;
        }

        private void FStock_EditValueChanged(object sender, EventArgs e)
        {
            this.view.Sync(Const_StockArea.Head_Field_FStock);
            DataChangedEventArgs args = new DataChangedEventArgs();
            args.Sender = Const_StockArea.Head_Field_FStock;
            args.Row = -1;
            controler.Invoke(Const_Event.DataChanged, args);
        }
        private void FMemo_EditValueChanged(object sender, EventArgs e)
        {
            this.view.Sync(Const_StockArea.Head_Field_FMemo);
        }

        private void btList_ItemClick(object sender, ItemClickEventArgs e)
        {
            string oldText = this.btList.Caption;
            this.btList.Caption = "请稍后";
            this.btList.Enabled = false;
            ButtonClickEventArgs args = new ButtonClickEventArgs();
            args.Sender = Const_StockArea.Head_Menu_List;
            controler.Invoke(Const_Event.ButtonClick, args);
            this.btList.Caption = oldText;
            this.btList.Enabled = true;
        }

        private void btNew_ItemClick(object sender, ItemClickEventArgs e)
        {
            string oldText = this.btNew.Caption;
            this.btNew.Caption = "请稍后";
            this.btNew.Enabled = false;
            ButtonClickEventArgs args = new ButtonClickEventArgs();
            args.Sender = Const_StockArea.Head_Menu_New;
            controler.Invoke(Const_Event.ButtonClick, args);
            this.btNew.Caption = oldText;
            this.btNew.Enabled = true;
        }

        private void btDelete_ItemClick(object sender, ItemClickEventArgs e)
        {
            string oldText = this.btDelete.Caption;
            this.btDelete.Caption = "请稍后";
            this.btDelete.Enabled = false;
            ButtonClickEventArgs args = new ButtonClickEventArgs();
            args.Sender = Const_StockArea.Head_Menu_Delete;
            controler.Invoke(Const_Event.ButtonClick, args);
            this.btDelete.Caption = oldText;
            this.btDelete.Enabled = true;
        }
    }
}
