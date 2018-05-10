using System;
using System.Data;
using System.Drawing;
using Chaint.Common.Core.Utils;
using Chaint.Common.Core;
using Chaint.Common.ServiceHelper;
using Chaint.Common.Entity.Utils;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Chaint.Instock.Main
{
    public partial class LogIn : DevExpress.XtraEditors.XtraForm
    {
        private Context Context;
        public delegate void GetUserInfoDelegate(string userId, string userName, string password);
        public event GetUserInfoDelegate GetUserInfoEvent;
        public LogIn(Context ctx)
        {
            Context = ctx;
            InitializeComponent();
            BindUser();
        }
        private void BindUser()
        {
            OperateResult result =
                        EmployeeServiceHelper.GetUserInfo(this.Context, "");
            DataTable dtSource = new DataTable();
            dtSource.Columns.Add("UserCode", typeof(string));
            dtSource.Columns.Add("UserName", typeof(string));
            dtSource.Columns["UserCode"].Caption = "编号";
            dtSource.Columns["UserName"].Caption = "名称";
            if (!result.ResultTable.IsEmpty())
            {
                foreach (var row in result.ResultTable)
                {
                    DataRow dRow = dtSource.NewRow();
                    dRow["UserCode"] = Convert.ToString(row["USERCODE"]);
                    dRow["UserName"] = Convert.ToString(row["USERNAME"]);
                    dtSource.Rows.Add(dRow);
                }
            }
            DataRow dRowAdmin = dtSource.NewRow();
            dRowAdmin["UserCode"] = "Admin";
            dRowAdmin["UserName"] = "管理员";
            dtSource.Rows.Add(dRowAdmin);
            DataSource ds = new DataSource();
            ds.DisplayMember = "UserName";
            ds.ValueMember = "UserCode";
            ds.Data = dtSource;
            sluUser.Bind(ds);
        }

        private void btLogIn_KeyDown(object sender,KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.btLogIn_Click(sender,null);
            }
        }
        private void btLogIn_Click(object sender, EventArgs e)
        {
            string userCode = Convert.ToString(sluUser.EditValue);
            string userName = sluUser.Text;
            string password = tbPassword.Text.ToString();
            bool isVal = true;
            if (userCode == "Admin")
            {
                isVal = ValidateAdmin(userCode, password);
            }
            else
            {
                isVal = EmployeeServiceHelper.ValidateUser(this.Context, userCode, password);
            }
            if (!isVal)
            {
                ChaintMessageBox.Show("用户名或密码错误，请重试!");
                return;
            }
            else
            {
                GetUserInfoEvent(userCode, userName, password);
                this.Close();
            }
        }
        private bool ValidateAdmin(string userCode,string password)
        {
            string sql = @"SELECT FUSERID,FPASSWORD FROM T_AUTOSCAN_PASSWORD WHERE FUSERID =@FUSERID AND FPASSWORD=
                            @FPASSWORD";
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@FUSERID", DbType.String));
            param.Add(new SqlParameter("@FPASSWORD", DbType.String));
            param[0].Value = userCode;
            param[1].Value = password;
            string[] tableName = new string[] { "T1" };
            OperateResults result = DBAccessServiceHelper.ExcuteQuery(this.Context, sql, tableName, param.ToArray());
            if (!result.IsSuccess|| result.ResultData.Tables.Count<=0
                || result.ResultData.Tables["T1"].Rows.Count<=0)
            {
                return false;
            }
            return true;
        }
    }
}