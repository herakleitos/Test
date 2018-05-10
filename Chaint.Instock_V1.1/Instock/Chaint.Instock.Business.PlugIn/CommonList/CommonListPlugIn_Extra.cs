using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Columns;
using Chaint.Common.Core.Utils;
using Chaint.Common.Core;
using Chaint.Common.Core.Const;
using Chaint.Instock.Core;
using Chaint.Common.ServiceHelper;

namespace Chaint.Instock.Business.PlugIns
{
    partial class CommonListPlugIn
    {
        private void CreateColumns()
        {
            if (formName == Const_Option.Const_StockAreaPlan)
            {
                form.Text = "库区属性定义列表";
                CreateStockAreaPlanColumns();
            }
            else if (formName == Const_Option.Const_StockArea)
            {
                form.Text = "库区列表";
                CreateStockAreaColumns();
            }
            else if (formName == Const_Option.Const_Stock)
            {
                form.Text = "仓库列表";
                CreateStockColumns();
            }
        }

        private void QueryData()
        {
            if (formName == Const_Option.Const_StockAreaPlan)
            {
                BindStockAreaPlanData();
            }
            else if (Convert.ToString(formName) == Const_Option.Const_StockArea)
            {
                BindStockAreaData();
            }
            else if (Convert.ToString(formName) == Const_Option.Const_Stock)
            {
                BindStockData();
            }
            this.View.GetControl<GridView>(Const_CommonList.Main_GridView).BestFitColumns();
        }

        private void BindStockData()
        {
            if (Validate())
            {
                DateTime beginDate = this.View.GetValue<DateTime>(Const_CommonList.Query_FBeginDate).Date;
                DateTime endDate = this.View.GetValue<DateTime>(Const_CommonList.Query_FEndDate).Date.AddDays(1).AddSeconds(-1);
                string sql = @"SELECT 0 AS FCHECK,STOCK.*
                                FROM T_AUTOSCAN_STOCK STOCK
                                WHERE STOCK.FCREATEDATE BETWEEN @FBEGINDATE  AND @FENDDATE 
                                ORDER BY STOCK.FCREATEDATE";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@FBEGINDATE", DbType.DateTime));
                parameters.Add(new SqlParameter("@FENDDATE", DbType.DateTime));
                parameters[0].Value = beginDate;
                parameters[1].Value = endDate;
                string[] tableName = new string[] { "T1" };
                OperateResults result = DBAccessServiceHelper.ExcuteQuery(this.Context, sql, tableName, parameters.ToArray());
                if (result.IsSuccess)
                {
                    GridControl gvMain = this.View.GetControl<GridControl>(Const_CommonList.Main_GridControl);
                    gvMain.DataSource = result.ResultData.Tables["T1"];
                }
            }
        }

        private void BindStockAreaData()
        {
            if (Validate())
            {
                DateTime beginDate = this.View.GetValue<DateTime>(Const_CommonList.Query_FBeginDate).Date;
                DateTime endDate = this.View.GetValue<DateTime>(Const_CommonList.Query_FEndDate).Date.AddDays(1).AddSeconds(-1);
                string sql = @"SELECT 0 AS FCHECK,SA.FID,SAE.FENTRYID,STK.FNAME AS FSTOCK,SAE.FSTOCKAREANUMBER,SAE.FSTOCKAREANAME,
                                SAE.FTOTALCAPACITY,SAE.FLOCATION,SA.FCREATEDATE,SA.FMODIFYDATE
                                FROM T_AUTOSCAN_STOCKAREA SA
								LEFT JOIN T_AUTOSCAN_STOCK STK ON SA.FSTOCK = STK.FNUMBER
                                INNER JOIN T_AUTOSCAN_STOCKAREAENTRY SAE ON SA.FID = SAE.FHEADID
                                WHERE SA.FCREATEDATE BETWEEN @FBEGINDATE  AND @FENDDATE 
                                ORDER BY SA.FCREATEDATE,SAE.FSEQ";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@FBEGINDATE", DbType.DateTime));
                parameters.Add(new SqlParameter("@FENDDATE", DbType.DateTime));
                parameters[0].Value = beginDate;
                parameters[1].Value = endDate;
                string[] tableName = new string[] { "T1" };
                OperateResults result = DBAccessServiceHelper.ExcuteQuery(this.Context, sql, tableName, parameters.ToArray());
                if (result.IsSuccess)
                {
                    GridControl gvMain = this.View.GetControl<GridControl>(Const_CommonList.Main_GridControl);
                    gvMain.DataSource = result.ResultData.Tables["T1"];
                }
            }
        }

        private void BindStockAreaPlanData()
        {
            if (Validate())
            {
                DateTime beginDate = this.View.GetValue<DateTime>(Const_CommonList.Query_FBeginDate).Date;
                DateTime endDate = this.View.GetValue<DateTime>(Const_CommonList.Query_FEndDate).Date.AddDays(1).AddSeconds(-1);
                string sql = @"SELECT 0 AS FCHECK,FID,FNUMBER,FNAME, 
                CASE FPAPERTYPE WHEN '1' THEN '卷筒纸' WHEN '2' THEN '平板纸' END AS FPAPERTYPE,
                FPAPERGRADE,FCERTIFICATION,FPACKTYPE,FTRANSPORTTYPE,FCOLOR,FCOREDIAMETERORREAM,FDIAMETERORSLIDES,FLENGTH,
                FTRADEMARKSTYLE,FSPECPRODNAME,FSPECCUSTNAME,FWEIGHTMODE,FCREATEDATE,FMODIFYDATE,FSPECIFICATION,FMEMO,
                FPONUMBER,FDELIVERDATE,FSPCUSTOMER
                FROM T_AUTOSCAN_STOCKAREAPLAN WHERE FCREATEDATE 
                BETWEEN @FBEGINDATE  AND @FENDDATE ORDER BY FCREATEDATE";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@FBEGINDATE", DbType.DateTime));
                parameters.Add(new SqlParameter("@FENDDATE", DbType.DateTime));
                parameters[0].Value = beginDate;
                parameters[1].Value = endDate;
                string[] tableName = new string[] { "T1" };
                OperateResults result = DBAccessServiceHelper.ExcuteQuery(this.Context, sql, tableName, parameters.ToArray());
                if (result.IsSuccess)
                {
                    GridControl gvMain = this.View.GetControl<GridControl>(Const_CommonList.Main_GridControl);
                    DataTable dt = result.ResultData.Tables["T1"];
                    foreach (DataRow row in dt.Rows)
                    {
                        //去除尾0
                        row["FCOREDIAMETERORREAM"] = ObjectUtils.TrimEndZero(Convert.ToString(row["FCOREDIAMETERORREAM"]),false,true);
                        row["FDIAMETERORSLIDES"] = ObjectUtils.TrimEndZero(Convert.ToString(row["FDIAMETERORSLIDES"]), false, true);
                        row["FLENGTH"] = ObjectUtils.TrimEndZero(Convert.ToString(row["FLENGTH"]), false, true);
                    }
                    gvMain.DataSource = dt;
                }
            }
        }
        private bool Validate()
        {
            DateTime beginDate = this.View.GetValue<DateTime>(Const_CommonList.Query_FBeginDate);
            DateTime endDate = this.View.GetValue<DateTime>(Const_CommonList.Query_FEndDate);
            if (beginDate == null || beginDate == DateTime.MinValue)
            {
                ChaintMessageBox.Show("请选择查询开始日期！");
                return false;
            }
            if (endDate == null || endDate == DateTime.MinValue)
            {
                ChaintMessageBox.Show("请选择查询结束日期！");
                return false;
            }
            return true;
        }
        private void CreateStockAreaPlanColumns()
        {
            GridView gv_View = this.View.GetControl<GridView>(Const_CommonList.Main_GridView);
            GridColumn colFID = GridViewUtils.CreateGridColumn("主键", "FID", 50);
            colFID.Visible = false;
            gv_View.Columns.Add(colFID);
            gv_View.Columns.Add(GridViewUtils.CreateGridColumn("编码", "FNUMBER", 50));
            gv_View.Columns.Add(GridViewUtils.CreateGridColumn("名称", "FNAME", 50));
            gv_View.Columns.Add(GridViewUtils.CreateGridColumn("产品类型", "FPAPERTYPE", 100));
            gv_View.Columns.Add(GridViewUtils.CreateGridColumn("产品等级", "FPAPERGRADE", 100));
            gv_View.Columns.Add(GridViewUtils.CreateGridColumn("产品认证", "FCERTIFICATION", 100));
            gv_View.Columns.Add(GridViewUtils.CreateGridColumn("包装方式", "FPACKTYPE", 100));
            gv_View.Columns.Add(GridViewUtils.CreateGridColumn("夹板包装", "FTRANSPORTTYPE", 100));
            gv_View.Columns.Add(GridViewUtils.CreateGridColumn("产品色相", "FCOLOR", 100));
            gv_View.Columns.Add(GridViewUtils.CreateGridColumn("纸芯径", "FCOREDIAMETER", 70));
            gv_View.Columns.Add(GridViewUtils.CreateGridColumn("令数", "FREAM", 50));
            gv_View.Columns.Add(GridViewUtils.CreateGridColumn("直径", "FDIAMETER", 50));
            gv_View.Columns.Add(GridViewUtils.CreateGridColumn("令张数", "FSLIDES", 70));
            gv_View.Columns.Add(GridViewUtils.CreateGridColumn("线长", "FLENGTH", 50));
            gv_View.Columns.Add(GridViewUtils.CreateGridColumn("合格证样式", "FTRADEMARKSTYLE", 100));
            gv_View.Columns.Add(GridViewUtils.CreateGridColumn("产品专用", "FSPECPRODNAME", 100));
            gv_View.Columns.Add(GridViewUtils.CreateGridColumn("客户专用", "FSPECCUSTNAME", 100));
            gv_View.Columns.Add(GridViewUtils.CreateGridColumn("计重方式", "FWEIGHTMODE", 100));
            gv_View.Columns.Add(GridViewUtils.CreateGridColumn("规格", "FSPECIFICATION", 50));
            gv_View.Columns.Add(GridViewUtils.CreateGridColumn("产品备注", "FSPCUSTOMER", 50));
            gv_View.Columns.Add(GridViewUtils.CreateGridColumn("备注", "FMEMO", 50));
            gv_View.Columns.Add(GridViewUtils.CreateGridColumn("订单号", "FPONUMBER", 70));
            gv_View.Columns.Add(GridViewUtils.CreateGridColumn("交货日期", "FDELIVERDATE", 100));
            gv_View.Columns.Add(GridViewUtils.CreateGridColumn("创建日期", "FCREATEDATE", 100));
            gv_View.Columns.Add(GridViewUtils.CreateGridColumn("修改日期", "FMODIFYDATE", 100));
        }
        private void CreateStockAreaColumns()
        {
            GridView gv_View = this.View.GetControl<GridView>(Const_CommonList.Main_GridView);
            GridColumn colFID = GridViewUtils.CreateGridColumn("主键", "FID", 50);
            colFID.Visible = false;
            gv_View.Columns.Add(colFID);
            GridColumn colFEntryID = GridViewUtils.CreateGridColumn("分录主键", "FENTRYID", 50);
            colFEntryID.Visible = false;
            gv_View.Columns.Add(colFEntryID);
            GridColumn colFStock= GridViewUtils.CreateGridColumn("仓库", "FSTOCK", 50);
            colFStock.GroupIndex = 0;
            gv_View.Columns.Add(colFStock);
            gv_View.Columns.Add(GridViewUtils.CreateGridColumn("库区编码", "FSTOCKAREANUMBER", 100));
            gv_View.Columns.Add(GridViewUtils.CreateGridColumn("库区名称", "FSTOCKAREANAME", 100));
            gv_View.Columns.Add(GridViewUtils.CreateGridColumn("库区位置", "FLOCATION", 120));
            gv_View.Columns.Add(GridViewUtils.CreateGridColumn("库区总容量", "FTOTALCAPACITY", 120));
            gv_View.Columns.Add(GridViewUtils.CreateGridColumn("创建日期", "FCREATEDATE", 120));
            gv_View.Columns.Add(GridViewUtils.CreateGridColumn("修改日期", "FMODIFYDATE", 120));
        }

        private void CreateStockColumns()
        {
            GridView gv_View = this.View.GetControl<GridView>(Const_CommonList.Main_GridView);
            GridColumn colFID = GridViewUtils.CreateGridColumn("主键", "FID", 50);
            colFID.Visible = false;
            gv_View.Columns.Add(colFID);
            gv_View.Columns.Add(GridViewUtils.CreateGridColumn("仓库编码", "FNUMBER", 100));
            gv_View.Columns.Add(GridViewUtils.CreateGridColumn("仓库名称", "FNAME", 100));
            gv_View.Columns.Add(GridViewUtils.CreateGridColumn("位置", "FLOCATION", 200));
            gv_View.Columns.Add(GridViewUtils.CreateGridColumn("备注", "FMEMO", 120));
            gv_View.Columns.Add(GridViewUtils.CreateGridColumn("创建日期", "FCREATEDATE", 120));
            gv_View.Columns.Add(GridViewUtils.CreateGridColumn("修改日期", "FMODIFYDATE", 120));
        }
    }
}
