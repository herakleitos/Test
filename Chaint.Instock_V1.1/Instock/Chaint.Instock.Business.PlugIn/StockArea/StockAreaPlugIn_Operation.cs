using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DevExpress.XtraGrid;
using DevExpress.DataAccess.Sql;
using Chaint.Instock.Core;
using Chaint.Common.Core;
using Chaint.Common.ServiceHelper;
using Chaint.Common.Core.Utils;
namespace Chaint.Instock.Business.PlugIns
{
    partial class StockAreaPlugIn
    {
        private void SaveToDB()
        {
            this.View.Model.SetValue(Const_StockArea.Head_Field_FModifyDate, DateTime.Now);
            if (!Validate()) return;
            OperationResult result = this.View.Model.Save();
            if (result.IsSuccess)
            {
                this.form.Text = "库区(修改)";
                DateTime modify =
                    this.View.Model.GetValue<DateTime>(Const_StockArea.Head_Field_FModifyDate);
                this.View.SetValue(Const_StockArea.Head_Field_FModifyDate, DateTime.Now);
                ChaintMessageBox.Show("保存成功!");
                BindStock();
            }
            else
            {
                ChaintMessageBox.Show("保存失败,请查看日志!");
            }
        }
        private bool Validate()
        {
            string stock = this.View.Model.GetValue<string>(Const_StockArea.Head_Field_FStock);
            if (stock.IsNullOrEmptyOrWhiteSpace())
            {
                ChaintMessageBox.Show("请输入仓库！");
                return false;
            }
            GridControl gcData = this.View.GetControl<GridControl>("gc_Data");
            DataTable dataSource = (DataTable)gcData.DataSource;
            dataSource.AcceptChanges();
            if (dataSource.Rows.Count <= 0)
            {
                ChaintMessageBox.Show("请至少录入一条分录！");
                return false;
            }
            Dictionary<string, int> addedNumber = new Dictionary<string, int>();
            Dictionary<string, string> entryIdMap = new Dictionary<string, string>();
            foreach (DataRow row in dataSource.Rows)
            {
                string entryid = row.GetValue<string>(Const_StockArea.Entry_Column_FEntryId);
                int seq = row.GetValue<int>(Const_StockArea.Entry_Column_FSeq);
                string stockAreaNumber = row.GetValue<string>(Const_StockArea.Entry_Column_FStockAreaNumber);
                if (stockAreaNumber.IsNullOrEmptyOrWhiteSpace())
                {
                    ChaintMessageBox.Show(string.Format("第{0}条分录，请输入库区编码!", seq));
                    return false;
                }
                if (addedNumber.Keys.Contains(stockAreaNumber))
                {
                    ChaintMessageBox.Show(string.Format("第{0}条分录中的库区编码【{1}】在第{2}条分录已存在!",seq, stockAreaNumber, addedNumber[stockAreaNumber]));
                    return false;
                }
                else
                {
                    addedNumber.Add(stockAreaNumber, seq);
                    entryIdMap.Add(stockAreaNumber, entryid);
                }
                string stockAreaName = row.GetValue<string>(Const_StockArea.Entry_Column_FStockAreaName);
                if (stockAreaName.IsNullOrEmptyOrWhiteSpace())
                {
                    ChaintMessageBox.Show(string.Format("第{0}条分录，请输入库区名称!", seq));
                    return false;
                }
            }
            return ValidateWithDB(stock,addedNumber, entryIdMap);
        }
        private bool ValidateWithDB(string stock,Dictionary<string, int> addedNumber,
            Dictionary<string, string> entryIdMap)
        {
            string sql = @" SELECT SA.FID,SAE.FENTRYID,SA.FSTOCK,SAE.FSTOCKAREANUMBER FROM T_AUTOSCAN_STOCKAREA SA 
                     INNER JOIN T_AUTOSCAN_STOCKAREAENTRY SAE ON SA.FID = SAE.FHEADID
                     WHERE SA.FSTOCK=@FSTOCK AND SAE.FSTOCKAREANUMBER IN ({0})";
            int i = 0;
            StringBuilder filter =new StringBuilder();
            char seperator = ',';
            List<QueryParameter> parameters = new List<QueryParameter>();
            parameters.Add(new QueryParameter("@FSTOCK", typeof(string), stock));
            foreach (string id in addedNumber.Keys)
            {
                i++;
                string parameterName =
                    string.Format("@{0}{1}", "FSTOCKAREANUMBER", i);
                filter.Append(parameterName);
                filter.Append(seperator);
                parameters.Add(new QueryParameter(parameterName,typeof(string), id));
            }
            sql = string.Format(sql, filter.ToString().TrimEnd(seperator));
            OperateResult result = DBAccessServiceHelper.ExcuteQuery(this.Context, sql, parameters.ToArray());
            if (!result.IsSuccess)
            {
                ChaintMessageBox.Show("发生错误，请查看日志!");
                return false;
            }
            if (result.ResultTable.Count() <= 0) return true;
            foreach (var item in result.ResultTable)
            {
                string number = item.GetValue<string>("FSTOCKAREANUMBER");
                string dbId = item.GetValue<string>("FENTRYID");
                string id = string.Empty;
                entryIdMap.TryGetValue(number,out id);
                if (id.IsNullOrEmptyOrWhiteSpace()) continue;
                if (id.Equals(dbId)) continue;
                int seq = 0;
                addedNumber.TryGetValue(number,out seq);
                ChaintMessageBox.Show(string.Format("第{0}条分录，库区编码【{1}】已存在于数据库中，请从库区列表中查看!", seq, number));
                return false;
            }
            return true;
        }
    }
}
