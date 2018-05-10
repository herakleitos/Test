using System;
using System.Collections.Generic;
using System.Text;

using Chaint.Common.Devices.PLC.Utils;

/*-----------------------------------------------------------------------------------
 * 作者: Automation&IT Dept. 
 * 
 * 创建时间: 2014-02-17
 * 
 * 功能描述: 
 *      PLC Prodave6.0 操作类
 * 
 ------------------------------------------------------------------------------------*/
namespace Chaint.Common.Devices.PLC
{
    public class PLCOperation5D0: PLCProdave5D0, IPLCOperation, IDisposable
    {


        #region 变量定义
        public event PLCMessageEventHandler OnPLCMessageChanged;
        private PLCErrMessage m_PLCErrMsg = null;
        private PLCConnItem m_PLCConnectItem = new PLCConnItem();
        private bool m_blnConnected = false;
        #endregion

        #region 属性定义
        public bool Connected
        {
            get { return m_blnConnected; }
        }

        public PLCType CurrPLCType
        {
            get { return PLCType.PLC_5D0; }
        }
        #endregion

        #region 初始化PLC
        /// 建立连接，同一个连接只容许调用一次
        /// 连接号1-4
        /// 指定链接参数
        /// 返回10进制错误号，0表示没有错误
        private int Inital_PLC(byte cnnNo, PLCAddrType[] cnnInfo)
        {
            int err;
            //传递参数不正确
            if (cnnInfo.Length <= 0)
            {
                return -1;
            }
            byte[,] btr = new byte[cnnInfo.Length + 1, 4];
            //转换链接表
            for (int i = 0; i < cnnInfo.Length; i++)
            {
                btr[i, 0] = cnnInfo[i].Addres;
                btr[i, 1] = cnnInfo[i].Segment;
                btr[i, 2] = cnnInfo[i].Rack;
                btr[i, 3] = cnnInfo[i].Slot;
            }
            btr[cnnInfo.Length, 0] = 0;
            btr[cnnInfo.Length, 1] = 0;
            btr[cnnInfo.Length, 2] = 0;
            btr[cnnInfo.Length, 3] = 0;
            //调用初始化函数
            err = load_tool(cnnNo, "s7online", btr);
            return err;
        }
        #endregion

        #region 构造函数
        /// <summary>
        /// PLC构造
        /// </summary>
        /// <param name="plcConnect">PLC连接参数</param>
        public PLCOperation5D0(PLCConnItem plcConnect)
        {
            m_PLCConnectItem = plcConnect;
            m_PLCErrMsg = new PLCErrMessage(PLCType.PLC_5D6);
        }

        /// <summary>
        /// 实例化PLC操作类
        /// </summary>
        /// <param name="ConnNr">连接号:取值为1-4</param>
        /// <param name="Address">地址,一般为2</param>
        /// <param name="Segment">段号，一般为0</param>
        /// <param name="Rack">机架号,一般为2</param>
        /// <param name="Slot">槽号,一般为0</param>
        public PLCOperation5D0(int ConnNr, int Address, int Segment, int Rack, int Slot)
        {
            m_PLCConnectItem = new PLCConnItem();
            m_PLCConnectItem.ConnNr = (byte)ConnNr;
            m_PLCConnectItem.Address = Address.ToString();
            m_PLCConnectItem.SegmentNO = (byte)Segment;
            m_PLCConnectItem.RackNO = (byte)Rack;
            m_PLCConnectItem.SlotNO = (byte)Slot;
            m_PLCErrMsg = new PLCErrMessage(PLCType.PLC_5D6);
        }

        #endregion

        #region 连接与关闭
        public bool ConnectPLC()
        {
            PLCAddrType[] Cnn = new PLCAddrType[1];
            Cnn[0].Addres = Convert.ToByte(m_PLCConnectItem.Address);
            Cnn[0].Segment = (byte)(m_PLCConnectItem.SegmentNO);
            Cnn[0].Rack = (byte)(m_PLCConnectItem.RackNO);
            Cnn[0].Slot = (byte)(m_PLCConnectItem.SlotNO);

            lock(this)
            {
                // int ret = Inital_PLC((byte)m_PLCConnectItem.ConnNr, Cnn);
                int ret = Inital_PLC(1, Cnn);
                if (ret == 0)
                {
                    new_ss(0);
                    m_blnConnected = true;
                    return true;
                }
                else
                {
                    SendPLCMessage(false, "连接失败：" + GetPLCErrMsgByErrCode(ret));
                    return false;
                }
            }
         
        }

        /// <summary>
        /// <remark>重新连接PLC</remark>   
        /// </summary>
        public bool ReConnectPLC()
        {
            CloseConnectPLC();
            System.Threading.Thread.Sleep(200);
            m_blnConnected = ConnectPLC();
            return m_blnConnected;
        }

        public bool CloseConnectPLC()
        {
            try
            {

                lock(this)
                {
                    if (unload_tool() == 0)
                    {
                        m_blnConnected = false;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
             
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        #endregion

        #region 读取PLC数据

        /// <summary>
        /// 主要读取某个布尔值,此处以字节来处理
        /// </summary>
        /// <param name="dbno">DB单元</param>
        /// <param name="strAddr">地址 格式X.Y   X为地址,Y为位置 如：5.3表示第5个字节第3位</param>
        /// <param name="retBoolValue">返回的布尔值，1或者0</param>
        /// <returns></returns>
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

        /// <summary>
        /// 读取PLC数据,返回字符串数据
        /// </summary>
        /// <param name="dbno"></param>
        /// <param name="dwno"></param>
        /// <param name="amount"></param>
        /// <param name="retValue"></param>
        /// <returns></returns>
        public bool ReadData(int dbno, int dwno, int amount, ref string retValue)
        {
            byte[] buffer = new byte[amount];
            retValue = "0";
         
            bool bln = ReadData(dbno, dwno, amount, ref buffer);
            if (bln)
            {
                retValue = DataConverter.ByteToLong(buffer).ToString();
            }
            return bln;
        }

        /// <summary>
        /// 读取PLC数据,返回数组
        /// </summary>
        /// <param name="dbno">db单元</param>
        /// <param name="dwno">地址</param>
        /// <param name="amount">字节数</param>
        /// <param name="buffer">返回读取数据字节</param>
        /// <returns></returns>
        public bool ReadData(int dbno, int dwno, int amount, ref byte[] buffer)
        {
            string strMsg = "successful";
            try
            {
                int ret = -1;
                lock (this)
                {
                    ret = d_field_read(dbno, dwno, amount, buffer);
                }
                if (ret == 0)
                    m_blnConnected = true;
                else
                {
                    m_blnConnected = false;
                    strMsg = "读取失败:" + GetPLCErrMsgByErrCode(ret);
                }
            }
            catch(Exception ex)
            {
                m_blnConnected = false;
                strMsg = "读取出错,原因:"+ex.Message;
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

        /// <summary>
        /// 写入PLC数据
        /// </summary>
        /// <param name="dbno"></param>
        /// <param name="dwno"></param>
        /// <param name="amount"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool WriteData(int dbno, int dwno, int amount, ulong data)
        {
            byte[] buffer = DataConverter.LongToByte(data, amount);
            return WriteData(dbno, dwno, amount, buffer);
        }

        /// <summary>
        /// 将buffer数组中的字节数据写入指定的PLC地址中
        /// </summary>
        /// <param name="dbno">db块号(100)</param>
        /// <param name="dwno">相对于db块的偏移地址号(2)</param>
        /// <param name="amount">写入字节个数</param>
        /// <param name="buffer">待写入的字节数据</param>
        /// <returns>成功返回真</returns>
        public bool WriteData(int dbno, int dwno, int amount, byte[] buffer)
        {
            try
            {
                int ret = -1;
                lock (this)
                {
                    ret = d_field_write(dbno, dwno, amount, buffer);
                }
                if (ret == 0)
                    return true;
                else
                {
                    SendPLCMessage(false, "写入失败：" + GetPLCErrMsgByErrCode(ret));
                    return false;
                }
            }
            catch(Exception ex)
            {
                SendPLCMessage(false, "写入出错,原因:" + ex.Message);
                return false;
            }
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
