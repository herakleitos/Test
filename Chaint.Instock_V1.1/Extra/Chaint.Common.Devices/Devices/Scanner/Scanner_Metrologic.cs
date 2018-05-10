using System;
using System.Collections.Generic;
using System.Text;

using System.IO.Ports;

namespace Chaint.Common.Devices.Devices
{
    public class Scanner_Metrologic : Scanner, IDisposable
    {
        private ConnectionType m_ConnType = ConnectionType.Serial;
        private Param_Base m_ConnParam = null;
        protected SerialPort m_sp = null;


        public override event RunMessageEventHandler OnRunMessage;
        public override event ReadStringArrivedHandler OnBarcodeValue;

        public override ScanType DeviceType
        {
            get { return ScanType.Metrologic; }
        }

        public Scanner_Metrologic(Param_Base connParam)
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

                    SendMessage(OnRunMessage, string.Format("打开端口<{0}>成功!", param.PortName));
                    return true;
                }
                else
                {
                    Param_Ethernet param = m_ConnParam as Param_Ethernet;
                    return false;
                }
            }
            catch (System.Exception ex)
            {
                SendMessage(OnRunMessage, "打开扫描端口出错,原因:" + ex.Message);
                m_sp.Close();
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

                m_ConnParam.State = ConnectionState.Disconnected;
                m_ConnParam.LastOperationTime = DateTime.Now;
                
            }
        }

        public override string Read()
        {
            return "";
        }

        public override bool Write(byte[] bytData)
        {
            if (m_ConnType == ConnectionType.Serial)
            {
                if (m_sp != null && m_sp.IsOpen)
                {
                    m_sp.Write(bytData, 0, bytData.Length);
                    
                    m_ConnParam.LastOperationTime = DateTime.Now;
                    
                    return true;
                }
                else
                    return false;
            }
            else
            {
                //TCP 作为客户端
                return false;
            }
        }


        public override bool Write(string strData)
        {
            if (m_ConnType == ConnectionType.Serial)
            {
                if (m_sp != null && m_sp.IsOpen)
                {
                    m_sp.Write(strData);
                    m_ConnParam.LastOperationTime = DateTime.Now;
                    return true;
                }
                else
                    return false;
            }
            else
            {
                //TCP 作为客户端
                return false;
            }
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                if (m_sp == null || m_sp.IsOpen == false) return;
                string strReadData = m_sp.ReadLine();
                string str = "";
                if (strReadData.Length > 2)
                {
                    str = strReadData.Substring(0, strReadData.Length - 1).Replace((char)13, ' ').Trim();
                    str = str.Replace((char)10, ' ').Replace("*", "").Trim();
                    if (OnBarcodeValue != null) OnBarcodeValue(str);
                }
            }
            catch (System.Exception ex)
            {
                SendMessage(OnRunMessage, "读取扫描条码出错,原因:" + ex.Message);
            }
        }

    }

}
