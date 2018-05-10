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
    public partial class StockAreaPlanPlugIn : AbstractBillPlugIn
    {
        XtraForm form;
        public StockAreaPlanPlugIn(StockAreaPlanView view) : base(view)
        {
            form = this.View.GetControl<XtraForm>("form");
        }
        public override void OnLoad()
        {
            base.OnLoad();
            BindData();
        }
    
        public override void ButtonClick(ButtonClickEventArgs e)
        {
            base.ButtonClick(e);
            switch (e.sender)
            {
                case "bt_Save":
                    break;
                case "bt_New":
                    XtraMessageBox.Show("确定放弃当前界面数据么!");
                    break;
                case "bt_Delete":
                    XtraMessageBox.Show("删除成功!");
                    break;
            }
        }
        public override void DataChanged(DataChangedEventArgs e)
        {
            base.DataChanged(e);
            switch (e.sender)
            {
                case "lu_PaperType":
                    var lengthField = this.View.GetControl<LookUpEdit>("lu_Length");
                    var filed = this.View.GetControl<LookUpEdit>("lu_PaperType");
                    string paperType = Convert.ToString(filed.EditValue);
                    if (paperType == "1")//卷筒
                    {
                        BindCoreDiameter();
                        BindRollDiameter();
                        BindRollLength();
                        lengthField.Enabled = true;
                    }
                    else if (paperType == "2")//平板纸
                    {
                        BindReam();
                        BindSlidesOfReam();
                        lengthField.Properties.Dispose();
                        lengthField.Enabled = false;
                    }
                    break;
            }
        }
        #region Fun
        private void BindData()
        {
            BindPaperType();
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
        }
        #endregion
    }
}
