using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraEditors;
using Chaint.Common.Core;
using Chaint.Common.Core.Utils;
using Chaint.Common.Interface.View;
namespace Chaint.Common.Entity
{
    public abstract class AbstractBillView : IBillView,IDisposable
    {
        public Action<string, object> Open;
        public Context Context { get; protected set; }
        public AbstractBillModel Model { get; protected set; }
        public List<string> Fields { get; private set; }
        private Dictionary<string, object> _controlsDic;
        public void Disposable()
        {

        }
        public AbstractBillView()
        {
        }
        public AbstractBillView(Context ctx)
        {
            Context = ctx;
            _controlsDic = new Dictionary<string, object>();
            Fields = new List<string>();
        }
        public virtual void AddControl(string name, object obj)
        {
            if (_controlsDic.Keys.Contains(name)) return;
            _controlsDic.Add(name, obj);
            Fields.Add(name);
        }
        public virtual T GetControl<T>(string name)
        {
            object control = null;
            if (!_controlsDic.TryGetValue(name, out control)) return default(T);
            if (control == null) return default(T);
            if (control is T)
            {
                return (T)control;
            }
            return default(T);
        }
        public virtual void Sync(string fieldname)
        {
            object value = this.GetValue<object>(fieldname);
            this.Model.SetValue(fieldname, value);
            this.Model.IsDirty = true;
        }
        public virtual void Sync()
        {
            this.Model.IsDirty = true;
        }
        public virtual T GetValue<T>(string name)
        {
            object control = null;
            if (!_controlsDic.TryGetValue(name, out control)) return default(T);
            if (control == null) return default(T);
            string fieldType = control.GetType().Name;
            if (fieldType.EqualIgnorCase("ButtonEdit"))
            {
                object value = (control as ButtonEdit).Text;
                try
                {
                    if (value is T)
                    {
                        return (T)value;
                    }
                    else
                    {
                        return (T)Convert.ChangeType(value, typeof(T));
                    }
                }
                catch { return default(T); }
            }
            else if (fieldType.EqualIgnorCase("SimpleButton"))
            {
                object value = (control as SimpleButton).Text;
                try
                {
                    if (value is T)
                    {
                        return (T)value;
                    }
                    else
                    {
                        return (T)Convert.ChangeType(value, typeof(T));
                    }
                }
                catch { return default(T); }
            }
            else if (fieldType.EqualIgnorCase("LookUpEdit"))
            {
                object value = (control as LookUpEdit).EditValue;
                try
                {
                    if (value is T)
                    {
                        return (T)value;
                    }
                    else
                    {
                        return (T)Convert.ChangeType(value, typeof(T));
                    }
                }
                catch { return default(T); }
            }
            else if (fieldType.EqualIgnorCase("DateEdit"))
            {
                object value = (control as DateEdit).DateTime;
                try
                {
                    DateTime time = Convert.ToDateTime(value);
                    object processedValue = DateTime.Parse("1/1/1753 12:00:00");
                    if (time == DateTime.MinValue)
                    {
                        if (processedValue is T)
                        {
                            return (T)processedValue;
                        }
                        else
                        {
                            return (T)Convert.ChangeType(processedValue, typeof(T));
                        }
                    }
                    else
                    {
                        if (value is T)
                        {
                            return (T)value;
                        }
                        else
                        {
                            return (T)Convert.ChangeType(value, typeof(T));
                        }
                    }
                }
                catch { return default(T); }
            }
            else if (fieldType.EqualIgnorCase("ComboBoxEdit"))
            {
                object value = (control as ComboBoxEdit).Text;
                try
                {
                    if (value is T)
                    {
                        return (T)value;
                    }
                    else
                    {
                        return (T)Convert.ChangeType(value, typeof(T));
                    }
                }
                catch { return default(T); }
            }
            else if (fieldType.EqualIgnorCase("CheckedComboBoxEdit"))
            {
                object value = (control as CheckedComboBoxEdit).EditValue;
                try
                {
                    if (value is T)
                    {
                        return (T)value;
                    }
                    else
                    {
                        return (T)Convert.ChangeType(value, typeof(T));
                    }
                }
                catch { return default(T); }
            }
            else if (fieldType.EqualIgnorCase("TextEdit"))
            {
                object value = (control as TextEdit).Text;
                try
                {
                    if (value is T)
                    {
                        return (T)value;
                    }
                    else
                    {
                        return (T)Convert.ChangeType(value, typeof(T));
                    }
                }
                catch { return default(T); }
            }
            else if (fieldType.EqualIgnorCase("SearchLookUpEdit"))
            {
                object value = (control as SearchLookUpEdit).EditValue;
                try
                {
                    if (value is T)
                    {
                        return (T)value;
                    }
                    else
                    {
                        return (T)Convert.ChangeType(value, typeof(T));
                    }
                }
                catch { return default(T); }
            }
            else if (fieldType.EqualIgnorCase("MemoExEdit"))
            {
                object value = (control as MemoExEdit).EditValue;
                try
                {
                    if (value is T)
                    {
                        return (T)value;
                    }
                    else
                    {
                        return (T)Convert.ChangeType(value, typeof(T));
                    }
                }
                catch { return default(T); }
            }
            else if (fieldType.EqualIgnorCase("CalcEdit"))
            {
                object value = (control as CalcEdit).EditValue;
                try
                {
                    if (value is T)
                    {
                        return (T)value;
                    }
                    else
                    {
                        return (T)Convert.ChangeType(value, typeof(T));
                    }
                }
                catch { return default(T); }
            }
            else if (fieldType.EqualIgnorCase("CheckEdit"))
            {
                object value = (control as CheckEdit).EditValue;
                try
                {
                    if (value is T)
                    {
                        return (T)value;
                    }
                    else
                    {
                        return (T)Convert.ChangeType(value, typeof(T));
                    }
                }
                catch { return default(T); }
            }
            else
            {
                return default(T);
            }
        }
        public virtual void SetValueByName(string name, object value)
        {
            object control = null;
            if (!_controlsDic.TryGetValue(name, out control)) return;
            if (control == null) return;
            if (control.GetType().Name.EqualIgnorCase("SearchLookUpEdit"))
            {
                (control as SearchLookUpEdit).Text = Convert.ToString(value);
            }
            else if (control.GetType().Name.EqualIgnorCase("ComboBoxEdit"))
            {
                (control as ComboBoxEdit).Text = Convert.ToString(value);
            }
            //view设置值时，触发modle的值设定
            if (this.Model != null)
            {
                this.Model.SetValue(name, value);
            }
        }
        public virtual int GetEntryRowCount(string entryName)
        {
            object control = null;
            if (!_controlsDic.TryGetValue(entryName, out control)) return -1;
            if (control is GridView)
            {
                GridView view = control as GridView;
                return view.RowCount;
            }
            else { return -1; }
        }
        public virtual int GetSelectedRowIndex(string entryName, string checkField = "")
        {
            object control = null;
            if (!_controlsDic.TryGetValue(entryName, out control)) return -1;
            if (control is GridView)
            {
                GridView view = control as GridView;
                if (checkField.IsNullOrEmptyOrWhiteSpace())
                {
                    int[] selIndexs = view.GetSelectedRows();
                    if (selIndexs.Length > 0) return selIndexs[0];
                    else return -1;
                }
                else
                {
                    for (int i = 0; i < view.RowCount; i++)
                    {
                        object isCheck = view.GetRowCellValue(i, checkField);
                        if (isCheck.Equals(1))
                        {
                            return i;
                        }
                    }
                    return -1;
                }
            }
            else { return -1; }
        }
        public virtual T GetValue<T>(string entryName, string colName, int row)
        {
            object control = null;
            if (!_controlsDic.TryGetValue(entryName, out control)) return default(T);
            if (control is GridView)
            {
                try
                {
                    GridView view = control as GridView;
                    object value =
                        view.GetRowCellValue(row, colName);
                    if (value is T)
                    {
                        return (T)value;
                    }
                    else
                    {
                        return (T)Convert.ChangeType(value, typeof(T));
                    }
                }
                catch
                {
                    return default(T);
                }
            }
            else
            {
                return default(T);
            }
        }
        public virtual void SetValue(string entryName, string colName, object value, int row)
        {
            object control = null;
            if (!_controlsDic.TryGetValue(entryName, out control)) return;
            if (control is GridView)
            {
                GridView view = control as GridView;
                view.SetRowCellValue(row, colName, value);
            }
            if (this.Model != null)
            {
                this.Model.SetValue(colName, value, row);
            }
        }
        public virtual void SetValue(string name, object value)
        {
            object control = null;
            if (!_controlsDic.TryGetValue(name, out control)) return;
            if (control == null) return;
            string fieldType = control.GetType().Name;
            if (fieldType.EqualIgnorCase("ButtonEdit"))
            {
                (control as ButtonEdit).EditValue = value;
            }
            else if (fieldType.EqualIgnorCase("SimpleButton"))
            {
                (control as SimpleButton).Text = Convert.ToString(value);
            }
            else if (fieldType.EqualIgnorCase("LookUpEdit"))
            {
                (control as LookUpEdit).EditValue = value;
            }
            else if (fieldType.EqualIgnorCase("SearchLookUpEdit"))
            {
                (control as SearchLookUpEdit).EditValue = value;
            }
            else if (fieldType.EqualIgnorCase("CalcEdit"))
            {
                (control as CalcEdit).EditValue = value;
            }
            else if (fieldType.EqualIgnorCase("DateEdit"))
            {
                (control as DateEdit).DateTime = Convert.ToDateTime(value);
            }
            else if (fieldType.EqualIgnorCase("ComboBoxEdit"))
            {
                (control as ComboBoxEdit).EditValue = value;
            }
            else if (fieldType.EqualIgnorCase("TextEdit"))
            {
                (control as TextEdit).EditValue = value;
            }
            else if (fieldType.EqualIgnorCase("ListBoxControl"))
            {
                ListBoxControl lstBox = control as ListBoxControl;
                if (value != null)
                {
                    lstBox.Items.Add(value);
                }
                else
                {
                    lstBox.Items.Clear();
                }
            }
            else if (fieldType.EqualIgnorCase("CheckedComboBoxEdit"))
            {
                var item = (control as CheckedComboBoxEdit);
                item.EditValue = value;
                item.RefreshEditValue();
            }
            else if (fieldType.EqualIgnorCase("LabelControl"))
            {
                if (value == null)
                {
                    (control as LabelControl).Text = string.Empty;
                }
                else
                {
                    (control as LabelControl).Text = Convert.ToString(value);
                }
            }
            else if (fieldType.EqualIgnorCase("MemoEdit"))
            {
                if (value == null)
                {
                    (control as MemoEdit).Text = string.Empty;
                }
                else
                {
                    (control as MemoEdit).Text = Convert.ToString(value);
                }
            }
            else if (fieldType.EqualIgnorCase("MemoExEdit"))
            {
                if (value == null)
                {
                    (control as MemoExEdit).Text = string.Empty;
                }
                else
                {
                    (control as MemoExEdit).Text = Convert.ToString(value);
                }
            }
            //view设置值时，触发modle的值设定
            if (this.Model != null)
            {
                this.Model.SetValue(name, value);
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)。
                    Fields.Clear();
                    _controlsDic.Clear();
                    if (Model != null)
                    {
                        Model.Dispose();
                    }
                }

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。

                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~AbstractBillView() {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
