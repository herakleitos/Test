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

namespace CTWH.StockIn
{
    public partial class U_StockInDetail : DevExpress.XtraEditors.XtraUserControl
    {
        CTWH.Common.MSSQL.WMSAccess _WMSAccess;

        public U_StockInDetail()
        {
            InitializeComponent();
        }
        private void DisposeData()
        {
            _WMSAccess.SqlStateChange -= new CTWH.Common.MSSQL.WMSAccess.SqlStateEventHandler(access_SqlStateChange);
            _WMSAccess = null;
        }

        private void U_StockInDetail_Load(object sender, EventArgs e)
        {
            _WMSAccess = Utils.WMSSqlAccess;
            _WMSAccess.SqlStateChange += new CTWH.Common.MSSQL.WMSAccess.SqlStateEventHandler(access_SqlStateChange);
            //加载人员
            LoadT_Factory();
            LoadT_User();

            this.date_Start.DateTime = DateTime.Now.AddDays(-1);
            this.date_End.DateTime = DateTime.Now;

            this.FConfirmSDate.DateTime = DateTime.Now.AddDays(-1);
            this.FConfirmEDate.DateTime = DateTime.Now;
        }

        private void LoadT_Factory()
        {
            WMSDS wmsDS = this._WMSAccess.Select_T_Factory(true, false);

            wmsDS.T_Factory.Rows.Add(wmsDS.T_Factory.NewT_FactoryRow());
            this.cmb_Factory.Properties.DataSource = wmsDS.T_Factory;
            this.cmb_Factory.Properties.ValueMember = "OnlyID";
            this.cmb_Factory.Properties.DisplayMember = "MachineID";
        }
        private void LoadT_User()
        {
            WMSDS wmsDS = this._WMSAccess.Select_T_User("");
            wmsDS.T_User.Rows.Add(wmsDS.T_User.NewT_UserRow());

            this.cmb_Emp.Properties.DataSource = wmsDS.T_User;
            this.cmb_Emp.Properties.ValueMember = "UserCode";
            this.cmb_Emp.Properties.DisplayMember = "UserName";
        }
        void access_SqlStateChange(object sender, SqlStateEventArgs e)
        {
            if (e.IsConnect == false)
            {
                Utils.WriteTxtLog(Utils.FilePath_txtMSSQLLog, "DataBase Error:" + e.Info);
            }
        }

        private void btn_Search_Click(object sender, EventArgs e)
        {
            string factory = this.cmb_Factory.Text.Trim();
            factory = factory == "全部" ? "" : factory;
            string user = this.cmb_Emp.EditValue == null ? "全部" : this.cmb_Emp.EditValue.ToString();
            user = user == "全部" ? "" : user;

            string dateS = this.date_Start.DateTime.ToString("yyyy-MM-dd HH:mm:ss");
            string dateE = this.date_End.DateTime.ToString("yyyy-MM-dd HH:mm:ss");
            string confirmDateS = this.FConfirmSDate.DateTime.ToString("yyyy-MM-dd HH:mm:ss");
            string confirmDateE = this.FConfirmEDate.DateTime.ToString("yyyy-MM-dd HH:mm:ss");
            bool isConfirm = this.FIsConfirm.Checked;
            string pType = "";
            WMSDS wms = this._WMSAccess.Select_T_Product_InForDetail(factory, user, dateS, dateE, confirmDateS, confirmDateE,isConfirm, pType);
            this.grid_InDetail.DataSource = wms.T_Product_In;
            //this.gridView1.BestFitColumns();
        }

        private void btn_Export_Click(object sender, EventArgs e)
        {
            SaveFileDialog sbd = new SaveFileDialog();
            sbd.Filter = "Excel 文件 (*.xls)|*.xls";
            sbd.FileName = "入库明细数据" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
            DialogResult res = sbd.ShowDialog();
            if (DialogResult.OK == res || DialogResult.Yes == res)
            {
                this.grid_InDetail.ExportToXls(sbd.FileName);
            }
            //if (sbd.ShowDialog() == DialogResult.OK || sbd.ShowDialog() == DialogResult.Yes)
            //    this.grid_InDetail.ExportToXls(sbd.FileName);
        }

        private void btn_Update_Click(object sender, EventArgs e)
        {
            //得到要更新的ID
            int[] ids = this.gridView1.GetSelectedRows();
            if (ids.Length > 200)
            {
                MessageBox.Show("修改失败，多选条码不能超过200条。");
                return;
            }
            else if (ids.Length == 0)
            {
                MessageBox.Show("修改失败，请选择要修改的条码。");
                return;
            }
            else
            {

                F_EditProduct wp = new F_EditProduct();
                if (wp.ShowDialog() == DialogResult.OK)
                {

                    string diameter = wp.Diameter==""?"0":wp.Diameter;
                    string length = wp.Length == "" ? "0" : wp.Length;
                    string package = wp.Package;
                    string customer = wp.Customer;
                    string remark = wp.Remark;
                    string slidesofReam = wp.SlidesOfReam;
                    string slidesofSheet = wp.SlidesOfSheet;
                    string color = wp.Color;

                    string idstr = "";

                    //循环组合id
                    foreach (int id in ids)
                    {
                        string onlyid = this.gridView1.GetRowCellDisplayText(id, "OnlyID");

                        idstr += onlyid.ToString() + ",";
                    }
                    idstr = idstr.TrimEnd(',');
                    //string barcode=   this.gridView1.GetRowCellDisplayText(id,"ProductID");

                    //WMSDS wmsds = new WMSDS();
                    //WMSDS.T_Product_InRow tpirow = wmsds.T_Product_In.NewT_Product_InRow();
                    //tpirow.OnlyID = Convert.ToInt32(onlyid);
                    //   if(diameter!="-1")
                    //tpirow.DiameterLabel = Convert.ToDecimal(diameter);
                    //   if (length != "-1")
                    //tpirow.LengthLabel =Convert.ToDecimal( length);
                    //   if (package != "-1")
                    //tpirow.IsPolyHook = package;
                    //   if (customer != "-1")
                    //tpirow.CustTrademark = customer;
                    //   if (remark != "-1")
                    //tpirow.WHRemark = remark;
                    //  string ret= this._WMSAccess.Update_T_Product_InByRow(tpirow);
                    string ret = this._WMSAccess.Update_T_Product_InByIDs(diameter, length, package, customer, remark, idstr,slidesofReam,slidesofSheet,color);
                    if (ret != "")
                    {
                        MessageBox.Show("批量修改失败！" + ret);
                    }
                }
                this.btn_Search_Click(null,null);
                //this.btnRecordRefresh.Enabled = false;
                //waitdig = new DevExpress.Utils.WaitDialogForm(null, "数据正在更新！请稍后...");

                //BackgroundWorker bgWHPEdit = new BackgroundWorker();
                //bgWHPEdit.DoWork += new DoWorkEventHandler(bgWHPEdit_DoWork);

                //bgWHPEdit.RunWorkerAsync(wprow);

            }
        }
    }


}

       
    
