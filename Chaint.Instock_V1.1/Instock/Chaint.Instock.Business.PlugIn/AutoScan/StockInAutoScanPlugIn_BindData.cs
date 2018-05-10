using System;
using System.Linq;
using System.Data;
using DevExpress.XtraEditors;
using Chaint.Instock.ServiceHelper;
using Chaint.Common.Core;
using Chaint.Instock.Core;
using Chaint.Common.Core.Sql;
using Chaint.Common.Core.Enums;
using Chaint.Common.Core.Const;
using Chaint.Common.Entity.Utils;
using Chaint.Common.Core.Utils;
using Chaint.Common.ServiceHelper;
namespace Chaint.Instock.Business.PlugIns
{
    partial class StockInAutoScanPlugIn
    {
        /// <summary>
        /// 绑定操作员
        /// </summary>
        private void BindOperator(string userCode = "")
        {
            OperateResult result =
                        EmployeeServiceHelper.GetUserInfo(this.Context, userCode);
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
            var field = this.View.GetControl<SearchLookUpEdit>(Const_StockInAutoScan.Head_Field_FOperator);
            field.Bind(ds);
            field.EditValue = userCode;
        }
        /// <summary>
        /// 绑定机台
        /// </summary>
        private void BindWHMachine(bool isChoose, bool isLocal)
        {
            OperateResult result =
                        StockInAutoScanServiceHelper.GetFactoryOrg(this.Context, isChoose, isLocal);
            if (result.ResultTable.IsEmpty()) return;
            DataTable dtSource = new DataTable();
            //dtSource.Columns.Add("FactoryName", typeof(string));
            dtSource.Columns.Add("MachineID", typeof(string));
            //dtSource.Columns["MachineID"].Caption = "编号";
           // dtSource.Columns["FactoryName"].Caption = "名称";
            foreach (var row in result.ResultTable)
            {
                DataRow dRow = dtSource.NewRow();
               // dRow["FactoryName"] = Convert.ToString(row["FACTORYNAME"]);
                dRow["MachineID"] = Convert.ToString(row["MACHINEID"]);
                dtSource.Rows.Add(dRow);
            }
            DataSource ds = new DataSource();
            ds.DisplayMember = "MachineID";
            //ds.ValueMember = "MachineID";
            ds.Data = dtSource;
            var field = this.View.GetControl<ComboBoxEdit>(Const_StockInAutoScan.Head_Field_FFactory);
            field.Bind(ds);
        }
        /// <summary>
        /// 绑定业务类型
        /// </summary>
        private void BindBusinessTypes(string type)
        {
            OperateResult result =
                        StockInAutoScanServiceHelper.GetBusinessType(this.Context, type);
            if (result.ResultTable.IsEmpty()) return;
            DataTable dtSource = new DataTable();
            dtSource.Columns.Add("BusinessCode", typeof(string));
            dtSource.Columns.Add("BusinessName", typeof(string));
            dtSource.Columns["BusinessCode"].Caption = "编号";
            dtSource.Columns["BusinessName"].Caption = "名称";
            foreach (var row in result.ResultTable)
            {
                DataRow dRow = dtSource.NewRow();
                dRow["BusinessCode"] = Convert.ToString(row["BUSINESSCODE"]);
                dRow["BusinessName"] = Convert.ToString(row["BUSINESSNAME"]);
                dtSource.Rows.Add(dRow);
            }
            DataSource ds = new DataSource();
            ds.DisplayMember = "BusinessName";
            ds.ValueMember = "BusinessCode";
            ds.Data = dtSource;
            var field = this.View.GetControl<SearchLookUpEdit>(Const_StockInAutoScan.Head_Field_FBusinessType);
            field.Bind(ds);
        }
        /// <summary>
        /// 绑定班组
        /// </summary>
        private void BindShift(string shiftName = "")
        {
            OperateResult result =
                        StockInAutoScanServiceHelper.GetShiftInfo(this.Context, shiftName);
            if (result.ResultTable.IsEmpty()) return;
            DataTable dtSource = new DataTable();
            dtSource.Columns.Add("ShiftName", typeof(string));
            foreach (var row in result.ResultTable)
            {
                DataRow dRow = dtSource.NewRow();
                dRow["ShiftName"] = Convert.ToString(row["SHIFTNAME"]);
                dtSource.Rows.Add(dRow);
            }
            DataSource ds = new DataSource();
            ds.DisplayMember = "ShiftName";
            ds.Data = dtSource;
            var field = this.View.GetControl<ComboBoxEdit>(Const_StockInAutoScan.Head_Field_FShift);
            field.Bind(ds);
        }
    }
}
