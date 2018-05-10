using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataModel;
using CTWH.Common;

namespace CTWH.StockIn
{
    public partial class U_StockInStat : UserControl
    {

        CTWH.Common.MSSQL.WMSAccess _WMSAccess;

        public U_StockInStat()
        {
            InitializeComponent();
        }
        private void DisposeData()
        {
            _WMSAccess.SqlStateChange -= new CTWH.Common.MSSQL.WMSAccess.SqlStateEventHandler(access_SqlStateChange);
            _WMSAccess = null;
        }
        private void btn_Query_Click(object sender, EventArgs e)
        {
            string factory = this.cmb_Factory.Text.Trim();
            factory = factory == "全部" ? "" : factory;
            string user = this.cmb_Emp.EditValue == null ? "全部" : this.cmb_Emp.EditValue.ToString();
            user = user == "全部" ? "" : user;

            string dateS = this.date_Start.DateTime.ToString("yyyy-MM-dd HH:mm:ss");
            string dateE = this.date_End.DateTime.ToString("yyyy-MM-dd HH:mm:ss");
            string pType = "";
            WMSQueryDS wms = this._WMSAccess.Select_T_Product_InForStat(factory, user, dateS, dateE, pType);
            this.grid_InStat.DataSource = wms.T_Product_In_Stat;
            //this.gridView1.BestFitColumns();
        }
        private void LoadT_Factory()
        {
            WMSDS wmsDS = this._WMSAccess.Select_T_Factory(true,false);
            wmsDS.T_Factory.Rows.Add(wmsDS.T_Factory.NewT_FactoryRow());
            this.cmb_Factory.Properties.DataSource = wmsDS.T_Factory;
            this.cmb_Factory.Properties.ValueMember = "OnlyID";
            this.cmb_Factory.Properties.DisplayMember = "MachineID";
        }
        //private void LoadMachine()
        //{
        //    //加载机台
        //    string ss = pdaform.LoadWHMachine();
        //    string[] strs = ss.TrimStart(Utils.WMSMessage._StartChar).TrimEnd(Utils.WMSMessage._EndChar).Split(Utils.WMSMessage._SpliteChar);
        //    string result = strs[2];
        //    if (result == "0")
        //    {
        //        this.cmb_Factory.Properties.Items.Clear();
        //        string[] users = strs[4].Split(Utils.WMSMessage._ForeachChar);
        //        foreach (string user in users)
        //        {
        //            this.cmb_Factory.Properties.Items.Add(user);
        //        }
        //    }
        //}
        private void LoadT_User()
        {
            WMSDS wmsDS = this._WMSAccess.Select_T_User("");
            wmsDS.T_User.Rows.Add(wmsDS.T_User.NewT_UserRow());
            this.cmb_Emp.Properties.DataSource = wmsDS.T_User;
            this.cmb_Emp.Properties.ValueMember = "UserCode";
            this.cmb_Emp.Properties.DisplayMember = "UserName";
        }
        void access_SqlStateChange(object sender, SqlStateEventArgs e)
        {
            if (e.IsConnect == false)
            {
                Utils.WriteTxtLog(Utils.FilePath_txtMSSQLLog, "DataBase Error:" + e.Info);
            }
        }
        private void U_StockInStat_Load(object sender, EventArgs e)
        {
            _WMSAccess = Utils.WMSSqlAccess;
            _WMSAccess.SqlStateChange += new CTWH.Common.MSSQL.WMSAccess.SqlStateEventHandler(access_SqlStateChange);

            //加载人员
            LoadT_Factory();
            LoadT_User();
            this.date_Start.DateTime = DateTime.Now.AddDays(-1);
            this.date_End.DateTime = DateTime.Now;
        }

        private void btn_Export_Click(object sender, EventArgs e)
        {
            SaveFileDialog sbd = new SaveFileDialog();
            sbd.Filter = "Excel 文件 (*.xls)|*.xls";
            sbd.FileName = "入库统计数据"+DateTime.Now.ToString("yyyyMMdd_HHmmss");

            DialogResult res = sbd.ShowDialog();
            if (DialogResult.OK == res || DialogResult.Yes == res)
            {
                this.grid_InStat.ExportToXls(sbd.FileName);
            }

            //if (sbd.ShowDialog() == DialogResult.OK || sbd.ShowDialog() == DialogResult.Yes)
            //    this.grid_InStat.ExportToXls(sbd.FileName);
        }

    }
}
