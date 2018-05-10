namespace Chaint.Instock.Main
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.barManager = new DevExpress.XtraBars.BarManager(this.components);
            this.mainBar = new DevExpress.XtraBars.Bar();
            this.lbMenu = new DevExpress.XtraBars.BarSubItem();
            this.Base = new DevExpress.XtraBars.BarStaticItem();
            this.btStock = new DevExpress.XtraBars.BarButtonItem();
            this.btStockArea = new DevExpress.XtraBars.BarButtonItem();
            this.btStockAreaPropertity = new DevExpress.XtraBars.BarButtonItem();
            this.Init = new DevExpress.XtraBars.BarStaticItem();
            this.btStockAreaInit = new DevExpress.XtraBars.BarButtonItem();
            this.btInitDataQuery = new DevExpress.XtraBars.BarButtonItem();
            this.Plan = new DevExpress.XtraBars.BarStaticItem();
            this.btStockAreaDataPlan = new DevExpress.XtraBars.BarButtonItem();
            this.btStockAreaPlanManage = new DevExpress.XtraBars.BarButtonItem();
            this.btStockAreaPlanQuery = new DevExpress.XtraBars.BarButtonItem();
            this.Data = new DevExpress.XtraBars.BarStaticItem();
            this.btStockAreaDataManage = new DevExpress.XtraBars.BarButtonItem();
            this.btStockAreaDataTransfer = new DevExpress.XtraBars.BarButtonItem();
            this.btStockAreaDataQuery = new DevExpress.XtraBars.BarButtonItem();
            this.Confirm = new DevExpress.XtraBars.BarStaticItem();
            this.btInstockConfirm = new DevExpress.XtraBars.BarButtonItem();
            this.AutoScan = new DevExpress.XtraBars.BarStaticItem();
            this.btAutoScan = new DevExpress.XtraBars.BarButtonItem();
            this.Auth = new DevExpress.XtraBars.BarStaticItem();
            this.btAuthManage = new DevExpress.XtraBars.BarButtonItem();
            this.btLogIn = new DevExpress.XtraBars.BarButtonItem();
            this.btLogOut = new DevExpress.XtraBars.BarButtonItem();
            this.btClose = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.btStockAreaPlanList = new DevExpress.XtraBars.BarButtonItem();
            this.btStockAreaList = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem3 = new DevExpress.XtraBars.BarButtonItem();
            this.barStaticItem8 = new DevExpress.XtraBars.BarStaticItem();
            this.skinBarSubItem1 = new DevExpress.XtraBars.SkinBarSubItem();
            this.btPasswordModify = new DevExpress.XtraBars.BarButtonItem();
            this.xtraTabbedMdiManager = new DevExpress.XtraTabbedMdi.XtraTabbedMdiManager(this.components);
            this.barStaticItem6 = new DevExpress.XtraBars.BarStaticItem();
            this.barStaticItem7 = new DevExpress.XtraBars.BarStaticItem();
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabbedMdiManager)).BeginInit();
            this.SuspendLayout();
            // 
            // barManager
            // 
            this.barManager.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.mainBar});
            this.barManager.DockControls.Add(this.barDockControlTop);
            this.barManager.DockControls.Add(this.barDockControlBottom);
            this.barManager.DockControls.Add(this.barDockControlLeft);
            this.barManager.DockControls.Add(this.barDockControlRight);
            this.barManager.DockWindowTabFont = new System.Drawing.Font("宋体", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.barManager.Form = this;
            this.barManager.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.lbMenu,
            this.btStockAreaPropertity,
            this.btStockArea,
            this.btAutoScan,
            this.btClose,
            this.btStockAreaPlanList,
            this.btStockAreaList,
            this.btStockAreaInit,
            this.btInstockConfirm,
            this.btStockAreaDataPlan,
            this.btStockAreaPlanQuery,
            this.btStockAreaPlanManage,
            this.btLogIn,
            this.btLogOut,
            this.Base,
            this.Plan,
            this.Confirm,
            this.AutoScan,
            this.btStock,
            this.Data,
            this.btStockAreaDataManage,
            this.barButtonItem3,
            this.btStockAreaDataTransfer,
            this.barStaticItem8,
            this.skinBarSubItem1,
            this.btInitDataQuery,
            this.Init,
            this.Auth,
            this.btAuthManage,
            this.btPasswordModify,
            this.btStockAreaDataQuery});
            this.barManager.MaxItemId = 42;
            // 
            // mainBar
            // 
            this.mainBar.BarName = "菜单";
            this.mainBar.DockCol = 0;
            this.mainBar.DockRow = 0;
            this.mainBar.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.mainBar.FloatLocation = new System.Drawing.Point(730, 231);
            this.mainBar.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.lbMenu, "", true, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btLogIn, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(this.btLogOut),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btClose, DevExpress.XtraBars.BarItemPaintStyle.Caption)});
            this.mainBar.OptionsBar.AllowQuickCustomization = false;
            this.mainBar.OptionsBar.MultiLine = true;
            this.mainBar.OptionsBar.RotateWhenVertical = false;
            this.mainBar.OptionsBar.UseWholeRow = true;
            resources.ApplyResources(this.mainBar, "mainBar");
            // 
            // lbMenu
            // 
            this.lbMenu.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Left;
            resources.ApplyResources(this.lbMenu, "lbMenu");
            this.lbMenu.Id = 2;
            this.lbMenu.ImageUri.Uri = "ListBullets";
            this.lbMenu.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.Base),
            new DevExpress.XtraBars.LinkPersistInfo(this.btStock),
            new DevExpress.XtraBars.LinkPersistInfo(this.btStockArea),
            new DevExpress.XtraBars.LinkPersistInfo(this.btStockAreaPropertity),
            new DevExpress.XtraBars.LinkPersistInfo(this.Init),
            new DevExpress.XtraBars.LinkPersistInfo(this.btStockAreaInit),
            new DevExpress.XtraBars.LinkPersistInfo(this.btInitDataQuery),
            new DevExpress.XtraBars.LinkPersistInfo(this.Plan),
            new DevExpress.XtraBars.LinkPersistInfo(this.btStockAreaDataPlan),
            new DevExpress.XtraBars.LinkPersistInfo(this.btStockAreaPlanManage),
            new DevExpress.XtraBars.LinkPersistInfo(this.btStockAreaPlanQuery),
            new DevExpress.XtraBars.LinkPersistInfo(this.Data),
            new DevExpress.XtraBars.LinkPersistInfo(this.btStockAreaDataManage),
            new DevExpress.XtraBars.LinkPersistInfo(this.btStockAreaDataTransfer),
            new DevExpress.XtraBars.LinkPersistInfo(this.btStockAreaDataQuery),
            new DevExpress.XtraBars.LinkPersistInfo(this.Confirm),
            new DevExpress.XtraBars.LinkPersistInfo(this.btInstockConfirm),
            new DevExpress.XtraBars.LinkPersistInfo(this.AutoScan),
            new DevExpress.XtraBars.LinkPersistInfo(this.btAutoScan),
            new DevExpress.XtraBars.LinkPersistInfo(this.Auth),
            new DevExpress.XtraBars.LinkPersistInfo(this.btAuthManage)});
            this.lbMenu.Name = "lbMenu";
            // 
            // Base
            // 
            resources.ApplyResources(this.Base, "Base");
            this.Base.Id = 22;
            this.Base.ItemAppearance.Normal.Font = ((System.Drawing.Font)(resources.GetObject("Base.ItemAppearance.Normal.Font")));
            this.Base.ItemAppearance.Normal.Options.UseFont = true;
            this.Base.Name = "Base";
            this.Base.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // btStock
            // 
            resources.ApplyResources(this.btStock, "btStock");
            this.btStock.Id = 26;
            this.btStock.Name = "btStock";
            this.btStock.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btStock_ItemClick);
            // 
            // btStockArea
            // 
            resources.ApplyResources(this.btStockArea, "btStockArea");
            this.btStockArea.Id = 6;
            this.btStockArea.Name = "btStockArea";
            this.btStockArea.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btStockArea_ItemClick);
            // 
            // btStockAreaPropertity
            // 
            resources.ApplyResources(this.btStockAreaPropertity, "btStockAreaPropertity");
            this.btStockAreaPropertity.Id = 5;
            this.btStockAreaPropertity.Name = "btStockAreaPropertity";
            this.btStockAreaPropertity.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btStockAreaPropertity_ItemClick);
            // 
            // Init
            // 
            resources.ApplyResources(this.Init, "Init");
            this.Init.Id = 34;
            this.Init.ItemAppearance.Normal.Font = ((System.Drawing.Font)(resources.GetObject("Init.ItemAppearance.Normal.Font")));
            this.Init.ItemAppearance.Normal.Options.UseFont = true;
            this.Init.Name = "Init";
            this.Init.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // btStockAreaInit
            // 
            resources.ApplyResources(this.btStockAreaInit, "btStockAreaInit");
            this.btStockAreaInit.Id = 12;
            this.btStockAreaInit.Name = "btStockAreaInit";
            this.btStockAreaInit.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btStockAreaInit_ItemClick);
            // 
            // btInitDataQuery
            // 
            resources.ApplyResources(this.btInitDataQuery, "btInitDataQuery");
            this.btInitDataQuery.Id = 33;
            this.btInitDataQuery.Name = "btInitDataQuery";
            this.btInitDataQuery.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btInitDataQuery_ItemClick);
            // 
            // Plan
            // 
            resources.ApplyResources(this.Plan, "Plan");
            this.Plan.Id = 23;
            this.Plan.ItemAppearance.Normal.Font = ((System.Drawing.Font)(resources.GetObject("Plan.ItemAppearance.Normal.Font")));
            this.Plan.ItemAppearance.Normal.Options.UseFont = true;
            this.Plan.Name = "Plan";
            this.Plan.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // btStockAreaDataPlan
            // 
            resources.ApplyResources(this.btStockAreaDataPlan, "btStockAreaDataPlan");
            this.btStockAreaDataPlan.Id = 14;
            this.btStockAreaDataPlan.Name = "btStockAreaDataPlan";
            this.btStockAreaDataPlan.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btStockAreaDataPlan_ItemClick);
            // 
            // btStockAreaPlanManage
            // 
            resources.ApplyResources(this.btStockAreaPlanManage, "btStockAreaPlanManage");
            this.btStockAreaPlanManage.Id = 16;
            this.btStockAreaPlanManage.Name = "btStockAreaPlanManage";
            this.btStockAreaPlanManage.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btStockAreaPlanManage_ItemClick);
            // 
            // btStockAreaPlanQuery
            // 
            resources.ApplyResources(this.btStockAreaPlanQuery, "btStockAreaPlanQuery");
            this.btStockAreaPlanQuery.Id = 15;
            this.btStockAreaPlanQuery.Name = "btStockAreaPlanQuery";
            this.btStockAreaPlanQuery.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btStockAreaPlanQuery_ItemClick);
            // 
            // Data
            // 
            resources.ApplyResources(this.Data, "Data");
            this.Data.Id = 27;
            this.Data.ItemAppearance.Normal.Font = ((System.Drawing.Font)(resources.GetObject("Data.ItemAppearance.Normal.Font")));
            this.Data.ItemAppearance.Normal.Options.UseFont = true;
            this.Data.Name = "Data";
            this.Data.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // btStockAreaDataManage
            // 
            resources.ApplyResources(this.btStockAreaDataManage, "btStockAreaDataManage");
            this.btStockAreaDataManage.Id = 28;
            this.btStockAreaDataManage.Name = "btStockAreaDataManage";
            this.btStockAreaDataManage.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btStockAreaDataManage_ItemClick);
            // 
            // btStockAreaDataTransfer
            // 
            resources.ApplyResources(this.btStockAreaDataTransfer, "btStockAreaDataTransfer");
            this.btStockAreaDataTransfer.Id = 30;
            this.btStockAreaDataTransfer.Name = "btStockAreaDataTransfer";
            this.btStockAreaDataTransfer.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btStockAreaDataTransfer_ItemClick);
            // 
            // btStockAreaDataQuery
            // 
            resources.ApplyResources(this.btStockAreaDataQuery, "btStockAreaDataQuery");
            this.btStockAreaDataQuery.Id = 41;
            this.btStockAreaDataQuery.Name = "btStockAreaDataQuery";
            this.btStockAreaDataQuery.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btStockAreaDataQuery_ItemClick);
            // 
            // Confirm
            // 
            resources.ApplyResources(this.Confirm, "Confirm");
            this.Confirm.Id = 24;
            this.Confirm.ItemAppearance.Normal.Font = ((System.Drawing.Font)(resources.GetObject("Confirm.ItemAppearance.Normal.Font")));
            this.Confirm.ItemAppearance.Normal.Options.UseFont = true;
            this.Confirm.Name = "Confirm";
            this.Confirm.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // btInstockConfirm
            // 
            resources.ApplyResources(this.btInstockConfirm, "btInstockConfirm");
            this.btInstockConfirm.Id = 13;
            this.btInstockConfirm.Name = "btInstockConfirm";
            this.btInstockConfirm.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btInstockConfirm_ItemClick);
            // 
            // AutoScan
            // 
            resources.ApplyResources(this.AutoScan, "AutoScan");
            this.AutoScan.Id = 25;
            this.AutoScan.ItemAppearance.Normal.Font = ((System.Drawing.Font)(resources.GetObject("AutoScan.ItemAppearance.Normal.Font")));
            this.AutoScan.ItemAppearance.Normal.Options.UseFont = true;
            this.AutoScan.Name = "AutoScan";
            this.AutoScan.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // btAutoScan
            // 
            resources.ApplyResources(this.btAutoScan, "btAutoScan");
            this.btAutoScan.Id = 7;
            this.btAutoScan.Name = "btAutoScan";
            this.btAutoScan.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btAutoScan_ItemClick);
            // 
            // Auth
            // 
            resources.ApplyResources(this.Auth, "Auth");
            this.Auth.Id = 35;
            this.Auth.ItemAppearance.Normal.Font = ((System.Drawing.Font)(resources.GetObject("Auth.ItemAppearance.Normal.Font")));
            this.Auth.ItemAppearance.Normal.Options.UseFont = true;
            this.Auth.Name = "Auth";
            this.Auth.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // btAuthManage
            // 
            resources.ApplyResources(this.btAuthManage, "btAuthManage");
            this.btAuthManage.Id = 36;
            this.btAuthManage.Name = "btAuthManage";
            this.btAuthManage.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btAuthManage_ItemClick);
            // 
            // btLogIn
            // 
            this.btLogIn.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            resources.ApplyResources(this.btLogIn, "btLogIn");
            this.btLogIn.Id = 20;
            this.btLogIn.Name = "btLogIn";
            this.btLogIn.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btLogIn_ItemClick);
            // 
            // btLogOut
            // 
            this.btLogOut.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            resources.ApplyResources(this.btLogOut, "btLogOut");
            this.btLogOut.Id = 21;
            this.btLogOut.Name = "btLogOut";
            this.btLogOut.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btLogOut_ItemClick);
            // 
            // btClose
            // 
            this.btClose.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            resources.ApplyResources(this.btClose, "btClose");
            this.btClose.Id = 8;
            this.btClose.Name = "btClose";
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            resources.ApplyResources(this.barDockControlTop, "barDockControlTop");
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            resources.ApplyResources(this.barDockControlBottom, "barDockControlBottom");
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            resources.ApplyResources(this.barDockControlLeft, "barDockControlLeft");
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            resources.ApplyResources(this.barDockControlRight, "barDockControlRight");
            // 
            // btStockAreaPlanList
            // 
            this.btStockAreaPlanList.Id = 38;
            this.btStockAreaPlanList.Name = "btStockAreaPlanList";
            // 
            // btStockAreaList
            // 
            this.btStockAreaList.Id = 39;
            this.btStockAreaList.Name = "btStockAreaList";
            // 
            // barButtonItem3
            // 
            this.barButtonItem3.Id = 40;
            this.barButtonItem3.Name = "barButtonItem3";
            // 
            // barStaticItem8
            // 
            resources.ApplyResources(this.barStaticItem8, "barStaticItem8");
            this.barStaticItem8.Id = 31;
            this.barStaticItem8.ItemAppearance.Normal.Font = ((System.Drawing.Font)(resources.GetObject("barStaticItem8.ItemAppearance.Normal.Font")));
            this.barStaticItem8.ItemAppearance.Normal.Options.UseFont = true;
            this.barStaticItem8.Name = "barStaticItem8";
            this.barStaticItem8.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // skinBarSubItem1
            // 
            resources.ApplyResources(this.skinBarSubItem1, "skinBarSubItem1");
            this.skinBarSubItem1.Id = 32;
            this.skinBarSubItem1.Name = "skinBarSubItem1";
            // 
            // btPasswordModify
            // 
            resources.ApplyResources(this.btPasswordModify, "btPasswordModify");
            this.btPasswordModify.Id = 37;
            this.btPasswordModify.Name = "btPasswordModify";
            // 
            // xtraTabbedMdiManager
            // 
            this.xtraTabbedMdiManager.Appearance.BackColor = ((System.Drawing.Color)(resources.GetObject("xtraTabbedMdiManager.Appearance.BackColor")));
            this.xtraTabbedMdiManager.Appearance.Options.UseBackColor = true;
            this.xtraTabbedMdiManager.MdiParent = this;
            // 
            // barStaticItem6
            // 
            resources.ApplyResources(this.barStaticItem6, "barStaticItem6");
            this.barStaticItem6.Id = 23;
            this.barStaticItem6.ItemAppearance.Normal.Font = ((System.Drawing.Font)(resources.GetObject("barStaticItem6.ItemAppearance.Normal.Font")));
            this.barStaticItem6.ItemAppearance.Normal.Options.UseFont = true;
            this.barStaticItem6.Name = "barStaticItem6";
            this.barStaticItem6.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // barStaticItem7
            // 
            resources.ApplyResources(this.barStaticItem7, "barStaticItem7");
            this.barStaticItem7.Id = 23;
            this.barStaticItem7.ItemAppearance.Normal.Font = ((System.Drawing.Font)(resources.GetObject("barStaticItem7.ItemAppearance.Normal.Font")));
            this.barStaticItem7.ItemAppearance.Normal.Options.UseFont = true;
            this.barStaticItem7.Name = "barStaticItem7";
            this.barStaticItem7.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // MainForm
            // 
            this.Appearance.BackColor = ((System.Drawing.Color)(resources.GetObject("MainForm.Appearance.BackColor")));
            this.Appearance.Options.UseBackColor = true;
            this.Appearance.Options.UseFont = true;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.IsMdiContainer = true;
            this.LookAndFeel.SkinName = "Office 2016 Dark";
            this.Name = "MainForm";
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabbedMdiManager)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarManager barManager;
        private DevExpress.XtraBars.Bar mainBar;
        private DevExpress.XtraBars.BarSubItem lbMenu;
        private DevExpress.XtraBars.BarButtonItem btStockAreaPropertity;
        private DevExpress.XtraBars.BarButtonItem btStockArea;
        private DevExpress.XtraBars.BarButtonItem btAutoScan;
        private DevExpress.XtraBars.BarButtonItem btClose;
        private DevExpress.XtraBars.BarButtonItem btStockAreaPlanList;
        private DevExpress.XtraBars.BarButtonItem btStockAreaList;
        private DevExpress.XtraBars.BarButtonItem btStockAreaInit;
        private DevExpress.XtraBars.BarButtonItem btInstockConfirm;
        private DevExpress.XtraBars.BarButtonItem btStockAreaDataPlan;
        private DevExpress.XtraBars.BarButtonItem btStockAreaPlanQuery;
        private DevExpress.XtraBars.BarButtonItem btStockAreaPlanManage;
        private DevExpress.XtraBars.BarButtonItem btLogIn;
        private DevExpress.XtraBars.BarButtonItem btLogOut;
        private DevExpress.XtraTabbedMdi.XtraTabbedMdiManager xtraTabbedMdiManager;
        private DevExpress.XtraBars.BarStaticItem Base;
        private DevExpress.XtraBars.BarStaticItem Plan;
        private DevExpress.XtraBars.BarStaticItem Confirm;
        private DevExpress.XtraBars.BarStaticItem AutoScan;
        private DevExpress.XtraBars.BarButtonItem btStock;
        private DevExpress.XtraBars.BarStaticItem Data;
        private DevExpress.XtraBars.BarButtonItem btStockAreaDataManage;
        private DevExpress.XtraBars.BarButtonItem barButtonItem3;
        private DevExpress.XtraBars.BarButtonItem btStockAreaDataTransfer;
        private DevExpress.XtraBars.BarStaticItem barStaticItem8;
        private DevExpress.XtraBars.BarButtonItem btInitDataQuery;
        private DevExpress.XtraBars.SkinBarSubItem skinBarSubItem1;
        private DevExpress.XtraBars.BarStaticItem barStaticItem6;
        private DevExpress.XtraBars.BarStaticItem barStaticItem7;
        private DevExpress.XtraBars.BarStaticItem Init;
        private DevExpress.XtraBars.BarStaticItem Auth;
        private DevExpress.XtraBars.BarButtonItem btPasswordModify;
        private DevExpress.XtraBars.BarButtonItem btAuthManage;
        private DevExpress.XtraBars.BarButtonItem btStockAreaDataQuery;
    }
}
