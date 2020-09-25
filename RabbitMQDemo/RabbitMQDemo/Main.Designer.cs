namespace RabbitMQDemo
{
    partial class Main
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
            this.tb_1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tb_3 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.rb_7 = new System.Windows.Forms.RadioButton();
            this.rb_11 = new System.Windows.Forms.RadioButton();
            this.label4 = new System.Windows.Forms.Label();
            this.tb_2 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tb_1
            // 
            this.tb_1.Location = new System.Drawing.Point(95, 38);
            this.tb_1.Name = "tb_1";
            this.tb_1.Size = new System.Drawing.Size(316, 21);
            this.tb_1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(281, 231);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(94, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "发送转发消息";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(49, 231);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(104, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "发送流程消息";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "项目代码";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 124);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "事项实例编码";
            // 
            // tb_3
            // 
            this.tb_3.Location = new System.Drawing.Point(95, 115);
            this.tb_3.Name = "tb_3";
            this.tb_3.Size = new System.Drawing.Size(316, 21);
            this.tb_3.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 174);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "消息类型";
            // 
            // rb_7
            // 
            this.rb_7.AutoSize = true;
            this.rb_7.Location = new System.Drawing.Point(95, 172);
            this.rb_7.Name = "rb_7";
            this.rb_7.Size = new System.Drawing.Size(29, 16);
            this.rb_7.TabIndex = 8;
            this.rb_7.TabStop = true;
            this.rb_7.Text = "7";
            this.rb_7.UseVisualStyleBackColor = true;
            // 
            // rb_11
            // 
            this.rb_11.AutoSize = true;
            this.rb_11.Location = new System.Drawing.Point(161, 170);
            this.rb_11.Name = "rb_11";
            this.rb_11.Size = new System.Drawing.Size(35, 16);
            this.rb_11.TabIndex = 9;
            this.rb_11.TabStop = true;
            this.rb_11.Text = "11";
            this.rb_11.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 83);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 12);
            this.label4.TabIndex = 11;
            this.label4.Text = "并联实例编码";
            // 
            // tb_2
            // 
            this.tb_2.Location = new System.Drawing.Point(95, 74);
            this.tb_2.Name = "tb_2";
            this.tb_2.Size = new System.Drawing.Size(316, 21);
            this.tb_2.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(67, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 12;
            this.label5.Text = "label5";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(422, 279);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tb_2);
            this.Controls.Add(this.rb_11);
            this.Controls.Add(this.rb_7);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tb_3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tb_1);
            this.Name = "Main";
            this.Text = "RabbitMQ Test";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tb_1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb_3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton rb_7;
        private System.Windows.Forms.RadioButton rb_11;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tb_2;
        private System.Windows.Forms.Label label5;
    }
}

