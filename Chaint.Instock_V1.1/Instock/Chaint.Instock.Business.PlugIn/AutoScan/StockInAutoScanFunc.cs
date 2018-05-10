using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Data.SqlClient;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Data;
using DevExpress.DataAccess.Sql;
using Chaint.Common.Core;
using Chaint.Common.Core.AppConfig;
using Chaint.Common.Devices.PLC;
using Chaint.Common.Core.Sql;
using Chaint.Common.Core.Enums;
using Chaint.Common.Devices.PLC.Utils;
using Chaint.Common.Devices.LED;
using Chaint.Common.Devices.Data;
using Chaint.Common.Devices.Utils;
using Chaint.Common.Core.Const;
using Chaint.Common.Core.Log;
using Chaint.Common.Core.Utils;
using Chaint.Common.ServiceHelper;
using Chaint.Instock.Core;
using Chaint.Common.Devices.Devices;
namespace Chaint.Instock.Business.PlugIns
{
    public class StockInAutoScanFunc
    {
        private Context ctx;
        private AppConfig_INI appConfiger;
        public DataTable plcConnection;
        public DataTable plcConfig;
        public StockInAutoScanFunc(Context context)
        {
            ctx = context;
            appConfiger = new AppConfig_INI(ctx.DevicesConfigFilePath);
            InitPlcSetting();
        }
        #region private fun
        private void InitPlcSetting()
        {
            string gongWeiType = appConfiger.GetValue("PLCConnection", "GongWeiType", "");
            string moBanNo = appConfiger.GetValue("PLCConnection", "MoBanNo", "");
            string sql = @"SELECT * FROM PLC_CONFIG WHERE  GONGWEITYPE=@GONGWEITYPE AND MOBANNO=@MOBANNO;;
                        SELECT * FROM PLC_CONNECT WHERE GONGWEITYPE=@GONGWEITYPE AND MOBANNO=@MOBANNO;";
            StringBuilder sb = new StringBuilder();
            List<SqlParameter> parameters = new List<SqlParameter>();
            SqlParameter sqlPara1 = new SqlParameter("@GONGWEITYPE", DbType.String);
            sqlPara1.Value = gongWeiType;
            parameters.Add(sqlPara1);
            SqlParameter sqlPara2 = new SqlParameter("@MOBANNO", DbType.String);
            sqlPara2.Value = moBanNo;
            parameters.Add(sqlPara2);
            sql = string.Format(sql, sb.ToString().TrimEnd(','));
            OperateResults results =
                DBAccessServiceHelper.ExcuteQuery(this.ctx, sql, new string[] { "T1", "T2" }, parameters.ToArray());
            plcConnection = results.ResultData.Tables["T2"];
            plcConfig = results.ResultData.Tables["T1"];
        }
        #endregion
    }
}
