using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using CTWH.Common;

namespace CTWH.StockIn
{
    public partial class U_StockInScanPallet : DevExpress.XtraEditors.XtraUserControl
    {


        MainLayout _MainLayout = null;
        public U_StockInScanPallet(MainLayout tl)
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
        WH_PDA_Service.PDAForm pdaform = new WH_PDA_Service.PDAForm();

        public U_StockInScanPallet()
        {
            InitializeComponent();
        }

       

        private void txt_Barcode_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {


            //发送条形码
            string barcode = this.txt_Barcode.Text.Trim();
            string emp = this.cmb_InEmp.Text.Split('.')[0];

            if (this.chkCancel.Checked)
            {
                //取消入库


                string[] ip03 = new string[] { "IP03", barcode, emp, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") };
                string msg = Utils.WMSMessage.MakeWMSSocketMsg(ip03);

                string[] strs = msg.TrimStart(Utils.WMSMessage._StartChar).TrimEnd(Utils.WMSMessage._EndChar).Split(Utils.WMSMessage._SpliteChar);

                string result = pdaform.Process_IP03(strs);
                this.txt_Info.Text = result;
            }
            else
            {

                string trademode = "";// this.cmb_tradeType.Text.Split('.')[0];
                string iswhite = ""; //this.cmb_Iswhite.Text.Split('.')[0];

                string package = "";// this.cmb_Package.Text.Split('.')[0];
                string remark = this.cmb_WHRemark.Text.Trim();
                string org = this.cmb_Factory.Text.Split('.')[0];
                string businesstype = this.cmbBusiness.Text.Split('.')[0];
                string whcode = this.cmbWH.Text.Split('.')[0];
                string shift = this.cmb_InShift.Text.Split('.')[0];

                string shiftTime = this.cmb_ShiftTime.Text;
                string[] ip02 = new string[] { "IP02", barcode, this.dateS.DateTime.ToString("yyyy-MM-dd HH:mm:ss"), businesstype, "", org, whcode, emp, "", shift, shiftTime, remark, iswhite, trademode };

                string msg = Utils.WMSMessage.MakeWMSSocketMsg(ip02);
                string[] strs = msg.TrimStart(Utils.WMSMessage._StartChar).TrimEnd(Utils.WMSMessage._EndChar).Split(Utils.WMSMessage._SpliteChar);

                string result = pdaform.Process_IP02(strs);
                this.txt_Info.Text = result;
            }
        }

        private void U_StockInScanPallet_Load(object sender, EventArgs e)
        {
            this.dateS.DateTime = DateTime.Now;
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
        }
    }
}
