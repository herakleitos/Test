using System;
using System.Data;
using System.Data.SqlClient;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using Chaint.Common.Core;
using Chaint.Common.Core.Const;
using Chaint.Common.Core.Utils;
using Chaint.Common.BasePlugIn;
using Chaint.Common.Core.EventArgs;
using Chaint.Common.ServiceHelper;
using Chaint.Common.Entity.Utils;
using Chaint.Instock.Core;
using Chaint.Instock.Business.View;
using System.Windows.Forms;

namespace Chaint.Instock.Business.PlugIns
{
    public partial class StockAreaPlugIn : AbstractBillPlugIn
    {
        XtraForm form;
        public StockAreaPlugIn(StockAreaView view) : base(view)
        {
            form = this.View.GetControl<XtraForm>(Const_StockArea.Base_Form);
        }
        public override void OnSetBusinessInfo()
        {
            base.OnSetBusinessInfo();
            BindStock();
        }
        public override void OnBind()
        {
            base.OnBind();
            this.View.SetValue(Const_StockArea.Head_Field_FCreateDate, DateTime.Now);
            BindEntry();
            object data = this.Context.GetOption("FID");
            this.Context.RemoveOption("FID");
            if (data == null) return;
            DataSet resultData = (DataSet)data;
            BindEditData(resultData, true);
        }
        public override void ButtonClick(ButtonClickEventArgs e)
        {
            base.ButtonClick(e);
            switch (e.Sender)
            {
                case Const_StockArea.Entry_Menu_NewEntry:
                    var gridNew = this.View.GetControl<GridView>(Const_StockArea.Entry_Entry);
                    gridNew.AddNewRow();
                    break;
                case Const_StockArea.Entry_Menu_DeleteEntry:
                    var gridDel = this.View.GetControl<GridView>(Const_StockArea.Entry_Entry);
                    int row = gridDel.FocusedRowHandle;
                    gridDel.DeleteRow(row);
                    gridDel.UpdateCurrentRow();
                    break;
                case Const_StockArea.Head_Menu_Save:
                    SaveToDB();
                    break;
                case Const_StockArea.Head_Menu_List:
                    this.View.Open(Const_Option.Const_CommonList,Const_Option.Const_StockArea);
                    break;
                case Const_StockArea.Head_Menu_New:
                    RefreshPage();
                    break;
                case Const_StockArea.Head_Menu_Delete:
                    Delete();
                    break;
            }
        }

        public override void DataChanged(DataChangedEventArgs e)
        {
            base.DataChanged(e);
            switch (e.Sender)
            {
                case Const_StockArea.Head_Field_FStock:
                    BindData();
                    break;
            }
        }
        private void RefreshPage()
        {
            bool isNew = true;
            if (this.View.Model.IsDirty)
            {
                DialogResult result = ChaintMessageBox.ShowConfirmDialog("确定放弃当前数据么？");
                if (result != DialogResult.Yes)
                {
                    isNew = false;
                }
            }
            if (isNew)
            {
                BindStock();
                ClearEditData(isNew);
            }
        }
        private void Delete()
        {
            if (!this.View.Model.isSaved)
            {
                ChaintMessageBox.Show("当前数据未保存，无法删除!");
                return;
            }
            bool isDelete = true;
            DialogResult result = ChaintMessageBox.ShowConfirmDialog("确定删除当前数据么？");
            if (result != DialogResult.Yes)
            {
                isDelete = false;
            }
            if (isDelete)
            {
                OperationResult delResult =this.View.Model.Delete();
                if (!delResult.IsSuccess)
                {
                    ChaintMessageBox.Show("删除失败，请查看日志！");
                    return;
                }
                ChaintMessageBox.Show("删除成功！");
                BindStock();
                ClearEditData();
            }
        }
        private void BindData()
        {
            string stock = this.View.Model.GetValue<string>(Const_StockArea.Head_Field_FStock);
            if (stock.IsNullOrEmptyOrWhiteSpace())
            {
                ClearEditData();
                return;
            }
            string sql = @" SELECT * FROM T_AUTOSCAN_STOCKAREA WHERE FSTOCK =@FSTOCK1 ;
                            SELECT SAE.* FROM T_AUTOSCAN_STOCKAREAENTRY  SAE 
                            INNER JOIN T_AUTOSCAN_STOCKAREA SA ON SA.FID = SAE.FHEADID
                            WHERE SA.FSTOCK =@FSTOCK2 ORDER BY FSEQ;";
            SqlParameter para1 = new SqlParameter("@FSTOCK1", DbType.String);
            para1.Value = stock;
            SqlParameter para2 = new SqlParameter("@FSTOCK2", DbType.String);
            para2.Value = stock;
            SqlParameter[] parameters =
                new SqlParameter[] { para1, para2 };
            OperateResults result
                = DBAccessServiceHelper.ExcuteQuery(this.Context, sql, new string[] { "T1", "T2" }, parameters);
            if (!result.IsSuccess)
            {
                ChaintMessageBox.Show("发生错误，请查看日志!");
                return;
            }
            if (result.ResultData.Tables["T1"].Rows.Count <= 0)
            {
                ClearEditData();
                return;
            }
            BindEditData(result.ResultData, false);
        }

        public override void AfterDeleteEntryRow(AfterDeleteEntryRowEventArgs e)
        {
            base.AfterDeleteEntryRow(e);
        }
        public override void AfterCreateNewEntryRow(AfterCreateNewEntryRowEventArgs e)
        {
            base.AfterCreateNewEntryRow(e);
        }


        private void BindStock()
        {
            string sql = "SELECT FNUMBER,FNAME FROM T_AUTOSCAN_STOCK";
            OperateResult result =
                DBAccessServiceHelper.ExcuteQuery(this.Context, sql);
            if(!result.IsSuccess)
            {
                ChaintMessageBox.Show("发生错误，请查看日志!");
                return;
            }
            DataTable dtSource = new DataTable();
            dtSource.Columns.Add("FNUMBER", typeof(string));
            dtSource.Columns.Add("FNAME", typeof(string));
            dtSource.Columns["FNUMBER"].Caption = "编号";
            dtSource.Columns["FNAME"].Caption = "名称";
            foreach (var row in result.ResultTable)
            {
                DataRow dRow = dtSource.NewRow();
                dRow["FNUMBER"] = Convert.ToString(row["FNUMBER"]);
                dRow["FNAME"] = Convert.ToString(row["FNAME"]);
                dtSource.Rows.Add(dRow);
            }
            dtSource.AcceptChanges();
            DataSource ds = new DataSource();
            ds.DisplayMember = "FNAME";
            ds.ValueMember = "FNUMBER";
            ds.Data = dtSource;
            var field = this.View.GetControl<SearchLookUpEdit>(Const_StockArea.Head_Field_FStock);
            field.Bind(ds);
        }

        private void BindEntry()
        {
            DataTable dataSource = new DataTable();
            dataSource.Columns.Add(Const_StockArea.Entry_Column_FID, typeof(string));
            dataSource.Columns.Add(Const_StockArea.Entry_Column_FSeq, typeof(int));
            dataSource.Columns.Add(Const_StockArea.Entry_Column_FEntryId, typeof(string));
            dataSource.Columns.Add(Const_StockArea.Entry_Column_FStockAreaNumber, typeof(string));
            dataSource.Columns.Add(Const_StockArea.Entry_Column_FStockAreaName, typeof(string));
            dataSource.Columns.Add(Const_StockArea.Entry_Column_FLocation, typeof(string));
            dataSource.Columns.Add(Const_StockArea.Entry_Column_FTotalCapacity, typeof(int));
            dataSource.Columns.Add(Const_StockArea.Entry_Column_FMemo, typeof(string));
            this.View.GetControl<GridControl>("gc_Data").DataSource = dataSource;
            this.View.Model.BindEntryData(Const_StockArea.Entry_TableName, dataSource);
        }
        private void BindEditData(DataSet resultData,bool isFromList)
        {
            form.Text = "库区(修改)";
            DataRow row = resultData.Tables["T1"].Rows[0];
            this.View.SetValue(Const_StockArea.Head_Field_FID,
            row[Const_StockArea.Head_Column_FID]);
            if (isFromList)
            {
                this.View.SetValue(Const_StockArea.Head_Field_FStock,
                row[Const_StockArea.Head_Column_FStock]);
            }
            this.View.SetValue(Const_StockArea.Head_Field_FMemo,
            row[Const_StockArea.Head_Column_FMemo]);
                        this.View.SetValue(Const_StockArea.Head_Field_FCreateDate,
            row[Const_StockArea.Head_Column_FCreateDate]);
                        this.View.SetValue(Const_StockArea.Head_Field_FModifyDate,
            row[Const_StockArea.Head_Column_FModifyDate]);
            GridControl gcData
                = this.View.GetControl<GridControl>(Const_StockArea.Entry_Control);
            gcData.DataSource = resultData.Tables["T2"];
            this.View.Model.BindEntryData(Const_StockArea.Entry_TableName, resultData.Tables["T2"]);
            this.View.Model.isSaved = true;
            this.View.Model.IsDirty = false;
        }
        private void ClearEditData(bool isNew=false)
        {
            form.Text = "库区(新增)";
            this.View.SetValue(Const_StockArea.Head_Field_FID,
            SequenceGuid.NewGuid().ToString());
            if (isNew)
            {
                this.View.SetValue(Const_StockArea.Head_Field_FStock, string.Empty);
            }
            this.View.SetValue(Const_StockArea.Head_Field_FMemo,string.Empty);
            this.View.SetValue(Const_StockArea.Head_Field_FCreateDate,DateTime.Now);
            this.View.SetValue(Const_StockArea.Head_Field_FModifyDate, DateTime.Now);
            BindEntry();
            this.View.Model.isSaved = false;
            this.View.Model.IsDirty = false;
        }
    }
}
