using System;
using DevExpress.XtraEditors;
using Chaint.Common.Core;
using Chaint.Common.Core.Const;
using Chaint.Common.BasePlugIn;
using Chaint.Instock.Core;
using Chaint.Common.Core.EventArgs;
using Chaint.Instock.Business.View;
using Chaint.Common.Core.AppConfig;
using System.Windows.Forms;

namespace Chaint.Instock.Business.PlugIns
{
    public partial class StockAreaPlanPlugIn : AbstractBillPlugIn
    {
        XtraForm form;
        private AppConfig_INI appConfiger;
        public StockAreaPlanPlugIn(StockAreaPlanView view) : base(view)
        {
            form = this.View.GetControl<XtraForm>(Const_StockAreaPlan.Base_Form);
            appConfiger = new AppConfig_INI(view.Context.DevicesConfigFilePath);
        }
        public override void OnSetBusinessInfo()
        {
            base.OnSetBusinessInfo();
            BindData();
        }
        public override void OnBind()
        {
            base.OnBind();
            SetDefaultData();
            //如果是从列表打开的，绑定界面数据
            BindEditData();
        }
        public override void ButtonClick(ButtonClickEventArgs e)
        {
            base.ButtonClick(e);
            switch (e.Sender)
            {
                case Const_StockAreaPlan.Head_Menu_Save:
                    SaveToDB();
                    break;
                case Const_StockAreaPlan.Head_Menu_New:
                    RefreshPage();
                    break;
                case Const_StockAreaPlan.Head_Menu_Delete:
                    Delete();
                    break;
                case Const_StockAreaPlan.Head_Menu_List:
                    this.View.Open(Const_Option.Const_CommonList, Const_Option.Const_StockAreaPlan);
                    break;
            }
        }

        public override void DataChanged(DataChangedEventArgs e)
        {
            base.DataChanged(e);
            switch (e.Sender)
            {
                case Const_StockAreaPlan.Head_Field_FPaperType:
                    var lengthField = this.View.GetControl<ComboBoxEdit>(Const_StockAreaPlan.Head_Field_FLength);
                    string paperType =
                        this.View.GetValue<string>(Const_StockAreaPlan.Head_Field_FPaperType);
                    if (paperType == "1")//卷筒
                    {

                        BindCoreDiameter();
                        BindRollDiameter();
                        BindRollLength();
                        // lengthField.Enabled = true;
                    }
                    else if (paperType == "2")//平板纸
                    {
                        BindReam();
                        BindSlidesOfReam();
                        lengthField.Properties.Dispose();
                        // lengthField.Enabled = false;
                    }
                    BindPackType(paperType);
                    break;
            }
        }
        #region Fun
        private void BindData()
        {
            BindPaperType();

            BindSpecification();
            BindCoreDiameter();
            BindPaperGrade();
            BindCertification();
            BindPackType("1");
            BindTransportType();
            BindRollDiameter();
            BindRollLength();
            BindColor();
            BindSpecCustomerName();
            BindSpecProductName();
            BindTrademarkStyle();
            BindWeightMode();
            BindSpCustomer();
        }
        private void RefreshPage()
        {
            bool isNew = true;
            if (this.View.Model.IsDirty)
            {
                DialogResult result = ChaintMessageBox.ShowConfirmDialog("确定放弃当前数据么？");
                if (result != DialogResult.Yes)
                {
                    isNew = false;
                }
            }
            if (isNew)
            {
                ClearEditData();
            }
        }
        private void Delete()
        {
            if (!this.View.Model.isSaved)
            {
                ChaintMessageBox.Show("当前数据未保存，无法删除!");
                return;
            }
            bool isDelete = true;
            DialogResult result = ChaintMessageBox.ShowConfirmDialog("确定删除当前数据么？");
            if (result != DialogResult.Yes)
            {
                isDelete = false;
            }
            if (isDelete)
            {
                OperationResult delResult = this.View.Model.Delete();
                if (!delResult.IsSuccess)
                {
                    ChaintMessageBox.Show("删除失败，请查看日志！");
                    return;
                }
                ChaintMessageBox.Show("删除成功！");
                ClearEditData();
            }
        }
        /// <summary>
        /// 设置默认值
        /// </summary>
        private void SetDefaultData()
        {
            string paperType = appConfiger.GetValue("PAPERTYPE", "Type", "2");
            this.View.SetValue(Const_StockAreaPlan.Head_Field_FPaperType, paperType);
            this.View.SetValue(Const_StockAreaPlan.Head_Field_FCreateDate, DateTime.Now);
            this.View.SetValue(Const_StockAreaPlan.Head_Field_FDeliverDate, DateTime.Now);
            this.View.Model.IsDirty = false;
        }
        #endregion
    }
}
