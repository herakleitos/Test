using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Net;
using System.Net.Sockets;

namespace Chaint.Common.Devices.Devices
{
    public class InkJetter_Imaje4020 : InkJetter, IDisposable
    {

        protected SerialPort m_sp = null;
        private ConnectionType m_ConnType = ConnectionType.Serial;

        private Param_Base m_ConnParam = null;

        public override event RunMessageEventHandler OnRunMessage;
        public override event ReadStringArrivedHandler OnInkJetRetValue;
        public override InkJetterType DeviceType
        {
            get { return InkJetterType.Imaje4020; }
        }

        public InkJetter_Imaje4020(Param_Base connParam)
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
                    Param_Ethernet param = m_ConnParam as Param_Ethernet;
                    m_ConnParam.State = ConnectionState.Connecting;
                    m_ConnParam.LastOperationTime = DateTime.Now;
                    return true;
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
            if(m_ConnType==ConnectionType.Serial)
            {
                for (int i = 0; i < strLines.Length; i++)
                {
                    //str += "!!" + "v!5" + (char)34 + row[PaperRollData.ROLL_ID_FIELD].ToString() + " " + (char)34 + (char)13;
                    sbJetInfo.AppendFormat("!!v!{0}{1}{2}{3}{4}", (i + 1), (char)34, strLines[i], (char)34, (char)13);
                }

                if (!m_sp.IsOpen) m_sp.Open();
                m_sp.Write(sbJetInfo.ToString());

                SendMessage(OnRunMessage,sbJetInfo.ToString());             //显示喷码信息   
                return true;
            }
            else
            {
                for (int i = 0; i < strLines.Length; i++)
                {
                    //str += "!!" + "v!5" + (char)34 + row[PaperRollData.ROLL_ID_FIELD].ToString() + " " + (char)34 + (char)13;
                    sbJetInfo.AppendFormat("!!v!{0}{1}{2}{3}{4}", (i + 1), (char)34, strLines[i], (char)34, (char)13);
                }
                if (SendPrintContentBySck(sbJetInfo.ToString()))
                {
                    SendMessage(OnRunMessage,sbJetInfo.ToString());             //显示喷码信息   
                }
                return true;
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

        private StringBuilder m_sbJetRetMsg = new StringBuilder();
        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                if (m_sp == null || m_sp.IsOpen == false) return;
                string retValue = m_sp.ReadExisting();
                if (OnInkJetRetValue != null) OnInkJetRetValue(retValue);
            }
            catch (System.Exception ex)
            {
                SendMessage(OnRunMessage, "读取喷码返回值出错,原因:" + ex.Message);
            }
        }


        private bool SendPrintContentBySck(string strPrintContent)
        {
            IPAddress printerip;
            Byte[] sendBytes = new Byte[4096];
            //Encoding encoder = System.Text.Encoding.GetEncoding("ASCII");
            Encoding encoder = System.Text.Encoding.UTF8;

            string retMsg = "";
              
            string strIPAddress = "";
            int intPort = 9100;
            try
            {
                if(m_ConnParam is Param_Ethernet)
                {
                    strIPAddress = ((Param_Ethernet)m_ConnParam).IPAddress;
                    intPort = ((Param_Ethernet)m_ConnParam).Port;
                }

                if (IPAddress.TryParse(strIPAddress, out printerip))
                {
                    sendBytes = encoder.GetBytes(strPrintContent);
                    IPEndPoint ipPoint = new IPEndPoint(printerip, intPort);
                    Socket clientSck = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    clientSck.Connect(ipPoint);
                    clientSck.Send(sendBytes, sendBytes.Length, 0);
                    System.Threading.Thread.Sleep(200);
                    clientSck.Disconnect(true);
                    clientSck.Close();
                    clientSck.Dispose();
                    clientSck = null;
                    return true;
                }
                else
                {
                    retMsg = string.Format("IP地址【{0}】输入有误，请检查打印机IP配置是否正确!", strIPAddress);
                    SendMessage(OnRunMessage, retMsg);
                    return false;
                }
            }
            catch (Exception ex)
            {
                retMsg = "Imaje打印机未连接成功,原因:" + ex.Message;
                SendMessage(OnRunMessage, retMsg);
                return false;
            }
        }


    }
}
