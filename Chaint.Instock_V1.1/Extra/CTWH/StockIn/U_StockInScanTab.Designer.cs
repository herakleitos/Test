namespace CTWH.StockIn
{
    partial class U_StockInScanTab
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.pg_Scan = new DevExpress.XtraTab.XtraTabPage();
            this.pg_Detail = new DevExpress.XtraTab.XtraTabPage();
            this.pg_Stat = new DevExpress.XtraTab.XtraTabPage();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Appearance.BackColor = System.Drawing.Color.Silver;
            this.xtraTabControl1.Appearance.Options.UseBackColor = true;
            this.xtraTabControl1.AppearancePage.PageClient.BackColor = System.Drawing.Color.Silver;
            this.xtraTabControl1.AppearancePage.PageClient.BorderColor = System.Drawing.Color.Silver;
            this.xtraTabControl1.AppearancePage.PageClient.Options.UseBackColor = true;
            this.xtraTabControl1.AppearancePage.PageClient.Options.UseBorderColor = true;
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl1.Location = new System.Drawing.Point(0, 0);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.pg_Scan;
            this.xtraTabControl1.Size = new System.Drawing.Size(1013, 415);
            this.xtraTabControl1.TabIndex = 0;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.pg_Scan,
            this.pg_Detail,
            this.pg_Stat});
            this.xtraTabControl1.CloseButtonClick += new System.EventHandler(this.xtraTabControl1_CloseButtonClick);
            // 
            // pg_Scan
            // 
            this.pg_Scan.Appearance.PageClient.BackColor = System.Drawing.Color.Gainsboro;
            this.pg_Scan.Appearance.PageClient.Options.UseBackColor = true;
            this.pg_Scan.Name = "pg_Scan";
            this.pg_Scan.Size = new System.Drawing.Size(1007, 386);
            this.pg_Scan.Text = "扫描入库";
            // 
            // pg_Detail
            // 
            this.pg_Detail.Name = "pg_Detail";
            this.pg_Detail.Size = new System.Drawing.Size(1007, 280);
            this.pg_Detail.Text = "入库明细";
            // 
            // pg_Stat
            // 
            this.pg_Stat.Name = "pg_Stat";
            this.pg_Stat.Size = new System.Drawing.Size(1007, 280);
            this.pg_Stat.Text = "入库统计";
            // 
            // U_StockInScan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.xtraTabControl1);
            this.Name = "U_StockInScan";
            this.Size = new System.Drawing.Size(1013, 415);
            this.Load += new System.EventHandler(this.U_StockInScan_Load);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage pg_Scan;
        private DevExpress.XtraTab.XtraTabPage pg_Detail;
        private DevExpress.XtraTab.XtraTabPage pg_Stat;
    }
}
