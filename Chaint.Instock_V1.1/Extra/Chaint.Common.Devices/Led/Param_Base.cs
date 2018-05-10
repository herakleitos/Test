using System;
using System.Collections.Generic;
using System.Text;

namespace Chaint.Common.Devices.LED
{
    public delegate void RunMessageEventHandler(object sender,string strMsg);
    public delegate void ReadStringArrivedHandler(string strReadValue);
    public delegate void ReadDoubleArrivedHandler(double dValue);       //激光测距

    public class Param_Base
    {
        private DateTime m_LastAction;  
        private ConnectionState m_State;

        private string m_DeviceName = "";
        private string m_JobName = "";

        public ConnectionState State
        {
            get { return m_State; }
            set { m_State = value; }
        }

        public DateTime LastOperationTime
        {
            get { return m_LastAction; }
            set { m_LastAction = value; }
        }

        /// <summary>
        /// 设备名
        /// </summary>
        public string DeviceName
        {
            get { return m_DeviceName; }
            set { m_DeviceName = value; }
        }

        /// <summary>
        /// 作业名
        /// </summary>
        public string JobName
        {
            get { return m_JobName; }
            set { m_JobName = value; }
        }

    }

    public enum ConnectionType
    {
        Serial,
        TCP,
    }

    public enum ConnectionState
    {
        Disconnected = 0,
        Connecting = 1,
        Connected = 2,
        Disconnecting = 3,
        Error=4,
    }

    public class ScanEventArgs
    {
        public ConnectionState state;
        public string RetMsg;
        public DateTime ActionTime;
    }

}
