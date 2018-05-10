using System;
using System.Data;
using System.Data.SqlClient;
using DevExpress.XtraEditors;
using DevExpress.XtraBars;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraLayout;
using Chaint.Common.Core.Utils;
using Chaint.Common.BasePlugIn;
using Chaint.Common.Core.EventArgs;
using Chaint.Instock.Core;
using Chaint.Instock.Business.View;
using System.Collections.Generic;
using Chaint.Common.Core;
using Chaint.Common.Entity.Utils;
using Chaint.Common.ServiceHelper;
using System.Windows.Forms;

namespace Chaint.Instock.Business.PlugIns
{
    public partial class StockAreaDataQueryPlugIn : AbstractBillPlugIn
    {
        XtraForm form;
        public StockAreaDataQueryPlugIn(StockAreaDataQueryView view) : base(view)
        {
            form = this.View.GetControl<XtraForm>(Const_StockAreaDataQuery.Base_Form);
        }
        public override void OnSetBusinessInfo()
        {
            base.OnSetBusinessInfo();
            InitView();
        }

        public override void OnBind()
        {
            base.OnBind();
            BindData();
            this.View.SetValue(Const_StockAreaDataQuery.Head_Field_FBeginDate, DateTime.Now);
            this.View.SetValue(Const_StockAreaDataQuery.Head_Field_FEndDate, DateTime.Now.AddDays(1));
        }
        public override void ButtonClick(ButtonClickEventArgs e)
        {
            base.ButtonClick(e);
            switch (e.Sender)
            {
                case Const_StockAreaDataQuery.Head_Menu_Query:
                    GetData();
                    break;
                case Const_StockAreaDataQuery.Head_Menu_Export:
                    ExportToExcel();
                    break;
            }
        }

        public override void DataChanged(DataChangedEventArgs e)
        {
            base.DataChanged(e);
            switch (e.Sender)
            {
                case Const_StockAreaDataQuery.Head_Field_FStock:
                    BindStockArea();
                    break;
                case Const_StockAreaDataQuery.Head_Field_FPaperType:
                    var lengthField = this.View.GetControl<ComboBoxEdit>(Const_StockAreaDataQuery.Head_Field_FLength);
                    string paperType =
                        this.View.GetValue<string>(Const_StockAreaDataQuery.Head_Field_FPaperType);
                    if (paperType == "1")//卷筒
                    {

                        BindCoreDiameter();
                    }
                    else if (paperType == "2")//平板纸
                    {
                        lengthField.Properties.Dispose();
                    }
                    BindPackType(paperType);
                    break;
            }
        }
        private void InitView()
        {
            if (this.Context.CompanyCode == "2")
            {
                var entry = this.View.GetControl<GridView>(Const_StockAreaDataQuery.Entry_Entry);
                foreach (GridColumn item in entry.Columns)
                {
                    if (item.Name.EqualIgnorCase(Const_StockAreaDataQuery.Entry_Column_FSpecification)
                        || item.Name.EqualIgnorCase(Const_StockAreaDataQuery.Entry_Column_FStockAreaPlan)
                        || item.Name.EqualIgnorCase(Const_StockAreaDataQuery.Entry_Column_FPaperGrade)
                        || item.Name.EqualIgnorCase(Const_StockAreaDataQuery.Entry_Column_FPackType)
                        || item.Name.EqualIgnorCase(Const_StockAreaDataQuery.Entry_Column_FCertification)
                        || item.Name.EqualIgnorCase(Const_StockAreaDataQuery.Entry_Column_FColor)
                        || item.Name.EqualIgnorCase(Const_StockAreaDataQuery.Entry_Column_FCoreDiameterOrReam)
                        || item.Name.EqualIgnorCase(Const_StockAreaDataQuery.Entry_Column_FDeliverDate)
                        || item.Name.EqualIgnorCase(Const_StockAreaDataQuery.Entry_Column_FDiameterOrSlides)
                        || item.Name.EqualIgnorCase(Const_StockAreaDataQuery.Entry_Column_FLength)
                        || item.Name.EqualIgnorCase(Const_StockAreaDataQuery.Entry_Column_FSpCustomer)
                     
                        || item.Name.EqualIgnorCase(Const_StockAreaDataQuery.Entry_Column_FSpecProdName)
                        || item.Name.EqualIgnorCase(Const_StockAreaData.Entry_Column_FPoNumber)
                        || item.Name.EqualIgnorCase(Const_StockAreaDataQuery.Entry_Column_FSpecProdName)
                        || item.Name.EqualIgnorCase(Const_StockAreaDataQuery.Entry_Column_FTrademarkStyle)
                        || item.Name.EqualIgnorCase(Const_StockAreaDataQuery.Entry_Column_FTransportType)
                        || item.Name.EqualIgnorCase(Const_StockAreaDataQuery.Entry_Column_FWeightMode))
                    {
                        item.Visible = false;
                        continue;
                    }
                    if (item.Name.EqualIgnorCase(Const_StockAreaDataQuery.Entry_Column_FSpecCustName))
                    {
                        item.Caption = "客户";
                    }
                }
                //container
                this.View.GetControl<LayoutControlItem>(Const_StockAreaDataQuery.Entry_Container_FPaperGrade).Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                this.View.GetControl<LayoutControlItem>(Const_StockAreaDataQuery.Entry_Container_FTransportType).Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                this.View.GetControl<LayoutControlItem>(Const_StockAreaDataQuery.Entry_Container_FCertification).Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                this.View.GetControl<LayoutControlItem>(Const_StockAreaDataQuery.Entry_Container_FSpecification).Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                this.View.GetControl<LayoutControlItem>(Const_StockAreaDataQuery.Entry_Container_FCoreDiameterOrReam).Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                this.View.GetControl<LayoutControlItem>(Const_StockAreaDataQuery.Entry_Container_FDiameterOrSlides).Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                this.View.GetControl<LayoutControlItem>(Const_StockAreaDataQuery.Entry_Container_FLength).Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                this.View.GetControl<LayoutControlItem>(Const_StockAreaDataQuery.Entry_Container_FTrademarkStyle).Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                this.View.GetControl<LayoutControlItem>(Const_StockAreaDataQuery.Entry_Container_FPoNumber).Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                this.View.GetControl<LayoutControlItem>(Const_StockAreaDataQuery.Entry_Container_FSpecProdName).Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                this.View.GetControl<LayoutControlItem>(Const_StockAreaDataQuery.Entry_Container_FPackType).Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                this.View.GetControl<LayoutControlItem>(Const_StockAreaDataQuery.Entry_Container_FDeliverDate).Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                this.View.GetControl<LayoutControlItem>(Const_StockAreaDataQuery.Entry_Container_FWeightMode).Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                this.View.GetControl<LayoutControlItem>(Const_StockAreaDataQuery.Entry_Container_FColor).Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                this.View.GetControl<LayoutControlItem>(Const_StockAreaDataQuery.Entry_Container_FSpCustomer).Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;

                this.View.GetControl<LayoutControlItem>(Const_StockAreaDataQuery.Entry_Container_FSpecCustName).Text = "客户";
            }
        }
        private void ExportToExcel()
        {
            var gridControl = this.View.GetControl<GridControl>(Const_StockAreaDataQuery.Entry_Control);
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Title = "导出Excel";
            dialog.Filter = "Excel文件(*.xls)|*.xls";
            DialogResult dialogResult = dialog.ShowDialog(form);
            if (dialogResult == DialogResult.OK)
            {
                DevExpress.XtraPrinting.XlsExportOptions options = new DevExpress.XtraPrinting.XlsExportOptions();
                gridControl.ExportToXls(dialog.FileName);
                DevExpress.XtraEditors.XtraMessageBox.Show("保存成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void BindData()
        {
            if (this.Context.CompanyCode != "2")
            {
                BindSpCustomer();
                BindCoreDiameter();
                BindPaperGrade();
                BindCertification();
                BindPackType("1");
                BindTransportType();
                BindColor();
                BindSpecProductName();
                BindTrademarkStyle();
                BindWeightMode();
            }
            BindSpecCustomerName();
            BindProduct();
            BindStock();
            BindPaperType();
        }
    }
}
