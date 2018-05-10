namespace CTWH.StockIn
{
    partial class U_StockInConfirm
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.rtb_BarCode = new System.Windows.Forms.RichTextBox();
            this.lb_BarCode = new System.Windows.Forms.Label();
            this.spScan = new System.IO.Ports.SerialPort();
            this.lb_Message = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // rtb_BarCode
            // 
            this.rtb_BarCode.Font = new System.Drawing.Font("宋体", 42F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rtb_BarCode.Location = new System.Drawing.Point(100, 62);
            this.rtb_BarCode.Multiline = false;
            this.rtb_BarCode.Name = "rtb_BarCode";
            this.rtb_BarCode.Size = new System.Drawing.Size(1100, 120);
            this.rtb_BarCode.TabIndex = 1;
            this.rtb_BarCode.Text = "";
            this.rtb_BarCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.StockInConfirm_KeyPress);
            // 
            // lb_BarCode
            // 
            this.lb_BarCode.AutoSize = true;
            this.lb_BarCode.Font = new System.Drawing.Font("宋体", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lb_BarCode.Location = new System.Drawing.Point(4, 106);
            this.lb_BarCode.Name = "lb_BarCode";
            this.lb_BarCode.Size = new System.Drawing.Size(90, 26);
            this.lb_BarCode.TabIndex = 2;
            this.lb_BarCode.Text = "条形码";
            // 
            // lb_Message
            // 
            this.lb_Message.Font = new System.Drawing.Font("宋体", 19F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lb_Message.Location = new System.Drawing.Point(100, 299);
            this.lb_Message.Name = "lb_Message";
            this.lb_Message.Size = new System.Drawing.Size(1100, 153);
            this.lb_Message.TabIndex = 3;
            this.lb_Message.Text = "处理结果";
            // 
            // U_StockInConfirm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.Controls.Add(this.lb_Message);
            this.Controls.Add(this.lb_BarCode);
            this.Controls.Add(this.rtb_BarCode);
            this.Name = "U_StockInConfirm";
            this.Size = new System.Drawing.Size(1223, 616);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtb_BarCode;
        private System.Windows.Forms.Label lb_BarCode;
        private System.IO.Ports.SerialPort spScan;
        private System.Windows.Forms.RichTextBox lb_Message;
    }
}
