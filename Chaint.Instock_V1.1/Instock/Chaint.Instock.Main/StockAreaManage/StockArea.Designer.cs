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
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.FStock = new DevExpress.XtraEditors.LookUpEdit();
            this.barManager = new DevExpress.XtraBars.BarManager(this.components);
            this.mainBar = new DevExpress.XtraBars.Bar();
            this.bt_New = new DevExpress.XtraBars.BarButtonItem();
            this.bt_Save = new DevExpress.XtraBars.BarButtonItem();
            this.bt_Delete = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lo_Stock = new DevExpress.XtraLayout.LayoutControlItem();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.FStockAreaNumber = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemTextEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.FStockAreaName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.FStockAreaPlan = new DevExpress.XtraGrid.Columns.GridColumn();
            this.FTotalCapacity = new DevExpress.XtraGrid.Columns.GridColumn();
            this.FLeftCapacity = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemSearchLookUpEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit();
            this.repositoryItemSearchLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.repositoryItemTextEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.repositoryItemCalcEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCalcEdit();
            this.repositoryItemCalcEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemCalcEdit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FStock.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_Stock)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSearchLookUpEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSearchLookUpEdit1View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCalcEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCalcEdit2)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainerControl1
            // 
            resources.ApplyResources(this.splitContainerControl1, "splitContainerControl1");
            this.splitContainerControl1.Horizontal = false;
            this.splitContainerControl1.Name = "splitContainerControl1";
            this.splitContainerControl1.Panel1.Controls.Add(this.panelControl1);
            resources.ApplyResources(this.splitContainerControl1.Panel1, "splitContainerControl1.Panel1");
            this.splitContainerControl1.Panel2.Controls.Add(this.gridControl1);
            resources.ApplyResources(this.splitContainerControl1.Panel2, "splitContainerControl1.Panel2");
            this.splitContainerControl1.SplitterPosition = 91;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.layoutControl1);
            resources.ApplyResources(this.panelControl1, "panelControl1");
            this.panelControl1.Name = "panelControl1";
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.FStock);
            resources.ApplyResources(this.layoutControl1, "layoutControl1");
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.layoutControlGroup1;
            // 
            // FStock
            // 
            resources.ApplyResources(this.FStock, "FStock");
            this.FStock.MenuManager = this.barManager;
            this.FStock.Name = "FStock";
            this.FStock.Properties.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("FStock.Properties.Appearance.Font")));
            this.FStock.Properties.Appearance.Options.UseFont = true;
            this.FStock.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(((DevExpress.XtraEditors.Controls.ButtonPredefines)(resources.GetObject("FStock.Properties.Buttons"))))});
            this.FStock.StyleController = this.layoutControl1;
            // 
            // barManager
            // 
            this.barManager.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.mainBar});
            this.barManager.DockControls.Add(this.barDockControlTop);
            this.barManager.DockControls.Add(this.barDockControlBottom);
            this.barManager.DockControls.Add(this.barDockControlLeft);
            this.barManager.DockControls.Add(this.barDockControlRight);
            this.barManager.Form = this;
            this.barManager.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.bt_New,
            this.bt_Save,
            this.bt_Delete});
            this.barManager.MaxItemId = 8;
            // 
            // mainBar
            // 
            this.mainBar.BarName = "菜单";
            this.mainBar.DockCol = 0;
            this.mainBar.DockRow = 0;
            this.mainBar.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.mainBar.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.bt_New, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.bt_Save, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.bt_Delete, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.mainBar.OptionsBar.AllowQuickCustomization = false;
            resources.ApplyResources(this.mainBar, "mainBar");
            // 
            // bt_New
            // 
            resources.ApplyResources(this.bt_New, "bt_New");
            this.bt_New.Id = 5;
            this.bt_New.ImageUri.Uri = "Add";
            this.bt_New.Name = "bt_New";
            // 
            // bt_Save
            // 
            resources.ApplyResources(this.bt_Save, "bt_Save");
            this.bt_Save.Id = 6;
            this.bt_Save.ImageUri.Uri = "Save";
            this.bt_Save.Name = "bt_Save";
            // 
            // bt_Delete
            // 
            resources.ApplyResources(this.bt_Delete, "bt_Delete");
            this.bt_Delete.Id = 7;
            this.bt_Delete.ImageUri.Uri = "Delete";
            this.bt_Delete.Name = "bt_Delete";
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
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lo_Stock});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.OptionsItemText.TextToControlDistance = 5;
            this.layoutControlGroup1.Size = new System.Drawing.Size(375, 85);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // lo_Stock
            // 
            this.lo_Stock.Control = this.FStock;
            this.lo_Stock.Location = new System.Drawing.Point(0, 0);
            this.lo_Stock.Name = "lo_Stock";
            this.lo_Stock.Size = new System.Drawing.Size(345, 55);
            resources.ApplyResources(this.lo_Stock, "lo_Stock");
            this.lo_Stock.TextSize = new System.Drawing.Size(36, 22);
            // 
            // gridControl1
            // 
            resources.ApplyResources(this.gridControl1, "gridControl1");
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.MenuManager = this.barManager;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemTextEdit1,
            this.repositoryItemSearchLookUpEdit1,
            this.repositoryItemTextEdit2,
            this.repositoryItemCalcEdit1,
            this.repositoryItemCalcEdit2});
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.FStockAreaNumber,
            this.FStockAreaName,
            this.FStockAreaPlan,
            this.FTotalCapacity,
            this.FLeftCapacity});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            // 
            // FStockAreaNumber
            // 
            this.FStockAreaNumber.AppearanceCell.Font = ((System.Drawing.Font)(resources.GetObject("FStockAreaNumber.AppearanceCell.Font")));
            this.FStockAreaNumber.AppearanceCell.Options.UseFont = true;
            resources.ApplyResources(this.FStockAreaNumber, "FStockAreaNumber");
            this.FStockAreaNumber.ColumnEdit = this.repositoryItemTextEdit1;
            this.FStockAreaNumber.Name = "FStockAreaNumber";
            // 
            // repositoryItemTextEdit1
            // 
            resources.ApplyResources(this.repositoryItemTextEdit1, "repositoryItemTextEdit1");
            this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
            // 
            // FStockAreaName
            // 
            this.FStockAreaName.AppearanceCell.Font = ((System.Drawing.Font)(resources.GetObject("FStockAreaName.AppearanceCell.Font")));
            this.FStockAreaName.AppearanceCell.Options.UseFont = true;
            resources.ApplyResources(this.FStockAreaName, "FStockAreaName");
            this.FStockAreaName.ColumnEdit = this.repositoryItemTextEdit2;
            this.FStockAreaName.Name = "FStockAreaName";
            // 
            // FStockAreaPlan
            // 
            this.FStockAreaPlan.AppearanceCell.Font = ((System.Drawing.Font)(resources.GetObject("FStockAreaPlan.AppearanceCell.Font")));
            this.FStockAreaPlan.AppearanceCell.Options.UseFont = true;
            resources.ApplyResources(this.FStockAreaPlan, "FStockAreaPlan");
            this.FStockAreaPlan.ColumnEdit = this.repositoryItemSearchLookUpEdit1;
            this.FStockAreaPlan.Name = "FStockAreaPlan";
            // 
            // FTotalCapacity
            // 
            this.FTotalCapacity.AppearanceCell.Font = ((System.Drawing.Font)(resources.GetObject("FTotalCapacity.AppearanceCell.Font")));
            this.FTotalCapacity.AppearanceCell.Options.UseFont = true;
            resources.ApplyResources(this.FTotalCapacity, "FTotalCapacity");
            this.FTotalCapacity.ColumnEdit = this.repositoryItemCalcEdit1;
            this.FTotalCapacity.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.FTotalCapacity.Name = "FTotalCapacity";
            // 
            // FLeftCapacity
            // 
            this.FLeftCapacity.AppearanceCell.Font = ((System.Drawing.Font)(resources.GetObject("FLeftCapacity.AppearanceCell.Font")));
            this.FLeftCapacity.AppearanceCell.Options.UseFont = true;
            resources.ApplyResources(this.FLeftCapacity, "FLeftCapacity");
            this.FLeftCapacity.ColumnEdit = this.repositoryItemCalcEdit2;
            this.FLeftCapacity.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.FLeftCapacity.Name = "FLeftCapacity";
            // 
            // repositoryItemSearchLookUpEdit1
            // 
            resources.ApplyResources(this.repositoryItemSearchLookUpEdit1, "repositoryItemSearchLookUpEdit1");
            this.repositoryItemSearchLookUpEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(((DevExpress.XtraEditors.Controls.ButtonPredefines)(resources.GetObject("repositoryItemSearchLookUpEdit1.Buttons"))))});
            this.repositoryItemSearchLookUpEdit1.Name = "repositoryItemSearchLookUpEdit1";
            this.repositoryItemSearchLookUpEdit1.View = this.repositoryItemSearchLookUpEdit1View;
            // 
            // repositoryItemSearchLookUpEdit1View
            // 
            this.repositoryItemSearchLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.repositoryItemSearchLookUpEdit1View.Name = "repositoryItemSearchLookUpEdit1View";
            this.repositoryItemSearchLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.repositoryItemSearchLookUpEdit1View.OptionsView.ShowGroupPanel = false;
            // 
            // repositoryItemTextEdit2
            // 
            resources.ApplyResources(this.repositoryItemTextEdit2, "repositoryItemTextEdit2");
            this.repositoryItemTextEdit2.Name = "repositoryItemTextEdit2";
            // 
            // repositoryItemCalcEdit1
            // 
            resources.ApplyResources(this.repositoryItemCalcEdit1, "repositoryItemCalcEdit1");
            this.repositoryItemCalcEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(((DevExpress.XtraEditors.Controls.ButtonPredefines)(resources.GetObject("repositoryItemCalcEdit1.Buttons"))))});
            this.repositoryItemCalcEdit1.Name = "repositoryItemCalcEdit1";
            // 
            // repositoryItemCalcEdit2
            // 
            resources.ApplyResources(this.repositoryItemCalcEdit2, "repositoryItemCalcEdit2");
            this.repositoryItemCalcEdit2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(((DevExpress.XtraEditors.Controls.ButtonPredefines)(resources.GetObject("repositoryItemCalcEdit2.Buttons"))))});
            this.repositoryItemCalcEdit2.Name = "repositoryItemCalcEdit2";
            // 
            // StockArea
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainerControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "StockArea";
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.FStock.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_Stock)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSearchLookUpEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSearchLookUpEdit1View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCalcEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCalcEdit2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
        private DevExpress.XtraBars.BarManager barManager;
        private DevExpress.XtraBars.Bar mainBar;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarButtonItem bt_New;
        private DevExpress.XtraBars.BarButtonItem bt_Save;
        private DevExpress.XtraBars.BarButtonItem bt_Delete;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraEditors.LookUpEdit FStock;
        private DevExpress.XtraLayout.LayoutControlItem lo_Stock;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn FStockAreaNumber;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit1;
        private DevExpress.XtraGrid.Columns.GridColumn FStockAreaName;
        private DevExpress.XtraGrid.Columns.GridColumn FStockAreaPlan;
        private DevExpress.XtraGrid.Columns.GridColumn FTotalCapacity;
        private DevExpress.XtraGrid.Columns.GridColumn FLeftCapacity;
        private DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit repositoryItemSearchLookUpEdit1;
        private DevExpress.XtraGrid.Views.Grid.GridView repositoryItemSearchLookUpEdit1View;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit2;
        private DevExpress.XtraEditors.Repository.RepositoryItemCalcEdit repositoryItemCalcEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemCalcEdit repositoryItemCalcEdit2;
    }
}
