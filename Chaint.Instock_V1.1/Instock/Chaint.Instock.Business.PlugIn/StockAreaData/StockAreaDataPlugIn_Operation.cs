using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using DevExpress.DataAccess.Sql;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using Chaint.Instock.Core;
using Chaint.Common.Core;
using Chaint.Common.Core.Log;
using Chaint.Common.ServiceHelper;
using Chaint.Common.Core.Utils;
using Chaint.Common.Core.Sql;
using DevExpress.XtraReports.UI;
using Chaint.Common.Core.Const;
using System.Windows.Forms;

namespace Chaint.Instock.Business.PlugIns
{
    partial class StockAreaDataPlugIn
    {
        private string filePath = string.Empty;
        private void SaveToDB()
        {
            //做移库操作
            if (type == 4)
            {
                DoTrans();
                return;
            }
            if (!Validate()) return;
            if (type == 3)
            {
                if (!alterInfo.IsEmpty())
                {
                    if (!ValidateAmount()) return;
                }
            }
            OperationResult result = this.View.Model.Save();
            if (result.IsSuccess)
            {
                if (type == 3)
                {
                    GridControl gcData = this.View.GetControl<GridControl>(Const_StockAreaData.Entry_Control);
                    DataTable dataSource = (DataTable)gcData.DataSource;
                    if (dataSource.Rows.Count > 0)
                    {
                        LogData logData = new LogData();
                        foreach (DataRow row in dataSource.Rows)
                        {
                            string product = row.GetValue<string>(Const_StockAreaData.Entry_Column_FProduct);
                            string productName = string.Empty;
                            prdInfo.TryGetValue(product, out productName);
                            string entryId = row.GetValue<string>(Const_StockAreaData.Entry_Column_FEntryId);
                            if (!alterInfo.ContainsKey(entryId)) continue;
                            string stockArea =
                            row.GetValue<string>(Const_StockAreaData.Entry_Column_FStockArea);
                            int amount = row.GetValue<int>(Const_StockAreaData.Entry_Column_FAmount);
                            LogRecord record = new LogRecord();
                            record.Key = "库区数据修改";
                            record.Operator = this.Context.UserID;
                            record.SourceId = entryId;
                            record.SourceTableName = Const_StockAreaData.Entry_TableName;
                            record.Content = string.Format("产品{0}，库区{1}，件数修改为{2},操作员{3}",
                                productName, stockArea, amount, this.Context.UserName);
                            logData.AddRecord(record);
                        }
                        BatchInsertParam param = logData.ToBulkInsertObject();
                        OperateResult logResult =
                            DBAccessServiceHelper.BulkInsert(this.Context, param);
                    }
                }
                ChaintMessageBox.Show("保存成功!");
            }
            else
            {
                ChaintMessageBox.Show("保存失败!");
            }
        }

        private bool ValidateAmount()
        {
            string sql = "SELECT FENTRYID,FAMOUNT,FWEIGHT FROM T_AUTOSCAN_STOCKAREADATAENTRY WHERE FSTATUS=3 AND FENTRYID IN ({0})";
            StringBuilder filter = new StringBuilder();
            char seperator = ',';
            List<QueryParameter> parameters = new List<QueryParameter>();
            int i = 0;
            foreach (string entryid in alterInfo.Keys)
            {
                i++;
                string parameterProdName =
                    string.Format("@{0}{1}", "FENTRYID", i);
                filter.Append(parameterProdName);
                filter.Append(seperator);
                parameters.Add(new QueryParameter(parameterProdName, typeof(string), entryid));

            }
            sql = string.Format(sql, filter.ToString().TrimEnd(seperator));
            OperateResult result = DBAccessServiceHelper.ExcuteQuery(this.Context, sql, parameters.ToArray());
            if (!result.IsSuccess)
            {
                ChaintMessageBox.Show("发生错误，请重试！");
                return false;
            }
            foreach (var row in result.ResultTable)
            {
                string entryId = row.GetValue<string>("FENTRYID");
                int newAmount = row.GetValue<int>("FAMOUNT");
                decimal newWeight = row.GetValue<decimal>("FWEIGHT");
                if (entryId.IsNullOrEmptyOrWhiteSpace()) continue;
                int rowIndex = 0;
                if (!alterInfo.TryGetValue(entryId, out rowIndex)) continue;
                Tuple<int, decimal> oldAmount;
                if (!amountInfo.TryGetValue(entryId, out oldAmount)) continue;
                bool isVal = true;
                string message = string.Empty;
                if (newAmount != oldAmount.Item1)
                {
                    isVal = false;
                    message = "第{0}行的件数已经发生了改变，点击确定查询最新数据！";
                }
                else if (newWeight != oldAmount.Item2)
                {
                    isVal = false;
                    message = "第{0}行的重量已经发生了改变，点击确定查询最新数据！";
                }
                if (!isVal)
                {
                    DialogResult isRefresh = ChaintMessageBox.ShowOkDialog(string.Format(message, rowIndex + 1));
                    if (isRefresh == DialogResult.OK)
                    {
                        SetData();
                    }
                    return false;
                }
            }
            return true;
        }

        private void UpdateStockData()
        {
            DataTable dt = this.View.Model.GetBillEntryData(Const_StockAreaData.Entry_TableName);
            string sql = @"MERGE INTO T_AUTOSCAN_STOCKAREADATAENTRY SADE
                    USING (SELECT 
					SUM(SADIS.FAMOUNT) AS FAMOUNT,ISNULL(SUM(SADIS.FWEIGHT),0) AS FWEIGHT,
                    SADIS.FPRODUCT,SADIS.FSTOCK,SADIS.FSTOCKAREA,
					SADIS.FPAPERTYPE,SADIS.FSPECIFICATION,SADIS.FPAPERGRADE,SADIS.FCERTIFICATION,SADIS.FTRANSPORTTYPE,
					SADIS.FCOREDIAMETERORREAM,SADIS.FDIAMETERORSLIDES,SADIS.FLENGTH,SADIS.FTRADEMARKSTYLE,
					SADIS.FSPECCUSTNAME,SADIS.FSPECPRODNAME,SADIS.FPACKTYPE,SADIS.FDELIVERDATE,
                    SADIS.FWEIGHTMODE,SADIS.FSPCUSTOMER,
					SADIS.FCOLOR,SADIS.FPONUMBER,SADIS.FMEMO,SADIS.FCUSTOMER,SADIS.FSTOCKDATE
                    FROM T_AUTOSCAN_STOCKAREADATAENTRY SADIS
                    WHERE SADIS.FSTATUS=0 AND SADIS.FENTRYID IN ({0}) 
					GROUP BY SADIS.FPRODUCT,SADIS.FSTOCK,SADIS.FSTOCKAREA,
					SADIS.FPAPERTYPE,SADIS.FSPECIFICATION,SADIS.FPAPERGRADE,SADIS.FCERTIFICATION,SADIS.FTRANSPORTTYPE,
					SADIS.FCOREDIAMETERORREAM,SADIS.FDIAMETERORSLIDES,SADIS.FLENGTH,SADIS.FTRADEMARKSTYLE,
					SADIS.FSPECCUSTNAME,SADIS.FSPECPRODNAME,SADIS.FPACKTYPE,SADIS.FDELIVERDATE,SADIS.FWEIGHTMODE,SADIS.FSPCUSTOMER,
					SADIS.FCOLOR,SADIS.FPONUMBER,SADIS.FMEMO,SADIS.FCUSTOMER,SADIS.FSTOCKDATE) TEMP
                    ON (SADE.FPRODUCT =TEMP.FPRODUCT AND SADE.FSTOCK=TEMP.FSTOCK AND SADE.FSTOCKAREA = TEMP.FSTOCKAREA
					AND  TEMP.FPAPERTYPE = SADE.FPAPERTYPE AND  TEMP.FSPECIFICATION =SADE.FSPECIFICATION
					AND TEMP.FPAPERGRADE = SADE.FPAPERGRADE AND TEMP.FCERTIFICATION = SADE.FCERTIFICATION
					AND TEMP.FTRANSPORTTYPE = SADE.FTRANSPORTTYPE AND TEMP.FCOREDIAMETERORREAM =  SADE.FCOREDIAMETERORREAM 
					AND TEMP.FDIAMETERORSLIDES= SADE.FDIAMETERORSLIDES AND TEMP.FLENGTH =  SADE.FLENGTH 
					AND TEMP.FTRADEMARKSTYLE= SADE.FTRADEMARKSTYLE AND TEMP.FSPECCUSTNAME=  SADE.FSPECCUSTNAME 
					AND TEMP.FSPECPRODNAME = SADE.FSPECPRODNAME AND TEMP.FPACKTYPE = SADE.FPACKTYPE
					AND ISNULL(TEMP.FDELIVERDATE,'2017-10-26') = ISNULL(SADE.FDELIVERDATE,'2017-10-26') 
					AND TEMP.FWEIGHTMODE= SADE.FWEIGHTMODE AND TEMP.FSPCUSTOMER = SADE.FSPCUSTOMER 
					AND TEMP.FCOLOR= SADE.FCOLOR AND TEMP.FPONUMBER= SADE.FPONUMBER
					AND TEMP.FMEMO= SADE.FMEMO AND TEMP.FCUSTOMER= SADE.FCUSTOMER 
					AND TEMP.FSTOCKDATE= SADE.FSTOCKDATE
                    AND SADE.FSTATUS = 3)
                    WHEN MATCHED THEN
                    UPDATE SET SADE.FAMOUNT +=  TEMP.FAMOUNT,SADE.FWEIGHT +=  TEMP.FWEIGHT
					WHEN NOT MATCHED THEN
						INSERT (FENTRYID,FPRODUCT,FAMOUNT,FWEIGHT,FPLANAMOUNT,FSTATUS,FDATE,FSTOCK,FSTOCKAREA,
						FPAPERTYPE,FSPECIFICATION,FPAPERGRADE,FCERTIFICATION,FTRANSPORTTYPE,FCOREDIAMETERORREAM,FDIAMETERORSLIDES,
						FLENGTH,FTRADEMARKSTYLE,FSPECCUSTNAME,FSPECPRODNAME,FPACKTYPE,FDELIVERDATE,FWEIGHTMODE,FSPCUSTOMER,
						FCOLOR,FPONUMBER,FMEMO,FCUSTOMER,FSTOCKDATE)
                    VALUES(newid(),TEMP.FPRODUCT,TEMP.FAMOUNT,TEMP.FWEIGHT,0,3,GETDATE(),TEMP.FSTOCK,TEMP.FSTOCKAREA,
					TEMP.FPAPERTYPE,TEMP.FSPECIFICATION,TEMP.FPAPERGRADE,TEMP.FCERTIFICATION,TEMP.FTRANSPORTTYPE,
					TEMP.FCOREDIAMETERORREAM,TEMP.FDIAMETERORSLIDES,TEMP.FLENGTH,TEMP.FTRADEMARKSTYLE,
					TEMP.FSPECCUSTNAME,TEMP.FSPECPRODNAME,TEMP.FPACKTYPE,TEMP.FDELIVERDATE,TEMP.FWEIGHTMODE,TEMP.FSPCUSTOMER,
					TEMP.FCOLOR,TEMP.FPONUMBER,TEMP.FMEMO,TEMP.FCUSTOMER,TEMP.FSTOCKDATE);";
            List<SqlParameter> parameters = new List<SqlParameter>();
            string filter = GetFilterStringAndParameter(dt, parameters);
            ExcuteObject obj1 = new ExcuteObject();
            obj1.Parameters.AddRange(parameters);
            obj1.Sql = string.Format(sql, filter);
            List<ExcuteObject> excuteObjs = new List<ExcuteObject>();
            excuteObjs.Add(obj1);

            //将初始化的数据状态更新为-1，表示仅供查询，不可再次修改
            string updateStatus = @"UPDATE T_AUTOSCAN_STOCKAREADATAENTRY SET FSTATUS=-1 WHERE FENTRYID IN ({0})";
            ExcuteObject obj2 = new ExcuteObject();
            obj2.Parameters.AddRange(parameters);
            obj2.Sql = string.Format(updateStatus, filter);
            excuteObjs.Add(obj2);
            OperationResult opResult = DBAccessServiceHelper.ExcuteWithTransaction(this.Context, excuteObjs);
            if (opResult.IsSuccess)
            {
                ChaintMessageBox.Show("更新成功!");
                BindEntry();
            }
            else
            {
                ChaintMessageBox.Show("更新失败，请重试！");
            }
        }
        private string GetFilterStringAndParameter(DataTable dt, List<SqlParameter> sqlParameters = null,
          List<QueryParameter> queryParameters = null)
        {
            StringBuilder filterSb = new StringBuilder();
            List<QueryParameter> param = new List<QueryParameter>();
            int i = 0;
            char seperator = ',';
            foreach (DataRow row in dt.Rows)
            {
                i++;
                string fid = row.GetValue<string>(Const_StockAreaData.Entry_Column_FEntryId);
                string parameterName = string.Format("@{0}{1}", Const_StockAreaData.Entry_Column_FEntryId, i);
                filterSb.Append(parameterName);
                filterSb.Append(seperator);
                SqlParameter para = new SqlParameter(parameterName, DbType.String);
                para.Value = fid;
                if (sqlParameters != null)
                {
                    sqlParameters.Add(para);
                }
                if (queryParameters != null)
                {
                    queryParameters.Add(new QueryParameter(parameterName, typeof(string), fid));
                }
            }
            return filterSb.ToString().TrimEnd(seperator);
        }
        private void Complete()
        {
            GridControl gcData =
              this.View.GetControl<GridControl>(Const_StockAreaData.Entry_Control);
            gcData.RefreshDataSource();
            DataTable dt = (DataTable)gcData.DataSource;
            DataRow[] rows = dt.Select("FCHECK= 1 AND FSTATUS =1 ");
            if (rows.Length < 1)
            {
                ChaintMessageBox.Show("请至少选择一行未强制完成的数据进行操作!");
                return;
            }
            List<string> entryids = rows.Select(s => s.GetValue<string>(Const_StockAreaData.Entry_Column_FEntryId)).ToList();

            UpdateObject upObj = new UpdateObject();
            upObj.TableName = Const_StockAreaData.Entry_TableName;
            upObj.AddUpdateItem(new UpdateItem(Const_StockAreaData.Entry_Column_FStatus,
                string.Format("@{0}", Const_StockAreaData.Entry_Column_FStatus), 2, Common.Core.Enums.Enums_FieldType.Int32));
            WhereItem whereItem = new WhereItem(Const_StockAreaData.Entry_Column_FEntryId, Common.Core.Enums.Enums_FieldType.String);
            int i = 0;
            foreach (string entryid in entryids)
            {
                i++;
                SqlParam para = new SqlParam(string.Format("@{0}{1}", Const_StockAreaData.Entry_Column_FEntryId,i),
                    entryid, Common.Core.Enums.Enums_FieldType.String);
                whereItem.InOption.Add(para);
            }
            upObj.AddWhereItem(whereItem);
            string sql = upObj.ToSqlString();
            OperateResult opResult = DBAccessServiceHelper.ExcuteNonQuery(this.Context, sql, upObj.QueryParameters.ToArray());
            if (opResult.IsSuccess)
            {
                ChaintMessageBox.Show("强制完成成功!");
                SetData();
            }
            else
            {
                ChaintMessageBox.Show("强制完成失败，请重试!");
            }
        }

        private void DoTrans()
        {
            GridControl gcData = this.View.GetControl<GridControl>(Const_StockAreaData.Entry_Control);
            DataTable dataSource = (DataTable)gcData.DataSource;
            dataSource.AcceptChanges();
            List<string> ids = new List<string>();
            if (dataSource.Rows.Count <= 0) return;
            Dictionary<string, Tuple<string, string, int, decimal>> processingData =
                new Dictionary<string, Tuple<string, string, int, decimal>>();
            LogData logData = new LogData();
            foreach (DataRow row in dataSource.Rows)
            {
                int seq = dataSource.Rows.IndexOf(row) + 1;
                string entryId = row.GetValue<string>(Const_StockAreaData.Entry_Column_FEntryId);
                string inStockArea =
                    row.GetValue<string>(Const_StockAreaData.Entry_Column_FInStockArea);
                string inStock = string.Empty;
                stockAreaToStockDic.TryGetValue(inStockArea, out inStock);
                string stockArea =
                row.GetValue<string>(Const_StockAreaData.Entry_Column_FStockArea);
                int outAmount = row.GetValue<int>(Const_StockAreaData.Entry_Column_FOutAmount);
                int amount = row.GetValue<int>(Const_StockAreaData.Entry_Column_FAmount);
                decimal weight = row.GetValue<int>(Const_StockAreaData.Entry_Column_FWeight);
                decimal outWeight = row.GetValue<int>(Const_StockAreaData.Entry_Column_FOutWeight);
                if (inStockArea.Equals(stockArea))
                {
                    ChaintMessageBox.Show(string.Format("第{0}条分录，移出仓库不能和移入仓库相同!", seq));
                    return;
                }
                if (outAmount > amount)
                {
                    ChaintMessageBox.Show(string.Format("第{0}条分录，移出件数大于当前库区的产品总件数!", seq));
                    return;
                }
                if (outWeight > weight)
                {
                    ChaintMessageBox.Show(string.Format("第{0}条分录，移出重量大于当前库区的产品总重量!", seq));
                    return;
                }
                if (inStockArea.IsNullOrEmptyOrWhiteSpace() || outAmount <= 0) continue;
                if (processingData.Keys.Contains(entryId)) continue;
                Tuple<string, string, int, decimal> item = new Tuple<string, string, int, decimal>(inStock, inStockArea, outAmount, outWeight);
                processingData.Add(entryId, item);
                string product = row.GetValue<string>(Const_StockAreaData.Entry_Column_FProduct);
                string productName = string.Empty;
                prdInfo.TryGetValue(product, out productName);
                LogRecord record = new LogRecord();
                record.Key = "移库";
                record.Operator = this.Context.UserID;
                record.SourceId = entryId;
                record.SourceTableName = Const_StockAreaData.Entry_TableName;
                record.Content = string.Format("产品{0}，移出库区{1}，总件数{2}，移出件数{3}，移入库区{4},操作员{5}",
                    productName, stockArea, amount, outAmount, inStockArea, this.Context.UserName);
                logData.AddRecord(record);
            }
            if (processingData.IsEmpty())
            {
                ChaintMessageBox.Show("当前没有需要移库的数据!");
                return;
            }
            List<ExcuteObject> excuteObjs = new List<ExcuteObject>();
            foreach (string key in processingData.Keys)
            {
                string entryId = key;
                string stock = processingData[key].Item1;
                string stockArea = processingData[key].Item2;
                int outAmount = processingData[key].Item3;
                decimal outWeight = processingData[key].Item4;
                string sql1 = @"UPDATE T_AUTOSCAN_STOCKAREADATAENTRY SET FAMOUNT = FAMOUNT-@FOUTAMOUNT,
                    FWEIGHT = FWEIGHT-@FOUTWEIGHT
                    WHERE FENTRYID=@FENTRYID;";
                string sql2 = @"MERGE INTO T_AUTOSCAN_STOCKAREADATAENTRY SADE
                    USING(SELECT
                    SADE2.FPRODUCT, SADE2.FSTOCK, SADE2.FSTOCKAREA,
                    SADE2.FPAPERTYPE, SADE2.FSPECIFICATION, SADE2.FPAPERGRADE, SADE2.FCERTIFICATION, SADE2.FTRANSPORTTYPE,
                    SADE2.FCOREDIAMETERORREAM, SADE2.FDIAMETERORSLIDES, SADE2.FLENGTH, SADE2.FTRADEMARKSTYLE,
                    SADE2.FSPECCUSTNAME, SADE2.FSPECPRODNAME, SADE2.FPACKTYPE, SADE2.FDELIVERDATE, 
                    SADE2.FWEIGHTMODE,SADE2.FSPCUSTOMER,
                    SADE2.FCOLOR, SADE2.FPONUMBER, SADE2.FMEMO, SADE2.FCUSTOMER, SADE2.FSTOCKDATE
                    FROM T_AUTOSCAN_STOCKAREADATAENTRY SADE2
                    WHERE SADE2.FENTRYID = @FENTRYID  AND SADE2.FSTATUS = 3
                    ) TEMP
                    ON (SADE.FPRODUCT = TEMP.FPRODUCT
                    AND  TEMP.FPAPERTYPE = SADE.FPAPERTYPE AND  TEMP.FSPECIFICATION = SADE.FSPECIFICATION
                    AND TEMP.FPAPERGRADE = SADE.FPAPERGRADE AND TEMP.FCERTIFICATION = SADE.FCERTIFICATION
                    AND TEMP.FTRANSPORTTYPE = SADE.FTRANSPORTTYPE AND TEMP.FCOREDIAMETERORREAM = SADE.FCOREDIAMETERORREAM
                    AND TEMP.FDIAMETERORSLIDES = SADE.FDIAMETERORSLIDES AND TEMP.FLENGTH = SADE.FLENGTH
                    AND TEMP.FTRADEMARKSTYLE = SADE.FTRADEMARKSTYLE AND TEMP.FSPECCUSTNAME = SADE.FSPECCUSTNAME
                    AND TEMP.FSPECPRODNAME = SADE.FSPECPRODNAME AND TEMP.FPACKTYPE = SADE.FPACKTYPE
                    AND ISNULL(TEMP.FDELIVERDATE,'2017-10-26') = ISNULL(SADE.FDELIVERDATE,'2017-10-26') 
                    AND TEMP.FWEIGHTMODE = SADE.FWEIGHTMODE AND TEMP.FSPCUSTOMER = SADE.FSPCUSTOMER
                    AND TEMP.FCOLOR = SADE.FCOLOR AND TEMP.FPONUMBER = SADE.FPONUMBER
                    AND TEMP.FMEMO = SADE.FMEMO AND TEMP.FCUSTOMER = SADE.FCUSTOMER
                    AND TEMP.FSTOCKDATE = SADE.FSTOCKDATE
                    AND SADE.FSTATUS = 3 AND SADE.FSTOCKAREA = @FSTOCKAREA AND SADE.FSTOCK = @FSTOCK)
                    WHEN MATCHED THEN
                    UPDATE SET SADE.FAMOUNT += @FOUTAMOUNT,FWEIGHT+=@FOUTWEIGHT,FDATE=GETDATE()
                    WHEN NOT MATCHED THEN
                    INSERT(FENTRYID, FPRODUCT, FAMOUNT,FWEIGHT, FPLANAMOUNT, FSTATUS, FDATE,FSTOCK,FSTOCKAREA,
                    FPAPERTYPE, FSPECIFICATION, FPAPERGRADE, FCERTIFICATION, FTRANSPORTTYPE, FCOREDIAMETERORREAM, FDIAMETERORSLIDES,
                    FLENGTH, FTRADEMARKSTYLE, FSPECCUSTNAME, FSPECPRODNAME, FPACKTYPE, FDELIVERDATE, FWEIGHTMODE,FSPCUSTOMER,
                    FCOLOR, FPONUMBER, FMEMO, FCUSTOMER, FSTOCKDATE)
                    VALUES(newid(), TEMP.FPRODUCT, @FOUTAMOUNT,@FOUTWEIGHT, 0, 3, GETDATE(),@FSTOCK,@FSTOCKAREA,
                    TEMP.FPAPERTYPE, TEMP.FSPECIFICATION, TEMP.FPAPERGRADE, TEMP.FCERTIFICATION, TEMP.FTRANSPORTTYPE,
                    TEMP.FCOREDIAMETERORREAM, TEMP.FDIAMETERORSLIDES, TEMP.FLENGTH, TEMP.FTRADEMARKSTYLE,
                    TEMP.FSPECCUSTNAME, TEMP.FSPECPRODNAME, TEMP.FPACKTYPE, TEMP.FDELIVERDATE, TEMP.FWEIGHTMODE,TEMP.FSPCUSTOMER,
                    TEMP.FCOLOR, TEMP.FPONUMBER, TEMP.FMEMO, TEMP.FCUSTOMER, TEMP.FSTOCKDATE); ";
                SqlParameter para1 = new SqlParameter("@FOUTAMOUNT", SqlDbType.Int);
                para1.Value = outAmount;
                SqlParameter para2 = new SqlParameter("@FOUTWEIGHT", SqlDbType.Decimal);
                para2.Value = outWeight;
                SqlParameter para3 = new SqlParameter("@FENTRYID", SqlDbType.VarChar);
                para3.Value = entryId;
                SqlParameter para4 = new SqlParameter("@FSTOCKAREA", SqlDbType.VarChar);
                para4.Value = stockArea;
                SqlParameter para5 = new SqlParameter("@FSTOCK", SqlDbType.VarChar);
                para5.Value = stock;
                ExcuteObject obj1 = new ExcuteObject();
                obj1.Sql = sql1;
                obj1.Parameters.Add(para1);
                obj1.Parameters.Add(para2);
                obj1.Parameters.Add(para3);
                excuteObjs.Add(obj1);

                ExcuteObject obj2 = new ExcuteObject();
                obj2.Sql = sql2;
                obj2.Parameters.Add(para1);
                obj2.Parameters.Add(para2);
                obj2.Parameters.Add(para3);
                obj2.Parameters.Add(para4);
                obj2.Parameters.Add(para5);
                excuteObjs.Add(obj2);
            }
            OperationResult opResult = DBAccessServiceHelper.ExcuteWithTransaction(this.Context, excuteObjs);
            if (opResult.IsSuccess)
            {
                ChaintMessageBox.Show("移库成功!");
                BatchInsertParam param = logData.ToBulkInsertObject();
                OperateResult logResult =
                    DBAccessServiceHelper.BulkInsert(this.Context, param);
                SetData();
            }
            else
            {
                ChaintMessageBox.Show("移库失败，请重试!");
            }
        }
        private bool Validate()
        {

            GridControl gcData = this.View.GetControl<GridControl>(Const_StockAreaData.Entry_Control);
            DataTable dataSource = (DataTable)gcData.DataSource;
            dataSource.AcceptChanges();
            if (dataSource.Rows.Count <= 0)
            {
                ChaintMessageBox.Show("请至少录入一条分录！");
                return false;
            }
            Dictionary<string, Dictionary<string, int>> container = new Dictionary<string, Dictionary<string, int>>();
            Dictionary<string, Dictionary<string, string>> entryIdMap = new Dictionary<string, Dictionary<string, string>>();
            foreach (DataRow row in dataSource.Rows)
            {
                string entryId = row.GetValue<string>(Const_StockAreaData.Entry_Column_FEntryId);
                int rowIndex = dataSource.Rows.IndexOf(row);
                int seq = dataSource.Rows.IndexOf(row) + 1;
                string name = row.GetValue<string>(Const_StockAreaData.Entry_Column_FName);
                if (name.IsNullOrEmptyOrWhiteSpace() && type == 1)
                {
                    ChaintMessageBox.Show(string.Format("第{0}条分录，请输入计划名称!", seq));
                    return false;
                }
                string product = row.GetValue<string>(Const_StockAreaData.Entry_Column_FProduct);
                if (product.IsNullOrEmptyOrWhiteSpace())
                {
                    ChaintMessageBox.Show(string.Format("第{0}条分录，请输入产品!", seq));
                    return false;
                }
                string stock = row.GetValue<string>(Const_StockAreaData.Entry_Column_FStock);
                if (stock.IsNullOrEmptyOrWhiteSpace())
                {
                    ChaintMessageBox.Show(string.Format("第{0}条分录，请输入仓库!", seq));
                    return false;
                }
                string stockArea = row.GetValue<string>(Const_StockAreaData.Entry_Column_FStockArea);
                if (stockArea.IsNullOrEmptyOrWhiteSpace())
                {
                    ChaintMessageBox.Show(string.Format("第{0}条分录，请输入库区!", seq));
                    return false;
                }
                if (type == 1)
                {
                    if (container.Keys.Contains(product))
                    {
                        if (container[product].Keys.Contains(name))
                        {
                            int oldSeq = container[product][name];
                            ChaintMessageBox.Show(string.Format("第{0}条分录中的产品【{1}】，计划名称【{2}】在第{3}条分录中已存在，请检查修改!", seq, product, name, oldSeq));
                            return false;
                        }
                        else
                        {
                            container[product].Add(name, seq);
                            entryIdMap[product].Add(name, entryId);
                        }
                    }
                    else
                    {
                        Dictionary<string, int> item = new Dictionary<string, int>();
                        item.Add(name, seq);
                        container.Add(product, item);

                        Dictionary<string, string> itemMap = new Dictionary<string, string>();
                        itemMap.Add(name, entryId);
                        entryIdMap.Add(product, itemMap);
                    }
                }
                if (type == 1)
                {
                    int amount = row.GetValue<int>(Const_StockAreaData.Entry_Column_FPlanAmount);
                    if (amount <= 0)
                    {
                        ChaintMessageBox.Show(string.Format("第{0}条分录，计划件数必须大于0!", seq));
                        return false;
                    }
                }
                else if (type == 0)
                {
                    int amount = row.GetValue<int>(Const_StockAreaData.Entry_Column_FAmount);
                    if (amount <= 0)
                    {
                        ChaintMessageBox.Show(string.Format("第{0}条分录，件数必须大于0!", seq));
                        return false;
                    }
                }
            }
            return ValidateWithDB(container, entryIdMap);
        }
        private bool ValidateWithDB(Dictionary<string, Dictionary<string, int>> container,
            Dictionary<string, Dictionary<string, string>> entryIdMap)
        {
            if (container.IsEmpty()) return true;
            string sql = @" SELECT SADE.FENTRYID,SADE.FPRODUCT,SADE.FNAME
                            FROM T_AUTOSCAN_STOCKAREADATAENTRY SADE
                            WHERE SADE.FPRODUCT IN ({0}) and SADE.FNAME IN ({1}); ";
            int i = 0;
            StringBuilder filterProd = new StringBuilder();
            StringBuilder filterPlan = new StringBuilder();
            char seperator = ',';
            List<QueryParameter> parameters = new List<QueryParameter>();
            foreach (string product in container.Keys)
            {
                string parameterProdName =
                    string.Format("@{0}{1}", "FPRODUCT", i);
                filterProd.Append(parameterProdName);
                filterProd.Append(seperator);
                parameters.Add(new QueryParameter(parameterProdName, typeof(string), product));
                foreach (string name in container[product].Keys)
                {
                    i++;
                    string parameterPlanName =
                        string.Format("@{0}{1}", "FNAME", i);
                    filterPlan.Append(parameterPlanName);
                    filterPlan.Append(seperator);
                    parameters.Add(new QueryParameter(parameterPlanName, typeof(string), name));
                }
            }
            sql = string.Format(sql, filterProd.ToString().TrimEnd(seperator), filterPlan.ToString().TrimEnd(seperator));
            OperateResult result = DBAccessServiceHelper.ExcuteQuery(this.Context, sql, parameters.ToArray());
            if (!result.IsSuccess)
            {
                ChaintMessageBox.Show("发生错误，请重试!");
                return false;
            }
            if (result.ResultTable.Count() <= 0) return true;
            foreach (var item in result.ResultTable)
            {
                string product = Convert.ToString(item["FPRODUCT"]);
                string name = Convert.ToString(item["FNAME"]);
                string dbId = Convert.ToString(item["FENTRYID"]);
                if (container.Keys.Contains(product))
                {
                    if (container[product].Keys.Contains(name) && type == 1)
                    {
                        string id = entryIdMap[product][name];
                        if (id.IsNullOrEmptyOrWhiteSpace()) continue;
                        if (id.Equals(dbId)) continue;
                        int seq = container[product][name];
                        ChaintMessageBox.Show(string.Format(@"第{0}条分录,产品【{1}】，计划名称【{2}】已存在，请修改!", seq, product, name));
                        return false;
                    }
                }
            }
            return true;
        }
        private void SetPrintTemplet()
        {

            DataTable dt = GetDataSource();
            ReportDesigner rptDesigner = new ReportDesigner(dt, "模板设计");
            rptDesigner.ShowDesigner();
        }
        private void Print()
        {
            try
            {
                if (filePath.IsNullOrEmptyOrWhiteSpace())
                {
                    ChaintMessageBox.Show("请选择打印模板!");
                    return;
                }
                XtraReport rpt = XtraReport.FromFile(filePath, true);
                rpt.DataSource = GetDataSource();
                DevExpress.XtraReports.UI.ReportPrintTool tool = new DevExpress.XtraReports.UI.ReportPrintTool(rpt);
                tool.AutoShowParametersPanel = false;
                rpt.RequestParameters = false;
                tool.Print();
                tool.Dispose();
                rpt.Dispose();
                rpt = null;
                tool = null;
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                ChaintMessageBox.Show("发生错误，请重试！");
            }
        }
        private void PrintPreview()
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Title = "选择打印模板";
                dialog.Filter = "打印模板(*.repx)|*.repx";
                dialog.InitialDirectory = Const_TempletFilePath.DefaultReportTempletFolder;
                DialogResult result = dialog.ShowDialog(form);
                if (result == DialogResult.OK)
                {
                    filePath = dialog.FileName;
                    XtraReport rpt = XtraReport.FromFile(filePath, true);
                    rpt.DataSource = GetDataSource();
                    DevExpress.XtraReports.UI.ReportPrintTool tool = new DevExpress.XtraReports.UI.ReportPrintTool(rpt);
                    tool.AutoShowParametersPanel = false;
                    rpt.RequestParameters = false;
                    tool.ShowPreviewDialog();
                    tool.Dispose();
                    rpt.Dispose();
                    rpt = null;
                    tool = null;
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                ChaintMessageBox.Show("发生错误，请重试！");
            }
        }
        private DataTable GetDataSource()
        {
            DataTable dt = this.View.Model.GetBillEntryData(Const_StockAreaData.Entry_TableName);
            DataTable result = dt.Copy();
            result.Columns.Remove(Const_StockAreaData.Entry_Column_FCustomer);
            result.Columns.Remove(Const_StockAreaData.Entry_Column_FStatus);
            result.Columns.Remove(Const_StockAreaData.Entry_Column_FEntryId);
            result.Columns.Remove(Const_StockAreaData.Entry_Column_FAmount);
            result.Columns.Remove(Const_StockAreaData.Entry_Column_FStockAreaPlan);
            result.Columns.Remove(Const_StockAreaData.Entry_Column_FDate);
            result.Columns.Remove(Const_StockAreaData.Entry_Column_FOutAmount);
            result.Columns.Remove(Const_StockAreaData.Entry_Column_FWeight);
            result.Columns.Remove(Const_StockAreaData.Entry_Column_FOutWeight);
            result.Columns.Remove(Const_StockAreaData.Entry_Column_FInStockArea);
            result.Columns.Remove(Const_StockAreaData.Entry_Column_FUsedCapacity);
            result.Columns.Remove(Const_StockAreaData.Entry_Column_FTotalCapacity);
            result.Columns.Remove(Const_StockAreaData.Entry_Column_FStockDate);
            result.Columns[Const_StockAreaData.Entry_Column_FName].Caption = "计划名称";
            result.Columns[Const_StockAreaData.Entry_Column_FProduct].Caption = "产品名称";
            result.Columns[Const_StockAreaData.Entry_Column_FStock].Caption = "仓库";
            result.Columns[Const_StockAreaData.Entry_Column_FStockArea].Caption = "库区";
            result.Columns[Const_StockAreaData.Entry_Column_FPaperType].Caption = "产品类型";
            result.Columns[Const_StockAreaData.Entry_Column_FPlanAmount].Caption = "计划件数";
            result.Columns[Const_StockAreaData.Entry_Column_FSpecification].Caption = "规格";
            result.Columns[Const_StockAreaData.Entry_Column_FPaperGrade].Caption = "产品等级";
            result.Columns[Const_StockAreaData.Entry_Column_FCertification].Caption = "产品认证";
            result.Columns[Const_StockAreaData.Entry_Column_FTransportType].Caption = "夹板包装";
            result.Columns[Const_StockAreaData.Entry_Column_FCoreDiameterOrReam].Caption = "纸芯/令数";
            result.Columns[Const_StockAreaData.Entry_Column_FDiameterOrSlides].Caption = "直径/令张数";
            result.Columns[Const_StockAreaData.Entry_Column_FLength].Caption = "线长/张数";
            result.Columns[Const_StockAreaData.Entry_Column_FTrademarkStyle].Caption = "合格证样式";
            result.Columns[Const_StockAreaData.Entry_Column_FSpecCustName].Caption = "客户专用";
            result.Columns[Const_StockAreaData.Entry_Column_FSpecProdName].Caption = "产品专用";
            result.Columns[Const_StockAreaData.Entry_Column_FPackType].Caption = "包装方式";
            result.Columns[Const_StockAreaData.Entry_Column_FDeliverDate].Caption = "交货日期";
            result.Columns[Const_StockAreaData.Entry_Column_FWeightMode].Caption = "计重方式";
            result.Columns[Const_StockAreaData.Entry_Column_FColor].Caption = "色相";
            result.Columns[Const_StockAreaData.Entry_Column_FPoNumber].Caption = "订单号";
            result.Columns[Const_StockAreaData.Entry_Column_FMemo].Caption = "备注";
            foreach (DataRow row in result.Rows)
            {
                string productId = row.GetValue<string>(Const_StockAreaData.Entry_Column_FProduct);
                if (productId == null) continue;
                string productName = string.Empty;
                prdInfo.TryGetValue(productId, out productName);
                row[Const_StockAreaData.Entry_Column_FProduct] = productName;

                string stock = row.GetValue<string>(Const_StockAreaData.Entry_Column_FStock);
                string stockName = string.Empty;
                stockInfo.TryGetValue(stock, out stockName);
                row[Const_StockAreaData.Entry_Column_FStock] = stockName;
            }
            return result;
        }
        private void ExportToExcel()
        {
            var gridControl = this.View.GetControl<GridControl>(Const_StockAreaData.Entry_Control);
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Title = "导出Excel";
            dialog.Filter = "Excel文件(*.xls)|*.xls";
            DialogResult dialogResult = dialog.ShowDialog(form);
            if (dialogResult == DialogResult.OK)
            {
                DevExpress.XtraPrinting.XlsExportOptions options = new DevExpress.XtraPrinting.XlsExportOptions();
                gridControl.ExportToXls(dialog.FileName);
                DevExpress.XtraEditors.XtraMessageBox.Show("保存成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        /// <summary>
        /// 复制行
        /// </summary>
        private void CopyToNewRow()
        {
            int row = this.View.GetSelectedRowIndex(Const_StockAreaData.Entry_Entry);
            var gridControl = this.View.GetControl<GridControl>(Const_StockAreaData.Entry_Control);
            DataTable dataSource = (DataTable)gridControl.DataSource;
            if (row < 0)
            {
                ChaintMessageBox.Show("请选中一条分录进行复制！");
                return;
            }
            DataRow dataRow = dataSource.Rows[row];
            DataRow newRow = dataSource.NewRow();
            newRow.ItemArray = dataRow.ItemArray.Clone<object[]>();
            newRow[Const_StockAreaData.Entry_Column_FEntryId] = SequenceGuid.NewGuid().ToString();
            dataSource.Rows.Add(newRow);
            dataSource.AcceptChanges();
            gridControl.DataSource = dataSource;
            this.View.Model.BindEntryData(Const_StockAreaData.Entry_TableName, dataSource);
        }
    }
}
