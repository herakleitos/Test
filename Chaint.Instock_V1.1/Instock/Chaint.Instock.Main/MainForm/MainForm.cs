using System;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Chaint.Common.Core.EventArgs;
using Chaint.Common.Core;
using Chaint.Instock.Business;

namespace Chaint.Instock.Main
{
    public partial class MainForm : XtraForm
    {
        private MainFormView view;
        private MainFormControler controler;
        private Context context;
        public MainForm()
        {
        }
        public MainForm(Context ctx)
        {
            context = ctx;
            InitializeComponent();
            RegisterEvent();
            InitView();
            InitControler();
        }
        private void InitControler()
        {
            controler = new MainFormControler(view);
        }

        private void InitView()
        {
            this.view = new MainFormView(this.context);
            this.view.AddControl("form", this);
            this.view.AddControl("xtraTabbedMdiManager", xtraTabbedMdiManager);
            this.view.AddControl("barDockControlLeft", barDockControlLeft);
            this.view.AddControl("barDockControlRight", barDockControlRight);
            this.view.AddControl("barDockControlBottom", barDockControlBottom);
            this.view.AddControl("barDockControlTop", barDockControlTop);
            this.view.AddControl("barManager", barManager);
            this.view.AddControl("mainBar", mainBar);
            this.view.AddControl("lbMenu", lbMenu);
            this.view.AddControl("btLogIn", btLogIn);
            this.view.AddControl("btClose", btClose);


            this.view.AddControl("Base", Base);
            this.view.AddControl("btStock", btStock);
            this.view.AddControl("btStockArea", btStockArea);
            this.view.AddControl("btStockAreaPropertity", btStockAreaPropertity);

            this.view.AddControl("Init", Init);
            this.view.AddControl("btStockAreaInit", btStockAreaInit);
            this.view.AddControl("btInitDataQuery", btInitDataQuery);

            this.view.AddControl("Plan", Plan);
            this.view.AddControl("btStockAreaDataPlan", btStockAreaDataPlan);
            this.view.AddControl("btStockAreaPlanManage", btStockAreaPlanManage);
            this.view.AddControl("btStockAreaDataQuery", btStockAreaPlanQuery);

            this.view.AddControl("Data", Data);
            this.view.AddControl("btStockAreaDataManage", btStockAreaDataManage);
            this.view.AddControl("btStockAreaDataTransfer", btStockAreaDataTransfer);

            this.view.AddControl("Confirm", Confirm);
            this.view.AddControl("btInstockConfirm", btInstockConfirm);

            this.view.AddControl("AutoScan", AutoScan);
            this.view.AddControl("btAutoScan", btAutoScan);

            this.view.AddControl("Auth", Auth);
            this.view.AddControl("btAuthManage", btAuthManage);
            this.view.AddControl("btPasswordModify", btPasswordModify);
        }
        #region 注册控件事件
        private void RegisterEvent()
        {
            this.Load += new EventHandler(this.OnLoad);
            this.FormClosing += new FormClosingEventHandler(this.BeforeClose);
            this.btStockAreaPropertity.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btStockAreaPlan_ItemClick);
            this.btClose.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btClose_ItemClick);
        }

        private void OnLoad(object sender, EventArgs e)
        {
            controler.Invoke("OnLoad");
        }
        private void BeforeClose(object sender, FormClosingEventArgs e)
        {
            var children = this.MdiChildren;
            foreach (var child in children)
            {
                if (child is StockInAutoScan)
                {
                    ChaintMessageBox.Show("请先关闭自动扫描页面!");
                    e.Cancel = true;
                }
            }
        }
        #endregion
        private void btStockAreaPlan_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ButtonClickEventArgs args = new ButtonClickEventArgs();
            args.Sender = "btStockAreaPlan";
            controler.Invoke("ButtonClick", args);
        }

        private void btClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ButtonClickEventArgs args = new ButtonClickEventArgs();
            args.Sender = "btClose";
            controler.Invoke("ButtonClick", args);
        }

        private void btStockAreaInit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ButtonClickEventArgs args = new ButtonClickEventArgs();
            args.Sender = "btStockAreaInit";
            controler.Invoke("ButtonClick", args);
        }

        private void btInstockConfirm_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ButtonClickEventArgs args = new ButtonClickEventArgs();
            args.Sender = "btInstockConfirm";
            controler.Invoke("ButtonClick", args);
        }
        private void btStockAreaDataPlan_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ButtonClickEventArgs args = new ButtonClickEventArgs();
            args.Sender = "btStockAreaDataPlan";
            controler.Invoke("ButtonClick", args);
        }

        private void btStockAreaPlanQuery_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ButtonClickEventArgs args = new ButtonClickEventArgs();
            args.Sender = "btStockAreaPlanQuery";
            controler.Invoke("ButtonClick", args);
        }

        private void btStockAreaPlanManage_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ButtonClickEventArgs args = new ButtonClickEventArgs();
            args.Sender = "btStockAreaPlanManage";
            controler.Invoke("ButtonClick", args);
        }
        private void btLogIn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ButtonClickEventArgs args = new ButtonClickEventArgs();
            args.Sender = "btLogIn";
            controler.Invoke("ButtonClick", args);
        }
        private void btLogOut_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ButtonClickEventArgs args = new ButtonClickEventArgs();
            args.Sender = "btLogOut";
            controler.Invoke("ButtonClick", args);
        }

        private void btStock_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ButtonClickEventArgs args = new ButtonClickEventArgs();
            args.Sender = "btStock";
            controler.Invoke("ButtonClick", args);
        }
        private void btStockAreaDataManage_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ButtonClickEventArgs args = new ButtonClickEventArgs();
            args.Sender = "btStockAreaDataManage";
            controler.Invoke("ButtonClick", args);
        }

        private void btStockAreaDataTransfer_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ButtonClickEventArgs args = new ButtonClickEventArgs();
            args.Sender = "btStockAreaDataTransfer";
            controler.Invoke("ButtonClick", args);
        }
        private void btInitDataQuery_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ButtonClickEventArgs args = new ButtonClickEventArgs();
            args.Sender = "btInitDataQuery";
            controler.Invoke("ButtonClick", args);
        }
        private void btAuthManage_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ButtonClickEventArgs args = new ButtonClickEventArgs();
            args.Sender = "btAuthManage";
            controler.Invoke("ButtonClick", args);
        }

        private void btStockArea_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ButtonClickEventArgs args = new ButtonClickEventArgs();
            args.Sender = "btStockArea";
            controler.Invoke("ButtonClick", args);
        }

        private void btStockAreaPropertity_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ButtonClickEventArgs args = new ButtonClickEventArgs();
            args.Sender = "btStockAreaPropertity";
            controler.Invoke("ButtonClick", args);
        }

        private void btAutoScan_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ButtonClickEventArgs args = new ButtonClickEventArgs();
            args.Sender = "btAutoScan";
            controler.Invoke("ButtonClick", args);
        }
        private void btStockAreaDataQuery_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ButtonClickEventArgs args = new ButtonClickEventArgs();
            args.Sender = "btStockAreaDataQuery";
            controler.Invoke("ButtonClick", args);
        }
    }
}