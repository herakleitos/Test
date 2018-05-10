
namespace Chaint.Instock.Business
{
    partial class Auth
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Auth));
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.FUser = new DevExpress.XtraEditors.SearchLookUpEdit();
            this.barManager = new DevExpress.XtraBars.BarManager();
            this.mainBar = new DevExpress.XtraBars.Bar();
            this.btSave = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControl1 = new DevExpress.XtraBars.BarDockControl();
            this.barDockControl2 = new DevExpress.XtraBars.BarDockControl();
            this.barDockControl3 = new DevExpress.XtraBars.BarDockControl();
            this.barDockControl4 = new DevExpress.XtraBars.BarDockControl();
            this.searchLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lo_User = new DevExpress.XtraLayout.LayoutControlItem();
            this.gc_Entry = new DevExpress.XtraGrid.GridControl();
            this.gv_Entry = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.FUserID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.FCheck = new DevExpress.XtraGrid.Columns.GridColumn();
            this.cb_Check = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.FFormName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.FFormId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.FEntryID = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FUser.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLookUpEdit1View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_User)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gc_Entry)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gv_Entry)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_Check)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainerControl1
            // 
            resources.ApplyResources(this.splitContainerControl1, "splitContainerControl1");
            this.splitContainerControl1.Horizontal = false;
            this.splitContainerControl1.Name = "splitContainerControl1";
            this.splitContainerControl1.Panel1.Appearance.BackColor = ((System.Drawing.Color)(resources.GetObject("splitContainerControl1.Panel1.Appearance.BackColor")));
            this.splitContainerControl1.Panel1.Appearance.BackColor2 = ((System.Drawing.Color)(resources.GetObject("splitContainerControl1.Panel1.Appearance.BackColor2")));
            this.splitContainerControl1.Panel1.Appearance.BorderColor = ((System.Drawing.Color)(resources.GetObject("splitContainerControl1.Panel1.Appearance.BorderColor")));
            this.splitContainerControl1.Panel1.Appearance.Options.UseBackColor = true;
            this.splitContainerControl1.Panel1.Appearance.Options.UseBorderColor = true;
            this.splitContainerControl1.Panel1.Controls.Add(this.layoutControl1);
            this.splitContainerControl1.Panel1.Controls.Add(this.barDockControl3);
            this.splitContainerControl1.Panel1.Controls.Add(this.barDockControl4);
            this.splitContainerControl1.Panel1.Controls.Add(this.barDockControl2);
            this.splitContainerControl1.Panel1.Controls.Add(this.barDockControl1);
            resources.ApplyResources(this.splitContainerControl1.Panel1, "splitContainerControl1.Panel1");
            this.splitContainerControl1.Panel2.Controls.Add(this.gc_Entry);
            resources.ApplyResources(this.splitContainerControl1.Panel2, "splitContainerControl1.Panel2");
            this.splitContainerControl1.SplitterPosition = 150;
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.FUser);
            resources.ApplyResources(this.layoutControl1, "layoutControl1");
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.layoutControlGroup1;
            // 
            // FUser
            // 
            resources.ApplyResources(this.FUser, "FUser");
            this.FUser.MenuManager = this.barManager;
            this.FUser.Name = "FUser";
            this.FUser.Properties.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("FUser.Properties.Appearance.Font")));
            this.FUser.Properties.Appearance.Options.UseFont = true;
            this.FUser.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(((DevExpress.XtraEditors.Controls.ButtonPredefines)(resources.GetObject("FUser.Properties.Buttons"))))});
            this.FUser.Properties.NullText = resources.GetString("FUser.Properties.NullText");
            this.FUser.Properties.View = this.searchLookUpEdit1View;
            this.FUser.StyleController = this.layoutControl1;
            this.FUser.EditValueChanged += new System.EventHandler(this.FUser_EditValueChanged);
            // 
            // barManager
            // 
            this.barManager.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.mainBar});
            this.barManager.DockControls.Add(this.barDockControl1);
            this.barManager.DockControls.Add(this.barDockControl2);
            this.barManager.DockControls.Add(this.barDockControl3);
            this.barManager.DockControls.Add(this.barDockControl4);
            this.barManager.Form = this.splitContainerControl1.Panel1;
            this.barManager.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.btSave});
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
            this.mainBar.BarName = "菜单";
            this.mainBar.DockCol = 0;
            this.mainBar.DockRow = 0;
            this.mainBar.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.mainBar.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.btSave)});
            this.mainBar.OptionsBar.MultiLine = true;
            this.mainBar.OptionsBar.UseWholeRow = true;
            resources.ApplyResources(this.mainBar, "mainBar");
            // 
            // btSave
            // 
            resources.ApplyResources(this.btSave, "btSave");
            this.btSave.Id = 1;
            this.btSave.Name = "btSave";
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
            this.lo_User});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.OptionsItemText.TextToControlDistance = 5;
            this.layoutControlGroup1.Size = new System.Drawing.Size(506, 100);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // lo_User
            // 
            this.lo_User.Control = this.FUser;
            this.lo_User.Location = new System.Drawing.Point(0, 0);
            this.lo_User.Name = "lo_User";
            this.lo_User.Size = new System.Drawing.Size(476, 70);
            resources.ApplyResources(this.lo_User, "lo_User");
            this.lo_User.TextSize = new System.Drawing.Size(36, 22);
            // 
            // gc_Entry
            // 
            resources.ApplyResources(this.gc_Entry, "gc_Entry");
            this.gc_Entry.MainView = this.gv_Entry;
            this.gc_Entry.MenuManager = this.barManager;
            this.gc_Entry.Name = "gc_Entry";
            this.gc_Entry.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.cb_Check});
            this.gc_Entry.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gv_Entry});
            // 
            // gv_Entry
            // 
            this.gv_Entry.Appearance.Empty.BackColor = ((System.Drawing.Color)(resources.GetObject("gv_Entry.Appearance.Empty.BackColor")));
            this.gv_Entry.Appearance.Empty.Options.UseBackColor = true;
            this.gv_Entry.Appearance.FocusedRow.BackColor = ((System.Drawing.Color)(resources.GetObject("gv_Entry.Appearance.FocusedRow.BackColor")));
            this.gv_Entry.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gv_Entry.Appearance.GroupRow.BackColor = ((System.Drawing.Color)(resources.GetObject("gv_Entry.Appearance.GroupRow.BackColor")));
            this.gv_Entry.Appearance.GroupRow.Options.UseBackColor = true;
            this.gv_Entry.Appearance.Row.BackColor = ((System.Drawing.Color)(resources.GetObject("gv_Entry.Appearance.Row.BackColor")));
            this.gv_Entry.Appearance.Row.Options.UseBackColor = true;
            this.gv_Entry.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.FUserID,
            this.FCheck,
            this.FFormName,
            this.FFormId,
            this.FEntryID});
            this.gv_Entry.GridControl = this.gc_Entry;
            this.gv_Entry.Name = "gv_Entry";
            this.gv_Entry.OptionsView.ColumnAutoWidth = false;
            this.gv_Entry.OptionsView.ShowGroupPanel = false;
            // 
            // FUserID
            // 
            resources.ApplyResources(this.FUserID, "FUserID");
            this.FUserID.FieldName = "FUSERID";
            this.FUserID.Name = "FUserID";
            // 
            // FCheck
            // 
            resources.ApplyResources(this.FCheck, "FCheck");
            this.FCheck.ColumnEdit = this.cb_Check;
            this.FCheck.FieldName = "FCHECK";
            this.FCheck.Name = "FCheck";
            // 
            // cb_Check
            // 
            resources.ApplyResources(this.cb_Check, "cb_Check");
            this.cb_Check.Name = "cb_Check";
            this.cb_Check.ValueChecked = 1;
            this.cb_Check.ValueUnchecked = 0;
            // 
            // FFormName
            // 
            resources.ApplyResources(this.FFormName, "FFormName");
            this.FFormName.FieldName = "FFORMNAME";
            this.FFormName.Name = "FFormName";
            this.FFormName.OptionsColumn.ReadOnly = true;
            // 
            // FFormId
            // 
            resources.ApplyResources(this.FFormId, "FFormId");
            this.FFormId.FieldName = "FFORMID";
            this.FFormId.Name = "FFormId";
            // 
            // FEntryID
            // 
            resources.ApplyResources(this.FEntryID, "FEntryID");
            this.FEntryID.FieldName = "FENTRYID";
            this.FEntryID.Name = "FEntryID";
            // 
            // Auth
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.splitContainerControl1);
            this.Name = "Auth";
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.FUser.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLookUpEdit1View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_User)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gc_Entry)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gv_Entry)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_Check)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private DevExpress.XtraBars.BarManager barManager;
        private DevExpress.XtraBars.Bar mainBar;
        private DevExpress.XtraBars.BarDockControl barDockControl1;
        private DevExpress.XtraBars.BarDockControl barDockControl2;
        private DevExpress.XtraBars.BarDockControl barDockControl3;
        private DevExpress.XtraBars.BarDockControl barDockControl4;
        private DevExpress.XtraBars.BarButtonItem btSave;
        private DevExpress.XtraGrid.GridControl gc_Entry;
        private DevExpress.XtraGrid.Views.Grid.GridView gv_Entry;
        private DevExpress.XtraGrid.Columns.GridColumn FUserID;
        private DevExpress.XtraGrid.Columns.GridColumn FFormName;
        private DevExpress.XtraGrid.Columns.GridColumn FCheck;
        private DevExpress.XtraGrid.Columns.GridColumn FFormId;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit cb_Check;
        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraEditors.SearchLookUpEdit FUser;
        private DevExpress.XtraGrid.Views.Grid.GridView searchLookUpEdit1View;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.LayoutControlItem lo_User;
        private DevExpress.XtraGrid.Columns.GridColumn FEntryID;
    }
}
