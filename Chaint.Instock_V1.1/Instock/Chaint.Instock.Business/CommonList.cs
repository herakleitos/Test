using System;
using DevExpress.XtraGrid.Views.Grid;
using Chaint.Common.Core;
using Chaint.Common.Core.Const;
using Chaint.Instock.Core;
using Chaint.Common.Core.EventArgs;
using Chaint.Instock.Business.View;
using Chaint.Instock.Business.Controler;
namespace Chaint.Instock.Business
{
    public partial class CommonList : DevExpress.XtraEditors.XtraForm
    {
        private Context context;
        public CommonListView view;
        private CommonListControler controler;
        public CommonList(Context ctx)
        {
            context = ctx;
            InitializeComponent();
            RegesitEvent();
            InitView();
            InitControler();
        }

        private void RegesitEvent()
        {
            this.Disposed += new EventHandler(this.OnDisposed);
            this.btQuery.Click += new System.EventHandler(this.bt_Query_Click);
            this.gv_View.RowCellClick += new RowCellClickEventHandler(this.gv_View_RowClick);
        }

        private void OnDisposed(object sender, EventArgs e)
        {
            this.btQuery.Click -= new System.EventHandler(this.bt_Query_Click);
            this.gv_View.RowCellClick -= new RowCellClickEventHandler(this.gv_View_RowClick);
            this.view.Dispose();
        }

        private void gv_View_RowClick(object sender, RowCellClickEventArgs e)
        {
            EntryRowClickEventArgs args = new EntryRowClickEventArgs();
            args.Parent = Const_CommonList.Main_GridControl;
            args.Sender = Const_CommonList.Main_GridView;
            args.RowIndex = e.RowHandle;
            args.ClickTime = e.Clicks;
            controler.Invoke(Const_Event.EntryRowClick, args);
        }

        private void OnLoad(object sender, EventArgs e)
        {
            controler.Invoke(Const_Event.OnLoad);
        }
        private void InitView()
        {
            this.view = new CommonListView(context);
            this.view.AddControl(Const_CommonList.Base_Form, this);
            this.view.AddControl(Const_CommonList.Main_GridControl, this.gc_Main);
            this.view.AddControl(Const_CommonList.Main_GridView, this.gv_View);
            this.view.AddControl(Const_CommonList.Query_Control_ButtonQuery, this.btQuery);
            this.view.AddControl(Const_CommonList.Query_FBeginDate, this.FBeginDate);
            this.view.AddControl(Const_CommonList.Query_FEndDate, this.FEndDate);
        }
        private void InitControler()
        {
            controler = new CommonListControler(view);
        }

        private void bt_Query_Click(object sender, EventArgs e)
        {
            string oldText = this.btQuery.Text;
            this.btQuery.Text = "请稍后";
            this.btQuery.Enabled = false;
            ButtonClickEventArgs args = new ButtonClickEventArgs();
            args.Sender = Const_CommonList.Query_Control_ButtonQuery;
            controler.Invoke(Const_Event.ButtonClick, args);
            this.btQuery.Text = oldText;
            this.btQuery.Enabled = true;
        }
        private void btDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string oldText = this.btDelete.Caption;
            this.btDelete.Caption = "请稍后";
            this.btDelete.Enabled = false;
            ButtonClickEventArgs args = new ButtonClickEventArgs();
            args.Sender = Const_CommonList.Query_Control_ButtonDelete;
            controler.Invoke(Const_Event.ButtonClick, args);
            this.btDelete.Caption = oldText;
            this.btDelete.Enabled = true;
        }

        private void btRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string oldText = this.btRefresh.Caption;
            this.btRefresh.Caption = "请稍后";
            this.btRefresh.Enabled = false;
            ButtonClickEventArgs args = new ButtonClickEventArgs();
            args.Sender = Const_CommonList.Query_Control_ButtonRefresh;
            controler.Invoke(Const_Event.ButtonClick, args);
            this.btRefresh.Caption = oldText;
            this.btRefresh.Enabled = true;
        }

        private void btOpen_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string oldText = this.btOpen.Caption;
            this.btOpen.Caption = "请稍后";
            this.btOpen.Enabled = false;
            ButtonClickEventArgs args = new ButtonClickEventArgs();
            args.Sender = Const_CommonList.Query_Control_ButtonOpen;
            controler.Invoke(Const_Event.ButtonClick, args);
            this.btOpen.Caption = oldText;
            this.btOpen.Enabled = true;
        }

        private void btNew_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string oldText = this.btNew.Caption;
            this.btNew.Caption = "请稍后";
            this.btNew.Enabled = false;
            ButtonClickEventArgs args = new ButtonClickEventArgs();
            args.Sender = Const_CommonList.Query_Control_ButtonNew;
            controler.Invoke(Const_Event.ButtonClick, args);
            this.btNew.Caption = oldText;
            this.btNew.Enabled = true;
        }
    }
}