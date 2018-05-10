using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace Chaint.Common.Devices.Devices
{
    public class Weighter_Toledo: Weighter, IDisposable
    {

        public override event RunMessageEventHandler OnRunMessage;
        public override event ReadStringArrivedHandler OnWeightValue;

        private Param_Base m_ConnParam = null;
        private ConnectionType m_ConnType = ConnectionType.Serial;
        protected SerialPort m_sp = null;

        public override WeighterType DeviceType
        {
            get { return WeighterType.Toledo; }
        }

        public Weighter_Toledo(Param_Base connParam)
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

        private string m_strWeight = "";
        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                if (m_sp == null || m_sp.IsOpen == false) return;
                int dp = 0; //小数点位数
                string ss = m_sp.ReadExisting();
                double ddd = 0;
                string s = "";
                if (ss[0] == (char)2)//如果是起始位
                {
                    if (m_strWeight.Length == 17 || m_strWeight.Length == 18)
                    {
                        dp = Convert.ToInt16((Convert.ToChar(m_strWeight[1]) & 07)); //这里& 07 是把原来 + 16获得的符号转换为数字
                        s = m_strWeight.Substring(4, 6);
                        ddd = Convert.ToInt32(s) * System.Math.Pow(10, 2 - dp);
                        if (OnWeightValue != null) OnWeightValue(ddd.ToString());
                    }
                    m_strWeight = ss;
                }
                else
                {
                    m_strWeight = m_strWeight + ss;
                }
            }
            catch (System.Exception ex)
            {
                SendMessage(OnRunMessage, "读取称重值出错,原因:" + ex.Message);
            }
        }

    }
}
