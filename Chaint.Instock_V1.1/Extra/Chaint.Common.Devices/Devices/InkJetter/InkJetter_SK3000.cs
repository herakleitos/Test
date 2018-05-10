using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace Chaint.Common.Devices.Devices
{
    public class InkJetter_SK3000:InkJetter,IDisposable
    {
        protected SerialPort m_sp = null;
        private ConnectionType m_ConnType = ConnectionType.Serial;

        private Param_Base m_ConnParam = null;

        public override event RunMessageEventHandler OnRunMessage;
        public override event ReadStringArrivedHandler OnInkJetRetValue;
        public override InkJetterType DeviceType
        {
            get { return InkJetterType.SK3000; }
        }

        public InkJetter_SK3000(Param_Base connParam)
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
                    //m_sp.StopBits = param.StopBits;     //marsh 的停止位一般为2位
                    m_sp.StopBits = StopBits.Two;
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
                SendMessage(OnRunMessage, "打开喷码端口出错,原因:" + ex.Message);
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

        public override bool Write(string[] strLines)
        {
            if (strLines.Length == 0) return false;

            StringBuilder sbJetInfo = new StringBuilder();

            sbJetInfo.AppendFormat("{0}{1}", (char)2, (char)32);
            for (int i = 0; i < strLines.Length; i++)
            {
                sbJetInfo.Append(strLines);
            }
            sbJetInfo.AppendFormat("{0}", (char)3);

            if(m_ConnType==ConnectionType.Serial)
            {
                if (!m_sp.IsOpen) m_sp.Open();

                m_sp.Write(sbJetInfo.ToString());

                SendMessage(OnRunMessage, sbJetInfo.ToString());             //显示喷码信息  
                return true;
            }
            else
            {
                //TCP 作为客户端
                return false;
            }
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
                if (OnInkJetRetValue != null) OnInkJetRetValue(ss);
            }
            catch (System.Exception ex)
            {
                SendMessage(OnRunMessage, "读取喷码返回值出错,原因:" + ex.Message);
            }
        }

    }
}
