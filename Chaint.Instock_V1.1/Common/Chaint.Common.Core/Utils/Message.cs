using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chaint.Common.Core.Const;
namespace Chaint.Common.Core.Utils
{
    public static class Message
    {
        public static string MakeWMSSocketMsg(string[] msgs)
        {
            string retMsg = Const_WMSMessage._SpliteChar + "";
            retMsg += String.Join(Const_WMSMessage._SpliteChar + "", msgs);
            retMsg.TrimEnd(Const_WMSMessage._SpliteChar);
            return Const_WMSMessage._StartChar + (retMsg.Length + 6).ToString().PadLeft(4, '0') + retMsg + Const_WMSMessage._EndChar;
        }
        public static string TrimEndZero(string number)
        {
            if (number.Contains("."))
            {
                number = number.TrimEnd('0').TrimEnd('.');
            }
            return number;
        }
    }
}
