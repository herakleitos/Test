using System;
using System.Collections.Generic;
using System.Text;

namespace Chaint.Common.Devices.PLC
{
    public enum PLCType { PLC_5D6, PLC_6D0, PLC_5D0, PLC_LibNoDave,PLC_Test };

    //public delegate void PLCLogMessageEventHandler(object sender, string strLogMsg);        //运行日志信息

    public delegate void PLCMessageEventHandler(object sender,PLCEventArgument args);       //PLC返回消息

    public delegate void PLCAlaramEventHandler(object sender, PLCAlaramEventArgs args);     //PLC报警信息

    /// <summary>
    /// PLC操作
    /// </summary>
    public class PLCEventArgument:EventArgs
    {
        bool m_blnConnected = false;
        string m_strMessage = "";

        public bool Connected
        {
            get { return m_blnConnected; }
        }

        public string RetMessage
        {
            get { return m_strMessage; }
        }

        public PLCEventArgument(bool blnConnected, string strMessage)
        {
            m_blnConnected = blnConnected;
            m_strMessage = strMessage;
        }
    }

    /// <summary>
    /// PLC报警
    /// </summary>
    public class PLCAlaramEventArgs : EventArgs
    {
        private readonly string m_retMsg = "";
        private readonly System.Collections.ArrayList m_arMsgCollects = new System.Collections.ArrayList();

        public PLCAlaramEventArgs(string strRetMsg)
        {
            m_retMsg = strRetMsg;
        }

        public PLCAlaramEventArgs(System.Collections.ArrayList arPLCAlarmMsgCollects)
        {
            m_arMsgCollects.Clear();
            m_arMsgCollects = arPLCAlarmMsgCollects;
        }
      
        public string RetMsg
        {
            get { return m_retMsg; }
        }

        public System.Collections.ArrayList RetMsgCollects
        {
            get { return m_arMsgCollects; }
        }
    }

}
