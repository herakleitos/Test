using System;
using System.Text;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Sockets;

/* 说明： TCP异步通讯类,包括客户端及服务器端
 * 
 */

namespace Chaint.Common.Devices.Net
{

    /// <summary>
    /// TCP 异步通讯类客户端
    /// </summary>
    public class ClientSocket
    {
        #region Delegates
        public delegate void ConnectionDelegate(Socket soc);
        public delegate void ErrorDelegate(string ErroMessage, Socket soc, int ErroCode);
        #endregion

        #region Events
        public event ConnectionDelegate OnConnect;
        public event ConnectionDelegate OnDisconnect;
        public event ConnectionDelegate OnRead;
        public event ConnectionDelegate OnWrite;
        public event ErrorDelegate OnError;
        public event ConnectionDelegate OnSendFile;
        #endregion

        #region Variables
        private AsyncCallback WorkerCallBack;
        private Socket mainSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private IPEndPoint serverEndPoint;
        private byte[] dataBuffer = new byte[1024];
        private int mPort = 0;
        private byte[] mBytesReceived;
        private string mTextReceived = "";
        private string mTextSent = "";
        private string mRemoteAddress = "";
        private string mRemoteHost = "";
        #endregion

        #region Propetiers
        /// <summary>
        /// Port to connect to server
        /// </summary>
        public int Port
        {
            get
            {
                return (mPort);
            }
        }

        /// <summary>
        /// Bytes received by the Socket
        /// </summary>
        public byte[] ReceivedBytes
        {
            get
            {
                byte[] temp = null;
                if (mBytesReceived != null)
                {
                    temp = mBytesReceived;
                    mBytesReceived = null;
                }
                return (temp);
            }
        }

        /// <summary>
        /// Message received by the Socket
        /// </summary>
        public string ReceivedText
        {
            get
            {
                string temp = mTextReceived;
                mTextReceived = "";
                return (temp);
            }
        }

        /// <summary>
        /// Message send by the Socket
        /// </summary>
        public string WriteText
        {
            get
            {
                string temp = mTextSent;
                mTextSent = "";
                return (temp);
            }
        }

        /// <summary>
        /// IP Server
        /// </summary>
        public string RemoteAddress
        {
            get
            {
                if (mainSocket.Connected)
                    return (mRemoteAddress);
                else
                    return "";
            }
        }

        /// <summary>
        /// Host Server
        /// </summary>
        public string RemoteHost
        {
            get
            {
                if (mainSocket.Connected)
                    return (mRemoteHost);
                else
                    return "";
            }
        }

        /// <summary>
        /// Return true if the ClientSocket is connected to the Server
        /// </summary>
        public bool Connected
        {
            get
            {
                return (mainSocket.Connected);
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="port">Port to connection
        /// </param>
        public ClientSocket(string IP, int port)
        {
            try
            {
                mPort = port;
                IPAddress ipAddress = IPAddress.Parse(IP);
                mRemoteAddress = ipAddress.ToString();
                //                 IPHostEntry ipss = Dns.GetHostEntry(mRemoteAddress);
                //                 mRemoteHost = ipss.HostName;
                mRemoteHost = mRemoteAddress;
                serverEndPoint = new IPEndPoint(ipAddress, port);
            }
            catch (Exception ex)
            {
                if (OnError != null)
                    OnError(ex.Message, null, 0);
            }
        }
        #endregion

        #region Functions and Events
        /// <summary>
        /// Establishes connection with the IP and Port Server
        /// </summary>
        public bool Connect()
        {
            try
            {
                //Connect to Server
                mainSocket.BeginConnect(serverEndPoint, new AsyncCallback(ConfirmConnect), null);
                return true;
            }
            catch (ArgumentException ex)
            {
                if (OnError != null)
                    OnError(ex.Message, null, 0);
                return false;
            }
            catch (InvalidOperationException ex)
            {
                if (OnError != null)
                    OnError(ex.Message, null, 0);
                return false;
            }
            catch (SocketException se)
            {
                //Added 2010-08-17
                if (mainSocket != null)
                    mainSocket.Close();
                System.GC.Collect();

                //------------------------------
                if (OnError != null && se.ErrorCode != 10022 && se.ErrorCode != 10056)
                    OnError(se.Message, mainSocket, se.ErrorCode);
                return false;
            }
        }

        private void ConfirmConnect(IAsyncResult asyn)
        {
            try
            {
                mainSocket.EndConnect(asyn);
                WaitForData(mainSocket);
                if (OnConnect != null)
                    OnConnect(mainSocket);
            }
            catch (ObjectDisposedException se)
            {
                if (OnError != null)
                    OnError(se.Message, null, 0);
            }
            catch (SocketException se)
            {
                if (OnError != null)
                    OnError(se.Message, null, 0);

                //Added 2010-08-17
                if (mainSocket != null)
                    mainSocket.Close();
                System.GC.Collect();
            }
        }

        private void WaitForData(Socket soc)
        {
            try
            {
                if (WorkerCallBack == null)
                    WorkerCallBack = new AsyncCallback(OnDataReceived);
                soc.BeginReceive(dataBuffer, 0, dataBuffer.Length, SocketFlags.None, WorkerCallBack, null);
            }
            catch (SocketException se)
            {
                if (OnError != null)
                    OnError(se.Message, soc, se.ErrorCode);
            }
        }

        private void OnDataReceived(IAsyncResult asyn)
        {
            try
            {
                int iRx = mainSocket.EndReceive(asyn);
                if (iRx < 1)
                {
                    mainSocket.Close();
                    if (!mainSocket.Connected)
                        if (OnDisconnect != null)
                            OnDisconnect(mainSocket);
                }
                else
                {
                    mBytesReceived = dataBuffer;
                    char[] chars = new char[iRx + 1];
                    Decoder d = Encoding.UTF8.GetDecoder();
                    d.GetChars(dataBuffer, 0, iRx, chars, 0);
                    mTextReceived = new String(chars);
                    if (OnRead != null)
                        OnRead(mainSocket);
                    WaitForData(mainSocket);
                }
            }
            catch (ArgumentException se)
            {
                if (OnError != null)
                    OnError(se.Message, null, 0);
            }
            catch (InvalidOperationException se)
            {
                mainSocket.Close();
                if (!mainSocket.Connected)
                    if (OnDisconnect != null)
                        OnDisconnect(mainSocket);
                    else
                        if (OnError != null)
                            OnError(se.Message, null, 0);
            }
            catch (SocketException se)
            {
                if (OnError != null)
                    OnError(se.Message, mainSocket, se.ErrorCode);
                if (!mainSocket.Connected)
                    if (OnDisconnect != null)
                        OnDisconnect(mainSocket);
            }
        }

        /// <summary>
        /// Send a text message
        /// </summary>
        /// <param name="mens">Message</param>
        public bool SendText(string mens)
        {
            try
            {
                byte[] byData = System.Text.Encoding.UTF8.GetBytes(mens);
                int NumBytes = mainSocket.Send(byData);
                if (NumBytes == byData.Length)
                {
                    if (OnWrite != null)
                    {
                        mTextSent = mens;
                        OnWrite(mainSocket);
                    }
                    return true;
                }
                else
                    return false;
            }
            catch (ArgumentException se)
            {
                if (OnError != null)
                    OnError(se.Message, null, 0);
                return false;
            }
            catch (ObjectDisposedException se)
            {
                if (OnError != null)
                    OnError(se.Message, null, 0);
                return false;
            }
            catch (SocketException se)
            {
                if (OnError != null)
                    OnError(se.Message, mainSocket, se.ErrorCode);
                return false;
            }
            catch (Exception ex)
            {
                if (OnError != null)
                    OnError(ex.Message, mainSocket, 0);
                return false;
            }
        }

        /// <summary>
        /// Send file
        /// </summary>
        /// <param name="FileName">Path File</param>
        public bool SendFile(string FileName)
        {
            try
            {
                mainSocket.BeginSendFile(FileName, new AsyncCallback(FileSendCallback), mainSocket);
                return true;
            }
            catch (FileNotFoundException se)
            {
                if (OnError != null)
                    OnError(se.Message, null, 0);
                return false;
            }
            catch (ObjectDisposedException se)
            {
                if (OnError != null)
                    OnError(se.Message, null, 0);
                return false;
            }
            catch (SocketException se)
            {
                if (OnError != null)
                    OnError(se.Message, mainSocket, se.ErrorCode);
                return false;
            }
        }

        /// <summary>
        /// Send file
        /// </summary>
        /// <param name="FileName">Path File</param>
        /// <param name="PreString">Message sent before the file</param>
        /// <param name="PosString">Message sent after the File</param>
        public bool SendFile(string FileName, string PreString, string PosString)
        {
            try
            {
                byte[] preBuf = Encoding.UTF8.GetBytes(PreString);
                byte[] postBuf = Encoding.UTF8.GetBytes(PosString);
                mainSocket.BeginSendFile(FileName, preBuf, postBuf, 0, new AsyncCallback(FileSendCallback), mainSocket);
                return true;
            }
            catch (ArgumentException se)
            {
                if (OnError != null)
                    OnError(se.Message, null, 0);
                return false;
            }
            catch (ObjectDisposedException se)
            {
                if (OnError != null)
                    OnError(se.Message, null, 0);
                return false;
            }
            catch (SocketException se)
            {
                if (OnError != null)
                    OnError(se.Message, mainSocket, se.ErrorCode);
                return false;
            }
        }

        private void FileSendCallback(IAsyncResult ar)
        {
            Socket workerSocket = (Socket)ar.AsyncState;
            workerSocket.EndSendFile(ar);
            if (OnSendFile != null)
                OnSendFile(workerSocket);
        }

        /// <summary>
        /// Close connection to the server
        /// </summary>
        public bool Disconnect()
        {
            mainSocket.Close();
            if (!mainSocket.Connected)
                return true;
            else
                return false;
        }
        #endregion
    }

    /// <summary>
    /// TCP 异步通讯类服务器端
    /// </summary>
    public class ServerSocket
    {
        #region Delegates
        public delegate void ConnectionDelegate(Socket soc);
        public delegate void ErrorDelegate(string ErroMessage, Socket soc, int ErroCode);
        public delegate void ListenDelegate();
        #endregion

        #region Events
        public event ConnectionDelegate OnConnect;
        public event ConnectionDelegate OnDisconnect;
        public event ConnectionDelegate OnRead;
        public event ConnectionDelegate OnWrite;
        public event ErrorDelegate OnError;
        public event ListenDelegate OnListen;
        public event ConnectionDelegate OnSendFile;
        #endregion

        #region Variables
        private ArrayList Clients = ArrayList.Synchronized(new ArrayList());
        private AsyncCallback WorkerCallBack;
        private Socket mainSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private IPEndPoint serverEndPoint;
        private int mPort = 0;
        private byte[] mBytesReceived;
        private string mTextReceived = "";
        private string mTextSent = "";
        private bool mListening = false;  //Variable control
        #endregion

        #region Propetiers
        /// <summary>
        /// Port to connect with clients
        /// </summary>
        public int Port
        {
            get
            {
                return (mPort);
            }
        }

        /// <summary>
        /// Bytes received by the Socket
        /// </summary>
        public byte[] ReceivedBytes
        {
            get
            {
                byte[] temp = null;
                if (mBytesReceived != null)
                {
                    temp = mBytesReceived;
                    mBytesReceived = null;
                }
                return (temp);
            }
        }

        /// <summary>
        /// Message received by the Socket
        /// </summary>
        public string ReceivedText
        {
            get
            {
                string temp = mTextReceived;
                mTextReceived = "";
                return (temp);
            }
        }

        /// <summary>
        /// Message send by the Socket
        /// </summary>
        public string WriteText
        {
            get
            {
                string temp = mTextSent;
                mTextSent = "";
                return (temp);
            }
        }

        /// <summary>
        /// Number of active connections
        /// </summary>
        public int ActiveConnections
        {
            get
            {
                return (Clients.Count);
            }
        }
        /// <summary>
        /// 连接客户端集合
        /// </summary>
        public ArrayList ConnClients
        {
            get { return Clients; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="port">Port to wait for call</param>
        public ServerSocket(int port)
        {
            try
            {
                mPort = port;
                serverEndPoint = new IPEndPoint(IPAddress.Any, mPort);
            }
            catch (Exception ex)
            {
                if (OnError != null)
                    OnError(ex.Message, null, 0);
            }
        }
        #endregion

        #region Class
        private class SocketPacket
        {
            public SocketPacket(Socket soc)
            {
                m_currentSocket = soc;
            }
            public Socket m_currentSocket;
            public byte[] dataBuffer = new byte[1024];
        }
        #endregion

        #region Functions and Events
        /// <summary>
        /// Active waiting for the call
        /// </summary>
        public bool Active()
        {
            try
            {
                mListening = true;
                mainSocket.Bind(serverEndPoint);
                mainSocket.Listen(0);
                mainSocket.BeginAccept(new AsyncCallback(OnClientConnect), null);
                if (OnListen != null)
                    OnListen();
                return true;
            }
            catch (SocketException se)
            {
                mListening = false;
                if (OnError != null)
                    OnError(se.Message, mainSocket, se.ErrorCode);
                return false;
            }
        }

        private void OnClientConnect(IAsyncResult asyn)
        {
            try
            {
                if (mListening)
                {
                    Socket workSocket = mainSocket.EndAccept(asyn);
                    try
                    {
                        WaitForData(workSocket);
                        lock (this)
                        {
                            Clients.Add(workSocket);
                        }
                        OnConnect(workSocket);
                        mainSocket.BeginAccept(new AsyncCallback(OnClientConnect), null);
                    }
                    catch (SocketException se)
                    {
                        if (OnError != null)
                            OnError(se.Message, workSocket, se.ErrorCode);
                    }
                }
            }
            catch (ObjectDisposedException ex)
            {
                if (OnError != null)
                    OnError(ex.Message, null, 0);
            }
        }

        private void WaitForData(Socket soc)
        {
            try
            {
                if (WorkerCallBack == null) //Active only the beginning
                    WorkerCallBack = new AsyncCallback(OnDataReceived);
                SocketPacket theSocPkt = new SocketPacket(soc);
                soc.BeginReceive(theSocPkt.dataBuffer, 0, theSocPkt.dataBuffer.Length, SocketFlags.None, WorkerCallBack, theSocPkt);
            }
            catch (SocketException se)
            {
                if (OnError != null)
                    OnError(se.Message, soc, se.ErrorCode);
            }
        }

        private void OnDataReceived(IAsyncResult asyn)
        {
            SocketPacket socketData = (SocketPacket)asyn.AsyncState;
            try
            {
                int iRx = socketData.m_currentSocket.EndReceive(asyn);
                if (iRx < 1)
                {
                    socketData.m_currentSocket.Close();
                    if (!socketData.m_currentSocket.Connected)
                    {
                        if (OnDisconnect != null)
                            OnDisconnect(socketData.m_currentSocket);
                        Clients.Remove(socketData.m_currentSocket);
                        socketData.m_currentSocket = null;
                    }
                }
                else
                {
                    mBytesReceived = socketData.dataBuffer;
                    char[] chars = new char[iRx + 1];
                    Decoder d = Encoding.UTF8.GetDecoder();
                    d.GetChars(socketData.dataBuffer, 0, iRx, chars, 0);
                    mTextReceived = new String(chars);
                    if (OnRead != null)
                        OnRead(socketData.m_currentSocket);
                    WaitForData(socketData.m_currentSocket);
                }
            }
            catch (InvalidOperationException ex)
            {
                //if (socketData.m_currentSocket.Connected)
                //    socketData.m_currentSocket.Close();
                if (socketData.m_currentSocket.Connected)
                {
                    if (OnDisconnect != null)
                        OnDisconnect(socketData.m_currentSocket);
                    Clients.Remove(socketData.m_currentSocket);
                    socketData.m_currentSocket = null;
                }
                else
                    if (OnError != null)
                        OnError(ex.Message, null, 0);
            }
            catch (SocketException se)
            {
                if (OnError != null)
                    OnError(se.Message, socketData.m_currentSocket, se.ErrorCode);
                if (!socketData.m_currentSocket.Connected)
                {
                    if (OnDisconnect != null)
                        OnDisconnect(socketData.m_currentSocket);
                    Clients.Remove(socketData.m_currentSocket);
                    socketData.m_currentSocket = null;
                }
            }
        }

        /// <summary>
        /// Send a text messageby connecting selected
        /// </summary>
        /// <param name="mens">Message</param>
        /// <param name="SocketIndex">Index of connection</param>
        public bool SendText(string mens, int SocketIndex)
        {
            if ((Clients.Count - 1) >= SocketIndex)
            {
                Socket workerSocket = (Socket)Clients[SocketIndex];
                try
                {
                    byte[] byData = System.Text.Encoding.UTF8.GetBytes(mens);
                    int NumBytes = workerSocket.Send(byData);
                    if (NumBytes == byData.Length)
                    {
                        if (OnWrite != null)
                        {
                            mTextSent = mens;
                            OnWrite(workerSocket);
                        }
                        return true;
                    }
                    else
                        return false;
                }
                catch (ArgumentException ex)
                {
                    if (OnError != null)
                        OnError(ex.Message, null, 0);
                    return false;
                }
                catch (ObjectDisposedException ex)
                {
                    if (OnError != null)
                        OnError(ex.Message, null, 0);
                    return false;
                }
                catch (SocketException se)
                {
                    if (OnError != null)
                        OnError(se.Message, null, 0);
                    return false;
                }
            }
            else
            {
                if (OnError != null)
                    OnError("索引超出范围", null, 0);
                return false;
            }
        }

        /// <summary>
        /// Send file by connecting selected
        /// </summary>
        /// <param name="FileName">Path File</param>
        /// <param name="SocketIndex">Index of connection</param>
        public bool SendFile(string FileName, int SocketIndex)
        {
            if ((Clients.Count - 1) >= SocketIndex)
            {
                Socket workerSocket = (Socket)Clients[SocketIndex];
                try
                {
                    workerSocket.BeginSendFile(FileName, new AsyncCallback(FileSendCallback), workerSocket);
                    return true;
                }
                catch (FileNotFoundException se)
                {
                    if (OnError != null)
                        OnError(se.Message, null, 0);
                    return false;
                }
                catch (ObjectDisposedException se)
                {
                    if (OnError != null)
                        OnError(se.Message, null, 0);
                    return false;
                }
                catch (SocketException se)
                {
                    if (OnError != null)
                        OnError(se.Message, workerSocket, se.ErrorCode);
                    return false;
                }
            }
            else
            {
                if (OnError != null)
                    OnError("索引超出范围", null, 0);
                return false;
            }
        }

        /// <summary>
        /// Send file by connecting selected
        /// </summary>
        /// <param name="FileName">Path File</param>
        /// <param name="PreString">Message sent before the file</param>
        /// <param name="PosString">Message sent after the File</param>
        /// <param name="SocketIndex">Index of connection</param>
        public bool SendFile(string FileName, string PreString, string PosString, int SocketIndex)
        {
            if ((Clients.Count - 1) >= SocketIndex)
            {
                Socket workerSocket = (Socket)Clients[SocketIndex];
                try
                {
                    byte[] preBuf = Encoding.UTF8.GetBytes(PreString);
                    byte[] postBuf = Encoding.UTF8.GetBytes(PosString);
                    workerSocket.BeginSendFile(FileName, preBuf, postBuf, 0, new AsyncCallback(FileSendCallback), workerSocket);
                    return true;
                }
                catch (ArgumentException se)
                {
                    if (OnError != null)
                        OnError(se.Message, null, 0);
                    return false;
                }
                catch (ObjectDisposedException se)
                {
                    if (OnError != null)
                        OnError(se.Message, null, 0);
                    return false;
                }
                catch (SocketException se)
                {
                    if (OnError != null)
                        OnError(se.Message, workerSocket, se.ErrorCode);
                    return false;
                }
            }
            else
            {
                if (OnError != null)
                    OnError("索引超出范围", null, 0);
                return false;
            }
        }

        private void FileSendCallback(IAsyncResult ar)
        {
            Socket workerSocket = (Socket)ar.AsyncState;
            workerSocket.EndSendFile(ar);
            if (OnSendFile != null)
                OnSendFile(workerSocket);
        }

        /// <summary>
        /// Deactivates the ServerSocket closing all connections
        /// </summary>
        public bool Deactive()
        {
            mListening = false;
            if (mainSocket != null)
                mainSocket.Close();
            int total = Clients.Count;
            if (total == 0)
                return true;
            try
            {
                for (int i = 0; i < total; i++)
                {
                    Socket workerSocket = (Socket)Clients[i];
                    if (workerSocket != null)
                    {
                        if (OnDisconnect != null)
                            OnDisconnect(workerSocket);
                        workerSocket.Close();
                    }
                }
                Clients.Clear();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Disables a specific connection
        /// </summary>
        /// <param name="SocketIndex">Index of connection</param>
        public bool CloseConnection(int SocketIndex)
        {
            if ((Clients.Count - 1) >= SocketIndex)
            {
                Socket workerSocket = (Socket)Clients[SocketIndex];
                if (workerSocket != null)
                    workerSocket.Close();
                if (!workerSocket.Connected)
                    return true;
                else
                    return false;
            }
            else
            {
                if (OnError != null)
                    OnError("索引值超出范围!", null, 0);
                return false;
            }
        }

        /// <summary>
        /// Returns true if the specific socket is connected
        /// </summary>
        /// <param name="SocketIndex">Index of connection</param>
        public bool Connected(int SocketIndex)
        {
            if ((Clients.Count - 1) >= SocketIndex)
            {
                Socket soc = (Socket)Clients[SocketIndex];
                return soc.Connected;
            }
            else
                return false;
        }

        /// <summary>
        /// Returns the client's IP connected to the specific socket
        /// </summary>
        /// <param name="SocketIndex">Index of connection</param>
        public string RemoteAddress(int SocketIndex)
        {
            if ((Clients.Count - 1) >= SocketIndex)
            {
                Socket soc = (Socket)Clients[SocketIndex];
                try
                {
                    string temp = soc.RemoteEndPoint.ToString();
                    return temp.Substring(0, temp.IndexOf(":"));
                }
                catch (ArgumentException se)
                {
                    if (OnError != null)
                        OnError(se.Message, null, 0);
                    return "";
                }
                catch (SocketException se)
                {
                    if (OnError != null)
                        OnError(se.Message, null, 0);
                    return "";
                }
            }
            else
                return "";
        }

        /// <summary>
        /// Returns the client's Host connected to the specific socket
        /// </summary>
        /// <param name="SocketIndex">Index of connection</param>
        public string RemoteHost(int SocketIndex)
        {
            if ((Clients.Count - 1) >= SocketIndex)
            {
                Socket soc = (Socket)Clients[SocketIndex];
                try
                {
                    string temp = soc.RemoteEndPoint.ToString();
                    temp = temp.Substring(0, temp.IndexOf(":"));
                    IPHostEntry retorno = Dns.GetHostEntry(temp);
                    return retorno.HostName;
                }
                catch (ArgumentException se)
                {
                    if (OnError != null)
                        OnError(se.Message, null, 0);
                    return "";
                }
                catch (SocketException se)
                {
                    if (OnError != null)
                        OnError(se.Message, null, 0);
                    return "";
                }
            }
            else
                return "";
        }

        /// <summary>
        /// Returns the index of the specific socket
        /// </summary>
        /// <param name="soc">Socket</param>
        public int IndexOf(Socket soc)
        {
            return Clients.IndexOf(soc);
        }
        #endregion
    }

}
