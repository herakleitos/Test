namespace Chaint.Instock.Business
{
    partial class StockInAutoScan
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.spScan = new System.IO.Ports.SerialPort();
            this.gc_StockInfo = new DevExpress.XtraEditors.GroupControl();
            this.lc_StockInfo = new DevExpress.XtraLayout.LayoutControl();
            this.FMessage = new DevExpress.XtraEditors.ListBoxControl();
            this.FDateS = new DevExpress.XtraEditors.DateEdit();
            this.FOperator = new DevExpress.XtraEditors.SearchLookUpEdit();
            this.searchLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.FBusinessType = new DevExpress.XtraEditors.SearchLookUpEdit();
            this.gridView5 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.FShiftTime = new DevExpress.XtraEditors.ComboBoxEdit();
            this.FWHRemark = new DevExpress.XtraEditors.ComboBoxEdit();
            this.FShift = new DevExpress.XtraEditors.ComboBoxEdit();
            this.FWareHouse = new DevExpress.XtraEditors.ComboBoxEdit();
            this.FFactory = new DevExpress.XtraEditors.ComboBoxEdit();
            this.layoutControlGroup2 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lo_ShiftTime = new DevExpress.XtraLayout.LayoutControlItem();
            this.lo_DateS = new DevExpress.XtraLayout.LayoutControlItem();
            this.lo_BusinessType = new DevExpress.XtraLayout.LayoutControlItem();
            this.lo_WHRemark = new DevExpress.XtraLayout.LayoutControlItem();
            this.lo_Shift = new DevExpress.XtraLayout.LayoutControlItem();
            this.lo_Factory = new DevExpress.XtraLayout.LayoutControlItem();
            this.lo_WareHouse = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.lo_Operator = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.cb_ForceToManual = new DevExpress.XtraEditors.CheckEdit();
            this.btDirectionConfig = new DevExpress.XtraEditors.SimpleButton();
            this.btOpenScanner = new DevExpress.XtraEditors.SimpleButton();
            this.btMotorStart = new DevExpress.XtraEditors.SimpleButton();
            this.btUseScanner = new DevExpress.XtraEditors.SimpleButton();
            this.gc_ProducytInfo = new DevExpress.XtraEditors.GroupControl();
            this.lc_ProductInfo = new DevExpress.XtraLayout.LayoutControl();
            this.FPlanDiameter = new DevExpress.XtraEditors.TextEdit();
            this.FPlanLength = new DevExpress.XtraEditors.TextEdit();
            this.FSpCustomer = new DevExpress.XtraEditors.TextEdit();
            this.FColor = new DevExpress.XtraEditors.TextEdit();
            this.FDiameterOrSlides = new DevExpress.XtraEditors.TextEdit();
            this.FRemark = new DevExpress.XtraEditors.TextEdit();
            this.FDirection = new DevExpress.XtraEditors.TextEdit();
            this.FTransportType = new DevExpress.XtraEditors.TextEdit();
            this.FPackType = new DevExpress.XtraEditors.TextEdit();
            this.FTrademarkStyle = new DevExpress.XtraEditors.TextEdit();
            this.FPoNumber = new DevExpress.XtraEditors.TextEdit();
            this.FWeight = new DevExpress.XtraEditors.TextEdit();
            this.FSku = new DevExpress.XtraEditors.TextEdit();
            this.FCount = new DevExpress.XtraEditors.TextEdit();
            this.FWeightMode = new DevExpress.XtraEditors.TextEdit();
            this.FSpecification = new DevExpress.XtraEditors.TextEdit();
            this.FCertification = new DevExpress.XtraEditors.TextEdit();
            this.FLayers = new DevExpress.XtraEditors.TextEdit();
            this.FGrade = new DevExpress.XtraEditors.TextEdit();
            this.FSpecCustName = new DevExpress.XtraEditors.TextEdit();
            this.FSpecProdName = new DevExpress.XtraEditors.TextEdit();
            this.FLength = new DevExpress.XtraEditors.TextEdit();
            this.FCoreDiameterOrReam = new DevExpress.XtraEditors.TextEdit();
            this.FMaterial = new DevExpress.XtraEditors.TextEdit();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lo_Material = new DevExpress.XtraLayout.LayoutControlItem();
            this.lo_CoreDiameterOrReam = new DevExpress.XtraLayout.LayoutControlItem();
            this.lo_SpecProdName = new DevExpress.XtraLayout.LayoutControlItem();
            this.lo_Specification = new DevExpress.XtraLayout.LayoutControlItem();
            this.lo_PackType = new DevExpress.XtraLayout.LayoutControlItem();
            this.lo_TransportType = new DevExpress.XtraLayout.LayoutControlItem();
            this.lo_DiameterOrSlides = new DevExpress.XtraLayout.LayoutControlItem();
            this.lo_Length = new DevExpress.XtraLayout.LayoutControlItem();
            this.lo_SpecCustName = new DevExpress.XtraLayout.LayoutControlItem();
            this.lo_Grade = new DevExpress.XtraLayout.LayoutControlItem();
            this.lo_WeightMode = new DevExpress.XtraLayout.LayoutControlItem();
            this.lo_Weight = new DevExpress.XtraLayout.LayoutControlItem();
            this.lo_Count = new DevExpress.XtraLayout.LayoutControlItem();
            this.lo_Layers = new DevExpress.XtraLayout.LayoutControlItem();
            this.lo_Sku = new DevExpress.XtraLayout.LayoutControlItem();
            this.lo_Certification = new DevExpress.XtraLayout.LayoutControlItem();
            this.lo_Remark = new DevExpress.XtraLayout.LayoutControlItem();
            this.lo_TrademarkStyle = new DevExpress.XtraLayout.LayoutControlItem();
            this.lo_PoNumber = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.lo_PlanLength = new DevExpress.XtraLayout.LayoutControlItem();
            this.lo_PlanDiameter = new DevExpress.XtraLayout.LayoutControlItem();
            this.lo_Direction = new DevExpress.XtraLayout.LayoutControlItem();
            this.lo_SpCustomer = new DevExpress.XtraLayout.LayoutControlItem();
            this.lo_Color = new DevExpress.XtraLayout.LayoutControlItem();
            this.gc_ScanInfo = new DevExpress.XtraEditors.GroupControl();
            this.lc_ScanInfo = new DevExpress.XtraLayout.LayoutControl();
            this.FPlcInfo = new DevExpress.XtraEditors.LabelControl();
            this.FMonitor = new DevExpress.XtraEditors.ListBoxControl();
            this.FScannerStatus = new DevExpress.XtraEditors.LabelControl();
            this.FPlcConStatus = new DevExpress.XtraEditors.LabelControl();
            this.FLedConStatus = new DevExpress.XtraEditors.LabelControl();
            this.FScannerConStatus = new DevExpress.XtraEditors.LabelControl();
            this.FBarcode = new DevExpress.XtraEditors.ButtonEdit();
            this.FResult = new DevExpress.XtraEditors.TextEdit();
            this.layoutControlGroup4 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lo_Barcode = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem7 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem8 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem9 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem10 = new DevExpress.XtraLayout.LayoutControlItem();
            this.lo_Result = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem13 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlGroup3 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem3 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem11 = new DevExpress.XtraLayout.LayoutControlItem();
            this.lc_DirectionConfig = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem12 = new DevExpress.XtraLayout.LayoutControlItem();
            this.lc_ForceToManual = new DevExpress.XtraLayout.LayoutControlItem();
            this.xtraScrollableControl1 = new DevExpress.XtraEditors.XtraScrollableControl();
            ((System.ComponentModel.ISupportInitialize)(this.gc_StockInfo)).BeginInit();
            this.gc_StockInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lc_StockInfo)).BeginInit();
            this.lc_StockInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FMessage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FDateS.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FDateS.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FOperator.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLookUpEdit1View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FBusinessType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FShiftTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FWHRemark.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FShift.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FWareHouse.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FFactory.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_ShiftTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_DateS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_BusinessType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_WHRemark)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_Shift)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_Factory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_WareHouse)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_Operator)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cb_ForceToManual.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gc_ProducytInfo)).BeginInit();
            this.gc_ProducytInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lc_ProductInfo)).BeginInit();
            this.lc_ProductInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FPlanDiameter.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FPlanLength.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FSpCustomer.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FColor.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FDiameterOrSlides.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FRemark.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FDirection.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FTransportType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FPackType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FTrademarkStyle.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FPoNumber.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FWeight.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FSku.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FCount.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FWeightMode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FSpecification.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FCertification.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FLayers.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FGrade.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FSpecCustName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FSpecProdName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FLength.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FCoreDiameterOrReam.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FMaterial.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_Material)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_CoreDiameterOrReam)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_SpecProdName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_Specification)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_PackType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_TransportType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_DiameterOrSlides)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_Length)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_SpecCustName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_Grade)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_WeightMode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_Weight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_Count)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_Layers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_Sku)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_Certification)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_Remark)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_TrademarkStyle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_PoNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_PlanLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_PlanDiameter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_Direction)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_SpCustomer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_Color)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gc_ScanInfo)).BeginInit();
            this.gc_ScanInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lc_ScanInfo)).BeginInit();
            this.lc_ScanInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FMonitor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FBarcode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FResult.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_Barcode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_Result)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem13)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem11)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lc_DirectionConfig)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem12)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lc_ForceToManual)).BeginInit();
            this.xtraScrollableControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gc_StockInfo
            // 
            this.gc_StockInfo.Controls.Add(this.lc_StockInfo);
            this.gc_StockInfo.Location = new System.Drawing.Point(18, 778);
            this.gc_StockInfo.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gc_StockInfo.Name = "gc_StockInfo";
            this.gc_StockInfo.Size = new System.Drawing.Size(1412, 232);
            this.gc_StockInfo.TabIndex = 8;
            this.gc_StockInfo.Text = "仓库信息";
            // 
            // lc_StockInfo
            // 
            this.lc_StockInfo.Controls.Add(this.FMessage);
            this.lc_StockInfo.Controls.Add(this.FDateS);
            this.lc_StockInfo.Controls.Add(this.FOperator);
            this.lc_StockInfo.Controls.Add(this.FBusinessType);
            this.lc_StockInfo.Controls.Add(this.FShiftTime);
            this.lc_StockInfo.Controls.Add(this.FWHRemark);
            this.lc_StockInfo.Controls.Add(this.FShift);
            this.lc_StockInfo.Controls.Add(this.FWareHouse);
            this.lc_StockInfo.Controls.Add(this.FFactory);
            this.lc_StockInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lc_StockInfo.Location = new System.Drawing.Point(3, 33);
            this.lc_StockInfo.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lc_StockInfo.Name = "lc_StockInfo";
            this.lc_StockInfo.Root = this.layoutControlGroup2;
            this.lc_StockInfo.Size = new System.Drawing.Size(1406, 196);
            this.lc_StockInfo.TabIndex = 0;
            this.lc_StockInfo.Text = "layoutControl2";
            // 
            // FMessage
            // 
            this.FMessage.Location = new System.Drawing.Point(1078, 18);
            this.FMessage.Name = "FMessage";
            this.FMessage.Size = new System.Drawing.Size(310, 150);
            this.FMessage.StyleController = this.lc_StockInfo;
            this.FMessage.TabIndex = 17;
            // 
            // FDateS
            // 
            this.FDateS.EditValue = new System.DateTime(2014, 9, 4, 22, 11, 53, 755);
            this.FDateS.Location = new System.Drawing.Point(793, 18);
            this.FDateS.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.FDateS.Name = "FDateS";
            this.FDateS.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold);
            this.FDateS.Properties.Appearance.Options.UseFont = true;
            this.FDateS.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.FDateS.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.FDateS.Properties.DisplayFormat.FormatString = "g";
            this.FDateS.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.FDateS.Properties.EditFormat.FormatString = "g";
            this.FDateS.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.FDateS.Properties.Mask.EditMask = "g";
            this.FDateS.Size = new System.Drawing.Size(279, 46);
            this.FDateS.StyleController = this.lc_StockInfo;
            this.FDateS.TabIndex = 15;
            // 
            // FOperator
            // 
            this.FOperator.Location = new System.Drawing.Point(93, 18);
            this.FOperator.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.FOperator.Name = "FOperator";
            this.FOperator.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold);
            this.FOperator.Properties.Appearance.Options.UseFont = true;
            this.FOperator.Properties.AppearanceDropDown.Font = new System.Drawing.Font("Tahoma", 15.75F);
            this.FOperator.Properties.AppearanceDropDown.Options.UseFont = true;
            this.FOperator.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.FOperator.Properties.NullText = "";
            this.FOperator.Properties.PopupSizeable = false;
            this.FOperator.Properties.View = this.searchLookUpEdit1View;
            this.FOperator.Size = new System.Drawing.Size(177, 46);
            this.FOperator.StyleController = this.lc_StockInfo;
            this.FOperator.TabIndex = 11;
            // 
            // searchLookUpEdit1View
            // 
            this.searchLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.searchLookUpEdit1View.Name = "searchLookUpEdit1View";
            this.searchLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.searchLookUpEdit1View.OptionsView.ShowGroupPanel = false;
            // 
            // FBusinessType
            // 
            this.FBusinessType.Enabled = false;
            this.FBusinessType.Location = new System.Drawing.Point(93, 70);
            this.FBusinessType.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.FBusinessType.Name = "FBusinessType";
            this.FBusinessType.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold);
            this.FBusinessType.Properties.Appearance.Options.UseFont = true;
            this.FBusinessType.Properties.AppearanceDropDown.Font = new System.Drawing.Font("Tahoma", 15.75F);
            this.FBusinessType.Properties.AppearanceDropDown.Options.UseFont = true;
            this.FBusinessType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.FBusinessType.Properties.NullText = "";
            this.FBusinessType.Properties.PopupSizeable = false;
            this.FBusinessType.Properties.View = this.gridView5;
            this.FBusinessType.Size = new System.Drawing.Size(177, 46);
            this.FBusinessType.StyleController = this.lc_StockInfo;
            this.FBusinessType.TabIndex = 12;
            // 
            // gridView5
            // 
            this.gridView5.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridView5.Name = "gridView5";
            this.gridView5.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView5.OptionsView.ShowGroupPanel = false;
            // 
            // FShiftTime
            // 
            this.FShiftTime.Location = new System.Drawing.Point(597, 18);
            this.FShiftTime.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.FShiftTime.Name = "FShiftTime";
            this.FShiftTime.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold);
            this.FShiftTime.Properties.Appearance.Options.UseFont = true;
            this.FShiftTime.Properties.AppearanceDropDown.Font = new System.Drawing.Font("Tahoma", 15.75F);
            this.FShiftTime.Properties.AppearanceDropDown.Options.UseFont = true;
            this.FShiftTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.FShiftTime.Properties.Items.AddRange(new object[] {
            "早班",
            "中班",
            "夜班"});
            this.FShiftTime.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.FShiftTime.Size = new System.Drawing.Size(115, 46);
            this.FShiftTime.StyleController = this.lc_StockInfo;
            this.FShiftTime.TabIndex = 10;
            // 
            // FWHRemark
            // 
            this.FWHRemark.Location = new System.Drawing.Point(597, 70);
            this.FWHRemark.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.FWHRemark.Name = "FWHRemark";
            this.FWHRemark.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold);
            this.FWHRemark.Properties.Appearance.Options.UseFont = true;
            this.FWHRemark.Properties.AppearanceDropDown.Font = new System.Drawing.Font("Tahoma", 15.75F);
            this.FWHRemark.Properties.AppearanceDropDown.Options.UseFont = true;
            this.FWHRemark.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.FWHRemark.Properties.Items.AddRange(new object[] {
            "",
            "L",
            "J",
            "JD"});
            this.FWHRemark.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.FWHRemark.Size = new System.Drawing.Size(475, 46);
            this.FWHRemark.StyleController = this.lc_StockInfo;
            this.FWHRemark.TabIndex = 8;
            // 
            // FShift
            // 
            this.FShift.Location = new System.Drawing.Point(351, 18);
            this.FShift.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.FShift.Name = "FShift";
            this.FShift.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FShift.Properties.Appearance.Options.UseFont = true;
            this.FShift.Properties.AppearanceDropDown.Font = new System.Drawing.Font("Tahoma", 15.75F);
            this.FShift.Properties.AppearanceDropDown.Options.UseFont = true;
            this.FShift.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.FShift.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.FShift.Size = new System.Drawing.Size(165, 46);
            this.FShift.StyleController = this.lc_StockInfo;
            this.FShift.TabIndex = 7;
            // 
            // FWareHouse
            // 
            this.FWareHouse.Location = new System.Drawing.Point(93, 122);
            this.FWareHouse.Name = "FWareHouse";
            this.FWareHouse.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 15.75F);
            this.FWareHouse.Properties.Appearance.Options.UseFont = true;
            this.FWareHouse.Properties.AppearanceDropDown.Font = new System.Drawing.Font("Tahoma", 15.75F);
            this.FWareHouse.Properties.AppearanceDropDown.Options.UseFont = true;
            this.FWareHouse.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.FWareHouse.Properties.PopupSizeable = true;
            this.FWareHouse.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.FWareHouse.Size = new System.Drawing.Size(979, 46);
            this.FWareHouse.StyleController = this.lc_StockInfo;
            this.FWareHouse.TabIndex = 16;
            // 
            // FFactory
            // 
            this.FFactory.Location = new System.Drawing.Point(351, 70);
            this.FFactory.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.FFactory.Name = "FFactory";
            this.FFactory.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold);
            this.FFactory.Properties.Appearance.Options.UseFont = true;
            this.FFactory.Properties.AppearanceDropDown.Font = new System.Drawing.Font("Tahoma", 15.75F);
            this.FFactory.Properties.AppearanceDropDown.Options.UseFont = true;
            this.FFactory.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.FFactory.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.FFactory.Size = new System.Drawing.Size(165, 46);
            this.FFactory.StyleController = this.lc_StockInfo;
            this.FFactory.TabIndex = 12;
            // 
            // layoutControlGroup2
            // 
            this.layoutControlGroup2.AppearanceGroup.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.layoutControlGroup2.AppearanceGroup.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.layoutControlGroup2.AppearanceGroup.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.layoutControlGroup2.AppearanceGroup.Options.UseBackColor = true;
            this.layoutControlGroup2.AppearanceGroup.Options.UseBorderColor = true;
            this.layoutControlGroup2.AppearanceItemCaption.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.layoutControlGroup2.AppearanceItemCaption.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.layoutControlGroup2.AppearanceItemCaption.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.layoutControlGroup2.AppearanceItemCaption.Options.UseBackColor = true;
            this.layoutControlGroup2.AppearanceItemCaption.Options.UseBorderColor = true;
            this.layoutControlGroup2.AppearanceTabPage.PageClient.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.layoutControlGroup2.AppearanceTabPage.PageClient.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.layoutControlGroup2.AppearanceTabPage.PageClient.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.layoutControlGroup2.AppearanceTabPage.PageClient.Options.UseBackColor = true;
            this.layoutControlGroup2.AppearanceTabPage.PageClient.Options.UseBorderColor = true;
            this.layoutControlGroup2.CustomizationFormText = "layoutControlGroup2";
            this.layoutControlGroup2.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup2.GroupBordersVisible = false;
            this.layoutControlGroup2.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lo_ShiftTime,
            this.lo_DateS,
            this.lo_BusinessType,
            this.lo_WHRemark,
            this.lo_Shift,
            this.lo_Factory,
            this.lo_WareHouse,
            this.layoutControlItem4,
            this.lo_Operator,
            this.emptySpaceItem2});
            this.layoutControlGroup2.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup2.Name = "layoutControlGroup2";
            this.layoutControlGroup2.Size = new System.Drawing.Size(1406, 196);
            this.layoutControlGroup2.TextVisible = false;
            // 
            // lo_ShiftTime
            // 
            this.lo_ShiftTime.Control = this.FShiftTime;
            this.lo_ShiftTime.CustomizationFormText = "班次";
            this.lo_ShiftTime.Location = new System.Drawing.Point(504, 0);
            this.lo_ShiftTime.MinSize = new System.Drawing.Size(50, 25);
            this.lo_ShiftTime.Name = "lo_ShiftTime";
            this.lo_ShiftTime.Size = new System.Drawing.Size(196, 52);
            this.lo_ShiftTime.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lo_ShiftTime.Text = "班次";
            this.lo_ShiftTime.TextSize = new System.Drawing.Size(72, 22);
            // 
            // lo_DateS
            // 
            this.lo_DateS.Control = this.FDateS;
            this.lo_DateS.CustomizationFormText = "交班时间";
            this.lo_DateS.Location = new System.Drawing.Point(700, 0);
            this.lo_DateS.MinSize = new System.Drawing.Size(106, 36);
            this.lo_DateS.Name = "lo_DateS";
            this.lo_DateS.Size = new System.Drawing.Size(360, 52);
            this.lo_DateS.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lo_DateS.Text = "交班时间";
            this.lo_DateS.TextSize = new System.Drawing.Size(72, 22);
            // 
            // lo_BusinessType
            // 
            this.lo_BusinessType.Control = this.FBusinessType;
            this.lo_BusinessType.CustomizationFormText = "业务类型";
            this.lo_BusinessType.Location = new System.Drawing.Point(0, 52);
            this.lo_BusinessType.Name = "lo_BusinessType";
            this.lo_BusinessType.Size = new System.Drawing.Size(258, 52);
            this.lo_BusinessType.Text = "业务类型";
            this.lo_BusinessType.TextSize = new System.Drawing.Size(72, 22);
            // 
            // lo_WHRemark
            // 
            this.lo_WHRemark.Control = this.FWHRemark;
            this.lo_WHRemark.CustomizationFormText = "入库仓库";
            this.lo_WHRemark.Location = new System.Drawing.Point(504, 52);
            this.lo_WHRemark.MinSize = new System.Drawing.Size(50, 25);
            this.lo_WHRemark.Name = "lo_WHRemark";
            this.lo_WHRemark.Size = new System.Drawing.Size(556, 52);
            this.lo_WHRemark.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lo_WHRemark.Text = "入库仓库";
            this.lo_WHRemark.TextSize = new System.Drawing.Size(72, 22);
            // 
            // lo_Shift
            // 
            this.lo_Shift.Control = this.FShift;
            this.lo_Shift.CustomizationFormText = "入库班组";
            this.lo_Shift.Location = new System.Drawing.Point(258, 0);
            this.lo_Shift.MinSize = new System.Drawing.Size(50, 25);
            this.lo_Shift.Name = "lo_Shift";
            this.lo_Shift.Size = new System.Drawing.Size(246, 52);
            this.lo_Shift.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lo_Shift.Text = "入库班组";
            this.lo_Shift.TextSize = new System.Drawing.Size(72, 22);
            // 
            // lo_Factory
            // 
            this.lo_Factory.Control = this.FFactory;
            this.lo_Factory.CustomizationFormText = "机台号";
            this.lo_Factory.Location = new System.Drawing.Point(258, 52);
            this.lo_Factory.Name = "lo_Factory";
            this.lo_Factory.Size = new System.Drawing.Size(246, 52);
            this.lo_Factory.Text = "包装机台";
            this.lo_Factory.TextSize = new System.Drawing.Size(72, 22);
            // 
            // lo_WareHouse
            // 
            this.lo_WareHouse.Control = this.FWareHouse;
            this.lo_WareHouse.Location = new System.Drawing.Point(0, 104);
            this.lo_WareHouse.Name = "lo_WareHouse";
            this.lo_WareHouse.Size = new System.Drawing.Size(1060, 52);
            this.lo_WareHouse.Text = "入库仓库";
            this.lo_WareHouse.TextSize = new System.Drawing.Size(72, 22);
            this.lo_WareHouse.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.FMessage;
            this.layoutControlItem4.Location = new System.Drawing.Point(1060, 0);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(316, 156);
            this.layoutControlItem4.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem4.TextVisible = false;
            // 
            // lo_Operator
            // 
            this.lo_Operator.Control = this.FOperator;
            this.lo_Operator.CustomizationFormText = "入库人员";
            this.lo_Operator.Location = new System.Drawing.Point(0, 0);
            this.lo_Operator.Name = "lo_Operator";
            this.lo_Operator.Size = new System.Drawing.Size(258, 52);
            this.lo_Operator.Text = "入库人员";
            this.lo_Operator.TextSize = new System.Drawing.Size(72, 22);
            // 
            // emptySpaceItem2
            // 
            this.emptySpaceItem2.AllowHotTrack = false;
            this.emptySpaceItem2.Location = new System.Drawing.Point(0, 156);
            this.emptySpaceItem2.Name = "emptySpaceItem2";
            this.emptySpaceItem2.Size = new System.Drawing.Size(1376, 10);
            this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.cb_ForceToManual);
            this.layoutControl1.Controls.Add(this.btDirectionConfig);
            this.layoutControl1.Controls.Add(this.btOpenScanner);
            this.layoutControl1.Controls.Add(this.btMotorStart);
            this.layoutControl1.Controls.Add(this.btUseScanner);
            this.layoutControl1.Controls.Add(this.gc_StockInfo);
            this.layoutControl1.Controls.Add(this.gc_ProducytInfo);
            this.layoutControl1.Controls.Add(this.gc_ScanInfo);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(422, 294, 375, 525);
            this.layoutControl1.Root = this.layoutControlGroup3;
            this.layoutControl1.Size = new System.Drawing.Size(1448, 1028);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // cb_ForceToManual
            // 
            this.cb_ForceToManual.Location = new System.Drawing.Point(1202, 18);
            this.cb_ForceToManual.Name = "cb_ForceToManual";
            this.cb_ForceToManual.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 11F);
            this.cb_ForceToManual.Properties.Appearance.Options.UseFont = true;
            this.cb_ForceToManual.Properties.Caption = "强制去人工仓库";
            this.cb_ForceToManual.Size = new System.Drawing.Size(204, 31);
            this.cb_ForceToManual.StyleController = this.layoutControl1;
            this.cb_ForceToManual.TabIndex = 21;
            // 
            // btDirectionConfig
            // 
            this.btDirectionConfig.Appearance.Font = new System.Drawing.Font("Tahoma", 13F);
            this.btDirectionConfig.Appearance.Options.UseFont = true;
            this.btDirectionConfig.Location = new System.Drawing.Point(909, 18);
            this.btDirectionConfig.Name = "btDirectionConfig";
            this.btDirectionConfig.Size = new System.Drawing.Size(287, 40);
            this.btDirectionConfig.StyleController = this.layoutControl1;
            this.btDirectionConfig.TabIndex = 20;
            this.btDirectionConfig.Text = "立库去向设置";
            this.btDirectionConfig.Click += new System.EventHandler(this.btDirectionConfig_Click);
            // 
            // btOpenScanner
            // 
            this.btOpenScanner.Appearance.Font = new System.Drawing.Font("Tahoma", 13F);
            this.btOpenScanner.Appearance.Options.UseFont = true;
            this.btOpenScanner.Location = new System.Drawing.Point(600, 18);
            this.btOpenScanner.Name = "btOpenScanner";
            this.btOpenScanner.Size = new System.Drawing.Size(303, 40);
            this.btOpenScanner.StyleController = this.layoutControl1;
            this.btOpenScanner.TabIndex = 19;
            this.btOpenScanner.Text = "打开扫描仪";
            this.btOpenScanner.Click += new System.EventHandler(this.btOpenScanner_Click);
            // 
            // btMotorStart
            // 
            this.btMotorStart.Appearance.Font = new System.Drawing.Font("Tahoma", 13F);
            this.btMotorStart.Appearance.Options.UseFont = true;
            this.btMotorStart.Location = new System.Drawing.Point(18, 18);
            this.btMotorStart.Name = "btMotorStart";
            this.btMotorStart.Size = new System.Drawing.Size(283, 40);
            this.btMotorStart.StyleController = this.layoutControl1;
            this.btMotorStart.TabIndex = 17;
            this.btMotorStart.Text = "扫描仪下降(人工)";
            this.btMotorStart.Click += new System.EventHandler(this.btMotorStart_Click);
            // 
            // btUseScanner
            // 
            this.btUseScanner.Appearance.Font = new System.Drawing.Font("Tahoma", 13F);
            this.btUseScanner.Appearance.Options.UseFont = true;
            this.btUseScanner.Location = new System.Drawing.Point(307, 18);
            this.btUseScanner.Name = "btUseScanner";
            this.btUseScanner.Size = new System.Drawing.Size(287, 40);
            this.btUseScanner.StyleController = this.layoutControl1;
            this.btUseScanner.TabIndex = 18;
            this.btUseScanner.Text = "点击投入扫描仪";
            this.btUseScanner.Click += new System.EventHandler(this.btUseScanner_Click);
            // 
            // gc_ProducytInfo
            // 
            this.gc_ProducytInfo.Controls.Add(this.lc_ProductInfo);
            this.gc_ProducytInfo.Location = new System.Drawing.Point(18, 442);
            this.gc_ProducytInfo.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gc_ProducytInfo.Name = "gc_ProducytInfo";
            this.gc_ProducytInfo.Size = new System.Drawing.Size(1412, 330);
            this.gc_ProducytInfo.TabIndex = 6;
            this.gc_ProducytInfo.Text = "产品信息";
            // 
            // lc_ProductInfo
            // 
            this.lc_ProductInfo.Controls.Add(this.FPlanDiameter);
            this.lc_ProductInfo.Controls.Add(this.FPlanLength);
            this.lc_ProductInfo.Controls.Add(this.FSpCustomer);
            this.lc_ProductInfo.Controls.Add(this.FColor);
            this.lc_ProductInfo.Controls.Add(this.FDiameterOrSlides);
            this.lc_ProductInfo.Controls.Add(this.FRemark);
            this.lc_ProductInfo.Controls.Add(this.FDirection);
            this.lc_ProductInfo.Controls.Add(this.FTransportType);
            this.lc_ProductInfo.Controls.Add(this.FPackType);
            this.lc_ProductInfo.Controls.Add(this.FTrademarkStyle);
            this.lc_ProductInfo.Controls.Add(this.FPoNumber);
            this.lc_ProductInfo.Controls.Add(this.FWeight);
            this.lc_ProductInfo.Controls.Add(this.FSku);
            this.lc_ProductInfo.Controls.Add(this.FCount);
            this.lc_ProductInfo.Controls.Add(this.FWeightMode);
            this.lc_ProductInfo.Controls.Add(this.FSpecification);
            this.lc_ProductInfo.Controls.Add(this.FCertification);
            this.lc_ProductInfo.Controls.Add(this.FLayers);
            this.lc_ProductInfo.Controls.Add(this.FGrade);
            this.lc_ProductInfo.Controls.Add(this.FSpecCustName);
            this.lc_ProductInfo.Controls.Add(this.FSpecProdName);
            this.lc_ProductInfo.Controls.Add(this.FLength);
            this.lc_ProductInfo.Controls.Add(this.FCoreDiameterOrReam);
            this.lc_ProductInfo.Controls.Add(this.FMaterial);
            this.lc_ProductInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lc_ProductInfo.Location = new System.Drawing.Point(3, 33);
            this.lc_ProductInfo.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lc_ProductInfo.Name = "lc_ProductInfo";
            this.lc_ProductInfo.Root = this.layoutControlGroup1;
            this.lc_ProductInfo.Size = new System.Drawing.Size(1406, 294);
            this.lc_ProductInfo.TabIndex = 0;
            this.lc_ProductInfo.Text = "layoutControl1";
            // 
            // FPlanDiameter
            // 
            this.FPlanDiameter.Location = new System.Drawing.Point(377, 238);
            this.FPlanDiameter.Name = "FPlanDiameter";
            this.FPlanDiameter.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 13F);
            this.FPlanDiameter.Properties.Appearance.Options.UseFont = true;
            this.FPlanDiameter.Size = new System.Drawing.Size(208, 38);
            this.FPlanDiameter.StyleController = this.lc_ProductInfo;
            this.FPlanDiameter.TabIndex = 43;
            // 
            // FPlanLength
            // 
            this.FPlanLength.Location = new System.Drawing.Point(100, 238);
            this.FPlanLength.Name = "FPlanLength";
            this.FPlanLength.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 13F);
            this.FPlanLength.Properties.Appearance.Options.UseFont = true;
            this.FPlanLength.Size = new System.Drawing.Size(189, 38);
            this.FPlanLength.StyleController = this.lc_ProductInfo;
            this.FPlanLength.TabIndex = 42;
            // 
            // FSpCustomer
            // 
            this.FSpCustomer.Location = new System.Drawing.Point(377, 194);
            this.FSpCustomer.Name = "FSpCustomer";
            this.FSpCustomer.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 13F);
            this.FSpCustomer.Properties.Appearance.Options.UseFont = true;
            this.FSpCustomer.Size = new System.Drawing.Size(208, 38);
            this.FSpCustomer.StyleController = this.lc_ProductInfo;
            this.FSpCustomer.TabIndex = 41;
            // 
            // FColor
            // 
            this.FColor.Location = new System.Drawing.Point(100, 194);
            this.FColor.Name = "FColor";
            this.FColor.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 13F);
            this.FColor.Properties.Appearance.Options.UseFont = true;
            this.FColor.Size = new System.Drawing.Size(189, 38);
            this.FColor.StyleController = this.lc_ProductInfo;
            this.FColor.TabIndex = 40;
            // 
            // FDiameterOrSlides
            // 
            this.FDiameterOrSlides.Location = new System.Drawing.Point(377, 62);
            this.FDiameterOrSlides.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.FDiameterOrSlides.Name = "FDiameterOrSlides";
            this.FDiameterOrSlides.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FDiameterOrSlides.Properties.Appearance.Options.UseFont = true;
            this.FDiameterOrSlides.Size = new System.Drawing.Size(208, 38);
            this.FDiameterOrSlides.StyleController = this.lc_ProductInfo;
            this.FDiameterOrSlides.TabIndex = 39;
            // 
            // FRemark
            // 
            this.FRemark.Location = new System.Drawing.Point(673, 194);
            this.FRemark.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.FRemark.Name = "FRemark";
            this.FRemark.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FRemark.Properties.Appearance.Options.UseFont = true;
            this.FRemark.Size = new System.Drawing.Size(715, 38);
            this.FRemark.StyleController = this.lc_ProductInfo;
            this.FRemark.TabIndex = 38;
            // 
            // FDirection
            // 
            this.FDirection.Location = new System.Drawing.Point(673, 238);
            this.FDirection.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.FDirection.Name = "FDirection";
            this.FDirection.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FDirection.Properties.Appearance.Options.UseFont = true;
            this.FDirection.Size = new System.Drawing.Size(323, 38);
            this.FDirection.StyleController = this.lc_ProductInfo;
            this.FDirection.TabIndex = 36;
            // 
            // FTransportType
            // 
            this.FTransportType.Location = new System.Drawing.Point(948, 150);
            this.FTransportType.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.FTransportType.Name = "FTransportType";
            this.FTransportType.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FTransportType.Properties.Appearance.Options.UseFont = true;
            this.FTransportType.Size = new System.Drawing.Size(187, 38);
            this.FTransportType.StyleController = this.lc_ProductInfo;
            this.FTransportType.TabIndex = 35;
            // 
            // FPackType
            // 
            this.FPackType.Location = new System.Drawing.Point(1223, 150);
            this.FPackType.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.FPackType.Name = "FPackType";
            this.FPackType.Properties.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F);
            this.FPackType.Properties.Appearance.Options.UseFont = true;
            this.FPackType.Size = new System.Drawing.Size(165, 36);
            this.FPackType.StyleController = this.lc_ProductInfo;
            this.FPackType.TabIndex = 33;
            // 
            // FTrademarkStyle
            // 
            this.FTrademarkStyle.Location = new System.Drawing.Point(100, 106);
            this.FTrademarkStyle.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.FTrademarkStyle.Name = "FTrademarkStyle";
            this.FTrademarkStyle.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FTrademarkStyle.Properties.Appearance.Options.UseFont = true;
            this.FTrademarkStyle.Size = new System.Drawing.Size(189, 38);
            this.FTrademarkStyle.StyleController = this.lc_ProductInfo;
            this.FTrademarkStyle.TabIndex = 31;
            // 
            // FPoNumber
            // 
            this.FPoNumber.Location = new System.Drawing.Point(100, 150);
            this.FPoNumber.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.FPoNumber.Name = "FPoNumber";
            this.FPoNumber.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FPoNumber.Properties.Appearance.Options.UseFont = true;
            this.FPoNumber.Size = new System.Drawing.Size(189, 38);
            this.FPoNumber.StyleController = this.lc_ProductInfo;
            this.FPoNumber.TabIndex = 37;
            // 
            // FWeight
            // 
            this.FWeight.Location = new System.Drawing.Point(948, 106);
            this.FWeight.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.FWeight.Name = "FWeight";
            this.FWeight.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FWeight.Properties.Appearance.Options.UseFont = true;
            this.FWeight.Size = new System.Drawing.Size(187, 38);
            this.FWeight.StyleController = this.lc_ProductInfo;
            this.FWeight.TabIndex = 30;
            // 
            // FSku
            // 
            this.FSku.Location = new System.Drawing.Point(1223, 62);
            this.FSku.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.FSku.Name = "FSku";
            this.FSku.Properties.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F);
            this.FSku.Properties.Appearance.Options.UseFont = true;
            this.FSku.Size = new System.Drawing.Size(165, 36);
            this.FSku.StyleController = this.lc_ProductInfo;
            this.FSku.TabIndex = 29;
            // 
            // FCount
            // 
            this.FCount.Location = new System.Drawing.Point(1223, 106);
            this.FCount.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.FCount.Name = "FCount";
            this.FCount.Properties.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F);
            this.FCount.Properties.Appearance.Options.UseFont = true;
            this.FCount.Size = new System.Drawing.Size(165, 36);
            this.FCount.StyleController = this.lc_ProductInfo;
            this.FCount.TabIndex = 28;
            // 
            // FWeightMode
            // 
            this.FWeightMode.Location = new System.Drawing.Point(673, 106);
            this.FWeightMode.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.FWeightMode.Name = "FWeightMode";
            this.FWeightMode.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FWeightMode.Properties.Appearance.Options.UseFont = true;
            this.FWeightMode.Size = new System.Drawing.Size(187, 38);
            this.FWeightMode.StyleController = this.lc_ProductInfo;
            this.FWeightMode.TabIndex = 27;
            // 
            // FSpecification
            // 
            this.FSpecification.EditValue = "";
            this.FSpecification.Location = new System.Drawing.Point(948, 18);
            this.FSpecification.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.FSpecification.Name = "FSpecification";
            this.FSpecification.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FSpecification.Properties.Appearance.Options.UseFont = true;
            this.FSpecification.Size = new System.Drawing.Size(187, 38);
            this.FSpecification.StyleController = this.lc_ProductInfo;
            this.FSpecification.TabIndex = 26;
            // 
            // FCertification
            // 
            this.FCertification.Location = new System.Drawing.Point(377, 106);
            this.FCertification.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.FCertification.Name = "FCertification";
            this.FCertification.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FCertification.Properties.Appearance.Options.UseFont = true;
            this.FCertification.Size = new System.Drawing.Size(208, 38);
            this.FCertification.StyleController = this.lc_ProductInfo;
            this.FCertification.TabIndex = 25;
            // 
            // FLayers
            // 
            this.FLayers.Location = new System.Drawing.Point(948, 62);
            this.FLayers.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.FLayers.Name = "FLayers";
            this.FLayers.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FLayers.Properties.Appearance.Options.UseFont = true;
            this.FLayers.Size = new System.Drawing.Size(187, 38);
            this.FLayers.StyleController = this.lc_ProductInfo;
            this.FLayers.TabIndex = 23;
            // 
            // FGrade
            // 
            this.FGrade.Location = new System.Drawing.Point(1223, 18);
            this.FGrade.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.FGrade.Name = "FGrade";
            this.FGrade.Properties.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F);
            this.FGrade.Properties.Appearance.Options.UseFont = true;
            this.FGrade.Size = new System.Drawing.Size(165, 36);
            this.FGrade.StyleController = this.lc_ProductInfo;
            this.FGrade.TabIndex = 21;
            // 
            // FSpecCustName
            // 
            this.FSpecCustName.Location = new System.Drawing.Point(673, 150);
            this.FSpecCustName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.FSpecCustName.Name = "FSpecCustName";
            this.FSpecCustName.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FSpecCustName.Properties.Appearance.Options.UseFont = true;
            this.FSpecCustName.Size = new System.Drawing.Size(187, 38);
            this.FSpecCustName.StyleController = this.lc_ProductInfo;
            this.FSpecCustName.TabIndex = 20;
            // 
            // FSpecProdName
            // 
            this.FSpecProdName.Location = new System.Drawing.Point(377, 150);
            this.FSpecProdName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.FSpecProdName.Name = "FSpecProdName";
            this.FSpecProdName.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FSpecProdName.Properties.Appearance.Options.UseFont = true;
            this.FSpecProdName.Size = new System.Drawing.Size(208, 38);
            this.FSpecProdName.StyleController = this.lc_ProductInfo;
            this.FSpecProdName.TabIndex = 19;
            // 
            // FLength
            // 
            this.FLength.Location = new System.Drawing.Point(673, 62);
            this.FLength.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.FLength.Name = "FLength";
            this.FLength.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FLength.Properties.Appearance.Options.UseFont = true;
            this.FLength.Size = new System.Drawing.Size(187, 38);
            this.FLength.StyleController = this.lc_ProductInfo;
            this.FLength.TabIndex = 18;
            // 
            // FCoreDiameterOrReam
            // 
            this.FCoreDiameterOrReam.Location = new System.Drawing.Point(100, 62);
            this.FCoreDiameterOrReam.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.FCoreDiameterOrReam.Name = "FCoreDiameterOrReam";
            this.FCoreDiameterOrReam.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FCoreDiameterOrReam.Properties.Appearance.Options.UseFont = true;
            this.FCoreDiameterOrReam.Size = new System.Drawing.Size(189, 38);
            this.FCoreDiameterOrReam.StyleController = this.lc_ProductInfo;
            this.FCoreDiameterOrReam.TabIndex = 17;
            // 
            // FMaterial
            // 
            this.FMaterial.EditValue = "";
            this.FMaterial.Location = new System.Drawing.Point(100, 18);
            this.FMaterial.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.FMaterial.Name = "FMaterial";
            this.FMaterial.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FMaterial.Properties.Appearance.Options.UseFont = true;
            this.FMaterial.Size = new System.Drawing.Size(760, 38);
            this.FMaterial.StyleController = this.lc_ProductInfo;
            this.FMaterial.TabIndex = 16;
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.AppearanceGroup.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.layoutControlGroup1.AppearanceGroup.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.layoutControlGroup1.AppearanceGroup.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.layoutControlGroup1.AppearanceGroup.Options.UseBackColor = true;
            this.layoutControlGroup1.AppearanceGroup.Options.UseBorderColor = true;
            this.layoutControlGroup1.AppearanceItemCaption.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.layoutControlGroup1.AppearanceItemCaption.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.layoutControlGroup1.AppearanceItemCaption.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.layoutControlGroup1.AppearanceItemCaption.Options.UseBackColor = true;
            this.layoutControlGroup1.AppearanceItemCaption.Options.UseBorderColor = true;
            this.layoutControlGroup1.AppearanceTabPage.PageClient.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.layoutControlGroup1.AppearanceTabPage.PageClient.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.layoutControlGroup1.AppearanceTabPage.PageClient.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.layoutControlGroup1.AppearanceTabPage.PageClient.Options.UseBackColor = true;
            this.layoutControlGroup1.AppearanceTabPage.PageClient.Options.UseBorderColor = true;
            this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lo_Material,
            this.lo_CoreDiameterOrReam,
            this.lo_SpecProdName,
            this.lo_Specification,
            this.lo_PackType,
            this.lo_TransportType,
            this.lo_DiameterOrSlides,
            this.lo_Length,
            this.lo_SpecCustName,
            this.lo_Grade,
            this.lo_WeightMode,
            this.lo_Weight,
            this.lo_Count,
            this.lo_Layers,
            this.lo_Sku,
            this.lo_Certification,
            this.lo_Remark,
            this.lo_TrademarkStyle,
            this.lo_PoNumber,
            this.emptySpaceItem1,
            this.lo_PlanLength,
            this.lo_PlanDiameter,
            this.lo_Direction,
            this.lo_SpCustomer,
            this.lo_Color});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Size = new System.Drawing.Size(1406, 294);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // lo_Material
            // 
            this.lo_Material.Control = this.FMaterial;
            this.lo_Material.CustomizationFormText = "物料名称";
            this.lo_Material.Location = new System.Drawing.Point(0, 0);
            this.lo_Material.Name = "lo_Material";
            this.lo_Material.Size = new System.Drawing.Size(848, 44);
            this.lo_Material.Text = "物料名称";
            this.lo_Material.TextSize = new System.Drawing.Size(79, 22);
            // 
            // lo_CoreDiameterOrReam
            // 
            this.lo_CoreDiameterOrReam.Control = this.FCoreDiameterOrReam;
            this.lo_CoreDiameterOrReam.CustomizationFormText = "纸芯/令数";
            this.lo_CoreDiameterOrReam.Location = new System.Drawing.Point(0, 44);
            this.lo_CoreDiameterOrReam.MinSize = new System.Drawing.Size(111, 34);
            this.lo_CoreDiameterOrReam.Name = "lo_CoreDiameterOrReam";
            this.lo_CoreDiameterOrReam.Size = new System.Drawing.Size(277, 44);
            this.lo_CoreDiameterOrReam.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lo_CoreDiameterOrReam.Text = "纸芯/令数";
            this.lo_CoreDiameterOrReam.TextSize = new System.Drawing.Size(79, 22);
            // 
            // lo_SpecProdName
            // 
            this.lo_SpecProdName.Control = this.FSpecProdName;
            this.lo_SpecProdName.CustomizationFormText = "产品专用";
            this.lo_SpecProdName.Location = new System.Drawing.Point(277, 132);
            this.lo_SpecProdName.Name = "lo_SpecProdName";
            this.lo_SpecProdName.Size = new System.Drawing.Size(296, 44);
            this.lo_SpecProdName.Text = "产品专用";
            this.lo_SpecProdName.TextSize = new System.Drawing.Size(79, 22);
            // 
            // lo_Specification
            // 
            this.lo_Specification.Control = this.FSpecification;
            this.lo_Specification.CustomizationFormText = "规格";
            this.lo_Specification.Location = new System.Drawing.Point(848, 0);
            this.lo_Specification.Name = "lo_Specification";
            this.lo_Specification.Size = new System.Drawing.Size(275, 44);
            this.lo_Specification.Text = "规格";
            this.lo_Specification.TextSize = new System.Drawing.Size(79, 22);
            // 
            // lo_PackType
            // 
            this.lo_PackType.Control = this.FPackType;
            this.lo_PackType.CustomizationFormText = "包装类型";
            this.lo_PackType.Location = new System.Drawing.Point(1123, 132);
            this.lo_PackType.Name = "lo_PackType";
            this.lo_PackType.Size = new System.Drawing.Size(253, 44);
            this.lo_PackType.Text = "包装方式";
            this.lo_PackType.TextSize = new System.Drawing.Size(79, 22);
            // 
            // lo_TransportType
            // 
            this.lo_TransportType.Control = this.FTransportType;
            this.lo_TransportType.CustomizationFormText = "夹板方式";
            this.lo_TransportType.Location = new System.Drawing.Point(848, 132);
            this.lo_TransportType.Name = "lo_TransportType";
            this.lo_TransportType.Size = new System.Drawing.Size(275, 44);
            this.lo_TransportType.Text = "夹板包装";
            this.lo_TransportType.TextSize = new System.Drawing.Size(79, 22);
            // 
            // lo_DiameterOrSlides
            // 
            this.lo_DiameterOrSlides.Control = this.FDiameterOrSlides;
            this.lo_DiameterOrSlides.CustomizationFormText = "直径";
            this.lo_DiameterOrSlides.Location = new System.Drawing.Point(277, 44);
            this.lo_DiameterOrSlides.Name = "lo_DiameterOrSlides";
            this.lo_DiameterOrSlides.Size = new System.Drawing.Size(296, 44);
            this.lo_DiameterOrSlides.Text = "直径/令张";
            this.lo_DiameterOrSlides.TextSize = new System.Drawing.Size(79, 22);
            // 
            // lo_Length
            // 
            this.lo_Length.Control = this.FLength;
            this.lo_Length.CustomizationFormText = "线长/张数";
            this.lo_Length.Location = new System.Drawing.Point(573, 44);
            this.lo_Length.Name = "lo_Length";
            this.lo_Length.Size = new System.Drawing.Size(275, 44);
            this.lo_Length.Text = "线长/张数";
            this.lo_Length.TextSize = new System.Drawing.Size(79, 22);
            // 
            // lo_SpecCustName
            // 
            this.lo_SpecCustName.Control = this.FSpecCustName;
            this.lo_SpecCustName.CustomizationFormText = "客户专用";
            this.lo_SpecCustName.Location = new System.Drawing.Point(573, 132);
            this.lo_SpecCustName.Name = "lo_SpecCustName";
            this.lo_SpecCustName.Size = new System.Drawing.Size(275, 44);
            this.lo_SpecCustName.Text = "客户专用";
            this.lo_SpecCustName.TextSize = new System.Drawing.Size(79, 22);
            // 
            // lo_Grade
            // 
            this.lo_Grade.Control = this.FGrade;
            this.lo_Grade.CustomizationFormText = "等级";
            this.lo_Grade.Location = new System.Drawing.Point(1123, 0);
            this.lo_Grade.Name = "lo_Grade";
            this.lo_Grade.Size = new System.Drawing.Size(253, 44);
            this.lo_Grade.Text = "等级";
            this.lo_Grade.TextSize = new System.Drawing.Size(79, 22);
            // 
            // lo_WeightMode
            // 
            this.lo_WeightMode.Control = this.FWeightMode;
            this.lo_WeightMode.CustomizationFormText = "重量模式";
            this.lo_WeightMode.Location = new System.Drawing.Point(573, 88);
            this.lo_WeightMode.Name = "lo_WeightMode";
            this.lo_WeightMode.Size = new System.Drawing.Size(275, 44);
            this.lo_WeightMode.Text = "计重方式";
            this.lo_WeightMode.TextSize = new System.Drawing.Size(79, 22);
            // 
            // lo_Weight
            // 
            this.lo_Weight.Control = this.FWeight;
            this.lo_Weight.CustomizationFormText = "重量";
            this.lo_Weight.Location = new System.Drawing.Point(848, 88);
            this.lo_Weight.Name = "lo_Weight";
            this.lo_Weight.Size = new System.Drawing.Size(275, 44);
            this.lo_Weight.Text = "重量";
            this.lo_Weight.TextSize = new System.Drawing.Size(79, 22);
            // 
            // lo_Count
            // 
            this.lo_Count.Control = this.FCount;
            this.lo_Count.CustomizationFormText = "件数";
            this.lo_Count.Location = new System.Drawing.Point(1123, 88);
            this.lo_Count.Name = "lo_Count";
            this.lo_Count.Size = new System.Drawing.Size(253, 44);
            this.lo_Count.Text = "入库件数";
            this.lo_Count.TextSize = new System.Drawing.Size(79, 22);
            // 
            // lo_Layers
            // 
            this.lo_Layers.Control = this.FLayers;
            this.lo_Layers.CustomizationFormText = "层数";
            this.lo_Layers.Location = new System.Drawing.Point(848, 44);
            this.lo_Layers.Name = "lo_Layers";
            this.lo_Layers.Size = new System.Drawing.Size(275, 44);
            this.lo_Layers.Text = "层数";
            this.lo_Layers.TextSize = new System.Drawing.Size(79, 22);
            // 
            // lo_Sku
            // 
            this.lo_Sku.Control = this.FSku;
            this.lo_Sku.CustomizationFormText = "SKU";
            this.lo_Sku.Location = new System.Drawing.Point(1123, 44);
            this.lo_Sku.Name = "lo_Sku";
            this.lo_Sku.Size = new System.Drawing.Size(253, 44);
            this.lo_Sku.Text = "SKU";
            this.lo_Sku.TextSize = new System.Drawing.Size(79, 22);
            // 
            // lo_Certification
            // 
            this.lo_Certification.Control = this.FCertification;
            this.lo_Certification.CustomizationFormText = "纸种认证";
            this.lo_Certification.Location = new System.Drawing.Point(277, 88);
            this.lo_Certification.Name = "lo_Certification";
            this.lo_Certification.Size = new System.Drawing.Size(296, 44);
            this.lo_Certification.Text = "纸种认证";
            this.lo_Certification.TextSize = new System.Drawing.Size(79, 22);
            // 
            // lo_Remark
            // 
            this.lo_Remark.Control = this.FRemark;
            this.lo_Remark.CustomizationFormText = "备注";
            this.lo_Remark.Location = new System.Drawing.Point(573, 176);
            this.lo_Remark.Name = "lo_Remark";
            this.lo_Remark.Size = new System.Drawing.Size(803, 44);
            this.lo_Remark.Text = "备注";
            this.lo_Remark.TextSize = new System.Drawing.Size(79, 22);
            // 
            // lo_TrademarkStyle
            // 
            this.lo_TrademarkStyle.Control = this.FTrademarkStyle;
            this.lo_TrademarkStyle.CustomizationFormText = "商标类型";
            this.lo_TrademarkStyle.Location = new System.Drawing.Point(0, 88);
            this.lo_TrademarkStyle.MinSize = new System.Drawing.Size(111, 34);
            this.lo_TrademarkStyle.Name = "lo_TrademarkStyle";
            this.lo_TrademarkStyle.Size = new System.Drawing.Size(277, 44);
            this.lo_TrademarkStyle.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lo_TrademarkStyle.Text = "商标类型";
            this.lo_TrademarkStyle.TextSize = new System.Drawing.Size(79, 22);
            // 
            // lo_PoNumber
            // 
            this.lo_PoNumber.Control = this.FPoNumber;
            this.lo_PoNumber.CustomizationFormText = "订单号";
            this.lo_PoNumber.Location = new System.Drawing.Point(0, 132);
            this.lo_PoNumber.Name = "lo_PoNumber";
            this.lo_PoNumber.Size = new System.Drawing.Size(277, 44);
            this.lo_PoNumber.Text = "订单号";
            this.lo_PoNumber.TextSize = new System.Drawing.Size(79, 22);
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.Location = new System.Drawing.Point(984, 220);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(392, 44);
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // lo_PlanLength
            // 
            this.lo_PlanLength.Control = this.FPlanLength;
            this.lo_PlanLength.Location = new System.Drawing.Point(0, 220);
            this.lo_PlanLength.Name = "lo_PlanLength";
            this.lo_PlanLength.Size = new System.Drawing.Size(277, 44);
            this.lo_PlanLength.Text = "计划线长";
            this.lo_PlanLength.TextSize = new System.Drawing.Size(79, 22);
            // 
            // lo_PlanDiameter
            // 
            this.lo_PlanDiameter.Control = this.FPlanDiameter;
            this.lo_PlanDiameter.Location = new System.Drawing.Point(277, 220);
            this.lo_PlanDiameter.Name = "lo_PlanDiameter";
            this.lo_PlanDiameter.Size = new System.Drawing.Size(296, 44);
            this.lo_PlanDiameter.Text = "计划直径";
            this.lo_PlanDiameter.TextSize = new System.Drawing.Size(79, 22);
            // 
            // lo_Direction
            // 
            this.lo_Direction.Control = this.FDirection;
            this.lo_Direction.CustomizationFormText = "方向";
            this.lo_Direction.Location = new System.Drawing.Point(573, 220);
            this.lo_Direction.MinSize = new System.Drawing.Size(111, 34);
            this.lo_Direction.Name = "lo_Direction";
            this.lo_Direction.Size = new System.Drawing.Size(411, 44);
            this.lo_Direction.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lo_Direction.Text = "方向";
            this.lo_Direction.TextSize = new System.Drawing.Size(79, 22);
            this.lo_Direction.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            // 
            // lo_SpCustomer
            // 
            this.lo_SpCustomer.Control = this.FSpCustomer;
            this.lo_SpCustomer.Location = new System.Drawing.Point(277, 176);
            this.lo_SpCustomer.Name = "lo_SpCustomer";
            this.lo_SpCustomer.Size = new System.Drawing.Size(296, 44);
            this.lo_SpCustomer.Text = "产品备注";
            this.lo_SpCustomer.TextSize = new System.Drawing.Size(79, 22);
            // 
            // lo_Color
            // 
            this.lo_Color.Control = this.FColor;
            this.lo_Color.CustomizationFormText = "色相";
            this.lo_Color.Location = new System.Drawing.Point(0, 176);
            this.lo_Color.Name = "lo_Color";
            this.lo_Color.Size = new System.Drawing.Size(277, 44);
            this.lo_Color.Text = "色相";
            this.lo_Color.TextSize = new System.Drawing.Size(79, 22);
            // 
            // gc_ScanInfo
            // 
            this.gc_ScanInfo.Controls.Add(this.lc_ScanInfo);
            this.gc_ScanInfo.Location = new System.Drawing.Point(18, 64);
            this.gc_ScanInfo.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gc_ScanInfo.Name = "gc_ScanInfo";
            this.gc_ScanInfo.Size = new System.Drawing.Size(1412, 372);
            this.gc_ScanInfo.TabIndex = 5;
            this.gc_ScanInfo.Text = "扫描信息";
            // 
            // lc_ScanInfo
            // 
            this.lc_ScanInfo.Controls.Add(this.FPlcInfo);
            this.lc_ScanInfo.Controls.Add(this.FMonitor);
            this.lc_ScanInfo.Controls.Add(this.FScannerStatus);
            this.lc_ScanInfo.Controls.Add(this.FPlcConStatus);
            this.lc_ScanInfo.Controls.Add(this.FLedConStatus);
            this.lc_ScanInfo.Controls.Add(this.FScannerConStatus);
            this.lc_ScanInfo.Controls.Add(this.FBarcode);
            this.lc_ScanInfo.Controls.Add(this.FResult);
            this.lc_ScanInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lc_ScanInfo.Location = new System.Drawing.Point(3, 33);
            this.lc_ScanInfo.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lc_ScanInfo.Name = "lc_ScanInfo";
            this.lc_ScanInfo.Root = this.layoutControlGroup4;
            this.lc_ScanInfo.Size = new System.Drawing.Size(1406, 336);
            this.lc_ScanInfo.TabIndex = 0;
            this.lc_ScanInfo.Text = "扫描信息";
            // 
            // FPlcInfo
            // 
            this.FPlcInfo.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.FPlcInfo.Location = new System.Drawing.Point(18, 146);
            this.FPlcInfo.Name = "FPlcInfo";
            this.FPlcInfo.Size = new System.Drawing.Size(1105, 26);
            this.FPlcInfo.StyleController = this.lc_ScanInfo;
            this.FPlcInfo.TabIndex = 17;
            this.FPlcInfo.Text = "PLC数据读取中";
            // 
            // FMonitor
            // 
            this.FMonitor.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.FMonitor.Appearance.Options.UseBackColor = true;
            this.FMonitor.Location = new System.Drawing.Point(865, 178);
            this.FMonitor.Name = "FMonitor";
            this.FMonitor.Size = new System.Drawing.Size(523, 140);
            this.FMonitor.StyleController = this.lc_ScanInfo;
            this.FMonitor.TabIndex = 16;
            // 
            // FScannerStatus
            // 
            this.FScannerStatus.Appearance.Font = new System.Drawing.Font("Tahoma", 14F);
            this.FScannerStatus.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.FScannerStatus.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.FScannerStatus.Location = new System.Drawing.Point(1129, 138);
            this.FScannerStatus.Name = "FScannerStatus";
            this.FScannerStatus.Size = new System.Drawing.Size(259, 34);
            this.FScannerStatus.StyleController = this.lc_ScanInfo;
            this.FScannerStatus.TabIndex = 8;
            // 
            // FPlcConStatus
            // 
            this.FPlcConStatus.Appearance.Font = new System.Drawing.Font("Tahoma", 14F);
            this.FPlcConStatus.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.FPlcConStatus.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.FPlcConStatus.Location = new System.Drawing.Point(1129, 18);
            this.FPlcConStatus.Name = "FPlcConStatus";
            this.FPlcConStatus.Size = new System.Drawing.Size(259, 34);
            this.FPlcConStatus.StyleController = this.lc_ScanInfo;
            this.FPlcConStatus.TabIndex = 5;
            // 
            // FLedConStatus
            // 
            this.FLedConStatus.Appearance.Font = new System.Drawing.Font("Tahoma", 14F);
            this.FLedConStatus.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.FLedConStatus.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.FLedConStatus.Location = new System.Drawing.Point(1129, 98);
            this.FLedConStatus.Name = "FLedConStatus";
            this.FLedConStatus.Size = new System.Drawing.Size(259, 34);
            this.FLedConStatus.StyleController = this.lc_ScanInfo;
            this.FLedConStatus.TabIndex = 7;
            // 
            // FScannerConStatus
            // 
            this.FScannerConStatus.Appearance.Font = new System.Drawing.Font("Tahoma", 14F);
            this.FScannerConStatus.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.FScannerConStatus.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.FScannerConStatus.Location = new System.Drawing.Point(1129, 58);
            this.FScannerConStatus.Name = "FScannerConStatus";
            this.FScannerConStatus.Size = new System.Drawing.Size(259, 34);
            this.FScannerConStatus.StyleController = this.lc_ScanInfo;
            this.FScannerConStatus.TabIndex = 6;
            // 
            // FBarcode
            // 
            this.FBarcode.EditValue = "1234567890123456";
            this.FBarcode.Location = new System.Drawing.Point(93, 18);
            this.FBarcode.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.FBarcode.Name = "FBarcode";
            this.FBarcode.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FBarcode.Properties.Appearance.Options.UseFont = true;
            this.FBarcode.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.FBarcode.Size = new System.Drawing.Size(1030, 122);
            this.FBarcode.StyleController = this.lc_ScanInfo;
            this.FBarcode.TabIndex = 4;
            // 
            // FResult
            // 
            this.FResult.EditValue = "";
            this.FResult.Location = new System.Drawing.Point(93, 178);
            this.FResult.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.FResult.Name = "FResult";
            this.FResult.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 48F, System.Drawing.FontStyle.Bold);
            this.FResult.Properties.Appearance.Options.UseFont = true;
            this.FResult.Size = new System.Drawing.Size(766, 122);
            this.FResult.StyleController = this.lc_ScanInfo;
            this.FResult.TabIndex = 5;
            // 
            // layoutControlGroup4
            // 
            this.layoutControlGroup4.AppearanceGroup.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.layoutControlGroup4.AppearanceGroup.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.layoutControlGroup4.AppearanceGroup.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.layoutControlGroup4.AppearanceGroup.Options.UseBackColor = true;
            this.layoutControlGroup4.AppearanceGroup.Options.UseBorderColor = true;
            this.layoutControlGroup4.AppearanceItemCaption.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.layoutControlGroup4.AppearanceItemCaption.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.layoutControlGroup4.AppearanceItemCaption.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.layoutControlGroup4.AppearanceItemCaption.Options.UseBackColor = true;
            this.layoutControlGroup4.AppearanceItemCaption.Options.UseBorderColor = true;
            this.layoutControlGroup4.AppearanceTabPage.PageClient.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.layoutControlGroup4.AppearanceTabPage.PageClient.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.layoutControlGroup4.AppearanceTabPage.PageClient.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.layoutControlGroup4.AppearanceTabPage.PageClient.Options.UseBackColor = true;
            this.layoutControlGroup4.AppearanceTabPage.PageClient.Options.UseBorderColor = true;
            this.layoutControlGroup4.CustomizationFormText = "layoutControlGroup4";
            this.layoutControlGroup4.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup4.GroupBordersVisible = false;
            this.layoutControlGroup4.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lo_Barcode,
            this.layoutControlItem7,
            this.layoutControlItem6,
            this.layoutControlItem8,
            this.layoutControlItem9,
            this.layoutControlItem10,
            this.lo_Result,
            this.layoutControlItem13});
            this.layoutControlGroup4.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup4.Name = "layoutControlGroup4";
            this.layoutControlGroup4.Size = new System.Drawing.Size(1406, 336);
            this.layoutControlGroup4.TextVisible = false;
            // 
            // lo_Barcode
            // 
            this.lo_Barcode.Control = this.FBarcode;
            this.lo_Barcode.CustomizationFormText = "条形码";
            this.lo_Barcode.Location = new System.Drawing.Point(0, 0);
            this.lo_Barcode.Name = "lo_Barcode";
            this.lo_Barcode.Size = new System.Drawing.Size(1111, 128);
            this.lo_Barcode.Text = "条形码";
            this.lo_Barcode.TextSize = new System.Drawing.Size(72, 22);
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.Control = this.FPlcConStatus;
            this.layoutControlItem7.Location = new System.Drawing.Point(1111, 0);
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Size = new System.Drawing.Size(265, 40);
            this.layoutControlItem7.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem7.TextVisible = false;
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this.FScannerConStatus;
            this.layoutControlItem6.Location = new System.Drawing.Point(1111, 40);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Size = new System.Drawing.Size(265, 40);
            this.layoutControlItem6.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem6.TextVisible = false;
            // 
            // layoutControlItem8
            // 
            this.layoutControlItem8.Control = this.FLedConStatus;
            this.layoutControlItem8.Location = new System.Drawing.Point(1111, 80);
            this.layoutControlItem8.Name = "layoutControlItem8";
            this.layoutControlItem8.Size = new System.Drawing.Size(265, 40);
            this.layoutControlItem8.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem8.TextVisible = false;
            // 
            // layoutControlItem9
            // 
            this.layoutControlItem9.Control = this.FScannerStatus;
            this.layoutControlItem9.Location = new System.Drawing.Point(1111, 120);
            this.layoutControlItem9.Name = "layoutControlItem9";
            this.layoutControlItem9.Size = new System.Drawing.Size(265, 40);
            this.layoutControlItem9.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem9.TextVisible = false;
            // 
            // layoutControlItem10
            // 
            this.layoutControlItem10.Control = this.FMonitor;
            this.layoutControlItem10.Location = new System.Drawing.Point(847, 160);
            this.layoutControlItem10.Name = "layoutControlItem10";
            this.layoutControlItem10.Size = new System.Drawing.Size(529, 146);
            this.layoutControlItem10.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem10.TextVisible = false;
            // 
            // lo_Result
            // 
            this.lo_Result.Control = this.FResult;
            this.lo_Result.CustomizationFormText = "扫描结果";
            this.lo_Result.Location = new System.Drawing.Point(0, 160);
            this.lo_Result.Name = "lo_Result";
            this.lo_Result.Size = new System.Drawing.Size(847, 146);
            this.lo_Result.Text = "扫描结果";
            this.lo_Result.TextSize = new System.Drawing.Size(72, 22);
            // 
            // layoutControlItem13
            // 
            this.layoutControlItem13.Control = this.FPlcInfo;
            this.layoutControlItem13.Location = new System.Drawing.Point(0, 128);
            this.layoutControlItem13.MinSize = new System.Drawing.Size(109, 28);
            this.layoutControlItem13.Name = "layoutControlItem13";
            this.layoutControlItem13.Size = new System.Drawing.Size(1111, 32);
            this.layoutControlItem13.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem13.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem13.TextVisible = false;
            // 
            // layoutControlGroup3
            // 
            this.layoutControlGroup3.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup3.GroupBordersVisible = false;
            this.layoutControlGroup3.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlItem2,
            this.layoutControlItem3,
            this.emptySpaceItem3,
            this.layoutControlItem5,
            this.layoutControlItem11,
            this.lc_DirectionConfig,
            this.layoutControlItem12,
            this.lc_ForceToManual});
            this.layoutControlGroup3.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup3.Name = "Root";
            this.layoutControlGroup3.OptionsItemText.TextToControlDistance = 5;
            this.layoutControlGroup3.Size = new System.Drawing.Size(1448, 1028);
            this.layoutControlGroup3.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.gc_ScanInfo;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 46);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(1418, 378);
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextVisible = false;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.gc_ProducytInfo;
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 424);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(1418, 336);
            this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem2.TextVisible = false;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.gc_StockInfo;
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 760);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(1418, 238);
            this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem3.TextVisible = false;
            // 
            // emptySpaceItem3
            // 
            this.emptySpaceItem3.AllowHotTrack = false;
            this.emptySpaceItem3.Location = new System.Drawing.Point(1394, 0);
            this.emptySpaceItem3.Name = "emptySpaceItem3";
            this.emptySpaceItem3.Size = new System.Drawing.Size(24, 46);
            this.emptySpaceItem3.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.btMotorStart;
            this.layoutControlItem5.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem5.MinSize = new System.Drawing.Size(219, 46);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Size = new System.Drawing.Size(289, 46);
            this.layoutControlItem5.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem5.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem5.TextVisible = false;
            // 
            // layoutControlItem11
            // 
            this.layoutControlItem11.Control = this.btUseScanner;
            this.layoutControlItem11.Location = new System.Drawing.Point(289, 0);
            this.layoutControlItem11.MinSize = new System.Drawing.Size(130, 38);
            this.layoutControlItem11.Name = "layoutControlItem11";
            this.layoutControlItem11.Size = new System.Drawing.Size(293, 46);
            this.layoutControlItem11.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem11.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem11.TextVisible = false;
            // 
            // lc_DirectionConfig
            // 
            this.lc_DirectionConfig.Control = this.btDirectionConfig;
            this.lc_DirectionConfig.Location = new System.Drawing.Point(891, 0);
            this.lc_DirectionConfig.MinSize = new System.Drawing.Size(173, 46);
            this.lc_DirectionConfig.Name = "lc_DirectionConfig";
            this.lc_DirectionConfig.Size = new System.Drawing.Size(293, 46);
            this.lc_DirectionConfig.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lc_DirectionConfig.TextSize = new System.Drawing.Size(0, 0);
            this.lc_DirectionConfig.TextVisible = false;
            // 
            // layoutControlItem12
            // 
            this.layoutControlItem12.Control = this.btOpenScanner;
            this.layoutControlItem12.Location = new System.Drawing.Point(582, 0);
            this.layoutControlItem12.MinSize = new System.Drawing.Size(147, 46);
            this.layoutControlItem12.Name = "layoutControlItem12";
            this.layoutControlItem12.Size = new System.Drawing.Size(309, 46);
            this.layoutControlItem12.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem12.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem12.TextVisible = false;
            // 
            // lc_ForceToManual
            // 
            this.lc_ForceToManual.Control = this.cb_ForceToManual;
            this.lc_ForceToManual.Location = new System.Drawing.Point(1184, 0);
            this.lc_ForceToManual.Name = "lc_ForceToManual";
            this.lc_ForceToManual.Size = new System.Drawing.Size(210, 46);
            this.lc_ForceToManual.TextSize = new System.Drawing.Size(0, 0);
            this.lc_ForceToManual.TextVisible = false;
            // 
            // xtraScrollableControl1
            // 
            this.xtraScrollableControl1.Controls.Add(this.layoutControl1);
            this.xtraScrollableControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraScrollableControl1.Location = new System.Drawing.Point(0, 0);
            this.xtraScrollableControl1.Name = "xtraScrollableControl1";
            this.xtraScrollableControl1.Size = new System.Drawing.Size(1448, 1028);
            this.xtraScrollableControl1.TabIndex = 9;
            // 
            // StockInAutoScan
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 22F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1448, 1028);
            this.Controls.Add(this.xtraScrollableControl1);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "StockInAutoScan";
            this.Text = "自动扫描入库";
            ((System.ComponentModel.ISupportInitialize)(this.gc_StockInfo)).EndInit();
            this.gc_StockInfo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lc_StockInfo)).EndInit();
            this.lc_StockInfo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.FMessage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FDateS.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FDateS.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FOperator.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLookUpEdit1View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FBusinessType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FShiftTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FWHRemark.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FShift.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FWareHouse.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FFactory.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_ShiftTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_DateS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_BusinessType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_WHRemark)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_Shift)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_Factory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_WareHouse)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_Operator)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cb_ForceToManual.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gc_ProducytInfo)).EndInit();
            this.gc_ProducytInfo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lc_ProductInfo)).EndInit();
            this.lc_ProductInfo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.FPlanDiameter.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FPlanLength.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FSpCustomer.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FColor.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FDiameterOrSlides.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FRemark.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FDirection.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FTransportType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FPackType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FTrademarkStyle.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FPoNumber.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FWeight.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FSku.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FCount.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FWeightMode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FSpecification.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FCertification.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FLayers.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FGrade.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FSpecCustName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FSpecProdName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FLength.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FCoreDiameterOrReam.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FMaterial.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_Material)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_CoreDiameterOrReam)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_SpecProdName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_Specification)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_PackType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_TransportType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_DiameterOrSlides)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_Length)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_SpecCustName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_Grade)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_WeightMode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_Weight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_Count)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_Layers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_Sku)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_Certification)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_Remark)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_TrademarkStyle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_PoNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_PlanLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_PlanDiameter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_Direction)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_SpCustomer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_Color)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gc_ScanInfo)).EndInit();
            this.gc_ScanInfo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lc_ScanInfo)).EndInit();
            this.lc_ScanInfo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.FMonitor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FBarcode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FResult.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_Barcode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lo_Result)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem13)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem11)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lc_DirectionConfig)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem12)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lc_ForceToManual)).EndInit();
            this.xtraScrollableControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion
        private System.IO.Ports.SerialPort spScan;
        private DevExpress.XtraEditors.GroupControl gc_ScanInfo;
        private DevExpress.XtraLayout.LayoutControl lc_ScanInfo;
        private DevExpress.XtraEditors.ButtonEdit FBarcode;
        private DevExpress.XtraEditors.TextEdit FResult;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup4;
        private DevExpress.XtraLayout.LayoutControlItem lo_Barcode;
        private DevExpress.XtraLayout.LayoutControlItem lo_Result;
        private DevExpress.XtraEditors.GroupControl gc_ProducytInfo;
        private DevExpress.XtraLayout.LayoutControl lc_ProductInfo;
        private DevExpress.XtraEditors.TextEdit FDiameterOrSlides;
        private DevExpress.XtraEditors.TextEdit FRemark;
        private DevExpress.XtraEditors.TextEdit FDirection;
        private DevExpress.XtraEditors.TextEdit FTransportType;
        private DevExpress.XtraEditors.TextEdit FPackType;
        private DevExpress.XtraEditors.TextEdit FTrademarkStyle;
        private DevExpress.XtraEditors.TextEdit FPoNumber;
        private DevExpress.XtraEditors.TextEdit FWeight;
        private DevExpress.XtraEditors.TextEdit FSku;
        private DevExpress.XtraEditors.TextEdit FCount;
        private DevExpress.XtraEditors.TextEdit FWeightMode;
        private DevExpress.XtraEditors.TextEdit FSpecification;
        private DevExpress.XtraEditors.TextEdit FCertification;
        private DevExpress.XtraEditors.TextEdit FLayers;
        private DevExpress.XtraEditors.TextEdit FGrade;
        private DevExpress.XtraEditors.TextEdit FSpecCustName;
        private DevExpress.XtraEditors.TextEdit FSpecProdName;
        private DevExpress.XtraEditors.TextEdit FLength;
        private DevExpress.XtraEditors.TextEdit FCoreDiameterOrReam;
        private DevExpress.XtraEditors.TextEdit FMaterial;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.LayoutControlItem lo_Material;
        private DevExpress.XtraLayout.LayoutControlItem lo_CoreDiameterOrReam;
        private DevExpress.XtraLayout.LayoutControlItem lo_SpecProdName;
        private DevExpress.XtraLayout.LayoutControlItem lo_Specification;
        private DevExpress.XtraLayout.LayoutControlItem lo_PackType;
        private DevExpress.XtraLayout.LayoutControlItem lo_TransportType;
        private DevExpress.XtraLayout.LayoutControlItem lo_DiameterOrSlides;
        private DevExpress.XtraLayout.LayoutControlItem lo_Length;
        private DevExpress.XtraLayout.LayoutControlItem lo_SpecCustName;
        private DevExpress.XtraLayout.LayoutControlItem lo_Grade;
        private DevExpress.XtraLayout.LayoutControlItem lo_WeightMode;
        private DevExpress.XtraLayout.LayoutControlItem lo_Weight;
        private DevExpress.XtraLayout.LayoutControlItem lo_Count;
        private DevExpress.XtraLayout.LayoutControlItem lo_Layers;
        private DevExpress.XtraLayout.LayoutControlItem lo_Sku;
        private DevExpress.XtraLayout.LayoutControlItem lo_Certification;
        private DevExpress.XtraLayout.LayoutControlItem lo_Remark;
        private DevExpress.XtraLayout.LayoutControlItem lo_TrademarkStyle;
        private DevExpress.XtraLayout.LayoutControlItem lo_PoNumber;
        private DevExpress.XtraLayout.LayoutControlItem lo_Direction;
        private DevExpress.XtraEditors.GroupControl gc_StockInfo;
        private DevExpress.XtraLayout.LayoutControl lc_StockInfo;
        private DevExpress.XtraEditors.DateEdit FDateS;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup2;
        private DevExpress.XtraLayout.LayoutControlItem lo_ShiftTime;
        private DevExpress.XtraLayout.LayoutControlItem lo_DateS;
        private DevExpress.XtraLayout.LayoutControlItem lo_BusinessType;
        private DevExpress.XtraLayout.LayoutControlItem lo_WHRemark;
        private DevExpress.XtraLayout.LayoutControlItem lo_Operator;
        private DevExpress.XtraLayout.LayoutControlItem lo_Shift;
        private DevExpress.XtraLayout.LayoutControlItem lo_Factory;
        private DevExpress.XtraEditors.ListBoxControl FMessage;
        private DevExpress.XtraLayout.LayoutControlItem lo_WareHouse;
        private DevExpress.XtraEditors.SearchLookUpEdit FOperator;
        private DevExpress.XtraGrid.Views.Grid.GridView searchLookUpEdit1View;
        private DevExpress.XtraEditors.SearchLookUpEdit FBusinessType;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView5;
        private DevExpress.XtraEditors.XtraScrollableControl xtraScrollableControl1;
        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup3;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraEditors.ComboBoxEdit FShiftTime;
        private DevExpress.XtraEditors.ComboBoxEdit FWHRemark;
        private DevExpress.XtraEditors.ComboBoxEdit FShift;
        private DevExpress.XtraEditors.ComboBoxEdit FWareHouse;
        private DevExpress.XtraEditors.LabelControl FScannerConStatus;
        private DevExpress.XtraEditors.LabelControl FPlcConStatus;
        private DevExpress.XtraEditors.LabelControl FScannerStatus;
        private DevExpress.XtraEditors.LabelControl FLedConStatus;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem7;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem8;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem9;
        private DevExpress.XtraEditors.ListBoxControl FMonitor;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem10;
        private DevExpress.XtraEditors.SimpleButton btUseScanner;
        private DevExpress.XtraEditors.SimpleButton btMotorStart;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem11;
        private DevExpress.XtraEditors.ComboBoxEdit FFactory;
        private DevExpress.XtraEditors.TextEdit FSpCustomer;
        private DevExpress.XtraEditors.TextEdit FColor;
        private DevExpress.XtraLayout.LayoutControlItem lo_Color;
        private DevExpress.XtraLayout.LayoutControlItem lo_SpCustomer;
        private DevExpress.XtraEditors.SimpleButton btOpenScanner;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem12;
        private DevExpress.XtraEditors.TextEdit FPlanLength;
        private DevExpress.XtraLayout.LayoutControlItem lo_PlanLength;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
        private DevExpress.XtraEditors.TextEdit FPlanDiameter;
        private DevExpress.XtraLayout.LayoutControlItem lo_PlanDiameter;
        private DevExpress.XtraEditors.LabelControl FPlcInfo;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem13;
        private DevExpress.XtraEditors.SimpleButton btDirectionConfig;
        private DevExpress.XtraLayout.LayoutControlItem lc_DirectionConfig;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem3;
        private DevExpress.XtraEditors.CheckEdit cb_ForceToManual;
        private DevExpress.XtraLayout.LayoutControlItem lc_ForceToManual;
    }
}