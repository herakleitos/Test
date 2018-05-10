using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using Chaint.Common.Core;
using Chaint.Common.Core.EventArgs;
namespace Chaint.Instock.Business
{
    public partial class StockAreaPlan : XtraForm
    {
        private Context context; 
        private StockAreaPlanControler controler;
        private StockAreaPlanView view;
        public StockAreaPlan()
        {
        }
        public StockAreaPlan(Context ctx)
        {
            context = ctx;
            InitializeComponent();
            RegisterEvent();
            InitView();
            InitControler();
            controler.Invoke("OnLoad");
        }
        private void InitControler()
        {
            controler = new StockAreaPlanControler(view);
        }

        private void InitView()
        {
            this.view = new StockAreaPlanView(context);
            this.view.AddControl("form", this);
            this.view.AddControl("bt_Save", bt_Save);
            this.view.AddControl("bt_New", bt_New);
            this.view.AddControl("bt_Delete", bt_Delete);

            this.view.AddControl("FPaperType", this.FPaperType);
            this.view.AddControl("FCoreDiameterOrReam", this.FCoreDiameterOrReam);
            this.view.AddControl("FLength", this.FLength);
            this.view.AddControl("FDiameterOrSlides", this.FDiameterOrSlides);
            this.view.AddControl("FTransportType", this.FTransportType);
            this.view.AddControl("FWeightMode", this.FWeightMode);
            this.view.AddControl("FMemo", this.FMemo);
            this.view.AddControl("FPoNumber", this.FPoNumber);
            this.view.AddControl("FCertification", this.FCertification);
            this.view.AddControl("FTrademarkStyle", this.FTrademarkStyle);
            this.view.AddControl("FPackType", this.FPackType);
            this.view.AddControl("FSpecCustName", this.FSpecCustName);
            this.view.AddControl("FPaperGrade", this.FPaperGrade);
            this.view.AddControl("FSpecProdName", this.FSpecProdName);
            this.view.AddControl("FDeliverDate", this.FDeliverDate);
            this.view.AddControl("FColor", this.FColor);
        }
        #region 注册控件事件
        private void RegisterEvent()
        {
            this.bt_Delete.ItemClick += new ItemClickEventHandler(this.bt_Delete_ItemClick);
            this.bt_New.ItemClick += new ItemClickEventHandler(this.bt_New_ItemClick);
            this.bt_Save.ItemClick += new ItemClickEventHandler(this.bt_Save_ItemClick);

            this.FPaperType.EditValueChanged += new System.EventHandler(this.lu_PaperType_EditValueChanged);
        }
        #endregion
        private void bt_Save_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ButtonClickEventArgs args = new ButtonClickEventArgs();
            args.sender = "bt_Save";
            controler.Invoke("ButtonClick", args);
        }

        private void bt_New_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ButtonClickEventArgs args = new ButtonClickEventArgs();
            args.sender = "bt_New";
            controler.Invoke("ButtonClick", args);
        }

        private void bt_Delete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ButtonClickEventArgs args = new ButtonClickEventArgs();
            args.sender = "bt_Delete";
            controler.Invoke("ButtonClick", args);
        }
        private void lu_PaperType_EditValueChanged(object sender, EventArgs e)
        {
            DataChangedEventArgs args = new DataChangedEventArgs();
            args.sender = "lu_PaperType";
            controler.Invoke("DataChanged", args);
        }
    }
}
