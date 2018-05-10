using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using CTWH.Common;

namespace CTWH.StockOut
{
    public partial class U_StockOutScan : DevExpress.XtraEditors.XtraUserControl
    {
        public U_StockOutScan()
        {
            InitializeComponent();
            this.Disposed += new EventHandler(U_StockInScan_Disposed);

        }
        void U_StockInScan_Disposed(object sender, EventArgs e)
        {
            this.pdaform.DisposeData();
            this.ClosSerialPort(this.spScan);


        }
        private void cmb_Barcode_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            string voucherno = this.cmb_Voucherno.Text;
            string barcode = this.cmb_Barcode.Text;

            if (voucherno == "")//单据为空,需输入单据号
            {
                MessageBox.Show("请输入该取消出库纸卷的单号!");
                return;
            }

            if (this.chk_Cancel.Checked)//取消出库
            {
                string[] o01 = new string[] { "O08", barcode, voucherno, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") };
                string msg = Utils.WMSMessage.MakeWMSSocketMsg(o01);
                string[] strs = msg.TrimStart(Utils.WMSMessage._StartChar).TrimEnd(Utils.WMSMessage._EndChar).Split(Utils.WMSMessage._SpliteChar);

                string result = pdaform.Process_O08(strs);
                //this.txtMemo.Text = result;
                this.ShowInformation(result);

            }
            else {
                string[] o01 = new string[] { "O01", barcode, voucherno, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") };
                string msg = Utils.WMSMessage.MakeWMSSocketMsg(o01);

                string[] strs = msg.TrimStart(Utils.WMSMessage._StartChar).TrimEnd(Utils.WMSMessage._EndChar).Split(Utils.WMSMessage._SpliteChar);

                string result = pdaform.Process_O01(strs,false);
                //this.txtMemo.Text = result;
                this.ShowInformation(result);

                  string [] strs2=result.Split(Utils.WMSMessage._SpliteChar);
                  this.txtResult.Text = strs2[3];

                  if (strs2[2] == "0") {
                  //出库成功
                  } else if (strs2[2] == "1") {
                      //超重量和超件

                      if (DialogResult.Yes == MessageBox.Show(strs2[3], "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                      {
                          string[] ao01 = new string[] { "O02", barcode, voucherno, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") };
                          string amsg = Utils.WMSMessage.MakeWMSSocketMsg(ao01);

                          string[] astrs = amsg.TrimStart(Utils.WMSMessage._StartChar).TrimEnd(Utils.WMSMessage._EndChar).Split(Utils.WMSMessage._SpliteChar);

                          string aresult = pdaform.Process_O02(astrs);
                          //this.txtMemo.Text = aresult;
                          this.ShowInformation(result);

                          string[] astrs2 = aresult.Split(Utils.WMSMessage._SpliteChar);
                          this.txtResult.Text = astrs2[3];
                      }
                      else { 
                      
                      }
                  } else if (strs2[2] == "2") {

                      //超重量
                      if (DialogResult.Yes == MessageBox.Show(strs2[3], "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                      {
                          string[] ao01 = new string[] { "O03", barcode, voucherno, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") };
                          string amsg = Utils.WMSMessage.MakeWMSSocketMsg(ao01);

                          string[] astrs = amsg.TrimStart(Utils.WMSMessage._StartChar).TrimEnd(Utils.WMSMessage._EndChar).Split(Utils.WMSMessage._SpliteChar);

                          string aresult = pdaform.Process_O03(astrs);
                          //this.txtMemo.Text = aresult;
                          this.ShowInformation(result);

                          string[] astrs2 = aresult.Split(Utils.WMSMessage._SpliteChar);
                          this.txtResult.Text = astrs2[3];
                      }
                      else
                      {

                      }
                  }
                  else if (strs2[2] == "3")
                  {

                      //超件数
                      if (DialogResult.Yes == MessageBox.Show(strs2[3], "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                      {
                          string[] ao01 = new string[] { "O04", barcode, voucherno, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") };
                          string amsg = Utils.WMSMessage.MakeWMSSocketMsg(ao01);

                          string[] astrs = amsg.TrimStart(Utils.WMSMessage._StartChar).TrimEnd(Utils.WMSMessage._EndChar).Split(Utils.WMSMessage._SpliteChar);

                          string aresult = pdaform.Process_O04(astrs);
                          //this.txtMemo.Text = aresult;
                          this.ShowInformation(result);

                          string[] astrs2 = aresult.Split(Utils.WMSMessage._SpliteChar);
                          this.txtResult.Text = astrs2[3];
                      }
                      else
                      {

                      }
                  }
                  else if (strs2[2] == "8")
                  {

                      //什么都没匹配上
                  }
                  else if (strs2[2] == "9")
                  {
                      //出错

                  }
            }
           

        }

        private void btn_refreash_Click(object sender, EventArgs e)
        {
         string voucherno=   this.cmb_Voucherno.Text.Trim();
            string[] ao01 = new string[] { "O15",  voucherno, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") };
            string amsg = Utils.WMSMessage.MakeWMSSocketMsg(ao01);

            string[] astrs = amsg.TrimStart(Utils.WMSMessage._StartChar).TrimEnd(Utils.WMSMessage._EndChar).Split(Utils.WMSMessage._SpliteChar);

            string aresult = pdaform.Process_O15(astrs);
            //this.txtMemo.Text = aresult;

            string[] astrs2 = aresult.TrimStart(Utils.WMSMessage._StartChar).TrimEnd(Utils.WMSMessage._EndChar).Split(Utils.WMSMessage._SpliteChar);
            this.txtResult.Text = astrs2[3];
            //
            DataTable statDT = new DataTable();
            statDT.Columns.Add(new DataColumn("分录号"));

            statDT.Columns.Add(new DataColumn("物料名称"));
            statDT.Columns.Add(new DataColumn("规格"));
            statDT.Columns.Add(new DataColumn("等级"));
            statDT.Columns.Add(new DataColumn("模式"));
            statDT.Columns.Add(new DataColumn("纸芯/令数"));
            //statDT.Columns.Add(new DataColumn("令数"));

            statDT.Columns.Add(new DataColumn("直径/令张"));
            statDT.Columns.Add(new DataColumn("线长/件张"));
            statDT.Columns.Add(new DataColumn("包装"));
            statDT.Columns.Add(new DataColumn("SKU"));

            statDT.Columns.Add(new DataColumn("认证"));
            statDT.Columns.Add(new DataColumn("产专"));
            statDT.Columns.Add(new DataColumn("客专"));

            statDT.Columns.Add(new DataColumn("夹板"));
            

            statDT.Columns.Add(new DataColumn("商标"));
            statDT.Columns.Add(new DataColumn("订单"));
            statDT.Columns.Add(new DataColumn("备注"));
          //  statDT.Columns.Add(new DataColumn("批次号"));
            statDT.Columns.Add(new DataColumn("计划件数"));
            statDT.Columns.Add(new DataColumn("计划重量"));
            statDT.Columns.Add(new DataColumn("实际件数"));
            statDT.Columns.Add(new DataColumn("实际重量"));
            if (astrs2.Length > 4)
            {
                string[] rows = astrs2[6].Split(Utils.WMSMessage._ForeachChar);
                try
                {
                    foreach (string row in rows)
                    {
                        statDT.Rows.Add(row.Split(Utils.WMSMessage._ColumnChar));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
            this.gridControl1.DataSource = statDT.DefaultView;
            this.gridView1.BestFitColumns();
            //
        }
        WH_PDA_Service.PDAForm pdaform = null;
        private void U_StockOutScan_Load(object sender, EventArgs e)
        {
       pdaform=    new WH_PDA_Service.PDAForm(false);
       OpenSerialPort(this.spScan, Utils.SerialParaScan1.PortName);
       this.dt_Bill.DateTime = DateTime.Now;

        }

        private void btn_Voucher_Click(object sender, EventArgs e)
        {
            string dt = this.dt_Bill.Text.Trim();
           
            string[] ao01 = new string[] { "O10", dt };
            string amsg = Utils.WMSMessage.MakeWMSSocketMsg(ao01);

            string[] astrs = amsg.TrimStart(Utils.WMSMessage._StartChar).TrimEnd(Utils.WMSMessage._EndChar).Split(Utils.WMSMessage._SpliteChar);

            string aresult = pdaform.Process_Q07(dt);
            //this.txtMemo.Text = aresult;

            string[] astrs2 = aresult.TrimStart(Utils.WMSMessage._StartChar).TrimEnd(Utils.WMSMessage._EndChar).Split(Utils.WMSMessage._SpliteChar);
            this.txtResult.Text = astrs2[3];

            if (astrs2[2] == "0")
            {
                this.cmb_Voucherno.Properties.Items.Clear();
                string[] users = astrs2[4].Split(Utils.WMSMessage._ForeachChar);
                foreach (string user in users)
                {
                    this.cmb_Voucherno.Properties.Items.Add(user);
                }
            
            }
        }

        private void ShowInformation(string result)
        {
            string[] strs = result.TrimStart(Utils.WMSMessage._StartChar).TrimEnd(Utils.WMSMessage._EndChar).Split(Utils.WMSMessage._SpliteChar);

            if (strs.Length > 4)
            {

                this.txt_Material.Text = strs[6];
                this.txt_Grade_Desc.Text = strs[7];
                this.txt_PaperWidth.Text = strs[8];
                this.txt_Diameter.Text = strs[9];
                this.txt_Core.Text = strs[10];
                this.txt_Length.Text = strs[11];
                //this.txt_Direction.Text = strs[12];
                this.txt_Layers.Text = strs[13];
                this.txt_WeightMode.Text = strs[14];
                //this.txt_SlidesReam.Text = strs[15];
                this.txt_Weight.Text = strs[16];
                string txt_Count = strs[17];
                this.txt_PaperCert.Text = strs[18];
                this.txt_Sku.Text = strs[19];
                this.txt_SpecProdName.Text = strs[20];
                this.txt_SpecCustName.Text = strs[21];
                //this.txt_CustTrademark.Text = strs[22];
                this.txt_Contract.Text = strs[23];
                this.txt_Remark_Scan.Text = strs[24];
                this.txt_IsWhiteFlag.Text = strs[25];
                this.txt_Package.Text = strs[26];
                this.txt_borde.Text = strs[27];
                //this.txt_Brand.Text = strs[28];
                //this.txt_TradeMode.Text = strs[29];
                //
                //this.cmb_CustomerName.Text = strs[30];
                //this.cmb_WMSRemark.Text = strs[31];


            }

            if (strs[2] == "0")
                this.txtResult.ForeColor = Color.Blue;
            else
                this.txtResult.ForeColor = Color.Red;
            //this.txtResult.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.txtResult.Text = strs[3];

            //
            //if (strs[2] == "1")
            //{
            //    if (DialogResult.Yes == MessageBox.Show("重量和件数超出，是否要继续出库？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2))
            //    {
            //        this.ScanOut(cmb_Barcode.Text.Trim(), "O02");
            //    }
            //}

            //if (strs[2] == "2")
            //{
            //    if (DialogResult.Yes == MessageBox.Show("重量超出，是否要继续出库？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2))
            //    {
            //        this.ScanOut(cmb_Barcode.Text.Trim(), "O03");
            //    }
            //}

            //if (strs[2] == "3")
            //{
            //    if (DialogResult.Yes == MessageBox.Show("件数超出，是否要继续出库？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2))
            //    {
            //        this.ScanOut(cmb_Barcode.Text.Trim(), "O04");
            //    }
            //}
        }

        private void ScanOut(string ProductID, string command)
        {
            string[] msgs = { command, ProductID, this.cmb_Voucherno.Text.Trim(), this.dt_Bill.Text };
            string msg = (Utils.WMSMessage.MakeWMSSocketMsg(msgs));
            this.ClearOldInfomation();

            this.SendSocketMsg(msg,msgs);
        }

        private void SendSocketMsg(string msg,string[] msgs)
        {
            string ss = "";
            if (msgs[1] == "O02") {
               ss=  pdaform.Process_O02(msgs);
            }
            if (msgs[1] == "O03")
            {
                 ss = pdaform.Process_O03(msgs);
            } if (msgs[1] == "O04")
            {
                 ss = pdaform.Process_O04(msgs);
            } if (msgs[1] == "O08")
            {
                 ss = pdaform.Process_O08(msgs);
            } if (msgs[1] == "O15")
            {
                 ss = pdaform.Process_O15(msgs);
            }
            string[] strs = ss.TrimStart(Utils.WMSMessage._StartChar).TrimEnd(Utils.WMSMessage._EndChar).Split(Utils.WMSMessage._SpliteChar);
            this.txtResult.Text=strs[3];
        }

        private void ClearOldInfomation()
        {
            this.txt_Contract.Text = "";
            this.txt_Core.Text = "";
            this.txt_Grade_Desc.Text = "";
            this.txt_PaperWidth.Text = ""; ;
            this.txt_Remark_Scan.Text = "";
            this.txt_PaperCert.Text = "";
            this.txtResult.Text = "";
            this.txt_Weight.Text = "";
            this.txt_Material.Text = "";
            this.txt_Length.Text = "";
            this.txt_Layers.Text = "";
            //this.txt_Direction.Text = "";
            //this.txt_Count.Text = "";
            //this.txt_CustTrademark.Text = "";
            this.txt_PaperCert.Text = "";
            this.txt_Sku.Text = "";
            this.txt_SpecCustName.Text = "";
            this.txt_SpecProdName.Text = "";
            this.txt_WeightMode.Text = "";
            this.txt_Diameter.Text = "";
            //this.txt_Direction.Text = "";
            this.txt_IsWhiteFlag.Text = "";
            this.txt_Package.Text = "";
            this.txt_Remark_Scan.Text = "";
            //this.txt_TradeMode.Text = "";
            //this.txt_Brand.Text = "";
            this.txt_borde.Text = "";
            //this.txt_SlidesReam.Text = "";
        }

        #region  串口
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

        private bool ClosSerialPort(System.IO.Ports.SerialPort serialport)
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
        private delegate void deSetText(string text, string color);

        private void SetText(string text, string color)
        {

            if (this.cmb_Barcode.InvokeRequired)
            {
                deSetText d = new deSetText(SetText);
                this.Invoke(d, new object[] { text, "" });
            }
            else
            {
                this.cmb_Barcode.Text = "";
                this.cmb_Barcode.Text = text;
                this.cmb_Barcode_ButtonClick(null,null);
                // SetRollID(text.Substring(0, 14));
                // SetScanProductID_HM(text);
            }
        }
        #endregion

        private void cmb_Barcode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
                this.cmb_Barcode_ButtonClick(null,null);
        }
    }
}
