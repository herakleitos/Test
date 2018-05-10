using System;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using Chaint.Common.Core;
using Chaint.Common.Core.Const;
using Chaint.Instock.Core;
using Chaint.Common.Core.Utils;
using Chaint.Common.Core.EventArgs;
using Chaint.Instock.Business.View;
using Chaint.Instock.Business.Controler;
using System.Windows.Forms;

namespace Chaint.Instock.Business
{
    public partial class StockAreaPlan : XtraForm
    {
        private Context context;
        private StockAreaPlanControler controler;
        public StockAreaPlanView view;
        public StockAreaPlan()
        {
        }
        public StockAreaPlan(Context ctx)
        {
            context = ctx;
            InitializeComponent();
            RegisterEvent();
            InitView();
            InitControler();
        }
        private void OnLoad(object sender, EventArgs e)
        {
            controler.Invoke(Const_Event.OnLoad);
        }
        private void InitControler()
        {
            controler = new StockAreaPlanControler(view);
        }
        private void InitView()
        {
            this.view = new StockAreaPlanView(context);
            this.view.AddControl(Const_StockAreaPlan.Base_Form, this);
            this.view.AddControl(Const_StockAreaPlan.Head_Menu_Save, btSave);
            this.view.AddControl(Const_StockAreaPlan.Head_Menu_New, btNew);
            this.view.AddControl(Const_StockAreaPlan.Head_Menu_Delete, btDelete);
            this.view.AddControl(Const_StockAreaPlan.Head_Field_FID, this.FID);
            this.view.AddControl(Const_StockAreaPlan.Head_Field_FNumber, this.FNumber);
            this.view.AddControl(Const_StockAreaPlan.Head_Field_FName, this.FName);
            this.view.AddControl(Const_StockAreaPlan.Head_Field_FPaperType, this.FPaperType);
            this.view.AddControl(Const_StockAreaPlan.Head_Field_FSpecification, this.FSpecification);
            this.view.AddControl(Const_StockAreaPlan.Head_Field_FCoreDiameterOrReam, this.FCoreDiameterOrReam);
            this.view.AddControl(Const_StockAreaPlan.Head_Field_FLength, this.FLength);
            this.view.AddControl(Const_StockAreaPlan.Head_Field_FDiameterOrSlides, this.FDiameterOrSlides);
            this.view.AddControl(Const_StockAreaPlan.Head_Field_FTransportType, this.FTransportType);
            this.view.AddControl(Const_StockAreaPlan.Head_Field_FWeightMode, this.FWeightMode);
            this.view.AddControl(Const_StockAreaPlan.Head_Field_FMemo, this.FMemo);
            this.view.AddControl(Const_StockAreaPlan.Head_Field_FPoNumber, this.FPoNumber);
            this.view.AddControl(Const_StockAreaPlan.Head_Field_FCertification, this.FCertification);
            this.view.AddControl(Const_StockAreaPlan.Head_Field_FTrademarkStyle, this.FTrademarkStyle);
            this.view.AddControl(Const_StockAreaPlan.Head_Field_FPackType, this.FPackType);
            this.view.AddControl(Const_StockAreaPlan.Head_Field_FSpecCustName, this.FSpecCustName);
            this.view.AddControl(Const_StockAreaPlan.Head_Field_FPaperGrade, this.FPaperGrade);
            this.view.AddControl(Const_StockAreaPlan.Head_Field_FSpecProdName, this.FSpecProdName);
            this.view.AddControl(Const_StockAreaPlan.Head_Field_FDeliverDate, this.FDeliverDate);
            this.view.AddControl(Const_StockAreaPlan.Head_Field_FColor, this.FColor);
            this.view.AddControl(Const_StockAreaPlan.Head_Field_FSpCustomer, this.FSpCustomer);
            this.view.AddControl(Const_StockAreaPlan.Head_Field_FCreateDate, this.FCreateDate);
            this.view.AddControl(Const_StockAreaPlan.Head_Field_FModifyDate, this.FModifyDate);
            //初始化view时，给fid赋值。
            view.SetValue(Const_StockAreaPlan.Head_Field_FID, SequenceGuid.NewGuid().ToString());
        }
        #region 注册控件事件
        private void RegisterEvent()
        {
            this.Disposed += new EventHandler(this.OnDisposed);
            this.Load += new EventHandler(this.OnLoad);
            this.btDelete.ItemClick += new ItemClickEventHandler(this.bt_Delete_ItemClick);
            this.btNew.ItemClick += new ItemClickEventHandler(this.bt_New_ItemClick);
            this.btSave.ItemClick += new ItemClickEventHandler(this.bt_Save_ItemClick);
            this.FName.EditValueChanged += new System.EventHandler(this.FName_EditValueChanged);
            this.FMemo.EditValueChanged += new System.EventHandler(this.FMemo_EditValueChanged);
            this.FPaperType.EditValueChanged += new System.EventHandler(this.FPaperType_EditValueChanged);
            this.FNumber.EditValueChanged += new System.EventHandler(this.FNumber_EditValueChanged);
            this.FPoNumber.EditValueChanged += new System.EventHandler(this.FPoNumber_EditValueChanged);
            this.FDeliverDate.EditValueChanged += new System.EventHandler(this.FDeliverDate_ValueChanged);
            this.FSpecification.EditValueChanged += new System.EventHandler(this.FSpecification_EditValueChanged);
            this.FPaperGrade.EditValueChanged += new System.EventHandler(this.FPaperGrade_EditValueChanged);
            this.FCertification.EditValueChanged += new System.EventHandler(this.FCertification_EditValueChanged);
            this.FTransportType.EditValueChanged += new System.EventHandler(this.FTransportType_EditValueChanged);
            this.FColor.EditValueChanged += new System.EventHandler(this.FColor_EditValueChanged);
            this.FTrademarkStyle.EditValueChanged += new System.EventHandler(this.FTrademarkStyle_EditValueChanged);
            this.FSpecProdName.EditValueChanged += new System.EventHandler(this.FSpecProdName_EditValueChanged);
            this.FPackType.EditValueChanged += new System.EventHandler(this.FPackType_EditValueChanged);
            this.FWeightMode.EditValueChanged += new System.EventHandler(this.FWeightMode_EditValueChanged);
            this.FSpecCustName.EditValueChanged += new System.EventHandler(this.FSpecCustName_EditValueChanged);

            this.FCoreDiameterOrReam.EditValueChanged += new EventHandler(this.FCoreDiameterOrReam_EditValueChanged);
            this.FLength.EditValueChanged += new EventHandler(this.FLength_EditValueChanged);
            this.FDiameterOrSlides.EditValueChanged += new EventHandler(this.FDiameterOrSlides_EditValueChanged);

            this.FCoreDiameterOrReam.KeyPress += new System.Windows.Forms.KeyPressEventHandler(NumberOnlyValidate);
            this.FLength.KeyPress += new System.Windows.Forms.KeyPressEventHandler(NumberOnlyValidate);
            this.FDiameterOrSlides.KeyPress += new System.Windows.Forms.KeyPressEventHandler(NumberOnlyValidate);
        }

        private void OnDisposed(object sender, EventArgs e)
        {
            this.Load -= new EventHandler(this.OnLoad);
            this.btDelete.ItemClick -= new ItemClickEventHandler(this.bt_Delete_ItemClick);
            this.btNew.ItemClick -= new ItemClickEventHandler(this.bt_New_ItemClick);
            this.btSave.ItemClick -= new ItemClickEventHandler(this.bt_Save_ItemClick);
            this.FName.EditValueChanged -= new System.EventHandler(this.FName_EditValueChanged);
            this.FMemo.EditValueChanged -= new System.EventHandler(this.FMemo_EditValueChanged);
            this.FPaperType.EditValueChanged -= new System.EventHandler(this.FPaperType_EditValueChanged);
            this.FNumber.EditValueChanged -= new System.EventHandler(this.FNumber_EditValueChanged);
            this.FPoNumber.EditValueChanged -= new System.EventHandler(this.FPoNumber_EditValueChanged);
            this.FDeliverDate.EditValueChanged -= new System.EventHandler(this.FDeliverDate_ValueChanged);
            this.FSpecification.EditValueChanged -= new System.EventHandler(this.FSpecification_EditValueChanged);
            this.FPaperGrade.EditValueChanged -= new System.EventHandler(this.FPaperGrade_EditValueChanged);
            this.FCertification.EditValueChanged -= new System.EventHandler(this.FCertification_EditValueChanged);
            this.FTransportType.EditValueChanged -= new System.EventHandler(this.FTransportType_EditValueChanged);
            this.FColor.EditValueChanged -= new System.EventHandler(this.FColor_EditValueChanged);
            this.FTrademarkStyle.EditValueChanged -= new System.EventHandler(this.FTrademarkStyle_EditValueChanged);
            this.FSpecProdName.EditValueChanged -= new System.EventHandler(this.FSpecProdName_EditValueChanged);
            this.FPackType.EditValueChanged -= new System.EventHandler(this.FPackType_EditValueChanged);
            this.FWeightMode.EditValueChanged -= new System.EventHandler(this.FWeightMode_EditValueChanged);
            this.FSpecCustName.EditValueChanged -= new System.EventHandler(this.FSpecCustName_EditValueChanged);

            this.FCoreDiameterOrReam.EditValueChanged -= new EventHandler(this.FCoreDiameterOrReam_EditValueChanged);
            this.FLength.EditValueChanged -= new EventHandler(this.FLength_EditValueChanged);
            this.FDiameterOrSlides.EditValueChanged -= new EventHandler(this.FDiameterOrSlides_EditValueChanged);

            this.FCoreDiameterOrReam.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(NumberOnlyValidate);
            this.FLength.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(NumberOnlyValidate);
            this.FDiameterOrSlides.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(NumberOnlyValidate);
            this.view.Dispose();
        }
        #endregion
        private void NumberOnlyValidate(object sender, KeyPressEventArgs e)
        {
            object value = (sender as ComboBoxEdit).EditValue;
            bool isVal = controler.IsNumberVal(value,e.KeyChar);
            if (!isVal)//如果不是数字或者
            {
                e.Handled = true;
            }
        }
        private void bt_Save_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string oldText = this.btSave.Caption;
            this.btSave.Caption = "请稍后";
            this.btSave.Enabled = false;
            ButtonClickEventArgs args = new ButtonClickEventArgs();
            args.Sender = Const_StockAreaPlan.Head_Menu_Save;
            controler.Invoke(Const_Event.ButtonClick, args);
            this.btSave.Caption = oldText;
            this.btSave.Enabled = true;
        }

        private void bt_New_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string oldText = this.btNew.Caption;
            this.btNew.Caption = "请稍后";
            this.btNew.Enabled = false;
            ButtonClickEventArgs args = new ButtonClickEventArgs();
            args.Sender = Const_StockAreaPlan.Head_Menu_New;
            controler.Invoke(Const_Event.ButtonClick, args);
            this.btNew.Caption = oldText;
            this.btNew.Enabled = true;
        }

        private void bt_Delete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string oldText = this.btDelete.Caption;
            this.btDelete.Caption = "请稍后";
            this.btDelete.Enabled = false;
            ButtonClickEventArgs args = new ButtonClickEventArgs();
            args.Sender = Const_StockAreaPlan.Head_Menu_Delete;
            controler.Invoke(Const_Event.ButtonClick, args);
            this.btDelete.Caption = oldText;
            this.btDelete.Enabled = true;
        }
        private void FPaperType_EditValueChanged(object sender, EventArgs e)
        {
            this.view.Sync(Const_StockAreaPlan.Head_Field_FPaperType);
            DataChangedEventArgs args = new DataChangedEventArgs();
            args.Sender = Const_StockAreaPlan.Head_Field_FPaperType;
            args.Row = -1;
            controler.Invoke(Const_Event.DataChanged, args);
        }
        private void FNumber_EditValueChanged(object sender, EventArgs e)
        {
            this.view.Sync(Const_StockAreaPlan.Head_Field_FNumber);
        }
        private void FName_EditValueChanged(object sender, EventArgs e)
        {
            this.view.Sync(Const_StockAreaPlan.Head_Field_FName);
        }
        private void FSpecification_EditValueChanged(object sender, EventArgs e)
        {
            this.view.Sync(Const_StockAreaPlan.Head_Field_FSpecification);
        }

        private void FPaperGrade_EditValueChanged(object sender, EventArgs e)
        {
            this.view.Sync(Const_StockAreaPlan.Head_Field_FPaperGrade);
        }

        private void FCertification_EditValueChanged(object sender, EventArgs e)
        {
            this.view.Sync(Const_StockAreaPlan.Head_Field_FCertification);
        }

        private void FTransportType_EditValueChanged(object sender, EventArgs e)
        {
            this.view.Sync(Const_StockAreaPlan.Head_Field_FTransportType);
        }

        private void FColor_EditValueChanged(object sender, EventArgs e)
        {
            this.view.Sync(Const_StockAreaPlan.Head_Field_FColor);
        }

        private void FDeliverDate_ValueChanged(object sender, EventArgs e)
        {
            this.view.Sync(Const_StockAreaPlan.Head_Field_FDeliverDate);
        }

        private void FMemo_EditValueChanged(object sender, EventArgs e)
        {
            this.view.Sync(Const_StockAreaPlan.Head_Field_FMemo);
        }

        private void FPoNumber_EditValueChanged(object sender, EventArgs e)
        {
            this.view.Sync(Const_StockAreaPlan.Head_Field_FPoNumber);
        }

        private void FTrademarkStyle_EditValueChanged(object sender, EventArgs e)
        {
            this.view.Sync(Const_StockAreaPlan.Head_Field_FTrademarkStyle);
        }

        private void FLength_EditValueChanged(object sender, EventArgs e)
        {
            this.view.Sync(Const_StockAreaPlan.Head_Field_FLength);
        }

        private void FDiameterOrSlides_EditValueChanged(object sender, EventArgs e)
        {
            this.view.Sync(Const_StockAreaPlan.Head_Field_FDiameterOrSlides);
        }

        private void FSpecProdName_EditValueChanged(object sender, EventArgs e)
        {
            this.view.Sync(Const_StockAreaPlan.Head_Field_FSpecProdName);
        }

        private void FPackType_EditValueChanged(object sender, EventArgs e)
        {
            this.view.Sync(Const_StockAreaPlan.Head_Field_FPackType);
        }

        private void FWeightMode_EditValueChanged(object sender, EventArgs e)
        {
            this.view.Sync(Const_StockAreaPlan.Head_Field_FWeightMode);
        }

        private void FSpecCustName_EditValueChanged(object sender, EventArgs e)
        {
            this.view.Sync(Const_StockAreaPlan.Head_Field_FSpecCustName);
        }

        private void FCoreDiameterOrReam_EditValueChanged(object sender, EventArgs e)
        {
            this.view.Sync(Const_StockAreaPlan.Head_Field_FCoreDiameterOrReam);
        }

        private void FSpecification_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.view.Sync(Const_StockAreaPlan.Head_Field_FSpecification);
        }

        private void btList_ItemClick(object sender, ItemClickEventArgs e)
        {
            string oldText = this.btList.Caption;
            this.btList.Caption = "请稍后";
            this.btList.Enabled = false;
            ButtonClickEventArgs args = new ButtonClickEventArgs();
            args.Sender = Const_StockAreaPlan.Head_Menu_List;
            controler.Invoke(Const_Event.ButtonClick, args);
            this.btList.Caption = oldText;
            this.btList.Enabled = true;
        }

        private void FSpCustomer_EditValueChanged(object sender, EventArgs e)
        {
            this.view.Sync(Const_StockAreaPlan.Head_Field_FSpCustomer);
        }
    }
}
