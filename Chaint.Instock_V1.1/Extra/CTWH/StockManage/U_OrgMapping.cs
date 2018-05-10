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
    public partial class U_OrgMapping : DevExpress.XtraEditors.XtraUserControl
    {
        CTWH.Common.MSSQL.WMSAccess _WMSAccess;

        public U_OrgMapping()
        {
            InitializeComponent();
        }

        private void btn_Refresh_Click(object sender, EventArgs e)
        {
           WMSDS wmsDS= this._WMSAccess.Select_T_Factory("");
           this.grid_CT.DataSource = wmsDS.T_Factory; 
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
            string onlyid = this.gridView1.GetFocusedRowCellDisplayText("OnlyID");
            string factoryname = this.gridView1.GetFocusedRowCellDisplayText("FactoryName");
            string factoryaddr = this.gridView1.GetFocusedRowCellDisplayText("FactoryAddr");
            string factoryabbr = this.gridView1.GetFocusedRowCellDisplayText("FactoryAbbr");
            string factoryPhone = this.gridView1.GetFocusedRowCellDisplayText("FactoryPhone");

            string machineid = this.gridView1.GetFocusedRowCellDisplayText("MachineID");
            string ischoose = this.gridView1.GetFocusedRowCellDisplayText("IsChoose");
            string islocal = this.gridView1.GetFocusedRowCellDisplayText("IsLocal");
            this.txt_Addr.Text = factoryaddr;
            this.txt_Code.Text = machineid;
            this.txt_Name.Text = factoryname;
            this.look_ERP.EditValue = factoryabbr;
            this.txt_Phone.Text = factoryPhone;
            this.chk_IsUse.Checked = ischoose=="Checked"?true:false;
            this.chk_IsLocal.Checked = islocal=="Checked"?true:false;
            this.txt_ID.Text = onlyid;
        }

        private void btn_Mapping_Click(object sender, EventArgs e)
        {
            //保存
            WMSDS.T_FactoryRow frow = new WMSDS().T_Factory.NewT_FactoryRow();
            frow.OnlyID =Convert.ToInt32(this.txt_ID.Text.Trim());
            frow.MachineID = this.txt_Code.Text.Trim();
            frow.IsChoose =this.chk_IsUse.Checked;
            frow.IsLocal =this.chk_IsLocal.Checked;
            frow.FactoryAbbr =this.look_ERP.EditValue.ToString();
            frow.FactoryAddr = this.txt_Addr.Text.Trim();
            frow.FactoryName = this.txt_Name.Text.Trim();
            frow.FactoryPhone = this.txt_Phone.Text.Trim();

            //frow.FactoryPhone = "";
           string ret= this._WMSAccess.Update_T_Factory(frow);
           if (ret == "")
               MessageBox.Show("保存成功");
           this.btn_Refresh_Click(null,null);
        }
    }
}
