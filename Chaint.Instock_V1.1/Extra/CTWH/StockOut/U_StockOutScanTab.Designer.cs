namespace CTWH.StockOut
{
    partial class U_StockOutScanTab
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
            this.mainTab = new DevExpress.XtraTab.XtraTabControl();
            this.pg_Scan = new DevExpress.XtraTab.XtraTabPage();
            this.pg_Detail = new DevExpress.XtraTab.XtraTabPage();
            this.pg_Stat = new DevExpress.XtraTab.XtraTabPage();
            ((System.ComponentModel.ISupportInitialize)(this.mainTab)).BeginInit();
            this.mainTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainTab
            // 
            this.mainTab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTab.Location = new System.Drawing.Point(0, 0);
            this.mainTab.Name = "mainTab";
            this.mainTab.SelectedTabPage = this.pg_Scan;
            this.mainTab.Size = new System.Drawing.Size(884, 406);
            this.mainTab.TabIndex = 0;
            this.mainTab.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.pg_Scan,
            this.pg_Detail,
            this.pg_Stat});
            // 
            // pg_Scan
            // 
            this.pg_Scan.Name = "pg_Scan";
            this.pg_Scan.Size = new System.Drawing.Size(878, 377);
            this.pg_Scan.Text = "扫描出库";
            // 
            // pg_Detail
            // 
            this.pg_Detail.Name = "pg_Detail";
            this.pg_Detail.Size = new System.Drawing.Size(878, 377);
            this.pg_Detail.Text = "出库明细";
            // 
            // pg_Stat
            // 
            this.pg_Stat.Name = "pg_Stat";
            this.pg_Stat.Size = new System.Drawing.Size(878, 377);
            this.pg_Stat.Text = "出库统计";
            // 
            // U_StockOutScanTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mainTab);
            this.Name = "U_StockOutScanTab";
            this.Size = new System.Drawing.Size(884, 406);
            this.Load += new System.EventHandler(this.U_StockOutScanTab_Load);
            ((System.ComponentModel.ISupportInitialize)(this.mainTab)).EndInit();
            this.mainTab.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraTab.XtraTabControl mainTab;
        private DevExpress.XtraTab.XtraTabPage pg_Scan;
        private DevExpress.XtraTab.XtraTabPage pg_Detail;
        private DevExpress.XtraTab.XtraTabPage pg_Stat;
    }
}
