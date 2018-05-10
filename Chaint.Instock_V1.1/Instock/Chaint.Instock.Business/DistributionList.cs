using System;
using DevExpress.XtraGrid.Views.Grid;
using Chaint.Common.Core;
using Chaint.Common.Core.Const;
using Chaint.Instock.Core;
using Chaint.Common.Core.EventArgs;
using Chaint.Instock.Business.View;
using Chaint.Instock.Business.Controler;
using DevExpress.XtraGrid.Views.Base;

namespace Chaint.Instock.Business
{
    public partial class DistributionList : DevExpress.XtraEditors.XtraForm
    {
        private Context context;
        public DistributionView view;
        private DistributionControler controler;
        public DistributionList()
        {
        }
        public DistributionList(Context ctx)
        {
            context = ctx;
            InitializeComponent();
            RegesitEvent();
            InitView();
            InitControler();
        }

        private void OnLoad(object sender, EventArgs e)
        {
            controler.Invoke("OnLoad");
        }
        private void RegesitEvent()
        {
            this.Disposed += new EventHandler(this.OnDisposed);
            this.Load += new EventHandler(OnLoad);
            this.btConfirm.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(btConfirm_ItemClick);
            this.btSave.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btSave_ItemClick);
            this.btQuery.Click += new EventHandler(btQuery_Click);
            this.slu_StockAreaPlan.EditValueChanged+= new EventHandler(GridViewEditorValueChanged);
            this.gv_View.CellValueChanged +=
              new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gv_Entry_CellValueChanged);
            this.gv_View.CustomRowCellEdit += new CustomRowCellEditEventHandler(gv_View_CustomRowCellEdit);
            this.gv_View.CustomDrawRowIndicator += new RowIndicatorCustomDrawEventHandler(gv_View_CustomDrawRowIndicator);
        }

        private void gv_View_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void OnDisposed(object sender, EventArgs e)
        {
            this.Load -= new EventHandler(OnLoad);
            this.btConfirm.ItemClick -= new DevExpress.XtraBars.ItemClickEventHandler(btConfirm_ItemClick);
            this.btSave.ItemClick -= new DevExpress.XtraBars.ItemClickEventHandler(this.btSave_ItemClick);
            this.btQuery.Click -= new EventHandler(btQuery_Click);
            this.slu_StockAreaPlan.EditValueChanged -= new EventHandler(GridViewEditorValueChanged);
            this.gv_View.CellValueChanged -=
              new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gv_Entry_CellValueChanged);
            this.gv_View.CustomRowCellEdit -= new CustomRowCellEditEventHandler(gv_View_CustomRowCellEdit);
            this.view.Dispose();
        }

        private void gv_Entry_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            //即时更新数据源
            if (!gv_View.IsNewItemRow(gv_View.FocusedRowHandle))
            {
                gv_View.CloseEditor();
                gv_View.UpdateCurrentRow();
            }
            DataChangedEventArgs args = new DataChangedEventArgs();
            args.Row = e.RowHandle;
            args.Parent = Const_Distribution.Main_GridView;
            args.Sender = e.Column.Name;
            args.Value = e.Value;
            controler.Invoke(Const_Event.DataChanged, args);
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

        private void InitView()
        {
            this.view = new DistributionView(context);
            this.view.AddControl(Const_Distribution.Base_Form, this);
            this.view.AddControl(Const_Distribution.Main_GridControl, this.gc_Data);
            this.view.AddControl(Const_Distribution.Main_GridView, this.gv_View);
            this.view.AddControl(Const_Distribution.Query_Control_ButtonQuery, this.btQuery);
            this.view.AddControl(Const_Distribution.Query_Control_ButtonConfirm, this.btConfirm);
            this.view.AddControl(Const_Distribution.Query_Control_ButtonSave, this.btSave);
            this.view.AddControl(Const_Distribution.Query_FBeginDate, this.FBeginDate);
            this.view.AddControl(Const_Distribution.Query_FOper, this.FOper);
            this.view.AddControl(Const_Distribution.Query_FEndDate, this.FEndDate);
            this.view.AddControl(Const_Distribution.Query_FIsConfirm, this.FIsConfirm);

            this.view.AddControl(Const_Distribution.Query_FProduct, this.FHProduct);
            this.view.AddControl(Const_Distribution.Query_FStock, this.FHStock);
            this.view.AddControl(Const_Distribution.Query_FStockArea, this.FHStockArea);
            this.view.AddControl(Const_Distribution.Query_FSpecification, this.FHSpecification);

            this.view.AddControl(Const_Distribution.Entry_Control_Status, this.slu_Status);
            this.view.AddControl(Const_Distribution.Entry_Control_StockAreaPlan, this.slu_StockAreaPlan);
            this.view.AddControl(Const_Distribution.Entry_Field_FID, this.FID);
            this.view.AddControl(Const_Distribution.Entry_Field_FOperator, this.FOperator);
            this.view.AddControl(Const_Distribution.Entry_Field_FProduct, this.FProductName);
            this.view.AddControl(Const_Distribution.Entry_Field_FStock, this.FStock);
            this.view.AddControl(Const_Distribution.Entry_Field_FStockArea, this.FStockArea);
            this.view.AddControl(Const_Distribution.Entry_Field_FStockAreaPlan, this.FStockAreaPlan);
            this.view.AddControl(Const_Distribution.Entry_Field_FAmount, this.FAmount);
            this.view.AddControl(Const_Distribution.Entry_Field_FStatus, this.FStatusName);
            this.view.AddControl(Const_Distribution.Entry_Field_FDate, this.FDate);
            this.view.AddControl(Const_Distribution.Entry_Field_FWeight, this.FWeight);

            this.view.AddControl(Const_Distribution.Entry_Column_FPlanName, this.FPlanName);
            this.view.AddControl(Const_Distribution.Entry_Column_FStockAreaPlan, this.FStockAreaPlan);

            this.view.AddControl(Const_Distribution.Entry_Field_FCheck, this.FCheck);
            this.view.AddControl(Const_Distribution.Entry_Control_FCheck, this.cb_Check);
            this.view.AddControl(Const_Distribution.Entry_Control_FAllCheck, this.FAllCheck);
        }
        private void InitControler()
        {
            controler = new DistributionControler(view);
        }
        private void btQuery_Click(object sender, EventArgs e)
        {
            string oldText = this.btQuery.Text;
            this.btQuery.Text = "请稍后";
            this.btQuery.Enabled = false;
            ButtonClickEventArgs args = new ButtonClickEventArgs();
            args.Sender = Const_Distribution.Query_Control_ButtonQuery;
            controler.Invoke(Const_Event.ButtonClick, args);
            this.btQuery.Text = oldText;
            this.btQuery.Enabled = true;
        }
        private void GridViewEditorValueChanged(object sender, EventArgs e)
        {
            if (!this.gv_View.IsNewItemRow(gv_View.FocusedRowHandle))
            {
                gv_View.CloseEditor();
                gv_View.UpdateCurrentRow();
            }
        }
        private void FAllCheck_CheckedChanged(object sender, EventArgs e)
        {
            DataChangedEventArgs args = new DataChangedEventArgs();
            args.Sender = Const_Distribution.Entry_Control_FAllCheck;
            args.Value = this.FAllCheck.Checked;
            controler.Invoke(Const_Event.DataChanged, args);
        }

        private void btConfirm_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string oldText = this.btConfirm.Caption;
            this.btConfirm.Caption = "请稍后";
            this.btConfirm.Enabled = false;
            ButtonClickEventArgs args = new ButtonClickEventArgs();
            args.Sender = Const_Distribution.Query_Control_ButtonConfirm;
            controler.Invoke(Const_Event.ButtonClick, args);
            this.btConfirm.Caption = oldText;
            this.btConfirm.Enabled = true;
        }
        private void btSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string oldText = this.btSave.Caption;
            this.btSave.Caption = "请稍后";
            this.btSave.Enabled = false;
            ButtonClickEventArgs args = new ButtonClickEventArgs();
            args.Sender = Const_Distribution.Query_Control_ButtonSave;
            controler.Invoke(Const_Event.ButtonClick, args);
            this.btSave.Caption = oldText;
            this.btSave.Enabled = true;
        }

        private void FHStock_EditValueChanged(object sender, EventArgs e)
        {
            DataChangedEventArgs args = new DataChangedEventArgs();
            args.Sender = Const_Distribution.Query_FStock;
            controler.Invoke(Const_Event.DataChanged, args);
        }

        private void btBatchFill_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string oldText = this.btBatchFill.Caption;
            this.btBatchFill.Caption = "请稍后";
            this.btBatchFill.Enabled = false;
            ButtonClickEventArgs args = new ButtonClickEventArgs();
            args.Sender = Const_Distribution.Query_Control_ButtonBatchFill;
            controler.Invoke(Const_Event.ButtonClick, args);
            this.btBatchFill.Caption = oldText;
            this.btBatchFill.Enabled = true;
        }
    }
}