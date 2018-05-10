using System;
using System.Collections.Generic;
using System.Text;

using System.Net;
using System.Net.Sockets;

using Chaint.Common.Devices.Net;


namespace Chaint.Common.Devices.Devices
{
    public class Scanner_SckCommService:IDisposable
    {
        private delegate void CallbackSocketProcess(Socket sck);

        private DateTime m_LastConnTime = DateTime.Now;
        private DateTime m_LastKATime = DateTime.Now;          //最近一次发送KA的时间 
        private int m_SendKATimeOut = 20;                      //发送KA的时间间隔
        private int m_ReConnTimeOut =30;                       //重连时间间隔要大于KA时间间隔


        private string m_kaCommands = string.Format("||>GET COM.DMCC-RESPONSE\r\n");
        private char m_EndChr = (char)10;
      
        private ClientSocket m_ClientSck = null;
        private Param_Ethernet m_ConnectParam = null;
        private System.Threading.Thread m_KAThread = null;

        public event RunMessageEventHandler OnRunMessage;
        public event ReadStringArrivedHandler OnBarcodeValue;

        /// <summary>
        /// 自定义一个KeepAlive指令,针对没有定义KA的设备,且自定义指令需要有反馈
        /// 未包含首尾格式字符
        /// </summary>
        public string CustomKACommand
        {
            get { return m_kaCommands; }
            set{m_kaCommands=value;}
        }

        /// <summary>
        /// 连接状态
        /// </summary>
        public bool SckConnectState
        {
            get {
                if (m_ClientSck == null) return false;
                else
                    return m_ClientSck.Connected; 
            }
        }

        public Scanner_SckCommService(Param_Ethernet param)
        {
            m_ConnectParam = param;

            m_ReConnTimeOut = m_SendKATimeOut + 10;
        }

        /// <summary>
        /// 连接TCP 服务器端
        /// </summary>
        /// <param name="strIP"></param>
        /// <param name="intPort"></param>
        public void OpenConnect()
        {
            IPAddress ip;
            if (IPAddress.TryParse(m_ConnectParam.IPAddress, out ip))
            {
                if (m_ConnectParam.Port < 65536)
                {
                    m_ClientSck = new ClientSocket(m_ConnectParam.IPAddress, m_ConnectParam.Port);
                    m_ClientSck.OnConnect += new ClientSocket.ConnectionDelegate(ClientSck_OnConnect);
                    m_ClientSck.OnDisconnect += new ClientSocket.ConnectionDelegate(ClientSck_OnDisconnect);
                    m_ClientSck.OnError += new ClientSocket.ErrorDelegate(ClientSck_OnError);
                    m_ClientSck.OnRead += new ClientSocket.ConnectionDelegate(ClientSck_OnRead);
                    m_ClientSck.OnWrite += new ClientSocket.ConnectionDelegate(ClientSck_OnWrite);
                    bool isConnected  = m_ClientSck.Connect();
                }
                else
                { SendMessage("连接服务器端口定义错误<0---65535>!", true); }
            }
            else
            { SendMessage("服务器端IP地址定义错误!", true); }
        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        public void CloseConnect()
        {
            if (m_ClientSck != null)
            {
                m_ClientSck.Disconnect();
                m_ClientSck.OnConnect -= new ClientSocket.ConnectionDelegate(ClientSck_OnConnect);
                m_ClientSck.OnDisconnect -= new ClientSocket.ConnectionDelegate(ClientSck_OnDisconnect);
                m_ClientSck.OnError -= new ClientSocket.ErrorDelegate(ClientSck_OnError);
                m_ClientSck.OnRead -= new ClientSocket.ConnectionDelegate(ClientSck_OnRead);
                m_ClientSck.OnWrite -= new ClientSocket.ConnectionDelegate(ClientSck_OnWrite);

                m_ClientSck = null;
            }

            if (m_KAThread != null)
            {
                m_KAThread.Abort();
                m_KAThread = null;
            }
        }

        /// <summary>
        /// 释放线程等资源
        /// </summary>
        public void Dispose()
        {
           
            CloseConnect();
        }

        /// <summary>
        /// 重新连接
        /// </summary>
        public void Reconnect()
        {
            m_LastConnTime = DateTime.Now;

            SendMessage(string.Format("由于相隔<{0}>秒未连接到服务端,系统将重新建立新的连接!", m_ReConnTimeOut), false);
            CloseConnect();
            System.Threading.Thread.Sleep(200);
            OpenConnect();
        }

        public bool WriteData(byte[] bytData)
        {
            if (m_ClientSck != null && m_ClientSck.Connected)
            {
                m_ClientSck.SendText(Encoding.ASCII.GetString(bytData));
                return true;
            }
            else
            {
                SendMessage("连接不成功或未初始化,发送数据失败", true);
                return false;
            }
        }

        public bool WriteData(string strData)
        {
            if (m_ClientSck != null && m_ClientSck.Connected)
            {
                m_ClientSck.SendText(strData);
                return true;
            }
            else
            {
                SendMessage("连接不成功或未初始化,发送数据失败", true);
                return false;
            }
        }
        public string ReadData()
        {
            if (m_ClientSck != null)
            {
                return m_ClientSck.ReceivedText;
            }
            else
            {
                SendMessage("连接不成功或未初始化,读取数据失败", true);
                return string.Empty;
            }
        }
        private void ThreadKeepAlive()
        {
            while (true)
            {
                try
                {

                    if (m_KAThread == null || m_KAThread.ThreadState == System.Threading.ThreadState.Stopped || m_KAThread.ThreadState == System.Threading.ThreadState.Aborted) return;
                   
                    //(1) 发送KeepAlive
                    if (m_LastKATime.AddSeconds(m_SendKATimeOut) <= DateTime.Now)
                    {
                        SendKeepAlive();    //只要对方在规定时间内有反馈，就没有必要再发KA
                    }

                    //(2) 重新连接
                    if (m_LastConnTime.AddSeconds(m_ReConnTimeOut) <= DateTime.Now)
                    {
                        Reconnect();
                    }
                }
                catch (System.Exception ex)
                {
                    SendMessage("<ThreadKeepAlive>出错,原因:" + ex.Message, true);
                }

                System.Threading.Thread.Sleep(2000);
            }
        }

        /// <summary>
        /// 当连接后
        /// </summary>
        /// <param name="l_sck"></param>
        private void ClientSck_OnConnect(Socket l_sck)
        {
            m_LastConnTime = DateTime.Now;
            string sndMsg = string.Format("已连接服务端: {0}", l_sck.RemoteEndPoint.ToString());
            SendMessage(sndMsg, false);

            if (m_KAThread == null)
            {
                m_KAThread = new System.Threading.Thread(new System.Threading.ThreadStart(ThreadKeepAlive));
                m_KAThread.Start();
            }
        }

        /// <summary>
        /// 断开连接时
        /// </summary>
        /// <param name="l_sck"></param>
        private void ClientSck_OnDisconnect(Socket l_sck)
        {
            string sndMsg = string.Format("已断开服务端: {0}", l_sck.RemoteEndPoint.ToString());
            SendMessage(sndMsg, false);
        }

        /// <summary>
        /// 出现错误时
        /// </summary>
        /// <param name="ErroMsg"></param>
        /// <param name="l_sck"></param>
        /// <param name="ErroCode"></param>
        private void ClientSck_OnError(string strErrMsg, Socket l_sck, int ErrCode)
        {
            string sndMsg = null;
            if (ErrCode == 0)
            {
                System.Threading.Thread.Sleep(100);
                if (m_ClientSck != null)
                {
                    m_ClientSck.Disconnect();
                    m_ClientSck.OnConnect -= new ClientSocket.ConnectionDelegate(ClientSck_OnConnect);
                    m_ClientSck.OnDisconnect -= new ClientSocket.ConnectionDelegate(ClientSck_OnDisconnect);
                    m_ClientSck.OnError -= new ClientSocket.ErrorDelegate(ClientSck_OnError);
                    m_ClientSck.OnRead -= new ClientSocket.ConnectionDelegate(ClientSck_OnRead);
                    m_ClientSck.OnWrite -= new ClientSocket.ConnectionDelegate(ClientSck_OnWrite);
                }

                return;
            }
            if (l_sck == null || ErrCode == 10054) return;

            if (l_sck != null)
            {
                sndMsg = string.Format("错误套接字:{0}  代码:{1}", strErrMsg, ErrCode);
            }
        }

        /// <summary>
        /// 当从服务器端读取信息
        /// </summary>
        /// <param name="l_sck"></param>
        private void ClientSck_OnRead(Socket l_sck)
        {
            try
            {
                m_LastKATime = DateTime.Now;
                m_LastConnTime = DateTime.Now;

                string RevTxt = m_ClientSck.ReceivedText.TrimEnd('\0');
                string rndMsg = string.Format("【Rev】:{0}", RevTxt);

                if (RevTxt == "") return;
                string[] strRevItems = RevTxt.Split(new char[] { m_EndChr }, StringSplitOptions.RemoveEmptyEntries);
                if (strRevItems.Length < 1) return;

                //逐条消息处理过程
                for (int intdexItem = 0; intdexItem < strRevItems.Length; intdexItem++)
                {
                    if (OnBarcodeValue != null ) OnBarcodeValue(strRevItems[intdexItem]);
                }
            }
            catch (System.Exception ex)
            {
                SendMessage("<ClientSck_OnRead>出错,原因:" + ex.Message, true);
            }
        }

        /// <summary>
        /// 当向服务器端写入信息
        /// </summary>
        /// <param name="l_sck"></param>
        private void ClientSck_OnWrite(Socket l_sck)
        {
            m_LastConnTime = DateTime.Now;
            m_LastKATime = DateTime.Now;

            if (m_ClientSck == null) return;

            string strRevMsg = m_ClientSck.WriteText.TrimEnd('\0');
            SendMessage("【Snd】:" + strRevMsg,false);
        }

        private void SendKeepAlive()
        {
            m_LastKATime = DateTime.Now;
            if (CustomKACommand != "" && m_ClientSck!=null)
            {
                m_ClientSck.SendText(CustomKACommand);
            }
        }

        private void SendMessage(string strMsg, bool blnError)
        {
            if (OnRunMessage != null) OnRunMessage(this, strMsg);
        }

    }
}
