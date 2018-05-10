using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using CTWH.Common;

namespace CTWH.StockManage
{
    public partial class U_TransferScan : DevExpress.XtraEditors.XtraUserControl
    {
        WH_PDA_Service.PDAForm pdaform = new WH_PDA_Service.PDAForm(false);

        public U_TransferScan()
        {
            InitializeComponent();
            this.Disposed += new EventHandler(U_TransCommand_Disposed);

        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            string barcode = this.cmb_barcode.Text.Trim();
            if (barcode == "")
            {
                MessageBox.Show("请输入条形码");
                return;
            }
            if (barcode.Length!= 16)
            {
                MessageBox.Show("条形码位数不正确");
                return;
            }
            string source = this.cmb_Source.Text.Trim(); 
            if (source == "")
            {
                MessageBox.Show("请输入来源机台");
                return;
            }
            string accept = this.cmb_accept.Text.Trim();
            if (accept == "")
            {
                MessageBox.Show("请输入接收机台");
                return;
            }
            string operate = this.cmb_operate.Text.Trim();
            if (operate == "")
            {
                MessageBox.Show("请输入操作方式");
                return;
            }
            string dest = this.cmb_dest.Text.Trim().Split('.')[0];
            if (dest == "")
            {
                MessageBox.Show("请输入接收去向");
                return;
            }
            if (source == accept)
            {
                MessageBox.Show("来源机台和接收机台不能相同");
                return;
            }

            //开始保存
            string[] ir03 = new string[] { "Q18",barcode,source,accept,dest,operate,Utils.SocketParaPDA.IPAddresss[0], DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") };
            string msg = Utils.WMSMessage.MakeWMSSocketMsg(ir03);
            string[] strs = msg.TrimStart(Utils.WMSMessage._StartChar).TrimEnd(Utils.WMSMessage._EndChar).Split(Utils.WMSMessage._SpliteChar);

            string result = pdaform.Process_Q18(strs);

            string[] retstrs = result.TrimStart(Utils.WMSMessage._StartChar).TrimEnd(Utils.WMSMessage._EndChar).Split(Utils.WMSMessage._SpliteChar);
            if (retstrs[2] == "0")
                this.txt_result.ForeColor = Color.Blue;
            else
                this.txt_result.ForeColor = Color.Red;
            this.txt_result.Text = retstrs[3];
        }

        private void U_TransCommand_Load(object sender, EventArgs e)
        {
            OpenSerialPort(this.spScan, Utils.SerialParaScan1.PortName);

        }
        void U_TransCommand_Disposed(object sender, EventArgs e)
        {
            this.pdaform.DisposeData();
            this.CloseSerialPort(this.spScan);
        }
        private void btn_LoadFactory_Click(object sender, EventArgs e)
        {
            string[] ir03 = new string[] { "Q16",Utils.SocketParaPDA.IPAddresss[0], DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") };
            string msg = Utils.WMSMessage.MakeWMSSocketMsg(ir03);
            string[] strs = msg.TrimStart(Utils.WMSMessage._StartChar).TrimEnd(Utils.WMSMessage._EndChar).Split(Utils.WMSMessage._SpliteChar);
                      
           string result = pdaform.Process_Q16(strs);
          
            string[] retstrs = result.TrimStart(Utils.WMSMessage._StartChar).TrimEnd(Utils.WMSMessage._EndChar).Split(Utils.WMSMessage._SpliteChar);
            if (retstrs[2] == "0")
            {
                this.cmb_Source.Properties.Items.Clear();
                this.cmb_accept.Properties.Items.Clear();

                string[] users = retstrs[4].Split(Utils.WMSMessage._ForeachChar);
                foreach (string user in users)
                {
                    this.cmb_Source.Properties.Items.Add(user);
                    this.cmb_accept.Properties.Items.Add(user);

                }
                this.txt_result.ForeColor = Color.Blue;
            }
            else
                this.txt_result.ForeColor = Color.Red;
           this.txt_result.Text = retstrs[3];

          
        }

        private void cmb_accept_TextChanged(object sender, EventArgs e)
        {
            
        }
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
        private StringBuilder sbBarCode = new StringBuilder();
        private delegate void deSetText(string text, string color);

        private void SetText(string text, string color)
        {

            if (this.cmb_barcode.InvokeRequired)
            {
                deSetText d = new deSetText(SetText);
                this.Invoke(d, new object[] { text, "" });
            }
            else
            {
                this.cmb_barcode.Text = "";
                this.cmb_barcode.Text = text;
                this.btn_Save_Click(null, null);
            }
        }
        private void spScan_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {

            int Length = this.spScan.BytesToRead;
            byte[] receive = new byte[Length];
            this.spScan.Read(receive, 0, Length);



            foreach (byte b in receive)
            {
                if (b <= '9' && b >= '0' || b <= 'z' && b >= 'a' || b <= 'Z' && b >= 'A')  //Start 2    stop 3  Get From Config
                    sbBarCode.Append((char)b);


                if (b == 13) //表示字符串接受结束
                    if (sbBarCode.Length >= 10)  //10 11
                    {
                        //SetText(sbBarCode.ToString().Substring(0, 8));

                        //测试  
                        SetText(sbBarCode.ToString(), "");
                        // txtBarcode_ButtonClick(this.txtBarcode, new DevExpress.XtraEditors.Controls.ButtonPressedEventArgs(this.txtBarcode.Properties.Buttons[0]));

                        sbBarCode = new StringBuilder();
                    }

                if (b == 13)
                    sbBarCode = new StringBuilder();

                //char 13 结束      char 10 开始


            }
        }

        private void btn_Dest_Click(object sender, EventArgs e)
        {
            if (cmb_accept.Text.Trim() != "")
            {
                string[] ir03 = new string[] { "Q17", cmb_accept.Text.Trim(), Utils.SocketParaPDA.IPAddresss[0], DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") };
                string msg = Utils.WMSMessage.MakeWMSSocketMsg(ir03);
                string[] strs = msg.TrimStart(Utils.WMSMessage._StartChar).TrimEnd(Utils.WMSMessage._EndChar).Split(Utils.WMSMessage._SpliteChar);
                string result = pdaform.Process_Q17(strs);
                string[] retstrs = result.TrimStart(Utils.WMSMessage._StartChar).TrimEnd(Utils.WMSMessage._EndChar).Split(Utils.WMSMessage._SpliteChar);
                if (retstrs[2] == "0")
                {
                    this.cmb_dest.Properties.Items.Clear();
                    string[] users = retstrs[4].Split(Utils.WMSMessage._ForeachChar);
                    foreach (string user in users)
                    {
                        this.cmb_dest.Properties.Items.Add(user);
                    }
                    this.txt_result.ForeColor = Color.Blue;
                }
                else
                    this.txt_result.ForeColor = Color.Red;
                this.txt_result.Text = retstrs[3];
            }
            else {
                MessageBox.Show("请先选择接收机台");
            }
        }
    }
}
