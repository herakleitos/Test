using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace CTWH.StockOut
{
    public partial class U_StockOutScanTab : DevExpress.XtraEditors.XtraUserControl
    {
        private MainLayout mainLayout;

        public U_StockOutScanTab()
        {
            InitializeComponent();
        }

        public U_StockOutScanTab(MainLayout mainLayout)
        {
            // TODO: Complete member initialization
            InitializeComponent();
            this.mainLayout = mainLayout;
        }

        private void U_StockOutScanTab_Load(object sender, EventArgs e)
        {
            
            //加载扫描入库
            StockOut.U_StockOutScan sisr = new StockOut.U_StockOutScan();
            this.pg_Scan.Controls.Add(sisr);
            sisr.Dock = DockStyle.Fill;

            //加载入库明细
            StockOut.U_StockOutDetail sid = new StockOut.U_StockOutDetail();
            this.pg_Detail.Controls.Add(sid);
            sid.Dock = DockStyle.Fill;

            //加载入库统计
            StockOut.U_StockOutStat sis = new StockOut.U_StockOutStat();
            this.pg_Stat.Controls.Add(sis);
            sis.Dock = DockStyle.Fill;
        }
    }
}
