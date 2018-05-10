using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CTSocket;
using DataModel;

namespace CTWH.Common.PLCSokcet
{
    public   class PLCReadClient
    {

        System.DateTime LasActionDateTime = Utils.DateTimeNow;


        //一个socket客户端传入的接口类（用户可以根据自己的需求继续定义一些业务逻辑规则）
        public  Common.SocketService.ClientSocketService PLCClientService = new Common.SocketService.ClientSocketService();
        //一个socket客户端的定义
        CTSocket.SocketClient client_plc;

        //定义客户端进行通信的连接通道 
        //PLCCommunication
        private SocketConnector connector_PLCCommunication;
        //private SocketConnector connector_PLCReadBuffer;        
        //DB400
        //private SocketConnector connector_PLCReadBufferDB400;

        bool IsDispose = false;
        System.Threading.Thread thKeepAlive = null;

        public PLCReadClient()
        {
           // InitPLCClient();
        }

        #region  PLC Server相关操作

        public bool InitPLCClient()
        {
            if (client_plc != null)
            {
                return false;
            }

            bool ret = false;
            //启动客户端
            //StartClient
            //初始化一个客户端
            client_plc = new SocketClient(CallbackThreadType.ctWorkerThread, PLCClientService);
            //消息的分隔符（末尾符），这里表示为一个数据消息包中的结束符号
            //client_jet.Delimiter = Utils.ImajeJetEncoding.GetBytes(new char[] { (char)3 });           

            //消息接受后末尾符号不包含在显示的消息中（也可以设置包含在其中）
            client_plc.DelimiterType = DelimiterType.dtNone;
            //client_plc.Delimiter = Utils.SocketParaPLCReadServer.SocketEncoding.GetBytes(new char[] { '>' });
            //负责检查客户端中全部消息通道各种异常的时间间隔
            client_plc.IdleCheckInterval = 5000;
            //超时时间，此处的意思描述为超时时间为60s，即客户端有异常超过60s后自动进行回收以及其他操作，实际中操作时间为（60-70）s
            //client_jet.IdleTimeOutValue = 600000;
            client_plc.IdleTimeOutValue = 0;
            //一个数据包的最大字节
            client_plc.MessageBufferSize = 1024 * 16;
            //socket缓冲池的字节数
            client_plc.SocketBufferSize = 1024 * 2;
            if (!client_plc.Active)
            {
                connector_PLCCommunication = client_plc.AddConnector("PLCCommunication", new System.Net.IPEndPoint(System.Net.IPAddress.Parse(Utils.SocketParaPLCCommunication.IPAddress), Utils.SocketParaPLCCommunication.SocketPort));
                //断开后重新连接的时间间隔
                connector_PLCCommunication.ReconnectAttemptInterval = 3000;
                //允许重复连接的最大次数
                connector_PLCCommunication.ReconnectAttempts = int.MaxValue;

                //connector_PLCReadBuffer = client_plc.AddConnector("PLCRead from Server", new System.Net.IPEndPoint(System.Net.IPAddress.Parse(Utils.SocketParaPLCReadServer.IPAddress), Utils.SocketParaPLCReadServer.SocketPort));
                ////断开后重新连接的时间间隔
                //connector_PLCReadBuffer.ReconnectAttemptInterval = 3000;
                ////允许重复连接的最大次数
                //connector_PLCReadBuffer.ReconnectAttempts = int.MaxValue;
                ////DB400  
                // connector_PLCReadBufferDB400 = client_plc.AddConnector("PLCRead from Server DB400", new System.Net.IPEndPoint(System.Net.IPAddress.Parse(Utils.SocketParaPLCReadServer.IPAddress), 20024));
                ////断开后重新连接的时间间隔
                //connector_PLCReadBufferDB400.ReconnectAttemptInterval = 3000;
                ////允许重复连接的最大次数
                //connector_PLCReadBufferDB400.ReconnectAttempts = int.MaxValue;

                //侦听需要连接的PLCReadServer的端口
                foreach (int port in Utils.SocketParaPLCReadServer.SocketPorts)
                {
                    SocketConnector connectorReadbuffer;// = new SocketConnector();
                    connectorReadbuffer = client_plc.AddConnector("PLCRead port" + port, new System.Net.IPEndPoint(System.Net.IPAddress.Parse(Utils.SocketParaPLCReadServer.IPAddress), port));
                    //断开后重新连接的时间间隔
                    connectorReadbuffer.ReconnectAttemptInterval = 3000;
                    //允许重复连接的最大次数
                    connectorReadbuffer.ReconnectAttempts = int.MaxValue;
                }
                client_plc.Start();
                ret = true;
            }

            thKeepAlive = new System.Threading.Thread(new System.Threading.ThreadStart(thKeepAlive_DoWork));
            thKeepAlive.Start();

            return ret;
        }

        void thKeepAlive_DoWork()
        {
            while (!IsDispose)
            {
                if (LasActionDateTime.AddSeconds(10).CompareTo(Utils.DateTimeNow) < 0)
                {
                    SendToPLCServer(Utils.SocketParaPLCReadServer.IPAddress, Utils.SocketParaPLCReadServer.SocketPort, Utils.SocketParaPLCReadServer.SocketEncoding.GetBytes("K" + Utils.SocketDelimiterPLCCommunication));
                    SendToPLCServer(Utils.SocketParaPLCCommunication.IPAddress, Utils.SocketParaPLCCommunication.SocketPort, Utils.SocketParaPLCCommunication.SocketEncoding.GetBytes("K" + Utils.SocketDelimiterPLCCommunication));
                }

               System.Threading.Thread.Sleep(10000);
            }
        }

        public void DisposePLCClient()
        {
            IsDispose = true;
            //停止Keepalive

            if (client_plc != null)
            {
                client_plc.Stop();
                client_plc.Dispose();
            }

            if (this.thKeepAlive != null && this.thKeepAlive.IsAlive)
            {
                this.thKeepAlive.Join(500);
                this.thKeepAlive.Abort();
                this.thKeepAlive = null;
            }
        }

        public bool PLC_WriteInt(ushort ConNr, ushort dbno, ushort dwno, int value)
        {
            return PLC_WriteData(ConNr, dbno, dwno, 2, Common.PLC.BufferOperate.IntToByte(value));
        }

        public bool PLC_WriteDInt(ushort ConNr, ushort dbno, ushort dwno, int value)
        {
            return PLC_WriteData(ConNr, dbno, dwno, 4, Common.PLC.BufferOperate.DIntToByte(value));
        }

        //public bool PLC_WriteData(ushort ConNr, ushort dbno, ushort dwno, ushort amount, int value)
        //{
        //     string tempstr = String.Join("|", "", 2, ConNr, dbno, dwno, amount, value, Utils.SocketDelimiterPLCCommunication);
             
                
        //       if( SendToPLCServer(Utils.SocketParaPLCCommunication.IPAddress,
        //        Utils.SocketParaPLCCommunication.SocketPort,
        //        Utils.SocketParaPLCCommunication.SocketEncoding.GetBytes(tempstr) ))
        //       {
        //           LasActionDateTime = Utils.DateTimeNow;
        //           return true;
        //       }
        //       else
        //       {
        //        Utils.WriteTxtLog(Utils.FilePath_txtPLCLog, "PLCReadService异常,PLC_WriteData写入错误！" + "PLC_WriteData(" + ConNr + "," + dbno + "," + dwno + ", 2, " + value + ")");
        //        return false;
        //       }
        //}

        public bool PLC_WriteData(ushort ConNr, ushort dbno, ushort dwno, ushort amount, byte[] value)
        {
            string tempstr = String.Join("|", "", 2, ConNr, dbno, dwno, amount, 
                //value,
                String.Join(",",  value),
                Utils.SocketDelimiterPLCCommunication);


            if (SendToPLCServer(Utils.SocketParaPLCCommunication.IPAddress,
             Utils.SocketParaPLCCommunication.SocketPort,
             Utils.SocketParaPLCCommunication.SocketEncoding.GetBytes(tempstr)))
            {
                LasActionDateTime = Utils.DateTimeNow;
                return true;
            }
            else
            {
                Utils.WriteTxtLog(Utils.FilePath_txtPLCLog, "PLCReadService异常,PLC_WriteData写入错误！" + "PLC_WriteData(" + ConNr + "," + dbno + "," + dwno + ", 2, " + value + ")");
                return false;
            }
        }


        public bool PLC_WriteData(PLCServerDS.PLCReWriteDataTable WriteTB)
        {
            string tempstr = "";// "|3|";
            foreach(PLCServerDS.PLCReWriteRow row in WriteTB.Rows)
            {

                if (tempstr == "")
                {
                    tempstr = String.Join(";", row.PLCNO, row.DBNO, row.Begin, row.Amount,
                     String.Join(",", row.Value),
                     Utils.SocketDelimiterPLCCommunication);
                }
                else
                {
                    tempstr = tempstr + "/" +String.Join(";", row.PLCNO, row.DBNO, row.Begin, row.Amount,
                        String.Join(",", row.Value),
                        Utils.SocketDelimiterPLCCommunication);
                }

            }

            //  |3|1;100;0;2;0,86;1;100;6;4;0,0,1,67
            tempstr = "|3|" + tempstr;

            if (SendToPLCServer(Utils.SocketParaPLCCommunication.IPAddress,
             Utils.SocketParaPLCCommunication.SocketPort,
             Utils.SocketParaPLCCommunication.SocketEncoding.GetBytes(tempstr)))
            {
                LasActionDateTime = Utils.DateTimeNow;
                return true;
            }
            else
            {
                Utils.WriteTxtLog(Utils.FilePath_txtPLCLog, "PLCReadService异常,PLC_WriteData写入错误！" + "WriteTB:" + tempstr);
                return false;
            }
        }


        public bool SendToPLCServer( string IPAddress,int SocketPort ,byte[] buffer)
        {
             //InitPLCClient();
            bool ret = false;
            
            //从当前客户端对象中找到全部的建立了通信的服务端连接
            foreach (ISocketConnectionInfo isocketinfo in client_plc.GetConnections())
            {
                //发送消息到指定的ip地址的服务端 全部发送
                if (isocketinfo.RemoteEndPoint.Address.ToString() == IPAddress && isocketinfo.RemoteEndPoint.Port == SocketPort)
                {
                    (isocketinfo as BaseSocketConnection).BeginSend(buffer);
                    ret = true;
                }
            }
            return ret;
        }

        #endregion


    }
}
