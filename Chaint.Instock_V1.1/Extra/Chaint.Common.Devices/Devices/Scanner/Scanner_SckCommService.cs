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
        private DateTime m_LastKATime = DateTime.Now;          //���һ�η���KA��ʱ�� 
        private int m_SendKATimeOut = 20;                      //����KA��ʱ����
        private int m_ReConnTimeOut =30;                       //����ʱ����Ҫ����KAʱ����


        private string m_kaCommands = string.Format("||>GET COM.DMCC-RESPONSE\r\n");
        private char m_EndChr = (char)10;
      
        private ClientSocket m_ClientSck = null;
        private Param_Ethernet m_ConnectParam = null;
        private System.Threading.Thread m_KAThread = null;

        public event RunMessageEventHandler OnRunMessage;
        public event ReadStringArrivedHandler OnBarcodeValue;

        /// <summary>
        /// �Զ���һ��KeepAliveָ��,���û�ж���KA���豸,���Զ���ָ����Ҫ�з���
        /// δ������β��ʽ�ַ�
        /// </summary>
        public string CustomKACommand
        {
            get { return m_kaCommands; }
            set{m_kaCommands=value;}
        }

        /// <summary>
        /// ����״̬
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
        /// ����TCP ��������
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
                { SendMessage("���ӷ������˿ڶ������<0---65535>!", true); }
            }
            else
            { SendMessage("��������IP��ַ�������!", true); }
        }

        /// <summary>
        /// �ر�����
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
        /// �ͷ��̵߳���Դ
        /// </summary>
        public void Dispose()
        {
           
            CloseConnect();
        }

        /// <summary>
        /// ��������
        /// </summary>
        public void Reconnect()
        {
            m_LastConnTime = DateTime.Now;

            SendMessage(string.Format("�������<{0}>��δ���ӵ������,ϵͳ�����½����µ�����!", m_ReConnTimeOut), false);
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
                SendMessage("���Ӳ��ɹ���δ��ʼ��,��������ʧ��", true);
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
                SendMessage("���Ӳ��ɹ���δ��ʼ��,��������ʧ��", true);
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
                SendMessage("���Ӳ��ɹ���δ��ʼ��,��ȡ����ʧ��", true);
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
                   
                    //(1) ����KeepAlive
                    if (m_LastKATime.AddSeconds(m_SendKATimeOut) <= DateTime.Now)
                    {
                        SendKeepAlive();    //ֻҪ�Է��ڹ涨ʱ�����з�������û�б�Ҫ�ٷ�KA
                    }

                    //(2) ��������
                    if (m_LastConnTime.AddSeconds(m_ReConnTimeOut) <= DateTime.Now)
                    {
                        Reconnect();
                    }
                }
                catch (System.Exception ex)
                {
                    SendMessage("<ThreadKeepAlive>����,ԭ��:" + ex.Message, true);
                }

                System.Threading.Thread.Sleep(2000);
            }
        }

        /// <summary>
        /// �����Ӻ�
        /// </summary>
        /// <param name="l_sck"></param>
        private void ClientSck_OnConnect(Socket l_sck)
        {
            m_LastConnTime = DateTime.Now;
            string sndMsg = string.Format("�����ӷ����: {0}", l_sck.RemoteEndPoint.ToString());
            SendMessage(sndMsg, false);

            if (m_KAThread == null)
            {
                m_KAThread = new System.Threading.Thread(new System.Threading.ThreadStart(ThreadKeepAlive));
                m_KAThread.Start();
            }
        }

        /// <summary>
        /// �Ͽ�����ʱ
        /// </summary>
        /// <param name="l_sck"></param>
        private void ClientSck_OnDisconnect(Socket l_sck)
        {
            string sndMsg = string.Format("�ѶϿ������: {0}", l_sck.RemoteEndPoint.ToString());
            SendMessage(sndMsg, false);
        }

        /// <summary>
        /// ���ִ���ʱ
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
                sndMsg = string.Format("�����׽���:{0}  ����:{1}", strErrMsg, ErrCode);
            }
        }

        /// <summary>
        /// ���ӷ������˶�ȡ��Ϣ
        /// </summary>
        /// <param name="l_sck"></param>
        private void ClientSck_OnRead(Socket l_sck)
        {
            try
            {
                m_LastKATime = DateTime.Now;
                m_LastConnTime = DateTime.Now;

                string RevTxt = m_ClientSck.ReceivedText.TrimEnd('\0');
                string rndMsg = string.Format("��Rev��:{0}", RevTxt);

                if (RevTxt == "") return;
                string[] strRevItems = RevTxt.Split(new char[] { m_EndChr }, StringSplitOptions.RemoveEmptyEntries);
                if (strRevItems.Length < 1) return;

                //������Ϣ�������
                for (int intdexItem = 0; intdexItem < strRevItems.Length; intdexItem++)
                {
                    if (OnBarcodeValue != null ) OnBarcodeValue(strRevItems[intdexItem]);
                }
            }
            catch (System.Exception ex)
            {
                SendMessage("<ClientSck_OnRead>����,ԭ��:" + ex.Message, true);
            }
        }

        /// <summary>
        /// �����������д����Ϣ
        /// </summary>
        /// <param name="l_sck"></param>
        private void ClientSck_OnWrite(Socket l_sck)
        {
            m_LastConnTime = DateTime.Now;
            m_LastKATime = DateTime.Now;

            if (m_ClientSck == null) return;

            string strRevMsg = m_ClientSck.WriteText.TrimEnd('\0');
            SendMessage("��Snd��:" + strRevMsg,false);
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
