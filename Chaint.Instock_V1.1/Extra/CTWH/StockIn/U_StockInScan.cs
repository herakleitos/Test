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
    public partial class U_StockInScan :  DevExpress.XtraEditors.XtraUserControl
    {
        MainLayout _MainLayout = null;
        CTWH.Common.MSSQL.WMSAccess _WMSAccess;
        private bool _IsLoaded = false;
        public U_StockInScan()
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
        public U_StockInScan(MainLayout tl)
        {
            InitializeComponent();
            this._MainLayout = tl;
            this._MainLayout.OnMenuClickEvent += new MenuClickEventHandle(OnMenuClickEventProcess);
            this.Disposed += new EventHandler(U_StockInScan_Disposed);
        }
        void U_StockInScan_Disposed(object sender, EventArgs e)
        {
            this._MainLayout.OnMenuClickEvent -= new MenuClickEventHandle(OnMenuClickEventProcess);
            this.pdaform.DisposeData();
            this.CloseSerialPort(this.spScan);
        }
        public void OnMenuClickEventProcess(object sender, MenuClickEventArgs e)
        {

            switch (e.MenuName)
            {
                case Utils.WMSMenu._New:
                    MessageBox.Show("S_New");
                    break;
                case Utils.WMSMenu._Edit:
                    MessageBox.Show("S_Update");
                    break;
                case Utils.WMSMenu._Save:
                    MessageBox.Show("S_Save");
                    break;
                case Utils.WMSMenu._Delete:
                    MessageBox.Show("S_Delete");
                    break;

            }

        }
        WH_PDA_Service.PDAForm pdaform = new WH_PDA_Service.PDAForm(false);

        private void txt_Barcode_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {

            //判断该填的东西是否都全了
            if (!this.CheckWHInfo())
                return;

            //发送条形码
            
            string barcode = this.txt_Barcode.Text.Trim();
            string emp = this.cmb_InEmp.Text.Split('.')[0];
            string trademode = "";// this.cmb_tradeType.Text.Split('.')[0];
            string iswhite = "";// this.cmb_Iswhite.Text.Split('.')[0];

            string package = "";//this.cmb_Package.Text.Split('.')[0];
            string remark = this.cmb_WHRemark.Text.Trim();
            string org = this.cmb_Factory.Text.Split('.')[0];
            string businesstype = this.cmbBusiness.Text.Split('.')[0];
            string whcode = this.cmbWH.Text.Split('.')[0];
            string shift = this.cmb_InShift.Text.Split('.')[0];

            string shiftTime = this.cmb_ShiftTime.Text;
            switch (businesstype)
            {
                case  "HDRK": //那么就是红单的情况
                    if (this.chkCancel.Checked)
                    {
                        //取消红单入库
                        string command = "";
                        string[] ir03 = new string[] { command, barcode, emp, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") };
                        string msg = Utils.WMSMessage.MakeWMSSocketMsg(ir03);
                        string[] strs = msg.TrimStart(Utils.WMSMessage._StartChar).TrimEnd(Utils.WMSMessage._EndChar).Split(Utils.WMSMessage._SpliteChar);
                        string result = "";
                        if (barcode.Substring(2, 1) == "1")
                        {
                            command = "IR05";
                            result = pdaform.Process_IR05(strs);
                        }
                        else if (barcode.Substring(2, 1) == "2")
                        {
                            command = "IP05";
                            result = pdaform.Process_IP05(strs);
                        }
                        else {
                            MessageBox.Show("条形码错误。");
                            return;
                        }
                        this.ShowNewInformation(result);
                        if (this.lst_msg.Items.Count > 1000)
                            this.lst_msg.Items.Clear();
                        this.lst_msg.Items.Add(DateTime.Now.ToString("HH:mm:ss|") + barcode + "|取消红单入库");
                    }
                    else
                    {
                        //红单入库
                        string command = "";
                        string[] ir02 = new string[] { command, barcode, this.dateS.DateTime.ToString("yyyy-MM-dd HH:mm:ss"), businesstype, "", org, whcode, emp, "", shift, shiftTime, remark, iswhite, trademode };

                        string msg = Utils.WMSMessage.MakeWMSSocketMsg(ir02);
                        string[] strs = msg.TrimStart(Utils.WMSMessage._StartChar).TrimEnd(Utils.WMSMessage._EndChar).Split(Utils.WMSMessage._SpliteChar);
                        string result = "";
                        if (barcode.Substring(2, 1) == "1")
                        {
                            command = "IR04";
                            result = pdaform.Process_IR04(strs);
                        }
                        else if (barcode.Substring(2, 1) == "2")
                        {
                            command = "IP04";
                            result = pdaform.Process_IP04(strs);
                        }
                        else
                        {
                            MessageBox.Show("条形码错误。");
                            return;
                        }


                        this.ShowNewInformation(result);
                        if (this.lst_msg.Items.Count > 1000)
                            this.lst_msg.Items.Clear();
                        this.lst_msg.Items.Add(DateTime.Now.ToString("HH:mm:ss|") + barcode + "|红单扫描入库");
                    }
                    break;
                default : //蓝单的情况
                    if (this.chkCancel.Checked)
                    {
                        //取消入库
                        string command = "";
                        if (barcode.Substring(2, 1) == "1")
                            command = "IR03";
                        else if (barcode.Substring(2, 1) == "2")
                            command = "IP03";

                        else
                        {
                            MessageBox.Show("条形码错误。");
                            return;
                        }

                        string[] ir03 = new string[] { command, barcode, emp, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") };
                        string msg = Utils.WMSMessage.MakeWMSSocketMsg(ir03);

                        string[] strs = msg.TrimStart(Utils.WMSMessage._StartChar).TrimEnd(Utils.WMSMessage._EndChar).Split(Utils.WMSMessage._SpliteChar);
                        string result = "";
                        if (command == "IR03")
                            result = pdaform.Process_IR03(strs);
                        else if (command == "IP03")
                            result = pdaform.Process_IP03(strs);
                        //this.txt_Info.Text = result;
                        this.ShowNewInformation(result);
                        if (this.lst_msg.Items.Count > 1000)
                            this.lst_msg.Items.Clear();
                        this.lst_msg.Items.Add(DateTime.Now.ToString("HH:mm:ss|") + barcode + "|取消入库");
                    }
                    else
                    {
                        string command = "";
                        if (barcode.Substring(2, 1) == "1")
                            command = "IR02";
                        else if (barcode.Substring(2, 1) == "2")
                            command = "IP02";

                        else
                        {
                            MessageBox.Show("条形码错误。");
                            return;
                        }

                        string[] ir02 = new string[] { command, barcode, this.dateS.DateTime.ToString("yyyy-MM-dd HH:mm:ss"), businesstype, "", org, whcode, emp, "", shift, shiftTime, remark, iswhite, trademode };

                        string msg = Utils.WMSMessage.MakeWMSSocketMsg(ir02);
                        string[] strs = msg.TrimStart(Utils.WMSMessage._StartChar).TrimEnd(Utils.WMSMessage._EndChar).Split(Utils.WMSMessage._SpliteChar);
                        string result = "";
                        if (command == "IR02")
                            result = pdaform.Process_IR02(strs);
                        else if (command == "IP02")
                            result = pdaform.Process_IP02(strs);
                        //this.txt_Info.Text = result;


                        this.ShowNewInformation(result);
                        if (this.lst_msg.Items.Count > 1000)
                            this.lst_msg.Items.Clear();
                        this.lst_msg.Items.Add(DateTime.Now.ToString("HH:mm:ss|") + barcode + "|扫描入库");
                    }
                    break;



            }
            
        }
        /// <summary>
        /// 检查入库的仓库信息是否全了
        /// </summary>
        /// <returns></returns>
        private bool CheckWHInfo()
        {
            string user = this.cmb_InEmp.Text;
            if (user == "")
            {
                MessageBox.Show("请选择入库人员。");
                return false;
            }
            string shift = this.cmb_InShift.Text;
            if (shift == "")
            {
                MessageBox.Show("请选择入库班组。");
                return false;
            }
            string shifttime = this.cmb_ShiftTime.Text;
            if (shifttime == "")
            {
                MessageBox.Show("请选择入库班次。");
                return false;
            }
            DateTime time = this.dateS.DateTime;
            if (time.CompareTo(DateTime.Now)>0)
            {
                MessageBox.Show("交班时间不能晚于系统时间。");
                return false;
            }
            string business = this.cmbBusiness.Text;
            if (business == "")
            {
                MessageBox.Show("请选择业务类型。");
                return false;
            }
            string factory = this.cmb_Factory.Text;
            //if (factory == "")
            //{
            //    MessageBox.Show("请选择入库组织。");
            //    return false;
            //}
            string wh = this.cmbWH.Text;
            //if (wh == "")
            //{
            //    MessageBox.Show("请选择入库仓库。");
            //    return false;
            //}
            string remark = this.cmb_WHRemark.Text;

            return true;
        }

        private void ShowNewInformation(string result)
        {
            string[] strs = result.TrimStart(Utils.WMSMessage._StartChar).TrimEnd(Utils.WMSMessage._EndChar).Split(Utils.WMSMessage._SpliteChar);
            if (strs[1] == "IRA02" || strs[1] == "IPA02" || strs[1] == "IRA04" || strs[1] == "IPA04")
            {
                if (strs.Length > 5)
                {

                    this.txt_Material.Text = strs[6];
                    this.txt_Grade_Desc.Text = strs[7];
                    this.txt_PaperWidth.Text = strs[8];
                    this.txt_Diameter.Text = strs[30];
                    this.txt_Core.Text = strs[10];
                    this.txt_Length.Text = strs[11];
                    this.txt_Direction.Text = strs[12];
                    this.txt_Layers.Text = strs[13];
                    this.txt_WeightMode.Text = strs[14];
                    //this.txt_SlidesReam.Text = strs[15];
                    this.txt_Weight.Text = strs[16];
                    this.txt_Count.Text = strs[17];
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
                }

                if (strs[2] == "0")
                    this.txt_Result.ForeColor = Color.Blue;
                else
                    this.txt_Result.ForeColor = Color.Red;
                this.txt_Result.Text = strs[3];
            }
            else if (strs[1] == "IRA03" || strs[1] == "IPA03" || strs[1] == "IRA05" || strs[1] == "IPA05")
            {
                if (strs[2] == "0")
                    this.txt_Result.ForeColor = Color.Blue;
                else
                    this.txt_Result.ForeColor = Color.Red;
                this.txt_Result.Text = strs[3];
            }
        }

       
        private void U_StockInScanRoll_Load(object sender, EventArgs e)
        {
            this.dateS.DateTime = DateTime.Now;

            OpenSerialPort(this.spScan, Utils.SerialParaScan1.PortName);

          
            //如果默认数据为空就 插入一条新的

            //加载人员
            string ss = pdaform.LoadWHUser();
            string[] strs = ss.TrimStart(Utils.WMSMessage._StartChar).TrimEnd(Utils.WMSMessage._EndChar).Split(Utils.WMSMessage._SpliteChar);
            string result = strs[2];
            if (result == "0")
            {
                this.cmb_InEmp.Properties.Items.Clear();
                string[] users = strs[4].Split(Utils.WMSMessage._ForeachChar);
                foreach (string user in users)
                {
                    this.cmb_InEmp.Properties.Items.Add(user);
                }
            }
            this.cmb_InEmp.SelectedText = Utils.LoginUserName;
            //加载机台
            string ss2 = pdaform.LoadWHMachine();
            string[] strs2 = ss2.TrimStart(Utils.WMSMessage._StartChar).TrimEnd(Utils.WMSMessage._EndChar).Split(Utils.WMSMessage._SpliteChar);
            string result2 = strs2[2];
            if (result2 == "0")
            {
                this.cmb_Factory.Properties.Items.Clear();
                string[] users = strs2[4].Split(Utils.WMSMessage._ForeachChar);
                foreach (string user in users)
                {
                    this.cmb_Factory.Properties.Items.Add(user);
                }
            }



            //加载业务类型
            string ss3 = pdaform.LoadWHBusinessType("in");
            string[] strs3 = ss3.TrimStart(Utils.WMSMessage._StartChar).TrimEnd(Utils.WMSMessage._EndChar).Split(Utils.WMSMessage._SpliteChar);
            string result3 = strs3[2];
            if (result3 == "0")
            {
                this.cmbBusiness.Properties.Items.Clear();
                string[] users = strs3[4].Split(Utils.WMSMessage._ForeachChar);
                foreach (string user in users)
                {
                    this.cmbBusiness.Properties.Items.Add(user);
                }
            }

            //加载班组
            string ss4 = pdaform.LoadWHShift();
            string[] strs4 = ss4.TrimStart(Utils.WMSMessage._StartChar).TrimEnd(Utils.WMSMessage._EndChar).Split(Utils.WMSMessage._SpliteChar);
            string result4 = strs4[2];
            if (result4 == "0")
            {
                this.cmb_InShift.Properties.Items.Clear();
                string[] users = strs4[4].Split(Utils.WMSMessage._ForeachChar);
                foreach (string user in users)
                {
                    this.cmb_InShift.Properties.Items.Add(user);
                }
            }

            //加载默认数据
            _WMSAccess = Utils.WMSSqlAccess;
            _WMSAccess.SqlStateChange += new CTWH.Common.MSSQL.WMSAccess.SqlStateEventHandler(access_SqlStateChange);

            WMSDS ds = this._WMSAccess.Select_T_DefaultDisplay("SCANIN");
            if ( ds.T_DefaultDisplay.Rows.Count > 0)
            {
                string user = ds.T_DefaultDisplay.Rows[0][ds.T_DefaultDisplay.define1Column].ToString();
                string shift = ds.T_DefaultDisplay.Rows[0][ds.T_DefaultDisplay.define2Column].ToString();
                string shifttime = ds.T_DefaultDisplay.Rows[0][ds.T_DefaultDisplay.define3Column].ToString();
                string time = ds.T_DefaultDisplay.Rows[0][ds.T_DefaultDisplay.define4Column].ToString();
                string business = ds.T_DefaultDisplay.Rows[0][ds.T_DefaultDisplay.define5Column].ToString();
                string org = ds.T_DefaultDisplay.Rows[0][ds.T_DefaultDisplay.define6Column].ToString();
                string remark = ds.T_DefaultDisplay.Rows[0][ds.T_DefaultDisplay.define7Column].ToString();
                this.cmb_Factory.EditValue = org;
                this.cmb_InEmp.EditValue = user;
                this.cmb_InShift.EditValue = shift;
                this.cmb_ShiftTime.EditValue = shifttime;
                this.dateS.DateTime = Convert.ToDateTime(time);
                this.cmb_WHRemark.Text = remark;
                this.cmbBusiness.EditValue = business;
            }
            else { 
            //没有就插入一条
                string user = this.cmb_InEmp.Text.ToString();
                string shift = this.cmb_InShift.Text.ToString();
                string shifttime = this.cmb_ShiftTime.Text.ToString();
                string time = this.dateS.DateTime.ToString("yyyy-MM-dd HH:mm:ss");
                string business = this.cmbBusiness.Text.ToString();
                string org = this.cmb_Factory.Text.ToString();
                string remark = this.cmb_WHRemark.Text.ToString();
                WMSDS.T_DefaultDisplayRow ddr = new WMSDS().T_DefaultDisplay.NewT_DefaultDisplayRow();
                ddr.define1 = user;
                ddr.define2 = shift;
                ddr.define3 = shifttime;
                ddr.define4 = time;
                ddr.define5 = business;
                ddr.define6 = org;
                ddr.define7 = remark;
                ddr.dispType = "SCANIN";
             int id=   this._WMSAccess.Insert_T_DefaultDisplay(ddr);
             if (id > 0) { 
             //插入成功
             }
            }
            this._IsLoaded = true;

        }

        private void txt_Barcode_EditValueChanged(object sender, EventArgs e)
        {
            this.ClearOldInfomation();
        }

        private void ClearOldInfomation()
        {
            this.txt_Contract.Text = "";
            this.txt_Core.Text = "";
            this.txt_Grade_Desc.Text = "";
            this.txt_PaperWidth.Text = ""; ;
            this.txt_Remark_Scan.Text = "";
            this.txt_PaperCert.Text = "";
            this.txt_Result.Text = "";
            this.txt_Weight.Text = "";
            this.txt_Material.Text = "";
            this.txt_Length.Text = "";
            this.txt_Layers.Text = "";
            this.txt_Direction.Text = "";
            this.txt_Count.Text = "";
            //this.txt_CustTrademark.Text = "";
            this.txt_PaperCert.Text = "";
            this.txt_Sku.Text = "";
            this.txt_SpecCustName.Text = "";
            this.txt_SpecProdName.Text = "";
            this.txt_WeightMode.Text = "";
            this.txt_Diameter.Text = "";
            this.txt_Direction.Text = "";
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
                        SetText(sbBarCode.ToString(),"");  
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

            if (this.txt_Barcode.InvokeRequired)
            {
                deSetText d = new deSetText(SetText);
                this.Invoke(d, new object[] { text, "" });
            }
            else
            {
                this.txt_Barcode.Text = "";
                this.txt_Barcode.Text = text;
                this.txt_Barcode_ButtonClick(null,null);
                // SetRollID(text.Substring(0, 14));
                // SetScanProductID_HM(text);
            }
        }
        #endregion

        private void txt_Barcode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
                this.txt_Barcode_ButtonClick(null,null);
        }

        private void cmb_InEmp_TextChanged(object sender, EventArgs e)
        {
            if(this._IsLoaded)
            this.UpdateDefaultDisplay();
        }

        private void UpdateDefaultDisplay()
        {
            //有变化就更新一条
            string user = this.cmb_InEmp.Text.ToString();
            string shift = this.cmb_InShift.Text.ToString();
            string shifttime = this.cmb_ShiftTime.Text.ToString();
            string time = this.dateS.DateTime.ToString("yyyy-MM-dd HH:mm:ss");
            string business = this.cmbBusiness.Text.ToString();
            string org = this.cmb_Factory.Text.ToString();
            string remark = this.cmb_WHRemark.Text.ToString();
            WMSDS.T_DefaultDisplayRow ddr = new WMSDS().T_DefaultDisplay.NewT_DefaultDisplayRow();
            ddr.define1 = user;
            ddr.define2 = shift;
            ddr.define3 = shifttime;
            ddr.define4 = time;
            ddr.define5 = business;
            ddr.define6 = org;
            ddr.define7 = remark;
            ddr.dispType = "SCANIN";
            string id = this._WMSAccess.Update_T_DefaultDisplay(ddr);
            if (id == "")
            {
                //更新成功
            }
        }

        private void cmb_InShift_TextChanged(object sender, EventArgs e)
        {
            if (this._IsLoaded)

            this.UpdateDefaultDisplay();
        }

        private void cmb_ShiftTime_TextChanged(object sender, EventArgs e)
        {
            if (this._IsLoaded)

            this.UpdateDefaultDisplay();
        }

        private void dateS_TextChanged(object sender, EventArgs e)
        {
            if (this._IsLoaded)

            this.UpdateDefaultDisplay();
        }

        private void cmbBusiness_TextChanged(object sender, EventArgs e)
        {
            if (this._IsLoaded)

            this.UpdateDefaultDisplay();
        }

        private void cmb_Factory_TextChanged(object sender, EventArgs e)
        {
            if (this._IsLoaded)

            this.UpdateDefaultDisplay();
        }

        private void cmb_WHRemark_TextChanged(object sender, EventArgs e)
        {
            if (this._IsLoaded)

            this.UpdateDefaultDisplay();
        }

    }
}
