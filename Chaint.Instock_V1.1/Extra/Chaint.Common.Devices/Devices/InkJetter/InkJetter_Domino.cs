using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace Chaint.Common.Devices.Devices
{
    public class InkJetter_Domino:InkJetter,IDisposable
    {
        protected SerialPort m_sp = null;
        private ConnectionType m_ConnType = ConnectionType.Serial;

        private Param_Base m_ConnParam = null;

        public override event RunMessageEventHandler OnRunMessage;
        public override event ReadStringArrivedHandler OnInkJetRetValue;
        public override InkJetterType DeviceType
        {
            get { return InkJetterType.Domino; }
        }

        public InkJetter_Domino(Param_Base connParam)
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
  
            if(m_ConnType==ConnectionType.Serial)
            {
                if (!m_sp.IsOpen) m_sp.Open();
                m_sp.Write(string.Format("{0}R{1}", (char)27, (char)4));
                m_sp.Write(string.Format("{0}P1001{1}", (char)27, (char)4));
                m_sp.Write(string.Format("{0}P2002{1}", (char)27, (char)4));
                switch (strLines.Length)
                {
                    case 4:
                        //喷1-2行
                        //mscJet.Output = Chr(27) & "S001" & strLine(0) & Chr(27) & "r" & strLine(1) & Chr(4)
                        //喷3-4行
                        //mscJet.Output = Chr(27) & "S002" & strLine(2) & Chr(27) & "r" & strLine(3) & Chr(4)
                        m_sp.Write(string.Format("{0}S001{1}{2}r{3}{4}", (char)27, strLines[0], (char)27, strLines[1], (char)4));
                        m_sp.Write(string.Format("{0}S002{1}{2}r{3}{4}", (char)27, strLines[2], (char)27, strLines[3], (char)4));
                        break;
                    case 2:
                        //喷1行
                        //mscJet.Output = Chr(27) & "S001" & Chr(27) & "u2" & strLine(0) & Chr(27) & "u1" & Chr(4)
                        //喷2行
                        //mscJet.Output = Chr(27) & "S002" & Chr(27) & "u2" & strLine(1) & Chr(27) & "u1" & Chr(4)
                        m_sp.Write(string.Format("{0}S001{1}u2{2}{3}u1{4}", (char)27, (char)27, strLines[0], (char)27, (char)4));
                        m_sp.Write(string.Format("{0}S002{1}u2{2}{3}u1{4}", (char)27, (char)27, strLines[1], (char)27, (char)4));
                        break;
                }
                //mscJet.Output = Chr(27) & "S001" & "?" & Chr(4)
                //mscJet.Output = Chr(27) & "S002" & "?" & Chr(4)
                m_sp.Write(string.Format("{0}S001?{1}", (char)27, (char)4));
                m_sp.Write(string.Format("{0}S002?{1}", (char)27, (char)4));
                System.Threading.Thread.Sleep(200);


                //显示喷码信息  
                StringBuilder sbJetInfo = new StringBuilder();
                for (int i = 0; i < strLines.Length; i++)
                {
                    sbJetInfo.AppendFormat("{0}",strLines[i]);
                }
                SendMessage(OnRunMessage, sbJetInfo.ToString());             
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

        private StringBuilder m_sbJetRetMsg = new StringBuilder();
        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                if (m_sp == null || m_sp.IsOpen == false) return;
               
                int length = m_sp.BytesToRead;
                byte[] receive = new byte[length];
                m_sp.Read(receive, 0, length);
                foreach (byte b in receive)
                {
                    if ((char)b == (char)4)
                    {
                        if (m_sbJetRetMsg.ToString().Trim().Length > 0)
                        {
                            if (OnInkJetRetValue != null) OnInkJetRetValue(m_sbJetRetMsg.ToString());
                            m_sbJetRetMsg = new StringBuilder();
                        }
                    }
                    else
                    {
                        if ((char)b == (char)6) continue;
                        m_sbJetRetMsg.AppendFormat("{0}", (char)b);
                    }
                }
            }
            catch (System.Exception ex)
            {
                SendMessage(OnRunMessage, "读取喷码返回值出错,原因:" + ex.Message);
            }
        }

    }
}
