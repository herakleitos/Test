using System;
using DevExpress.XtraEditors;
using Chaint.Common.Core;
using Chaint.Common.Core.Const;
using Chaint.Common.BasePlugIn;
using Chaint.Instock.Core;
using Chaint.Common.Core.Utils;
using Chaint.Common.Core.EventArgs;
using Chaint.Instock.Business.View;
using System.Windows.Forms;
using System.Data;
namespace Chaint.Instock.Business.PlugIns
{
    public class StockPlugIn : AbstractBillPlugIn
    {
        XtraForm form;
        public StockPlugIn(StockView view) : base(view)
        {
            form = this.View.GetControl<XtraForm>(Const_Stock.Base_Form);
        }
        public override void OnBind()
        {
            base.OnBind();
            this.View.SetValue(Const_Stock.Head_Field_FCreateDate,DateTime.Now);
            //如果是从列表打开的，绑定界面数据
            BindEditData();
        }
        public override void ButtonClick(ButtonClickEventArgs e)
        {
            base.ButtonClick(e);
            switch (e.Sender)
            {
                case Const_StockAreaPlan.Head_Menu_Save:
                    SaveToDB();
                    break;
                case Const_StockAreaPlan.Head_Menu_New:
                    RefreshPage();
                    break;
                case Const_StockAreaPlan.Head_Menu_Delete:
                    Delete();
                    break;
                case Const_StockAreaPlan.Head_Menu_List:
                    this.View.Open(Const_Option.Const_CommonList, Const_Option.Const_Stock);
                    break;
            }
        }

        #region fun
        /// <summary>
        ///保存数据到数据库
        /// </summary>
        private void SaveToDB()
        {
            this.View.Model.SetValue(Const_Stock.Head_Field_FModifyDate, DateTime.Now);
            if (!Validate()) return;
            OperationResult result = this.View.Model.Save();
            if (result.IsSuccess)
            {
                form.Text = "仓库(修改)";
                DateTime modify =
                    this.View.Model.GetValue<DateTime>(Const_Stock.Head_Field_FModifyDate);
                this.View.SetValue(Const_Stock.Head_Field_FModifyDate, modify);
                this.View.Model.IsDirty = false;
                ChaintMessageBox.Show("保存成功！");
            }
            else
            {
                ChaintMessageBox.Show("保存失败,请查看日志!");
            }
        }
        //保存之前校验
        private bool Validate()
        {
            string fnumber =
                this.View.Model.GetValue<string>(Const_Stock.Head_Field_FNumber);
            if (fnumber.IsNullOrEmptyOrWhiteSpace())
            {
                ChaintMessageBox.Show("请输入编码！");
                return false;
            }
            string fname =
                this.View.Model.GetValue<string>(Const_Stock.Head_Field_FName);
            if (fname.IsNullOrEmptyOrWhiteSpace())
            {
                ChaintMessageBox.Show("请输入名称！");
                return false;
            }
            return true;
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
                ClearEditData();
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
                OperationResult delResult = this.View.Model.Delete();
                if (!delResult.IsSuccess)
                {
                    ChaintMessageBox.Show("删除失败，请查看日志！");
                    return;
                }
                ChaintMessageBox.Show("删除成功！");
                ClearEditData();
            }
        }
        private void ClearEditData()
        {
            form.Text = "仓库(新增)";
            this.View.SetValue(Const_Stock.Head_Field_FID,
            SequenceGuid.NewGuid().ToString());
            this.View.SetValue(Const_Stock.Head_Field_FNumber, string.Empty);
            this.View.SetValue(Const_Stock.Head_Field_FName, string.Empty);
            this.View.SetValue(Const_Stock.Head_Field_FLocation, string.Empty);
            this.View.SetValue(Const_Stock.Head_Field_FMemo, string.Empty);
            this.View.SetValue(Const_Stock.Head_Field_FCreateDate, DateTime.Now);
            this.View.SetValue(Const_Stock.Head_Field_FModifyDate, null);
            this.View.Model.isSaved = false;
            this.View.Model.IsDirty = false;
        }
        private void BindEditData()
        {
            object data = this.Context.GetOption("FID");
            this.Context.RemoveOption("FID");
            if (data == null) return;
            DataSet resultData = (DataSet)data;
            form.Text = "仓库(修改)";
            DataRow row = resultData.Tables["T1"].Rows[0];
            this.View.SetValue(Const_Stock.Head_Field_FID,
            row[Const_Stock.Head_Column_FID]);
            this.View.SetValue(Const_Stock.Head_Field_FNumber,
            row[Const_Stock.Head_Column_FNumber]);
            this.View.SetValue(Const_Stock.Head_Field_FName,
            row[Const_Stock.Head_Column_FName]);
            this.View.SetValue(Const_Stock.Head_Field_FLocation,
            row[Const_Stock.Head_Column_FLocation]);
            this.View.SetValue(Const_Stock.Head_Field_FMemo,
            row[Const_Stock.Head_Column_FMemo]);
            this.View.SetValue(Const_Stock.Head_Field_FCreateDate,
            row[Const_Stock.Head_Column_FCreateDate]);
            this.View.SetValue(Const_Stock.Head_Field_FModifyDate,
            row[Const_Stock.Head_Column_FModifyDate]);
            this.View.Model.isSaved = true;
            this.View.Model.IsDirty = false;
        }
        #endregion
    }
}
