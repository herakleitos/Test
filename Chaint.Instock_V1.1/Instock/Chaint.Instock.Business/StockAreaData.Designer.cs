namespace Chaint.Instock.Business
{
    partial class StockAreaData
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
                this.barManager = null;
                this.barDockControlBottom = null;
                this.barDockControlLeft = null;
                this.barDockControlRight = null;
                this.barDockControlTop = null;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StockAreaData));
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            this.lc_Head = new DevExpress.XtraLayout.LayoutControl();
            this.FHSpecification = new DevExpress.XtraEditors.TextEdit();
            this.barManagerEntry = new DevExpress.XtraBars.BarManager(this.components);
            this.entryBar = new DevExpress.XtraBars.Bar();
            this.bt_NewEntry = new DevExpress.XtraBars.BarButtonItem();
            this.bt_CopyEntry = new DevExpress.XtraBars.BarButtonItem();
            this.bt_DeleteEntry = new DevExpress.XtraBars.BarButtonItem();
            this.btComplete = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControl1 = new DevExpress.XtraBars.BarDockControl();
            this.barDockControl2 = new DevExpress.XtraBars.BarDockControl();
            this.barDockControl3 = new DevExpress.XtraBars.BarDockControl();
            this.barDockControl4 = new DevExpress.XtraBars.BarDockControl();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.FHPaperType = new DevExpress.XtraEditors.SearchLookUpEdit();
            this.gridView18 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.FHProduct = new DevExpress.XtraEditors.SearchLookUpEdit();
            this.searchLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.FHStockArea = new DevExpress.XtraEditors.CheckedComboBoxEdit();
            this.FHStock = new DevExpress.XtraEditors.CheckedComboBoxEdit();
            this.btQuery = new DevExpress.XtraEditors.SimpleButton();
            this.FBeginDate = new DevExpress.XtraEditors.DateEdit();
            this.FHStatus = new DevExpress.XtraEditors.CheckedComboBoxEdit();
            this.FEndDate = new DevExpress.XtraEditors.DateEdit();
            this.layoutControlGroup3 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lo_EndDate = new DevExpress.XtraLayout.LayoutControlItem();
            this.lo_Stock = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.lo_Product = new DevExpress.XtraLayout.LayoutControlItem();
            this.lo_BeginDate = new DevExpress.XtraLayout.LayoutControlItem();
            this.lo_Status = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.lo_HPaperType = new DevExpress.XtraLayout.LayoutControlItem();
            this.lo_HSpecification = new DevExpress.XtraLayout.LayoutControlItem();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.FCheckAll = new DevExpress.XtraEditors.CheckEdit();
            this.gc_Data = new DevExpress.XtraGrid.GridControl();
            this.gv_Entry = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.FEntryID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.FName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.FProduct = new DevExpress.XtraGrid.Columns.GridColumn();
            this.slu_Product = new DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit();
            this.gridView3 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.FStock = new DevExpress.XtraGrid.Columns.GridColumn();
            this.slu_Stock = new DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.FStockArea = new DevExpress.XtraGrid.Columns.GridColumn();
            this.slu_StockArea = new DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit();
            this.gridView4 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.FStockAreaPlan = new DevExpress.XtraGrid.Columns.GridColumn();
            this.slu_StockAreaPlan = new DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit();
            this.repositoryItemSearchLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.FAmount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.cab_Amount = new DevExpress.XtraEditors.Repository.RepositoryItemCalcEdit();
            this.FDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.deDate = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.FPlanAmount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.cab_PlanAmount = new DevExpress.XtraEditors.Repository.RepositoryItemCalcEdit();
            this.FStatus = new DevExpress.XtraGrid.Columns.GridColumn();
            this.slu_Status = new DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit();
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.FPaperType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.slu_PaperType = new DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit();
            this.gridView5 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.FSpecification = new DevExpress.XtraGrid.Columns.GridColumn();
            this.cb_Specification = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.FPaperGrade = new DevExpress.XtraGrid.Columns.GridColumn();
            this.slu_PaperGrade = new DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit();
            this.gridView6 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.FCertification = new DevExpress.XtraGrid.Columns.GridColumn();
            this.slu_Certification = new DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit();
            this.gridView7 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.FTransportType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.slu_TransportType = new DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit();
            this.gridView8 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.FCoreDiameterOrReam = new DevExpress.XtraGrid.Columns.GridColumn();
            this.cb_CoreDiameterOrReam = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.FDiameterOrSlides = new DevExpress.XtraGrid.Columns.GridColumn();
            this.cb_DiameterOrSlides = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.FLength = new DevExpress.XtraGrid.Columns.GridColumn();
            this.cb_Length = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.FTrademarkStyle = new DevExpress.XtraGrid.Columns.GridColumn();
            this.slu_TrademarkStyle = new DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit();
            this.gridView9 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.FSpecCustName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.slu_SpecCustName = new DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit();
            this.gridView10 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.FSpecProdName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.slu_SpecProdName = new DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit();
            this.gridView11 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.FPackType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.slu_PackType = new DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit();
            this.gridView12 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.FDeliverDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.date_DeliverDate = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.FWeightMode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.slu_WeightMode = new DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit();
            this.gridView13 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.FColor = new DevExpress.XtraGrid.Columns.GridColumn();
            this.slu_Color = new DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit();
            this.gridView14 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.FPoNumber = new DevExpress.XtraGrid.Columns.GridColumn();
            this.FSpCustomer = new DevExpress.XtraGrid.Columns.GridColumn();
            this.slu_SpCustomer = new DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit();
            this.gridView16 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.FMemo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.FTotalCapacity = new DevExpress.XtraGrid.Columns.GridColumn();
            this.FUsedCapacity = new DevExpress.XtraGrid.Columns.GridColumn();
            this.FCustomer = new DevExpress.XtraGrid.Columns.GridColumn();
            this.chk_Customer = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.FInStockArea = new DevExpress.XtraGrid.Columns.GridColumn();
            this.slu_InStockArea = new DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit();
            this.gridView17 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.FOutAmount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ce_OutAmount = new DevExpress.XtraEditors.Repository.RepositoryItemCalcEdit();
            this.FWeight = new DevExpress.XtraGrid.Columns.GridColumn();
            this.cab_Weight = new DevExpress.XtraEditors.Repository.RepositoryItemCalcEdit();
            this.FOutWeight = new DevExpress.XtraGrid.Columns.GridColumn();
            this.cab_OutWeight = new DevExpress.XtraEditors.Repository.RepositoryItemCalcEdit();
            this.FStockDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.FCheck = new DevExpress.XtraGrid.Columns.GridColumn();
            this.cb_Check = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.barManager = new DevExpress.XtraBars.BarManager(this.components);
            this.mainBar = new DevExpress.XtraBars.Bar();
            this.bt_Save = new DevExpress.XtraBars.BarButtonItem();
            this.btUpdate = new DevExpress.XtraBars.BarButtonItem();
            this.bsPrint = new DevExpress.XtraBars.BarSubItem();
            this.barButtonItem3 = new DevExpress.XtraBars.BarButtonItem();
            this.btPrintPreview = new DevExpress.XtraBars.BarButtonItem();
            this.btPrintTemplet = new DevExpress.XtraBars.BarButtonItem();
            this.btExport = new DevExpress.XtraBars.BarButtonItem();
            this.bt_New = new DevExpress.XtraBars.BarButtonItem();
            this.bt_Delete = new DevExpress.XtraBars.BarButtonItem();
            this.btRefresh = new DevExpress.XtraBars.BarButtonItem();
            this.btPrint = new DevExpress.XtraBars.BarButtonItem();
            this.barListItem1 = new DevExpress.XtraBars.BarListItem();
            this.gridView15 = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lc_Head)).BeginInit();
            this.lc_Head.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FHSpecification.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManagerEntry)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FHPaperType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView18)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FHProduct.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLookUpEdit1View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FHStockArea.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FHStock.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FBeginDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FBeginDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FHStatus.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FEndDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FEndDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_EndDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_Stock)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_Product)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_BeginDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_Status)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_HPaperType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_HSpecification)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FCheckAll.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gc_Data)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gv_Entry)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.slu_Product)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.slu_Stock)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.slu_StockArea)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.slu_StockAreaPlan)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSearchLookUpEdit1View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cab_Amount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deDate.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cab_PlanAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.slu_Status)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.slu_PaperType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_Specification)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.slu_PaperGrade)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.slu_Certification)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.slu_TransportType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_CoreDiameterOrReam)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_DiameterOrSlides)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_Length)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.slu_TrademarkStyle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.slu_SpecCustName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.slu_SpecProdName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView11)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.slu_PackType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView12)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.date_DeliverDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.date_DeliverDate.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.slu_WeightMode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView13)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.slu_Color)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView14)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.slu_SpCustomer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView16)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chk_Customer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.slu_InStockArea)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView17)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ce_OutAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cab_Weight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cab_OutWeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_Check)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView15)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Appearance.BackColor = ((System.Drawing.Color)(resources.GetObject("splitContainerControl1.Appearance.BackColor")));
            this.splitContainerControl1.Appearance.BackColor2 = ((System.Drawing.Color)(resources.GetObject("splitContainerControl1.Appearance.BackColor2")));
            this.splitContainerControl1.Appearance.BorderColor = ((System.Drawing.Color)(resources.GetObject("splitContainerControl1.Appearance.BorderColor")));
            this.splitContainerControl1.Appearance.Options.UseBackColor = true;
            this.splitContainerControl1.Appearance.Options.UseBorderColor = true;
            resources.ApplyResources(this.splitContainerControl1, "splitContainerControl1");
            this.splitContainerControl1.Horizontal = false;
            this.splitContainerControl1.Name = "splitContainerControl1";
            this.splitContainerControl1.Panel1.Appearance.BackColor = ((System.Drawing.Color)(resources.GetObject("splitContainerControl1.Panel1.Appearance.BackColor")));
            this.splitContainerControl1.Panel1.Appearance.BackColor2 = ((System.Drawing.Color)(resources.GetObject("splitContainerControl1.Panel1.Appearance.BackColor2")));
            this.splitContainerControl1.Panel1.Appearance.BorderColor = ((System.Drawing.Color)(resources.GetObject("splitContainerControl1.Panel1.Appearance.BorderColor")));
            this.splitContainerControl1.Panel1.Appearance.Options.UseBackColor = true;
            this.splitContainerControl1.Panel1.Appearance.Options.UseBorderColor = true;
            this.splitContainerControl1.Panel1.Controls.Add(this.lc_Head);
            this.splitContainerControl1.Panel1.Controls.Add(this.barDockControlLeft);
            this.splitContainerControl1.Panel1.Controls.Add(this.barDockControlRight);
            this.splitContainerControl1.Panel1.Controls.Add(this.barDockControlBottom);
            this.splitContainerControl1.Panel1.Controls.Add(this.barDockControlTop);
            resources.ApplyResources(this.splitContainerControl1.Panel1, "splitContainerControl1.Panel1");
            this.splitContainerControl1.Panel2.Controls.Add(this.FCheckAll);
            this.splitContainerControl1.Panel2.Controls.Add(this.gc_Data);
            this.splitContainerControl1.Panel2.Controls.Add(this.barDockControl3);
            this.splitContainerControl1.Panel2.Controls.Add(this.barDockControl4);
            this.splitContainerControl1.Panel2.Controls.Add(this.barDockControl2);
            this.splitContainerControl1.Panel2.Controls.Add(this.barDockControl1);
            resources.ApplyResources(this.splitContainerControl1.Panel2, "splitContainerControl1.Panel2");
            this.splitContainerControl1.SplitterPosition = 191;
            // 
            // lc_Head
            // 
            this.lc_Head.Controls.Add(this.FHSpecification);
            this.lc_Head.Controls.Add(this.FHPaperType);
            this.lc_Head.Controls.Add(this.FHProduct);
            this.lc_Head.Controls.Add(this.FHStockArea);
            this.lc_Head.Controls.Add(this.FHStock);
            this.lc_Head.Controls.Add(this.btQuery);
            this.lc_Head.Controls.Add(this.FBeginDate);
            this.lc_Head.Controls.Add(this.FHStatus);
            this.lc_Head.Controls.Add(this.FEndDate);
            resources.ApplyResources(this.lc_Head, "lc_Head");
            this.lc_Head.Name = "lc_Head";
            this.lc_Head.Root = this.layoutControlGroup3;
            // 
            // FHSpecification
            // 
            resources.ApplyResources(this.FHSpecification, "FHSpecification");
            this.FHSpecification.MenuManager = this.barManagerEntry;
            this.FHSpecification.Name = "FHSpecification";
            this.FHSpecification.Properties.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("FHSpecification.Properties.Appearance.Font")));
            this.FHSpecification.Properties.Appearance.Options.UseFont = true;
            this.FHSpecification.StyleController = this.lc_Head;
            // 
            // barManagerEntry
            // 
            this.barManagerEntry.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.entryBar});
            this.barManagerEntry.DockControls.Add(this.barDockControl1);
            this.barManagerEntry.DockControls.Add(this.barDockControl2);
            this.barManagerEntry.DockControls.Add(this.barDockControl3);
            this.barManagerEntry.DockControls.Add(this.barDockControl4);
            this.barManagerEntry.Form = this.splitContainerControl1.Panel2;
            this.barManagerEntry.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.bt_NewEntry,
            this.bt_DeleteEntry,
            this.barButtonItem2,
            this.btComplete,
            this.bt_CopyEntry});
            this.barManagerEntry.MainMenu = this.entryBar;
            this.barManagerEntry.MaxItemId = 6;
            // 
            // entryBar
            // 
            this.entryBar.BarName = "表体菜单";
            this.entryBar.DockCol = 0;
            this.entryBar.DockRow = 0;
            this.entryBar.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.entryBar.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.bt_NewEntry),
            new DevExpress.XtraBars.LinkPersistInfo(this.bt_CopyEntry),
            new DevExpress.XtraBars.LinkPersistInfo(this.bt_DeleteEntry),
            new DevExpress.XtraBars.LinkPersistInfo(this.btComplete)});
            this.entryBar.OptionsBar.AllowQuickCustomization = false;
            resources.ApplyResources(this.entryBar, "entryBar");
            // 
            // bt_NewEntry
            // 
            resources.ApplyResources(this.bt_NewEntry, "bt_NewEntry");
            this.bt_NewEntry.Id = 0;
            this.bt_NewEntry.Name = "bt_NewEntry";
            // 
            // bt_CopyEntry
            // 
            resources.ApplyResources(this.bt_CopyEntry, "bt_CopyEntry");
            this.bt_CopyEntry.Id = 5;
            this.bt_CopyEntry.Name = "bt_CopyEntry";
            this.bt_CopyEntry.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bt_CopyEntry_ItemClick);
            // 
            // bt_DeleteEntry
            // 
            resources.ApplyResources(this.bt_DeleteEntry, "bt_DeleteEntry");
            this.bt_DeleteEntry.Id = 1;
            this.bt_DeleteEntry.Name = "bt_DeleteEntry";
            // 
            // btComplete
            // 
            resources.ApplyResources(this.btComplete, "btComplete");
            this.btComplete.Id = 3;
            this.btComplete.Name = "btComplete";
            this.btComplete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.btComplete.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btComplete_ItemClick);
            // 
            // barDockControl1
            // 
            this.barDockControl1.CausesValidation = false;
            resources.ApplyResources(this.barDockControl1, "barDockControl1");
            // 
            // barDockControl2
            // 
            this.barDockControl2.CausesValidation = false;
            resources.ApplyResources(this.barDockControl2, "barDockControl2");
            // 
            // barDockControl3
            // 
            this.barDockControl3.CausesValidation = false;
            resources.ApplyResources(this.barDockControl3, "barDockControl3");
            // 
            // barDockControl4
            // 
            this.barDockControl4.CausesValidation = false;
            resources.ApplyResources(this.barDockControl4, "barDockControl4");
            // 
            // barButtonItem2
            // 
            resources.ApplyResources(this.barButtonItem2, "barButtonItem2");
            this.barButtonItem2.Id = 2;
            this.barButtonItem2.Name = "barButtonItem2";
            // 
            // FHPaperType
            // 
            resources.ApplyResources(this.FHPaperType, "FHPaperType");
            this.FHPaperType.MenuManager = this.barManagerEntry;
            this.FHPaperType.Name = "FHPaperType";
            this.FHPaperType.Properties.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("FHPaperType.Properties.Appearance.Font")));
            this.FHPaperType.Properties.Appearance.Options.UseFont = true;
            this.FHPaperType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(((DevExpress.XtraEditors.Controls.ButtonPredefines)(resources.GetObject("FHPaperType.Properties.Buttons"))))});
            this.FHPaperType.Properties.NullText = resources.GetString("FHPaperType.Properties.NullText");
            this.FHPaperType.Properties.View = this.gridView18;
            this.FHPaperType.StyleController = this.lc_Head;
            this.FHPaperType.EditValueChanged += new System.EventHandler(this.FHPaperType_EditValueChanged);
            // 
            // gridView18
            // 
            this.gridView18.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridView18.Name = "gridView18";
            this.gridView18.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView18.OptionsView.ShowGroupPanel = false;
            // 
            // FHProduct
            // 
            resources.ApplyResources(this.FHProduct, "FHProduct");
            this.FHProduct.MenuManager = this.barManagerEntry;
            this.FHProduct.Name = "FHProduct";
            this.FHProduct.Properties.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("FHProduct.Properties.Appearance.Font")));
            this.FHProduct.Properties.Appearance.Options.UseFont = true;
            this.FHProduct.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(((DevExpress.XtraEditors.Controls.ButtonPredefines)(resources.GetObject("FHProduct.Properties.Buttons"))))});
            this.FHProduct.Properties.NullText = resources.GetString("FHProduct.Properties.NullText");
            this.FHProduct.Properties.View = this.searchLookUpEdit1View;
            this.FHProduct.StyleController = this.lc_Head;
            // 
            // searchLookUpEdit1View
            // 
            this.searchLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.searchLookUpEdit1View.Name = "searchLookUpEdit1View";
            this.searchLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.searchLookUpEdit1View.OptionsView.ShowGroupPanel = false;
            // 
            // FHStockArea
            // 
            resources.ApplyResources(this.FHStockArea, "FHStockArea");
            this.FHStockArea.MenuManager = this.barManagerEntry;
            this.FHStockArea.Name = "FHStockArea";
            this.FHStockArea.Properties.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("FHStockArea.Properties.Appearance.Font")));
            this.FHStockArea.Properties.Appearance.Options.UseFont = true;
            this.FHStockArea.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(((DevExpress.XtraEditors.Controls.ButtonPredefines)(resources.GetObject("FHStockArea.Properties.Buttons"))))});
            this.FHStockArea.Properties.PopupFormSize = new System.Drawing.Size(340, 400);
            this.FHStockArea.Properties.PopupResizeMode = DevExpress.XtraEditors.Controls.ResizeMode.FrameResize;
            this.FHStockArea.StyleController = this.lc_Head;
            // 
            // FHStock
            // 
            resources.ApplyResources(this.FHStock, "FHStock");
            this.FHStock.MenuManager = this.barManagerEntry;
            this.FHStock.Name = "FHStock";
            this.FHStock.Properties.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("FHStock.Properties.Appearance.Font")));
            this.FHStock.Properties.Appearance.Options.UseFont = true;
            this.FHStock.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(((DevExpress.XtraEditors.Controls.ButtonPredefines)(resources.GetObject("FHStock.Properties.Buttons"))))});
            this.FHStock.Properties.PopupFormSize = new System.Drawing.Size(340, 400);
            this.FHStock.Properties.PopupResizeMode = DevExpress.XtraEditors.Controls.ResizeMode.FrameResize;
            this.FHStock.StyleController = this.lc_Head;
            this.FHStock.EditValueChanged += new System.EventHandler(this.FHStock_EditValueChanged);
            // 
            // btQuery
            // 
            this.btQuery.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("btQuery.Appearance.Font")));
            this.btQuery.Appearance.Options.UseFont = true;
            resources.ApplyResources(this.btQuery, "btQuery");
            this.btQuery.Name = "btQuery";
            this.btQuery.StyleController = this.lc_Head;
            this.btQuery.Click += new System.EventHandler(this.btQuery_Click);
            // 
            // FBeginDate
            // 
            resources.ApplyResources(this.FBeginDate, "FBeginDate");
            this.FBeginDate.MenuManager = this.barManagerEntry;
            this.FBeginDate.Name = "FBeginDate";
            this.FBeginDate.Properties.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("FBeginDate.Properties.Appearance.Font")));
            this.FBeginDate.Properties.Appearance.Options.UseFont = true;
            this.FBeginDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(((DevExpress.XtraEditors.Controls.ButtonPredefines)(resources.GetObject("FBeginDate.Properties.Buttons"))))});
            this.FBeginDate.Properties.CalendarTimeEditing = DevExpress.Utils.DefaultBoolean.True;
            this.FBeginDate.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(((DevExpress.XtraEditors.Controls.ButtonPredefines)(resources.GetObject("FBeginDate.Properties.CalendarTimeProperties.Buttons"))))});
            this.FBeginDate.Properties.CalendarView = DevExpress.XtraEditors.Repository.CalendarView.Vista;
            this.FBeginDate.Properties.DisplayFormat.FormatString = "G";
            this.FBeginDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.FBeginDate.Properties.EditFormat.FormatString = "G";
            this.FBeginDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.FBeginDate.Properties.Mask.EditMask = resources.GetString("FBeginDate.Properties.Mask.EditMask");
            this.FBeginDate.Properties.VistaDisplayMode = DevExpress.Utils.DefaultBoolean.True;
            this.FBeginDate.StyleController = this.lc_Head;
            // 
            // FHStatus
            // 
            resources.ApplyResources(this.FHStatus, "FHStatus");
            this.FHStatus.Name = "FHStatus";
            this.FHStatus.Properties.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("FHStatus.Properties.Appearance.Font")));
            this.FHStatus.Properties.Appearance.Options.UseFont = true;
            this.FHStatus.StyleController = this.lc_Head;
            // 
            // FEndDate
            // 
            resources.ApplyResources(this.FEndDate, "FEndDate");
            this.FEndDate.MenuManager = this.barManagerEntry;
            this.FEndDate.Name = "FEndDate";
            this.FEndDate.Properties.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("FEndDate.Properties.Appearance.Font")));
            this.FEndDate.Properties.Appearance.Options.UseFont = true;
            this.FEndDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(((DevExpress.XtraEditors.Controls.ButtonPredefines)(resources.GetObject("FEndDate.Properties.Buttons"))))});
            this.FEndDate.Properties.CalendarTimeEditing = DevExpress.Utils.DefaultBoolean.True;
            this.FEndDate.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(((DevExpress.XtraEditors.Controls.ButtonPredefines)(resources.GetObject("FEndDate.Properties.CalendarTimeProperties.Buttons"))))});
            this.FEndDate.Properties.CalendarView = DevExpress.XtraEditors.Repository.CalendarView.Vista;
            this.FEndDate.Properties.DisplayFormat.FormatString = "G";
            this.FEndDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.FEndDate.Properties.EditFormat.FormatString = "G";
            this.FEndDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.FEndDate.Properties.Mask.EditMask = resources.GetString("FEndDate.Properties.Mask.EditMask");
            this.FEndDate.Properties.VistaDisplayMode = DevExpress.Utils.DefaultBoolean.True;
            this.FEndDate.StyleController = this.lc_Head;
            // 
            // layoutControlGroup3
            // 
            this.layoutControlGroup3.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup3.GroupBordersVisible = false;
            this.layoutControlGroup3.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lo_EndDate,
            this.lo_Stock,
            this.layoutControlItem3,
            this.lo_Product,
            this.lo_BeginDate,
            this.lo_Status,
            this.layoutControlItem1,
            this.emptySpaceItem1,
            this.lo_HPaperType,
            this.lo_HSpecification});
            this.layoutControlGroup3.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup3.Name = "layoutControlGroup3";
            this.layoutControlGroup3.OptionsItemText.TextToControlDistance = 5;
            this.layoutControlGroup3.Size = new System.Drawing.Size(1412, 141);
            this.layoutControlGroup3.TextVisible = false;
            // 
            // lo_EndDate
            // 
            this.lo_EndDate.Control = this.FEndDate;
            this.lo_EndDate.Location = new System.Drawing.Point(355, 44);
            this.lo_EndDate.Name = "lo_EndDate";
            this.lo_EndDate.Size = new System.Drawing.Size(376, 67);
            resources.ApplyResources(this.lo_EndDate, "lo_EndDate");
            this.lo_EndDate.TextSize = new System.Drawing.Size(72, 22);
            // 
            // lo_Stock
            // 
            this.lo_Stock.Control = this.FHStock;
            this.lo_Stock.Location = new System.Drawing.Point(355, 0);
            this.lo_Stock.Name = "lo_Stock";
            this.lo_Stock.Size = new System.Drawing.Size(376, 44);
            resources.ApplyResources(this.lo_Stock, "lo_Stock");
            this.lo_Stock.TextSize = new System.Drawing.Size(72, 22);
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.FHStockArea;
            this.layoutControlItem3.Location = new System.Drawing.Point(731, 0);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(296, 44);
            resources.ApplyResources(this.layoutControlItem3, "layoutControlItem3");
            this.layoutControlItem3.TextSize = new System.Drawing.Size(72, 22);
            // 
            // lo_Product
            // 
            this.lo_Product.Control = this.FHProduct;
            this.lo_Product.Location = new System.Drawing.Point(0, 0);
            this.lo_Product.Name = "lo_Product";
            this.lo_Product.Size = new System.Drawing.Size(355, 44);
            resources.ApplyResources(this.lo_Product, "lo_Product");
            this.lo_Product.TextSize = new System.Drawing.Size(72, 22);
            // 
            // lo_BeginDate
            // 
            this.lo_BeginDate.Control = this.FBeginDate;
            this.lo_BeginDate.Location = new System.Drawing.Point(0, 44);
            this.lo_BeginDate.Name = "lo_BeginDate";
            this.lo_BeginDate.Size = new System.Drawing.Size(355, 67);
            resources.ApplyResources(this.lo_BeginDate, "lo_BeginDate");
            this.lo_BeginDate.TextSize = new System.Drawing.Size(72, 22);
            // 
            // lo_Status
            // 
            this.lo_Status.Control = this.FHStatus;
            this.lo_Status.Location = new System.Drawing.Point(1027, 44);
            this.lo_Status.Name = "lo_Status";
            this.lo_Status.Size = new System.Drawing.Size(247, 67);
            resources.ApplyResources(this.lo_Status, "lo_Status");
            this.lo_Status.TextSize = new System.Drawing.Size(72, 22);
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.btQuery;
            this.layoutControlItem1.Location = new System.Drawing.Point(1274, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(108, 46);
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextVisible = false;
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.Location = new System.Drawing.Point(1274, 46);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(108, 65);
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // lo_HPaperType
            // 
            this.lo_HPaperType.Control = this.FHPaperType;
            this.lo_HPaperType.Location = new System.Drawing.Point(1027, 0);
            this.lo_HPaperType.Name = "lo_HPaperType";
            this.lo_HPaperType.Size = new System.Drawing.Size(247, 44);
            resources.ApplyResources(this.lo_HPaperType, "lo_HPaperType");
            this.lo_HPaperType.TextSize = new System.Drawing.Size(72, 22);
            // 
            // lo_HSpecification
            // 
            this.lo_HSpecification.Control = this.FHSpecification;
            this.lo_HSpecification.Location = new System.Drawing.Point(731, 44);
            this.lo_HSpecification.Name = "lo_HSpecification";
            this.lo_HSpecification.Size = new System.Drawing.Size(296, 67);
            resources.ApplyResources(this.lo_HSpecification, "lo_HSpecification");
            this.lo_HSpecification.TextSize = new System.Drawing.Size(72, 22);
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
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            resources.ApplyResources(this.barDockControlBottom, "barDockControlBottom");
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            resources.ApplyResources(this.barDockControlTop, "barDockControlTop");
            // 
            // FCheckAll
            // 
            resources.ApplyResources(this.FCheckAll, "FCheckAll");
            this.FCheckAll.MenuManager = this.barManagerEntry;
            this.FCheckAll.Name = "FCheckAll";
            this.FCheckAll.Properties.Appearance.BackColor = ((System.Drawing.Color)(resources.GetObject("FCheckAll.Properties.Appearance.BackColor")));
            this.FCheckAll.Properties.Appearance.Options.UseBackColor = true;
            this.FCheckAll.Properties.Caption = resources.GetString("FCheckAll.Properties.Caption");
            // 
            // gc_Data
            // 
            resources.ApplyResources(this.gc_Data, "gc_Data");
            this.gc_Data.MainView = this.gv_Entry;
            this.gc_Data.Name = "gc_Data";
            this.gc_Data.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.slu_StockAreaPlan,
            this.cab_Amount,
            this.deDate,
            this.cab_PlanAmount,
            this.slu_Status,
            this.slu_Product,
            this.slu_Stock,
            this.slu_StockArea,
            this.slu_PaperType,
            this.cb_Specification,
            this.slu_PaperGrade,
            this.slu_Certification,
            this.slu_TransportType,
            this.slu_TrademarkStyle,
            this.cb_CoreDiameterOrReam,
            this.cb_DiameterOrSlides,
            this.cb_Length,
            this.slu_SpecCustName,
            this.slu_SpecProdName,
            this.slu_PackType,
            this.date_DeliverDate,
            this.slu_WeightMode,
            this.slu_Color,
            this.chk_Customer,
            this.ce_OutAmount,
            this.cab_Weight,
            this.cab_OutWeight,
            this.slu_SpCustomer,
            this.slu_InStockArea,
            this.cb_Check});
            this.gc_Data.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gv_Entry});
            // 
            // gv_Entry
            // 
            this.gv_Entry.Appearance.Empty.BackColor = ((System.Drawing.Color)(resources.GetObject("gv_Entry.Appearance.Empty.BackColor")));
            this.gv_Entry.Appearance.Empty.Options.UseBackColor = true;
            this.gv_Entry.Appearance.FocusedRow.BackColor = ((System.Drawing.Color)(resources.GetObject("gv_Entry.Appearance.FocusedRow.BackColor")));
            this.gv_Entry.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gv_Entry.Appearance.Row.BackColor = ((System.Drawing.Color)(resources.GetObject("gv_Entry.Appearance.Row.BackColor")));
            this.gv_Entry.Appearance.Row.Options.UseBackColor = true;
            this.gv_Entry.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.FEntryID,
            this.FName,
            this.FProduct,
            this.FStock,
            this.FStockArea,
            this.FStockAreaPlan,
            this.FAmount,
            this.FDate,
            this.FPlanAmount,
            this.FStatus,
            this.FPaperType,
            this.FSpecification,
            this.FPaperGrade,
            this.FCertification,
            this.FTransportType,
            this.FCoreDiameterOrReam,
            this.FDiameterOrSlides,
            this.FLength,
            this.FTrademarkStyle,
            this.FSpecCustName,
            this.FSpecProdName,
            this.FPackType,
            this.FDeliverDate,
            this.FWeightMode,
            this.FColor,
            this.FPoNumber,
            this.FSpCustomer,
            this.FMemo,
            this.FTotalCapacity,
            this.FUsedCapacity,
            this.FCustomer,
            this.FInStockArea,
            this.FOutAmount,
            this.FWeight,
            this.FOutWeight,
            this.FStockDate,
            this.FCheck});
            this.gv_Entry.GridControl = this.gc_Data;
            this.gv_Entry.IndicatorWidth = 60;
            this.gv_Entry.Name = "gv_Entry";
            this.gv_Entry.OptionsView.ColumnAutoWidth = false;
            this.gv_Entry.OptionsView.ShowAutoFilterRow = true;
            this.gv_Entry.OptionsView.ShowGroupPanel = false;
            // 
            // FEntryID
            // 
            this.FEntryID.AppearanceCell.Font = ((System.Drawing.Font)(resources.GetObject("FEntryID.AppearanceCell.Font")));
            this.FEntryID.AppearanceCell.Options.UseFont = true;
            resources.ApplyResources(this.FEntryID, "FEntryID");
            this.FEntryID.FieldName = "FENTRYID";
            this.FEntryID.Name = "FEntryID";
            // 
            // FName
            // 
            resources.ApplyResources(this.FName, "FName");
            this.FName.FieldName = "FNAME";
            this.FName.Name = "FName";
            // 
            // FProduct
            // 
            resources.ApplyResources(this.FProduct, "FProduct");
            this.FProduct.ColumnEdit = this.slu_Product;
            this.FProduct.FieldName = "FPRODUCT";
            this.FProduct.Name = "FProduct";
            // 
            // slu_Product
            // 
            resources.ApplyResources(this.slu_Product, "slu_Product");
            this.slu_Product.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(((DevExpress.XtraEditors.Controls.ButtonPredefines)(resources.GetObject("slu_Product.Buttons"))))});
            this.slu_Product.Name = "slu_Product";
            this.slu_Product.View = this.gridView3;
            // 
            // gridView3
            // 
            this.gridView3.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridView3.Name = "gridView3";
            this.gridView3.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView3.OptionsView.ShowGroupPanel = false;
            // 
            // FStock
            // 
            resources.ApplyResources(this.FStock, "FStock");
            this.FStock.ColumnEdit = this.slu_Stock;
            this.FStock.FieldName = "FSTOCK";
            this.FStock.Name = "FStock";
            this.FStock.OptionsColumn.ReadOnly = true;
            // 
            // slu_Stock
            // 
            resources.ApplyResources(this.slu_Stock, "slu_Stock");
            this.slu_Stock.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(((DevExpress.XtraEditors.Controls.ButtonPredefines)(resources.GetObject("slu_Stock.Buttons"))))});
            this.slu_Stock.Name = "slu_Stock";
            this.slu_Stock.View = this.gridView1;
            // 
            // gridView1
            // 
            this.gridView1.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // FStockArea
            // 
            resources.ApplyResources(this.FStockArea, "FStockArea");
            this.FStockArea.ColumnEdit = this.slu_StockArea;
            this.FStockArea.FieldName = "FSTOCKAREA";
            this.FStockArea.Name = "FStockArea";
            // 
            // slu_StockArea
            // 
            resources.ApplyResources(this.slu_StockArea, "slu_StockArea");
            this.slu_StockArea.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(((DevExpress.XtraEditors.Controls.ButtonPredefines)(resources.GetObject("slu_StockArea.Buttons"))))});
            this.slu_StockArea.Name = "slu_StockArea";
            this.slu_StockArea.View = this.gridView4;
            // 
            // gridView4
            // 
            this.gridView4.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridView4.Name = "gridView4";
            this.gridView4.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView4.OptionsView.ShowGroupPanel = false;
            // 
            // FStockAreaPlan
            // 
            resources.ApplyResources(this.FStockAreaPlan, "FStockAreaPlan");
            this.FStockAreaPlan.ColumnEdit = this.slu_StockAreaPlan;
            this.FStockAreaPlan.FieldName = "FSTOCKAREAPLAN";
            this.FStockAreaPlan.Name = "FStockAreaPlan";
            // 
            // slu_StockAreaPlan
            // 
            resources.ApplyResources(this.slu_StockAreaPlan, "slu_StockAreaPlan");
            this.slu_StockAreaPlan.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(((DevExpress.XtraEditors.Controls.ButtonPredefines)(resources.GetObject("slu_StockAreaPlan.Buttons"))))});
            this.slu_StockAreaPlan.Name = "slu_StockAreaPlan";
            this.slu_StockAreaPlan.View = this.repositoryItemSearchLookUpEdit1View;
            // 
            // repositoryItemSearchLookUpEdit1View
            // 
            this.repositoryItemSearchLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.repositoryItemSearchLookUpEdit1View.Name = "repositoryItemSearchLookUpEdit1View";
            this.repositoryItemSearchLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.repositoryItemSearchLookUpEdit1View.OptionsView.ShowGroupPanel = false;
            // 
            // FAmount
            // 
            resources.ApplyResources(this.FAmount, "FAmount");
            this.FAmount.ColumnEdit = this.cab_Amount;
            this.FAmount.FieldName = "FAMOUNT";
            this.FAmount.Name = "FAmount";
            // 
            // cab_Amount
            // 
            resources.ApplyResources(this.cab_Amount, "cab_Amount");
            this.cab_Amount.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(((DevExpress.XtraEditors.Controls.ButtonPredefines)(resources.GetObject("cab_Amount.Buttons"))))});
            this.cab_Amount.Name = "cab_Amount";
            // 
            // FDate
            // 
            resources.ApplyResources(this.FDate, "FDate");
            this.FDate.ColumnEdit = this.deDate;
            this.FDate.FieldName = "FDATE";
            this.FDate.Name = "FDate";
            // 
            // deDate
            // 
            resources.ApplyResources(this.deDate, "deDate");
            this.deDate.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(((DevExpress.XtraEditors.Controls.ButtonPredefines)(resources.GetObject("deDate.Buttons"))))});
            this.deDate.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(((DevExpress.XtraEditors.Controls.ButtonPredefines)(resources.GetObject("deDate.CalendarTimeProperties.Buttons"))))});
            this.deDate.Name = "deDate";
            this.deDate.ReadOnly = true;
            // 
            // FPlanAmount
            // 
            resources.ApplyResources(this.FPlanAmount, "FPlanAmount");
            this.FPlanAmount.ColumnEdit = this.cab_PlanAmount;
            this.FPlanAmount.FieldName = "FPLANAMOUNT";
            this.FPlanAmount.Name = "FPlanAmount";
            // 
            // cab_PlanAmount
            // 
            resources.ApplyResources(this.cab_PlanAmount, "cab_PlanAmount");
            this.cab_PlanAmount.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(((DevExpress.XtraEditors.Controls.ButtonPredefines)(resources.GetObject("cab_PlanAmount.Buttons"))))});
            this.cab_PlanAmount.Name = "cab_PlanAmount";
            // 
            // FStatus
            // 
            resources.ApplyResources(this.FStatus, "FStatus");
            this.FStatus.ColumnEdit = this.slu_Status;
            this.FStatus.FieldName = "FSTATUS";
            this.FStatus.Name = "FStatus";
            // 
            // slu_Status
            // 
            resources.ApplyResources(this.slu_Status, "slu_Status");
            this.slu_Status.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(((DevExpress.XtraEditors.Controls.ButtonPredefines)(resources.GetObject("slu_Status.Buttons"))))});
            this.slu_Status.Name = "slu_Status";
            this.slu_Status.ReadOnly = true;
            this.slu_Status.View = this.gridView2;
            // 
            // gridView2
            // 
            this.gridView2.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridView2.Name = "gridView2";
            this.gridView2.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView2.OptionsView.ShowGroupPanel = false;
            // 
            // FPaperType
            // 
            resources.ApplyResources(this.FPaperType, "FPaperType");
            this.FPaperType.ColumnEdit = this.slu_PaperType;
            this.FPaperType.FieldName = "FPAPERTYPE";
            this.FPaperType.Name = "FPaperType";
            // 
            // slu_PaperType
            // 
            resources.ApplyResources(this.slu_PaperType, "slu_PaperType");
            this.slu_PaperType.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(((DevExpress.XtraEditors.Controls.ButtonPredefines)(resources.GetObject("slu_PaperType.Buttons"))))});
            this.slu_PaperType.Name = "slu_PaperType";
            this.slu_PaperType.View = this.gridView5;
            // 
            // gridView5
            // 
            this.gridView5.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridView5.Name = "gridView5";
            this.gridView5.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView5.OptionsView.ShowGroupPanel = false;
            // 
            // FSpecification
            // 
            resources.ApplyResources(this.FSpecification, "FSpecification");
            this.FSpecification.ColumnEdit = this.cb_Specification;
            this.FSpecification.FieldName = "FSPECIFICATION";
            this.FSpecification.Name = "FSpecification";
            // 
            // cb_Specification
            // 
            resources.ApplyResources(this.cb_Specification, "cb_Specification");
            this.cb_Specification.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(((DevExpress.XtraEditors.Controls.ButtonPredefines)(resources.GetObject("cb_Specification.Buttons"))))});
            this.cb_Specification.Name = "cb_Specification";
            // 
            // FPaperGrade
            // 
            resources.ApplyResources(this.FPaperGrade, "FPaperGrade");
            this.FPaperGrade.ColumnEdit = this.slu_PaperGrade;
            this.FPaperGrade.FieldName = "FPAPERGRADE";
            this.FPaperGrade.Name = "FPaperGrade";
            // 
            // slu_PaperGrade
            // 
            resources.ApplyResources(this.slu_PaperGrade, "slu_PaperGrade");
            this.slu_PaperGrade.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(((DevExpress.XtraEditors.Controls.ButtonPredefines)(resources.GetObject("slu_PaperGrade.Buttons"))))});
            this.slu_PaperGrade.Name = "slu_PaperGrade";
            this.slu_PaperGrade.View = this.gridView6;
            // 
            // gridView6
            // 
            this.gridView6.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridView6.Name = "gridView6";
            this.gridView6.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView6.OptionsView.ShowGroupPanel = false;
            // 
            // FCertification
            // 
            resources.ApplyResources(this.FCertification, "FCertification");
            this.FCertification.ColumnEdit = this.slu_Certification;
            this.FCertification.FieldName = "FCERTIFICATION";
            this.FCertification.Name = "FCertification";
            // 
            // slu_Certification
            // 
            resources.ApplyResources(this.slu_Certification, "slu_Certification");
            this.slu_Certification.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(((DevExpress.XtraEditors.Controls.ButtonPredefines)(resources.GetObject("slu_Certification.Buttons"))))});
            this.slu_Certification.Name = "slu_Certification";
            this.slu_Certification.View = this.gridView7;
            // 
            // gridView7
            // 
            this.gridView7.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridView7.Name = "gridView7";
            this.gridView7.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView7.OptionsView.ShowGroupPanel = false;
            // 
            // FTransportType
            // 
            resources.ApplyResources(this.FTransportType, "FTransportType");
            this.FTransportType.ColumnEdit = this.slu_TransportType;
            this.FTransportType.FieldName = "FTRANSPORTTYPE";
            this.FTransportType.Name = "FTransportType";
            // 
            // slu_TransportType
            // 
            resources.ApplyResources(this.slu_TransportType, "slu_TransportType");
            this.slu_TransportType.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(((DevExpress.XtraEditors.Controls.ButtonPredefines)(resources.GetObject("slu_TransportType.Buttons"))))});
            this.slu_TransportType.Name = "slu_TransportType";
            this.slu_TransportType.View = this.gridView8;
            // 
            // gridView8
            // 
            this.gridView8.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridView8.Name = "gridView8";
            this.gridView8.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView8.OptionsView.ShowGroupPanel = false;
            // 
            // FCoreDiameterOrReam
            // 
            resources.ApplyResources(this.FCoreDiameterOrReam, "FCoreDiameterOrReam");
            this.FCoreDiameterOrReam.ColumnEdit = this.cb_CoreDiameterOrReam;
            this.FCoreDiameterOrReam.FieldName = "FCOREDIAMETERORREAM";
            this.FCoreDiameterOrReam.Name = "FCoreDiameterOrReam";
            // 
            // cb_CoreDiameterOrReam
            // 
            resources.ApplyResources(this.cb_CoreDiameterOrReam, "cb_CoreDiameterOrReam");
            this.cb_CoreDiameterOrReam.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(((DevExpress.XtraEditors.Controls.ButtonPredefines)(resources.GetObject("cb_CoreDiameterOrReam.Buttons"))))});
            this.cb_CoreDiameterOrReam.Name = "cb_CoreDiameterOrReam";
            // 
            // FDiameterOrSlides
            // 
            resources.ApplyResources(this.FDiameterOrSlides, "FDiameterOrSlides");
            this.FDiameterOrSlides.ColumnEdit = this.cb_DiameterOrSlides;
            this.FDiameterOrSlides.FieldName = "FDIAMETERORSLIDES";
            this.FDiameterOrSlides.Name = "FDiameterOrSlides";
            // 
            // cb_DiameterOrSlides
            // 
            resources.ApplyResources(this.cb_DiameterOrSlides, "cb_DiameterOrSlides");
            this.cb_DiameterOrSlides.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(((DevExpress.XtraEditors.Controls.ButtonPredefines)(resources.GetObject("cb_DiameterOrSlides.Buttons"))))});
            this.cb_DiameterOrSlides.Name = "cb_DiameterOrSlides";
            // 
            // FLength
            // 
            resources.ApplyResources(this.FLength, "FLength");
            this.FLength.ColumnEdit = this.cb_Length;
            this.FLength.FieldName = "FLENGTH";
            this.FLength.Name = "FLength";
            // 
            // cb_Length
            // 
            resources.ApplyResources(this.cb_Length, "cb_Length");
            this.cb_Length.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(((DevExpress.XtraEditors.Controls.ButtonPredefines)(resources.GetObject("cb_Length.Buttons"))))});
            this.cb_Length.Name = "cb_Length";
            // 
            // FTrademarkStyle
            // 
            resources.ApplyResources(this.FTrademarkStyle, "FTrademarkStyle");
            this.FTrademarkStyle.ColumnEdit = this.slu_TrademarkStyle;
            this.FTrademarkStyle.FieldName = "FTRADEMARKSTYLE";
            this.FTrademarkStyle.Name = "FTrademarkStyle";
            // 
            // slu_TrademarkStyle
            // 
            resources.ApplyResources(this.slu_TrademarkStyle, "slu_TrademarkStyle");
            this.slu_TrademarkStyle.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(((DevExpress.XtraEditors.Controls.ButtonPredefines)(resources.GetObject("slu_TrademarkStyle.Buttons"))))});
            this.slu_TrademarkStyle.Name = "slu_TrademarkStyle";
            this.slu_TrademarkStyle.View = this.gridView9;
            // 
            // gridView9
            // 
            this.gridView9.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridView9.Name = "gridView9";
            this.gridView9.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView9.OptionsView.ShowGroupPanel = false;
            // 
            // FSpecCustName
            // 
            resources.ApplyResources(this.FSpecCustName, "FSpecCustName");
            this.FSpecCustName.ColumnEdit = this.slu_SpecCustName;
            this.FSpecCustName.FieldName = "FSPECCUSTNAME";
            this.FSpecCustName.Name = "FSpecCustName";
            // 
            // slu_SpecCustName
            // 
            resources.ApplyResources(this.slu_SpecCustName, "slu_SpecCustName");
            this.slu_SpecCustName.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(((DevExpress.XtraEditors.Controls.ButtonPredefines)(resources.GetObject("slu_SpecCustName.Buttons"))))});
            this.slu_SpecCustName.Name = "slu_SpecCustName";
            this.slu_SpecCustName.View = this.gridView10;
            // 
            // gridView10
            // 
            this.gridView10.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridView10.Name = "gridView10";
            this.gridView10.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView10.OptionsView.ShowGroupPanel = false;
            // 
            // FSpecProdName
            // 
            resources.ApplyResources(this.FSpecProdName, "FSpecProdName");
            this.FSpecProdName.ColumnEdit = this.slu_SpecProdName;
            this.FSpecProdName.FieldName = "FSPECPRODNAME";
            this.FSpecProdName.Name = "FSpecProdName";
            // 
            // slu_SpecProdName
            // 
            resources.ApplyResources(this.slu_SpecProdName, "slu_SpecProdName");
            this.slu_SpecProdName.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(((DevExpress.XtraEditors.Controls.ButtonPredefines)(resources.GetObject("slu_SpecProdName.Buttons"))))});
            this.slu_SpecProdName.Name = "slu_SpecProdName";
            this.slu_SpecProdName.View = this.gridView11;
            // 
            // gridView11
            // 
            this.gridView11.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridView11.Name = "gridView11";
            this.gridView11.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView11.OptionsView.ShowGroupPanel = false;
            // 
            // FPackType
            // 
            resources.ApplyResources(this.FPackType, "FPackType");
            this.FPackType.ColumnEdit = this.slu_PackType;
            this.FPackType.FieldName = "FPACKTYPE";
            this.FPackType.Name = "FPackType";
            // 
            // slu_PackType
            // 
            resources.ApplyResources(this.slu_PackType, "slu_PackType");
            this.slu_PackType.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(((DevExpress.XtraEditors.Controls.ButtonPredefines)(resources.GetObject("slu_PackType.Buttons"))))});
            this.slu_PackType.Name = "slu_PackType";
            this.slu_PackType.View = this.gridView12;
            // 
            // gridView12
            // 
            this.gridView12.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridView12.Name = "gridView12";
            this.gridView12.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView12.OptionsView.ShowGroupPanel = false;
            // 
            // FDeliverDate
            // 
            resources.ApplyResources(this.FDeliverDate, "FDeliverDate");
            this.FDeliverDate.ColumnEdit = this.date_DeliverDate;
            this.FDeliverDate.FieldName = "FDELIVERDATE";
            this.FDeliverDate.Name = "FDeliverDate";
            // 
            // date_DeliverDate
            // 
            resources.ApplyResources(this.date_DeliverDate, "date_DeliverDate");
            this.date_DeliverDate.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(((DevExpress.XtraEditors.Controls.ButtonPredefines)(resources.GetObject("date_DeliverDate.Buttons"))))});
            this.date_DeliverDate.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(((DevExpress.XtraEditors.Controls.ButtonPredefines)(resources.GetObject("date_DeliverDate.CalendarTimeProperties.Buttons"))))});
            this.date_DeliverDate.Name = "date_DeliverDate";
            // 
            // FWeightMode
            // 
            resources.ApplyResources(this.FWeightMode, "FWeightMode");
            this.FWeightMode.ColumnEdit = this.slu_WeightMode;
            this.FWeightMode.FieldName = "FWEIGHTMODE";
            this.FWeightMode.Name = "FWeightMode";
            // 
            // slu_WeightMode
            // 
            resources.ApplyResources(this.slu_WeightMode, "slu_WeightMode");
            this.slu_WeightMode.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(((DevExpress.XtraEditors.Controls.ButtonPredefines)(resources.GetObject("slu_WeightMode.Buttons"))))});
            this.slu_WeightMode.Name = "slu_WeightMode";
            this.slu_WeightMode.View = this.gridView13;
            // 
            // gridView13
            // 
            this.gridView13.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridView13.Name = "gridView13";
            this.gridView13.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView13.OptionsView.ShowGroupPanel = false;
            // 
            // FColor
            // 
            resources.ApplyResources(this.FColor, "FColor");
            this.FColor.ColumnEdit = this.slu_Color;
            this.FColor.FieldName = "FCOLOR";
            this.FColor.Name = "FColor";
            // 
            // slu_Color
            // 
            resources.ApplyResources(this.slu_Color, "slu_Color");
            this.slu_Color.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(((DevExpress.XtraEditors.Controls.ButtonPredefines)(resources.GetObject("slu_Color.Buttons"))))});
            this.slu_Color.Name = "slu_Color";
            this.slu_Color.View = this.gridView14;
            // 
            // gridView14
            // 
            this.gridView14.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridView14.Name = "gridView14";
            this.gridView14.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView14.OptionsView.ShowGroupPanel = false;
            // 
            // FPoNumber
            // 
            resources.ApplyResources(this.FPoNumber, "FPoNumber");
            this.FPoNumber.FieldName = "FPONUMBER";
            this.FPoNumber.Name = "FPoNumber";
            // 
            // FSpCustomer
            // 
            resources.ApplyResources(this.FSpCustomer, "FSpCustomer");
            this.FSpCustomer.ColumnEdit = this.slu_SpCustomer;
            this.FSpCustomer.FieldName = "FSPCUSTOMER";
            this.FSpCustomer.Name = "FSpCustomer";
            // 
            // slu_SpCustomer
            // 
            resources.ApplyResources(this.slu_SpCustomer, "slu_SpCustomer");
            this.slu_SpCustomer.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(((DevExpress.XtraEditors.Controls.ButtonPredefines)(resources.GetObject("slu_SpCustomer.Buttons"))))});
            this.slu_SpCustomer.Name = "slu_SpCustomer";
            this.slu_SpCustomer.View = this.gridView16;
            // 
            // gridView16
            // 
            this.gridView16.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridView16.Name = "gridView16";
            this.gridView16.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView16.OptionsView.ShowGroupPanel = false;
            // 
            // FMemo
            // 
            resources.ApplyResources(this.FMemo, "FMemo");
            this.FMemo.FieldName = "FMEMO";
            this.FMemo.Name = "FMemo";
            // 
            // FTotalCapacity
            // 
            resources.ApplyResources(this.FTotalCapacity, "FTotalCapacity");
            this.FTotalCapacity.FieldName = "FTOTALCAPACITY";
            this.FTotalCapacity.Name = "FTotalCapacity";
            this.FTotalCapacity.OptionsColumn.ReadOnly = true;
            // 
            // FUsedCapacity
            // 
            resources.ApplyResources(this.FUsedCapacity, "FUsedCapacity");
            this.FUsedCapacity.FieldName = "FUSEDCAPACITY";
            this.FUsedCapacity.Name = "FUsedCapacity";
            this.FUsedCapacity.OptionsColumn.ReadOnly = true;
            // 
            // FCustomer
            // 
            resources.ApplyResources(this.FCustomer, "FCustomer");
            this.FCustomer.ColumnEdit = this.chk_Customer;
            this.FCustomer.FieldName = "FCUSTOMER";
            this.FCustomer.Name = "FCustomer";
            // 
            // chk_Customer
            // 
            resources.ApplyResources(this.chk_Customer, "chk_Customer");
            this.chk_Customer.Name = "chk_Customer";
            this.chk_Customer.ValueChecked = 1;
            this.chk_Customer.ValueUnchecked = 0;
            // 
            // FInStockArea
            // 
            resources.ApplyResources(this.FInStockArea, "FInStockArea");
            this.FInStockArea.ColumnEdit = this.slu_InStockArea;
            this.FInStockArea.FieldName = "FINSTOCKAREA";
            this.FInStockArea.Name = "FInStockArea";
            // 
            // slu_InStockArea
            // 
            resources.ApplyResources(this.slu_InStockArea, "slu_InStockArea");
            this.slu_InStockArea.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(((DevExpress.XtraEditors.Controls.ButtonPredefines)(resources.GetObject("slu_InStockArea.Buttons"))))});
            this.slu_InStockArea.Name = "slu_InStockArea";
            this.slu_InStockArea.View = this.gridView17;
            // 
            // gridView17
            // 
            this.gridView17.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridView17.Name = "gridView17";
            this.gridView17.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView17.OptionsView.ShowGroupPanel = false;
            // 
            // FOutAmount
            // 
            resources.ApplyResources(this.FOutAmount, "FOutAmount");
            this.FOutAmount.ColumnEdit = this.ce_OutAmount;
            this.FOutAmount.FieldName = "FOUTAMOUNT";
            this.FOutAmount.Name = "FOutAmount";
            // 
            // ce_OutAmount
            // 
            resources.ApplyResources(this.ce_OutAmount, "ce_OutAmount");
            this.ce_OutAmount.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(((DevExpress.XtraEditors.Controls.ButtonPredefines)(resources.GetObject("ce_OutAmount.Buttons"))))});
            this.ce_OutAmount.Name = "ce_OutAmount";
            // 
            // FWeight
            // 
            resources.ApplyResources(this.FWeight, "FWeight");
            this.FWeight.ColumnEdit = this.cab_Weight;
            this.FWeight.FieldName = "FWEIGHT";
            this.FWeight.Name = "FWeight";
            // 
            // cab_Weight
            // 
            resources.ApplyResources(this.cab_Weight, "cab_Weight");
            this.cab_Weight.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(((DevExpress.XtraEditors.Controls.ButtonPredefines)(resources.GetObject("cab_Weight.Buttons"))))});
            this.cab_Weight.Name = "cab_Weight";
            // 
            // FOutWeight
            // 
            resources.ApplyResources(this.FOutWeight, "FOutWeight");
            this.FOutWeight.ColumnEdit = this.cab_OutWeight;
            this.FOutWeight.FieldName = "FOUTWEIGHT";
            this.FOutWeight.Name = "FOutWeight";
            // 
            // cab_OutWeight
            // 
            resources.ApplyResources(this.cab_OutWeight, "cab_OutWeight");
            this.cab_OutWeight.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(((DevExpress.XtraEditors.Controls.ButtonPredefines)(resources.GetObject("cab_OutWeight.Buttons"))))});
            this.cab_OutWeight.Name = "cab_OutWeight";
            // 
            // FStockDate
            // 
            resources.ApplyResources(this.FStockDate, "FStockDate");
            this.FStockDate.FieldName = "FSTOCKDATE";
            this.FStockDate.Name = "FStockDate";
            // 
            // FCheck
            // 
            resources.ApplyResources(this.FCheck, "FCheck");
            this.FCheck.ColumnEdit = this.cb_Check;
            this.FCheck.FieldName = "FCHECK";
            this.FCheck.Name = "FCheck";
            this.FCheck.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.FCheck.OptionsColumn.AllowMove = false;
            this.FCheck.OptionsColumn.AllowShowHide = false;
            this.FCheck.OptionsColumn.AllowSize = false;
            this.FCheck.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.FCheck.OptionsColumn.FixedWidth = true;
            this.FCheck.OptionsFilter.AllowAutoFilter = false;
            this.FCheck.OptionsFilter.AllowFilter = false;
            // 
            // cb_Check
            // 
            resources.ApplyResources(this.cb_Check, "cb_Check");
            this.cb_Check.Name = "cb_Check";
            this.cb_Check.ValueChecked = 1;
            this.cb_Check.ValueUnchecked = 0;
            // 
            // barManager
            // 
            this.barManager.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.mainBar});
            this.barManager.DockControls.Add(this.barDockControlTop);
            this.barManager.DockControls.Add(this.barDockControlBottom);
            this.barManager.DockControls.Add(this.barDockControlLeft);
            this.barManager.DockControls.Add(this.barDockControlRight);
            this.barManager.Form = this.splitContainerControl1.Panel1;
            this.barManager.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.bt_New,
            this.bt_Save,
            this.bt_Delete,
            this.btRefresh,
            this.btUpdate,
            this.btPrint,
            this.barListItem1,
            this.bsPrint,
            this.btPrintTemplet,
            this.barButtonItem3,
            this.btPrintPreview,
            this.btExport});
            this.barManager.MainMenu = this.mainBar;
            this.barManager.MaxItemId = 13;
            // 
            // mainBar
            // 
            this.mainBar.BarAppearance.Disabled.Font = ((System.Drawing.Font)(resources.GetObject("mainBar.BarAppearance.Disabled.Font")));
            this.mainBar.BarAppearance.Disabled.Options.UseFont = true;
            this.mainBar.BarAppearance.Hovered.Font = ((System.Drawing.Font)(resources.GetObject("mainBar.BarAppearance.Hovered.Font")));
            this.mainBar.BarAppearance.Hovered.Options.UseFont = true;
            this.mainBar.BarAppearance.Normal.Font = ((System.Drawing.Font)(resources.GetObject("mainBar.BarAppearance.Normal.Font")));
            this.mainBar.BarAppearance.Normal.Options.UseFont = true;
            this.mainBar.BarAppearance.Pressed.Font = ((System.Drawing.Font)(resources.GetObject("mainBar.BarAppearance.Pressed.Font")));
            this.mainBar.BarAppearance.Pressed.Options.UseFont = true;
            this.mainBar.BarName = "Tools";
            this.mainBar.DockCol = 0;
            this.mainBar.DockRow = 0;
            this.mainBar.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.mainBar.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.bt_Save),
            new DevExpress.XtraBars.LinkPersistInfo(this.btUpdate),
            new DevExpress.XtraBars.LinkPersistInfo(this.bsPrint),
            new DevExpress.XtraBars.LinkPersistInfo(this.btExport)});
            this.mainBar.OptionsBar.AllowQuickCustomization = false;
            resources.ApplyResources(this.mainBar, "mainBar");
            // 
            // bt_Save
            // 
            resources.ApplyResources(this.bt_Save, "bt_Save");
            this.bt_Save.Id = 1;
            this.bt_Save.Name = "bt_Save";
            // 
            // btUpdate
            // 
            resources.ApplyResources(this.btUpdate, "btUpdate");
            this.btUpdate.Id = 5;
            this.btUpdate.Name = "btUpdate";
            this.btUpdate.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.btUpdate.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btUpdate_ItemClick);
            // 
            // bsPrint
            // 
            resources.ApplyResources(this.bsPrint, "bsPrint");
            this.bsPrint.Id = 8;
            this.bsPrint.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem3),
            new DevExpress.XtraBars.LinkPersistInfo(this.btPrintPreview),
            new DevExpress.XtraBars.LinkPersistInfo(this.btPrintTemplet)});
            this.bsPrint.Name = "bsPrint";
            // 
            // barButtonItem3
            // 
            resources.ApplyResources(this.barButtonItem3, "barButtonItem3");
            this.barButtonItem3.Id = 10;
            this.barButtonItem3.ItemAppearance.Disabled.Font = ((System.Drawing.Font)(resources.GetObject("barButtonItem3.ItemAppearance.Disabled.Font")));
            this.barButtonItem3.ItemAppearance.Disabled.Options.UseFont = true;
            this.barButtonItem3.ItemInMenuAppearance.Disabled.Font = ((System.Drawing.Font)(resources.GetObject("barButtonItem3.ItemInMenuAppearance.Disabled.Font")));
            this.barButtonItem3.ItemInMenuAppearance.Disabled.Options.UseFont = true;
            this.barButtonItem3.ItemInMenuAppearance.Hovered.Font = ((System.Drawing.Font)(resources.GetObject("barButtonItem3.ItemInMenuAppearance.Hovered.Font")));
            this.barButtonItem3.ItemInMenuAppearance.Hovered.Options.UseFont = true;
            this.barButtonItem3.ItemInMenuAppearance.Normal.Font = ((System.Drawing.Font)(resources.GetObject("barButtonItem3.ItemInMenuAppearance.Normal.Font")));
            this.barButtonItem3.ItemInMenuAppearance.Normal.Options.UseFont = true;
            this.barButtonItem3.ItemInMenuAppearance.Pressed.Font = ((System.Drawing.Font)(resources.GetObject("barButtonItem3.ItemInMenuAppearance.Pressed.Font")));
            this.barButtonItem3.ItemInMenuAppearance.Pressed.Options.UseFont = true;
            this.barButtonItem3.Name = "barButtonItem3";
            this.barButtonItem3.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btPrint_ItemClick);
            // 
            // btPrintPreview
            // 
            resources.ApplyResources(this.btPrintPreview, "btPrintPreview");
            this.btPrintPreview.Id = 11;
            this.btPrintPreview.ItemInMenuAppearance.Disabled.Font = ((System.Drawing.Font)(resources.GetObject("btPrintPreview.ItemInMenuAppearance.Disabled.Font")));
            this.btPrintPreview.ItemInMenuAppearance.Disabled.Options.UseFont = true;
            this.btPrintPreview.ItemInMenuAppearance.Hovered.Font = ((System.Drawing.Font)(resources.GetObject("btPrintPreview.ItemInMenuAppearance.Hovered.Font")));
            this.btPrintPreview.ItemInMenuAppearance.Hovered.Options.UseFont = true;
            this.btPrintPreview.ItemInMenuAppearance.Normal.Font = ((System.Drawing.Font)(resources.GetObject("btPrintPreview.ItemInMenuAppearance.Normal.Font")));
            this.btPrintPreview.ItemInMenuAppearance.Normal.Options.UseFont = true;
            this.btPrintPreview.ItemInMenuAppearance.Pressed.Font = ((System.Drawing.Font)(resources.GetObject("btPrintPreview.ItemInMenuAppearance.Pressed.Font")));
            this.btPrintPreview.ItemInMenuAppearance.Pressed.Options.UseFont = true;
            this.btPrintPreview.Name = "btPrintPreview";
            this.btPrintPreview.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btPrintPreview_ItemClick);
            // 
            // btPrintTemplet
            // 
            resources.ApplyResources(this.btPrintTemplet, "btPrintTemplet");
            this.btPrintTemplet.Id = 9;
            this.btPrintTemplet.ItemAppearance.Disabled.Font = ((System.Drawing.Font)(resources.GetObject("btPrintTemplet.ItemAppearance.Disabled.Font")));
            this.btPrintTemplet.ItemAppearance.Disabled.Options.UseFont = true;
            this.btPrintTemplet.ItemInMenuAppearance.Disabled.Font = ((System.Drawing.Font)(resources.GetObject("btPrintTemplet.ItemInMenuAppearance.Disabled.Font")));
            this.btPrintTemplet.ItemInMenuAppearance.Disabled.Options.UseFont = true;
            this.btPrintTemplet.ItemInMenuAppearance.Hovered.Font = ((System.Drawing.Font)(resources.GetObject("btPrintTemplet.ItemInMenuAppearance.Hovered.Font")));
            this.btPrintTemplet.ItemInMenuAppearance.Hovered.Options.UseFont = true;
            this.btPrintTemplet.ItemInMenuAppearance.Normal.Font = ((System.Drawing.Font)(resources.GetObject("btPrintTemplet.ItemInMenuAppearance.Normal.Font")));
            this.btPrintTemplet.ItemInMenuAppearance.Normal.Options.UseFont = true;
            this.btPrintTemplet.ItemInMenuAppearance.Pressed.Font = ((System.Drawing.Font)(resources.GetObject("btPrintTemplet.ItemInMenuAppearance.Pressed.Font")));
            this.btPrintTemplet.ItemInMenuAppearance.Pressed.Options.UseFont = true;
            this.btPrintTemplet.Name = "btPrintTemplet";
            this.btPrintTemplet.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btPrintTemplet_ItemClick);
            // 
            // btExport
            // 
            resources.ApplyResources(this.btExport, "btExport");
            this.btExport.Id = 12;
            this.btExport.Name = "btExport";
            this.btExport.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btExport_ItemClick);
            // 
            // bt_New
            // 
            resources.ApplyResources(this.bt_New, "bt_New");
            this.bt_New.Id = 0;
            this.bt_New.Name = "bt_New";
            this.bt_New.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            // 
            // bt_Delete
            // 
            resources.ApplyResources(this.bt_Delete, "bt_Delete");
            this.bt_Delete.Id = 2;
            this.bt_Delete.Name = "bt_Delete";
            this.bt_Delete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            // 
            // btRefresh
            // 
            resources.ApplyResources(this.btRefresh, "btRefresh");
            this.btRefresh.Id = 3;
            this.btRefresh.Name = "btRefresh";
            this.btRefresh.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btRefresh_ItemClick);
            // 
            // btPrint
            // 
            this.btPrint.ActAsDropDown = true;
            this.btPrint.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.DropDown;
            resources.ApplyResources(this.btPrint, "btPrint");
            this.btPrint.Id = 6;
            this.btPrint.Name = "btPrint";
            this.btPrint.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btPrint_ItemClick);
            // 
            // barListItem1
            // 
            resources.ApplyResources(this.barListItem1, "barListItem1");
            this.barListItem1.Id = 7;
            this.barListItem1.Name = "barListItem1";
            // 
            // gridView15
            // 
            this.gridView15.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridView15.Name = "gridView15";
            this.gridView15.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView15.OptionsView.ShowGroupPanel = false;
            // 
            // StockAreaData
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainerControl1);
            this.Name = "StockAreaData";
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lc_Head)).EndInit();
            this.lc_Head.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.FHSpecification.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManagerEntry)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FHPaperType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView18)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FHProduct.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLookUpEdit1View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FHStockArea.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FHStock.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FBeginDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FBeginDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FHStatus.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FEndDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FEndDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_EndDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_Stock)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_Product)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_BeginDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_Status)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_HPaperType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_HSpecification)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FCheckAll.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gc_Data)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gv_Entry)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.slu_Product)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.slu_Stock)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.slu_StockArea)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.slu_StockAreaPlan)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSearchLookUpEdit1View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cab_Amount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deDate.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cab_PlanAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.slu_Status)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.slu_PaperType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_Specification)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.slu_PaperGrade)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.slu_Certification)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.slu_TransportType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_CoreDiameterOrReam)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_DiameterOrSlides)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_Length)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.slu_TrademarkStyle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.slu_SpecCustName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.slu_SpecProdName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView11)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.slu_PackType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView12)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.date_DeliverDate.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.date_DeliverDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.slu_WeightMode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView13)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.slu_Color)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView14)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.slu_SpCustomer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView16)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chk_Customer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.slu_InStockArea)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView17)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ce_OutAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cab_Weight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cab_OutWeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_Check)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView15)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private DevExpress.XtraBars.BarDockControl barDockControl3;
        private DevExpress.XtraBars.BarDockControl barDockControl4;
        private DevExpress.XtraBars.BarDockControl barDockControl2;
        private DevExpress.XtraBars.BarDockControl barDockControl1;
        private DevExpress.XtraBars.BarManager barManagerEntry;
        private DevExpress.XtraBars.Bar entryBar;
        private DevExpress.XtraBars.BarButtonItem bt_NewEntry;
        private DevExpress.XtraBars.BarButtonItem bt_DeleteEntry;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
        private DevExpress.XtraBars.BarManager barManager;
        private DevExpress.XtraBars.Bar mainBar;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarButtonItem bt_New;
        private DevExpress.XtraBars.BarButtonItem bt_Save;
        private DevExpress.XtraBars.BarButtonItem bt_Delete;
        private DevExpress.XtraBars.BarButtonItem btRefresh;
        private DevExpress.XtraEditors.DateEdit FEndDate;
        private DevExpress.XtraEditors.DateEdit FBeginDate;
        private DevExpress.XtraEditors.SimpleButton btQuery;
        private DevExpress.XtraEditors.CheckedComboBoxEdit FHStatus;
        private DevExpress.XtraLayout.LayoutControl lc_Head;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup3;
        private DevExpress.XtraLayout.LayoutControlItem lo_BeginDate;
        private DevExpress.XtraLayout.LayoutControlItem lo_EndDate;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.LayoutControlItem lo_Status;
        private DevExpress.XtraGrid.GridControl gc_Data;
        private DevExpress.XtraGrid.Views.Grid.GridView gv_Entry;
        private DevExpress.XtraGrid.Columns.GridColumn FEntryID;
        private DevExpress.XtraGrid.Columns.GridColumn FProduct;
        private DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit slu_Product;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView3;
        private DevExpress.XtraGrid.Columns.GridColumn FStock;
        private DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit slu_Stock;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn FStockArea;
        private DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit slu_StockArea;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView4;
        private DevExpress.XtraGrid.Columns.GridColumn FStockAreaPlan;
        private DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit slu_StockAreaPlan;
        private DevExpress.XtraGrid.Views.Grid.GridView repositoryItemSearchLookUpEdit1View;
        private DevExpress.XtraGrid.Columns.GridColumn FAmount;
        private DevExpress.XtraEditors.Repository.RepositoryItemCalcEdit cab_Amount;
        private DevExpress.XtraGrid.Columns.GridColumn FDate;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit deDate;
        private DevExpress.XtraGrid.Columns.GridColumn FPlanAmount;
        private DevExpress.XtraEditors.Repository.RepositoryItemCalcEdit cab_PlanAmount;
        private DevExpress.XtraGrid.Columns.GridColumn FStatus;
        private DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit slu_Status;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
        private DevExpress.XtraGrid.Columns.GridColumn FPaperType;
        private DevExpress.XtraGrid.Columns.GridColumn FSpecification;
        private DevExpress.XtraGrid.Columns.GridColumn FPaperGrade;
        private DevExpress.XtraGrid.Columns.GridColumn FCertification;
        private DevExpress.XtraGrid.Columns.GridColumn FTransportType;
        private DevExpress.XtraGrid.Columns.GridColumn FCoreDiameterOrReam;
        private DevExpress.XtraGrid.Columns.GridColumn FDiameterOrSlides;
        private DevExpress.XtraGrid.Columns.GridColumn FLength;
        private DevExpress.XtraGrid.Columns.GridColumn FTrademarkStyle;
        private DevExpress.XtraGrid.Columns.GridColumn FSpecCustName;
        private DevExpress.XtraGrid.Columns.GridColumn FSpecProdName;
        private DevExpress.XtraGrid.Columns.GridColumn FPackType;
        private DevExpress.XtraGrid.Columns.GridColumn FDeliverDate;
        private DevExpress.XtraGrid.Columns.GridColumn FWeightMode;
        private DevExpress.XtraGrid.Columns.GridColumn FColor;
        private DevExpress.XtraGrid.Columns.GridColumn FPoNumber;
        private DevExpress.XtraGrid.Columns.GridColumn FMemo;
        private DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit slu_PaperType;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView5;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox cb_Specification;
        private DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit slu_PaperGrade;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView6;
        private DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit slu_Certification;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView7;
        private DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit slu_TransportType;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView8;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox cb_CoreDiameterOrReam;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox cb_DiameterOrSlides;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox cb_Length;
        private DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit slu_TrademarkStyle;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView9;
        private DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit slu_SpecCustName;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView10;
        private DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit slu_SpecProdName;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView11;
        private DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit slu_PackType;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView12;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit date_DeliverDate;
        private DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit slu_WeightMode;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView13;
        private DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit slu_Color;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView14;
        private DevExpress.XtraGrid.Columns.GridColumn FTotalCapacity;
        private DevExpress.XtraGrid.Columns.GridColumn FUsedCapacity;
        private DevExpress.XtraGrid.Columns.GridColumn FName;
        private DevExpress.XtraGrid.Columns.GridColumn FCustomer;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView15;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit chk_Customer;
        private DevExpress.XtraGrid.Columns.GridColumn FInStockArea;
        private DevExpress.XtraGrid.Columns.GridColumn FOutAmount;
        private DevExpress.XtraEditors.Repository.RepositoryItemCalcEdit ce_OutAmount;
        private DevExpress.XtraGrid.Columns.GridColumn FWeight;
        private DevExpress.XtraGrid.Columns.GridColumn FOutWeight;
        private DevExpress.XtraEditors.Repository.RepositoryItemCalcEdit cab_Weight;
        private DevExpress.XtraEditors.Repository.RepositoryItemCalcEdit cab_OutWeight;
        private DevExpress.XtraBars.BarButtonItem btUpdate;
        private DevExpress.XtraBars.BarButtonItem btComplete;
        private DevExpress.XtraBars.BarButtonItem btPrint;
        private DevExpress.XtraBars.BarListItem barListItem1;
        private DevExpress.XtraBars.BarSubItem bsPrint;
        private DevExpress.XtraBars.BarButtonItem barButtonItem3;
        private DevExpress.XtraBars.BarButtonItem btPrintTemplet;
        private DevExpress.XtraEditors.CheckedComboBoxEdit FHStockArea;
        private DevExpress.XtraEditors.CheckedComboBoxEdit FHStock;
        private DevExpress.XtraLayout.LayoutControlItem lo_Stock;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraGrid.Columns.GridColumn FSpCustomer;
        private DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit slu_SpCustomer;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView16;
        private DevExpress.XtraGrid.Columns.GridColumn FStockDate;
        private DevExpress.XtraBars.BarButtonItem bt_CopyEntry;
        private DevExpress.XtraBars.BarButtonItem btPrintPreview;
        private DevExpress.XtraBars.BarButtonItem btExport;
        private DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit slu_InStockArea;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView17;
        private DevExpress.XtraGrid.Columns.GridColumn FCheck;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit cb_Check;
        private DevExpress.XtraEditors.CheckEdit FCheckAll;
        private DevExpress.XtraEditors.SearchLookUpEdit FHProduct;
        private DevExpress.XtraGrid.Views.Grid.GridView searchLookUpEdit1View;
        private DevExpress.XtraLayout.LayoutControlItem lo_Product;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
        private DevExpress.XtraEditors.TextEdit FHSpecification;
        private DevExpress.XtraEditors.SearchLookUpEdit FHPaperType;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView18;
        private DevExpress.XtraLayout.LayoutControlItem lo_HPaperType;
        private DevExpress.XtraLayout.LayoutControlItem lo_HSpecification;
    }
}
