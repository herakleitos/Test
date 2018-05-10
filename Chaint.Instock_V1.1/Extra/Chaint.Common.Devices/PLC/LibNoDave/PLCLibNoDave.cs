using System;
using System.Collections.Generic;
using System.Text;

using Chaint.Common.Devices.PLC.Utils;

namespace Chaint.Common.Devices.PLC
{
    public class PLCLibNoDave : IPLCOperation, IDisposable
    {
        public event PLCMessageEventHandler OnPLCMessageChanged;
        private PLCErrMessage m_PLCErrMsg = null;
        private PLCConnItem m_PLCConnectItem = new PLCConnItem();
        private bool m_blnConnected = false;


        private DaveArea m_AreaDataType = DaveArea.daveDB;
        private libnodave.daveConnection m_NoDaveConnection;
        private libnodave.daveInterface m_NoDaveInterface;
        private DaveDeviceType m_DeviceType = DaveDeviceType.OpenSocket;        //默认均使用OpenSocket 类型(常用类型)

        private int m_LocalMPI = 2;
        private int m_DaveRFD = 0;      //此处相应于PLC设备资源句柄 



        #region 属性定义
        public bool Connected
        {
            get { return m_blnConnected; }
        }

        public PLCType CurrPLCType
        {
            get { return PLCType.PLC_LibNoDave; }
        }
        #endregion


         #region 构造函数
        public PLCLibNoDave(PLCConnItem plcConnect)
        {             
            m_PLCConnectItem = plcConnect;
            m_PLCErrMsg = new PLCErrMessage(PLCType.PLC_LibNoDave);
        }

        /// <summary>
        /// 实例化PLC操作类  仅针对Socket方式   Port:102,MPI方式不支持
        /// </summary>
        /// <param name="Address">地址,一般为2</param>
        /// <param name="Rack">机架号,一般为2</param>
        /// <param name="Slot">槽号,一般为0</param>
        public PLCLibNoDave(string IPAddress, int Rack, int Slot)
        {
            m_PLCConnectItem = new PLCConnItem();
            m_PLCConnectItem.ConnNr = 0;
            m_PLCConnectItem.Address = IPAddress;
            m_PLCConnectItem.SegmentNO = 0;
            m_PLCConnectItem.RackNO = (byte)Rack;
            m_PLCConnectItem.SlotNO = (byte)Slot;
            m_PLCErrMsg = new PLCErrMessage(PLCType.PLC_LibNoDave);
        }
        #endregion


        #region 连接/关闭/重连
        public bool ConnectPLC()
        {
            m_blnConnected = false;

            lock(this)
            {
                //创建接口
                m_NoDaveInterface = CreateDaveInterface();
                if (m_NoDaveInterface == null)
                {
                    SendPLCMessage(m_blnConnected, "创建接口失败");
                    return false;
                }

                //初始化适配器
                int res = m_NoDaveInterface.initAdapter();
                if (res == 0) ///OK
                {
                    //开始连接
                    m_NoDaveConnection = new libnodave.daveConnection(m_NoDaveInterface, m_LocalMPI, m_PLCConnectItem.RackNO, m_PLCConnectItem.SlotNO);
                    res = m_NoDaveConnection.connectPLC();
                    if (res == 0) ///OK
                    {
                        m_blnConnected = true;
                        SendPLCMessage(m_blnConnected, "PLC连接成功");
                    }
                    else///连接  connectPLC()  失败
                    {
                        SendPLCMessage(m_blnConnected, string.Format("PLC连接失败,返回代码:<{0}> ", res));
                    }
                }
                else
                {
                    ///初始化 initAdapter() 失败
                    SendPLCMessage(m_blnConnected, string.Format("函数<initAdapter>初始化失败,返回代码:<{0}> ", res));
                }
            }
            return m_blnConnected;
        }

        public bool CloseConnectPLC()
        {
            int res = -1;
            m_blnConnected = false;

            lock(this)
            {
                if (m_NoDaveConnection != null)
                    res = m_NoDaveConnection.disconnectPLC();
                if (res != 0)
                {
                    SendPLCMessage(m_blnConnected, string.Format("PLC断开连接失败,返回代码:<{0}> ", res));
                }
                else
                {
                    SendPLCMessage(m_blnConnected, "PLC断开连接成功");
                }

                if (m_NoDaveInterface != null) m_NoDaveInterface.disconnectAdapter();

                switch (m_DeviceType)
                {
                    case DaveDeviceType.OpenSocket:
                        libnodave.closeSocket(m_DaveRFD);
                        break;
                    case DaveDeviceType.OpenS7online:
                        libnodave.closeS7online(m_DaveRFD);
                        break;

                    case DaveDeviceType.OpenSerialPort:
                        libnodave.closePort(m_DaveRFD);
                        break;
                }
                m_NoDaveInterface = null;
                m_NoDaveConnection = null;

            }
            
            return true;
        }

        /// <summary>
        /// 重连PLC
        /// </summary>
        /// <returns></returns>
        public bool ReConnectPLC()
        {
            try
            {
                CloseConnectPLC();
                System.Threading.Thread.Sleep(300);
                ConnectPLC();
                return true;
            }
            catch (System.Exception ex)
            {
                SendPLCMessage(false, "重连PLC出错,原因:" + ex.Message);
                return false;
            }
        }


        private libnodave.daveOSserialType CreateDaveOSSeialType()
        {
            
            switch (m_DeviceType)
            {
                case DaveDeviceType.OpenSocket:
                    m_DaveRFD = libnodave.openSocket(102, m_PLCConnectItem.Address);
                    break;
                case DaveDeviceType.OpenS7online:
                    m_DaveRFD = libnodave.openS7online("S7ONLINE");
                    break;
                case DaveDeviceType.OpenSerialPort:
                    m_DaveRFD = libnodave.setPort("COM1", "38400", 0);///串口端口类型、波特率,校验位  此处不用，如用可以将此处参数化
                    break;
            }

            libnodave.daveOSserialType noDaveDataType = new libnodave.daveOSserialType()
            {
                rfd = m_DaveRFD,
                wfd = m_DaveRFD
            };

            return noDaveDataType;
        }

        private libnodave.daveInterface CreateDaveInterface()
        {
            //int LocalMPI = 0;
            libnodave.daveOSserialType daveOSSeialType = CreateDaveOSSeialType();

            if (m_DaveRFD == 0) return null;            //当Socket打开失败时,返回接口为空


            DaveProtocol daveProtocol = DaveProtocol.daveProtoISOTCP;

            switch (m_DeviceType)
            {
                case DaveDeviceType.OpenSocket:
                    ///LocalMPI = 0;
                    daveProtocol = DaveProtocol.daveProtoISOTCP;
                    break;
                case DaveDeviceType.OpenS7online:
                    daveProtocol = DaveProtocol.daveProtoS7online;
                    ///LocalMPI = 6;
                    break;
                case DaveDeviceType.OpenSerialPort:
                    daveProtocol = DaveProtocol.daveProtoMPI;
                    ///LocalMPI = 0;
                    break;
            }

            ///此处MPI修改为2，如有需要可以采用参数引用 
            libnodave.daveInterface daveIF = new libnodave.daveInterface(daveOSSeialType, "daveInterface1", m_LocalMPI, daveProtocol, DaveSpeed.daveSpeed187k);

            daveIF.setTimeout(1000000);

            return daveIF;
        }

        #endregion


        #region 读取数据
        public bool ReadData(int dbno, string strAddr, ref string retBoolValue)
        {
            retBoolValue = "0";

            if (strAddr.Split('.').Length < 2) return false;
            int intX = Convert.ToInt32(strAddr.Split('.')[0].Trim());
            int intY = Convert.ToInt32(strAddr.Split('.')[1].Trim());

            ushort dwno = (ushort)intX;
            byte[] bytBuffer = new byte[1];

            bool blnSucc = ReadData(dbno, dwno, 1, ref bytBuffer);
            if (!blnSucc) return false;

            bool isOne = DataConverter.IsOneInByte(bytBuffer[0], intY);
            retBoolValue = isOne ? "1" : "0";

            return true;
        }

        public bool ReadData(int dbno, int dwno, int amount, ref string retValue)
        {
            bool blnSuccess = true;
            if (amount > 4) amount = 4;
            byte[] bytBuffer = new byte[amount];
            retValue = "0";
            bool blnResult = ReadData(dbno, dwno, amount, ref bytBuffer);
            if (blnResult)
            {
                retValue = DataConverter.ByteToLong(bytBuffer).ToString();
                blnSuccess = true;
            }
            else
            {
                blnSuccess = false;
            }
            return blnSuccess;
        }

        /// <summary>
        /// 使用的是ReadManyBytes 函数
        /// </summary>
        /// <param name="dbno"></param>
        /// <param name="dwno"></param>
        /// <param name="amount"></param>
        /// <param name="bytBuffer"></param>
        /// <returns></returns>
        public bool ReadData(int dbno, int dwno, int amount, ref byte[] bytBuffer)
        {
            if (m_NoDaveConnection == null || m_blnConnected == false) return false;

            string strMsg = "";
            bool retValue = false;

            bytBuffer = new byte[amount];
            m_blnConnected = true;
            try
            {
                 int res = -1;
                 lock (this)
                 {
                     res = m_NoDaveConnection.readManyBytes(m_AreaDataType, dbno, dwno, amount, bytBuffer);

                   //  res = m_NoDaveConnection.readBytes(m_AreaDataType, dbno, dwno, amount, bytBuffer);
                 }
                 if (res == 0)
                 {
                     strMsg = string.Format("{0}", "PLC数据读取成功!");
                     retValue = true;
                 }
                 else
                 {
                     strMsg = string.Format("{0},代码<{1}>", "PLC数据读取失败!", res);
                 }
            }
            catch (Exception ex)
            {
                m_blnConnected = false;

                strMsg = string.Format("{0}", "PLC数据读取出错,原因:"+ex.Message);
            }

            SendPLCMessage(m_blnConnected, strMsg);

            return retValue;
        }

        #endregion

        #region 写入数据
        public bool WriteData(int dbno, string strAddr)
        {
            if (strAddr.Split('.').Length < 2) return false;
            int intX = Convert.ToInt32(strAddr.Split('.')[0].Trim());
            int intY = Convert.ToInt32(strAddr.Split('.')[1].Trim());

            ushort dwno = (ushort)intX;
            byte[] bytBuffer = new byte[1];

            bool blnSucc = ReadData(dbno, dwno, 1, ref bytBuffer);
            bytBuffer[0] = (byte)DataConverter.ConvertBit(bytBuffer[0], intY);
            return WriteData(dbno, dwno, 1, bytBuffer);
        }

        public bool WriteData(int dbno, string strAddr, bool blnValue)
        {
            if (strAddr.Split('.').Length < 2) return false;
            int intX = Convert.ToInt32(strAddr.Split('.')[0].Trim());
            int intY = Convert.ToInt32(strAddr.Split('.')[1].Trim());

            ushort dwno = (ushort)intX;
            byte[] bytBuffer = new byte[1];

            bool blnSucc = ReadData(dbno, dwno, 1, ref bytBuffer);
            bytBuffer[0] = (byte)DataConverter.SetBitBoolValue(bytBuffer[0], intY, blnValue);
            return WriteData(dbno, dwno, 1, bytBuffer);
        }

        public bool WriteData(int dbno, int dwno, int amount, string strData)
        {
            ulong tmpValue = 0;
            string strMsg = "";
            byte[] bytBuffer = new byte[amount];

            if (ulong.TryParse(strData, out tmpValue))
            {
                bytBuffer = DataConverter.LongToByte(tmpValue, amount);
                bool blnSucc = WriteData(dbno, dwno, amount, bytBuffer);
                return blnSucc;
            }
            else
            {
                strMsg = string.Format("写入值<{0}>不是有效的无符号长整型", strData);
                SendPLCMessage(true, strMsg);
                return false;
            }
        }

        public bool WriteData(int dbno, int dwno, int amount, ulong data)
        {
            byte[] bytBuffer = new byte[amount];
            bytBuffer = DataConverter.LongToByte(data, amount);
            bool blnSucc = WriteData(dbno, dwno, amount, bytBuffer);
            return blnSucc;
        }

        /// <summary>
        /// 使用的是writeManyBytes函数
        /// </summary>
        /// <param name="dbno"></param>
        /// <param name="dwno"></param>
        /// <param name="amount"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public bool WriteData(int dbno, int dwno, int amount, byte[] buffer)
        {
            if (m_NoDaveConnection == null || m_blnConnected == false) return false;

            string strMsg = "";
            bool retValue = false;

            m_blnConnected = true;
            try
            {
                int res = -1;
                lock (this)
                {
                    res = m_NoDaveConnection.writeManyBytes(m_AreaDataType, dbno, dwno, amount, buffer);

                    //res = m_NoDaveConnection.writeBytes(m_AreaDataType, dbno, dwno, amount, buffer);
                }
                if (res == 0)
                {
                    strMsg = string.Format("{0}", "PLC数据写入成功!");
                    retValue = true;
                }
                else
                {
                    strMsg = string.Format("{0},代码<{1}>", "PLC数据写入失败!", res);
                }
            }
            catch (Exception ex)
            {
                m_blnConnected = false;

                strMsg = string.Format("{0}", "PLC数据写入出错,原因:" + ex.Message);
            }

            SendPLCMessage(m_blnConnected, strMsg);

            return retValue;
        }

        #endregion

        #region 释放资源
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Part of basic design pattern for implementing Dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                CloseConnectPLC();
            }
        }
        #endregion


        private void SendPLCMessage(bool blnConnected, string strMsg)
        {
            if (OnPLCMessageChanged != null)
            {
                OnPLCMessageChanged(this, new PLCEventArgument(blnConnected, strMsg));
            }
        }

    }
}
