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
    public partial class U_UserManager : DevExpress.XtraEditors.XtraUserControl
    {
        CTWH.Common.MSSQL.WMSAccess _WMSAccess;

        public U_UserManager()
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

        private void U_UserManager_Load(object sender, EventArgs e)
        {
            _WMSAccess = Utils.WMSSqlAccess;
            _WMSAccess.SqlStateChange += new CTWH.Common.MSSQL.WMSAccess.SqlStateEventHandler(access_SqlStateChange);

            //加载班组
         WMSDS shiftDS =   this._WMSAccess.Select_T_Shift("");
         this.cmb_Shift.Properties.DataSource = shiftDS.T_Shift;
         this.cmb_Shift.Properties.DisplayMember = "ShiftName";
         this.cmb_Shift.Properties.ValueMember = "ShiftCode";
         //加载USERTYPE
         WMSDS userDS = this._WMSAccess.Select_T_UserType("");
         this.cmb_UserType.Properties.DataSource = userDS.T_User_Type;
         this.cmb_UserType.Properties.DisplayMember = "UserTypeName";
         this.cmb_UserType.Properties.ValueMember = "UserTypeCode";


         LoadAllUser();

        }
        private void LoadAllUser() {
            //显示所有的用户
            DataSet _UserDS = _WMSAccess.Select_T_UserForList("");
            if (_UserDS.Tables.Count > 0 && _UserDS.Tables["T_User"].Rows.Count > 0)
                this.grid_User.DataSource = _UserDS.Tables["T_User"];
        }
        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (this.gridView1.FocusedValue !=null)
            {
                this.cmb_Shift.EditValue = this.gridView1.GetFocusedRowCellValue("ShiftID").ToString();
                this.cmb_UserType.EditValue = this.gridView1.GetFocusedRowCellValue("UserTypeID").ToString();
                this.txt_UserCode.Text = this.gridView1.GetFocusedRowCellValue("UserCode").ToString();

                this.txt_UserName.Text = this.gridView1.GetFocusedRowCellValue("UserName").ToString();
                this.txt_Password.Text = this.gridView1.GetFocusedRowCellValue("Password").ToString();
            }
        }

        private void btn_Add_Click(object sender, EventArgs e)
        {
            //通过usercode添加或者修改用户
            string usercode =this.txt_UserCode.Text.Trim();
            string username = this.txt_UserName.Text.Trim();
            string password = this.txt_Password.Text.Trim();
            string shift = this.cmb_Shift.EditValue.ToString();
            string usertype = this.cmb_UserType.EditValue.ToString();

            int i = this._WMSAccess.Insert_Update_T_User(usercode, username, password, shift, usertype);
         if (i > -1) 
             MessageBox.Show("保存成功。");
         this.LoadAllUser();
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            string usercode = this.txt_UserCode.Text.Trim();
            string username = this.txt_UserName.Text.Trim();

            if (MessageBox.Show("确实要删除用户：" + username+"吗？","提示信息",MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.Yes) {

                int i = this._WMSAccess.DeleteT_User(usercode);
                if (i > -1)
                    MessageBox.Show("删除成功。");
                this.LoadAllUser();
            }
        }
    }
}
