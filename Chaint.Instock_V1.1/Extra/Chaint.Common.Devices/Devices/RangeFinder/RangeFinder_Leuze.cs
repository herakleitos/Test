using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

/*-----------------------------------------------------------------------------------
 * 作者: CH
 * 
 * 创建时间: 2013-09-06
 * 
 * 功能描述: 
 *      单个激光测距仪的读数(从端口读出来的未处理的值)
 * 
 ------------------------------------------------------------------------------------*/

namespace Chaint.Common.Devices.Devices
{
    public class RangeFinder_Leuze:RangeFinder,IDisposable
    {
        
        public override event RunMessageEventHandler OnRunMessage;
        public override event ReadStringArrivedHandler OnRetMeasureValue;

        private Param_Base m_ConnParam = null;
        private ConnectionType m_ConnType = ConnectionType.Serial;
        protected SerialPort m_sp = null;

        public override RangeFinderType DeviceType
        {
            get { return RangeFinderType.Leuze; }
        }

        public RangeFinder_Leuze(Param_Base connParam)
        {
            m_ConnParam = connParam;
            if (m_ConnParam is Param_SerialPort)
                m_ConnType = ConnectionType.Serial;
            else
                m_ConnType = ConnectionType.TCP;
        }

        public override bool Connect()
        {
            if (m_ConnParam == null) return false;

            try
            {
                if (m_ConnType == ConnectionType.Serial)
                {
                    Param_SerialPort param = m_ConnParam as Param_SerialPort;
                    m_sp = new SerialPort();
                    m_sp.PortName = param.PortName;
                    m_sp.BaudRate = param.BaudRate;
                    m_sp.StopBits = param.StopBits;
                    m_sp.Handshake = param.Handshake;
                    m_sp.ReceivedBytesThreshold = 1;
                    m_sp.DataBits = param.DataBits;
                    m_sp.DataReceived += new SerialDataReceivedEventHandler(SerialPort_DataReceived);
                    m_sp.Open();
                    m_ConnParam.State = ConnectionState.Connected;
                    m_ConnParam.LastOperationTime = DateTime.Now;
                    SendMessage(OnRunMessage, string.Format("打开端口<{0}>成功!", param.PortName));
                    return true;
                }
                else
                {
                    SendMessage(OnRunMessage, "没有定义网口通讯方式");
                    m_ConnParam.State = ConnectionState.Error;
                    m_ConnParam.LastOperationTime = DateTime.Now;
                    return false;
                }
            }
            catch (System.Exception ex)
            {
                SendMessage(OnRunMessage, "打开端口出错,原因:" + ex.Message);
                if (m_ConnType == ConnectionType.Serial && m_sp != null) m_sp.Close();
                return false;
            }
        }

        public override bool Disconnect()
        {
            if (m_sp != null)
            {
                m_sp.Close();
                m_sp.DataReceived -= new SerialDataReceivedEventHandler(SerialPort_DataReceived);
                m_sp.Dispose();
                m_sp = null;
                m_ConnParam.State = ConnectionState.Disconnected;
                m_ConnParam.LastOperationTime = DateTime.Now;
                SendMessage(OnRunMessage, "已断开连接!");
            }
            return true;
        }

        public void Dispose()
        {
            if (m_sp != null)
            {
                m_sp.Close();
                m_sp.DataReceived -= new SerialDataReceivedEventHandler(SerialPort_DataReceived);
                m_sp.Dispose();
                m_sp = null;
                m_ConnParam.LastOperationTime = DateTime.Now;
            }
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (m_sp == null || m_sp.IsOpen == false) return;

            try
            {
                int n = this.m_sp.BytesToRead;
                byte[] bytBuffer = new byte[n];
                m_sp.Read(bytBuffer, 0, n);
                StringBuilder sb = new StringBuilder();
                foreach (byte byt in bytBuffer)
                {
                    if (byt == (char)13)
                    {
                        string strValue = sb.ToString();
                        sb = new StringBuilder();
                        if (OnRetMeasureValue != null) OnRetMeasureValue(strValue.ToString());
                    }
                    else
                    {
                        sb.Append(string.Format("{0}", (char)byt));
                    }
                }
            }
            catch (System.Exception ex)
            {
                SendMessage(OnRunMessage, "读取测量值出错,原因:" + ex.Message);
            }
        }

    }
}
