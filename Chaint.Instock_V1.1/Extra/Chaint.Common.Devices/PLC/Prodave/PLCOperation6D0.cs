using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Text;

using Chaint.Common.Devices.PLC.Utils;

namespace Chaint.Common.Devices.PLC
{
    public class PLCOperation6D0 : PLCProdave6D0, IPLCOperation, IDisposable
    {
        #region 变量定义
        public event PLCMessageEventHandler OnPLCMessageChanged;
        private PLCErrMessage m_PLCErrMsg = null;
        private PLCConnItem m_PLCConnectItem = new PLCConnItem();
        
        private string m_strAccessPoint = "S7ONLINE";
        private AddrType m_ipAddrType = AddrType.IP;
        private bool m_blnConnected = false;
        #endregion

        #region 属性定义
        public bool Connected
        {
            get { return m_blnConnected; }
        }

        public PLCType CurrPLCType
        {
            get { return PLCType.PLC_6D0; }
        }
        #endregion

        #region 构造函数
        public PLCOperation6D0(PLCConnItem plcConnect)
        {             
            m_PLCConnectItem = plcConnect;
            m_PLCErrMsg = new PLCErrMessage(PLCType.PLC_6D0);
        }

        /// <summary>
        /// 实例化PLC操作类
        /// </summary>
        /// <param name="ConnNr">连接号:取值为0-63</param>
        /// <param name="Address">地址,一般为2</param>
        /// <param name="Segment">段号，一般为0</param>
        /// <param name="Rack">机架号,一般为2</param>
        /// <param name="Slot">槽号,一般为0</param>
        public PLCOperation6D0(int ConnNr, string  IPAddress, int Segment, int Rack, int Slot)
        {
            m_PLCConnectItem = new PLCConnItem();
            m_PLCConnectItem.ConnNr = (byte)ConnNr;
            m_PLCConnectItem.Address = IPAddress;
            m_PLCConnectItem.SegmentNO = (byte)Segment;
            m_PLCConnectItem.RackNO = (byte)Rack;
            m_PLCConnectItem.SlotNO = (byte)Slot;
            m_PLCErrMsg = new PLCErrMessage(PLCType.PLC_6D0);
        }
        #endregion

        #region 连接/关闭
        /// <summary>
        /// 连接PLC
        /// </summary>
        public bool ConnectPLC()
        {
            CON_TABLE_TYPE ConTable;
            string strMsg = "";
            int ConTableLen = System.Runtime.InteropServices.Marshal.SizeOf(typeof(PLCOperation6D0.CON_TABLE_TYPE));// 一般为9
            try
            {
                System.Net.IPAddress ipaddr0 = null;
                lock (this)
                {
                    //连接方式选择
                    if (m_ipAddrType == AddrType.IP)
                    {
                        System.Net.IPAddress ipaddr = null;
                        if (System.Net.IPAddress.TryParse(m_PLCConnectItem.Address, out ipaddr))
                        {
                            byte[] bytsIP = ipaddr.GetAddressBytes();
                            ConTable.Adr = new byte[] { bytsIP[0], bytsIP[1], bytsIP[2], bytsIP[3], 0, 0 };
                            ConTable.AdrType = (byte)AddrType.IP;
                            ConTable.SlotNr = m_PLCConnectItem.SlotNO;
                            ConTable.RackNr = m_PLCConnectItem.RackNO;
                            ipaddr0 = ipaddr;
                        }
                        else
                        {
                            strMsg = "连接PLC的IP地址输入错误,无法激活!";
                            m_blnConnected = false;
                            SendPLCMessage(m_blnConnected, strMsg);
                            return false;
                        }
                    }
                    else if (m_ipAddrType == AddrType.MAC)
                    {
                        string[] strIP = m_PLCConnectItem.Address.Split('-');
                        if (strIP.Length < 6)
                        {
                            strMsg = "PLC对应的MAC地址格式错误,请按照(08-00-06-01-AA-BB)格式输入(不包含括号)!";
                            m_blnConnected = false;
                            SendPLCMessage(m_blnConnected, strMsg);
                            return false;
                        }
                        ConTable.Adr = new byte[] { Convert.ToByte(strIP[0]), Convert.ToByte(strIP[1]), Convert.ToByte(strIP[2]), Convert.ToByte(strIP[3]), Convert.ToByte(strIP[4]), Convert.ToByte(strIP[5]) };
                        ConTable.AdrType = (byte)AddrType.MAC;
                        ConTable.SlotNr = m_PLCConnectItem.SlotNO;
                        ConTable.RackNr = m_PLCConnectItem.RackNO;
                    }
                    else
                    {
                        ConTable.Adr = new byte[] { Convert.ToByte(m_PLCConnectItem.Address), 0, 0, 0, 0, 0 };
                        ConTable.AdrType = (byte)AddrType.MPI;
                        ConTable.SlotNr = m_PLCConnectItem.SlotNO;
                        ConTable.RackNr = m_PLCConnectItem.RackNO;
                    }

                    //开始加载并激活
                    int result = LoadConnection_ex6(m_PLCConnectItem.ConnNr, m_strAccessPoint, ConTableLen, ref ConTable);
                    if (result == 0)
                    {
                        result = SetActiveConnection_ex6(m_PLCConnectItem.ConnNr);
                        if (result == 0)
                        {
                            strMsg = "PLC连接成功!";
                            m_blnConnected = true;
                            SendPLCMessage(m_blnConnected, strMsg);
                            return true;
                        }
                        else
                        {
                            m_blnConnected = false;
                            strMsg = string.Format("激活PLC连接失败,原因:{0}", GetPLCErrMsgByErrCode(result));
                            SendPLCMessage(m_blnConnected, strMsg);
                            return false;
                        }
                    }
                    else
                    {
                        m_blnConnected = false;
                        strMsg = string.Format("加载PL连接失败,原因:{0};{1}", GetPLCErrMsgByErrCode(result), ipaddr0);
                        SendPLCMessage(m_blnConnected, strMsg);
                        return false;
                    }
                }
            }
            catch (System.Exception ex)
            {
                m_blnConnected = false;
                strMsg = "PLC初始化出错,原因:" + ex.Message;
                SendPLCMessage(m_blnConnected, strMsg);
                return false;
            }
        }

        /// <summary>
        /// 此处关闭的连接号在关闭时其他程序不能使用
        /// </summary>
        public bool CloseConnectPLC()
        {
            bool retValue = false;
            UInt16 connNr = m_PLCConnectItem.ConnNr;
            string strMsg = "";
            try
            {

                lock(this)
                {
                    int result = UnloadConnection_ex6(connNr);
                    if (result == 0 || result == 28720 || result == 28693)
                    {
                        strMsg = string.Format("PLC已断开连接<{0}>", connNr);
                        retValue = true;
                    }
                    else
                    {
                        strMsg = string.Format("PLC断开连接<{0}>失败! {1}", connNr, GetPLCErrMsgByErrCode(result));
                    }
                }
              
                m_blnConnected = false;
                SendPLCMessage(m_blnConnected, strMsg);
            }
            catch (System.Exception ex)
            {
                strMsg = string.Format("PLC断开连接<{0}>出错,原因:{1}", connNr, ex.Message);
            }
            return retValue;
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
        #endregion

        #region 读取PLC数据
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

        public bool ReadData(int dbno, int dwno, int amount, ref byte[] bytBuffer)
        {
            FieldType FType = FieldType.D;
            UInt32 pDatLen = 0;
            bytBuffer = new byte[amount];
            UInt16 unitByts = (UInt16)200;  //定义每次读取的字节数
            string strMsg = "";
            byte[] tempbyts;
            UInt32 bytBufferLen =(UInt32)amount;
            int result = -1;

            try
            {

                lock(this)
                {
                    result = SetActiveConnection_ex6(m_PLCConnectItem.ConnNr);
                    if (result != 0)
                    {
                        strMsg = string.Format("激活PLC连接失败. {0}", GetPLCErrMsgByErrCode(result));
                        return false;
                    }
                    int intCount = (amount / unitByts) + (amount % unitByts > 1 ? 1 : 0);
                    if (intCount <= 1)
                    {
                        result = field_read_ex6(FType, (UInt16)dbno, (UInt16)dwno, (UInt32)amount, bytBufferLen, bytBuffer, ref pDatLen);
                    }
                    else
                    {
                        for (UInt16 i = 0; i < intCount; i++)
                        {
                            if (unitByts * (i + 1) < amount)
                            {
                                tempbyts = new byte[unitByts];
                                result = field_read_ex6(FType, (UInt16)dbno, Convert.ToUInt16(dwno + unitByts * i), unitByts, unitByts, tempbyts, ref pDatLen);
                            }
                            else
                            {
                                tempbyts = new byte[(amount - unitByts * i)];
                                result = field_read_ex6(FType, (UInt16)dbno, Convert.ToUInt16(dwno + unitByts * i), Convert.ToUInt16(amount - unitByts * i), Convert.ToUInt16(amount - unitByts * i), tempbyts, ref pDatLen);
                            }
                            tempbyts.CopyTo(bytBuffer, dwno + unitByts * i);
                        }
                        tempbyts = null;
                    }

                    if (result == 0)
                    {
                        strMsg = "PLC数据读取成功!";
                        m_blnConnected = true;
                    }
                    else
                    {
                        strMsg = GetPLCErrMsgByErrCode(result);
                        m_blnConnected = false;
                    }

                }
            }
            catch (System.Exception ex)
            {
                strMsg = "读取PLC出错,原因:" + ex.Message;
                m_blnConnected = false;
            }
            SendPLCMessage(m_blnConnected, strMsg);
            return m_blnConnected;
        }
        #endregion

        #region 写入PLC数据

        /// <summary>
        /// 翻转某个地址的值
        /// </summary>
        /// <param name="dbno">DB单元</param>
        /// <param name="strAddr">地址 格式X.Y   X为地址,Y为位置 如：5.3表示第5个字节第3位</param>
        /// <returns></returns>
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

        /// <summary>
        /// 将当前地址写1或0,此处以字节来处理
        /// </summary>
        /// <param name="dbno">DB单元</param>
        /// <param name="strAddr">地址 格式X.Y   X为地址,Y为位置 如：5.3表示第5个字节第3位</param>
        /// <param name="blnValue">值,非0为真,其他为假</param>
        /// <returns></returns>
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
        /// 写入PLC数据
        /// </summary>
        /// <param name="dbno">DB单元</param>
        /// <param name="dwno">初始地址</param>
        /// <param name="amount">字节数量</param>
        /// <param name="buffer">返回的字节数组</param>
        /// <returns>写入成功返回True</returns>
        public bool WriteData(int dbno, int dwno, int amount, byte[] buffer)
        {
            FieldType FType = FieldType.D;
            string strMsg = "";
            int result = -1;
            try
            {

                lock (this)
                {
                    result = SetActiveConnection_ex6(m_PLCConnectItem.ConnNr);
                    if (result != 0)
                    {
                        m_blnConnected = false;
                        strMsg = string.Format("激活PLC连接失败. {0}", GetPLCErrMsgByErrCode(result));
                    }
                    else
                    {
                        result = field_write_ex6(FType, (UInt16)dbno, (UInt16)dwno, (UInt32)amount, (UInt32)(buffer.Length), buffer);

                        if (result == 0)
                        {
                            strMsg = "PLC数据写入成功!";
                            m_blnConnected = true;
                        }
                        else
                        {
                            strMsg = GetPLCErrMsgByErrCode(result);
                            m_blnConnected = false;
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                strMsg = "写入PLC数据出错,原因:" + ex.Message;
                m_blnConnected = false;
            }
            SendPLCMessage(m_blnConnected, strMsg);
            return m_blnConnected;
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

        /// <summary>
        /// 输入对应的十进制代码
        /// </summary>
        /// <param name="errCode"></param>
        /// <returns></returns>
        private string GetPLCErrMsgByErrCode(int errCode)
        {
            string retValue = "";
            try
            {
                retValue = m_PLCErrMsg.GetErrorMsgByCode(errCode);
                retValue = string.Format("ErrCode:【{0}】,ErrMessage:【{1}】", Convert.ToString(errCode, 16).PadLeft(4, '0').ToUpper(), retValue);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
                retValue = "";
            }
            return retValue;
        }

    }
}
