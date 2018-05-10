using System;
using System.Collections.Generic;
using System.Text;

using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO.Ports;

namespace Chaint.Common.Devices.Devices
{
    /*-----------------------------------------------------------------------------------
     * 作者: CH
     * 
     * 创建时间: 2014-04-20
     * 
     * 功能描述: 
     *      扫描仪类
     * 
     *              m_CognexHelper.SendCommand("SET TRIGGER.TYPE 0");//先关闭康耐视扫描仪
                    System.Threading.Thread.Sleep(100);
                    m_CognexHelper.SendCommand("SET DECODER.REREAD-NEVER2X ON");//不读取重复值
                    m_CognexHelper.SendCommand("SET TRIGGER.TYPE 4");//连续读取
                    m_CognexHelper.SendCommand("TRIGGER ON");
     * 
     *               m_CognexHelper.SendCommand("SET TRIGGER.TYPE 0");//single读取模式
                     m_CognexHelper.SendCommand("TRIGGER OFF");
     * 
     * 
     ------------------------------------------------------------------------------------*/

    public class Scanner_Cognex:Scanner,IDisposable
    {
        private  char m_OpenCode = (char)2;
        private  char m_CloseCode = (char)3;
        private  char m_Startcode = (char)2;
        private  char m_Endcode = (char)3;

        protected SerialPort m_sp = null;
        private ConnectionType m_ConnType = ConnectionType.Serial;

        private Param_Base m_ConnParam=null;

        private Scanner_SckCommService m_SckSerivce = null;


        public override event RunMessageEventHandler OnRunMessage;
        public override event ReadStringArrivedHandler OnBarcodeValue;
        public override ScanType DeviceType
        {
            get { return ScanType.Cognex; }
        }

        public byte OpenCode
        {
            get { return Convert.ToByte(m_OpenCode); }
            set { m_OpenCode = (char)value; }
        }

        public byte CloseCode
        {
            get { return Convert.ToByte(m_CloseCode); }
            set { m_CloseCode = (char)value; }
        }

        public byte StartCode
        {
            get { return Convert.ToByte(m_Startcode); }
            set { m_Startcode = (char)value; }
        }

        public byte EndCode
        {
            get { return Convert.ToByte(m_Endcode); }
            set { m_Endcode = (char)value; }
        }

        public Scanner_Cognex(Param_Base connParam)
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
                    m_SckSerivce = new Scanner_SckCommService(param);
                    m_SckSerivce.OnBarcodeValue += new ReadStringArrivedHandler(SckSerivce_OnBarcodeValue);
                    m_SckSerivce.OnRunMessage+=new RunMessageEventHandler(SckSerivce_OnRunMessage);
                    m_SckSerivce.OpenConnect();

                    m_ConnParam.State = ConnectionState.Connecting;
                    m_ConnParam.LastOperationTime = DateTime.Now;
                    return true;
                }
            }
            catch (System.Exception ex)
            {
                SendMessage(OnRunMessage,"打开扫描端口出错,原因:" + ex.Message);
                if (m_ConnType == ConnectionType.Serial && m_sp!=null)  m_sp.Close();
                if (m_ConnType == ConnectionType.TCP && m_SckSerivce != null) m_SckSerivce.CloseConnect();

                return false;
            }
        }

        private void SckSerivce_OnRunMessage(object sender, string strMsg)
        {
            SendMessage(OnRunMessage, strMsg);
        }

        private void SckSerivce_OnBarcodeValue(string strBarcode)
        {
            if (OnBarcodeValue != null) OnBarcodeValue(strBarcode.TrimEnd(new char[] { (char)13, (char)10 }));
        }


        public override bool Disconnect()
        {
            if (m_ConnType == ConnectionType.Serial)
            {
                if (m_sp != null)
                {
                    m_sp.Close();
                    m_sp.DataReceived -= new SerialDataReceivedEventHandler(SerialPort_DataReceived);
                    m_sp.Dispose();
                    m_sp = null;
                }
            }
            else
            {
                if (m_SckSerivce != null)
                {
                    m_SckSerivce.CloseConnect();
                    m_SckSerivce = null;
                }
            }

            m_ConnParam.State = ConnectionState.Disconnected;
            m_ConnParam.LastOperationTime = DateTime.Now;
            SendMessage(OnRunMessage, "已断开连接!");

            return true;
        }

        public void Dispose()
        {
            if (m_ConnType == ConnectionType.Serial)
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
            else
            {
                if (m_SckSerivce != null)
                {
                    m_SckSerivce.CloseConnect();
                    m_SckSerivce.OnBarcodeValue-=new ReadStringArrivedHandler(SckSerivce_OnBarcodeValue);
                    m_SckSerivce.OnRunMessage-=new RunMessageEventHandler(SckSerivce_OnRunMessage);
                    m_SckSerivce.Dispose();
                    m_SckSerivce = null;
                }
            }
        }

        public override string Read()
        {
            string result = string.Empty;
            if (m_ConnType == ConnectionType.Serial)
            {
                if (m_sp != null && m_sp.IsOpen)
                {
                    result = m_sp.ReadLine();
                    m_ConnParam.LastOperationTime = DateTime.Now;
                }
            }
            else
            {
                if (m_SckSerivce != null)
                {
                    result = m_SckSerivce.ReadData();
                }
            }
            return result;
        }

        /// <summary>
        /// 发送数据,包含前后缀
        /// </summary>
        /// <param name="strData"></param>
        /// <returns></returns>
        public override bool Write(string strData)
        {
            if (strData.Length == 0) return false;

            strData = strData.ToUpper();

            if (strData.Substring(0, 1) != "|") strData = "||>" + strData;
            if (strData.ToCharArray()[strData.Length - 1] != (char)10) strData = strData + "\r\n";
       
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
                if (m_SckSerivce != null)
                {
                    m_SckSerivce.WriteData(strData);
                }
                return true;
            }
        }

        /// <summary>
        /// 发送数据,包含前后缀
        /// </summary>
        /// <param name="bytData"></param>
        /// <returns></returns>
        public override bool Write(byte[] bytData)
        {
            if (bytData.Length == 0) return false;

            int bytCount = bytData.Length;
           
            byte[] bytPrefix = null;
            byte[] bytSuffix = null;

            byte[] bytsCmd =null;
          
            if (bytData[0] != 124)
            {
                //如果没有前缀
                bytPrefix = new byte[] {124,124,62 };
                bytCount += bytPrefix.Length;
            }
            
            if (bytData[bytData.Length-1] != 10)
            {
                //如果没有后缀 
                bytSuffix = new byte[] { 13, 10 };
                bytCount += bytSuffix.Length;
            }

            bytsCmd = new byte[bytCount];
            if (bytPrefix != null)
            {
                bytPrefix.CopyTo(bytsCmd, 0);
                bytData.CopyTo(bytsCmd, bytPrefix.Length);
            }
            else
            {
                bytData.CopyTo(bytsCmd, 0);
            }

            if (bytSuffix != null) bytSuffix.CopyTo(bytsCmd, bytsCmd.Length-bytSuffix.Length);

            if (m_ConnType == ConnectionType.Serial)
            {
                if (m_sp != null && m_sp.IsOpen)
                {
                    m_sp.Write(bytData, 0, bytsCmd.Length);
                    m_ConnParam.LastOperationTime = DateTime.Now;
                    return true;
                }
                else
                    return false;
            }
            else
            {
                if (m_SckSerivce != null)
                {
                    m_SckSerivce.WriteData(bytsCmd);
                }
                return true;
            }
        }

        /// <summary>
        /// 打开激光线
        /// </summary>
        /// <returns></returns>
        public bool OpenLaserLine()
        {
            if (m_ConnType == ConnectionType.Serial)
            {
                if (m_sp != null && m_sp.IsOpen)
                {
                    m_sp.Write(m_OpenCode.ToString());
                    m_ConnParam.LastOperationTime = DateTime.Now;
                    return true;
                }
                else
                    return false;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 关闭激光线
        /// </summary>
        /// <returns></returns>
        public bool CloseLaserLine()
        {
            if (m_ConnType == ConnectionType.Serial)
            {
                if (m_sp != null && m_sp.IsOpen)
                {
                    m_sp.Write(m_CloseCode.ToString());
                    m_ConnParam.LastOperationTime = DateTime.Now;
                    return true;
                }
                else
                    return false;
            }
            else
            {
                return false;
            }
        }

        private StringBuilder sbReadString = new StringBuilder();
        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                if (m_sp == null || m_sp.IsOpen == false) return;
                int length = m_sp.BytesToRead;
                byte[] receive = new byte[length];
                bool isBarcode = false;
                m_sp.Read(receive, 0, length);
                foreach (byte b in receive)
                {
                    if (b == StartCode)
                        sbReadString = new StringBuilder();
                    if ((b <= '9' && b >= '0') || (b <= 'Z' && b >= 'A') || (b <= 'z' && b >= 'a'))
                        sbReadString.Append((char)b);
                    if (b == EndCode)
                    {
                        isBarcode = true;
                        break;
                    }
                }
                if (isBarcode)
                {
                    if (OnBarcodeValue != null) OnBarcodeValue(sbReadString.ToString());
                    sbReadString = new StringBuilder();
                }
            }
            catch (System.Exception ex)
            {
                SendMessage(OnRunMessage, "读取扫描条码出错,原因:" + ex.Message);
            }
        }

        private bool SendCommandBySck(string strCmd, Param_Ethernet param)
        {
            byte[] sendBytes = new byte[1024];
            Encoding encoder = System.Text.Encoding.GetEncoding("ASCII");
            sendBytes = encoder.GetBytes(strCmd);

            return SendCommandBySck(sendBytes, param);

        }

        private bool SendCommandBySck(byte[] bytData, Param_Ethernet param)
        {
            try
            {
                if (bytData == null || param == null)
                {
                    SendMessage(OnRunMessage,"参数为空,无法发送命令!");
                    return false;
                }

                IPAddress printerip;
                if (IPAddress.TryParse(param.IPAddress, out printerip))
                {

                    IPEndPoint ipPoint = new IPEndPoint(printerip, param.Port);
                    Socket clientSck = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    clientSck.Connect(ipPoint);
                    clientSck.Send(bytData, bytData.Length, 0);
                    System.Threading.Thread.Sleep(200);
                    clientSck.Close();
                    clientSck = null;
                    return true;
                }
                else
                {
                    SendMessage(OnRunMessage, string.Format("输入的IP地址<{0}>错误,无法连接!", param.IPAddress));
                    return false;
                }
            }
            catch (Exception ex)
            {
                SendMessage(OnRunMessage, "<SendCommandBySck>出错,原因:" + ex.Message);
                return false;
            }
        }


    }
}
