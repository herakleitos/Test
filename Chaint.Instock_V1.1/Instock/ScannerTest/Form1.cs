using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Chaint.Common.Devices.Devices;


namespace ScannerTest
{
    public partial class Form1 : Form
    {
        private const string OpenCmd = "TRIGGER ON";
        private const string CloseCmd = "TRIGGER OFF";

        private IList<string> m_lstBarcodes = new List<string>();

        private Scanner m_Scanner = null;
      

        public IList<string> ScanBarcodes
        {
            get { return m_lstBarcodes; }
            set { m_lstBarcodes = value; }
        }

        public string ScannerIPAddr
        {
            set
            {
                this.txtIPAddr.Text = value;
            }
            get
            {
                return this.txtIPAddr.Text;
            }
        }

        public int Port
        {
            get { return Convert.ToInt32(this.txtPort.Text); }
        }


        public Form1()
        {
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if(m_Scanner==null)
            {
                Param_Base scannParam = new Param_Ethernet(Param_Ethernet.CommMode.CLISERVICE, this.ScannerIPAddr, this.Port);
                m_Scanner = ScanFactory.CreateDevice(ScanType.Cognex, scannParam);
                m_Scanner.OnBarcodeValue += M_Scanner_OnBarcodeValue;
                m_Scanner.OnRunMessage += M_Scanner_OnRunMessage;
                m_Scanner.Connect();
                SetScanParams(4);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            CloseScanner();
        }


        private void CloseScanner()
        {
            if (m_Scanner != null)
            {
                m_Scanner.Disconnect();
                m_Scanner.OnBarcodeValue -= new ReadStringArrivedHandler(M_Scanner_OnBarcodeValue);
                m_Scanner.OnRunMessage -= new RunMessageEventHandler(M_Scanner_OnRunMessage);
                m_Scanner = null;
            }
        }


        /// <summary>
        /// 扫描反馈的消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="strMsg"></param>
        private void M_Scanner_OnRunMessage(object sender, string strMsg)
        {
            SendMessage(strMsg);
        }


        private void SendMessage(string strMsg)
        {
            this.Invoke(new MethodInvoker(delegate {
                lstInform.Items.Add(string.Format("{0} {1}", DateTime.Now.ToString("HH:mm:ss"), strMsg));
                lstInform.SelectedIndex = lstInform.Items.Count - 1;
            }));
        }

        /// <summary>
        /// 扫描的条码
        /// </summary>
        /// <param name="strReadValue">条码</param>
        private void M_Scanner_OnBarcodeValue(string strReadValue)
        {
            this.Invoke(new MethodInvoker(delegate {
                lstBarcodes.Items.Add(string.Format("{0} {1}", DateTime.Now.ToString("HH:mm:ss"), strReadValue));

                lstBarcodes.SelectedIndex = lstBarcodes.Items.Count - 1;
            }));
        }

        private void udTriggerType_ValueChanged(object sender, EventArgs e)
        {
            SendMessage(string.Format("触发打开方式: {0} ", udTriggerType.Value));
            SetScanParams(udTriggerType.Value);
        }


        private void SetScanParams(decimal decTriggerType)
        {
            if (m_Scanner != null)
            {
                m_Scanner.Write("SET DECODER.REREAD-NEVER2X OFF");//不读取重复条码
                //m_Scanner.Write("SET TRIGGER.TYPE 5");           //连续外部触发
                m_Scanner.Write(string.Format("SET TRIGGER.TYPE {0}",decTriggerType));           //内部触发

                m_Scanner.Write(OpenCmd);
            }
        }

        private void btnClearBarcode_Click(object sender, EventArgs e)
        {
            this.lstBarcodes.Items.Clear();
        }

        private void btnClearInform_Click(object sender, EventArgs e)
        {
            this.lstInform.Items.Clear();
        }
    }
}
