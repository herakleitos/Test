namespace Chaint.Instock.Business
{
    partial class StockAreaPlanQuery
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
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.FIsComplete = new DevExpress.XtraEditors.CheckEdit();
            this.btQuery = new DevExpress.XtraEditors.SimpleButton();
            this.FEndDate = new DevExpress.XtraEditors.DateEdit();
            this.FBeginDate = new DevExpress.XtraEditors.DateEdit();
            this.FStockArea = new DevExpress.XtraEditors.CheckedComboBoxEdit();
            this.FStock = new DevExpress.XtraEditors.CheckedComboBoxEdit();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lo_Stock = new DevExpress.XtraLayout.LayoutControlItem();
            this.lo_StockArea = new DevExpress.XtraLayout.LayoutControlItem();
            this.lo_BeginDate = new DevExpress.XtraLayout.LayoutControlItem();
            this.lo_EndDate = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.gc_Data = new DevExpress.XtraGrid.GridControl();
            this.gv_View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.FProduct = new DevExpress.XtraGrid.Columns.GridColumn();
            this.FStockAreaPlan = new DevExpress.XtraGrid.Columns.GridColumn();
            this.FPlanAmount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.FAmount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.FWAmount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.FTotalCapacity = new DevExpress.XtraGrid.Columns.GridColumn();
            this.FUsedCapacity = new DevExpress.XtraGrid.Columns.GridColumn();
            this.FEStockArea = new DevExpress.XtraGrid.Columns.GridColumn();
            this.FEStock = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemSearchLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FIsComplete.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FEndDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FEndDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FBeginDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FBeginDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FStockArea.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FStock.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_Stock)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_StockArea)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_BeginDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_EndDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gc_Data)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gv_View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSearchLookUpEdit1View)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.Horizontal = false;
            this.splitContainerControl1.Location = new System.Drawing.Point(0, 0);
            this.splitContainerControl1.Name = "splitContainerControl1";
            this.splitContainerControl1.Panel1.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.splitContainerControl1.Panel1.Appearance.Options.UseBackColor = true;
            this.splitContainerControl1.Panel1.Controls.Add(this.layoutControl1);
            this.splitContainerControl1.Panel1.Text = "Panel1";
            this.splitContainerControl1.Panel2.Controls.Add(this.gc_Data);
            this.splitContainerControl1.Panel2.Text = "Panel2";
            this.splitContainerControl1.Size = new System.Drawing.Size(1437, 701);
            this.splitContainerControl1.SplitterPosition = 122;
            this.splitContainerControl1.TabIndex = 0;
            this.splitContainerControl1.Text = "splitContainerControl1";
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.FIsComplete);
            this.layoutControl1.Controls.Add(this.btQuery);
            this.layoutControl1.Controls.Add(this.FEndDate);
            this.layoutControl1.Controls.Add(this.FBeginDate);
            this.layoutControl1.Controls.Add(this.FStockArea);
            this.layoutControl1.Controls.Add(this.FStock);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(1000, 122);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // FIsComplete
            // 
            this.FIsComplete.Location = new System.Drawing.Point(711, 18);
            this.FIsComplete.Name = "FIsComplete";
            this.FIsComplete.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.FIsComplete.Properties.Appearance.Options.UseFont = true;
            this.FIsComplete.Properties.Caption = "计划完成";
            this.FIsComplete.Size = new System.Drawing.Size(106, 28);
            this.FIsComplete.StyleController = this.layoutControl1;
            this.FIsComplete.TabIndex = 9;
            // 
            // btQuery
            // 
            this.btQuery.Appearance.Font = new System.Drawing.Font("Tahoma", 11F);
            this.btQuery.Appearance.Options.UseFont = true;
            this.btQuery.Location = new System.Drawing.Point(823, 18);
            this.btQuery.Name = "btQuery";
            this.btQuery.Size = new System.Drawing.Size(159, 36);
            this.btQuery.StyleController = this.layoutControl1;
            this.btQuery.TabIndex = 8;
            this.btQuery.Text = "查询";
            this.btQuery.Click += new System.EventHandler(this.btQuery_Click);
            // 
            // FEndDate
            // 
            this.FEndDate.EditValue = new System.DateTime(2017, 10, 19, 0, 0, 0, 0);
            this.FEndDate.Location = new System.Drawing.Point(441, 62);
            this.FEndDate.Name = "FEndDate";
            this.FEndDate.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 13F);
            this.FEndDate.Properties.Appearance.Options.UseFont = true;
            this.FEndDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph)});
            this.FEndDate.Properties.CalendarTimeEditing = DevExpress.Utils.DefaultBoolean.True;
            this.FEndDate.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph)});
            this.FEndDate.Properties.CalendarView = DevExpress.XtraEditors.Repository.CalendarView.Vista;
            this.FEndDate.Properties.DisplayFormat.FormatString = "G";
            this.FEndDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.FEndDate.Properties.EditFormat.FormatString = "G";
            this.FEndDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.FEndDate.Properties.Mask.EditMask = null;
            this.FEndDate.Properties.VistaDisplayMode = DevExpress.Utils.DefaultBoolean.True;
            this.FEndDate.Size = new System.Drawing.Size(264, 38);
            this.FEndDate.StyleController = this.layoutControl1;
            this.FEndDate.TabIndex = 7;
            // 
            // FBeginDate
            // 
            this.FBeginDate.EditValue = null;
            this.FBeginDate.Location = new System.Drawing.Point(95, 62);
            this.FBeginDate.Name = "FBeginDate";
            this.FBeginDate.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 13F);
            this.FBeginDate.Properties.Appearance.Options.UseFont = true;
            this.FBeginDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph)});
            this.FBeginDate.Properties.CalendarTimeEditing = DevExpress.Utils.DefaultBoolean.True;
            this.FBeginDate.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph)});
            this.FBeginDate.Properties.CalendarView = DevExpress.XtraEditors.Repository.CalendarView.Vista;
            this.FBeginDate.Properties.DisplayFormat.FormatString = "G";
            this.FBeginDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.FBeginDate.Properties.EditFormat.FormatString = "G";
            this.FBeginDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.FBeginDate.Properties.Mask.EditMask = null;
            this.FBeginDate.Properties.VistaDisplayMode = DevExpress.Utils.DefaultBoolean.True;
            this.FBeginDate.Size = new System.Drawing.Size(263, 38);
            this.FBeginDate.StyleController = this.layoutControl1;
            this.FBeginDate.TabIndex = 6;
            // 
            // FStockArea
            // 
            this.FStockArea.Location = new System.Drawing.Point(441, 18);
            this.FStockArea.Name = "FStockArea";
            this.FStockArea.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 13F);
            this.FStockArea.Properties.Appearance.Options.UseFont = true;
            this.FStockArea.Properties.AppearanceDropDown.Font = new System.Drawing.Font("Tahoma", 13F);
            this.FStockArea.Properties.AppearanceDropDown.Options.UseFont = true;
            this.FStockArea.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.FStockArea.Properties.PopupFormSize = new System.Drawing.Size(340, 400);
            this.FStockArea.Properties.PopupResizeMode = DevExpress.XtraEditors.Controls.ResizeMode.FrameResize;
            this.FStockArea.Size = new System.Drawing.Size(264, 38);
            this.FStockArea.StyleController = this.layoutControl1;
            this.FStockArea.TabIndex = 5;
            // 
            // FStock
            // 
            this.FStock.Location = new System.Drawing.Point(95, 18);
            this.FStock.Name = "FStock";
            this.FStock.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 13F);
            this.FStock.Properties.Appearance.Options.UseFont = true;
            this.FStock.Properties.AppearanceDropDown.Font = new System.Drawing.Font("Tahoma", 13F);
            this.FStock.Properties.AppearanceDropDown.Options.UseFont = true;
            this.FStock.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.FStock.Properties.PopupFormSize = new System.Drawing.Size(340, 400);
            this.FStock.Properties.PopupResizeMode = DevExpress.XtraEditors.Controls.ResizeMode.FrameResize;
            this.FStock.Size = new System.Drawing.Size(263, 38);
            this.FStock.StyleController = this.layoutControl1;
            this.FStock.TabIndex = 4;
            this.FStock.EditValueChanged += new System.EventHandler(this.FStock_EditValueChanged);
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lo_Stock,
            this.lo_StockArea,
            this.lo_BeginDate,
            this.lo_EndDate,
            this.layoutControlItem5,
            this.layoutControlItem1});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.OptionsItemText.TextToControlDistance = 5;
            this.layoutControlGroup1.Size = new System.Drawing.Size(1000, 122);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // lo_Stock
            // 
            this.lo_Stock.Control = this.FStock;
            this.lo_Stock.Location = new System.Drawing.Point(0, 0);
            this.lo_Stock.Name = "lo_Stock";
            this.lo_Stock.Size = new System.Drawing.Size(346, 44);
            this.lo_Stock.Text = "仓库";
            this.lo_Stock.TextSize = new System.Drawing.Size(72, 22);
            // 
            // lo_StockArea
            // 
            this.lo_StockArea.Control = this.FStockArea;
            this.lo_StockArea.Location = new System.Drawing.Point(346, 0);
            this.lo_StockArea.Name = "lo_StockArea";
            this.lo_StockArea.Size = new System.Drawing.Size(347, 44);
            this.lo_StockArea.Text = "库区";
            this.lo_StockArea.TextSize = new System.Drawing.Size(72, 22);
            // 
            // lo_BeginDate
            // 
            this.lo_BeginDate.Control = this.FBeginDate;
            this.lo_BeginDate.Location = new System.Drawing.Point(0, 44);
            this.lo_BeginDate.Name = "lo_BeginDate";
            this.lo_BeginDate.Size = new System.Drawing.Size(346, 48);
            this.lo_BeginDate.Text = "开始日期";
            this.lo_BeginDate.TextSize = new System.Drawing.Size(72, 22);
            // 
            // lo_EndDate
            // 
            this.lo_EndDate.Control = this.FEndDate;
            this.lo_EndDate.Location = new System.Drawing.Point(346, 44);
            this.lo_EndDate.Name = "lo_EndDate";
            this.lo_EndDate.Size = new System.Drawing.Size(347, 48);
            this.lo_EndDate.Text = "结束日期";
            this.lo_EndDate.TextSize = new System.Drawing.Size(72, 22);
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.btQuery;
            this.layoutControlItem5.Location = new System.Drawing.Point(805, 0);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Size = new System.Drawing.Size(165, 92);
            this.layoutControlItem5.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem5.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.FIsComplete;
            this.layoutControlItem1.Location = new System.Drawing.Point(693, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(112, 92);
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextVisible = false;
            // 
            // gc_Data
            // 
            this.gc_Data.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gc_Data.Location = new System.Drawing.Point(0, 0);
            this.gc_Data.MainView = this.gv_View;
            this.gc_Data.Name = "gc_Data";
            this.gc_Data.Size = new System.Drawing.Size(1437, 571);
            this.gc_Data.TabIndex = 0;
            this.gc_Data.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gv_View});
            // 
            // gv_View
            // 
            this.gv_View.Appearance.Empty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.gv_View.Appearance.Empty.Options.UseBackColor = true;
            this.gv_View.Appearance.FocusedRow.BackColor = System.Drawing.Color.Silver;
            this.gv_View.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gv_View.Appearance.GroupRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.gv_View.Appearance.GroupRow.Options.UseBackColor = true;
            this.gv_View.Appearance.Row.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.gv_View.Appearance.Row.Options.UseBackColor = true;
            this.gv_View.Appearance.SelectedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.gv_View.Appearance.SelectedRow.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.gv_View.Appearance.SelectedRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.gv_View.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gv_View.Appearance.SelectedRow.Options.UseBorderColor = true;
            this.gv_View.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.FProduct,
            this.FStockAreaPlan,
            this.FPlanAmount,
            this.FAmount,
            this.FWAmount,
            this.FTotalCapacity,
            this.FUsedCapacity,
            this.FEStockArea,
            this.FEStock});
            this.gv_View.GridControl = this.gc_Data;
            this.gv_View.Name = "gv_View";
            this.gv_View.OptionsBehavior.ReadOnly = true;
            this.gv_View.OptionsView.ShowGroupPanel = false;
            // 
            // FProduct
            // 
            this.FProduct.Caption = "产品";
            this.FProduct.FieldName = "FPRODUCT";
            this.FProduct.Name = "FProduct";
            this.FProduct.Visible = true;
            this.FProduct.VisibleIndex = 1;
            this.FProduct.Width = 157;
            // 
            // FStockAreaPlan
            // 
            this.FStockAreaPlan.Caption = "计划名称";
            this.FStockAreaPlan.FieldName = "FSTOCKAREAPLAN";
            this.FStockAreaPlan.Name = "FStockAreaPlan";
            this.FStockAreaPlan.Visible = true;
            this.FStockAreaPlan.VisibleIndex = 0;
            this.FStockAreaPlan.Width = 153;
            // 
            // FPlanAmount
            // 
            this.FPlanAmount.Caption = "计划件数";
            this.FPlanAmount.FieldName = "FPLANAMOUNT";
            this.FPlanAmount.Name = "FPlanAmount";
            this.FPlanAmount.Visible = true;
            this.FPlanAmount.VisibleIndex = 6;
            this.FPlanAmount.Width = 153;
            // 
            // FAmount
            // 
            this.FAmount.Caption = "已确认件数";
            this.FAmount.FieldName = "FAMOUNT";
            this.FAmount.Name = "FAmount";
            this.FAmount.Visible = true;
            this.FAmount.VisibleIndex = 7;
            this.FAmount.Width = 153;
            // 
            // FWAmount
            // 
            this.FWAmount.Caption = "待确认件数";
            this.FWAmount.FieldName = "FWAMOUNT";
            this.FWAmount.Name = "FWAmount";
            this.FWAmount.Visible = true;
            this.FWAmount.VisibleIndex = 8;
            this.FWAmount.Width = 154;
            // 
            // FTotalCapacity
            // 
            this.FTotalCapacity.Caption = "库区总容量";
            this.FTotalCapacity.FieldName = "FTOTALCAPACITY";
            this.FTotalCapacity.Name = "FTotalCapacity";
            this.FTotalCapacity.Visible = true;
            this.FTotalCapacity.VisibleIndex = 4;
            this.FTotalCapacity.Width = 157;
            // 
            // FUsedCapacity
            // 
            this.FUsedCapacity.Caption = "库区已使用容量";
            this.FUsedCapacity.FieldName = "FUSEDCAPACITY";
            this.FUsedCapacity.Name = "FUsedCapacity";
            this.FUsedCapacity.Visible = true;
            this.FUsedCapacity.VisibleIndex = 5;
            this.FUsedCapacity.Width = 173;
            // 
            // FEStockArea
            // 
            this.FEStockArea.Caption = "库区";
            this.FEStockArea.FieldName = "FSTOCKAREA";
            this.FEStockArea.Name = "FEStockArea";
            this.FEStockArea.Visible = true;
            this.FEStockArea.VisibleIndex = 3;
            this.FEStockArea.Width = 157;
            // 
            // FEStock
            // 
            this.FEStock.Caption = "仓库";
            this.FEStock.FieldName = "FSTOCK";
            this.FEStock.Name = "FEStock";
            this.FEStock.Visible = true;
            this.FEStock.VisibleIndex = 2;
            this.FEStock.Width = 157;
            // 
            // repositoryItemSearchLookUpEdit1View
            // 
            this.repositoryItemSearchLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.repositoryItemSearchLookUpEdit1View.Name = "repositoryItemSearchLookUpEdit1View";
            this.repositoryItemSearchLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.repositoryItemSearchLookUpEdit1View.OptionsView.ShowGroupPanel = false;
            // 
            // StockAreaPlanQuery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 22F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1437, 701);
            this.Controls.Add(this.splitContainerControl1);
            this.Name = "StockAreaPlanQuery";
            this.Text = "计划数据查询";
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.FIsComplete.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FEndDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FEndDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FBeginDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FBeginDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FStockArea.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FStock.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_Stock)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_StockArea)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_BeginDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_EndDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gc_Data)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gv_View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSearchLookUpEdit1View)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraEditors.SimpleButton btQuery;
        private DevExpress.XtraEditors.DateEdit FEndDate;
        private DevExpress.XtraEditors.DateEdit FBeginDate;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.LayoutControlItem lo_Stock;
        private DevExpress.XtraLayout.LayoutControlItem lo_StockArea;
        private DevExpress.XtraLayout.LayoutControlItem lo_BeginDate;
        private DevExpress.XtraLayout.LayoutControlItem lo_EndDate;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraGrid.GridControl gc_Data;
        private DevExpress.XtraGrid.Views.Grid.GridView gv_View;
        private DevExpress.XtraGrid.Columns.GridColumn FProduct;
        private DevExpress.XtraGrid.Columns.GridColumn FStockAreaPlan;
        private DevExpress.XtraGrid.Columns.GridColumn FPlanAmount;
        private DevExpress.XtraGrid.Columns.GridColumn FAmount;
        private DevExpress.XtraGrid.Columns.GridColumn FWAmount;
        private DevExpress.XtraEditors.CheckEdit FIsComplete;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraGrid.Columns.GridColumn FTotalCapacity;
        private DevExpress.XtraGrid.Columns.GridColumn FUsedCapacity;
        private DevExpress.XtraGrid.Views.Grid.GridView repositoryItemSearchLookUpEdit1View;
        private DevExpress.XtraEditors.CheckedComboBoxEdit FStockArea;
        private DevExpress.XtraGrid.Columns.GridColumn FEStockArea;
        private DevExpress.XtraEditors.CheckedComboBoxEdit FStock;
        private DevExpress.XtraGrid.Columns.GridColumn FEStock;
    }
}