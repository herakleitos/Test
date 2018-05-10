namespace Chaint.Instock.Business
{
    partial class StockArea
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StockArea));
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.tab_Base = new DevExpress.XtraTab.XtraTabPage();
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.FMemo = new DevExpress.XtraEditors.MemoExEdit();
            this.FStock = new DevExpress.XtraEditors.SearchLookUpEdit();
            this.searchLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lo_Stock = new DevExpress.XtraLayout.LayoutControlItem();
            this.lo_Memo = new DevExpress.XtraLayout.LayoutControlItem();
            this.tab_Other = new DevExpress.XtraTab.XtraTabPage();
            this.FID = new DevExpress.XtraEditors.TextEdit();
            this.barManagerEntry = new DevExpress.XtraBars.BarManager(this.components);
            this.entryBar = new DevExpress.XtraBars.Bar();
            this.bt_NewEntry = new DevExpress.XtraBars.BarButtonItem();
            this.bt_DeleteEntry = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControl1 = new DevExpress.XtraBars.BarDockControl();
            this.barDockControl2 = new DevExpress.XtraBars.BarDockControl();
            this.barDockControl3 = new DevExpress.XtraBars.BarDockControl();
            this.barDockControl4 = new DevExpress.XtraBars.BarDockControl();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.layoutControl2 = new DevExpress.XtraLayout.LayoutControl();
            this.FModifyDate = new DevExpress.XtraEditors.DateEdit();
            this.FCreateDate = new DevExpress.XtraEditors.DateEdit();
            this.layoutControlGroup2 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lo_CreateDate = new DevExpress.XtraLayout.LayoutControlItem();
            this.lo_ModifyDate = new DevExpress.XtraLayout.LayoutControlItem();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.gc_Data = new DevExpress.XtraGrid.GridControl();
            this.gv_Entry = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.FSeq = new DevExpress.XtraGrid.Columns.GridColumn();
            this.FStockAreaNumber = new DevExpress.XtraGrid.Columns.GridColumn();
            this.FStockAreaName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.FLocation = new DevExpress.XtraGrid.Columns.GridColumn();
            this.FTotalCapacity = new DevExpress.XtraGrid.Columns.GridColumn();
            this.cabTotalCapacity = new DevExpress.XtraEditors.Repository.RepositoryItemCalcEdit();
            this.FHeadID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.FEntryID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.FEMemo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.cabUsedCapacity = new DevExpress.XtraEditors.Repository.RepositoryItemCalcEdit();
            this.barManager = new DevExpress.XtraBars.BarManager(this.components);
            this.mainBar = new DevExpress.XtraBars.Bar();
            this.btNew = new DevExpress.XtraBars.BarButtonItem();
            this.btSave = new DevExpress.XtraBars.BarButtonItem();
            this.btDelete = new DevExpress.XtraBars.BarButtonItem();
            this.btList = new DevExpress.XtraBars.BarButtonItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.tab_Base.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FMemo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FStock.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLookUpEdit1View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_Stock)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_Memo)).BeginInit();
            this.tab_Other.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManagerEntry)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl2)).BeginInit();
            this.layoutControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FModifyDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FModifyDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FCreateDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FCreateDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_CreateDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_ModifyDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gc_Data)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gv_Entry)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cabTotalCapacity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cabUsedCapacity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).BeginInit();
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
            this.splitContainerControl1.Panel1.Controls.Add(this.xtraTabControl1);
            this.splitContainerControl1.Panel1.Controls.Add(this.barDockControlLeft);
            this.splitContainerControl1.Panel1.Controls.Add(this.barDockControlRight);
            this.splitContainerControl1.Panel1.Controls.Add(this.barDockControlBottom);
            this.splitContainerControl1.Panel1.Controls.Add(this.barDockControlTop);
            resources.ApplyResources(this.splitContainerControl1.Panel1, "splitContainerControl1.Panel1");
            this.splitContainerControl1.Panel2.Controls.Add(this.gc_Data);
            this.splitContainerControl1.Panel2.Controls.Add(this.barDockControl3);
            this.splitContainerControl1.Panel2.Controls.Add(this.barDockControl4);
            this.splitContainerControl1.Panel2.Controls.Add(this.barDockControl2);
            this.splitContainerControl1.Panel2.Controls.Add(this.barDockControl1);
            resources.ApplyResources(this.splitContainerControl1.Panel2, "splitContainerControl1.Panel2");
            this.splitContainerControl1.SplitterPosition = 173;
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Appearance.BackColor = ((System.Drawing.Color)(resources.GetObject("xtraTabControl1.Appearance.BackColor")));
            this.xtraTabControl1.Appearance.Options.UseBackColor = true;
            resources.ApplyResources(this.xtraTabControl1, "xtraTabControl1");
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.tab_Base;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.tab_Base,
            this.tab_Other});
            // 
            // tab_Base
            // 
            this.tab_Base.Appearance.PageClient.BackColor = ((System.Drawing.Color)(resources.GetObject("tab_Base.Appearance.PageClient.BackColor")));
            this.tab_Base.Appearance.PageClient.Options.UseBackColor = true;
            this.tab_Base.Controls.Add(this.layoutControl1);
            this.tab_Base.Name = "tab_Base";
            resources.ApplyResources(this.tab_Base, "tab_Base");
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.FMemo);
            this.layoutControl1.Controls.Add(this.FStock);
            resources.ApplyResources(this.layoutControl1, "layoutControl1");
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.layoutControlGroup1;
            // 
            // FMemo
            // 
            resources.ApplyResources(this.FMemo, "FMemo");
            this.FMemo.Name = "FMemo";
            this.FMemo.Properties.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("FMemo.Properties.Appearance.Font")));
            this.FMemo.Properties.Appearance.Options.UseFont = true;
            this.FMemo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(((DevExpress.XtraEditors.Controls.ButtonPredefines)(resources.GetObject("FMemo.Properties.Buttons"))))});
            this.FMemo.StyleController = this.layoutControl1;
            // 
            // FStock
            // 
            resources.ApplyResources(this.FStock, "FStock");
            this.FStock.Name = "FStock";
            this.FStock.Properties.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("FStock.Properties.Appearance.Font")));
            this.FStock.Properties.Appearance.Options.UseFont = true;
            this.FStock.Properties.AppearanceDropDown.Font = ((System.Drawing.Font)(resources.GetObject("FStock.Properties.AppearanceDropDown.Font")));
            this.FStock.Properties.AppearanceDropDown.Options.UseFont = true;
            this.FStock.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(((DevExpress.XtraEditors.Controls.ButtonPredefines)(resources.GetObject("FStock.Properties.Buttons"))))});
            this.FStock.Properties.NullText = resources.GetString("FStock.Properties.NullText");
            this.FStock.Properties.PopupSizeable = false;
            this.FStock.Properties.View = this.searchLookUpEdit1View;
            this.FStock.StyleController = this.layoutControl1;
            // 
            // searchLookUpEdit1View
            // 
            this.searchLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.searchLookUpEdit1View.Name = "searchLookUpEdit1View";
            this.searchLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.searchLookUpEdit1View.OptionsView.ShowGroupPanel = false;
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lo_Stock,
            this.lo_Memo});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "Root";
            this.layoutControlGroup1.OptionsItemText.TextToControlDistance = 5;
            this.layoutControlGroup1.Size = new System.Drawing.Size(750, 93);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // lo_Stock
            // 
            this.lo_Stock.Control = this.FStock;
            this.lo_Stock.Location = new System.Drawing.Point(0, 0);
            this.lo_Stock.Name = "lo_Stock";
            this.lo_Stock.Size = new System.Drawing.Size(355, 63);
            resources.ApplyResources(this.lo_Stock, "lo_Stock");
            this.lo_Stock.TextSize = new System.Drawing.Size(36, 22);
            // 
            // lo_Memo
            // 
            this.lo_Memo.Control = this.FMemo;
            resources.ApplyResources(this.lo_Memo, "lo_Memo");
            this.lo_Memo.Location = new System.Drawing.Point(355, 0);
            this.lo_Memo.Name = "lo_Memo";
            this.lo_Memo.Size = new System.Drawing.Size(365, 63);
            this.lo_Memo.TextSize = new System.Drawing.Size(36, 22);
            // 
            // tab_Other
            // 
            this.tab_Other.Appearance.PageClient.BackColor = ((System.Drawing.Color)(resources.GetObject("tab_Other.Appearance.PageClient.BackColor")));
            this.tab_Other.Appearance.PageClient.Options.UseBackColor = true;
            this.tab_Other.Controls.Add(this.FID);
            this.tab_Other.Controls.Add(this.layoutControl2);
            this.tab_Other.Name = "tab_Other";
            resources.ApplyResources(this.tab_Other, "tab_Other");
            // 
            // FID
            // 
            resources.ApplyResources(this.FID, "FID");
            this.FID.MenuManager = this.barManagerEntry;
            this.FID.Name = "FID";
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
            this.barButtonItem2});
            this.barManagerEntry.MainMenu = this.entryBar;
            this.barManagerEntry.MaxItemId = 3;
            // 
            // entryBar
            // 
            this.entryBar.BarAppearance.Disabled.Font = ((System.Drawing.Font)(resources.GetObject("entryBar.BarAppearance.Disabled.Font")));
            this.entryBar.BarAppearance.Disabled.Options.UseFont = true;
            this.entryBar.BarAppearance.Hovered.Font = ((System.Drawing.Font)(resources.GetObject("entryBar.BarAppearance.Hovered.Font")));
            this.entryBar.BarAppearance.Hovered.Options.UseFont = true;
            this.entryBar.BarAppearance.Normal.Font = ((System.Drawing.Font)(resources.GetObject("entryBar.BarAppearance.Normal.Font")));
            this.entryBar.BarAppearance.Normal.Options.UseFont = true;
            this.entryBar.BarAppearance.Pressed.Font = ((System.Drawing.Font)(resources.GetObject("entryBar.BarAppearance.Pressed.Font")));
            this.entryBar.BarAppearance.Pressed.Options.UseFont = true;
            this.entryBar.BarName = "表体菜单";
            this.entryBar.DockCol = 0;
            this.entryBar.DockRow = 0;
            this.entryBar.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.entryBar.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.bt_NewEntry),
            new DevExpress.XtraBars.LinkPersistInfo(this.bt_DeleteEntry)});
            this.entryBar.OptionsBar.AllowQuickCustomization = false;
            resources.ApplyResources(this.entryBar, "entryBar");
            // 
            // bt_NewEntry
            // 
            resources.ApplyResources(this.bt_NewEntry, "bt_NewEntry");
            this.bt_NewEntry.Id = 0;
            this.bt_NewEntry.Name = "bt_NewEntry";
            // 
            // bt_DeleteEntry
            // 
            resources.ApplyResources(this.bt_DeleteEntry, "bt_DeleteEntry");
            this.bt_DeleteEntry.Id = 1;
            this.bt_DeleteEntry.Name = "bt_DeleteEntry";
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
            // layoutControl2
            // 
            this.layoutControl2.Controls.Add(this.FModifyDate);
            this.layoutControl2.Controls.Add(this.FCreateDate);
            resources.ApplyResources(this.layoutControl2, "layoutControl2");
            this.layoutControl2.Name = "layoutControl2";
            this.layoutControl2.Root = this.layoutControlGroup2;
            // 
            // FModifyDate
            // 
            resources.ApplyResources(this.FModifyDate, "FModifyDate");
            this.FModifyDate.Name = "FModifyDate";
            this.FModifyDate.Properties.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("FModifyDate.Properties.Appearance.Font")));
            this.FModifyDate.Properties.Appearance.Options.UseFont = true;
            this.FModifyDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(((DevExpress.XtraEditors.Controls.ButtonPredefines)(resources.GetObject("FModifyDate.Properties.Buttons"))))});
            this.FModifyDate.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(((DevExpress.XtraEditors.Controls.ButtonPredefines)(resources.GetObject("FModifyDate.Properties.CalendarTimeProperties.Buttons"))))});
            this.FModifyDate.StyleController = this.layoutControl2;
            // 
            // FCreateDate
            // 
            resources.ApplyResources(this.FCreateDate, "FCreateDate");
            this.FCreateDate.Name = "FCreateDate";
            this.FCreateDate.Properties.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("FCreateDate.Properties.Appearance.Font")));
            this.FCreateDate.Properties.Appearance.Options.UseFont = true;
            this.FCreateDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(((DevExpress.XtraEditors.Controls.ButtonPredefines)(resources.GetObject("FCreateDate.Properties.Buttons"))))});
            this.FCreateDate.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(((DevExpress.XtraEditors.Controls.ButtonPredefines)(resources.GetObject("FCreateDate.Properties.CalendarTimeProperties.Buttons"))))});
            this.FCreateDate.StyleController = this.layoutControl2;
            // 
            // layoutControlGroup2
            // 
            this.layoutControlGroup2.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup2.GroupBordersVisible = false;
            this.layoutControlGroup2.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lo_CreateDate,
            this.lo_ModifyDate});
            this.layoutControlGroup2.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup2.Name = "layoutControlGroup2";
            this.layoutControlGroup2.OptionsItemText.TextToControlDistance = 5;
            this.layoutControlGroup2.Size = new System.Drawing.Size(750, 143);
            this.layoutControlGroup2.TextVisible = false;
            // 
            // lo_CreateDate
            // 
            this.lo_CreateDate.Control = this.FCreateDate;
            this.lo_CreateDate.Location = new System.Drawing.Point(0, 0);
            this.lo_CreateDate.Name = "lo_CreateDate";
            this.lo_CreateDate.Size = new System.Drawing.Size(356, 113);
            resources.ApplyResources(this.lo_CreateDate, "lo_CreateDate");
            this.lo_CreateDate.TextSize = new System.Drawing.Size(72, 22);
            // 
            // lo_ModifyDate
            // 
            this.lo_ModifyDate.Control = this.FModifyDate;
            this.lo_ModifyDate.Location = new System.Drawing.Point(356, 0);
            this.lo_ModifyDate.Name = "lo_ModifyDate";
            this.lo_ModifyDate.Size = new System.Drawing.Size(364, 113);
            resources.ApplyResources(this.lo_ModifyDate, "lo_ModifyDate");
            this.lo_ModifyDate.TextSize = new System.Drawing.Size(72, 22);
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
            // gc_Data
            // 
            resources.ApplyResources(this.gc_Data, "gc_Data");
            this.gc_Data.MainView = this.gv_Entry;
            this.gc_Data.Name = "gc_Data";
            this.gc_Data.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.cabTotalCapacity,
            this.cabUsedCapacity});
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
            this.FSeq,
            this.FStockAreaNumber,
            this.FStockAreaName,
            this.FLocation,
            this.FTotalCapacity,
            this.FHeadID,
            this.FEntryID,
            this.FEMemo});
            this.gv_Entry.GridControl = this.gc_Data;
            this.gv_Entry.Name = "gv_Entry";
            this.gv_Entry.OptionsView.ShowGroupPanel = false;
            // 
            // FSeq
            // 
            this.FSeq.AppearanceCell.Font = ((System.Drawing.Font)(resources.GetObject("FSeq.AppearanceCell.Font")));
            this.FSeq.AppearanceCell.Options.UseFont = true;
            resources.ApplyResources(this.FSeq, "FSeq");
            this.FSeq.FieldName = "FSEQ";
            this.FSeq.Name = "FSeq";
            this.FSeq.OptionsColumn.AllowEdit = false;
            // 
            // FStockAreaNumber
            // 
            this.FStockAreaNumber.AppearanceCell.Font = ((System.Drawing.Font)(resources.GetObject("FStockAreaNumber.AppearanceCell.Font")));
            this.FStockAreaNumber.AppearanceCell.Options.UseFont = true;
            resources.ApplyResources(this.FStockAreaNumber, "FStockAreaNumber");
            this.FStockAreaNumber.FieldName = "FSTOCKAREANUMBER";
            this.FStockAreaNumber.Name = "FStockAreaNumber";
            // 
            // FStockAreaName
            // 
            this.FStockAreaName.AppearanceCell.Font = ((System.Drawing.Font)(resources.GetObject("FStockAreaName.AppearanceCell.Font")));
            this.FStockAreaName.AppearanceCell.Options.UseFont = true;
            resources.ApplyResources(this.FStockAreaName, "FStockAreaName");
            this.FStockAreaName.FieldName = "FSTOCKAREANAME";
            this.FStockAreaName.Name = "FStockAreaName";
            // 
            // FLocation
            // 
            this.FLocation.AppearanceCell.Font = ((System.Drawing.Font)(resources.GetObject("FLocation.AppearanceCell.Font")));
            this.FLocation.AppearanceCell.Options.UseFont = true;
            resources.ApplyResources(this.FLocation, "FLocation");
            this.FLocation.FieldName = "FLOCATION";
            this.FLocation.Name = "FLocation";
            // 
            // FTotalCapacity
            // 
            this.FTotalCapacity.AppearanceCell.Font = ((System.Drawing.Font)(resources.GetObject("FTotalCapacity.AppearanceCell.Font")));
            this.FTotalCapacity.AppearanceCell.Options.UseFont = true;
            resources.ApplyResources(this.FTotalCapacity, "FTotalCapacity");
            this.FTotalCapacity.ColumnEdit = this.cabTotalCapacity;
            this.FTotalCapacity.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.FTotalCapacity.FieldName = "FTOTALCAPACITY";
            this.FTotalCapacity.Name = "FTotalCapacity";
            // 
            // cabTotalCapacity
            // 
            resources.ApplyResources(this.cabTotalCapacity, "cabTotalCapacity");
            this.cabTotalCapacity.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(((DevExpress.XtraEditors.Controls.ButtonPredefines)(resources.GetObject("cabTotalCapacity.Buttons"))))});
            this.cabTotalCapacity.Name = "cabTotalCapacity";
            // 
            // FHeadID
            // 
            this.FHeadID.AppearanceCell.Font = ((System.Drawing.Font)(resources.GetObject("FHeadID.AppearanceCell.Font")));
            this.FHeadID.AppearanceCell.Options.UseFont = true;
            resources.ApplyResources(this.FHeadID, "FHeadID");
            this.FHeadID.FieldName = "FHEADID";
            this.FHeadID.Name = "FHeadID";
            // 
            // FEntryID
            // 
            this.FEntryID.AppearanceCell.Font = ((System.Drawing.Font)(resources.GetObject("FEntryID.AppearanceCell.Font")));
            this.FEntryID.AppearanceCell.Options.UseFont = true;
            resources.ApplyResources(this.FEntryID, "FEntryID");
            this.FEntryID.FieldName = "FENTRYID";
            this.FEntryID.Name = "FEntryID";
            // 
            // FEMemo
            // 
            resources.ApplyResources(this.FEMemo, "FEMemo");
            this.FEMemo.FieldName = "FMEMO";
            this.FEMemo.Name = "FEMemo";
            // 
            // cabUsedCapacity
            // 
            resources.ApplyResources(this.cabUsedCapacity, "cabUsedCapacity");
            this.cabUsedCapacity.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(((DevExpress.XtraEditors.Controls.ButtonPredefines)(resources.GetObject("cabUsedCapacity.Buttons"))))});
            this.cabUsedCapacity.Name = "cabUsedCapacity";
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
            this.btNew,
            this.btSave,
            this.btDelete,
            this.btList});
            this.barManager.MainMenu = this.mainBar;
            this.barManager.MaxItemId = 4;
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
            new DevExpress.XtraBars.LinkPersistInfo(this.btNew),
            new DevExpress.XtraBars.LinkPersistInfo(this.btSave),
            new DevExpress.XtraBars.LinkPersistInfo(this.btDelete),
            new DevExpress.XtraBars.LinkPersistInfo(this.btList)});
            this.mainBar.OptionsBar.AllowQuickCustomization = false;
            resources.ApplyResources(this.mainBar, "mainBar");
            // 
            // btNew
            // 
            resources.ApplyResources(this.btNew, "btNew");
            this.btNew.Id = 0;
            this.btNew.Name = "btNew";
            this.btNew.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btNew_ItemClick);
            // 
            // btSave
            // 
            resources.ApplyResources(this.btSave, "btSave");
            this.btSave.Id = 1;
            this.btSave.Name = "btSave";
            // 
            // btDelete
            // 
            resources.ApplyResources(this.btDelete, "btDelete");
            this.btDelete.Id = 2;
            this.btDelete.Name = "btDelete";
            this.btDelete.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btDelete_ItemClick);
            // 
            // btList
            // 
            resources.ApplyResources(this.btList, "btList");
            this.btList.Id = 3;
            this.btList.Name = "btList";
            this.btList.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btList_ItemClick);
            // 
            // StockArea
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainerControl1);
            this.Name = "StockArea";
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.tab_Base.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.FMemo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FStock.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLookUpEdit1View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_Stock)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_Memo)).EndInit();
            this.tab_Other.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.FID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManagerEntry)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl2)).EndInit();
            this.layoutControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.FModifyDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FModifyDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FCreateDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FCreateDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_CreateDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_ModifyDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gc_Data)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gv_Entry)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cabTotalCapacity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cabUsedCapacity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.LayoutControlItem lo_Stock;
        private DevExpress.XtraGrid.GridControl gc_Data;
        private DevExpress.XtraGrid.Views.Grid.GridView gv_Entry;
        private DevExpress.XtraGrid.Columns.GridColumn FStockAreaName;
        private DevExpress.XtraGrid.Columns.GridColumn FTotalCapacity;
        private DevExpress.XtraEditors.MemoExEdit FMemo;
        private DevExpress.XtraLayout.LayoutControlItem lo_Memo;
        private DevExpress.XtraEditors.DateEdit FModifyDate;
        private DevExpress.XtraEditors.DateEdit FCreateDate;
        private DevExpress.XtraLayout.LayoutControl layoutControl2;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup2;
        private DevExpress.XtraLayout.LayoutControlItem lo_CreateDate;
        private DevExpress.XtraLayout.LayoutControlItem lo_ModifyDate;
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
        private DevExpress.XtraBars.BarButtonItem btNew;
        private DevExpress.XtraBars.BarButtonItem btSave;
        private DevExpress.XtraBars.BarButtonItem btDelete;
        private DevExpress.XtraGrid.Columns.GridColumn FStockAreaNumber;
        private DevExpress.XtraGrid.Columns.GridColumn FHeadID;
        private DevExpress.XtraGrid.Columns.GridColumn FSeq;
        private DevExpress.XtraGrid.Columns.GridColumn FEntryID;
        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage tab_Base;
        private DevExpress.XtraTab.XtraTabPage tab_Other;
        private DevExpress.XtraEditors.TextEdit FID;
        private DevExpress.XtraGrid.Columns.GridColumn FLocation;
        private DevExpress.XtraEditors.Repository.RepositoryItemCalcEdit cabTotalCapacity;
        private DevExpress.XtraBars.BarButtonItem btList;
        private DevExpress.XtraEditors.SearchLookUpEdit FStock;
        private DevExpress.XtraGrid.Views.Grid.GridView searchLookUpEdit1View;
        private DevExpress.XtraEditors.Repository.RepositoryItemCalcEdit cabUsedCapacity;
        private DevExpress.XtraGrid.Columns.GridColumn FEMemo;
    }
}
