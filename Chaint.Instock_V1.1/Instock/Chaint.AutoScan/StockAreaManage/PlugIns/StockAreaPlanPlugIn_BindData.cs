using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using DevExpress.XtraEditors;
using Chaint.Common.Core;
using Chaint.Common.BasePlugIn;
using Chaint.Common.Core.EventArgs;
using Chaint.Instock.ServiceHelper;

namespace Chaint.Instock.Business.PlugIns
{
    partial class StockAreaPlanPlugIn
    {
        private void BindPaperType()
        {
            DataTable dtSource = new DataTable();
            dtSource.Columns.Add("TypeId", typeof(string));
            dtSource.Columns.Add("TypeName", typeof(string));
            DataRow dRow1 = dtSource.NewRow();
            dRow1["TypeId"] = "1";
            dRow1["TypeName"] = "卷筒纸";
            dtSource.Rows.Add(dRow1);
            DataRow dRow2 = dtSource.NewRow();
            dRow2["TypeId"] = "2";
            dRow2["TypeName"] = "平板纸";
            dtSource.Rows.Add(dRow2);
            dtSource.AcceptChanges();
            this.View.BindLookUpEditWithSimpleColumn("FPaperType", "TypeName",
                "类型", "TypeId", dtSource);
        }
        /// <summary>
        /// 绑定纸芯数据
        /// </summary>
        private void BindCoreDiameter()
        {
            OperateResult result =
                        StockAreaPlanServiceHelper.GetCoreDiameter(this.View.Context);
            if (result.ResultTable.Count() <= 0) return;
            DataTable dtSource = new DataTable();
            dtSource.Columns.Add("DiameterName", typeof(string));
            foreach (var row in result.ResultTable)
            {
                DataRow dRow = dtSource.NewRow();
                dRow["DiameterName"] = Convert.ToString(row["COREDIAMETER"]);
                dtSource.Rows.Add(dRow);
            }
            dtSource.AcceptChanges();
            this.View.BindLookUpEditWithSimpleColumn("FDiameterOrReam", "DiameterName",
                "纸芯", "DiameterName", dtSource);
        }
        /// <summary>
        /// 绑定产品等级
        /// </summary>
        private void BindPaperGrade()
        {
            OperateResult result =
                        StockAreaPlanServiceHelper.GetPaperGrade(this.View.Context);
            if (result.ResultTable.Count() <= 0) return;
            DataTable dtSource = new DataTable();
            dtSource.Columns.Add("Grade", typeof(string));
            dtSource.Columns.Add("GradeDesc", typeof(string));
            foreach (var row in result.ResultTable)
            {
                DataRow dRow = dtSource.NewRow();
                dRow["Grade"] = Convert.ToString(row["GRADE"]);
                dRow["GradeDesc"] = Convert.ToString(row["GRADEDESC"]);
                dtSource.Rows.Add(dRow);
            }
            dtSource.AcceptChanges();
            this.View.BindLookUpEditWithSimpleColumn("FPaperGrade", "GradeDesc",
                "产品等级", "Grade", dtSource);
        }
        /// <summary>
        /// 绑定产品认证
        /// </summary>
        private void BindCertification()
        {
            OperateResult result =
                        StockAreaPlanServiceHelper.GetPaperCertification(this.View.Context);
            if (result.ResultTable.Count() <= 0) return;
            DataTable dtSource = new DataTable();
            dtSource.Columns.Add("OnlyID", typeof(string));
            dtSource.Columns.Add("PaperCert", typeof(string));
            foreach (var row in result.ResultTable)
            {
                DataRow dRow = dtSource.NewRow();
                dRow["OnlyID"] = Convert.ToString(row["ONLYID"]);
                dRow["PaperCert"] = Convert.ToString(row["PAPERCERT"]);
                dtSource.Rows.Add(dRow);
            }
            dtSource.AcceptChanges();
            this.View.BindLookUpEditWithSimpleColumn("FCertification", "PaperCert",
                "产品认证", "OnlyID", dtSource);
        }
        /// <summary>
        /// 绑定包装方式
        /// </summary>
        private void BindPackType(string paperType)
        {
            OperateResult result =
                        StockAreaPlanServiceHelper.GetPackType(this.View.Context, paperType);
            if (result.ResultTable.Count() <= 0) return;
            DataTable dtSource = new DataTable();
            dtSource.Columns.Add("OnlyID", typeof(string));
            dtSource.Columns.Add("PackType", typeof(string));
            foreach (var row in result.ResultTable)
            {
                DataRow dRow = dtSource.NewRow();
                dRow["OnlyID"] = Convert.ToString(row["ONLYID"]);
                dRow["PackType"] = Convert.ToString(row["PACKTYPE"]);
                dtSource.Rows.Add(dRow);
            }
            dtSource.AcceptChanges();
            this.View.BindLookUpEditWithSimpleColumn("FPackType", "PackType",
                "包装方式", "OnlyID", dtSource);
        }
        /// <summary>
        /// 绑定包装方式
        /// </summary>
        private void BindTransportType()
        {
            OperateResult result =
                        StockAreaPlanServiceHelper.GetTransportType(this.View.Context);
            if (result.ResultTable.Count() <= 0) return;
            DataTable dtSource = new DataTable();
            dtSource.Columns.Add("OnlyID", typeof(string));
            dtSource.Columns.Add("TransportType", typeof(string));
            foreach (var row in result.ResultTable)
            {
                DataRow dRow = dtSource.NewRow();
                dRow["OnlyID"] = Convert.ToString(row["ONLYID"]);
                dRow["TransportType"] = Convert.ToString(row["TRANSPORTTYPE"]);
                dtSource.Rows.Add(dRow);
            }
            dtSource.AcceptChanges();
            this.View.BindLookUpEditWithSimpleColumn("FTransportType", "TransportType",
                "包装方式", "OnlyID", dtSource);
        }
        /// <summary>
        /// 绑定令数
        /// </summary>
        private void BindReam()
        {
            OperateResult result =
                        StockAreaPlanServiceHelper.GetReam(this.View.Context);
            if (result.ResultTable.Count() <= 0) return;
            DataTable dtSource = new DataTable();
            dtSource.Columns.Add("SheetReam", typeof(string));
            foreach (var row in result.ResultTable)
            {
                DataRow dRow = dtSource.NewRow();
                dRow["SheetReam"] = Convert.ToString(row["SHEETREAMS"]);
                dtSource.Rows.Add(dRow);
            }
            dtSource.AcceptChanges();
            this.View.BindLookUpEditWithSimpleColumn("FDiameterOrReam", "SheetReam",
                "令数", "SheetReam", dtSource);
        }
        /// <summary>
        /// 绑定直径
        /// </summary>
        private void BindRollDiameter()
        {
            OperateResult result =
                        StockAreaPlanServiceHelper.GetRollDiameter(this.View.Context);
            if (result.ResultTable.Count() <= 0) return;
            DataTable dtSource = new DataTable();
            dtSource.Columns.Add("Diameter", typeof(string));
            foreach (var row in result.ResultTable)
            {
                DataRow dRow = dtSource.NewRow();
                dRow["Diameter"] = Convert.ToString(row["DIAMETER"]);
                dtSource.Rows.Add(dRow);
            }
            dtSource.AcceptChanges();
            this.View.BindLookUpEditWithSimpleColumn("FDiameterOrSlides", "Diameter",
                "直径MM", "Diameter", dtSource);
        }
        /// <summary>
        /// 绑定线长
        /// </summary>
        private void BindRollLength()
        {
            OperateResult result =
                        StockAreaPlanServiceHelper.GetRollLength(this.View.Context);
            if (result.ResultTable.Count() <= 0) return;
            DataTable dtSource = new DataTable();
            dtSource.Columns.Add("Length", typeof(string));
            foreach (var row in result.ResultTable)
            {
                DataRow dRow = dtSource.NewRow();
                dRow["Length"] = Convert.ToString(row["LENGTH"]);
                dtSource.Rows.Add(dRow);
            }
            dtSource.AcceptChanges();
            this.View.BindLookUpEditWithSimpleColumn("FLength", "Length",
                "线长M", "Length", dtSource);
        }
        /// <summary>
        /// 绑定产品色相
        /// </summary>
        private void BindColor()
        {
            OperateResult result =
                        StockAreaPlanServiceHelper.GetPaperColor(this.View.Context);
            if (result.ResultTable.Count() <= 0) return;
            DataTable dtSource = new DataTable();
            dtSource.Columns.Add("Color", typeof(string));
            dtSource.Columns.Add("ColorDesc", typeof(string));
            foreach (var row in result.ResultTable)
            {
                DataRow dRow = dtSource.NewRow();
                dRow["Color"] = Convert.ToString(row["COLOR"]);
                dRow["ColorDesc"] = Convert.ToString(row["COLORDESC"]);
                dtSource.Rows.Add(dRow);
            }
            dtSource.AcceptChanges();
            this.View.BindLookUpEditWithSimpleColumn("FColor", "ColorDesc",
                "产品色相", "Color", dtSource);
        }
        /// <summary>
        /// 绑定客户专用
        /// </summary>
        private void BindSpecCustomerName()
        {
            OperateResult result =
                        StockAreaPlanServiceHelper.GetSpecCustomerName(this.View.Context);
            if (result.ResultTable.Count() <= 0) return;
            DataTable dtSource = new DataTable();
            dtSource.Columns.Add("OnlyID", typeof(string));
            dtSource.Columns.Add("SpecCustName", typeof(string));
            foreach (var row in result.ResultTable)
            {
                DataRow dRow = dtSource.NewRow();
                dRow["OnlyID"] = Convert.ToString(row["ONLYID"]);
                dRow["SpecCustName"] = Convert.ToString(row["SPECCUSTNAME"]);
                dtSource.Rows.Add(dRow);
            }
            dtSource.AcceptChanges();
            this.View.BindLookUpEditWithSimpleColumn("FSpecCustName", "SpecCustName",
                "客户专用", "OnlyID", dtSource);
        }
        /// <summary>
        /// 绑定产品专用
        /// </summary>
        private void BindSpecProductName()
        {
            OperateResult result =
                        StockAreaPlanServiceHelper.GetSpecProductName(this.View.Context);
            if (result.ResultTable.Count() <= 0) return;
            DataTable dtSource = new DataTable();
            dtSource.Columns.Add("OnlyID", typeof(string));
            dtSource.Columns.Add("SpecProdName", typeof(string));
            foreach (var row in result.ResultTable)
            {
                DataRow dRow = dtSource.NewRow();
                dRow["OnlyID"] = Convert.ToString(row["ONLYID"]);
                dRow["SpecProdName"] = Convert.ToString(row["SPECPRODNAME"]);
                dtSource.Rows.Add(dRow);
            }
            dtSource.AcceptChanges();
            this.View.BindLookUpEditWithSimpleColumn("FSpecProdName", "SpecProdName",
                "产品专用", "OnlyID", dtSource);
        }
        /// <summary>
        /// 绑定合格证样式
        /// </summary>
        private void BindTrademarkStyle()
        {
            OperateResult result =
                        StockAreaPlanServiceHelper.GetTrademarkStyle(this.View.Context);
            if (result.ResultTable.Count() <= 0) return;
            DataTable dtSource = new DataTable();
            dtSource.Columns.Add("OnlyID", typeof(string));
            dtSource.Columns.Add("TrademarkStyle", typeof(string));
            foreach (var row in result.ResultTable)
            {
                DataRow dRow = dtSource.NewRow();
                dRow["OnlyID"] = Convert.ToString(row["ONLYID"]);
                dRow["TrademarkStyle"] = Convert.ToString(row["TRADEMARKSTYLE"]);
                dtSource.Rows.Add(dRow);
            }
            dtSource.AcceptChanges();
            this.View.BindLookUpEditWithSimpleColumn("FTrademarkStyle", "TrademarkStyle",
                "合格证样式", "OnlyID", dtSource);
        }
        /// <summary>
        /// 绑定计重方式
        /// </summary>
        private void BindWeightMode()
        {
            OperateResult result =
                        StockAreaPlanServiceHelper.GetWeightMode(this.View.Context);
            if (result.ResultTable.Count() <= 0) return;
            DataTable dtSource = new DataTable();
            dtSource.Columns.Add("WeightMode", typeof(string));
            dtSource.Columns.Add("WeightModeDesc", typeof(string));
            foreach (var row in result.ResultTable)
            {
                DataRow dRow = dtSource.NewRow();
                dRow["WeightMode"] = Convert.ToString(row["WEIGHTMODE"]);
                dRow["WeightModeDesc"] = Convert.ToString(row["WEIGHTMODEDESC"]);
                dtSource.Rows.Add(dRow);
            }
            dtSource.AcceptChanges();
            this.View.BindLookUpEditWithSimpleColumn("FWeightMode", "WeightModeDesc",
                "计重方式", "WeightMode", dtSource);
        }
        /// <summary>
        /// 绑定计重方式
        /// </summary>
        private void BindSlidesOfReam()
        {
            OperateResult result =
                        StockAreaPlanServiceHelper.GetSlidesOfReam(this.View.Context);
            if (result.ResultTable.Count() <= 0) return;
            DataTable dtSource = new DataTable();
            dtSource.Columns.Add("SlidesOfReam", typeof(string));
            foreach (var row in result.ResultTable)
            {
                DataRow dRow = dtSource.NewRow();
                dRow["SlidesOfReam"] = Convert.ToString(row["SLIDESOFREAM"]);
                dtSource.Rows.Add(dRow);
            }
            dtSource.AcceptChanges();
            this.View.BindLookUpEditWithSimpleColumn("FDiameterOrSlides", "SlidesOfReam",
                "令张数", "SlidesOfReam", dtSource);
        }
    }
}
