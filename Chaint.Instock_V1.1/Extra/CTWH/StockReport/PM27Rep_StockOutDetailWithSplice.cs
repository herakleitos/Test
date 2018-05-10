using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace CTWH.StockReport
{
    public partial class PM27Rep_StockOutDetailWithSplice : DevExpress.XtraReports.UI.XtraReport
    {
        public PM27Rep_StockOutDetailWithSplice()
        {
            InitializeComponent();
        }


        internal void SetData(DataModel.WMSReportDS.T_OutStockTitle_Domestic_RepDataTable t_OutStockTitle_Domestic_RepDataTable)
        {
            //if (t_OutStockTitle_Domestic_RepDataTable != null && t_OutStockTitle_Domestic_RepDataTable.Rows.Count > 0) { 
            //this.lbCarrier.Text=t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.CarrierNOColumn].ToString();;
            //    this.lbContainerNO.Text=t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.ContainerNOColumn].ToString();;
            //    this.lbControlNO.Text=t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.ControlNoColumn].ToString();;
            //    this.lbCustomerName.Text=t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.CustomerNameColumn].ToString();;
            //    this.lbDate_Exec.Text=t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.BillDateColumn].ToString();;
            //    this.lbDriverNO.Text=t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.VehicleNOColumn].ToString();;
            //    this.lbForklifter.Text=t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.ForklifterNOColumn].ToString();;
            //    this.lbMachine.Text=t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.MachineIDColumn].ToString();;
            //    this.lbOrderNO.Text=t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.OrderNOColumn].ToString();;
            //    this.lbSealNO.Text=t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.SealNOColumn].ToString();;
            //    this.lbSourceVoucherNO.Text=t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.SourceVoucherNOColumn].ToString();;
            //    //this.lbTitile.Text = t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.].ToString(); ;
            //    this.lbVoucherNO.Text=t_OutStockTitle_Domestic_RepDataTable.Rows[0][t_OutStockTitle_Domestic_RepDataTable.VoucherNOColumn].ToString();
            //}
        }
    }
}
