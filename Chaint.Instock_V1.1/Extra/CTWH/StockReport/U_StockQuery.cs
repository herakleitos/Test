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
    public partial class U_StockQuery : DevExpress.XtraEditors.XtraUserControl
    {
        CTWH.Common.MSSQL.WMSAccess _WMSAccess;

        public U_StockQuery()
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
        }
        private void btn_Query_Click(object sender, EventArgs e)
        {
            //获得productids
            string productids = "";
            foreach (string s in this.txt_productids.Lines)
            {
                if (s.Trim().Length > 0)
                {
                    productids = productids + "'" + s.Trim() + "',";

                }

            }

            if (productids.Length > 0)
            {
                productids = productids.TrimEnd(',');

            }

            DataSet detailDS = this._WMSAccess.Select_StockQuery(productids);
            this.grid_Detail.DataSource = detailDS.Tables["T_StockDetail"];
            //   int a = detailDS.Tables["T_StockDetail"].Rows.Count;
            this.gridView1.BestFitColumns();
        }

    }
}
