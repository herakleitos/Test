namespace ScannerTest
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.lstBarcodes = new System.Windows.Forms.ListBox();
            this.txtIPAddr = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.lstInform = new System.Windows.Forms.ListBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.udTriggerType = new System.Windows.Forms.NumericUpDown();
            this.btnClearBarcode = new System.Windows.Forms.Button();
            this.btnClearInform = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.udTriggerType)).BeginInit();
            this.SuspendLayout();
            // 
            // lstBarcodes
            // 
            this.lstBarcodes.FormattingEnabled = true;
            this.lstBarcodes.ItemHeight = 12;
            this.lstBarcodes.Location = new System.Drawing.Point(22, 48);
            this.lstBarcodes.Name = "lstBarcodes";
            this.lstBarcodes.Size = new System.Drawing.Size(418, 112);
            this.lstBarcodes.TabIndex = 0;
            // 
            // txtIPAddr
            // 
            this.txtIPAddr.Location = new System.Drawing.Point(67, 21);
            this.txtIPAddr.Name = "txtIPAddr";
            this.txtIPAddr.Size = new System.Drawing.Size(100, 21);
            this.txtIPAddr.TabIndex = 1;
            this.txtIPAddr.Text = "192.168.106.220";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "IPAddr";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(186, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "Port";
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(227, 21);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(41, 21);
            this.txtPort.TabIndex = 1;
            this.txtPort.Text = "23";
            // 
            // lstInform
            // 
            this.lstInform.FormattingEnabled = true;
            this.lstInform.ItemHeight = 12;
            this.lstInform.Location = new System.Drawing.Point(22, 168);
            this.lstInform.Name = "lstInform";
            this.lstInform.Size = new System.Drawing.Size(580, 148);
            this.lstInform.TabIndex = 0;
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(282, 19);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 3;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(363, 19);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(457, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "触发方式(0-5)";
            // 
            // udTriggerType
            // 
            this.udTriggerType.Location = new System.Drawing.Point(459, 73);
            this.udTriggerType.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.udTriggerType.Name = "udTriggerType";
            this.udTriggerType.Size = new System.Drawing.Size(81, 21);
            this.udTriggerType.TabIndex = 4;
            this.udTriggerType.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.udTriggerType.ValueChanged += new System.EventHandler(this.udTriggerType_ValueChanged);
            // 
            // btnClearBarcode
            // 
            this.btnClearBarcode.Location = new System.Drawing.Point(459, 100);
            this.btnClearBarcode.Name = "btnClearBarcode";
            this.btnClearBarcode.Size = new System.Drawing.Size(101, 25);
            this.btnClearBarcode.TabIndex = 3;
            this.btnClearBarcode.Text = "ClearBarcode";
            this.btnClearBarcode.UseVisualStyleBackColor = true;
            this.btnClearBarcode.Click += new System.EventHandler(this.btnClearBarcode_Click);
            // 
            // btnClearInform
            // 
            this.btnClearInform.Location = new System.Drawing.Point(459, 131);
            this.btnClearInform.Name = "btnClearInform";
            this.btnClearInform.Size = new System.Drawing.Size(101, 25);
            this.btnClearInform.TabIndex = 3;
            this.btnClearInform.Text = "ClearInform";
            this.btnClearInform.UseVisualStyleBackColor = true;
            this.btnClearInform.Click += new System.EventHandler(this.btnClearInform_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(614, 320);
            this.Controls.Add(this.udTriggerType);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnClearInform);
            this.Controls.Add(this.btnClearBarcode);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.txtIPAddr);
            this.Controls.Add(this.lstInform);
            this.Controls.Add(this.lstBarcodes);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "扫描仪测试";
            ((System.ComponentModel.ISupportInitialize)(this.udTriggerType)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lstBarcodes;
        private System.Windows.Forms.TextBox txtIPAddr;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.ListBox lstInform;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown udTriggerType;
        private System.Windows.Forms.Button btnClearBarcode;
        private System.Windows.Forms.Button btnClearInform;
    }
}

