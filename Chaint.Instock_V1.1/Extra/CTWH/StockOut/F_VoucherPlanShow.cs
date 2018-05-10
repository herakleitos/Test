using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DataModel;

namespace CTWH.StockOut
{
    public partial class F_VoucherPlanShow : DevExpress.XtraEditors.XtraForm
    {
        private CTWH.Common.MSSQL.WMSAccess access = new CTWH.Common.MSSQL.WMSAccess();
        public F_VoucherPlanShow()
        {
            InitializeComponent();
        }

        public void InitData(WMSDS.T_OutStock_PlanDataTable wvtable)
        {

            this.gridControl1.DataSource = wvtable;
            this.gridView1.BestFitColumns();
        }
        public string GetSelectVoucherNO()
        {
            return this.gridView1.GetFocusedRowCellDisplayText(this.colVoucherNO);
        }

        public WMSDS.T_OutStockRow GetWarehouse_VoucherRow()
        {

            return (this.gridView1.GetFocusedDataRow() as WMSDS.T_OutStockRow);
        }

        private void VoucherPlanShow_Load(object sender, EventArgs e)
        {


            this.dateEdit2.DateTime = DateTime.Now;
            this.dateEdit1.DateTime = DateTime.Now.AddHours(-24);
            this.simpleButton1_Click(sender, null);
            this.gridControl1.Focus();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //MainDS ds = access.Warehouse_VoucherQueryByFK("", this.dateEdit1.DateTime.ToString("yyyy-MM-dd") + " 00:00:00", this.dateEdit2.DateTime.ToString("yyyy-MM-dd") + " 23:59:59", "", "", "", "", -1, -1, 0, -1);
            WMSDS wmsDS = access.Select_T_OutStockByFK("", this.dateEdit1.DateTime.ToString("yyyy-MM-dd HH:mm:ss"), this.dateEdit2.DateTime.ToString("yyyy-MM-dd HH:mm:ss") , "", "", "", "", -1, -1, -1);
            this.InitData(wmsDS.T_OutStock);
        }

        private void InitData(WMSDS.T_OutStockDataTable t_OutStockDataTable)
        {
            this.gridControl1.DataSource = t_OutStockDataTable;
            this.gridView1.BestFitColumns();
        }

        
    }
}