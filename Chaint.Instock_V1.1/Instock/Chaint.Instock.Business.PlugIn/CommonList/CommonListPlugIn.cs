using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using Chaint.Common.BasePlugIn;
using Chaint.Common.Core.Utils;
using Chaint.Common.Core;
using Chaint.Common.Core.Const;
using Chaint.Instock.Core;
using DevExpress.DataAccess.Sql;
using Chaint.Common.ServiceHelper;
using Chaint.Common.Core.EventArgs;
using Chaint.Instock.Business.View;
using System.Windows.Forms;
using DevExpress.XtraGrid;

namespace Chaint.Instock.Business.PlugIns
{
    public partial class CommonListPlugIn : AbstractBillPlugIn
    {
        XtraForm form;
        string formName = string.Empty;
        public CommonListPlugIn(CommonListView view) : base(view)
        {
            form = this.View.GetControl<XtraForm>(Const_CommonList.Base_Form);
        }
        public override void OnSetBusinessInfo()
        {
            base.OnSetBusinessInfo();
            formName = Convert.ToString(this.Context.GetOption(Const_Option.Const_FormName));
            if (formName.IsNullOrEmptyOrWhiteSpace())
            {
                ChaintMessageBox.Show("发生错误，请重新打开页面!");
                return;
            }
            CreateColumns();
        }
        public override void OnBind()
        {
            base.OnBind();
            DateTime beginDate = DateTime.Now.AddDays(-7);
            DateTime endDate = DateTime.Now;
            this.View.SetValue(Const_CommonList.Query_FBeginDate, beginDate);
            this.View.SetValue(Const_CommonList.Query_FEndDate, endDate);
        }
        public override void AfterBind()
        {
            base.AfterBind();
            QueryData();
        }
        public override void EntryRowClick(EntryRowClickEventArgs e)
        {
            base.EntryRowClick(e);
            GridView gvView = this.View.GetControl<GridView>(Const_CommonList.Main_GridView);
            if (e.ClickTime == 1)
            {
                int oldCheck = this.View.GetValue<int>(Const_CommonList.Main_GridView, "FCHECK", e.RowIndex);
                int newCheck = oldCheck == 1 ? newCheck = 0 : newCheck = 1;
                this.View.SetValue(Const_CommonList.Main_GridView, "FCHECK", newCheck, e.RowIndex);
            }
            if (e.ClickTime == 2)
            {
                string fid = this.View.GetValue<string>(Const_CommonList.Main_GridView, "FID", e.RowIndex);
                OpenBill(fid);
            }
        }
        private void NewBill()
        {
            this.View.Open(formName, null);
        }
        private void OpenBill(string fid)
        {
            if (fid.IsNullOrEmptyOrWhiteSpace()) return;
            if (formName == Const_Option.Const_StockAreaPlan)
            {
                string sql = @" SELECT * FROM T_AUTOSCAN_STOCKAREAPLAN WHERE FID =@FID ";
                SqlParameter para = new SqlParameter("@FID", DbType.String);
                para.Value = fid;
                SqlParameter[] parameters =
                    new SqlParameter[] { para };
                OperateResults result
                    = DBAccessServiceHelper.ExcuteQuery(this.Context, sql, new string[] { "T1" }, parameters);
                if (!result.IsSuccess)
                {
                    ChaintMessageBox.Show("发生错误，请查看日志!");
                    return;
                }
                if (result.ResultData.Tables["T1"].Rows.Count <= 0)
                {
                    ChaintMessageBox.Show("当前库区计划已经被删除，请刷新!");
                    return;
                }
                this.View.Open(formName, result.ResultData);
            }
            else if (formName == Const_Option.Const_StockArea)
            {
                string sql = @" SELECT * FROM T_AUTOSCAN_STOCKAREA WHERE FID =@FID1 ;
                SELECT * FROM T_AUTOSCAN_STOCKAREAENTRY WHERE FHEADID =@FID2 ORDER BY FSEQ;";
                SqlParameter para1 = new SqlParameter("@FID1", DbType.String);
                para1.Value = fid;
                SqlParameter para2 = new SqlParameter("@FID2", DbType.String);
                para2.Value = fid;
                SqlParameter[] parameters =
                    new SqlParameter[] { para1, para2 };
                OperateResults result
                    = DBAccessServiceHelper.ExcuteQuery(this.Context, sql, new string[] { "T1", "T2" }, parameters);
                if (!result.IsSuccess)
                {
                    ChaintMessageBox.Show("发生错误，请查看日志!");
                    return;
                }
                if (result.ResultData.Tables["T1"].Rows.Count <= 0)
                {
                    ChaintMessageBox.Show("当前库区已经被删除，请刷新!");
                    return;
                }
                this.View.Open(formName, result.ResultData);
            }
            else if (formName == Const_Option.Const_Stock)
            {
                string sql = @" SELECT * FROM T_AUTOSCAN_STOCK WHERE FID =@FID ";
                SqlParameter para = new SqlParameter("@FID", DbType.String);
                para.Value = fid;
                SqlParameter[] parameters =
                    new SqlParameter[] { para };
                OperateResults result
                    = DBAccessServiceHelper.ExcuteQuery(this.Context, sql, new string[] { "T1" }, parameters);
                if (!result.IsSuccess)
                {
                    ChaintMessageBox.Show("发生错误，请查看日志!");
                    return;
                }
                if (result.ResultData.Tables["T1"].Rows.Count <= 0)
                {
                    ChaintMessageBox.Show("当前仓库已经被删除，请刷新!");
                    return;
                }
                this.View.Open(formName, result.ResultData);
            }
        }
        public override void ButtonClick(ButtonClickEventArgs e)
        {
            base.ButtonClick(e);
            switch (e.Sender)
            {
                case Const_CommonList.Query_Control_ButtonQuery:
                    QueryData();
                    break;
                case Const_CommonList.Query_Control_ButtonDelete:
                    DeleteData();
                    break;
                case Const_CommonList.Query_Control_ButtonRefresh:
                    QueryData();
                    break;
                case Const_CommonList.Query_Control_ButtonOpen:
                    int rowIndex = this.View.GetSelectedRowIndex(Const_CommonList.Main_GridView,"FCHECK");
                    if (rowIndex < 0)
                    {
                        ChaintMessageBox.Show("请选择一行数据!");
                        return;
                    }
                    string fid = this.View.GetValue<string>(Const_CommonList.Main_GridView, "FID", rowIndex);
                    OpenBill(fid);
                    break;
                case Const_CommonList.Query_Control_ButtonNew:
                    NewBill();
                    break;
            }
        }

        /// <summary>
        /// 删除列表中选择的内容
        /// </summary>
        private void DeleteData()
        {
            GridControl gvControl = this.View.GetControl<GridControl>(Const_CommonList.Main_GridControl);
            DataTable dt = (DataTable)gvControl.DataSource;
            List<string> ids = new List<string>();
            foreach(DataRow row in dt.Rows)
            {
                int isCheck = Convert.ToInt32(row["FCHECK"]);
                if (isCheck == 1)
                {
                    if (formName.EqualIgnorCase(Const_Option.Const_StockAreaPlan)||
                        formName.EqualIgnorCase(Const_Option.Const_Stock))
                    {
                        string fid = Convert.ToString(row["FID"]);
                        ids.Add(fid);
                    }
                    else if (formName.EqualIgnorCase(Const_Option.Const_StockArea))
                    {
                        string fid = Convert.ToString(row["FENTRYID"]);
                        ids.Add(fid);
                    }
                }
            }
            if (ids.IsEmpty())
            {
                ChaintMessageBox.Show("请至少选择一条记录进行删除！");
                return;
            }
            DialogResult diaResult = ChaintMessageBox.ShowConfirmDialog("确定删除当前选择的数据么？");
            if (diaResult != DialogResult.Yes)
            {
                return;
            }
            if (formName.EqualIgnorCase(Const_Option.Const_StockAreaPlan))
            {
                string sqlFormat = @"DELETE FROM T_AUTOSCAN_STOCKAREAPLAN WHERE FID IN ({0})";
                List<QueryParameter> parameters = new List<QueryParameter>();
                StringBuilder paraSb = new StringBuilder();
                int i = 0;
                char splitChar = ',';
                foreach (string fid in ids)
                {
                    i++;
                    string paraName = string.Format("@PARA{0}", i);
                    paraSb.Append(paraName);
                    paraSb.Append(splitChar);
                    QueryParameter para = new QueryParameter(paraName, typeof(string), fid);
                    parameters.Add(para);
                }
                string sql = string.Format(sqlFormat, paraSb.ToString().TrimEnd(splitChar));
                OperateResult result =
                    DBAccessServiceHelper.ExcuteNonQuery(this.Context, sql, parameters.ToArray());
                if (result.IsSuccess)
                {
                    ChaintMessageBox.Show("删除成功!");
                    BindStockAreaPlanData();
                    return;
                }
                else
                {
                    ChaintMessageBox.Show("删除失败!");
                    return;
                }
            }
            else if (formName.EqualIgnorCase(Const_Option.Const_StockArea))
            {
                string sqlFormat = @"DELETE FROM T_AUTOSCAN_STOCKAREAENTRY WHERE FENTRYID IN ({0});
                    DELETE SA FROM T_AUTOSCAN_STOCKAREA SA 
                    LEFT JOIN  T_AUTOSCAN_STOCKAREAENTRY SAE ON SA.FID =SAE.FHEADID
                    WHERE SAE.FENTRYID IS NULL;";
                List<QueryParameter> parameters = new List<QueryParameter>();
                StringBuilder paraSb = new StringBuilder();
                int i = 0;
                char splitChar = ',';
                foreach (string id in ids)
                {
                    i++;
                    string paraName = string.Format("@PARA{0}", i);
                    paraSb.Append(paraName);
                    paraSb.Append(splitChar);
                    QueryParameter para = new QueryParameter(paraName, typeof(string), id);
                    parameters.Add(para);
                }
                string sql = string.Format(sqlFormat, paraSb.ToString().TrimEnd(splitChar));
                OperateResult result =
                    DBAccessServiceHelper.ExcuteNonQuery(this.Context, sql, parameters.ToArray());
                if (result.IsSuccess)
                {
                    ChaintMessageBox.Show("删除成功!");
                    BindStockAreaData();
                    return;
                }
                else
                {
                    ChaintMessageBox.Show("删除失败!");
                    return;
                }
            }
            else if (formName.EqualIgnorCase(Const_Option.Const_Stock))
            {
                string sqlFormat = @"DELETE FROM T_AUTOSCAN_STOCK WHERE FID IN ({0})";
                List<QueryParameter> parameters = new List<QueryParameter>();
                StringBuilder paraSb = new StringBuilder();
                int i = 0;
                char splitChar = ',';
                foreach (string fid in ids)
                {
                    i++;
                    string paraName = string.Format("@PARA{0}", i);
                    paraSb.Append(paraName);
                    paraSb.Append(splitChar);
                    QueryParameter para = new QueryParameter(paraName, typeof(string), fid);
                    parameters.Add(para);
                }
                string sql = string.Format(sqlFormat, paraSb.ToString().TrimEnd(splitChar));
                OperateResult result =
                    DBAccessServiceHelper.ExcuteNonQuery(this.Context, sql, parameters.ToArray());
                if (result.IsSuccess)
                {
                    ChaintMessageBox.Show("删除成功!");
                    BindStockData();
                    return;
                }
                else
                {
                    ChaintMessageBox.Show("删除失败!");
                    return;
                }
            }
        }
    }
}
