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

namespace CTWH.StockManage
{
    public partial class U_WarehouseMapping : DevExpress.XtraEditors.XtraUserControl
    {
        CTWH.Common.MSSQL.WMSAccess _WMSAccess;

        public U_WarehouseMapping()
        {
            InitializeComponent();
        }

        private void btn_Refresh_Click(object sender, EventArgs e)
        {
           InterfaceDS iDS= this._WMSAccess.Select_T_Factory_Warehouse();
           this.grid_CT.DataSource = iDS.T_Factory_Warehouse; 
        }

        private void U_OrgMapping_Load(object sender, EventArgs e)
        {
            _WMSAccess = Utils.WMSSqlAccess;

            //加载erp组织
            //刷新库存组织
            InterfaceDS ids = this._WMSAccess.Select_CT_OrgInfo("");
            this.look_ERP.Properties.DataSource = ids.CT_OrgInfo;
            this.look_ERP.Properties.DisplayMember = "ORGName";// ids.CT_OrgInfo.ORGCodeColumn.ColumnName;
            this.look_ERP.Properties.ValueMember = "ORGCode";// ids.CT_OrgInfo.ORGNameColumn.ColumnName;
            this.btn_Refresh_Click(null,null);
        }

       

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            //string onlyid = this.gridView1.GetFocusedRowCellDisplayText("OnlyID");
            string orgcode = this.gridView1.GetFocusedRowCellDisplayText("OrgCode");
            string orgname = this.gridView1.GetFocusedRowCellDisplayText("OrgName");
            string ckcode = this.gridView1.GetFocusedRowCellDisplayText("CKCode");
            string ckname = this.gridView1.GetFocusedRowCellDisplayText("CKName");


            string factoryaddr = this.gridView1.GetFocusedRowCellDisplayText("FactoryAddr");
            string factoryPhone = this.gridView1.GetFocusedRowCellDisplayText("FactoryPhone");
            string ischoose = this.gridView1.GetFocusedRowCellDisplayText("IsChoose");
            string islocal = this.gridView1.GetFocusedRowCellDisplayText("IsLocal");

            this.txt_ORGCode.Text = orgcode;
            this.txt_Addr.Text = factoryaddr;
            this.txt_Code.Text = ckcode;
            this.txt_Name.Text = ckname;
            this.look_ERP.Text = orgname;
            this.txt_Phone.Text = factoryPhone;
            this.chk_IsUse.Checked = ischoose == "Checked" ? true : false;
            this.chk_IsLocal.Checked = islocal == "Checked" ? true : false;
            //this.txt_ID.Text = onlyid;
        }

        private void btn_Mapping_Click(object sender, EventArgs e)
        {
            //保存
            WMSDS.T_Factory_WarehouseRow frow = new WMSDS().T_Factory_Warehouse.NewT_Factory_WarehouseRow();

            //WMSDS.T_FactoryRow frow = new WMSDS().T_Factory.NewT_FactoryRow();
           frow.LSBH =Convert.ToInt32(this.gridView1.GetFocusedRowCellDisplayText("LSBH").Trim());
           frow.OrgCode = txt_ORGCode.Text.Trim();
           frow.OrgName = look_ERP.Text.Trim();
           frow.CKCode = txt_Code.Text.Trim();
           frow.CKName = txt_Name.Text.Trim();
           frow.FactoryAddr = txt_Addr.Text.Trim();
           frow.FactoryPhone = txt_Phone.Text.Trim();
           frow.IsChoose = this.chk_IsUse.Checked;
           frow.IsLocal = this.chk_IsLocal.Checked;

           string ret = this._WMSAccess.Update_T_Factory_Warehouse(frow);
           
           if (ret == "")
               MessageBox.Show("保存成功");
           this.btn_Refresh_Click(null,null);
        }
    }
}
