using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
namespace DialogflowFrm
{
    public partial class Frm : Form
    {
        private string jsonPath = "..\\..\\json\\My Project-2c0ddc26d37d.json";
        private string projectId = "virtue-d2a48";
        private delegate void deleSetText(Control source,Control control,string text, string color);
        public Frm()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string languageCode = this.comboBox2.SelectedItem?.ToString();
            if (string.IsNullOrWhiteSpace(languageCode))
            {
                MessageBox.Show("请选择语言");
                return;
            }
            this.button1.Enabled = false;
            this.button1.Text = "Processing...";
            string sessionId = Guid.NewGuid().ToString();
            string question = this.tbQuestion.Text;
            var cts = new CancellationTokenSource();
            Task.Factory.StartNew(() =>{

                string result = DetectIntentTexts.DetectIntentFromTexts(projectId, sessionId, jsonPath, question, languageCode);
                deleSetText setText = new deleSetText(SetText);
                this.Invoke(setText, new object[] {this.button1,this.rtbMessage, "DetectIntentTexts", result });
            },cts.Token);
            cts.CancelAfter(50000);
        }
        private void SetText(Control source,Control control,string oldText,string result)
        {
            string test = source.Text;
            if (control.InvokeRequired)
            {
                deleSetText setText = new deleSetText(SetText);
                this.Invoke(setText, new object[] { source, control, oldText, result });
            }
            else
            {
                control.Text = result;
            }
            source.Enabled = true;
            source.Text = oldText;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string languageCode = this.comboBox1.SelectedItem?.ToString();
            if (string.IsNullOrWhiteSpace(languageCode))
            {
                MessageBox.Show("请选择语言");
                return;
            }
            this.button2.Enabled = false;
            this.button2.Text = "Processing...";
            var cts = new CancellationTokenSource();
            Task.Factory.StartNew(() => {

                string result = Intents.GetList(projectId, jsonPath, languageCode);
                deleSetText setText = new deleSetText(SetText);
                this.Invoke(setText, new object[] {this.button2, this.rtbIntentResult, "Getlist", result });
            },cts.Token);
            cts.CancelAfter(50000);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string languageCode = this.comboBox1.SelectedItem?.ToString();
            if (string.IsNullOrWhiteSpace(languageCode))
            {
                MessageBox.Show("请选择语言");
                return;
            }
            this.button3.Enabled = false;
            this.button3.Text = "Processing...";
            var cts = new CancellationTokenSource();
            Task.Factory.StartNew(() => {

                string result = Intents.Create(projectId, jsonPath, languageCode);
                deleSetText setText = new deleSetText(SetText);
                this.Invoke(setText, new object[] { this.button3, this.rtbIntentResult, "Create", result });
            },cts.Token);
            cts.CancelAfter(50000);
        }

        private void btEntityGetList_Click(object sender, EventArgs e)
        {
            string languageCode = this.comboBox3.SelectedItem?.ToString();
            if (string.IsNullOrWhiteSpace(languageCode))
            {
                MessageBox.Show("请选择语言");
                return;
            }
            this.btEntityGetList.Enabled = false;
            this.btEntityGetList.Text = "Processing...";
            var cts = new CancellationTokenSource();
            Task.Factory.StartNew(() => {

                string result = EntityTypes.GetList(projectId, jsonPath, languageCode);
                deleSetText setText = new deleSetText(SetText);
                this.Invoke(setText, new object[] { this.btEntityGetList, this.rtbEntityResult, "Getlist", result });
            }, cts.Token);
            cts.CancelAfter(50000);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string languageCode = this.comboBox3.SelectedItem?.ToString();
            if (string.IsNullOrWhiteSpace(languageCode))
            {
                MessageBox.Show("请选择语言");
                return;
            }
            this.button5.Enabled = false;
            this.button5.Text = "Processing...";
            var cts = new CancellationTokenSource();
            Task.Factory.StartNew(() => {

                string result = EntityTypes.Create(projectId, jsonPath, languageCode);
                deleSetText setText = new deleSetText(SetText);
                this.Invoke(setText, new object[] { this.button5, this.rtbIntentResult, "Create", result });
            }, cts.Token);
            cts.CancelAfter(50000);
        }
    }
}
