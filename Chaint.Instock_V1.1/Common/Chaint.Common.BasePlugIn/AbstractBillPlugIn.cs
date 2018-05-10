using System;
using System.Data;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using Chaint.Common.Interface.PlugIn;
using Chaint.Common.Core.EventArgs;
using Chaint.Common.Core.Utils;
using Chaint.Common.Entity;
using Chaint.Common.Core;

namespace Chaint.Common.BasePlugIn
{
    public abstract class AbstractBillPlugIn : IPlugIn
    {
        public AbstractBillView View { get; private set; }
        public Context Context { get; private set; }
        public AbstractBillPlugIn(AbstractBillView _view)
        {
            this.View = _view;
            Context = _view.Context;
        }
        public virtual void ButtonClick(ButtonClickEventArgs e)
        {
            return;
        }
        public void OnLoad()
        {
            this.OnSetBusinessInfo();
            this.OnBind();
            this.AfterBind();
            //初始化model
            foreach (var item in this.View.Fields)
            {
                object value = this.View.GetValue<object>(item);
                //并不是所有view都会持有model
                if (this.View.Model != null)
                {
                    this.View.Model.SetValue(item, value);
                }
            }
        }
        /// <summary>
        /// 初始化界面元素,包括控件属性，控件绑定数据源等
        /// </summary>
        public virtual void OnSetBusinessInfo()
        {
            return;
        }
        /// <summary>
        /// 绑定界面数据，包括设定默认数据，打开单据时绑定数据包等
        /// </summary>
        public virtual void OnBind()
        {
            return;
        }
        /// <summary>
        /// 绑定界面数据之后，可以执行一些默认的操作等
        /// </summary>
        public virtual void AfterBind()
        {
            if (this.View.Model != null)
            {
                this.View.Model.IsDirty = false;
            }
            return;
        }
        public virtual void OnClose()
        {
            return;
        }
        public virtual void DataReceived(DataReceivedEventArgs e)
        {
            return;
        }
        public virtual void OnDispose()
        {
            return;
        }
        public virtual void DataChanged(DataChangedEventArgs e)
        {
            return;
        }
        public virtual void BeforeUpdateValue(DataChangingEventArgs e)
        {
            return;
        }
        public virtual void EntryRowClick(EntryRowClickEventArgs e)
        {
            return;
        }
        public virtual void AfterCreateNewEntryRow(AfterCreateNewEntryRowEventArgs e)
        {
            if (e.Sender.IsNullOrEmptyOrWhiteSpace()) return;
            if (e.Parent.IsNullOrEmptyOrWhiteSpace()) return;
            var gcData = this.View.GetControl<GridControl>(e.Parent);
            if (!e.ParentContainer.IsNullOrEmptyOrWhiteSpace() && !e.ParentKey.IsNullOrEmptyOrWhiteSpace())
            {
                var parentId = this.View.GetValue<string>(e.ParentKey);
                this.View.SetValue(e.Sender,e.ParentContainer, parentId, e.Row);
            }
            if (!e.SeqKey.IsNullOrEmptyOrWhiteSpace())
            {
                var seqField = this.View.GetControl<GridColumn>(e.SeqKey);
                this.View.SetValue(e.Sender, e.SeqKey, this.View.GetEntryRowCount(e.Sender), e.Row);
            }
            if (!e.PrimaryKey.IsNullOrEmptyOrWhiteSpace())
            {
                this.View.SetValue(e.Sender, e.PrimaryKey, SequenceGuid.NewGuid().ToString(), e.Row);
            }
            return;
        }
        public virtual void AfterDeleteEntryRow(AfterDeleteEntryRowEventArgs e)
        {
            if (e.Parent.IsNullOrEmptyOrWhiteSpace()) return;
            var gcData = this.View.GetControl<GridControl>(e.Parent);
            DataTable dt = gcData.DataSource as DataTable;
            dt.AcceptChanges();
            if (e.SeqName.IsNullOrEmptyOrWhiteSpace()) return;
            var seqField = this.View.GetControl<GridColumn>(e.SeqName);
            if (seqField == null) return;
            if (e.Row+1 >= dt.Rows.Count) return;
            //重整序号
            for (int rowIndex = e.Row; rowIndex < dt.Rows.Count; rowIndex++)
            {
                int seq = Convert.ToInt32(dt.Rows[rowIndex][seqField.FieldName]);
                dt.Rows[rowIndex][seqField.FieldName] = --seq;
            }
            gcData.DataSource = dt;
            return;
        }
        public virtual void BeforeDeleteEntryRow(BeforeDeleteEntryRowEventArgs e)
        {
            return;
        }
        public virtual void CustomEntryRowCell(CustomEntryRowCellEventArgs e)
        {
            return;          
        }
    }
}

