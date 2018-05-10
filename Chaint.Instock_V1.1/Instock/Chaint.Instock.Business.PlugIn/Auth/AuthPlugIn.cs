using System;
using DevExpress.XtraEditors;
using Chaint.Common.Core;
using Chaint.Common.Core.Const;
using Chaint.Common.BasePlugIn;
using Chaint.Instock.Core;
using Chaint.Common.Core.Utils;
using Chaint.Common.Core.EventArgs;
using Chaint.Instock.Business.View;
using Chaint.Common.ServiceHelper;
using System.Windows.Forms;
using System.Data;
using Chaint.Common.Entity.Utils;
using DevExpress.DataAccess.Sql;
using System.Collections.Generic;
using DevExpress.XtraGrid;
using System.Data.SqlClient;

namespace Chaint.Instock.Business.PlugIns
{
    public class AuthPlugIn : AbstractBillPlugIn
    {
        XtraForm form;
        public AuthPlugIn(AuthView view) : base(view)
        {
            form = this.View.GetControl<XtraForm>(Const_Stock.Base_Form);
        }
        public override void OnBind()
        {
            base.OnBind();
        }
        public override void AfterBind()
        {
            base.AfterBind();
            BindUser();
        }
        public override void ButtonClick(ButtonClickEventArgs e)
        {
            base.ButtonClick(e);
            switch (e.Sender)
            {
                case Const_Auth.Main_ButtonSave:
                    SaveToDB();
                    break;
            }
        }

        public override void DataChanged(DataChangedEventArgs e)
        {
            base.DataChanged(e);
            switch (e.Sender)
            {
                case Const_Auth.Head_Field_FUser:
                    BindAuthInfo();
                    break;
            }
        }
        #region fun
        private void BindAuthInfo()
        {
            string userId = this.View.GetValue<string>(Const_Auth.Head_Field_FUser);
            string sql = @"SELECT * FROM T_AUTOSCAN_AUTH WHERE FUSERID =@FUSERID";
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@FUSERID", DbType.String));
            param[0].Value = userId;
            string[] tableName = new string[] { "T1" };
            OperateResults result = DBAccessServiceHelper.ExcuteQuery(this.Context, sql, tableName, param.ToArray());
            if (!result.IsSuccess)
            {
                ChaintMessageBox.Show("发生错误,请重试!");
            }
            DataTable dt = new DataTable();
            if (result.ResultData.Tables["T1"].Rows.Count <= 0)
            {
                dt = GetInitData(userId);
            }
            else
            {
                dt = result.ResultData.Tables["T1"];
            }
            GridControl gvMain = this.View.GetControl<GridControl>(Const_Auth.Main_EntryControl);
            gvMain.DataSource = dt;
            this.View.Model.BindEntryData(Const_Auth.Entry_TableName, dt);
        }
        /// <summary>
        ///保存数据到数据库
        /// </summary>
        private void SaveToDB()
        {
            OperationResult result = this.View.Model.Save();
            if (result.IsSuccess)
            {
                this.View.Model.IsDirty = false;
                ChaintMessageBox.Show("保存成功！");
            }
            else
            {
                ChaintMessageBox.Show("保存失败,请查看日志!");
            }
        }
        private DataTable GetInitData(string userId)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(Const_Auth.Entry_Column_FCheck, typeof(int));
            dt.Columns.Add(Const_Auth.Entry_Column_FEntryId, typeof(string));
            dt.Columns.Add(Const_Auth.Entry_Column_FUserID, typeof(string));
            dt.Columns.Add(Const_Auth.Entry_Column_FFormId, typeof(string));
            dt.Columns.Add(Const_Auth.Entry_Column_FFormName, typeof(string));
            List<Tuple<string, string>> formInfo = new List<Tuple<string, string>>();
            formInfo.Add(new Tuple<string, string>(Const_Option.Const_Stock, "仓库"));
            formInfo.Add(new Tuple<string, string>(Const_Option.Const_StockArea, "库区划分"));
            foreach (var item in formInfo)
            {
                DataRow dRow = dt.NewRow();
                dRow[Const_Auth.Entry_Column_FCheck] = 0;
                dRow[Const_Auth.Entry_Column_FEntryId] = SequenceGuid.NewGuid().ToString();
                dRow[Const_Auth.Entry_Column_FUserID] = userId;
                dRow[Const_Auth.Entry_Column_FFormId] = item.Item1;
                dRow[Const_Auth.Entry_Column_FFormName] = item.Item2;
                dt.Rows.Add(dRow);
            }
            return dt;
        }
        private void BindUser()
        {
            OperateResult result =
                        EmployeeServiceHelper.GetUserInfo(this.Context, "");
            if (result.ResultTable.IsEmpty()) return;
            DataTable dtSource = new DataTable();
            dtSource.Columns.Add("UserCode", typeof(string));
            dtSource.Columns.Add("UserName", typeof(string));
            dtSource.Columns["UserCode"].Caption = "编号";
            dtSource.Columns["UserName"].Caption = "名称";
            foreach (var row in result.ResultTable)
            {
                DataRow dRow = dtSource.NewRow();
                dRow["UserCode"] = Convert.ToString(row["USERCODE"]);
                dRow["UserName"] = Convert.ToString(row["USERNAME"]);
                dtSource.Rows.Add(dRow);
            }
            DataSource ds = new DataSource();
            ds.DisplayMember = "UserName";
            ds.ValueMember = "UserCode";
            ds.Data = dtSource;
            var userField = this.View.GetControl<SearchLookUpEdit>(Const_Auth.Head_Field_FUser);
            userField.Bind(ds);
        }
        #endregion
    }
}
