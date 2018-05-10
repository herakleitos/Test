using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DataModel;

namespace CTWH.StockIn
{
    public partial class F_EditProduct : DevExpress.XtraEditors.XtraForm
    {
        public F_EditProduct()
        {
            InitializeComponent();

        }

        //public SQLAccess access = null;
        public CTWH.Common.MSSQL.WMSAccess _WMSAccess;


        //public MainDS.Warehouse_ProductRow GetPageRow()
        //{

        //    MainDS.Warehouse_ProductDataTable wptb = new MainDS.Warehouse_ProductDataTable();
        //    MainDS.Warehouse_ProductRow row = wptb.NewWarehouse_ProductRow();
        //    if (this.chkBrand.Checked)
        //        row.Brand = this.txtBrand.Text.Trim();
        //    if (this.chkContract.Checked)
        //        row.Contract = this.txtContract.Text.Trim();
        //    if (this.chkPalletType.Checked)
        //        row.PalletType = this.cbPalletType.Text.Trim();
        //    if (this.chkPosition.Checked)
        //        row.Position = this.txtPosition.Text.Trim();
        //    if (this.chkRemark.Checked)
        //        row.Remark1 = this.txtRemark.Text.Trim();
        //    if (this.chkSourceType.Checked)
        //        row.SourceType = this.cbSourceType.Text.Trim();
        //    if (this.chkWidthMode.Checked)
        //        row.WidthMode = this.cbWidthMode.SelectedIndex.ToString();
        //    //if(this.chkInspector.Checked)
        //    //    row.ins
        //    return row;

        //}












        public string Diameter
        {

            get { return !this.chk_Diameter.Checked ? "-1" : (this.cmb_Diameter.Text.Trim() == "" ? "0" : this.cmb_Diameter.Text.Trim()); }
        }
        public string Length
        {
            get { return !this.chk_Length.Checked ? "-1" : (this.cmb_Length.Text.Trim() == "" ? "0" : this.cmb_Length.Text.Trim()); }
        }
        public string Package
        {
            get { return !this.chk_Package.Checked ? "-1" : this.cmb_Package.Text.Trim(); }
        }

        public string Customer
        {
            get { return !this.chk_Customer.Checked ? "-1" : this.cmb_Customer.Text.Trim(); }
        }

        public string Remark
        {

            get { return !this.chk_Remark.Checked ? "-1" : this.cmb_Remark.Text.Trim(); }
        }

        public string SlidesOfReam
        {

            get { return !this.chk_SlidesOfReam.Checked ? "-1" : this.cmb_SlidesOfReam.Text.Trim(); }
        }

        public string SlidesOfSheet
        {

            get { return !this.chk_SlidesOfSheet.Checked ? "-1" : this.cmb_SlidesOfSheet.Text.Trim(); }
        }

        public string Color
        {

            get { return !this.chk_Color.Checked ? "-1" : this.cmb_Color.Text.Trim(); }
        }

        private void Form_EditProduct_Load(object sender, EventArgs e)
        {
            this.InitData();
        }
        /// <summary>
        /// 初始化
        /// </summary>
        private void InitData()
        {
            ///hm  加载数据库中的各种信息

            this._WMSAccess = new  CTWH.Common.MSSQL.WMSAccess();
            LoadPlyHookType();
            LoadSpecialCustomer();
            LoadColor();
        }
        /// <summary>
        /// 加载夹板方式
        /// </summary>
        private void LoadPlyHookType()
        {
            InterfaceDS iDS = this._WMSAccess.Select_CT_ZDYZD("JBBZ");
            for (int i = 0; i < iDS.CT_ZDYZD.Rows.Count; i++)
                this.cmb_Package.Properties.Items.Add(iDS.CT_ZDYZD.Rows[i]["Name"].ToString());

        }
        /// <summary>
        /// 加载特殊客户
        /// </summary>
        private void LoadSpecialCustomer()
        {
            InterfaceDS iDS = this._WMSAccess.Select_CT_ZDYZD("TSKH");
            for (int i = 0; i < iDS.CT_ZDYZD.Rows.Count; i++)
                this.cmb_Customer.Properties.Items.Add(iDS.CT_ZDYZD.Rows[i]["Name"].ToString());

        }

        /// <summary>
        /// 加载色相
        /// </summary>
        private void LoadColor()
        {
            InterfaceDS iDS = this._WMSAccess.Select_CT_ZDYZD("CPSX");
            for (int i = 0; i < iDS.CT_ZDYZD.Rows.Count; i++)
                this.cmb_Color.Properties.Items.Add(iDS.CT_ZDYZD.Rows[i]["Name"].ToString());

        }

        private void chk_Diameter_CheckedChanged(object sender, EventArgs e)
        {
            this.cmb_Diameter.Enabled = this.chk_Diameter.Checked;
        }

        private void chk_Length_CheckedChanged(object sender, EventArgs e)
        {
            this.cmb_Length.Enabled = this.chk_Length.Checked;
        }

        private void chk_Package_CheckedChanged(object sender, EventArgs e)
        {
            this.cmb_Package.Enabled = this.chk_Package.Checked;
        }

        private void chk_Customer_CheckedChanged(object sender, EventArgs e)
        {
            this.cmb_Customer.Enabled = this.chk_Customer.Checked;
            
        }

        private void chkRemark_CheckedChanged_1(object sender, EventArgs e)
        {
            this.cmb_Remark.Enabled = this.chk_Remark.Checked;
        }




    }
}