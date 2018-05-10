namespace CTWH.StockReport
{
    partial class Rep_StockOutLink_International_Sub
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

        #region Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.subRep2 = new DevExpress.XtraReports.UI.XRSubreport();
            this.subRep1 = new DevExpress.XtraReports.UI.XRSubreport();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.subRep2,
            this.subRep1});
            this.Detail.Dpi = 254F;
            this.Detail.HeightF = 1497.753F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 254F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // TopMargin
            // 
            this.TopMargin.Dpi = 254F;
            this.TopMargin.HeightF = 70F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 254F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.Dpi = 254F;
            this.BottomMargin.HeightF = 70F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 254F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // subRep2
            // 
            this.subRep2.Dpi = 254F;
            this.subRep2.LocationFloat = new DevExpress.Utils.PointFloat(0.0001211166F, 943.4807F);
            this.subRep2.Name = "subRep2";
            this.subRep2.ReportSource = new CTWH.StockReport.Rep_StockOutLink_International();
            this.subRep2.SizeF = new System.Drawing.SizeF(1961F, 554.2725F);
            // 
            // subRep1
            // 
            this.subRep1.Dpi = 254F;
            this.subRep1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.subRep1.Name = "subRep1";
            this.subRep1.ReportSource = new CTWH.StockReport.Rep_StockOutLink_International();
            this.subRep1.SizeF = new System.Drawing.SizeF(1961F, 521.335F);
            // 
            // Rep_StockOutLink_International_Sub
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin});
            this.Dpi = 254F;
            this.Margins = new System.Drawing.Printing.Margins(70, 70, 70, 70);
            this.PageHeight = 2970;
            this.PageWidth = 2100;
            this.PaperKind = System.Drawing.Printing.PaperKind.A4;
            this.ReportUnit = DevExpress.XtraReports.UI.ReportUnit.TenthsOfAMillimeter;
            this.RequestParameters = false;
            this.ShowPrintStatusDialog = false;
            this.SnapGridSize = 31.75F;
            this.Version = "13.2";
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        public DevExpress.XtraReports.UI.XRSubreport subRep1;
        public DevExpress.XtraReports.UI.XRSubreport subRep2;
    }
}
