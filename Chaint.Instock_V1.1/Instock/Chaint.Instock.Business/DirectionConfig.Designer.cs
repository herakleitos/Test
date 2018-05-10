namespace Chaint.Instock.Business
{
    partial class DirectionConfig
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
            this.filterBox = new DevExpress.XtraFilterEditor.FilterEditorControl();
            this.btConfirm = new DevExpress.XtraEditors.SimpleButton();
            this.SuspendLayout();
            // 
            // filterBox
            // 
            this.filterBox.AppearanceEmptyValueColor = System.Drawing.Color.Empty;
            this.filterBox.AppearanceFieldNameColor = System.Drawing.Color.Empty;
            this.filterBox.AppearanceGroupOperatorColor = System.Drawing.Color.Empty;
            this.filterBox.AppearanceOperatorColor = System.Drawing.Color.Empty;
            this.filterBox.AppearanceValueColor = System.Drawing.Color.Empty;
            this.filterBox.Location = new System.Drawing.Point(12, 22);
            this.filterBox.Name = "filterBox";
            this.filterBox.Size = new System.Drawing.Size(628, 410);
            this.filterBox.TabIndex = 0;
            this.filterBox.Text = "立库去向过滤条件";
            this.filterBox.UseMenuForOperandsAndOperators = false;
            // 
            // btConfirm
            // 
            this.btConfirm.Appearance.Font = new System.Drawing.Font("Tahoma", 13F);
            this.btConfirm.Appearance.Options.UseFont = true;
            this.btConfirm.Location = new System.Drawing.Point(417, 466);
            this.btConfirm.Name = "btConfirm";
            this.btConfirm.Size = new System.Drawing.Size(223, 52);
            this.btConfirm.TabIndex = 1;
            this.btConfirm.Text = "确定";
            this.btConfirm.Click += new System.EventHandler(this.btConfirm_Click);
            // 
            // DirectionConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(673, 569);
            this.Controls.Add(this.btConfirm);
            this.Controls.Add(this.filterBox);
            this.Name = "DirectionConfig";
            this.Text = "人工仓去向配置 ";
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraFilterEditor.FilterEditorControl filterBox;
        private DevExpress.XtraEditors.SimpleButton btConfirm;
    }
}