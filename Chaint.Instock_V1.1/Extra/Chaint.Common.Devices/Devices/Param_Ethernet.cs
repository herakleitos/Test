using System;
using System.Collections.Generic;
using System.Text;

using System.Net;
using System.Net.Sockets;

namespace Chaint.Common.Devices.Devices
{
    public class Param_Ethernet : Param_Base
    {
        public enum CommMode { CLISERVICE = 0, SERIALSERVER = 1 }        //直接使用CLI 服务通讯,另外一种使用串口服务器

        private string m_IPAddress;
        private int m_Port = 1912;

        private CommMode m_CommMode = CommMode.CLISERVICE;
      
        public Param_Ethernet(CommMode commMode, string ipAddress)
        {
            m_CommMode = commMode;
            m_IPAddress = ipAddress;
            if (commMode == CommMode.SERIALSERVER) m_Port = 4001;
        }
        public Param_Ethernet(CommMode commMode, string ipAddress, int port)
        {
            m_CommMode = commMode;
            m_IPAddress = ipAddress;
            m_Port = port;
        }

        public string IPAddress
        {
            get { return m_IPAddress; } 
        }
    
        public int Port 
        {
            get {return m_Port;}
        }

        /// <summary>
        /// 通讯操作模式
        /// </summary>
        public CommMode CommunicateMode
        { get { return m_CommMode; } }
       
       
    }
}
