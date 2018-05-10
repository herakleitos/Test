using System;
using System.Collections.Generic;
using System.Text;

using System.IO.Ports;

namespace Chaint.Common.Devices.Devices
{
    public class Param_SerialPort : Param_Base
    {
        private string m_PortName = "";
        private int m_BaudRate = 9600;
        private int m_DataBits = 8;
        private System.IO.Ports.StopBits m_StopBits = System.IO.Ports.StopBits.One;
        private Handshake m_Handshake = Handshake.None;
        private Parity m_Parity = Parity.None;
      
        public Param_SerialPort(string portName)
            : this(portName, 9600, Parity.None, 8, StopBits.One, Handshake.None)
        {

        }
        public Param_SerialPort(string portName, int baudrate)
            : this(portName, baudrate, Parity.None, 8, StopBits.One, Handshake.None)
        {
            
        }

        public Param_SerialPort(string portName, int baudrate, Parity parity, int dataBits, StopBits stopBits, Handshake handshake)
        {
            m_PortName = portName;
            m_BaudRate = baudrate;
            m_Parity = parity;
            m_DataBits = dataBits;
            m_StopBits = stopBits;
            m_Handshake = handshake;
        }

        /// <summary>
        /// �˿�����
        /// </summary>
        public string PortName
        {
            set { m_PortName = value; }
            get { return m_PortName; }
        }

        //������
        public int BaudRate
        {
            set { m_BaudRate = value; }
            get { return m_BaudRate; }
        }

        //����λ
        public int DataBits
        {
            set { m_DataBits = value; }
            get { return m_DataBits; }
        }

        /// <summary>
        /// ֹͣλ
        /// </summary>
        public System.IO.Ports.StopBits StopBits
        {
            set { m_StopBits = value; }
            get { return m_StopBits; }
        }

        public Handshake Handshake
        {
            get { return m_Handshake; }
            set { m_Handshake = value; } 
        }

        public Parity Parity
        { 
            get { return m_Parity; } 
            set { m_Parity = value; } 
        }

       
    }
}
