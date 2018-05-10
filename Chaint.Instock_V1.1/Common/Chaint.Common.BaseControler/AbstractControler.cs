using System;
using System.Collections.Generic;
using Chaint.Common.Core.EventArgs;
using Chaint.Common.Interface.Controler;
using Chaint.Common.Interface.PlugIn;
using Chaint.Common.Entity;
using Chaint.Common.Core.Const;
namespace Chaint.Common.BaseControler
{
    public class AbstractControler : IControler
    {
        List<IPlugIn> _billPlgIns = new List<IPlugIn>();
        public AbstractControler(AbstractBillView view)
        {

        }
        public void AddPluIn(IPlugIn plugIn)
        {
            _billPlgIns.Add(plugIn);
        }
        public void Invoke(string methodName, BaseEventArgs args)
        {
            foreach (IPlugIn plugIn in _billPlgIns)
            {
                switch (methodName)
                {
                    case Const_Event.ButtonClick:
                        plugIn.ButtonClick(args as ButtonClickEventArgs);
                        break;
                    case Const_Event.DataChanged:
                        plugIn.DataChanged(args as DataChangedEventArgs);
                        break;
                    case Const_Event.BeforeUpdateValue:
                        plugIn.BeforeUpdateValue(args as DataChangingEventArgs);
                        break;
                    case Const_Event.AfterCreateNewEntryRow:
                        plugIn.AfterCreateNewEntryRow(args as AfterCreateNewEntryRowEventArgs);
                        break;
                    case Const_Event.AfterDeleteEntryRow:
                        plugIn.AfterDeleteEntryRow(args as AfterDeleteEntryRowEventArgs);
                        break;
                    case Const_Event.BeforeDeleteEntryRow:
                        plugIn.BeforeDeleteEntryRow(args as BeforeDeleteEntryRowEventArgs);
                        break;
                    case Const_Event.DataReceived:
                        plugIn.DataReceived(args as DataReceivedEventArgs);
                        break;
                    case Const_Event.EntryRowClick:
                        plugIn.EntryRowClick(args as EntryRowClickEventArgs);
                        break;
                    case Const_Event.CustomEntryRowCell:
                        plugIn.CustomEntryRowCell(args as CustomEntryRowCellEventArgs);
                        break;
                }
            }
        }
        public void Invoke(string methodName)
        {
            foreach (IPlugIn plugIn in _billPlgIns)
            {
                switch (methodName)
                {
                    case Const_Event.OnLoad:
                        plugIn.OnLoad();
                        break;
                    case Const_Event.OnClose:
                        plugIn.OnClose();
                        break;
                    case Const_Event.OnDispose:
                        plugIn.OnDispose();
                        break;
                }
            }
        }
        public virtual bool IsNumberVal(object value,char keychar)
        {
            if (keychar == '\b')//Backspace符号不参与判断
            {
                return true;
            }
            if (keychar == '.')
            {
                if (value == null || value.Equals(string.Empty))
                {//符号之前不能是空
                    return false;
                }
                string str = Convert.ToString(value);
                if (str.Contains(".")) return false;
                if (str.StartsWith("0") && str.Length > 1)
                {
                    return false;
                }
            }
            else if (!char.IsNumber(keychar))//不是数字
            {
                return false;
            }
            else//如果是数字
            {
                if (value == null || value.Equals(string.Empty))
                {
                    return true;
                }
                string str = Convert.ToString(value);
                if (str.StartsWith("0") && str.Length == 1)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
