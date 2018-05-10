using System;
using Chaint.Common.Core;
using Chaint.Common.Core.Const;
using Chaint.Instock.Core;
using Chaint.Common.Core.EventArgs;
using Chaint.Instock.Business.View;
using Chaint.Instock.Business.Controler;
namespace Chaint.Instock.Business
{
    public partial class StockAreaPlanQuery : DevExpress.XtraEditors.XtraForm
    {
        private Context context;
        public StockAreaPlanQueryView view;
        private StockAreaPlanQueryControler controler;
        public StockAreaPlanQuery()
        {
        }
        public StockAreaPlanQuery(Context ctx)
        {
            context = ctx;
            InitializeComponent();
            RegesitEvent();
            InitView();
            InitControler();
        }
        private void OnLoad(object sender, EventArgs e)
        {
            controler.Invoke(Const_Event.OnLoad);
        }
        private void RegesitEvent()
        {
            this.Disposed += new EventHandler(this.OnDisposed);
            this.Load += new EventHandler(OnLoad);
        }

        private void OnDisposed(object sender, EventArgs e)
        {
            this.Load -= new EventHandler(OnLoad);
            this.view.Dispose();
        }

        private void InitView()
        {
            this.view = new StockAreaPlanQueryView(context);
            this.view.AddControl(Const_StockAreaPlanQuery.Base_Form, this);
            this.view.AddControl(Const_StockAreaPlanQuery.Main_GridControl, gc_Data);
            this.view.AddControl(Const_StockAreaPlanQuery.Main_GridView, gv_View);
            this.view.AddControl(Const_StockAreaPlanQuery.Query_FStock, FStock);
            this.view.AddControl(Const_StockAreaPlanQuery.Query_FStockArea, FStockArea);
            this.view.AddControl(Const_StockAreaPlanQuery.Query_FBeginDate, FBeginDate);
            this.view.AddControl(Const_StockAreaPlanQuery.Query_FEndDate, FEndDate);
            this.view.AddControl(Const_StockAreaPlanQuery.Query_FIsComplete, FIsComplete);
            this.view.AddControl(Const_StockAreaPlanQuery.Query_Control_ButtonQuery, btQuery);
        }
        private void InitControler()
        {
            controler = new StockAreaPlanQueryControler(view);
        }
        private void btQuery_Click(object sender, EventArgs e)
        {
            string oldText = this.btQuery.Text;
            this.btQuery.Text = "请稍后";
            this.btQuery.Enabled = false;
            ButtonClickEventArgs args = new ButtonClickEventArgs();
            args.Sender = Const_StockAreaPlanQuery.Query_Control_ButtonQuery;
            controler.Invoke(Const_Event.ButtonClick, args);
            this.btQuery.Text = oldText;
            this.btQuery.Enabled = true;
        }
        private void FStock_EditValueChanged(object sender, EventArgs e)
        {
            DataChangedEventArgs args = new DataChangedEventArgs();
            args.Row = -1;
            args.Sender = Const_StockAreaPlanQuery.Query_FStock;
            controler.Invoke(Const_Event.DataChanged, args);
        }
    }
}