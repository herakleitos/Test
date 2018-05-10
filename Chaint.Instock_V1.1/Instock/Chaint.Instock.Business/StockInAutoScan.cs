using System;
using System.Windows.Forms;
using Chaint.Common.Core;
using Chaint.Common.Core.Const;
using Chaint.Instock.Core;
using Chaint.Common.Core.EventArgs;
using System.IO.Ports;
using Chaint.Instock.Business.View;
using Chaint.Instock.Business.Controler;
namespace Chaint.Instock.Business
{
    public partial class StockInAutoScan : DevExpress.XtraEditors.XtraForm
    {
        private Context context;
        public StockInAutoScanView view;
        private StockInAutoScanControler controler;
        public StockInAutoScan(Context ctx)
        {
            context = ctx;
            InitializeComponent();
            RegisterEvent();
            InitView();
            InitControler();
        }

        private void InitControler()
        {
            controler=new StockInAutoScanControler(view);
        }

        private void InitView()
        {
            this.view = new StockInAutoScanView(context);
            this.view.AddControl(Const_StockInAutoScan.Base_Form, this);
            this.view.AddControl(Const_StockInAutoScan.Base_Port, this.spScan);
            this.view.AddControl(Const_StockInAutoScan.Head_Field_FBarcode, this.FBarcode);
            this.view.AddControl(Const_StockInAutoScan.Head_Field_FResult, this.FResult);
            this.view.AddControl(Const_StockInAutoScan.Head_Field_FPlcInfo, this.FPlcInfo);
            this.view.AddControl(Const_StockInAutoScan.Head_Field_FMaterial, this.FMaterial);
            this.view.AddControl(Const_StockInAutoScan.Head_Field_FSpecification, this.FSpecification);
            this.view.AddControl(Const_StockInAutoScan.Head_Field_FCoreDiameterOrReam, 
                this.FCoreDiameterOrReam);
            this.view.AddControl(Const_StockInAutoScan.Head_Field_FDiameterOrSlides, this.FDiameterOrSlides);
            this.view.AddControl(Const_StockInAutoScan.Head_Field_FLength, this.FLength);
            this.view.AddControl(Const_StockInAutoScan.Head_Field_FLayers, this.FLayers);
            this.view.AddControl(Const_StockInAutoScan.Head_Field_FSku, this.FSku);
            this.view.AddControl(Const_StockInAutoScan.Head_Field_FTrademarkStyle, this.FTrademarkStyle);
            this.view.AddControl(Const_StockInAutoScan.Head_Field_FCertification, this.FCertification);
            this.view.AddControl(Const_StockInAutoScan.Head_Field_FWeightMode, this.FWeightMode);
            this.view.AddControl(Const_StockInAutoScan.Head_Field_FWeight, this.FWeight);
            this.view.AddControl(Const_StockInAutoScan.Head_Field_FCount, this.FCount);
            this.view.AddControl(Const_StockInAutoScan.Head_Field_FGrade, this.FGrade);
            this.view.AddControl(Const_StockInAutoScan.Head_Field_FSpecProdName, this.FSpecProdName);
            this.view.AddControl(Const_StockInAutoScan.Head_Field_FSpecCustName, this.FSpecCustName);
            this.view.AddControl(Const_StockInAutoScan.Head_Field_FTransportType, this.FTransportType);
            this.view.AddControl(Const_StockInAutoScan.Head_Field_FPackType, this.FPackType);
            this.view.AddControl(Const_StockInAutoScan.Head_Field_FColor, this.FColor);
            this.view.AddControl(Const_StockInAutoScan.Head_Field_FSpCustomer, this.FSpCustomer);
            this.view.AddControl(Const_StockInAutoScan.Head_Field_FPlanDiameter, this.FPlanDiameter);
            this.view.AddControl(Const_StockInAutoScan.Head_Field_FPlanLength, this.FPlanLength);
            this.view.AddControl(Const_StockInAutoScan.Head_Field_FPoNumber,this.FPoNumber);
            this.view.AddControl(Const_StockInAutoScan.Head_Field_FDirection, this.FDirection);
            this.view.AddControl(Const_StockInAutoScan.Head_Field_FRemark, this.FRemark);
            this.view.AddControl(Const_StockInAutoScan.Head_Field_FOperator, this.FOperator);
            this.view.AddControl(Const_StockInAutoScan.Head_Field_FShift, this.FShift);
            this.view.AddControl(Const_StockInAutoScan.Head_Field_FShiftTime, this.FShiftTime);
            this.view.AddControl(Const_StockInAutoScan.Head_Field_FDateS, this.FDateS);
            this.view.AddControl(Const_StockInAutoScan.Head_Field_FBusinessType, this.FBusinessType);
            this.view.AddControl(Const_StockInAutoScan.Head_Field_FFactory, this.FFactory);
            this.view.AddControl(Const_StockInAutoScan.Head_Field_FWHRemark, this.FWHRemark);
            this.view.AddControl(Const_StockInAutoScan.Head_Field_FWareHouse, this.FWareHouse);
            this.view.AddControl(Const_StockInAutoScan.Head_Field_FMessage, this.FMessage);
            this.view.AddControl(Const_StockInAutoScan.Head_Field_FPlcConStatus, this.FPlcConStatus);
            this.view.AddControl(Const_StockInAutoScan.Head_Field_FScannerConStatus, this.FScannerConStatus);
            this.view.AddControl(Const_StockInAutoScan.Head_Field_FLedConStatus, this.FLedConStatus);
            this.view.AddControl(Const_StockInAutoScan.Head_Field_FScannerStatus, this.FScannerStatus);
            this.view.AddControl(Const_StockInAutoScan.Head_Field_FMonitor, this.FMonitor);
            this.view.AddControl(Const_StockInAutoScan.Head_Control_ButtonMotorStart, this.btMotorStart);
            this.view.AddControl(Const_StockInAutoScan.Head_Control_ButtonUseScanner, this.btUseScanner);
            this.view.AddControl(Const_StockInAutoScan.Head_Control_ButtonOpenScanner, this.btOpenScanner);
            this.view.AddControl(Const_StockInAutoScan.Head_Control_ButtonDirectionConfig, this.btDirectionConfig);
            this.view.AddControl(Const_StockInAutoScan.Head_Control_LayOutDirectionConfig, this.lc_DirectionConfig);
            this.view.AddControl(Const_StockInAutoScan.Head_Control_ForceToManual, this.cb_ForceToManual);

            if (this.context.CompanyCode == "2")
            {
                lo_Specification.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                lo_Grade.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                lo_CoreDiameterOrReam.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                lo_DiameterOrSlides.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                lo_Sku.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                lo_TrademarkStyle.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                lo_Certification.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                lo_WeightMode.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                lo_Count.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                lo_SpecProdName.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                lo_TransportType.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                lo_PackType.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                lo_Color.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                lo_SpCustomer.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                lo_PlanLength.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                lo_PlanDiameter.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                lo_Direction.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                lo_PoNumber.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                lo_Layers.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                lo_Length.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                lo_SpecCustName.Text = "客户";
                layoutControlItem3.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            }
        }

        private void RegisterEvent()
        {
            this.Disposed += new EventHandler(this.OnDispose);
            this.Load += new System.EventHandler(this.OnLoad);
            this.FormClosing += new FormClosingEventHandler(this.OnClose);
            this.FBusinessType.TextChanged += new System.EventHandler(this.Business_TextChanged);
            this.FFactory.TextChanged += new System.EventHandler(this.Factory_TextChanged);
            this.FWHRemark.TextChanged += new System.EventHandler(this.WHRemark_TextChanged);
            this.FDateS.TextChanged += new System.EventHandler(this.DateS_TextChanged);
            this.FShift.TextChanged += new System.EventHandler(this.Shift_TextChanged);
            this.FOperator.TextChanged += new System.EventHandler(this.Operator_TextChanged);
            this.FShiftTime.TextChanged += new System.EventHandler(this.ShiftTime_TextChanged);

            this.FBarcode.ButtonClick += 
                new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.Barcode_ButtonClick);
            this.FBarcode.EditValueChanged += 
                new System.EventHandler(this.Barcode_EditValueChanged);
            this.FBarcode.KeyPress += 
                new System.Windows.Forms.KeyPressEventHandler(this.Barcode_KeyPress);
            this.spScan.DataReceived += 
                new System.IO.Ports.SerialDataReceivedEventHandler(this.spScan_DataReceived);
        }
        private void OnDispose(object sender, EventArgs e)
        {
            controler.Invoke(Const_Event.OnDispose);
            this.Load -= new System.EventHandler(this.OnLoad);
            this.FormClosing -= new FormClosingEventHandler(this.OnClose);
            this.FBusinessType.TextChanged -= new System.EventHandler(this.Business_TextChanged);
            this.FFactory.TextChanged -= new System.EventHandler(this.Factory_TextChanged);
            this.FWHRemark.TextChanged -= new System.EventHandler(this.WHRemark_TextChanged);
            this.FDateS.TextChanged -= new System.EventHandler(this.DateS_TextChanged);
            this.FShift.TextChanged -= new System.EventHandler(this.Shift_TextChanged);
            this.FOperator.TextChanged -= new System.EventHandler(this.Operator_TextChanged);
            this.FShiftTime.TextChanged -= new System.EventHandler(this.ShiftTime_TextChanged);

            this.FBarcode.ButtonClick -=
                new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.Barcode_ButtonClick);
            this.FBarcode.EditValueChanged -=
                new System.EventHandler(this.Barcode_EditValueChanged);
            this.FBarcode.KeyPress -=
                new System.Windows.Forms.KeyPressEventHandler(this.Barcode_KeyPress);
            this.spScan.DataReceived -=
                new System.IO.Ports.SerialDataReceivedEventHandler(this.spScan_DataReceived);
        }
        private void OnClose(object sender, FormClosingEventArgs e)
        {
            controler.Invoke(Const_Event.OnClose);
        }
        private void spScan_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            DataReceivedEventArgs args = new DataReceivedEventArgs();
            args.Sender = Const_StockInAutoScan.Base_Port;
            controler.Invoke(Const_Event.DataReceived, args);
        }

        private void OnLoad(object sender, EventArgs e)
        {
            controler.Invoke(Const_Event.OnLoad);
        }
        private void Barcode_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            ButtonClickEventArgs args = new ButtonClickEventArgs();
            args.Sender = Const_StockInAutoScan.Head_Field_FBarcode;
            controler.Invoke(Const_Event.ButtonClick, args);
        }
        private void Barcode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                ButtonClickEventArgs args = new ButtonClickEventArgs();
                args.Sender = Const_StockInAutoScan.Head_Field_FBarcode;
                controler.Invoke(Const_Event.ButtonClick, args);
            }
        }
        private void Barcode_EditValueChanged(object sender, EventArgs e)
        {
            DataChangedEventArgs args = new DataChangedEventArgs();
            args.Sender = Const_StockInAutoScan.Head_Field_FBarcode;
            args.Row = -1;
            controler.Invoke(Const_Event.DataChanged, args);
        }
        private void DateS_TextChanged(object sender, EventArgs e)
        {
            DataChangedEventArgs args = new DataChangedEventArgs();
            args.Sender = Const_StockInAutoScan.Head_Field_FDateS;
            args.Row = -1;
            controler.Invoke(Const_Event.DataChanged, args);
        }
        private void Shift_TextChanged(object sender, EventArgs e)
        {
            DataChangedEventArgs args = new DataChangedEventArgs();
            args.Sender = Const_StockInAutoScan.Head_Field_FShift;
            args.Row = -1;
            controler.Invoke(Const_Event.DataChanged, args);
        }

        private void Operator_TextChanged(object sender, EventArgs e)
        {
            DataChangedEventArgs args = new DataChangedEventArgs();
            args.Sender = Const_StockInAutoScan.Head_Field_FOperator;
            args.Row = -1;
            controler.Invoke(Const_Event.DataChanged, args);
        }
        private void ShiftTime_TextChanged(object sender, EventArgs e)
        {
            DataChangedEventArgs args = new DataChangedEventArgs();
            args.Sender = Const_StockInAutoScan.Head_Field_FShiftTime;
            args.Row = -1;
            controler.Invoke(Const_Event.DataChanged, args);
        }

        private void Business_TextChanged(object sender, EventArgs e)
        {
            DataChangedEventArgs args = new DataChangedEventArgs();
            args.Sender = Const_StockInAutoScan.Head_Field_FBusinessType;
            args.Row = -1;
            controler.Invoke(Const_Event.DataChanged, args);
        }
        private void Factory_TextChanged(object sender, EventArgs e)
        {
            DataChangedEventArgs args = new DataChangedEventArgs();
            args.Sender = Const_StockInAutoScan.Head_Field_FFactory;
            args.Row = -1;
            controler.Invoke(Const_Event.DataChanged, args);
        }
        private void WHRemark_TextChanged(object sender, EventArgs e)
        {
            DataChangedEventArgs args = new DataChangedEventArgs();
            args.Sender = Const_StockInAutoScan.Head_Field_FWHRemark;
            args.Row = -1;
            controler.Invoke(Const_Event.DataChanged, args);
        }

        private void btMotorStart_Click(object sender, EventArgs e)
        {
            ButtonClickEventArgs args = new ButtonClickEventArgs();
            args.Sender = Const_StockInAutoScan.Head_Control_ButtonMotorStart;
            controler.Invoke(Const_Event.ButtonClick, args);
        }

        private void btUseScanner_Click(object sender, EventArgs e)
        {
            ButtonClickEventArgs args = new ButtonClickEventArgs();
            args.Sender = Const_StockInAutoScan.Head_Control_ButtonUseScanner;
            controler.Invoke(Const_Event.ButtonClick, args);
        }

        private void btOpenScanner_Click(object sender, EventArgs e)
        {
            ButtonClickEventArgs args = new ButtonClickEventArgs();
            args.Sender = Const_StockInAutoScan.Head_Control_ButtonOpenScanner;
            controler.Invoke(Const_Event.ButtonClick, args);
        }

        private void btDirectionConfig_Click(object sender, EventArgs e)
        {
            ButtonClickEventArgs args = new ButtonClickEventArgs();
            args.Sender = Const_StockInAutoScan.Head_Control_ButtonDirectionConfig;
            controler.Invoke(Const_Event.ButtonClick, args);
        }
    }
}