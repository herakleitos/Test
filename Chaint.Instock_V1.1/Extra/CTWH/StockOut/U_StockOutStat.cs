using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using CTWH.Common;
using DataModel;

namespace CTWH.StockOut
{
    public partial class U_StockOutStat : DevExpress.XtraEditors.XtraUserControl
    {

        CTWH.Common.MSSQL.WMSAccess _WMSAccess;

        public U_StockOutStat()
        {
            InitializeComponent();
        }
        private void DisposeData()
        {
            _WMSAccess.SqlStateChange -= new CTWH.Common.MSSQL.WMSAccess.SqlStateEventHandler(access_SqlStateChange);
            _WMSAccess = null;
        }
        void access_SqlStateChange(object sender, SqlStateEventArgs e)
        {
            if (e.IsConnect == false)
            {
                Utils.WriteTxtLog(Utils.FilePath_txtMSSQLLog, "DataBase Error:" + e.Info);
            }
        }
        private void btn_Query_Click(object sender, EventArgs e)
        {
         

            string dateS = this.date_Start.DateTime.ToString("yyyy-MM-dd HH:mm:ss");
            string dateE = this.date_End.DateTime.ToString("yyyy-MM-dd HH:mm:ss");
            string voucherNO = this.chk_Bill.Checked? this.cmb_Voucherno.Text.Trim():"";
            WMSQueryDS wms = this._WMSAccess.Select_T_Product_InForOutStat(dateS, dateE, voucherNO);
            this.grid_OutDetail.DataSource = wms.T_Product_In_Stat;
            //this.gridView1.BestFitColumns();
        }

        private void btn_Export_Click(object sender, EventArgs e)
        {
            SaveFileDialog sbd = new SaveFileDialog();
            sbd.Filter = "Excel 文件 (*.xls)|*.xls";
            sbd.FileName = "出库明细数据" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
            if (sbd.ShowDialog() == DialogResult.OK || sbd.ShowDialog() == DialogResult.Yes)
                this.grid_OutDetail.ExportToXls(sbd.FileName);
        }

        private void U__StockOutStat_Load(object sender, EventArgs e)
        {
            _WMSAccess = Utils.WMSSqlAccess;
            _WMSAccess.SqlStateChange += new CTWH.Common.MSSQL.WMSAccess.SqlStateEventHandler(access_SqlStateChange);

            this.date_End.DateTime = DateTime.Now;
            this.date_Start.DateTime = DateTime.Now.AddDays(-1);
        }
    }
}
