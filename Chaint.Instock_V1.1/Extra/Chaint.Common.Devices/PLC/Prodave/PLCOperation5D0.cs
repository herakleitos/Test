using System;
using System.Collections.Generic;
using System.Text;

using Chaint.Common.Devices.PLC.Utils;

/*-----------------------------------------------------------------------------------
 * ����: Automation&IT Dept. 
 * 
 * ����ʱ��: 2014-02-17
 * 
 * ��������: 
 *      PLC Prodave6.0 ������
 * 
 ------------------------------------------------------------------------------------*/
namespace Chaint.Common.Devices.PLC
{
    public class PLCOperation5D0: PLCProdave5D0, IPLCOperation, IDisposable
    {


        #region ��������
        public event PLCMessageEventHandler OnPLCMessageChanged;
        private PLCErrMessage m_PLCErrMsg = null;
        private PLCConnItem m_PLCConnectItem = new PLCConnItem();
        private bool m_blnConnected = false;
        #endregion

        #region ���Զ���
        public bool Connected
        {
            get { return m_blnConnected; }
        }

        public PLCType CurrPLCType
        {
            get { return PLCType.PLC_5D0; }
        }
        #endregion

        #region ��ʼ��PLC
        /// �������ӣ�ͬһ������ֻ�������һ��
        /// ���Ӻ�1-4
        /// ָ�����Ӳ���
        /// ����10���ƴ���ţ�0��ʾû�д���
        private int Inital_PLC(byte cnnNo, PLCAddrType[] cnnInfo)
        {
            int err;
            //���ݲ�������ȷ
            if (cnnInfo.Length <= 0)
            {
                return -1;
            }
            byte[,] btr = new byte[cnnInfo.Length + 1, 4];
            //ת�����ӱ�
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
            //���ó�ʼ������
            err = load_tool(cnnNo, "s7online", btr);
            return err;
        }
        #endregion

        #region ���캯��
        /// <summary>
        /// PLC����
        /// </summary>
        /// <param name="plcConnect">PLC���Ӳ���</param>
        public PLCOperation5D0(PLCConnItem plcConnect)
        {
            m_PLCConnectItem = plcConnect;
            m_PLCErrMsg = new PLCErrMessage(PLCType.PLC_5D6);
        }

        /// <summary>
        /// ʵ����PLC������
        /// </summary>
        /// <param name="ConnNr">���Ӻ�:ȡֵΪ1-4</param>
        /// <param name="Address">��ַ,һ��Ϊ2</param>
        /// <param name="Segment">�κţ�һ��Ϊ0</param>
        /// <param name="Rack">���ܺ�,һ��Ϊ2</param>
        /// <param name="Slot">�ۺ�,һ��Ϊ0</param>
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

        #region ������ر�
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
                    SendPLCMessage(false, "����ʧ�ܣ�" + GetPLCErrMsgByErrCode(ret));
                    return false;
                }
            }
         
        }

        /// <summary>
        /// <remark>��������PLC</remark>   
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

        #region ��ȡPLC����

        /// <summary>
        /// ��Ҫ��ȡĳ������ֵ,�˴����ֽ�������
        /// </summary>
        /// <param name="dbno">DB��Ԫ</param>
        /// <param name="strAddr">��ַ ��ʽX.Y   XΪ��ַ,YΪλ�� �磺5.3��ʾ��5���ֽڵ�3λ</param>
        /// <param name="retBoolValue">���صĲ���ֵ��1����0</param>
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
        /// ��ȡPLC����,�����ַ�������
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
        /// ��ȡPLC����,��������
        /// </summary>
        /// <param name="dbno">db��Ԫ</param>
        /// <param name="dwno">��ַ</param>
        /// <param name="amount">�ֽ���</param>
        /// <param name="buffer">���ض�ȡ�����ֽ�</param>
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
                    strMsg = "��ȡʧ��:" + GetPLCErrMsgByErrCode(ret);
                }
            }
            catch(Exception ex)
            {
                m_blnConnected = false;
                strMsg = "��ȡ����,ԭ��:"+ex.Message;
            }
            SendPLCMessage(m_blnConnected, strMsg);
            return m_blnConnected;
        }

        #endregion

        #region д��PLC����
        /// <summary>
        /// ��תĳ����ַ��ֵ
        /// </summary>
        /// <param name="dbno">DB��Ԫ</param>
        /// <param name="strAddr">��ַ ��ʽX.Y   XΪ��ַ,YΪλ�� �磺5.3��ʾ��5���ֽڵ�3λ</param>
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
        /// ����ǰ��ַд1��0,�˴����ֽ�������
        /// </summary>
        /// <param name="dbno">DB��Ԫ</param>
        /// <param name="strAddr">��ַ ��ʽX.Y   XΪ��ַ,YΪλ�� �磺5.3��ʾ��5���ֽڵ�3λ</param>
        /// <param name="blnValue">ֵ,��0Ϊ��,����Ϊ��</param>
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
                strMsg = string.Format("д��ֵ<{0}>������Ч���޷��ų�����", strData);
                SendPLCMessage(true, strMsg);
                return false;
            }
        }

        /// <summary>
        /// д��PLC����
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
        /// ��buffer�����е��ֽ�����д��ָ����PLC��ַ��
        /// </summary>
        /// <param name="dbno">db���(100)</param>
        /// <param name="dwno">�����db���ƫ�Ƶ�ַ��(2)</param>
        /// <param name="amount">д���ֽڸ���</param>
        /// <param name="buffer">��д����ֽ�����</param>
        /// <returns>�ɹ�������</returns>
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
                    SendPLCMessage(false, "д��ʧ�ܣ�" + GetPLCErrMsgByErrCode(ret));
                    return false;
                }
            }
            catch(Exception ex)
            {
                SendPLCMessage(false, "д�����,ԭ��:" + ex.Message);
                return false;
            }
        }
        #endregion

        #region �ͷ���Դ
        /// <summary>
        /// �ͷ���Դ
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
        /// �����Ӧ��ʮ���ƴ���
        /// </summary>
        /// <param name="errCode"></param>
        /// <returns></returns>
        private string GetPLCErrMsgByErrCode(int errCode)
        {
            string retValue = "";
            try
            {
                retValue = m_PLCErrMsg.GetErrorMsgByCode(errCode);
                retValue = string.Format("ErrCode:��{0}��,ErrMessage:��{1}��", Convert.ToString(errCode, 16).PadLeft(4, '0').ToUpper(), retValue);
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
