
namespace Chaint.Instock.Business
{
    partial class CommonList
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
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.btQuery = new DevExpress.XtraEditors.SimpleButton();
            this.FEndDate = new DevExpress.XtraEditors.DateEdit();
            this.FBeginDate = new DevExpress.XtraEditors.DateEdit();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.gc_Main = new DevExpress.XtraGrid.GridControl();
            this.gv_View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.FCheck = new DevExpress.XtraGrid.Columns.GridColumn();
            this.cbCheck = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar2 = new DevExpress.XtraBars.Bar();
            this.btNew = new DevExpress.XtraBars.BarButtonItem();
            this.btOpen = new DevExpress.XtraBars.BarButtonItem();
            this.btRefresh = new DevExpress.XtraBars.BarButtonItem();
            this.btDelete = new DevExpress.XtraBars.BarButtonItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FEndDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FEndDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FBeginDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FBeginDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gc_Main)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gv_View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbCheck)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
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
            this.splitContainerControl1.Panel1.Controls.Add(this.barDockControlLeft);
            this.splitContainerControl1.Panel1.Controls.Add(this.barDockControlRight);
            this.splitContainerControl1.Panel1.Controls.Add(this.barDockControlBottom);
            this.splitContainerControl1.Panel1.Controls.Add(this.barDockControlTop);
            this.splitContainerControl1.Panel1.Text = "Panel1";
            this.splitContainerControl1.Panel2.Controls.Add(this.gc_Main);
            this.splitContainerControl1.Panel2.Text = "Panel2";
            this.splitContainerControl1.Size = new System.Drawing.Size(1376, 654);
            this.splitContainerControl1.SplitterPosition = 133;
            this.splitContainerControl1.TabIndex = 0;
            this.splitContainerControl1.Text = "splitContainerControl1";
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.btQuery);
            this.layoutControl1.Controls.Add(this.FEndDate);
            this.layoutControl1.Controls.Add(this.FBeginDate);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.layoutControl1.Location = new System.Drawing.Point(0, 50);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(969, 83);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // btQuery
            // 
            this.btQuery.Appearance.Font = new System.Drawing.Font("Tahoma", 13F);
            this.btQuery.Appearance.Options.UseFont = true;
            this.btQuery.Location = new System.Drawing.Point(818, 18);
            this.btQuery.Name = "btQuery";
            this.btQuery.Size = new System.Drawing.Size(133, 40);
            this.btQuery.StyleController = this.layoutControl1;
            this.btQuery.TabIndex = 3;
            this.btQuery.Text = "查询";
            // 
            // FEndDate
            // 
            this.FEndDate.EditValue = null;
            this.FEndDate.Location = new System.Drawing.Point(495, 18);
            this.FEndDate.Name = "FEndDate";
            this.FEndDate.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 13F);
            this.FEndDate.Properties.Appearance.Options.UseFont = true;
            this.FEndDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.FEndDate.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.FEndDate.Size = new System.Drawing.Size(317, 38);
            this.FEndDate.StyleController = this.layoutControl1;
            this.FEndDate.TabIndex = 2;
            // 
            // FBeginDate
            // 
            this.FBeginDate.EditValue = null;
            this.FBeginDate.Location = new System.Drawing.Point(95, 18);
            this.FBeginDate.Name = "FBeginDate";
            this.FBeginDate.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 13F);
            this.FBeginDate.Properties.Appearance.Options.UseFont = true;
            this.FBeginDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.FBeginDate.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.FBeginDate.Size = new System.Drawing.Size(317, 38);
            this.FBeginDate.StyleController = this.layoutControl1;
            this.FBeginDate.TabIndex = 0;
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlItem2,
            this.layoutControlItem3});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.OptionsItemText.TextToControlDistance = 5;
            this.layoutControlGroup1.Size = new System.Drawing.Size(969, 83);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.FBeginDate;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(400, 53);
            this.layoutControlItem1.Text = "开始日期";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(72, 22);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.FEndDate;
            this.layoutControlItem2.Location = new System.Drawing.Point(400, 0);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(400, 53);
            this.layoutControlItem2.Text = "结束日期";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(72, 22);
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.btQuery;
            this.layoutControlItem3.Location = new System.Drawing.Point(800, 0);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(139, 53);
            this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem3.TextVisible = false;
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 50);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 83);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1376, 50);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 83);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 133);
            this.barDockControlBottom.Size = new System.Drawing.Size(1376, 0);
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(1376, 50);
            // 
            // gc_Main
            // 
            this.gc_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gc_Main.Font = new System.Drawing.Font("Tahoma", 13F);
            this.gc_Main.Location = new System.Drawing.Point(0, 0);
            this.gc_Main.MainView = this.gv_View;
            this.gc_Main.Name = "gc_Main";
            this.gc_Main.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.cbCheck});
            this.gc_Main.Size = new System.Drawing.Size(1376, 513);
            this.gc_Main.TabIndex = 0;
            this.gc_Main.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gv_View});
            // 
            // gv_View
            // 
            this.gv_View.Appearance.Empty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.gv_View.Appearance.Empty.Options.UseBackColor = true;
            this.gv_View.Appearance.FocusedRow.BackColor = System.Drawing.Color.Silver;
            this.gv_View.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gv_View.Appearance.Row.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.gv_View.Appearance.Row.Options.UseBackColor = true;
            this.gv_View.Appearance.ViewCaption.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.gv_View.Appearance.ViewCaption.Options.UseBackColor = true;
            this.gv_View.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.FCheck});
            this.gv_View.GridControl = this.gc_Main;
            this.gv_View.Name = "gv_View";
            this.gv_View.OptionsBehavior.Editable = false;
            this.gv_View.OptionsBehavior.ReadOnly = true;
            this.gv_View.OptionsView.ColumnAutoWidth = false;
            this.gv_View.OptionsView.ShowGroupPanel = false;
            // 
            // FCheck
            // 
            this.FCheck.Caption = "选择";
            this.FCheck.ColumnEdit = this.cbCheck;
            this.FCheck.FieldName = "FCHECK";
            this.FCheck.Name = "FCheck";
            this.FCheck.Visible = true;
            this.FCheck.VisibleIndex = 0;
            // 
            // cbCheck
            // 
            this.cbCheck.AutoHeight = false;
            this.cbCheck.Name = "cbCheck";
            this.cbCheck.ValueChecked = 1;
            this.cbCheck.ValueUnchecked = 0;
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
            this.btDelete,
            this.btRefresh,
            this.btOpen,
            this.btNew});
            this.barManager1.MainMenu = this.bar2;
            this.barManager1.MaxItemId = 4;
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
            new DevExpress.XtraBars.LinkPersistInfo(this.btNew),
            new DevExpress.XtraBars.LinkPersistInfo(this.btOpen),
            new DevExpress.XtraBars.LinkPersistInfo(this.btRefresh),
            new DevExpress.XtraBars.LinkPersistInfo(this.btDelete)});
            this.bar2.OptionsBar.MultiLine = true;
            this.bar2.OptionsBar.UseWholeRow = true;
            this.bar2.Text = "Main menu";
            // 
            // btNew
            // 
            this.btNew.Caption = "新增";
            this.btNew.Id = 3;
            this.btNew.Name = "btNew";
            this.btNew.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btNew_ItemClick);
            // 
            // btOpen
            // 
            this.btOpen.Caption = "打开";
            this.btOpen.Id = 2;
            this.btOpen.Name = "btOpen";
            this.btOpen.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btOpen_ItemClick);
            // 
            // btRefresh
            // 
            this.btRefresh.Caption = "刷新";
            this.btRefresh.Id = 1;
            this.btRefresh.Name = "btRefresh";
            this.btRefresh.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btRefresh_ItemClick);
            // 
            // btDelete
            // 
            this.btDelete.Caption = "删除";
            this.btDelete.Id = 0;
            this.btDelete.Name = "btDelete";
            this.btDelete.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btDelete_ItemClick);
            // 
            // CommonList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 22F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1376, 654);
            this.Controls.Add(this.splitContainerControl1);
            this.Name = "CommonList";
            this.Text = "列表";
            this.Load += new System.EventHandler(this.OnLoad);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.FEndDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FEndDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FBeginDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FBeginDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gc_Main)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gv_View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbCheck)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private DevExpress.XtraGrid.GridControl gc_Main;
        private DevExpress.XtraGrid.Views.Grid.GridView gv_View;
        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraEditors.SimpleButton btQuery;
        private DevExpress.XtraEditors.DateEdit FEndDate;
        private DevExpress.XtraEditors.DateEdit FBeginDate;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit cbCheck;
        public DevExpress.XtraGrid.Columns.GridColumn FCheck;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar2;
        private DevExpress.XtraBars.BarButtonItem btDelete;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarButtonItem btRefresh;
        private DevExpress.XtraBars.BarButtonItem btOpen;
        private DevExpress.XtraBars.BarButtonItem btNew;
    }
}