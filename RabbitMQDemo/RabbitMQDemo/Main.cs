using Newtonsoft.Json;
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
            string MQExchange = " ";


            this.label5.Text = string.Format("{0}:{1}:{2}:{3}:{4}", MQHostName, MQPort, MQUserName, MQPassword, MQExchange);
            mqService.InitMQService(MQHostName, MQPort, MQUserName, MQPassword, MQExchange);


            this.tb_1.Text = "20190002XTGC";
            this.tb_2.Text = "0020000620190042";
            this.tb_3.Text = "0020012920190028";

            this.rb_7.Select();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
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
                // mqService.SendMessage(body, "my_test", category);



                mqService.SendMessage(body, "GCJS_N_001", category);
                mqService.SendMessage(body, "GCJS_N_003", category);
                mqService.SendMessage(body, "GCJS_N_004", category);
                mqService.SendMessage(body, "GCJS_N_005", category);
                mqService.SendMessage(body, "GZJS_Q_Flow", category);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DateTime t1 = new DateTime(2019, 6, 15, 22, 30, 45);
            DateTime t2 = new DateTime(2019, 6, 16, 00, 10, 45);

            int day1 = t1.Minute;

            int day2 = t2.Minute;

            double day = (t2 - t1).TotalDays;
            string aaa = "000";
            //try
            //{
            //    SuspendStartVM suspendStartVM = new SuspendStartVM();
            //    suspendStartVM.suspendDays = 10;
            //    var aa = new List<SuspendStartVM>() { suspendStartVM }; ;

            //    string json = JsonConvert.SerializeObject(aa);

            //    FlowMessageBody body = new FlowMessageBody
            //    {
            //        ActionTime = DateTime.Now,
            //        KeyDigNumGather = this.tb_3.Text,
            //        ParallelKeyDigNumGather = this.tb_2.Text,
            //        ProjectCode = this.tb_1.Text,
            //    };
            //    int category = 7;
            //    category = this.rb_7.Checked ? 1 : 11;
            //    category = 24;
            //    mqService.SendMessage(body, "GZJS_Q_Flow", category);

            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }
    }

    /// <summary>
    /// 挂起开始
    /// </summary>
    public class SuspendStartVM
    {
        /// <summary>
        /// 挂起天数
        /// </summary>
        public int suspendDays { get; set; }
    }
}
