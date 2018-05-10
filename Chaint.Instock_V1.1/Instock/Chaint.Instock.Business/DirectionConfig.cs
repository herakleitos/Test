using Chaint.Common.Core;
using Chaint.Common.Core.AppConfig;
using Chaint.Common.Core.Const;
using Chaint.Common.Core.Utils;
using Chaint.Common.Entity.Utils;
using Chaint.Common.ServiceHelper;
using Chaint.Instock.ServiceHelper;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Filtering;
using DevExpress.XtraEditors.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Chaint.Instock.Business
{
    public partial class DirectionConfig : Form
    {
        private Context context;
        private AppConfig_INI deviceConfiger;
        public DirectionConfig(Context ctx)
        {
            context = ctx;
            deviceConfiger = new AppConfig_INI(ctx.DevicesConfigFilePath);
            RepositoryItemTextEdit textEdit = new RepositoryItemTextEdit();
            RepositoryItemCalcEdit calEdit = new RepositoryItemCalcEdit();
            InitializeComponent();
            filterBox.FilterColumns.Add(new UnboundFilterColumn("物料", "MaterialCode",
                typeof(string), GetProductControl(), DevExpress.Data.Filtering.Helpers.FilterColumnClauseClass.String));
            filterBox.FilterColumns.Add(new UnboundFilterColumn("机台", "MachineID",
                typeof(string), GetMachineIdControl(), DevExpress.Data.Filtering.Helpers.FilterColumnClauseClass.String));

            filterBox.FilterColumns.Add(new UnboundFilterColumn("产品名称", "ProductName",
                typeof(string), GetProductNameControl(), DevExpress.Data.Filtering.Helpers.FilterColumnClauseClass.String));

            filterBox.FilterColumns.Add(new UnboundFilterColumn("产品类别", "ProductType",
                typeof(string), GetProducTypeControl(), DevExpress.Data.Filtering.Helpers.FilterColumnClauseClass.String));

            filterBox.FilterColumns.Add(new UnboundFilterColumn("品牌", "Trademark",
        typeof(string), GetTradeRemarkControl(), DevExpress.Data.Filtering.Helpers.FilterColumnClauseClass.String));
            
            filterBox.FilterColumns.Add(new UnboundFilterColumn("等级", "SheetGrade",
typeof(string), GetPaperGradeControl(), DevExpress.Data.Filtering.Helpers.FilterColumnClauseClass.String));
            
            filterBox.FilterColumns.Add(new UnboundFilterColumn("包装方式", "ReamPackType",
typeof(string), GetPackTypeControl(), DevExpress.Data.Filtering.Helpers.FilterColumnClauseClass.String));

            filterBox.FilterColumns.Add(new UnboundFilterColumn("正反丝", "FiberDirect",
typeof(string), GetFiberDirectControl(), DevExpress.Data.Filtering.Helpers.FilterColumnClauseClass.String));

            filterBox.FilterColumns.Add(new UnboundFilterColumn("产品专用", "SpecProdName",
typeof(string), GetSpecProdNameControl(), DevExpress.Data.Filtering.Helpers.FilterColumnClauseClass.String));
            filterBox.FilterColumns.Add(new UnboundFilterColumn("客户专用", "SpecCustName",
typeof(string), GetSpecCustNameControl(), DevExpress.Data.Filtering.Helpers.FilterColumnClauseClass.String));

            filterBox.FilterColumns.Add(new UnboundFilterColumn("合格证类型", "IsWhiteFlag",
typeof(string), GetIsWhiteFlagControl(), DevExpress.Data.Filtering.Helpers.FilterColumnClauseClass.String));

            filterBox.FilterColumns.Add(new UnboundFilterColumn("色相", "Color",
typeof(string), GetColorControl(), DevExpress.Data.Filtering.Helpers.FilterColumnClauseClass.String));
            filterBox.FilterColumns.Add(new UnboundFilterColumn("纸种认证", "Color",
typeof(string), GetPaperCertControl(), DevExpress.Data.Filtering.Helpers.FilterColumnClauseClass.String));

            filterBox.FilterColumns.Add(new UnboundFilterColumn("订单号", "OrderNO",
typeof(string), textEdit, DevExpress.Data.Filtering.Helpers.FilterColumnClauseClass.String));
            filterBox.FilterColumns.Add(new UnboundFilterColumn("产品备注", "CustTrademark",
typeof(string), textEdit, DevExpress.Data.Filtering.Helpers.FilterColumnClauseClass.String));
            filterBox.FilterColumns.Add(new UnboundFilterColumn("生产备注", "PalletRemark",
typeof(string), textEdit, DevExpress.Data.Filtering.Helpers.FilterColumnClauseClass.String));

            filterBox.FilterColumns.Add(new UnboundFilterColumn("定量", "Basisweight",
  typeof(int), calEdit, DevExpress.Data.Filtering.Helpers.FilterColumnClauseClass.Generic));
            filterBox.FilterColumns.Add(new UnboundFilterColumn("令数", "SheetReams",
typeof(int), calEdit, DevExpress.Data.Filtering.Helpers.FilterColumnClauseClass.Generic));

            string filterString = deviceConfiger.GetValue("SCANDESRULE", "CustomFilter","0");
            if (filterString.Length > 0)
            {
                filterBox.FilterString = filterString;
            }
        }

        private void btConfirm_Click(object sender, EventArgs e)
        {
            string customFilter = filterBox.FilterString;
            deviceConfiger.SetValue("SCANDESRULE", "CustomFilter", customFilter);
            ChaintMessageBox.Show("设置成功!");
        }

        private RepositoryItemSearchLookUpEdit GetPaperGradeControl()
        {
            RepositoryItemSearchLookUpEdit item = new RepositoryItemSearchLookUpEdit();
            OperateResult result = new OperateResult();
            string strSql = @"SELECT GRADE,GRADEDESC FROM PAPER_GRADE ORDER BY GRADE";
            result = DBAccessServiceHelper.ExecuteQueryWithDataTable(context, strSql);        
            if (result.ResultDataTable.IsEmpty()) return item;
            result.ResultDataTable.Columns["GRADE"].Caption = "名称编号";
            result.ResultDataTable.Columns["GRADEDESC"].Caption = "名称";
            DataSource ds = new DataSource();
            ds.DisplayMember = "GRADEDESC";
            ds.ValueMember = "GRADE";
            ds.Data = result.ResultDataTable;
            item.Bind(ds);
            return item;
        }
        private RepositoryItemSearchLookUpEdit GetProductControl()
        {
            RepositoryItemSearchLookUpEdit item = new RepositoryItemSearchLookUpEdit();
            OperateResult result =
                StockAreaDataServiceHelper.GetProductInfo(this.context);
            if (result.ResultDataTable.IsEmpty()) return item;
            result.ResultDataTable.Columns["WLMC"].Caption = "名称";
            result.ResultDataTable.Columns["WLBH"].Caption = "编码";
            DataSource ds = new DataSource();
            ds.DisplayMember = "WLMC";
            ds.ValueMember = "WLBH";
            ds.Data = result.ResultDataTable;
            item.Bind(ds);
            return item;
        }
        private RepositoryItemSearchLookUpEdit GetMachineIdControl()
        {
            RepositoryItemSearchLookUpEdit item = new RepositoryItemSearchLookUpEdit();
            OperateResult result = new OperateResult();
            context.Section = Const_ConfigSection.WMSSection;
            string strSql = @"SELECT FACTORYNAME,MACHINEID FROM T_FACTORY ORDER BY MACHINEID";
            result = DBAccessServiceHelper.ExecuteQueryWithDataTable(context, strSql);
            context.Section = Const_ConfigSection.MainSection;
            if (result.ResultDataTable.IsEmpty()) return item;
            result.ResultDataTable.Columns["MACHINEID"].Caption = "编号";
            result.ResultDataTable.Columns["FACTORYNAME"].Caption = "名称";
            DataSource ds = new DataSource();
            ds.DisplayMember = "FACTORYNAME";
            ds.ValueMember = "MACHINEID";
            ds.Data = result.ResultDataTable;
            item.Bind(ds);
            return item;
        }
        private RepositoryItemSearchLookUpEdit GetProductNameControl()
        {
            RepositoryItemSearchLookUpEdit item = new RepositoryItemSearchLookUpEdit();
            OperateResult result = new OperateResult();
            string strSql = @"SELECT DISTINCT PZBM,PZMC FROM CT_WLZD";
            result = DBAccessServiceHelper.ExecuteQueryWithDataTable(context, strSql);
            if (result.ResultDataTable.IsEmpty()) return item;
            result.ResultDataTable.Columns["PZBM"].Caption = "编号";
            result.ResultDataTable.Columns["PZMC"].Caption = "名称";
            DataSource ds = new DataSource();
            ds.DisplayMember = "PZMC";
            ds.ValueMember = "PZBM";
            ds.Data = result.ResultDataTable;
            item.Bind(ds);
            return item;
        }
        private RepositoryItemSearchLookUpEdit GetProducTypeControl()
        {
            RepositoryItemSearchLookUpEdit item = new RepositoryItemSearchLookUpEdit();
            OperateResult result = new OperateResult();
            string strSql = @"SELECT DISTINCT LBBM,LBMC FROM CT_WLZD";
            result = DBAccessServiceHelper.ExecuteQueryWithDataTable(context, strSql);
            if (result.ResultDataTable.IsEmpty()) return item;
            result.ResultDataTable.Columns["LBBM"].Caption = "编号";
            result.ResultDataTable.Columns["LBMC"].Caption = "名称";
            DataSource ds = new DataSource();
            ds.DisplayMember = "LBMC";
            ds.ValueMember = "LBBM";
            ds.Data = result.ResultDataTable;
            item.Bind(ds);
            return item;
        }
        private RepositoryItemSearchLookUpEdit GetTradeRemarkControl()
        {
            RepositoryItemSearchLookUpEdit item = new RepositoryItemSearchLookUpEdit();
            OperateResult result = new OperateResult();
            string strSql = @"SELECT DISTINCT PPBM,PPMC FROM CT_WLZD";
            result = DBAccessServiceHelper.ExecuteQueryWithDataTable(context, strSql);
            if (result.ResultDataTable.IsEmpty()) return item;
            result.ResultDataTable.Columns["PPBM"].Caption = "编号";
            result.ResultDataTable.Columns["PPMC"].Caption = "名称";
            DataSource ds = new DataSource();
            ds.DisplayMember = "PPMC";
            ds.ValueMember = "PPBM";
            ds.Data = result.ResultDataTable;
            item.Bind(ds);
            return item;
        }
        private RepositoryItemSearchLookUpEdit GetPackTypeControl()
        {
            RepositoryItemSearchLookUpEdit item = new RepositoryItemSearchLookUpEdit();
            OperateResult result = new OperateResult();
            string strSql = @"SELECT ONLYID,PACKTYPE FROM PAPER_PACKTYPE  WHERE TYPE='2'";
            result = DBAccessServiceHelper.ExecuteQueryWithDataTable(context, strSql);
            if (result.ResultDataTable.IsEmpty()) return item;
            result.ResultDataTable.Columns["ONLYID"].Caption = "编号";
            result.ResultDataTable.Columns["PACKTYPE"].Caption = "名称";
            DataSource ds = new DataSource();
            ds.DisplayMember = "PACKTYPE";
            ds.ValueMember = "ONLYID";
            ds.Data = result.ResultDataTable;
            item.Bind(ds);
            return item;
        }
        private RepositoryItemComboBox GetFiberDirectControl()
        {
            RepositoryItemComboBox com = new RepositoryItemComboBox();
            com.Items.Add("正丝");
            com.Items.Add("反丝");
            return com;
        }
        private RepositoryItemSearchLookUpEdit GetSpecCustNameControl()
        {
            RepositoryItemSearchLookUpEdit item = new RepositoryItemSearchLookUpEdit();
            OperateResult result = new OperateResult();
            string strSql = @"SELECT ONLYID,SPECCUSTNAME FROM PAPER_SPECCUSTNAME ORDER BY ONLYID";
            result = DBAccessServiceHelper.ExecuteQueryWithDataTable(context, strSql);
            if (result.ResultDataTable.IsEmpty()) return item;
            result.ResultDataTable.Columns["ONLYID"].Caption = "编号";
            result.ResultDataTable.Columns["SPECCUSTNAME"].Caption = "名称";
            DataSource ds = new DataSource();
            ds.DisplayMember = "SPECCUSTNAME";
            ds.ValueMember = "ONLYID";
            ds.Data = result.ResultDataTable;
            item.Bind(ds);
            return item;
        }
        private RepositoryItemSearchLookUpEdit GetSpecProdNameControl()
        {
            RepositoryItemSearchLookUpEdit item = new RepositoryItemSearchLookUpEdit();
            OperateResult result = new OperateResult();
            string strSql = @"SELECT ONLYID,SPECPRODNAME FROM PAPER_SPECPRODNAME ORDER BY ONLYID";
            result = DBAccessServiceHelper.ExecuteQueryWithDataTable(context, strSql);
            if (result.ResultDataTable.IsEmpty()) return item;
            result.ResultDataTable.Columns["ONLYID"].Caption = "编号";
            result.ResultDataTable.Columns["SPECPRODNAME"].Caption = "名称";
            DataSource ds = new DataSource();
            ds.DisplayMember = "SPECPRODNAME";
            ds.ValueMember = "ONLYID";
            ds.Data = result.ResultDataTable;
            item.Bind(ds);
            return item;
        }
        private RepositoryItemSearchLookUpEdit GetIsWhiteFlagControl()
        {
            RepositoryItemSearchLookUpEdit item = new RepositoryItemSearchLookUpEdit();
            DataTable dtSource = new DataTable();
            dtSource.Columns.Add("TrademarkStyle", typeof(string));
            dtSource.Columns.Add("OnlyId", typeof(string));
            dtSource.Columns["OnlyId"].Caption = "编号";
            dtSource.Columns["TrademarkStyle"].Caption = "名称";
            DataRow dRow1 = dtSource.NewRow();
            dRow1["OnlyId"] = "1";
            dRow1["TrademarkStyle"] = "白证";
            DataRow dRow2 = dtSource.NewRow();
            dRow2["OnlyId"] = "0";
            dRow2["TrademarkStyle"] = "出口证";
            dtSource.Rows.Add(dRow1);
            dtSource.Rows.Add(dRow2);
            dtSource.AcceptChanges();
            DataSource ds = new DataSource();
            ds.DisplayMember = "TrademarkStyle";
            ds.ValueMember = "OnlyId";
            ds.Data = dtSource;

            item.Bind(ds);
            return item;
        }
        private RepositoryItemSearchLookUpEdit GetColorControl()
        {
            RepositoryItemSearchLookUpEdit item = new RepositoryItemSearchLookUpEdit();
            OperateResult result = new OperateResult();
            string strSql = @"SELECT COLOR,COLORDESC FROM PAPER_COLOR";
            result = DBAccessServiceHelper.ExecuteQueryWithDataTable(context, strSql);
            if (result.ResultDataTable.IsEmpty()) return item;
            result.ResultDataTable.Columns["COLOR"].Caption = "编号";
            result.ResultDataTable.Columns["COLORDESC"].Caption = "名称";
            DataSource ds = new DataSource();
            ds.DisplayMember = "COLORDESC";
            ds.ValueMember = "COLOR";
            ds.Data = result.ResultDataTable;
            item.Bind(ds);
            return item;
        }
        private RepositoryItemSearchLookUpEdit GetPaperCertControl()
        {
            RepositoryItemSearchLookUpEdit item = new RepositoryItemSearchLookUpEdit();
            OperateResult result = new OperateResult();
            string strSql = @"SELECT ONLYID ,PAPERCERT FROM PAPER_CERTIFICATION";
            result = DBAccessServiceHelper.ExecuteQueryWithDataTable(context, strSql);
            if (result.ResultDataTable.IsEmpty()) return item;
            result.ResultDataTable.Columns["ONLYID"].Caption = "编号";
            result.ResultDataTable.Columns["PAPERCERT"].Caption = "名称";
            DataSource ds = new DataSource();
            ds.DisplayMember = "PAPERCERT";
            ds.ValueMember = "ONLYID";
            ds.Data = result.ResultDataTable;
            item.Bind(ds);
            return item;
        }
    }
}
