using System;
using Chaint.Instock.Core;
using Chaint.Common.Core;
using Chaint.Common.Core.Utils;
namespace Chaint.Instock.Business.PlugIns
{
    partial class StockAreaPlanPlugIn
    {
        /// <summary>
        ///保存数据到数据库
        /// </summary>
        private void SaveToDB()
        {
            this.View.Model.SetValue(Const_StockAreaPlan.Head_Field_FModifyDate, DateTime.Now);
            if (!Validate()) return;
            OperationResult result = this.View.Model.Save();
            if (result.IsSuccess)
            {
                form.Text = "库区属性定义(修改)";
                DateTime modify =
                    this.View.Model.GetValue<DateTime>(Const_StockAreaPlan.Head_Field_FModifyDate);
                this.View.SetValue(Const_StockAreaPlan.Head_Field_FModifyDate, modify);
                this.View.Model.IsDirty = false;
                ChaintMessageBox.Show("保存成功！");
            }
            else
            {
                ChaintMessageBox.Show("保存失败,请查看日志!");
            }
        }

        //保存之前校验
        private bool Validate()
        {
            string paperType =
                this.View.Model.GetValue<string>(Const_StockAreaPlan.Head_Field_FPaperType);
            string fnumber =
                this.View.Model.GetValue<string>(Const_StockAreaPlan.Head_Field_FNumber);
            if (fnumber.IsNullOrEmptyOrWhiteSpace())
            {
                ChaintMessageBox.Show("请输入编码！");
                return false;
            }
            string fname =
                this.View.Model.GetValue<string>(Const_StockAreaPlan.Head_Field_FName);
            if (fname.IsNullOrEmptyOrWhiteSpace())
            {
                ChaintMessageBox.Show("请输入名称！");
                return false;
            }
            //string specification =
            //    this.View.Model.GetValue<string>(Const_StockAreaPlan.Head_Field_FSpecification);
            //if (specification.IsNullOrEmptyOrWhiteSpace())
            //{
            //    ChaintMessageBox.Show("请输入规格！");
            //    return false;
            //}
            //string paperGrade =
            //    this.View.Model.GetValue<string>(Const_StockAreaPlan.Head_Field_FPaperGrade);
            //if (paperGrade.IsNullOrEmptyOrWhiteSpace())
            //{
            //    ChaintMessageBox.Show("请输入产品等级！");
            //    return false;
            //}
            //string certification =
            //    this.View.Model.GetValue<string>(Const_StockAreaPlan.Head_Field_FCertification);
            //if (certification.IsNullOrEmptyOrWhiteSpace())
            //{
            //    ChaintMessageBox.Show("请输入产品认证！");
            //    return false;
            //}
            //string transportType =
            //    this.View.Model.GetValue<string>(Const_StockAreaPlan.Head_Field_FTransportType);
            //if (transportType.IsNullOrEmptyOrWhiteSpace())
            //{
            //    ChaintMessageBox.Show("请输入夹板包装！");
            //    return false;
            //}
            //decimal coreDismeterOrReam =
            //    this.View.Model.GetValue<decimal>(Const_StockAreaPlan.Head_Field_FCoreDiameterOrReam);
            //if (coreDismeterOrReam <= 0)
            //{
            //    ChaintMessageBox.Show("请输入纸芯/令数！");
            //    return false;
            //}
            //decimal dismeterOrSlides =
            //    this.View.Model.GetValue<decimal>(Const_StockAreaPlan.Head_Field_FDiameterOrSlides);
            //if (dismeterOrSlides <= 0)
            //{
            //    ChaintMessageBox.Show("请输入直径MM/令张数！");
            //    return false;
            //}
            //if (paperType == "1")
            //{
            //    decimal length =
            //     this.View.Model.GetValue<decimal>(Const_StockAreaPlan.Head_Field_FLength);
            //    if (length <= 0)
            //    {
            //        ChaintMessageBox.Show("请输入线长！");
            //        return false;
            //    }
            //}
            //string trademarkStyle =
            //    this.View.Model.GetValue<string>(Const_StockAreaPlan.Head_Field_FTrademarkStyle);
            //if (trademarkStyle.IsNullOrEmptyOrWhiteSpace())
            //{
            //    ChaintMessageBox.Show("请输入合格证样式！");
            //    return false;
            //}
            //string specCustName =
            //     this.View.Model.GetValue<string>(Const_StockAreaPlan.Head_Field_FSpecCustName);
            //if (specCustName.IsNullOrEmptyOrWhiteSpace())
            //{
            //    ChaintMessageBox.Show("请输入客户专用！");
            //    return false;
            //}
            //string specProdName =
            //    this.View.Model.GetValue<string>(Const_StockAreaPlan.Head_Field_FSpecProdName);
            //if (specProdName.IsNullOrEmptyOrWhiteSpace())
            //{
            //    ChaintMessageBox.Show("请输入产品专用！");
            //    return false;
            //}
            //string packType =
            //    this.View.Model.GetValue<string>(Const_StockAreaPlan.Head_Field_FPackType);
            //if (packType.IsNullOrEmptyOrWhiteSpace())
            //{
            //    ChaintMessageBox.Show("请输入包装方式！");
            //    return false;
            //}
            //DateTime deliverDate =
            //    this.View.Model.GetValue<DateTime>(Const_StockAreaPlan.Head_Field_FDeliverDate);
            //if (deliverDate == null || deliverDate == DateTime.MinValue)
            //{
            //    ChaintMessageBox.Show("请输入交货日期！");
            //    return false;
            //}
            //string weightMode =
            //    this.View.Model.GetValue<string>(Const_StockAreaPlan.Head_Field_FWeightMode);
            //if (weightMode.IsNullOrEmptyOrWhiteSpace())
            //{
            //    ChaintMessageBox.Show("请输入计重方式！");
            //    return false;
            //}
            //string color =
            //    this.View.Model.GetValue<string>(Const_StockAreaPlan.Head_Field_FColor);
            //if (color.IsNullOrEmptyOrWhiteSpace())
            //{
            //    ChaintMessageBox.Show("请输入产品色相！");
            //    return false;
            //}
            //string poNumber =
            //    this.View.Model.GetValue<string>(Const_StockAreaPlan.Head_Field_FPoNumber);
            //if (poNumber.IsNullOrEmptyOrWhiteSpace())
            //{
            //    ChaintMessageBox.Show("请输入订单编号！");
            //    return false;
            //}
            //string memo =
            //    this.View.Model.GetValue<string>(Const_StockAreaPlan.Head_Field_FMemo);
            //if (memo.IsNullOrEmptyOrWhiteSpace())
            //{
            //    ChaintMessageBox.Show("请输入备注！");
            //    return false;
            //}
            return true;
        }
    }
}
