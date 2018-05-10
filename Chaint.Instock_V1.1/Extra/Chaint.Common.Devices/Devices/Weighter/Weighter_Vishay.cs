using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO.Ports;

namespace Chaint.Common.Devices.Devices
{
    public class Weighter_Vishay:Weighter,IDisposable
    {

        public override event RunMessageEventHandler OnRunMessage;
        public override event ReadStringArrivedHandler OnWeightValue;

        private Param_Base m_ConnParam = null;
        private ConnectionType m_ConnType = ConnectionType.Serial;

        protected SerialPort m_sp = null;


        public override WeighterType DeviceType
        {
            get { return WeighterType.Vishay; }
        }

        public Weighter_Vishay(Param_Base connParam)
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
                SendMessage(OnRunMessage, "打开称重端口出错,原因:" + ex.Message);
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
            try
            {
                if (m_sp == null || m_sp.IsOpen == false) return;
                string ss = m_sp.ReadExisting();
                if (ss[0] == (char)2)//如果是起始位
                {
                    if (ss.Length >= 8)
                    {
                        if (OnWeightValue != null)
                            OnWeightValue(ss.Substring(1, 7).Trim());
                    }
                }
                else
                    ss = "";
            }
            catch (System.Exception ex)
            {
                SendMessage(OnRunMessage, "读取称重值出错,原因:" + ex.Message);
            }
        }


    }

  
}
