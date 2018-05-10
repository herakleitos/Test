using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace CTWH.StockIn
{
    public partial class U_StockInScanTab : DevExpress.XtraEditors.XtraUserControl
    {
        private MainLayout mainLayout;

        public U_StockInScanTab()
        {
            InitializeComponent();
        }

        public U_StockInScanTab(MainLayout mainLayout)
        {
            InitializeComponent();
            // TODO: Complete member initialization
            this.mainLayout = mainLayout;
        }

        private void U_StockInScan_Load(object sender, EventArgs e)
        {
            //加载扫描入库
            StockIn.U_StockInScan sisr = new StockIn.U_StockInScan(this.mainLayout);
            this.pg_Scan.Controls.Add(sisr);
            sisr.Dock = DockStyle.Fill;

            //加载入库明细
            StockIn.U_StockInDetail sid = new StockIn.U_StockInDetail();
            this.pg_Detail.Controls.Add(sid);
            sid.Dock = DockStyle.Fill;

            //加载入库统计
            StockIn.U_StockInStat sis = new StockIn.U_StockInStat();
            this.pg_Stat.Controls.Add(sis);
            sis.Dock = DockStyle.Fill;



        }

        private void xtraTabControl1_CloseButtonClick(object sender, EventArgs e)
        {
            xtraTabControl1.TabPages.Remove(this.xtraTabControl1.SelectedTabPage);
        }
    }
}
