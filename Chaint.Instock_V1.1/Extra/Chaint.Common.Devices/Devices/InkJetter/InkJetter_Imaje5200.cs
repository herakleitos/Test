using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO.Ports;

namespace Chaint.Common.Devices.Devices
{
    public class InkJetter_Imaje5200:InkJetter,IDisposable
    {
        protected SerialPort m_sp = null;
        private ConnectionType m_ConnType = ConnectionType.Serial;
        private Param_Base m_ConnParam = null;

        public override event RunMessageEventHandler OnRunMessage;
        public override event ReadStringArrivedHandler OnInkJetRetValue;
        public override InkJetterType DeviceType
        {
            get { return InkJetterType.Imaje5200; }
        }

        public InkJetter_Imaje5200(Param_Base connParam)
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

                    if(m_sp==null)
                    {
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
                        SendMessage(OnRunMessage, string.Format("打开喷码端口<{0}>成功!", param.PortName));
                    }
                    else
                    {
                        SendMessage(OnRunMessage, "已存在喷码类!");
                    }
                    return true;
                }
                else
                {
                    m_ConnParam.State = ConnectionState.Error;
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

            for (int i = 0; i < strLines.Length; i++)
            {
                sbJetInfo.Append(strLines);
            }
            
            if(m_ConnType==ConnectionType.Serial)
            {
                Param_SerialPort param = m_ConnParam as Param_SerialPort;
                m_ConnParam.State = ConnectionState.Connected;

                string strCmdLines=GetJetCommandStr_By_SerialPort(strLines);
                if(m_sp!=null && m_sp.IsOpen)
                {
                    m_sp.Write(strCmdLines);
                    SendMessage(OnRunMessage, sbJetInfo.ToString());     //显示喷码信息
                    return true;
                }
                else
                {
                    SendMessage(OnRunMessage,"喷码端口未打开");     
                    return false;
                } 
            }
            else
            {
                Param_Ethernet param = m_ConnParam as Param_Ethernet;
                string strCmdLines="";
                if (param.CommunicateMode == Param_Ethernet.CommMode.CLISERVICE)
                    strCmdLines = GetJetCommandStr_By_CLI(param.DeviceName, param.JobName, strLines);
                else
                    strCmdLines = GetJetCommandStr_By_SerialPort(strLines);

                bool blnSucc=SendPrintContentBySck(strCmdLines);

                return blnSucc;
            }
        }


        /// <summary>
        /// 按串口方式向喷码机发送喷码命令(包含按串口服务器的方式）
        /// </summary>
        /// <param name="strLines"></param>
        /// <returns></returns>
        private string GetJetCommandStr_By_SerialPort(string[] strLines)
        {
            StringBuilder sbJetInfo = new StringBuilder();

            //^UV<Sub Command>|<Number of Prints>|<Number Of Fields> |<Field Number>|<Data 1>|... |<Field Number>|<Data >| 
            sbJetInfo.AppendFormat("{0}{1}UV0|0|{2}|", (char)1, (char)94, strLines.Length);
            for (int i = 0; i < strLines.Length; i++)
            {
                //UV0 | 0 | 1 | 0 | 222222 |
                sbJetInfo.AppendFormat("{0}|{1}|", i, strLines[i]);
            }
            sbJetInfo.AppendFormat("{0}", (char)26);

            return sbJetInfo.ToString();
        }

        /// <summary>
        /// devices|lookup|5400|select|427|TEXT=789
        /// </summary>
        /// <param name="strDeviceName">设备名称</param>
        /// <param name="strJobName">作业名称</param>
        /// <param name="strLines">发送的需要更新的变量值 格式为: 变量名=变量值(TEXT=1234)</param>
        /// <returns></returns>
        private string GetJetCommandStr_By_CLI(string strDeviceName,string strJobName,string[] strLines)
        {
            StringBuilder sbJetInfo = new StringBuilder();
            sbJetInfo.AppendFormat("devices|lookup|{0}|select|{1}", strDeviceName, strJobName);
           
            //变量名与变量值对
            for (int i = 0; i < strLines.Length; i++)
            {
                //TEXT=789
                sbJetInfo.AppendFormat("|{0}",strLines[i]);
            }

            sbJetInfo.AppendFormat("{0}{1}", (char)13, (char)10);       //以回车换行符结束
            return sbJetInfo.ToString();
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

        private bool SendPrintContentBySck(string strPrintContent)
        {
            IPAddress printerip;
            Byte[] sendBytes = new Byte[4096];
            Encoding encoder = System.Text.Encoding.UTF8;

            string retMsg = "";

            string strIPAddress = "";
            int intPort = 4001;
            try
            {
                if (m_ConnParam is Param_Ethernet)
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

                    //异步
                   // clientSck.BeginSend(sendBytes, 0, sendBytes.Length, SocketFlags.None, null, null);

                    System.Threading.Thread.Sleep(100);
                    clientSck.Disconnect(true);
                    clientSck.Close();
                    clientSck.Dispose();
                    clientSck = null;
                    return true;
                }
                else
                {
                    retMsg = string.Format("IP地址【{0}】输入有误，请检查IP配置是否正确!", strIPAddress);
                    SendMessage(OnRunMessage, retMsg);
                    return false;
                }
            }
            catch (Exception ex)
            {
                retMsg = "Imaje喷码机未连接成功,原因:" + ex.Message;
                SendMessage(OnRunMessage, retMsg);
                return false;
            }
        }

    }
}
