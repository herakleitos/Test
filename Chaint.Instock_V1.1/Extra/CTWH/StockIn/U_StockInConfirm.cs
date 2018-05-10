using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CTWH.Common;
using DataModel;
namespace CTWH.StockIn
{
    public partial class U_StockInConfirm : UserControl
    {
        private MainLayout mainLayout;
        CTWH.Common.MSSQL.WMSAccess _WMSAccess;
        WH_PDA_Service.PDAForm pdaform = new WH_PDA_Service.PDAForm(false);
        public U_StockInConfirm()
        {
            InitializeComponent();
        }
        private void DisposeData()
        {
            _WMSAccess.SqlStateChange -= new CTWH.Common.MSSQL.WMSAccess.SqlStateEventHandler(access_SqlStateChange);
            _WMSAccess = null;
        }
        void access_SqlStateChange(object sender, SqlStateEventArgs e)
        {
            if (e.IsConnect == false)
            {
                Utils.WriteTxtLog(Utils.FilePath_txtMSSQLLog, "DataBase Error:" + e.Info);
            }
        }
        public U_StockInConfirm(MainLayout mainLayout)
        {
            InitializeComponent();
            this.mainLayout = mainLayout;
            this.Disposed += new EventHandler(U_StockInConfirm_Disposed);
        }
        void U_StockInConfirm_Disposed(object sender, EventArgs e)
        {
            this.CloseSerialPort(this.spScan);
        }
        private void U_StockInConfirm_Load(object sender, EventArgs e)
        {
            OpenSerialPort(this.spScan, Utils.SerialParaScan1.PortName);
        }
        #region 串口处理
        private StringBuilder sbBarCode = new StringBuilder();
        private delegate void deSetText(string text, string color);
        private bool OpenSerialPort(System.IO.Ports.SerialPort serialport, string PortName)
        {
            serialport.PortName = PortName;
            try
            {
                if (!serialport.IsOpen)
                    serialport.Open();
                return true;
            }
            catch
            {
                MessageBox.Show("打开串口" + serialport.PortName + "错误");
                return false;
            }
        }
        private void spScan_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {

        }
        private void SetText(string text, string color)
        {

            if (this.rtb_BarCode.InvokeRequired)
            {
                deSetText d = new deSetText(SetText);
                this.Invoke(d, new object[] { text, "" });
            }
            else
            {
                this.rtb_BarCode.Text = text;
                this.BarCodeChanged();
            }
        }
        private bool CloseSerialPort(System.IO.Ports.SerialPort serialport)
        {
            try
            {
                serialport.Close();
                return true;
            }
            catch
            {
                MessageBox.Show("关闭失败！");
                return false;
            }

        }
        #endregion
        public void StockInConfirm_Load(object sender, EventArgs e)
        {
            _WMSAccess = Utils.WMSSqlAccess;
            _WMSAccess.SqlStateChange += new CTWH.Common.MSSQL.WMSAccess.SqlStateEventHandler(access_SqlStateChange);
        }
        private void StockInConfirm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                BarCodeChanged();
            }
        }
        private void BarCodeChanged()
        {
            string barCode = this.rtb_BarCode.Text.Trim();
            if (barCode.Length <= 0)
            {
                MessageBox.Show("请输入条形码!");
                return;
            }
            string[] code = new string[] { "Q20", barCode };
            string msg = Utils.WMSMessage.MakeWMSSocketMsg(code);
            string[] strs = msg.TrimStart(Utils.WMSMessage._StartChar).TrimEnd(Utils.WMSMessage._EndChar).Split(Utils.WMSMessage._SpliteChar);
            string result = pdaform.Process_Q20(strs);
            string[] strResult
                = result.TrimStart(Utils.WMSMessage._StartChar).TrimEnd(Utils.WMSMessage._EndChar).Split(Utils.WMSMessage._SpliteChar);
            if (strResult.Length > 5)
            {
                string message = strResult[3];
                string alertmsg = strResult[4];
                if (strResult[2] == "9")
                {
                    MessageBox.Show(alertmsg);
                }
                if (message.Length > 0)
                {
                    string showMsg = message.Replace("\n", "");
                    this.lb_Message.Text = showMsg;
                }
            }
        }
    }
}
