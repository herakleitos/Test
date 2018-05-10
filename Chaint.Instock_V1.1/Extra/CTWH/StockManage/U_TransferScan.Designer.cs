namespace CTWH.StockManage
{
    partial class U_TransferScan
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.txt_result = new DevExpress.XtraEditors.MemoEdit();
            this.btn_LoadFactory = new DevExpress.XtraEditors.SimpleButton();
            this.cmb_operate = new DevExpress.XtraEditors.ComboBoxEdit();
            this.cmb_dest = new DevExpress.XtraEditors.ComboBoxEdit();
            this.cmb_accept = new DevExpress.XtraEditors.ComboBoxEdit();
            this.cmb_Source = new DevExpress.XtraEditors.ComboBoxEdit();
            this.cmb_barcode = new DevExpress.XtraEditors.ComboBoxEdit();
            this.btn_Save = new DevExpress.XtraEditors.SimpleButton();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.emptySpaceItem3 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem8 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem9 = new DevExpress.XtraLayout.LayoutControlItem();
            this.spScan = new System.IO.Ports.SerialPort(this.components);
            this.btn_Dest = new DevExpress.XtraEditors.SimpleButton();
            this.layoutControlItem7 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txt_result.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmb_operate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmb_dest.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmb_accept.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmb_Source.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmb_barcode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.txt_result);
            this.layoutControl1.Controls.Add(this.cmb_operate);
            this.layoutControl1.Controls.Add(this.cmb_dest);
            this.layoutControl1.Controls.Add(this.cmb_accept);
            this.layoutControl1.Controls.Add(this.cmb_Source);
            this.layoutControl1.Controls.Add(this.btn_LoadFactory);
            this.layoutControl1.Controls.Add(this.cmb_barcode);
            this.layoutControl1.Controls.Add(this.btn_Save);
            this.layoutControl1.Controls.Add(this.btn_Dest);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(1000, 582);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // txt_result
            // 
            this.txt_result.Location = new System.Drawing.Point(64, 146);
            this.txt_result.Name = "txt_result";
            this.txt_result.Size = new System.Drawing.Size(198, 61);
            this.txt_result.StyleController = this.layoutControl1;
            this.txt_result.TabIndex = 12;
            this.txt_result.UseOptimizedRendering = true;
            // 
            // btn_LoadFactory
            // 
            this.btn_LoadFactory.Image = global::CTWH.Properties.Resources.Synchronize_16x16;
            this.btn_LoadFactory.Location = new System.Drawing.Point(166, 46);
            this.btn_LoadFactory.Name = "btn_LoadFactory";
            this.btn_LoadFactory.Size = new System.Drawing.Size(96, 22);
            this.btn_LoadFactory.StyleController = this.layoutControl1;
            this.btn_LoadFactory.TabIndex = 11;
            this.btn_LoadFactory.Text = "加载机台";
            this.btn_LoadFactory.Click += new System.EventHandler(this.btn_LoadFactory_Click);
            // 
            // cmb_operate
            // 
            this.cmb_operate.Location = new System.Drawing.Point(64, 122);
            this.cmb_operate.Name = "cmb_operate";
            this.cmb_operate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmb_operate.Properties.Items.AddRange(new object[] {
            "A.添加",
            "U.更新",
            "D.删除"});
            this.cmb_operate.Size = new System.Drawing.Size(198, 20);
            this.cmb_operate.StyleController = this.layoutControl1;
            this.cmb_operate.TabIndex = 9;
            // 
            // cmb_dest
            // 
            this.cmb_dest.Location = new System.Drawing.Point(64, 98);
            this.cmb_dest.Name = "cmb_dest";
            this.cmb_dest.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmb_dest.Size = new System.Drawing.Size(198, 20);
            this.cmb_dest.StyleController = this.layoutControl1;
            this.cmb_dest.TabIndex = 8;
            // 
            // cmb_accept
            // 
            this.cmb_accept.Location = new System.Drawing.Point(64, 72);
            this.cmb_accept.Name = "cmb_accept";
            this.cmb_accept.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmb_accept.Size = new System.Drawing.Size(98, 20);
            this.cmb_accept.StyleController = this.layoutControl1;
            this.cmb_accept.TabIndex = 7;
            this.cmb_accept.TextChanged += new System.EventHandler(this.cmb_accept_TextChanged);
            // 
            // cmb_Source
            // 
            this.cmb_Source.Location = new System.Drawing.Point(64, 46);
            this.cmb_Source.Name = "cmb_Source";
            this.cmb_Source.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmb_Source.Size = new System.Drawing.Size(98, 20);
            this.cmb_Source.StyleController = this.layoutControl1;
            this.cmb_Source.TabIndex = 6;
            // 
            // cmb_barcode
            // 
            this.cmb_barcode.EditValue = "";
            this.cmb_barcode.Location = new System.Drawing.Point(64, 12);
            this.cmb_barcode.Name = "cmb_barcode";
            this.cmb_barcode.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 14F);
            this.cmb_barcode.Properties.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.cmb_barcode.Properties.Appearance.Options.UseFont = true;
            this.cmb_barcode.Properties.Appearance.Options.UseForeColor = true;
            this.cmb_barcode.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmb_barcode.Size = new System.Drawing.Size(198, 30);
            this.cmb_barcode.StyleController = this.layoutControl1;
            this.cmb_barcode.TabIndex = 5;
            // 
            // btn_Save
            // 
            this.btn_Save.Image = global::CTWH.Properties.Resources.Save_16x16;
            this.btn_Save.Location = new System.Drawing.Point(166, 211);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Size = new System.Drawing.Size(96, 22);
            this.btn_Save.StyleController = this.layoutControl1;
            this.btn_Save.TabIndex = 4;
            this.btn_Save.Text = "保存";
            this.btn_Save.Click += new System.EventHandler(this.btn_Save_Click);
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem2,
            this.layoutControlItem3,
            this.layoutControlItem4,
            this.layoutControlItem5,
            this.layoutControlItem6,
            this.layoutControlItem1,
            this.emptySpaceItem1,
            this.emptySpaceItem3,
            this.emptySpaceItem2,
            this.layoutControlItem9,
            this.layoutControlItem8,
            this.layoutControlItem7});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Size = new System.Drawing.Size(1000, 582);
            this.layoutControlGroup1.Text = "layoutControlGroup1";
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.cmb_barcode;
            this.layoutControlItem2.CustomizationFormText = "条形码";
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(254, 34);
            this.layoutControlItem2.Text = "条形码";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(48, 14);
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.cmb_Source;
            this.layoutControlItem3.CustomizationFormText = "来源机台";
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 34);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(154, 26);
            this.layoutControlItem3.Text = "来源机台";
            this.layoutControlItem3.TextSize = new System.Drawing.Size(48, 14);
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.cmb_accept;
            this.layoutControlItem4.CustomizationFormText = "接收机台";
            this.layoutControlItem4.Location = new System.Drawing.Point(0, 60);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(154, 26);
            this.layoutControlItem4.Text = "接收机台";
            this.layoutControlItem4.TextSize = new System.Drawing.Size(48, 14);
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.cmb_dest;
            this.layoutControlItem5.CustomizationFormText = "接收去向";
            this.layoutControlItem5.Location = new System.Drawing.Point(0, 86);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Size = new System.Drawing.Size(254, 24);
            this.layoutControlItem5.Text = "接收去向";
            this.layoutControlItem5.TextSize = new System.Drawing.Size(48, 14);
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this.cmb_operate;
            this.layoutControlItem6.CustomizationFormText = "操作类型";
            this.layoutControlItem6.Location = new System.Drawing.Point(0, 110);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Size = new System.Drawing.Size(254, 24);
            this.layoutControlItem6.Text = "操作类型";
            this.layoutControlItem6.TextSize = new System.Drawing.Size(48, 14);
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.btn_Save;
            this.layoutControlItem1.CustomizationFormText = "layoutControlItem1";
            this.layoutControlItem1.Location = new System.Drawing.Point(154, 199);
            this.layoutControlItem1.MaxSize = new System.Drawing.Size(100, 26);
            this.layoutControlItem1.MinSize = new System.Drawing.Size(100, 26);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(100, 26);
            this.layoutControlItem1.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem1.Text = "layoutControlItem1";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextToControlDistance = 0;
            this.layoutControlItem1.TextVisible = false;
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.CustomizationFormText = "emptySpaceItem1";
            this.emptySpaceItem1.Location = new System.Drawing.Point(254, 0);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(726, 225);
            this.emptySpaceItem1.Text = "emptySpaceItem1";
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // emptySpaceItem3
            // 
            this.emptySpaceItem3.AllowHotTrack = false;
            this.emptySpaceItem3.CustomizationFormText = "emptySpaceItem3";
            this.emptySpaceItem3.Location = new System.Drawing.Point(0, 225);
            this.emptySpaceItem3.Name = "emptySpaceItem3";
            this.emptySpaceItem3.Size = new System.Drawing.Size(980, 337);
            this.emptySpaceItem3.Text = "emptySpaceItem3";
            this.emptySpaceItem3.TextSize = new System.Drawing.Size(0, 0);
            // 
            // emptySpaceItem2
            // 
            this.emptySpaceItem2.AllowHotTrack = false;
            this.emptySpaceItem2.CustomizationFormText = "emptySpaceItem2";
            this.emptySpaceItem2.Location = new System.Drawing.Point(0, 199);
            this.emptySpaceItem2.Name = "emptySpaceItem2";
            this.emptySpaceItem2.Size = new System.Drawing.Size(154, 26);
            this.emptySpaceItem2.Text = "emptySpaceItem2";
            this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem8
            // 
            this.layoutControlItem8.Control = this.btn_LoadFactory;
            this.layoutControlItem8.CustomizationFormText = "layoutControlItem8";
            this.layoutControlItem8.Location = new System.Drawing.Point(154, 34);
            this.layoutControlItem8.MaxSize = new System.Drawing.Size(100, 26);
            this.layoutControlItem8.MinSize = new System.Drawing.Size(100, 26);
            this.layoutControlItem8.Name = "layoutControlItem8";
            this.layoutControlItem8.Size = new System.Drawing.Size(100, 26);
            this.layoutControlItem8.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem8.Text = "layoutControlItem8";
            this.layoutControlItem8.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem8.TextToControlDistance = 0;
            this.layoutControlItem8.TextVisible = false;
            // 
            // layoutControlItem9
            // 
            this.layoutControlItem9.Control = this.txt_result;
            this.layoutControlItem9.CustomizationFormText = "操作结果";
            this.layoutControlItem9.Location = new System.Drawing.Point(0, 134);
            this.layoutControlItem9.Name = "layoutControlItem9";
            this.layoutControlItem9.Size = new System.Drawing.Size(254, 65);
            this.layoutControlItem9.Text = "操作结果";
            this.layoutControlItem9.TextSize = new System.Drawing.Size(48, 14);
            // 
            // spScan
            // 
            this.spScan.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.spScan_DataReceived);
            // 
            // btn_Dest
            // 
            this.btn_Dest.Image = global::CTWH.Properties.Resources.Synchronize_16x16;
            this.btn_Dest.Location = new System.Drawing.Point(166, 72);
            this.btn_Dest.Name = "btn_Dest";
            this.btn_Dest.Size = new System.Drawing.Size(96, 22);
            this.btn_Dest.StyleController = this.layoutControl1;
            this.btn_Dest.TabIndex = 11;
            this.btn_Dest.Text = "刷新去向";
            this.btn_Dest.Click += new System.EventHandler(this.btn_Dest_Click);
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.Control = this.btn_Dest;
            this.layoutControlItem7.CustomizationFormText = "layoutControlItem7";
            this.layoutControlItem7.Location = new System.Drawing.Point(154, 60);
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Size = new System.Drawing.Size(100, 26);
            this.layoutControlItem7.Text = "layoutControlItem7";
            this.layoutControlItem7.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem7.TextToControlDistance = 0;
            this.layoutControlItem7.TextVisible = false;
            // 
            // U_TransferScan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.layoutControl1);
            this.Name = "U_TransferScan";
            this.Size = new System.Drawing.Size(1000, 582);
            this.Load += new System.EventHandler(this.U_TransCommand_Load);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txt_result.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmb_operate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmb_dest.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmb_accept.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmb_Source.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmb_barcode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraEditors.ComboBoxEdit cmb_operate;
        private DevExpress.XtraEditors.ComboBoxEdit cmb_dest;
        private DevExpress.XtraEditors.ComboBoxEdit cmb_accept;
        private DevExpress.XtraEditors.ComboBoxEdit cmb_Source;
        private DevExpress.XtraEditors.ComboBoxEdit cmb_barcode;
        private DevExpress.XtraEditors.SimpleButton btn_Save;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem3;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;
        private DevExpress.XtraEditors.SimpleButton btn_LoadFactory;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem8;
        private DevExpress.XtraEditors.MemoEdit txt_result;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem9;
        private System.IO.Ports.SerialPort spScan;
        private DevExpress.XtraEditors.SimpleButton btn_Dest;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem7;
    }
}
