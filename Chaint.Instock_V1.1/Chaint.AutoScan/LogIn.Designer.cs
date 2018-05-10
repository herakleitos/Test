using System;
using System.Windows.Forms;

namespace Chaint.Instock.Main
{
    partial class LogIn
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
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.btLogIn = new DevExpress.XtraEditors.SimpleButton();
            this.tbPassword = new DevExpress.XtraEditors.TextEdit();
            this.sluUser = new DevExpress.XtraEditors.SearchLookUpEdit();
            this.searchLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lo_LogIn = new DevExpress.XtraLayout.LayoutControlItem();
            this.lo_User = new DevExpress.XtraLayout.LayoutControlItem();
            this.lo_Password = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbPassword.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sluUser.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLookUpEdit1View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_LogIn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_User)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_Password)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.btLogIn);
            this.layoutControl1.Controls.Add(this.tbPassword);
            this.layoutControl1.Controls.Add(this.sluUser);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(466, 210);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // btLogIn
            // 
            this.btLogIn.Appearance.Font = new System.Drawing.Font("Tahoma", 15F);
            this.btLogIn.Appearance.Options.UseFont = true;
            this.btLogIn.Location = new System.Drawing.Point(18, 114);
            this.btLogIn.Name = "btLogIn";
            this.btLogIn.Size = new System.Drawing.Size(430, 45);
            this.btLogIn.StyleController = this.layoutControl1;
            this.btLogIn.TabIndex = 6;
            this.btLogIn.Text = "登陆";
            this.btLogIn.Click += new System.EventHandler(this.btLogIn_Click);
            // 
            // tbPassword
            // 
            this.tbPassword.Location = new System.Drawing.Point(59, 66);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 15F);
            this.tbPassword.Properties.Appearance.Options.UseFont = true;
            this.tbPassword.Properties.PasswordChar = '*';
            this.tbPassword.Size = new System.Drawing.Size(389, 42);
            this.tbPassword.StyleController = this.layoutControl1;
            this.tbPassword.TabIndex = 5;
            // 
            // sluUser
            // 
            this.sluUser.EditValue = "";
            this.sluUser.Location = new System.Drawing.Point(59, 18);
            this.sluUser.Name = "sluUser";
            this.sluUser.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 15F);
            this.sluUser.Properties.Appearance.Options.UseFont = true;
            this.sluUser.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.sluUser.Properties.NullText = "";
            this.sluUser.Properties.View = this.searchLookUpEdit1View;
            this.sluUser.Size = new System.Drawing.Size(389, 42);
            this.sluUser.StyleController = this.layoutControl1;
            this.sluUser.TabIndex = 4;
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
            this.lo_LogIn,
            this.lo_User,
            this.lo_Password});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.OptionsItemText.TextToControlDistance = 5;
            this.layoutControlGroup1.Size = new System.Drawing.Size(466, 210);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // lo_LogIn
            // 
            this.lo_LogIn.Control = this.btLogIn;
            this.lo_LogIn.ControlAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.lo_LogIn.Location = new System.Drawing.Point(0, 96);
            this.lo_LogIn.Name = "lo_LogIn";
            this.lo_LogIn.Size = new System.Drawing.Size(436, 84);
            this.lo_LogIn.TextSize = new System.Drawing.Size(0, 0);
            this.lo_LogIn.TextVisible = false;
            // 
            // lo_User
            // 
            this.lo_User.Control = this.sluUser;
            this.lo_User.CustomizationFormText = "用户";
            this.lo_User.Location = new System.Drawing.Point(0, 0);
            this.lo_User.Name = "lo_User";
            this.lo_User.Size = new System.Drawing.Size(436, 48);
            this.lo_User.Text = "用户";
            this.lo_User.TextSize = new System.Drawing.Size(36, 22);
            // 
            // lo_Password
            // 
            this.lo_Password.Control = this.tbPassword;
            this.lo_Password.Location = new System.Drawing.Point(0, 48);
            this.lo_Password.Name = "lo_Password";
            this.lo_Password.Size = new System.Drawing.Size(436, 48);
            this.lo_Password.Text = "密码";
            this.lo_Password.TextSize = new System.Drawing.Size(36, 22);
            // 
            // LogIn
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 22F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(466, 210);
            this.Controls.Add(this.layoutControl1);
            this.KeyPreview = true;
            this.Name = "LogIn";
            this.Text = "登陆";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.btLogIn_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tbPassword.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sluUser.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLookUpEdit1View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_LogIn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_User)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_Password)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraEditors.SearchLookUpEdit sluUser;
        private DevExpress.XtraGrid.Views.Grid.GridView searchLookUpEdit1View;
        private DevExpress.XtraLayout.LayoutControlItem lo_User;
        private DevExpress.XtraEditors.SimpleButton btLogIn;
        private DevExpress.XtraLayout.LayoutControlItem lo_LogIn;
        private DevExpress.XtraEditors.TextEdit tbPassword;
        private DevExpress.XtraLayout.LayoutControlItem lo_Password;
    }
}