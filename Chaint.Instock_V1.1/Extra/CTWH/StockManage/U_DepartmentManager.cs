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
    public partial class U_DepartmentManager : DevExpress.XtraEditors.XtraUserControl
    {
        CTWH.Common.MSSQL.WMSAccess _WMSAccess;

        public U_DepartmentManager()
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
        private void U_DepartmentManager_Load(object sender, EventArgs e)
        {
            _WMSAccess = Utils.WMSSqlAccess;
            _WMSAccess.SqlStateChange += new CTWH.Common.MSSQL.WMSAccess.SqlStateEventHandler(access_SqlStateChange);

            InterfaceDS ids = this._WMSAccess.Select_CT_BMZD("","");
            this.grid_Dept.DataSource = ids.CT_BMZD;
            this.gridView1.BestFitColumns();
        }
    }
}
