using System;
using System.Collections.Generic;
using System.Text;

namespace Chaint.Common.Devices.LED
{
    public class Param_LED_Ethernet:Param_Base
    {
        private string m_IPAddr = "192.168.0.1";

        private ushort m_IPPort = 8800;

        private uint m_ConnTimeOut = 3000;      //连接超时，默认30S
        private uint m_RevTimeOut = 2500;

        public Param_LED_Ethernet(string strIP)
            : this(strIP, 8800, 3000, 3000)
        {

        }

        public Param_LED_Ethernet(string strIP,ushort portNO)
            :this(strIP,portNO,3000,3000)
        {

        }

        public Param_LED_Ethernet(string strIP,ushort portNO,uint connTimeOut,uint revTimeOut)
        {
            m_IPAddr = strIP;
            m_IPPort = portNO;
            m_ConnTimeOut = connTimeOut;
            m_RevTimeOut = revTimeOut;
        }


        /// <summary>
        /// 连接超时时间
        /// </summary>
        public uint ConnTimeOut
        {
            get { return m_ConnTimeOut; }
            set { m_ConnTimeOut = value; }
        }
        
        /// <summary>
        /// 接收超时时间
        /// </summary>
        public uint RevTimeOut
        {
            get { return m_RevTimeOut; }
            set { m_RevTimeOut = value; }
        }



        public ushort IPPort
        {
            get { return m_IPPort; }
            set { m_IPPort = value; }
        }


        public string IPAddr
        {
            get { return m_IPAddr; }
            set { m_IPAddr = value; }
        }


    }
}
