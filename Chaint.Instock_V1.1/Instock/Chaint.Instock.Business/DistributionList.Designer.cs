using System;

namespace Chaint.Instock.Business
{
    partial class DistributionList
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
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar2 = new DevExpress.XtraBars.Bar();
            this.btSave = new DevExpress.XtraBars.BarButtonItem();
            this.btConfirm = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.FHSpecification = new DevExpress.XtraEditors.TextEdit();
            this.FHStockArea = new DevExpress.XtraEditors.CheckedComboBoxEdit();
            this.FHStock = new DevExpress.XtraEditors.CheckedComboBoxEdit();
            this.FHProduct = new DevExpress.XtraEditors.SearchLookUpEdit();
            this.searchLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.FOper = new DevExpress.XtraEditors.TextEdit();
            this.FIsConfirm = new DevExpress.XtraEditors.CheckEdit();
            this.btQuery = new DevExpress.XtraEditors.SimpleButton();
            this.FEndDate = new DevExpress.XtraEditors.DateEdit();
            this.FBeginDate = new DevExpress.XtraEditors.DateEdit();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.lo_Specification = new DevExpress.XtraLayout.LayoutControlItem();
            this.lo_StockArea = new DevExpress.XtraLayout.LayoutControlItem();
            this.lo_IsConfirm = new DevExpress.XtraLayout.LayoutControlItem();
            this.lo_Product = new DevExpress.XtraLayout.LayoutControlItem();
            this.lo_Stock = new DevExpress.XtraLayout.LayoutControlItem();
            this.lo_Oper = new DevExpress.XtraLayout.LayoutControlItem();
            this.lo_BeginDate = new DevExpress.XtraLayout.LayoutControlItem();
            this.lo_EndDate = new DevExpress.XtraLayout.LayoutControlItem();
            this.FAllCheck = new DevExpress.XtraEditors.CheckEdit();
            this.gc_Data = new DevExpress.XtraGrid.GridControl();
            this.gv_View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.FCheck = new DevExpress.XtraGrid.Columns.GridColumn();
            this.cb_Check = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.FID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.FOperator = new DevExpress.XtraGrid.Columns.GridColumn();
            this.FBarCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.FProductName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.FStockAreaPlan = new DevExpress.XtraGrid.Columns.GridColumn();
            this.slu_StockAreaPlan = new DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit();
            this.gridView3 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.FStock = new DevExpress.XtraGrid.Columns.GridColumn();
            this.FStockArea = new DevExpress.XtraGrid.Columns.GridColumn();
            this.FAmount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.cab_Amount = new DevExpress.XtraEditors.Repository.RepositoryItemCalcEdit();
            this.FStatusName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.slu_Status = new DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit();
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.FDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.FConfirmDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.deConfirmDate = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.FPaperTypeName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.FPaperType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.FSpecification = new DevExpress.XtraGrid.Columns.GridColumn();
            this.FPaperGrade = new DevExpress.XtraGrid.Columns.GridColumn();
            this.FCertification = new DevExpress.XtraGrid.Columns.GridColumn();
            this.FTransportType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.FCoreDiameterOrReam = new DevExpress.XtraGrid.Columns.GridColumn();
            this.FDiameterOrSlides = new DevExpress.XtraGrid.Columns.GridColumn();
            this.FLength = new DevExpress.XtraGrid.Columns.GridColumn();
            this.FTrademarkStyle = new DevExpress.XtraGrid.Columns.GridColumn();
            this.FSpecCustName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.FSpecProdName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.FPackType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.FDeliverDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.FWeightMode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.FColor = new DevExpress.XtraGrid.Columns.GridColumn();
            this.FPoNumber = new DevExpress.XtraGrid.Columns.GridColumn();
            this.FSpCustomer = new DevExpress.XtraGrid.Columns.GridColumn();
            this.FMemo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.FCustomer = new DevExpress.XtraGrid.Columns.GridColumn();
            this.FStockDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.FWeight = new DevExpress.XtraGrid.Columns.GridColumn();
            this.FProduct = new DevExpress.XtraGrid.Columns.GridColumn();
            this.FStatus = new DevExpress.XtraGrid.Columns.GridColumn();
            this.FProductInfo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.FStockName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.FStockAreaName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.FPlanName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.barDockControl3 = new DevExpress.XtraBars.BarDockControl();
            this.barDockControl4 = new DevExpress.XtraBars.BarDockControl();
            this.barDockControl2 = new DevExpress.XtraBars.BarDockControl();
            this.barDockControl1 = new DevExpress.XtraBars.BarDockControl();
            this.repositoryItemSearchLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridView4 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.barManager2 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar3 = new DevExpress.XtraBars.Bar();
            this.btBatchFill = new DevExpress.XtraBars.BarButtonItem();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FHSpecification.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FHStockArea.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FHStock.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FHProduct.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLookUpEdit1View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FOper.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FIsConfirm.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FEndDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FEndDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FBeginDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FBeginDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_Specification)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_StockArea)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_IsConfirm)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_Product)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_Stock)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_Oper)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_BeginDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_EndDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FAllCheck.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gc_Data)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gv_View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_Check)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.slu_StockAreaPlan)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cab_Amount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.slu_Status)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deConfirmDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deConfirmDate.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSearchLookUpEdit1View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager2)).BeginInit();
            this.SuspendLayout();
            // 
            // barManager1
            // 
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar2});
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this.splitContainerControl1.Panel1;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.btConfirm,
            this.btSave});
            this.barManager1.MainMenu = this.bar2;
            this.barManager1.MaxItemId = 2;
            // 
            // bar2
            // 
            this.bar2.BarAppearance.Disabled.Font = new System.Drawing.Font("Tahoma", 16F);
            this.bar2.BarAppearance.Disabled.Options.UseFont = true;
            this.bar2.BarAppearance.Hovered.Font = new System.Drawing.Font("Tahoma", 16F);
            this.bar2.BarAppearance.Hovered.Options.UseFont = true;
            this.bar2.BarAppearance.Normal.Font = new System.Drawing.Font("Tahoma", 16F);
            this.bar2.BarAppearance.Normal.Options.UseFont = true;
            this.bar2.BarAppearance.Pressed.Font = new System.Drawing.Font("Tahoma", 16F);
            this.bar2.BarAppearance.Pressed.Options.UseFont = true;
            this.bar2.BarName = "Main menu";
            this.bar2.DockCol = 0;
            this.bar2.DockRow = 0;
            this.bar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar2.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.btSave),
            new DevExpress.XtraBars.LinkPersistInfo(this.btConfirm)});
            this.bar2.OptionsBar.AllowQuickCustomization = false;
            this.bar2.OptionsBar.MultiLine = true;
            this.bar2.OptionsBar.UseWholeRow = true;
            this.bar2.Text = "Main menu";
            // 
            // btSave
            // 
            this.btSave.Caption = "保存";
            this.btSave.Id = 1;
            this.btSave.Name = "btSave";
            // 
            // btConfirm
            // 
            this.btConfirm.Caption = "确认";
            this.btConfirm.Id = 0;
            this.btConfirm.Name = "btConfirm";
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(1454, 50);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 177);
            this.barDockControlBottom.Size = new System.Drawing.Size(1454, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 50);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 127);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1454, 50);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 127);
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.splitContainerControl1.Appearance.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.splitContainerControl1.Appearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.splitContainerControl1.Appearance.Options.UseBackColor = true;
            this.splitContainerControl1.Appearance.Options.UseBorderColor = true;
            this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.Horizontal = false;
            this.splitContainerControl1.Location = new System.Drawing.Point(0, 0);
            this.splitContainerControl1.Name = "splitContainerControl1";
            this.splitContainerControl1.Panel1.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.splitContainerControl1.Panel1.Appearance.Options.UseBackColor = true;
            this.splitContainerControl1.Panel1.Controls.Add(this.layoutControl1);
            this.splitContainerControl1.Panel1.Controls.Add(this.barDockControlLeft);
            this.splitContainerControl1.Panel1.Controls.Add(this.barDockControlRight);
            this.splitContainerControl1.Panel1.Controls.Add(this.barDockControlBottom);
            this.splitContainerControl1.Panel1.Controls.Add(this.barDockControlTop);
            this.splitContainerControl1.Panel1.Text = "Panel1";
            this.splitContainerControl1.Panel2.Controls.Add(this.FAllCheck);
            this.splitContainerControl1.Panel2.Controls.Add(this.gc_Data);
            this.splitContainerControl1.Panel2.Controls.Add(this.barDockControl3);
            this.splitContainerControl1.Panel2.Controls.Add(this.barDockControl4);
            this.splitContainerControl1.Panel2.Controls.Add(this.barDockControl2);
            this.splitContainerControl1.Panel2.Controls.Add(this.barDockControl1);
            this.splitContainerControl1.Panel2.Text = "Panel2";
            this.splitContainerControl1.Size = new System.Drawing.Size(1454, 654);
            this.splitContainerControl1.SplitterPosition = 177;
            this.splitContainerControl1.TabIndex = 9;
            this.splitContainerControl1.Text = "splitContainerControl1";
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.FHSpecification);
            this.layoutControl1.Controls.Add(this.FHStockArea);
            this.layoutControl1.Controls.Add(this.FHStock);
            this.layoutControl1.Controls.Add(this.FHProduct);
            this.layoutControl1.Controls.Add(this.FOper);
            this.layoutControl1.Controls.Add(this.FIsConfirm);
            this.layoutControl1.Controls.Add(this.btQuery);
            this.layoutControl1.Controls.Add(this.FEndDate);
            this.layoutControl1.Controls.Add(this.FBeginDate);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.layoutControl1.Location = new System.Drawing.Point(0, 50);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(1451, 127);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // FHSpecification
            // 
            this.FHSpecification.Location = new System.Drawing.Point(1136, 18);
            this.FHSpecification.MenuManager = this.barManager1;
            this.FHSpecification.Name = "FHSpecification";
            this.FHSpecification.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 13F);
            this.FHSpecification.Properties.Appearance.Options.UseFont = true;
            this.FHSpecification.Size = new System.Drawing.Size(140, 38);
            this.FHSpecification.StyleController = this.layoutControl1;
            this.FHSpecification.TabIndex = 13;
            // 
            // FHStockArea
            // 
            this.FHStockArea.Location = new System.Drawing.Point(789, 18);
            this.FHStockArea.MenuManager = this.barManager1;
            this.FHStockArea.Name = "FHStockArea";
            this.FHStockArea.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 13F);
            this.FHStockArea.Properties.Appearance.Options.UseFont = true;
            this.FHStockArea.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.FHStockArea.Properties.PopupFormSize = new System.Drawing.Size(340, 400);
            this.FHStockArea.Properties.PopupResizeMode = DevExpress.XtraEditors.Controls.ResizeMode.FrameResize;
            this.FHStockArea.Size = new System.Drawing.Size(264, 38);
            this.FHStockArea.StyleController = this.layoutControl1;
            this.FHStockArea.TabIndex = 12;
            // 
            // FHStock
            // 
            this.FHStock.Location = new System.Drawing.Point(454, 18);
            this.FHStock.MenuManager = this.barManager1;
            this.FHStock.Name = "FHStock";
            this.FHStock.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 13F);
            this.FHStock.Properties.Appearance.Options.UseFont = true;
            this.FHStock.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.FHStock.Properties.PopupFormSize = new System.Drawing.Size(340, 400);
            this.FHStock.Properties.PopupResizeMode = DevExpress.XtraEditors.Controls.ResizeMode.FrameResize;
            this.FHStock.Size = new System.Drawing.Size(252, 38);
            this.FHStock.StyleController = this.layoutControl1;
            this.FHStock.TabIndex = 11;
            this.FHStock.EditValueChanged += new System.EventHandler(this.FHStock_EditValueChanged);
            // 
            // FHProduct
            // 
            this.FHProduct.Location = new System.Drawing.Point(95, 18);
            this.FHProduct.MenuManager = this.barManager1;
            this.FHProduct.Name = "FHProduct";
            this.FHProduct.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 13F);
            this.FHProduct.Properties.Appearance.Options.UseFont = true;
            this.FHProduct.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.FHProduct.Properties.NullText = "";
            this.FHProduct.Properties.View = this.searchLookUpEdit1View;
            this.FHProduct.Size = new System.Drawing.Size(276, 38);
            this.FHProduct.StyleController = this.layoutControl1;
            this.FHProduct.TabIndex = 10;
            // 
            // searchLookUpEdit1View
            // 
            this.searchLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.searchLookUpEdit1View.Name = "searchLookUpEdit1View";
            this.searchLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.searchLookUpEdit1View.OptionsView.ShowGroupPanel = false;
            // 
            // FOper
            // 
            this.FOper.Location = new System.Drawing.Point(95, 62);
            this.FOper.MenuManager = this.barManager1;
            this.FOper.Name = "FOper";
            this.FOper.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 13F);
            this.FOper.Properties.Appearance.Options.UseFont = true;
            this.FOper.Size = new System.Drawing.Size(276, 38);
            this.FOper.StyleController = this.layoutControl1;
            this.FOper.TabIndex = 9;
            // 
            // FIsConfirm
            // 
            this.FIsConfirm.Location = new System.Drawing.Point(1282, 18);
            this.FIsConfirm.MenuManager = this.barManager1;
            this.FIsConfirm.Name = "FIsConfirm";
            this.FIsConfirm.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 13F);
            this.FIsConfirm.Properties.Appearance.Options.UseFont = true;
            this.FIsConfirm.Properties.Caption = "已确认";
            this.FIsConfirm.Size = new System.Drawing.Size(151, 35);
            this.FIsConfirm.StyleController = this.layoutControl1;
            this.FIsConfirm.TabIndex = 8;
            // 
            // btQuery
            // 
            this.btQuery.Appearance.Font = new System.Drawing.Font("Tahoma", 13F);
            this.btQuery.Appearance.Options.UseFont = true;
            this.btQuery.Location = new System.Drawing.Point(1059, 62);
            this.btQuery.Name = "btQuery";
            this.btQuery.Size = new System.Drawing.Size(374, 40);
            this.btQuery.StyleController = this.layoutControl1;
            this.btQuery.TabIndex = 7;
            this.btQuery.Text = "查询";
            // 
            // FEndDate
            // 
            this.FEndDate.EditValue = null;
            this.FEndDate.Location = new System.Drawing.Point(789, 62);
            this.FEndDate.MenuManager = this.barManager1;
            this.FEndDate.Name = "FEndDate";
            this.FEndDate.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 13F);
            this.FEndDate.Properties.Appearance.Options.UseFont = true;
            this.FEndDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.FEndDate.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.FEndDate.Properties.DisplayFormat.FormatString = "g";
            this.FEndDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.FEndDate.Properties.EditFormat.FormatString = "g";
            this.FEndDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.FEndDate.Properties.Mask.EditMask = "g";
            this.FEndDate.Size = new System.Drawing.Size(264, 38);
            this.FEndDate.StyleController = this.layoutControl1;
            this.FEndDate.TabIndex = 5;
            // 
            // FBeginDate
            // 
            this.FBeginDate.EditValue = null;
            this.FBeginDate.Location = new System.Drawing.Point(454, 62);
            this.FBeginDate.MenuManager = this.barManager1;
            this.FBeginDate.Name = "FBeginDate";
            this.FBeginDate.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 13F);
            this.FBeginDate.Properties.Appearance.Options.UseFont = true;
            this.FBeginDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.FBeginDate.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.FBeginDate.Properties.DisplayFormat.FormatString = "g";
            this.FBeginDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.FBeginDate.Properties.EditFormat.FormatString = "g";
            this.FBeginDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.FBeginDate.Properties.Mask.EditMask = "g";
            this.FBeginDate.Size = new System.Drawing.Size(252, 38);
            this.FBeginDate.StyleController = this.layoutControl1;
            this.FBeginDate.TabIndex = 4;
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem4,
            this.lo_Specification,
            this.lo_StockArea,
            this.lo_IsConfirm,
            this.lo_Product,
            this.lo_Stock,
            this.lo_Oper,
            this.lo_BeginDate,
            this.lo_EndDate});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.OptionsItemText.TextToControlDistance = 5;
            this.layoutControlGroup1.Size = new System.Drawing.Size(1451, 127);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.btQuery;
            this.layoutControlItem4.Location = new System.Drawing.Point(1041, 44);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(380, 53);
            this.layoutControlItem4.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem4.TextVisible = false;
            // 
            // lo_Specification
            // 
            this.lo_Specification.Control = this.FHSpecification;
            this.lo_Specification.Location = new System.Drawing.Point(1041, 0);
            this.lo_Specification.Name = "lo_Specification";
            this.lo_Specification.Size = new System.Drawing.Size(223, 44);
            this.lo_Specification.Text = "规格";
            this.lo_Specification.TextSize = new System.Drawing.Size(72, 22);
            // 
            // lo_StockArea
            // 
            this.lo_StockArea.Control = this.FHStockArea;
            this.lo_StockArea.Location = new System.Drawing.Point(694, 0);
            this.lo_StockArea.Name = "lo_StockArea";
            this.lo_StockArea.Size = new System.Drawing.Size(347, 44);
            this.lo_StockArea.Text = "库区";
            this.lo_StockArea.TextSize = new System.Drawing.Size(72, 22);
            // 
            // lo_IsConfirm
            // 
            this.lo_IsConfirm.Control = this.FIsConfirm;
            this.lo_IsConfirm.CustomizationFormText = "已确认";
            this.lo_IsConfirm.Location = new System.Drawing.Point(1264, 0);
            this.lo_IsConfirm.Name = "lo_IsConfirm";
            this.lo_IsConfirm.Size = new System.Drawing.Size(157, 44);
            this.lo_IsConfirm.Text = "已确认";
            this.lo_IsConfirm.TextSize = new System.Drawing.Size(0, 0);
            this.lo_IsConfirm.TextVisible = false;
            // 
            // lo_Product
            // 
            this.lo_Product.Control = this.FHProduct;
            this.lo_Product.Location = new System.Drawing.Point(0, 0);
            this.lo_Product.Name = "lo_Product";
            this.lo_Product.Size = new System.Drawing.Size(359, 44);
            this.lo_Product.Text = "产品";
            this.lo_Product.TextSize = new System.Drawing.Size(72, 22);
            // 
            // lo_Stock
            // 
            this.lo_Stock.Control = this.FHStock;
            this.lo_Stock.Location = new System.Drawing.Point(359, 0);
            this.lo_Stock.Name = "lo_Stock";
            this.lo_Stock.Size = new System.Drawing.Size(335, 44);
            this.lo_Stock.Text = "仓库";
            this.lo_Stock.TextSize = new System.Drawing.Size(72, 22);
            // 
            // lo_Oper
            // 
            this.lo_Oper.Control = this.FOper;
            this.lo_Oper.Location = new System.Drawing.Point(0, 44);
            this.lo_Oper.Name = "lo_Oper";
            this.lo_Oper.Size = new System.Drawing.Size(359, 53);
            this.lo_Oper.Text = "当班";
            this.lo_Oper.TextSize = new System.Drawing.Size(72, 22);
            // 
            // lo_BeginDate
            // 
            this.lo_BeginDate.Control = this.FBeginDate;
            this.lo_BeginDate.Location = new System.Drawing.Point(359, 44);
            this.lo_BeginDate.Name = "lo_BeginDate";
            this.lo_BeginDate.Size = new System.Drawing.Size(335, 53);
            this.lo_BeginDate.Text = "开始日期";
            this.lo_BeginDate.TextSize = new System.Drawing.Size(72, 22);
            // 
            // lo_EndDate
            // 
            this.lo_EndDate.Control = this.FEndDate;
            this.lo_EndDate.Location = new System.Drawing.Point(694, 44);
            this.lo_EndDate.Name = "lo_EndDate";
            this.lo_EndDate.Size = new System.Drawing.Size(347, 53);
            this.lo_EndDate.Text = "结束日期";
            this.lo_EndDate.TextSize = new System.Drawing.Size(72, 22);
            // 
            // FAllCheck
            // 
            this.FAllCheck.Location = new System.Drawing.Point(83, 41);
            this.FAllCheck.MenuManager = this.barManager1;
            this.FAllCheck.Name = "FAllCheck";
            this.FAllCheck.Properties.Appearance.BackColor = System.Drawing.Color.WhiteSmoke;
            this.FAllCheck.Properties.Appearance.Options.UseBackColor = true;
            this.FAllCheck.Properties.Caption = "";
            this.FAllCheck.Size = new System.Drawing.Size(26, 19);
            this.FAllCheck.TabIndex = 5;
            this.FAllCheck.CheckedChanged += new System.EventHandler(this.FAllCheck_CheckedChanged);
            // 
            // gc_Data
            // 
            this.gc_Data.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gc_Data.Location = new System.Drawing.Point(0, 35);
            this.gc_Data.MainView = this.gv_View;
            this.gc_Data.MenuManager = this.barManager1;
            this.gc_Data.Name = "gc_Data";
            this.gc_Data.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.cab_Amount,
            this.slu_Status,
            this.slu_StockAreaPlan,
            this.deConfirmDate,
            this.cb_Check});
            this.gc_Data.Size = new System.Drawing.Size(1454, 434);
            this.gc_Data.TabIndex = 4;
            this.gc_Data.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gv_View});
            // 
            // gv_View
            // 
            this.gv_View.Appearance.Empty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.gv_View.Appearance.Empty.Options.UseBackColor = true;
            this.gv_View.Appearance.FocusedRow.BackColor = System.Drawing.Color.Silver;
            this.gv_View.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gv_View.Appearance.HeaderPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.gv_View.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gv_View.Appearance.Preview.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.gv_View.Appearance.Preview.Options.UseBackColor = true;
            this.gv_View.Appearance.Row.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.gv_View.Appearance.Row.Options.UseBackColor = true;
            this.gv_View.Appearance.TopNewRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.gv_View.Appearance.TopNewRow.Options.UseBackColor = true;
            this.gv_View.Appearance.VertLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.gv_View.Appearance.VertLine.Options.UseBackColor = true;
            this.gv_View.Appearance.ViewCaption.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.gv_View.Appearance.ViewCaption.Options.UseBackColor = true;
            this.gv_View.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.FCheck,
            this.FID,
            this.FOperator,
            this.FBarCode,
            this.FProductName,
            this.FStockAreaPlan,
            this.FStock,
            this.FStockArea,
            this.FAmount,
            this.FStatusName,
            this.FDate,
            this.FConfirmDate,
            this.FPaperTypeName,
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
            this.FCustomer,
            this.FStockDate,
            this.FWeight,
            this.FProduct,
            this.FStatus,
            this.FProductInfo,
            this.FStockName,
            this.FStockAreaName,
            this.FPlanName});
            this.gv_View.FooterPanelHeight = 20;
            this.gv_View.GridControl = this.gc_Data;
            this.gv_View.IndicatorWidth = 60;
            this.gv_View.Name = "gv_View";
            this.gv_View.OptionsBehavior.KeepGroupExpandedOnSorting = false;
            this.gv_View.OptionsCustomization.AllowFilter = false;
            this.gv_View.OptionsCustomization.AllowSort = false;
            this.gv_View.OptionsMenu.EnableFooterMenu = false;
            this.gv_View.OptionsMenu.EnableGroupPanelMenu = false;
            this.gv_View.OptionsView.ColumnAutoWidth = false;
            this.gv_View.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gv_View.OptionsView.ShowFooter = true;
            this.gv_View.OptionsView.ShowGroupPanel = false;
            // 
            // FCheck
            // 
            this.FCheck.Caption = " ";
            this.FCheck.ColumnEdit = this.cb_Check;
            this.FCheck.FieldName = "FCHECK";
            this.FCheck.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            this.FCheck.Name = "FCheck";
            this.FCheck.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.FCheck.OptionsColumn.AllowMove = false;
            this.FCheck.OptionsColumn.AllowShowHide = false;
            this.FCheck.OptionsColumn.AllowSize = false;
            this.FCheck.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.FCheck.OptionsColumn.FixedWidth = true;
            this.FCheck.OptionsFilter.AllowAutoFilter = false;
            this.FCheck.OptionsFilter.AllowFilter = false;
            this.FCheck.Visible = true;
            this.FCheck.VisibleIndex = 0;
            this.FCheck.Width = 60;
            // 
            // cb_Check
            // 
            this.cb_Check.AutoHeight = false;
            this.cb_Check.Name = "cb_Check";
            this.cb_Check.ValueChecked = 1;
            this.cb_Check.ValueUnchecked = 0;
            // 
            // FID
            // 
            this.FID.Caption = "主键";
            this.FID.FieldName = "FID";
            this.FID.Name = "FID";
            // 
            // FOperator
            // 
            this.FOperator.Caption = "当班";
            this.FOperator.FieldName = "FOPERATOR";
            this.FOperator.Name = "FOperator";
            this.FOperator.Visible = true;
            this.FOperator.VisibleIndex = 1;
            this.FOperator.Width = 118;
            // 
            // FBarCode
            // 
            this.FBarCode.Caption = "条形码";
            this.FBarCode.FieldName = "FBARCODE";
            this.FBarCode.Name = "FBarCode";
            this.FBarCode.OptionsColumn.ReadOnly = true;
            this.FBarCode.Visible = true;
            this.FBarCode.VisibleIndex = 9;
            this.FBarCode.Width = 126;
            // 
            // FProductName
            // 
            this.FProductName.Caption = "产品名称";
            this.FProductName.FieldName = "FPRODUCTNAME";
            this.FProductName.Name = "FProductName";
            this.FProductName.Visible = true;
            this.FProductName.VisibleIndex = 2;
            this.FProductName.Width = 208;
            // 
            // FStockAreaPlan
            // 
            this.FStockAreaPlan.Caption = "计划名称";
            this.FStockAreaPlan.ColumnEdit = this.slu_StockAreaPlan;
            this.FStockAreaPlan.FieldName = "FSTOCKAREAPLAN";
            this.FStockAreaPlan.Name = "FStockAreaPlan";
            this.FStockAreaPlan.Visible = true;
            this.FStockAreaPlan.VisibleIndex = 7;
            this.FStockAreaPlan.Width = 108;
            // 
            // slu_StockAreaPlan
            // 
            this.slu_StockAreaPlan.AutoHeight = false;
            this.slu_StockAreaPlan.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.slu_StockAreaPlan.Name = "slu_StockAreaPlan";
            this.slu_StockAreaPlan.NullText = "";
            this.slu_StockAreaPlan.View = this.gridView3;
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
            this.FStock.Caption = "仓库";
            this.FStock.FieldName = "FSTOCK";
            this.FStock.Name = "FStock";
            this.FStock.Width = 154;
            // 
            // FStockArea
            // 
            this.FStockArea.Caption = "库区";
            this.FStockArea.FieldName = "FSTOCKAREA";
            this.FStockArea.Name = "FStockArea";
            this.FStockArea.Width = 154;
            // 
            // FAmount
            // 
            this.FAmount.Caption = "件数";
            this.FAmount.ColumnEdit = this.cab_Amount;
            this.FAmount.FieldName = "FAMOUNT";
            this.FAmount.Name = "FAmount";
            this.FAmount.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "FAMOUNT", "合计={0:0.##}")});
            this.FAmount.Visible = true;
            this.FAmount.VisibleIndex = 3;
            this.FAmount.Width = 72;
            // 
            // cab_Amount
            // 
            this.cab_Amount.AutoHeight = false;
            this.cab_Amount.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cab_Amount.Name = "cab_Amount";
            // 
            // FStatusName
            // 
            this.FStatusName.Caption = "状态";
            this.FStatusName.ColumnEdit = this.slu_Status;
            this.FStatusName.FieldName = "FSTATUSNAME";
            this.FStatusName.Name = "FStatusName";
            this.FStatusName.Visible = true;
            this.FStatusName.VisibleIndex = 31;
            this.FStatusName.Width = 127;
            // 
            // slu_Status
            // 
            this.slu_Status.AutoHeight = false;
            this.slu_Status.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.slu_Status.Name = "slu_Status";
            this.slu_Status.NullText = "";
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
            // FDate
            // 
            this.FDate.Caption = "扫描日期";
            this.FDate.FieldName = "FDATE";
            this.FDate.Name = "FDate";
            this.FDate.Visible = true;
            this.FDate.VisibleIndex = 32;
            this.FDate.Width = 108;
            // 
            // FConfirmDate
            // 
            this.FConfirmDate.Caption = "确认日期";
            this.FConfirmDate.ColumnEdit = this.deConfirmDate;
            this.FConfirmDate.FieldName = "FCONFIRMDATE";
            this.FConfirmDate.Name = "FConfirmDate";
            this.FConfirmDate.Visible = true;
            this.FConfirmDate.VisibleIndex = 33;
            this.FConfirmDate.Width = 135;
            // 
            // deConfirmDate
            // 
            this.deConfirmDate.AutoHeight = false;
            this.deConfirmDate.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.deConfirmDate.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.deConfirmDate.Name = "deConfirmDate";
            // 
            // FPaperTypeName
            // 
            this.FPaperTypeName.Caption = "产品类型";
            this.FPaperTypeName.FieldName = "FPAPERTYPENAME";
            this.FPaperTypeName.Name = "FPaperTypeName";
            this.FPaperTypeName.Visible = true;
            this.FPaperTypeName.VisibleIndex = 30;
            // 
            // FPaperType
            // 
            this.FPaperType.Caption = "产品类型";
            this.FPaperType.FieldName = "FPAPERTYPE";
            this.FPaperType.Name = "FPaperType";
            this.FPaperType.OptionsColumn.ReadOnly = true;
            this.FPaperType.Width = 135;
            // 
            // FSpecification
            // 
            this.FSpecification.Caption = "规格";
            this.FSpecification.FieldName = "FSPECIFICATION";
            this.FSpecification.Name = "FSpecification";
            this.FSpecification.OptionsColumn.ReadOnly = true;
            this.FSpecification.Visible = true;
            this.FSpecification.VisibleIndex = 11;
            // 
            // FPaperGrade
            // 
            this.FPaperGrade.Caption = "产品等级";
            this.FPaperGrade.FieldName = "FPAPERGRADE";
            this.FPaperGrade.Name = "FPaperGrade";
            this.FPaperGrade.OptionsColumn.ReadOnly = true;
            this.FPaperGrade.Visible = true;
            this.FPaperGrade.VisibleIndex = 12;
            this.FPaperGrade.Width = 150;
            // 
            // FCertification
            // 
            this.FCertification.Caption = "纸种认证";
            this.FCertification.FieldName = "FCERTIFICATION";
            this.FCertification.Name = "FCertification";
            this.FCertification.OptionsColumn.ReadOnly = true;
            this.FCertification.Visible = true;
            this.FCertification.VisibleIndex = 13;
            this.FCertification.Width = 173;
            // 
            // FTransportType
            // 
            this.FTransportType.Caption = "夹板方式";
            this.FTransportType.FieldName = "FTRANSPORTTYPE";
            this.FTransportType.Name = "FTransportType";
            this.FTransportType.OptionsColumn.ReadOnly = true;
            this.FTransportType.Visible = true;
            this.FTransportType.VisibleIndex = 14;
            this.FTransportType.Width = 122;
            // 
            // FCoreDiameterOrReam
            // 
            this.FCoreDiameterOrReam.Caption = "纸芯/令数";
            this.FCoreDiameterOrReam.FieldName = "FCOREDIAMETERORREAM";
            this.FCoreDiameterOrReam.Name = "FCoreDiameterOrReam";
            this.FCoreDiameterOrReam.OptionsColumn.ReadOnly = true;
            this.FCoreDiameterOrReam.Visible = true;
            this.FCoreDiameterOrReam.VisibleIndex = 15;
            this.FCoreDiameterOrReam.Width = 114;
            // 
            // FDiameterOrSlides
            // 
            this.FDiameterOrSlides.Caption = "直径MM/令张数";
            this.FDiameterOrSlides.FieldName = "FDIAMETERORSLIDES";
            this.FDiameterOrSlides.Name = "FDiameterOrSlides";
            this.FDiameterOrSlides.OptionsColumn.ReadOnly = true;
            this.FDiameterOrSlides.Visible = true;
            this.FDiameterOrSlides.VisibleIndex = 16;
            this.FDiameterOrSlides.Width = 189;
            // 
            // FLength
            // 
            this.FLength.Caption = "线长/件张数";
            this.FLength.FieldName = "FLENGTH";
            this.FLength.Name = "FLength";
            this.FLength.OptionsColumn.ReadOnly = true;
            this.FLength.Visible = true;
            this.FLength.VisibleIndex = 17;
            this.FLength.Width = 156;
            // 
            // FTrademarkStyle
            // 
            this.FTrademarkStyle.Caption = "合格证类型";
            this.FTrademarkStyle.FieldName = "FTRADEMARKSTYLE";
            this.FTrademarkStyle.Name = "FTrademarkStyle";
            this.FTrademarkStyle.OptionsColumn.ReadOnly = true;
            this.FTrademarkStyle.Visible = true;
            this.FTrademarkStyle.VisibleIndex = 18;
            this.FTrademarkStyle.Width = 178;
            // 
            // FSpecCustName
            // 
            this.FSpecCustName.Caption = "客户专用";
            this.FSpecCustName.FieldName = "FSPECCUSTNAME";
            this.FSpecCustName.Name = "FSpecCustName";
            this.FSpecCustName.OptionsColumn.ReadOnly = true;
            this.FSpecCustName.Visible = true;
            this.FSpecCustName.VisibleIndex = 19;
            this.FSpecCustName.Width = 142;
            // 
            // FSpecProdName
            // 
            this.FSpecProdName.Caption = "产品专用";
            this.FSpecProdName.FieldName = "FSPECPRODNAME";
            this.FSpecProdName.Name = "FSpecProdName";
            this.FSpecProdName.OptionsColumn.ReadOnly = true;
            this.FSpecProdName.Visible = true;
            this.FSpecProdName.VisibleIndex = 20;
            this.FSpecProdName.Width = 119;
            // 
            // FPackType
            // 
            this.FPackType.Caption = "包装方式";
            this.FPackType.FieldName = "FPACKTYPE";
            this.FPackType.Name = "FPackType";
            this.FPackType.OptionsColumn.ReadOnly = true;
            this.FPackType.Visible = true;
            this.FPackType.VisibleIndex = 21;
            this.FPackType.Width = 133;
            // 
            // FDeliverDate
            // 
            this.FDeliverDate.Caption = "交货日期";
            this.FDeliverDate.FieldName = "FDELIVERDATE";
            this.FDeliverDate.Name = "FDeliverDate";
            this.FDeliverDate.OptionsColumn.ReadOnly = true;
            this.FDeliverDate.Visible = true;
            this.FDeliverDate.VisibleIndex = 22;
            this.FDeliverDate.Width = 187;
            // 
            // FWeightMode
            // 
            this.FWeightMode.Caption = "卷筒计重";
            this.FWeightMode.FieldName = "FWEIGHTMODE";
            this.FWeightMode.Name = "FWeightMode";
            this.FWeightMode.OptionsColumn.ReadOnly = true;
            this.FWeightMode.Visible = true;
            this.FWeightMode.VisibleIndex = 23;
            this.FWeightMode.Width = 147;
            // 
            // FColor
            // 
            this.FColor.Caption = "产品色相";
            this.FColor.FieldName = "FCOLOR";
            this.FColor.Name = "FColor";
            this.FColor.OptionsColumn.ReadOnly = true;
            this.FColor.Visible = true;
            this.FColor.VisibleIndex = 24;
            this.FColor.Width = 119;
            // 
            // FPoNumber
            // 
            this.FPoNumber.Caption = "客户订单号";
            this.FPoNumber.FieldName = "FPONUMBER";
            this.FPoNumber.Name = "FPoNumber";
            this.FPoNumber.OptionsColumn.ReadOnly = true;
            this.FPoNumber.Visible = true;
            this.FPoNumber.VisibleIndex = 26;
            this.FPoNumber.Width = 152;
            // 
            // FSpCustomer
            // 
            this.FSpCustomer.Caption = "产品备注";
            this.FSpCustomer.FieldName = "FSPCUSTOMER";
            this.FSpCustomer.Name = "FSpCustomer";
            this.FSpCustomer.Visible = true;
            this.FSpCustomer.VisibleIndex = 27;
            this.FSpCustomer.Width = 112;
            // 
            // FMemo
            // 
            this.FMemo.Caption = "备注";
            this.FMemo.FieldName = "FMEMO";
            this.FMemo.Name = "FMemo";
            this.FMemo.OptionsColumn.ReadOnly = true;
            this.FMemo.Visible = true;
            this.FMemo.VisibleIndex = 28;
            // 
            // FCustomer
            // 
            this.FCustomer.Caption = "客户";
            this.FCustomer.FieldName = "FCUSTOMER";
            this.FCustomer.Name = "FCustomer";
            this.FCustomer.OptionsColumn.ReadOnly = true;
            this.FCustomer.Visible = true;
            this.FCustomer.VisibleIndex = 29;
            this.FCustomer.Width = 93;
            // 
            // FStockDate
            // 
            this.FStockDate.Caption = "日期";
            this.FStockDate.FieldName = "FSTOCKDATE";
            this.FStockDate.Name = "FStockDate";
            this.FStockDate.OptionsColumn.ReadOnly = true;
            this.FStockDate.Visible = true;
            this.FStockDate.VisibleIndex = 25;
            // 
            // FWeight
            // 
            this.FWeight.Caption = "重量";
            this.FWeight.FieldName = "FWEIGHT";
            this.FWeight.Name = "FWeight";
            this.FWeight.Visible = true;
            this.FWeight.VisibleIndex = 4;
            // 
            // FProduct
            // 
            this.FProduct.Caption = "产品(隐藏)";
            this.FProduct.FieldName = "FPRODUCT";
            this.FProduct.Name = "FProduct";
            // 
            // FStatus
            // 
            this.FStatus.Caption = "状态(隐藏)";
            this.FStatus.FieldName = "FSTATUS";
            this.FStatus.Name = "FStatus";
            // 
            // FProductInfo
            // 
            this.FProductInfo.Caption = "产品信息";
            this.FProductInfo.FieldName = "FPRODUCTINFO";
            this.FProductInfo.Name = "FProductInfo";
            this.FProductInfo.Visible = true;
            this.FProductInfo.VisibleIndex = 10;
            this.FProductInfo.Width = 291;
            // 
            // FStockName
            // 
            this.FStockName.Caption = "仓库";
            this.FStockName.FieldName = "FSTOCKNAME";
            this.FStockName.Name = "FStockName";
            this.FStockName.OptionsColumn.ReadOnly = true;
            this.FStockName.Visible = true;
            this.FStockName.VisibleIndex = 5;
            // 
            // FStockAreaName
            // 
            this.FStockAreaName.Caption = "库区";
            this.FStockAreaName.FieldName = "FSTOCKAREANAME";
            this.FStockAreaName.Name = "FStockAreaName";
            this.FStockAreaName.OptionsColumn.ReadOnly = true;
            this.FStockAreaName.Visible = true;
            this.FStockAreaName.VisibleIndex = 6;
            // 
            // FPlanName
            // 
            this.FPlanName.Caption = "计划名称";
            this.FPlanName.FieldName = "FPLANNAME";
            this.FPlanName.Name = "FPlanName";
            this.FPlanName.Visible = true;
            this.FPlanName.VisibleIndex = 8;
            this.FPlanName.Width = 167;
            // 
            // barDockControl3
            // 
            this.barDockControl3.CausesValidation = false;
            this.barDockControl3.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControl3.Location = new System.Drawing.Point(0, 35);
            this.barDockControl3.Size = new System.Drawing.Size(0, 434);
            // 
            // barDockControl4
            // 
            this.barDockControl4.CausesValidation = false;
            this.barDockControl4.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControl4.Location = new System.Drawing.Point(1454, 35);
            this.barDockControl4.Size = new System.Drawing.Size(0, 434);
            // 
            // barDockControl2
            // 
            this.barDockControl2.CausesValidation = false;
            this.barDockControl2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControl2.Location = new System.Drawing.Point(0, 469);
            this.barDockControl2.Size = new System.Drawing.Size(1454, 0);
            // 
            // barDockControl1
            // 
            this.barDockControl1.CausesValidation = false;
            this.barDockControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControl1.Location = new System.Drawing.Point(0, 0);
            this.barDockControl1.Size = new System.Drawing.Size(1454, 35);
            // 
            // repositoryItemSearchLookUpEdit1View
            // 
            this.repositoryItemSearchLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.repositoryItemSearchLookUpEdit1View.Name = "repositoryItemSearchLookUpEdit1View";
            this.repositoryItemSearchLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.repositoryItemSearchLookUpEdit1View.OptionsView.ShowGroupPanel = false;
            // 
            // gridView1
            // 
            this.gridView1.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // gridView4
            // 
            this.gridView4.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridView4.Name = "gridView4";
            this.gridView4.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView4.OptionsView.ShowGroupPanel = false;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // barManager2
            // 
            this.barManager2.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar3});
            this.barManager2.DockControls.Add(this.barDockControl1);
            this.barManager2.DockControls.Add(this.barDockControl2);
            this.barManager2.DockControls.Add(this.barDockControl3);
            this.barManager2.DockControls.Add(this.barDockControl4);
            this.barManager2.Form = this.splitContainerControl1.Panel2;
            this.barManager2.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.btBatchFill});
            this.barManager2.MainMenu = this.bar3;
            this.barManager2.MaxItemId = 1;
            // 
            // bar3
            // 
            this.bar3.BarName = "Main menu";
            this.bar3.DockCol = 0;
            this.bar3.DockRow = 0;
            this.bar3.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar3.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.btBatchFill)});
            this.bar3.OptionsBar.MultiLine = true;
            this.bar3.OptionsBar.UseWholeRow = true;
            this.bar3.Text = "Main menu";
            // 
            // btBatchFill
            // 
            this.btBatchFill.Caption = "批量填充";
            this.btBatchFill.Id = 0;
            this.btBatchFill.Name = "btBatchFill";
            this.btBatchFill.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btBatchFill_ItemClick);
            // 
            // DistributionList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 22F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1454, 654);
            this.Controls.Add(this.splitContainerControl1);
            this.Name = "DistributionList";
            this.Text = "产品入库确认";
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.FHSpecification.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FHStockArea.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FHStock.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FHProduct.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLookUpEdit1View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FOper.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FIsConfirm.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FEndDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FEndDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FBeginDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FBeginDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_Specification)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_StockArea)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_IsConfirm)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_Product)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_Stock)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_Oper)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_BeginDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_EndDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FAllCheck.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gc_Data)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gv_View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_Check)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.slu_StockAreaPlan)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cab_Amount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.slu_Status)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deConfirmDate.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deConfirmDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSearchLookUpEdit1View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar2;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarButtonItem btConfirm;
        private DevExpress.XtraGrid.GridControl gc_Data;
        private DevExpress.XtraGrid.Views.Grid.GridView gv_View;
        private DevExpress.XtraGrid.Columns.GridColumn FID;
        private DevExpress.XtraGrid.Columns.GridColumn FProductName;
        private DevExpress.XtraGrid.Columns.GridColumn FStockAreaPlan;
        private DevExpress.XtraGrid.Columns.GridColumn FStock;
        private DevExpress.XtraGrid.Columns.GridColumn FStockArea;
        private DevExpress.XtraGrid.Columns.GridColumn FAmount;
        private DevExpress.XtraGrid.Columns.GridColumn FStatusName;
        private DevExpress.XtraGrid.Views.Grid.GridView repositoryItemSearchLookUpEdit1View;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraEditors.Repository.RepositoryItemCalcEdit cab_Amount;
        private DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit slu_Status;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
        private DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit slu_StockAreaPlan;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView3;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private DevExpress.XtraGrid.Columns.GridColumn FDate;
        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraEditors.SimpleButton btQuery;
        private DevExpress.XtraEditors.DateEdit FEndDate;
        private DevExpress.XtraEditors.DateEdit FBeginDate;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.LayoutControlItem lo_BeginDate;
        private DevExpress.XtraLayout.LayoutControlItem lo_EndDate;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraEditors.CheckEdit FIsConfirm;
        private DevExpress.XtraLayout.LayoutControlItem lo_IsConfirm;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView4;
        private DevExpress.XtraGrid.Columns.GridColumn FBarCode;
        private DevExpress.XtraGrid.Columns.GridColumn FConfirmDate;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit deConfirmDate;
        private DevExpress.XtraBars.BarButtonItem btSave;
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
        private DevExpress.XtraGrid.Columns.GridColumn FCustomer;
        private DevExpress.XtraGrid.Columns.GridColumn FStockDate;
        private DevExpress.XtraGrid.Columns.GridColumn FWeight;
        private DevExpress.XtraGrid.Columns.GridColumn FSpCustomer;
        private DevExpress.XtraGrid.Columns.GridColumn FOperator;
        private DevExpress.XtraGrid.Columns.GridColumn FCheck;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit cb_Check;
        private DevExpress.XtraEditors.CheckEdit FAllCheck;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private DevExpress.XtraGrid.Columns.GridColumn FProduct;
        private DevExpress.XtraGrid.Columns.GridColumn FStatus;
        private DevExpress.XtraEditors.TextEdit FOper;
        private DevExpress.XtraLayout.LayoutControlItem lo_Oper;
        private DevExpress.XtraGrid.Columns.GridColumn FPaperTypeName;
        private DevExpress.XtraEditors.TextEdit FHSpecification;
        private DevExpress.XtraEditors.CheckedComboBoxEdit FHStockArea;
        private DevExpress.XtraEditors.CheckedComboBoxEdit FHStock;
        private DevExpress.XtraEditors.SearchLookUpEdit FHProduct;
        private DevExpress.XtraGrid.Views.Grid.GridView searchLookUpEdit1View;
        private DevExpress.XtraLayout.LayoutControlItem lo_Product;
        private DevExpress.XtraLayout.LayoutControlItem lo_Stock;
        private DevExpress.XtraLayout.LayoutControlItem lo_StockArea;
        private DevExpress.XtraLayout.LayoutControlItem lo_Specification;
        private DevExpress.XtraGrid.Columns.GridColumn FProductInfo;
        private DevExpress.XtraGrid.Columns.GridColumn FStockName;
        private DevExpress.XtraGrid.Columns.GridColumn FStockAreaName;
        private DevExpress.XtraGrid.Columns.GridColumn FPlanName;
        private DevExpress.XtraBars.BarDockControl barDockControl3;
        private DevExpress.XtraBars.BarDockControl barDockControl4;
        private DevExpress.XtraBars.BarDockControl barDockControl2;
        private DevExpress.XtraBars.BarDockControl barDockControl1;
        private DevExpress.XtraBars.BarManager barManager2;
        private DevExpress.XtraBars.Bar bar3;
        private DevExpress.XtraBars.BarButtonItem btBatchFill;
    }
}