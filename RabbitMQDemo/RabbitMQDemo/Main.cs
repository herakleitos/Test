using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RabbitMQDemo
{
    public partial class Main : Form
    {
        MQService mqService;
        public Main()
        {
            InitializeComponent();
            mqService = new MQService();
            string MQHostName = "222.240.44.43";
            int MQPort = 20202;
            string MQUserName = "test";
            string MQPassword = "test@2019";
            string MQExchange = "GCJS";
            mqService.InitMQService(MQHostName, MQPort, MQUserName, MQPassword, MQExchange);


            this.tb_1.Text = "20190002XTGC";
            this.tb_2.Text = "0020000620190042";
            this.tb_3.Text = "0020012920190028";

            this.rb_7.Select();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FlowMessageBody body = new FlowMessageBody
            {
                ActionTime = DateTime.Now,
                KeyDigNumGather = this.tb_3.Text,
                ParallelKeyDigNumGather = this.tb_2.Text,
                ProjectCode = this.tb_1.Text,
            };
            int category = 7;
            category = this.rb_7.Checked ? 7 : 11;
            mqService.SendMessage(body, "GCJS_N_002", category);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FlowMessageBody body = new FlowMessageBody
            {
                ActionTime = DateTime.Now,
                KeyDigNumGather = this.tb_3.Text,
                ParallelKeyDigNumGather = this.tb_2.Text,
                ProjectCode = this.tb_1.Text,
            };
            int category = 7;
            category = this.rb_7.Checked ? 7 : 11;
            mqService.SendMessage(body, "GZJS_Q_Flow", category);
        }
    }
}
