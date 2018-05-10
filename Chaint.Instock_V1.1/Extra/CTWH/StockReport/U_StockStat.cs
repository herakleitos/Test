using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using CTWH.Common;
using DataModel;
namespace CTWH.StockReport
{
    public partial class U_StockStat : DevExpress.XtraEditors.XtraUserControl
    {
        CTWH.Common.MSSQL.WMSAccess _WMSAccess;

        public U_StockStat()
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
        private void U_StockDetail_Load(object sender, EventArgs e)
        {
            _WMSAccess = Utils.WMSSqlAccess;
            _WMSAccess.SqlStateChange += new CTWH.Common.MSSQL.WMSAccess.SqlStateEventHandler(access_SqlStateChange);
            this.dt_End.DateTime = DateTime.Now;
            this.dt_Start.DateTime = DateTime.Now.AddDays(-7);
        }
        private void btn_Query_Click(object sender, EventArgs e)
        {
            string dateS = this.dt_Start.EditValue.ToString();
            string dateE = this.dt_End.EditValue.ToString();
            DataSet detailDS = this._WMSAccess.Select_StockStat(dateS,dateE);
            this.grid_Detail.DataSource = detailDS.Tables["T_StockStat"];
            //   int a = detailDS.Tables["T_StockDetail"].Rows.Count;
            //this.gridView1.BestFitColumns();
        }

    }
}
