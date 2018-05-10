using System;
using System.Data;
using DevExpress.XtraEditors;
using Chaint.Common.Core;
using Chaint.Common.Core.Utils;
using Chaint.Instock.Core;
using Chaint.Instock.ServiceHelper;
using Chaint.Common.Entity.Utils;
namespace Chaint.Instock.Business.PlugIns
{
    partial class StockAreaPlanPlugIn
    {
        private void BindSpCustomer()
        {
            OperateResult result =
                        StockAreaPlanServiceHelper.GetSpCustomer(this.Context);
            if (result.ResultDataTable.IsEmpty()) return;
            result.ResultDataTable.Columns["NAME"].Caption = "名称";
            DataSource ds = new DataSource();
            ds.DisplayMember = "NAME";
            ds.ValueMember = "NAME";
            ds.Data = result.ResultDataTable;
            var field = this.View.GetControl<SearchLookUpEdit>(Const_StockAreaPlan.Head_Field_FSpCustomer);
            field.Bind(ds);
        }
        private void BindPaperType()
        {
            DataTable dtSource = new DataTable();
            dtSource.Columns.Add("TypeId", typeof(string));
            dtSource.Columns.Add("TypeName", typeof(string));
            dtSource.Columns["TypeId"].Caption = "编号";
            dtSource.Columns["TypeName"].Caption = "名称";
            DataRow dRow1 = dtSource.NewRow();
            dRow1["TypeId"] = "1";
            dRow1["TypeName"] = "卷筒纸";
            dtSource.Rows.Add(dRow1);
            DataRow dRow2 = dtSource.NewRow();
            dRow2["TypeId"] = "2";
            dRow2["TypeName"] = "平板纸";
            dtSource.Rows.Add(dRow2);
            dtSource.AcceptChanges();
            DataSource ds = new DataSource();
            ds.DisplayMember = "TypeName";
            ds.ValueMember = "TypeId";
            ds.Data = dtSource;
            var field = this.View.GetControl<SearchLookUpEdit>(Const_StockAreaPlan.Head_Field_FPaperType);
            field.Bind(ds);
        }
        /// <summary>
        /// 绑定规格
        /// </summary>
        private void BindSpecification()
        {
            //string paperType =
            //    this.View.Model.GetValue<string>(Const_StockAreaPlan.Head_Field_FPaperType);
            //if (paperType.IsNullOrEmptyOrWhiteSpace()) paperType = "1";
            //OperateResult result =
            //            StockAreaPlanServiceHelper.GetSpecification(this.Context, paperType);
            //if (result.ResultDataTable.IsEmpty()) return;
            //result.ResultDataTable.Columns["FSPECIFICATION"].Caption = "名称";
            //DataSource ds = new DataSource();
            //ds.DisplayMember = "FSPECIFICATION";
            //ds.Data = result.ResultDataTable;
            //var field = this.View.GetControl<ComboBoxEdit>(Const_StockAreaPlan.Head_Field_FSpecification);
            //field.Bind(ds);
        }
        /// <summary>
        /// 绑定纸芯数据
        /// </summary>
        private void BindCoreDiameter()
        {
            OperateResult result =
                        StockAreaPlanServiceHelper.GetCoreDiameter(this.Context);
            if (result.ResultDataTable.IsEmpty()) return;
            result.ResultDataTable.Columns["COREDIAMETER"].Caption = "名称";
            DataSource ds = new DataSource();
            ds.DisplayMember = "COREDIAMETER";
            ds.Data = result.ResultDataTable;
            var field = this.View.GetControl<ComboBoxEdit>(Const_StockAreaPlan.Head_Field_FCoreDiameterOrReam);
            field.Bind(ds);
        }
        /// <summary>
        /// 绑定产品等级
        /// </summary>
        private void BindPaperGrade()
        {
            OperateResult result =
                        StockAreaPlanServiceHelper.GetPaperGrade(this.Context);
            if (result.ResultDataTable.IsEmpty()) return;
            result.ResultDataTable.Columns["GRADEDESC"].Caption = "名称";
            DataSource ds = new DataSource();
            ds.DisplayMember = "GRADEDESC";
            ds.ValueMember = "GRADEDESC";
            ds.Data = result.ResultDataTable;
            var field = this.View.GetControl<SearchLookUpEdit>(Const_StockAreaPlan.Head_Field_FPaperGrade);
            field.Bind(ds);
        }
        /// <summary>
        /// 绑定产品认证
        /// </summary>
        private void BindCertification()
        {
            OperateResult result =
                        StockAreaPlanServiceHelper.GetPaperCertification(this.Context);
            if (result.ResultDataTable.IsEmpty()) return;
            result.ResultDataTable.Columns["PAPERCERT"].Caption = "名称"; 
            DataSource ds = new DataSource();
            ds.DisplayMember = "PAPERCERT";
            ds.ValueMember = "PAPERCERT";
            ds.Data = result.ResultDataTable;
            var field = this.View.GetControl<SearchLookUpEdit>(Const_StockAreaPlan.Head_Field_FCertification);
            field.Bind(ds);
        }
        /// <summary>
        /// 绑定包装方式
        /// </summary>
        private void BindPackType(string paperType)
        {
            OperateResult result =
                        StockAreaPlanServiceHelper.GetPackType(this.Context, paperType);
            if (result.ResultDataTable.IsEmpty()) return;
            result.ResultDataTable.Columns["PACKTYPE"].Caption = "名称";
            DataSource ds = new DataSource();
            ds.DisplayMember = "PACKTYPE";
            ds.ValueMember = "PACKTYPE";
            ds.Data = result.ResultDataTable;
            var field = this.View.GetControl<SearchLookUpEdit>(Const_StockAreaPlan.Head_Field_FPackType);
            field.Bind(ds);
        }
        /// <summary>
        /// 绑定夹板包装
        /// </summary>
        private void BindTransportType()
        {
            OperateResult result =
                        StockAreaPlanServiceHelper.GetTransportType(this.Context);
            if (result.ResultDataTable.IsEmpty()) return;
            result.ResultDataTable.Columns["TRANSPORTTYPE"].Caption = "名称";
            
            DataSource ds = new DataSource();
            ds.DisplayMember = "TRANSPORTTYPE";
            ds.ValueMember = "TRANSPORTTYPE";
            ds.Data = result.ResultDataTable;
            var field = this.View.GetControl<SearchLookUpEdit>(Const_StockAreaPlan.Head_Field_FTransportType);
            field.Bind(ds);
        }
        /// <summary>
        /// 绑定令数
        /// </summary>
        private void BindReam()
        {
            //OperateResult result =
            //            StockAreaPlanServiceHelper.GetReam(this.Context);
            //if (result.ResultDataTable.IsEmpty()) return;
            //result.ResultDataTable.Columns["SHEETREAMS"].Caption = "名称";
            //DataSource ds = new DataSource();
            //ds.DisplayMember = "SHEETREAMS";
            //ds.Data = result.ResultDataTable;
            //var field = this.View.GetControl<ComboBoxEdit>(Const_StockAreaPlan.Head_Field_FCoreDiameterOrReam);
            //field.Bind(ds);
        }
        /// <summary>
        /// 绑定直径
        /// </summary>
        private void BindRollDiameter()
        {
            //OperateResult result =
            //            StockAreaPlanServiceHelper.GetRollDiameter(this.Context);
            //if (result.ResultDataTable.IsEmpty()) return;
            //result.ResultDataTable.Columns["DIAMETER"].Caption = "名称";
            
            //DataSource ds = new DataSource();
            //ds.DisplayMember = "DIAMETER";
            //ds.Data = result.ResultDataTable;
            //var field = this.View.GetControl<ComboBoxEdit>(Const_StockAreaPlan.Head_Field_FDiameterOrSlides);
            //field.Bind(ds);
        }
        /// <summary>
        /// 绑定线长
        /// </summary>
        private void BindRollLength()
        {
            //OperateResult result =
            //            StockAreaPlanServiceHelper.GetRollLength(this.Context);
            //if (result.ResultDataTable.IsEmpty()) return;
            //result.ResultDataTable.Columns["LENGTH"].Caption = "名称";
          
            //DataSource ds = new DataSource();
            //ds.DisplayMember = "LENGTH";
            //ds.Data = result.ResultDataTable;
            //var field = this.View.GetControl<ComboBoxEdit>(Const_StockAreaPlan.Head_Field_FLength);
            //field.Bind(ds);
        }
        /// <summary>
        /// 绑定产品色相
        /// </summary>
        private void BindColor()
        {
            OperateResult result =
                        StockAreaPlanServiceHelper.GetPaperColor(this.Context);
            if (result.ResultDataTable.IsEmpty()) return;
            result.ResultDataTable.Columns["COLORDESC"].Caption = "名称";
           
            DataSource ds = new DataSource();
            ds.DisplayMember = "COLORDESC";
            ds.ValueMember = "COLORDESC";
            ds.Data = result.ResultDataTable;
            var field = this.View.GetControl<SearchLookUpEdit>(Const_StockAreaPlan.Head_Field_FColor);
            field.Bind(ds);
        }
        /// <summary>
        /// 绑定客户专用
        /// </summary>
        private void BindSpecCustomerName()
        {
            OperateResult result =
                        StockAreaPlanServiceHelper.GetSpecCustomerName(this.Context);
            if (result.ResultDataTable.IsEmpty()) return;
            result.ResultDataTable.Columns["SPECCUSTNAME"].Caption = "名称";       
            DataSource ds = new DataSource();
            ds.DisplayMember = "SPECCUSTNAME";
            ds.ValueMember = "SPECCUSTNAME";
            ds.Data = result.ResultDataTable;
            var field = this.View.GetControl<SearchLookUpEdit>(Const_StockAreaPlan.Head_Field_FSpecCustName);
            field.Bind(ds);
        }
        /// <summary>
        /// 绑定产品专用
        /// </summary>
        private void BindSpecProductName()
        {
            OperateResult result =
                        StockAreaPlanServiceHelper.GetSpecProductName(this.Context);
            if (result.ResultDataTable.IsEmpty()) return;
            result.ResultDataTable.Columns["SPECPRODNAME"].Caption = "名称";
            DataSource ds = new DataSource();
            ds.DisplayMember = "SPECPRODNAME";
            ds.ValueMember = "SPECPRODNAME";
            ds.Data = result.ResultDataTable;
            var field = this.View.GetControl<SearchLookUpEdit>(Const_StockAreaPlan.Head_Field_FSpecProdName);
            field.Bind(ds);
        }
        /// <summary>
        /// 绑定合格证样式
        /// </summary>
        private void BindTrademarkStyle()
        {
            DataTable dtSource = new DataTable();
            dtSource.Columns.Add("TrademarkStyle", typeof(string));
            dtSource.Columns["TrademarkStyle"].Caption = "名称";
            DataRow dRow1 = dtSource.NewRow();
            dRow1["TrademarkStyle"] = "白证";
            DataRow dRow2 = dtSource.NewRow();
            dRow2["TrademarkStyle"] = "出口证";
            dtSource.Rows.Add(dRow1);
            dtSource.Rows.Add(dRow2);
            dtSource.AcceptChanges();
            DataSource ds = new DataSource();
            ds.DisplayMember = "TrademarkStyle";
            ds.ValueMember = "TrademarkStyle";
            ds.Data = dtSource;
            var field = this.View.GetControl<SearchLookUpEdit>(Const_StockAreaPlan.Head_Field_FTrademarkStyle);
            field.Bind(ds);
        }
        /// <summary>
        /// 绑定计重方式
        /// </summary>
        private void BindWeightMode()
        {
            DataTable dtSource = new DataTable();
            dtSource.Columns.Add("WeightModeDesc", typeof(string));
            dtSource.Columns["WeightModeDesc"].Caption = "名称";
            DataRow dRow1 = dtSource.NewRow();
            dRow1["WeightModeDesc"] = "计重";
            DataRow dRow2 = dtSource.NewRow();
            dRow2["WeightModeDesc"] = "计米";
            DataRow dRow3 = dtSource.NewRow();
            dRow3["WeightModeDesc"] = "注米";
            DataRow dRow4 = dtSource.NewRow();
            dRow4["WeightModeDesc"] = "正丝";
            DataRow dRow5 = dtSource.NewRow();
            dRow5["WeightModeDesc"] = "反丝";
            dtSource.Rows.Add(dRow1);
            dtSource.Rows.Add(dRow2);
            dtSource.Rows.Add(dRow3);
            dtSource.Rows.Add(dRow4);
            dtSource.Rows.Add(dRow5);
            dtSource.AcceptChanges();
            DataSource ds = new DataSource();
            ds.DisplayMember = "WeightModeDesc";
            ds.ValueMember = "WeightModeDesc";
            ds.Data = dtSource;
            var field = this.View.GetControl<SearchLookUpEdit>(Const_StockAreaPlan.Head_Field_FWeightMode);
            field.Bind(ds);
        }
        /// <summary>
        /// 绑定令张数
        /// </summary>
        private void BindSlidesOfReam()
        {
            //OperateResult result =
            //            StockAreaPlanServiceHelper.GetSlidesOfReam(this.Context);
            //if (result.ResultDataTable.IsEmpty()) return;
            //result.ResultDataTable.Columns["SLIDESOFREAM"].Caption = "名称";
            
            //DataSource ds = new DataSource();
            //ds.DisplayMember = "SLIDESOFREAM";
            //ds.Data = result.ResultDataTable;
            //var field = this.View.GetControl<ComboBoxEdit>(Const_StockAreaPlan.Head_Field_FDiameterOrSlides);
            //field.Bind(ds);
        }
        private void BindEditData()
        {
            object data = this.Context.GetOption("FID");
            this.Context.RemoveOption("FID");
            if (data == null) return;
            DataSet resultData = (DataSet)data;
            form.Text = "库区属性定义(修改)";
            DataRow row = resultData.Tables["T1"].Rows[0];
            this.View.SetValue(Const_StockAreaPlan.Head_Field_FID,
            row[Const_StockAreaPlan.Head_Column_FID]);
            this.View.SetValue(Const_StockAreaPlan.Head_Field_FNumber,
            row[Const_StockAreaPlan.Head_Column_FNumber]);
            this.View.SetValue(Const_StockAreaPlan.Head_Field_FName,
            row[Const_StockAreaPlan.Head_Column_FName]);
            this.View.SetValue(Const_StockAreaPlan.Head_Field_FPaperType,
            row[Const_StockAreaPlan.Head_Column_FPaperType]);
            this.View.SetValue(Const_StockAreaPlan.Head_Field_FCreateDate,
            row[Const_StockAreaPlan.Head_Column_FCreateDate]);
            this.View.SetValue(Const_StockAreaPlan.Head_Field_FModifyDate,
            row[Const_StockAreaPlan.Head_Column_FModifyDate]);
            this.View.SetValue(Const_StockAreaPlan.Head_Field_FSpecification,
            row[Const_StockAreaPlan.Head_Column_FSpecification]);
            this.View.SetValue(Const_StockAreaPlan.Head_Field_FPaperGrade,
            row[Const_StockAreaPlan.Head_Column_FPaperGrade]);
            this.View.SetValue(Const_StockAreaPlan.Head_Field_FCertification,
            row[Const_StockAreaPlan.Head_Column_FCertification]);
            this.View.SetValue(Const_StockAreaPlan.Head_Field_FTransportType,
            row[Const_StockAreaPlan.Head_Column_FTransportType]);
            object cordiameter = row.GetValue<object>(Const_StockAreaPlan.Head_Column_FCoreDiameterOrReam);
            this.View.SetValue(Const_StockAreaPlan.Head_Field_FCoreDiameterOrReam, ObjectUtils.TrimEndZero(cordiameter));
            object diameter = row.GetValue<object>(Const_StockAreaPlan.Head_Column_FDiameterOrSlides);
            this.View.SetValue(Const_StockAreaPlan.Head_Field_FDiameterOrSlides, ObjectUtils.TrimEndZero(diameter));
            object length = row.GetValue<object>(Const_StockAreaPlan.Head_Column_FLength);
            this.View.SetValue(Const_StockAreaPlan.Head_Field_FLength, ObjectUtils.TrimEndZero(length));
            this.View.SetValue(Const_StockAreaPlan.Head_Field_FTrademarkStyle,
            row[Const_StockAreaPlan.Head_Column_FTrademarkStyle]);
            this.View.SetValue(Const_StockAreaPlan.Head_Field_FSpecCustName,
            row[Const_StockAreaPlan.Head_Column_FSpecCustName]);
            this.View.SetValue(Const_StockAreaPlan.Head_Field_FSpecProdName,
            row[Const_StockAreaPlan.Head_Column_FSpecProdName]);
            this.View.SetValue(Const_StockAreaPlan.Head_Field_FPackType,
            row[Const_StockAreaPlan.Head_Column_FPackType]);
            this.View.SetValue(Const_StockAreaPlan.Head_Field_FDeliverDate,
            row[Const_StockAreaPlan.Head_Column_FDeliverDate]);
            this.View.SetValue(Const_StockAreaPlan.Head_Field_FMemo,
            row[Const_StockAreaPlan.Head_Column_FMemo]);
            this.View.SetValue(Const_StockAreaPlan.Head_Field_FWeightMode,
            row[Const_StockAreaPlan.Head_Column_FWeightMode]);
            this.View.SetValue(Const_StockAreaPlan.Head_Field_FColor,
            row[Const_StockAreaPlan.Head_Column_FColor]);
            this.View.SetValue(Const_StockAreaPlan.Head_Field_FPoNumber,
            row[Const_StockAreaPlan.Head_Field_FPoNumber]);
            this.View.Model.isSaved = true;
            this.View.Model.IsDirty = false;
        }
        private void ClearEditData()
        {
            form.Text = "库区属性定义(新增)";
            this.View.SetValue(Const_StockAreaPlan.Head_Field_FID,
            SequenceGuid.NewGuid().ToString());
            this.View.SetValue(Const_StockAreaPlan.Head_Field_FNumber, string.Empty);
            this.View.SetValue(Const_StockAreaPlan.Head_Field_FName, string.Empty);
            this.View.SetValue(Const_StockAreaPlan.Head_Field_FPaperType, "1");
            this.View.SetValue(Const_StockAreaPlan.Head_Field_FCreateDate, DateTime.Now);
            this.View.SetValue(Const_StockAreaPlan.Head_Field_FModifyDate, null);
            this.View.SetValue(Const_StockAreaPlan.Head_Field_FSpecification, null);
            this.View.SetValue(Const_StockAreaPlan.Head_Field_FPaperGrade, null);
            this.View.SetValue(Const_StockAreaPlan.Head_Field_FCertification, null);
            this.View.SetValue(Const_StockAreaPlan.Head_Field_FTransportType, null);
            this.View.SetValue(Const_StockAreaPlan.Head_Field_FCoreDiameterOrReam, null);
            this.View.SetValue(Const_StockAreaPlan.Head_Field_FCoreDiameterOrReam, null);
            this.View.SetValue(Const_StockAreaPlan.Head_Field_FDiameterOrSlides, null);
            this.View.SetValue(Const_StockAreaPlan.Head_Field_FDiameterOrSlides, null);
            this.View.SetValue(Const_StockAreaPlan.Head_Field_FLength, null);
            this.View.SetValue(Const_StockAreaPlan.Head_Field_FTrademarkStyle, null);
            this.View.SetValue(Const_StockAreaPlan.Head_Field_FSpecCustName, null);
            this.View.SetValue(Const_StockAreaPlan.Head_Field_FSpecProdName, null);
            this.View.SetValue(Const_StockAreaPlan.Head_Field_FPackType, null);
            this.View.SetValue(Const_StockAreaPlan.Head_Field_FDeliverDate, DateTime.Now);
            this.View.SetValue(Const_StockAreaPlan.Head_Field_FMemo, null);
            this.View.SetValue(Const_StockAreaPlan.Head_Field_FWeightMode, null);
            this.View.SetValue(Const_StockAreaPlan.Head_Field_FColor, null);
            this.View.SetValue(Const_StockAreaPlan.Head_Field_FPoNumber, null);
            this.View.Model.isSaved = false;
            this.View.Model.IsDirty = false;
        }
    }
}
