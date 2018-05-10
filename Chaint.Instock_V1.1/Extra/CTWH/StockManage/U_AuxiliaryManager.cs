using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using CTWH.Common;
using DataModel;

namespace CTWH.StockManage
{
    public partial class U_AuxiliaryManager : DevExpress.XtraEditors.XtraUserControl
    {
        CTWH.Common.MSSQL.WMSAccess _WMSAccess;

        public U_AuxiliaryManager()
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
        private void btn_Query_Click(object sender, EventArgs e)
        {
            string type = this.cmb_Type.Text.Trim().Split('.')[0];
          InterfaceDS iDS =  this._WMSAccess.Select_CT_ZDYZD(type);
          this.grid_Aux.DataSource = iDS.CT_ZDYZD;
        }

        private void U_AuxiliaryManager_Load(object sender, EventArgs e)
        {
            _WMSAccess = Utils.WMSSqlAccess;
            _WMSAccess.SqlStateChange += new CTWH.Common.MSSQL.WMSAccess.SqlStateEventHandler(access_SqlStateChange);

        }
    }
}
