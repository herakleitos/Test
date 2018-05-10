using System;
using System.Collections.Generic;
using System.Text;
using CTSocket;
using System.Threading;
using System.Net;

namespace Chaint.Common.Devices.Net
{
    //需要引用Dll: CTSocket,此DLL由周正提供
    public class ServerSocketService: BaseSocketService
    {
        public ServerSocketService()
        {   }

        public delegate void DataReceivedDelegate(MessageEventArgs e);
        public event DataReceivedDelegate DataReceived;
        internal void OnDataReceived(MessageEventArgs e)
        {

            if (DataReceived != null)
                DataReceived(e);
        }

        public delegate void ClientConnectedDelegate(ConnectionEventArgs e);
        public event ClientConnectedDelegate ClientConnected;
        internal void OnClientConnected(ConnectionEventArgs e)
        {

            if (ClientConnected != null)
                ClientConnected(e);
        }

        public delegate void ClientDisconnectedDelegate(ConnectionEventArgs e);
        public event ClientDisconnectedDelegate ClientDisconnected;
        internal void OnClientDisconnected(ConnectionEventArgs e)
        {

            if (ClientDisconnected != null)
                ClientDisconnected(e);
        }

        #region Methods

        #region OnConnected
        public override void OnConnected(ConnectionEventArgs e)
        {
            base.OnConnected(e);
            OnClientConnected(e);
            if (e.Connection.Host.HostType == HostType.htServer)
            {
                e.Connection.BeginReceive();
            }
        }
        #endregion

        #region OnSend
        public override void OnSent(MessageEventArgs e)
        {
            base.OnSent(e);
            if (e.Connection.Host.HostType == HostType.htServer)
            {
                if (!e.SentByServer) e.Connection.BeginReceive();
            }
            else
            {
                e.Connection.BeginReceive();
            }
        }
        #endregion

        #region OnReceived
        public override void OnReceived(MessageEventArgs e)
        {
            base.OnReceived(e);
            OnDataReceived(e);
            if (e.Connection.Host.HostType == HostType.htServer)
            {
                if (!e.SentByServer) e.Connection.BeginReceive();
            }
            else
            {
                e.Connection.BeginReceive();
            }
        }
        #endregion

        #region OnDisconnected
        public override void OnDisconnected(ConnectionEventArgs e)
        {
            base.OnDisconnected(e);
            OnClientDisconnected(e);
            if (e.Connection.Host.HostType == HostType.htServer)
            { }
            else
                e.Connection.AsClientConnection().BeginReconnect();
        }
        #endregion

        #region OnException
        public override void OnException(ExceptionEventArgs e)
        {
            base.OnException(e);
            if (e.Connection != null) e.Connection.BeginDisconnect();
        }
        #endregion

        #endregion
    }

    public class ClientSocketService : BaseSocketService
    {
        public ClientSocketService()
        { }

        public delegate void DataReceivedDelegate(MessageEventArgs e);
        public event DataReceivedDelegate DataReceived;
        internal void OnDataReceived(MessageEventArgs e)
        {

            if (DataReceived != null)
                DataReceived(e);
        }

        public delegate void ClientConnectedDelegate(ConnectionEventArgs e);
        public event ClientConnectedDelegate ClientConnected;
        internal void OnClientConnected(ConnectionEventArgs e)
        {

            if (ClientConnected != null)
                ClientConnected(e);
        }

        public delegate void ClientDisconnectedDelegate(ConnectionEventArgs e);
        public event ClientDisconnectedDelegate ClientDisconnected;
        internal void OnClientDisconnected(ConnectionEventArgs e)
        {
            if (ClientDisconnected != null)
                ClientDisconnected(e);
        }

        #region Methods
        #region OnConnected
        public override void OnConnected(ConnectionEventArgs e)
        {
            base.OnConnected(e);
            OnClientConnected(e);
            e.Connection.BeginReceive();
        }
        #endregion

        #region OnSend
        public override void OnSent(MessageEventArgs e)
        {
            base.OnSent(e);
            e.Connection.BeginReceive();
        }

        #endregion

        #region OnReceived
        public override void OnReceived(MessageEventArgs e)
        {
            base.OnReceived(e);
            OnDataReceived(e);
            e.Connection.BeginReceive();
        }
        #endregion

        #region OnDisconnected
        public override void OnDisconnected(ConnectionEventArgs e)
        {
            base.OnDisconnected(e);
            OnClientDisconnected(e);
            if (e.Connection.Host.HostType == HostType.htServer)
            {}
            else
                e.Connection.AsClientConnection().BeginReconnect();
        }
        #endregion

        #region OnException
        public override void OnException(ExceptionEventArgs e)
        {
            base.OnException(e);
            if (e.Connection != null) e.Connection.BeginDisconnect();
        }
        #endregion
        #endregion

    }


}
