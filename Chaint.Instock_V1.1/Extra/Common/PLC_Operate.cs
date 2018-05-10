using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
namespace Common
{
    /// <summary>
    /// PLC_Operate ��ժҪ˵����
    /// </summary>
    public class PLC_Operate
    {

        //���ӶϿ��¼�
        public delegate void PLCStateEventHandler(object sender,
               PLCStateEventArgs e);


        public event PLCStateEventHandler PLCStateChange;


        protected virtual void OnPLCStateChange(PLCStateEventArgs e)
        {
            if (PLCStateChange != null)
            {
                PLCStateChange(this, e);//Raise the event
            }
        }






        private bool m_Connect = false;
        public bool IsConnect { get { return m_Connect; } }



        /// <summary>
        ///		<remark>PLCĬ�Ϲ��캯��</remark>   
        /// </summary>
        public PLC_Operate()
        {
            //
            // TODO: �ڴ˴���ӹ��캯���߼�
            //
        }


        /// <summary>
        /// ��PLCд������
        /// </summary>
        /// <param name="dbno">ָ��DB���</param>
        /// <param name="dwno">ָ��д�����ʼ�ֽ���ţ�=0��ʾDB0,=1��ʾDB1</param>
        /// <param name="amount">ָ��д��Ķ����ж��ٸ���</param>
        /// <param name="buffer">buffer:д��ֵ</param>
        public void PLC_WriteData(int dbno, int dwno, int amount, byte[] buffer)
        {
            PLCIO.d_field_write(dbno, dwno, amount, buffer);
        }


        /// <summary>
        /// д�뵥��Byte,SmallInt ��д��
        /// </summary>
        /// <param name="dbno"></param>
        /// <param name="dwno"></param>
        /// <param name="Num"></param>
        public void PLC_WriteSmallInt(int dbno, int dwno, int b)
        {
            byte[] bnum = SmallIntToByte(b);

            PLCIO.d_field_write(dbno, dwno, 1, bnum);
        }


        /// <summary>
        /// д�뵥�ֽ�Int
        /// </summary>
        /// <param name="dbno"></param>
        /// <param name="dwno"></param>
        /// <param name="Num"></param>
        public void PLC_WriteInt(int dbno, int dwno, int Num)
        {
            byte[] bnum = IntToByte(Num);

            PLCIO.d_field_write(dbno, dwno, 2, bnum);

        }

        /// <summary>
        /// д��˫�ֽ�DInt
        /// </summary>
        /// <param name="dbno"></param>
        /// <param name="dwno"></param>
        /// <param name="DNum"></param>
        public void PLC_WriteDInt(int dbno, int dwno, int DNum)
        {
            byte[] bnum = DIntToByte(DNum);

            PLCIO.d_field_write(dbno, dwno, 4, bnum);


        }

        /// <summary>
        /// ��ȡPLC DB ����
        /// </summary>
        /// <param name="dbno">DB ���</param>
        /// <param name="dwno">��ȡ���ݵ���ʼλ��</param>
        /// <param name="amount">��ȡ���ֽ���Ŀ</param>
        /// <param name="buffer">���صĶ�ȡֵ</param>
        public void PLC_ReadData(int dbno, int dwno, int amount, byte[] buffer)
        {
            int err = PLCIO.d_field_read(dbno, dwno, amount, buffer);
            if (err > 0)
            {
                OnPLCStateChange(new PLCStateEventArgs(false, "PLC��ȡʧ��"));
                m_Connect = false;
            }
            else
            {
                OnPLCStateChange(new PLCStateEventArgs(true, "PLC��ȡ�ɹ�"));
                m_Connect = true;
            }

        }
        /// <summary>
        ///		<remark>PLC�������캯������������PLC</remark>   
        /// </summary>
        ///     <param name="Address">MPI��ַ</param>
        ///     <param name="Rack">���ܺ�</param>
        ///     <param name="Slot">�ۺ�</param>
        public PLC_Operate(int Address, int Slot, int Rack)
        {
            Connect_PLC(Address, Slot, Rack);
        }

        /// <summary>
        ///		<remark>����PLC</remark>   
        /// </summary>
        ///     <param name="Address">MPI��ַ</param>
        ///     <param name="Rack">���ܺ�</param>
        ///     <param name="Slot">�ۺ�</param>
        public bool Connect_PLC(int Address, int Slot, int Rack)
        {
            if (m_Connect)
                return true;

            PLCIO.PLCInfo[] Cnn = new PLCIO.PLCInfo[1];
            Cnn[0].Addres = (byte)Address;
            Cnn[0].Segment = 0;
            Cnn[0].Slot = (byte)Slot;
            Cnn[0].Rack = (byte)Rack;
            if (PLCIO.Connect_PLC(1, Cnn) == 0)
            {
                OnPLCStateChange(new PLCStateEventArgs(true, "PLC���ӳɹ�"));

                m_Connect = true;
                PLCIO.new_ss(0);

            }
            else
            {
                OnPLCStateChange(new PLCStateEventArgs(false, "PLC����ʧ��"));
                m_Connect = false;
            }

            return m_Connect;
        }

        /// <summary>
        ///		<remark>��������PLC</remark>   
        /// </summary>
        ///     <param name="Address">MPI��ַ</param>
        ///     <param name="Rack">���ܺ�</param>
        ///     <param name="Slot">�ۺ�</param>
        public bool ReConnect_PLC(int Address, int Slot, int Rack)
        {
            if (m_Connect)
                return true;

            //��������֮ǰ �Ͽ�
            int ret = PLCIO.unload_tool();

            PLCIO.PLCInfo[] Cnn = new PLCIO.PLCInfo[1];
            Cnn[0].Addres = (byte)Address;
            Cnn[0].Segment = 0;
            Cnn[0].Slot = (byte)Slot;
            Cnn[0].Rack = (byte)Rack;
            PLCIO.Connect_PLC(1, Cnn);
            System.Threading.Thread.Sleep(100);
            if (PLCIO.Connect_PLC(1, Cnn) == 0)
            {
                OnPLCStateChange(new PLCStateEventArgs(true, "PLC�������ӳɹ�"));
                m_Connect = true;
                PLCIO.new_ss(0);

            }
            else
            {
                OnPLCStateChange(new PLCStateEventArgs(false, "PLC��������ʧ��"));
                m_Connect = false;
            }

            return m_Connect;
        }

        /// <summary>
        /// SmallInt ת��Ϊbyte����
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static byte[] SmallIntToByte(int i)
        {
            byte[] abyte0 = new byte[1];
            abyte0[0] = (byte)(0xff & i);
            return abyte0;
        }

        /// <summary>
        /// Int ת��Ϊbyte����
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static byte[] IntToByte(int i)
        {
            byte[] abyte0 = new byte[2];
            abyte0[1] = (byte)(0xff & i);
            abyte0[0] = (byte)((0xff00 & i) >> 8);
            return abyte0;
        }

        /// <summary>
        /// DInt ת��Ϊbyte����
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static byte[] DIntToByte(int i)
        {
            byte[] abyte0 = new byte[4];
            abyte0[3] = (byte)(0xff & i);
            abyte0[2] = (byte)((0xff00 & i) >> 8);
            abyte0[1] = (byte)((0xff0000 & i) >> 16);
            abyte0[0] = (byte)((0xff000000 & i) >> 24);
            return abyte0;
        }

        public static byte ConvertToByte(System.Collections.BitArray bits)
        {
            if (bits.Count > 8)
                throw new ArgumentException("ConvertToByte can only work with a BitArray containing a maximum of 8 values");
            byte result = 0;
            for (byte i = 0; i < bits.Count; i++)
            {
                if (bits[i])
                    result |= (byte)(1 << i);
            }
            return result;
        }

        /// <summary>
        /// byte����ת��ΪInt
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static int BytesToInt(byte[] bytes, int start)
        {
            int addr = bytes[start + 1] & 0xFF;
            addr |= ((bytes[start] << 8) & 0xFF00);
            return addr;
        }

        /// <summary>
        /// byte����ת��ΪDInt
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static int BytesToDInt(byte[] bytes, int start)
        {
            int addr = bytes[start + 3] & 0xFF;
            addr |= ((bytes[start + 2] << 8) & 0xFF00);
            addr |= ((bytes[start + 1] << 16) & 0xFF0000);
            addr |= (int)((bytes[start] << 24) & 0xFF000000);
            return addr;
        }

        /// <summary>
        /// byte����ת��ΪString
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="start"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string BytesToString(byte[] bytes, int start, int length)
        {
            string ret = "";
            for (int i = start; i < start + length; i++)
            {
                ret = ret + (char)bytes[i];
            }
            return ret;
        }

        public static bool BitOfByte(Byte byte0, Int16 index)
        {
            return ((byte0 >> (index)) & 1) == 1 ? true : false;
            // return ((byte0 >> (7 - index)) & 1) == 1 ? true : false; 
        }

        public static bool BitOfBuffer(Byte[] Buffer, int Byteindex, Int16 Bitindex)
        {
            return ((Buffer[Byteindex] >> (Bitindex)) & 1) == 1 ? true : false;   //���Խ�� ����ȡ��
            // return ((Buffer[Byteindex] >> (7 - Bitindex)) & 1) == 1 ? true : false; 
        }

        public static bool BitOfBuffer(Byte[] Buffer, double index)
        {
            string str = index.ToString("f1");
            return BitOfBuffer(Buffer, str);

        }

        /// <summary>
        /// ͨ��������ַ�����ȡBuffer��bit  ���� ��"5.3"
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="str">������ַ��� ����"5.4"</param>
        /// <returns></returns>
        public static bool BitOfBuffer(Byte[] Buffer, string str)
        {
            string[] strs = str.Split('.');
            int Byteindex = Convert.ToInt32(strs[0]);
            Int16 Bitindex = Convert.ToInt16(strs[1]);   //���Խ�� ��λ
            return BitOfBuffer(Buffer, Byteindex, Bitindex);
        }

        #region ������Byte ��ת String �ĺ���

        public static string ByteToString(byte[] InBytes)
        {
            string StringOut = "";
            foreach (byte InByte in InBytes)
            {
                StringOut += String.Format("{0:X2}", InByte);
            }
            return StringOut;
        }

        public static string ByteToString(byte[] InBytes, string Param) ///Ϊ����ʾ���������Ҫ��ÿ���ֽں��油��һ���ո�ȣ�
        {
            string StringOut = "";
            foreach (byte InByte in InBytes)
            {
                StringOut = StringOut + String.Format("{0:X2}", InByte) + Param;
            }
            StringOut = StringOut.Substring(0, StringOut.Length - Param.Length);
            return StringOut;
        }


        public static string ByteToString(byte iIn)
        {
            string sTmp = "";
            //   sTmp = Supply00( iIn.ToString("X") , 0, "0", 2);
            sTmp = sTmp + String.Format("{0:X2}", iIn);
            return sTmp;
        }
        public static string StrToHexAsc(string inString)
        {
            char[] inStringToChar = inString.ToCharArray();
            string outString = "";
            for (int i = 0; i < inStringToChar.Length; i++)
            {
                outString += String.Format("{0:X2}", Convert.ToByte(inStringToChar[i]));
            }
            return outString;
        }


        public static byte[] StringToByte(string InString, string Param)
        {
            string[] ByteStrings;
            ByteStrings = InString.Split(Param.ToCharArray());

            byte[] ByteOut;
            ByteOut = new byte[ByteStrings.Length];
            for (int i = 0; i <= ByteStrings.Length - 1; i++)
            {
                ByteOut[i] = Convert.ToByte(ByteStrings[i], 16);
            }

            return ByteOut;
        }

        public static byte[] StringToByte(string InString)
        {
            string[] ByteStrings;
            ByteStrings = new string[InString.Length / 2];
            for (int i = 0; i < ByteStrings.Length; i++)
            {
                ByteStrings[i] = InString.Substring(i * 2, 2);
            }

            byte[] ByteOut;
            ByteOut = new byte[ByteStrings.Length];
            for (int i = 0; i <= ByteStrings.Length - 1; i++)
            {
                ByteOut[i] = Convert.ToByte(ByteStrings[i], 16);
            }

            return ByteOut;
        }



        //��00����
        public static string Supply00(string sOldStr, int SupplyWay, string SupplyChar, long SupplyNum)
        {
            int i;
            string sTmp;

            sTmp = "";
            for (i = sOldStr.Length; i < SupplyNum; i++)
                sTmp = sTmp + SupplyChar;
            switch (SupplyWay)
            {
                case 0: //ǰ����
                    sTmp = sTmp + sOldStr;
                    break;
                case 1: //�󲹳�
                    sTmp = sOldStr + sTmp;
                    break;
                default:
                    sTmp = sOldStr;
                    break;
            }
            return sTmp;
        }


        public static string HexAscToStr(string inString)
        {

            byte[] CardData;
            CardData = StringToByte(inString);

            string outString = Encoding.ASCII.GetString(CardData);
            return outString;
        }

        #endregion

    }//Class Connect_PLC

    public class PLCStateEventArgs : EventArgs
    {
        private bool _IsConnect;
        private string _Info;
        public PLCStateEventArgs(bool isConnect, string msg)
        {
            this._IsConnect = isConnect;
            this._Info = msg;

        }
        public bool IsConnect
        {
            get
            {
                return _IsConnect;
            }
        }
        public string Info
        {
            get
            {
                return _Info;
            }
        }
    }






    public class PLCIO
    {
        public PLCIO()
        {
            //
            // TODO: �ڴ˴���ӹ��캯���߼�
            //
            //���ӳ�ʼ��
        }

        #region
        [DllImport("w95_s7m.dll")]
        public extern static int load_tool(byte nr, string device, byte[,] adr_table);
        #endregion

        #region
        //ȡ������
        [DllImport("w95_s7m.dll")]
        public extern static int unload_tool();
        #endregion

        #region
        //����һ������
        [DllImport("w95_s7m.dll")]
        public extern static int new_ss(byte no);
        #endregion

        /// 
        /// ��DB�ж�ȡ����(Byte)
        /// 
        /// dbno:ָ��DB���
        /// dwno:ָ����ȡ����ʼ�ֽ���ţ�=0��ʾDB0,=1��ʾDB1
        /// amount:ָ����ȡ�Ķ����ж��ٸ���
        /// buffer:����ֵ
        /// 
        [DllImport("w95_s7m.dll")]
        public extern static int d_field_read(int dbno, int dwno, int amount, byte[] buffer);

        /// 
        /// ��DB��д������(Byte)
        /// 
        /// dbno:ָ��DB���
        /// dwno:ָ��д�����ʼ�ֽ���ţ�=0��ʾDB0,=1��ʾDB1
        /// amount:ָ��д��Ķ����ж��ٸ���
        /// buffer:д��ֵ
        /// 
        [DllImport("w95_s7m.dll")]
        public extern static int d_field_write(int dbno, int dwno, int amount, byte[] buffer);


        #region �������ⲿ��ϵ�Ľṹ����
        /// 
        /// ����MPI���Ӳ���
        /// 
        public struct PLCInfo
        {
            public byte Addres;  // ����CPU��MPI��ַ һ��Ϊ2
            public byte Segment; // ����ε�ַ һ��Ϊ0
            public byte Rack;    // ����CPU�Ļ��ܺ� һ��Ϊ2
            public byte Slot;    // ����CPU�Ĳۺ� һ��Ϊ0
        }
        #endregion

        #region �붯̬�⺯�����Ӧ�Ĺ�������
        /// �������ӣ�ͬһ������ֻ�������һ��
        /// ���Ӻ�1-4
        /// ָ�����Ӳ���
        /// ����10���ƴ���ţ�0��ʾû�д���
        public static int Connect_PLC(byte cnnNo, PLCInfo[] cnnInfo)
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
                btr[i, 2] = cnnInfo[i].Slot;
                btr[i, 3] = cnnInfo[i].Rack;
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
    }




    public class PLC6_Operate
    {

        //���ӶϿ��¼�
        public delegate void PLCStateEventHandler(object sender,
               PLCStateEventArgs e);


        public event PLCStateEventHandler PLCStateChange;


        protected virtual void OnPLCStateChange(PLCStateEventArgs e)
        {
            if (PLCStateChange != null)
            {
                PLCStateChange(this, e);//Raise the event
            }
        }






        private bool m_Connect = false;
        public bool IsConnect { get { return m_Connect; } }



        /// <summary>
        ///		<remark>PLCĬ�Ϲ��캯��</remark>   
        /// </summary>
        public PLC6_Operate()
        {
            //
            // TODO: �ڴ˴���ӹ��캯���߼�
            //
        }


        /// <summary>
        /// ��PLCд������
        /// </summary>
        /// <param name="dbno">ָ��DB���</param>
        /// <param name="dwno">ָ��д�����ʼ�ֽ���ţ�=0��ʾDB0,=1��ʾDB1</param>
        /// <param name="amount">ָ��д��Ķ����ж��ٸ���</param>
        /// <param name="buffer">buffer:д��ֵ</param>
        public void PLC_WriteData(ushort dbno, ushort dwno, uint amount, byte[] buffer)
        {
            int ret = Prodave6.field_write_ex6(Prodave6.FieldType.D, dbno, dwno, amount, (ushort)buffer.Length, buffer);


        }


        /// <summary>
        /// д�뵥��Byte,SmallInt ��д��
        /// </summary>
        /// <param name="dbno"></param>
        /// <param name="dwno"></param>
        /// <param name="Num"></param>
        public void PLC_WriteSmallInt(ushort dbno, ushort dwno, int b)
        {
            byte[] bnum = SmallIntToByte(b);
            Prodave6.field_write_ex6(Prodave6.FieldType.D, dbno, dwno, 1, 1, bnum);
        }


        /// <summary>
        /// д�뵥�ֽ�Int
        /// </summary>
        /// <param name="dbno"></param>
        /// <param name="dwno"></param>
        /// <param name="Num"></param>
        public bool PLC_WriteInt(ushort dbno, ushort dwno, int Num)
        {
            byte[] bnum = IntToByte(Num);

            //PLCIO.d_field_write(dbno, dwno, 2, bnum);
            int ret = Prodave6.field_write_ex6(Prodave6.FieldType.D, dbno, dwno, 2, 2, bnum);

            return ret == 0;
        }



        /// <summary>
        /// д��˫�ֽ�DInt
        /// </summary>
        /// <param name="dbno"></param>
        /// <param name="dwno"></param>
        /// <param name="DNum"></param>
        public bool PLC_WriteDInt(ushort dbno, ushort dwno, int DNum)
        {
            byte[] bnum = DIntToByte(DNum);

            //PLCIO.d_field_write(dbno, dwno, 4, bnum);
            int ret = Prodave6.field_write_ex6(Prodave6.FieldType.D, dbno, dwno, 4, 4, bnum);

            return ret == 0;

        }


        /// <summary>
        /// ��ȡPLC DB ����
        /// </summary>
        /// <param name="dbno">DB ���</param>
        /// <param name="dwno">��ȡ���ݵ���ʼλ��</param>
        /// <param name="amount">��ȡ���ֽ���Ŀ</param>
        /// <param name="buffer">���صĶ�ȡֵ</param>
        public void PLC_ReadData(ushort dbno, ushort dwno, uint amount, byte[] buffer)
        {
            uint DataLen = 0;

            //int err = PLCIO.d_field_read(dbno, dwno, amount, buffer);
            int err = Prodave6.field_read_ex6(Prodave6.FieldType.D, dbno, dwno, amount, (uint)buffer.Length, buffer, ref DataLen);

            if (err > 0)
            {
                OnPLCStateChange(new PLCStateEventArgs(false, "PLC��ȡʧ��"));
                m_Connect = false;
            }
            else
            {
                OnPLCStateChange(new PLCStateEventArgs(true, "PLC��ȡ�ɹ�"));
                m_Connect = true;
            }

        }
        /// <summary>
        ///		<remark>PLC�������캯������������PLC</remark>   
        /// </summary>
        ///     <param name="ConNr">defualt 63(first connection),  connection number��(0 ... 63)��(max. 64 connections);</param>
        ///     <param name="Address">PLC IP��ַ,ex:192.168.1.10</param>
        ///     <param name="Rack">���ܺ�</param>
        ///     <param name="Slot">�ۺ�</param>
        //public PLC6_Operate(ushort ConNr, string Address, ushort Slot, ushort Rack)
        public PLC6_Operate(ushort ConNr, PLC.PLCAddress plcAddress)
        {
            Connect_PLC(ConNr, plcAddress);
        }

        /// <summary>
        ///		<remark>����PLC</remark>   
        /// </summary>
        ///     <param name="ConNr">defualt 63(first connection),  connection number��(0 ... 63)��(max. 64 connections);</param>
        ///     <param name="Address">PLC IP��ַ,ex:192.168.1.10</param>
        ///     <param name="Rack">���ܺ�</param>
        ///     <param name="Slot">�ۺ�</param>
        //public bool Connect_PLC(ushort ConNr, string Address, ushort Slot, ushort Rack)
        public bool Connect_PLC(ushort ConNr, PLC.PLCAddress plcAddress)
        {
            if (m_Connect)
                return true;

           // string[] IPstrs = Address.Split('.');
            string AccessPoint = "S7ONLINE"; // Default access point����S7ONLINE
            Prodave6.CON_TABLE_TYPE ConTable;// Connection tablE
            int ConTableLen = System.Runtime.InteropServices.Marshal.SizeOf(typeof(Prodave6.CON_TABLE_TYPE));// Length of the connection tablE
            int RetValue;
            //ConTable.Adr=new byte[]{192,168,1,200,0,0};
            //ConTable.Adr = new byte[] { Convert.ToByte(IPstrs[0]), Convert.ToByte(IPstrs[1]), Convert.ToByte(IPstrs[2]), Convert.ToByte(IPstrs[3]), 0, 0 };
            ConTable.Adr = plcAddress.Adr;//new byte[] { Convert.ToByte(Address),0,0,0,0,0};
            ConTable.AdrType = plcAddress.AdrType;//1; // Type of address: MPI/PB (1), IP (2), MAC (3)
            ConTable.SlotNr = plcAddress.SlotNr;// (byte)Slot; // ��ۺ�
            ConTable.RackNr = plcAddress.RackNr;// (byte)Rack; // ���ܺ�
            RetValue = Prodave6.LoadConnection_ex6(ConNr, AccessPoint, ConTableLen, ref ConTable);


            if (RetValue == 0)
            {
                //��������
                RetValue = Prodave6.SetActiveConnection_ex6(ConNr);

                if (RetValue == 0)
                {
                    m_Connect = true;
                    OnPLCStateChange(new PLCStateEventArgs(true, "PLC���ӳɹ�"));
                }
                else
                {
                    m_Connect = false;
                    //���²���GetErrorMessage_ex6
                    int ErrorNr = RetValue; // Block boundary exceeded, correct the number
                    StringBuilder Buffer = new StringBuilder(300); // Transfer buffer for error text
                    UInt32 BufLen = (UInt32)Buffer.Capacity; // Buffer length�������� 
                    Prodave6.GetErrorMessage_ex6(ErrorNr, BufLen, Buffer);
                    Utils.WriteTxtLog(Utils.FilePath_txtPLCLog, " ����ʧ�� error code :" + RetValue + " ����ʧ�� error msg :" + Buffer.ToString());

                }

            }
            else
            {
                OnPLCStateChange(new PLCStateEventArgs(false, "PLC����ʧ��"));
                m_Connect = false;
            }

            return m_Connect;
        }

        /// <summary>
        ///		<remark>��������PLC</remark>   
        /// </summary>
        ///     <param name="ConNr">defualt 63(first connection),  connection number��(0 ... 63)��(max. 64 connections);</param>
        ///     <param name="Address">PLC IP��ַ,ex:192.168.1.10</param>
        ///     <param name="Rack">���ܺ�</param>
        ///     <param name="Slot">�ۺ�</param>
        //public bool ReConnect_PLC(ushort ConNr, string Address, ushort Slot, ushort Rack)
        public bool ReConnect_PLC(ushort ConNr, PLC.PLCAddress plcAddress)
        {
            if (m_Connect)
                return true;

            //��������֮ǰ �Ͽ�
            //int ret = PLCIO.unload_tool();
            int RetValue = Prodave6.UnloadConnection_ex6(ConNr);
            System.Threading.Thread.Sleep(100);
            //PLCIO.PLCInfo[] Cnn = new PLCIO.PLCInfo[1];
            //Cnn[0].Addres = (byte)Address;
            //Cnn[0].Segment = 0;
            //Cnn[0].Slot = (byte)Slot;
            //Cnn[0].Rack = (byte)Rack;
            //PLCIO.Connect_PLC(1, Cnn);

            //δ����ʱ�Ͽ��Ļ���28693
            if (RetValue == 0 || RetValue == 28720 || RetValue == 28693)
            {//�Ͽ��ɹ� 
                //if (Connect_PLC(ConNr, Address, Slot, Rack))
                if (Connect_PLC(ConNr, plcAddress))   
                {
                    OnPLCStateChange(new PLCStateEventArgs(true, "PLC�������ӳɹ�"));
                    m_Connect = true;
                }
                else
                {
                    OnPLCStateChange(new PLCStateEventArgs(false, "PLC��������ʧ��"));
                    m_Connect = false;
                }
            }
            else
            {//�Ͽ�ʧ��
                OnPLCStateChange(new PLCStateEventArgs(false, "PLC��������ʧ��:�Ͽ�����ʧ��"));
                m_Connect = false;
            }

            return m_Connect;
        }

        /// <summary>
        /// SmallInt ת��Ϊbyte����
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static byte[] SmallIntToByte(int i)
        {
            byte[] abyte0 = new byte[1];
            abyte0[0] = (byte)(0xff & i);
            return abyte0;
        }


        /// <summary>
        /// Int ת��Ϊbyte����
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static byte[] IntToByte(int i)
        {
            byte[] abyte0 = new byte[2];
            abyte0[1] = (byte)(0xff & i);
            abyte0[0] = (byte)((0xff00 & i) >> 8);
            return abyte0;
        }

        /// <summary>
        /// DInt ת��Ϊbyte����
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static byte[] DIntToByte(int i)
        {
            byte[] abyte0 = new byte[4];
            abyte0[3] = (byte)(0xff & i);
            abyte0[2] = (byte)((0xff00 & i) >> 8);
            abyte0[1] = (byte)((0xff0000 & i) >> 16);
            abyte0[0] = (byte)((0xff000000 & i) >> 24);
            return abyte0;
        }


        public static byte ConvertToByte(System.Collections.BitArray bits)
        {
            if (bits.Count > 8)
                throw new ArgumentException("ConvertToByte can only work with a BitArray containing a maximum of 8 values");
            byte result = 0;
            for (byte i = 0; i < bits.Count; i++)
            {
                if (bits[i])
                    result |= (byte)(1 << i);
            }
            return result;
        }



        /// <summary>
        /// byte����ת��ΪInt
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static int BytesToInt(byte[] bytes, int start)
        {
            int addr = bytes[start + 1] & 0xFF;
            addr |= ((bytes[start] << 8) & 0xFF00);
            return addr;
        }

        /// <summary>
        /// byte����ת��ΪDInt
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static int BytesToDInt(byte[] bytes, int start)
        {
            int addr = bytes[start + 3] & 0xFF;
            addr |= ((bytes[start + 2] << 8) & 0xFF00);
            addr |= ((bytes[start + 1] << 16) & 0xFF0000);
            addr |= (int)((bytes[start] << 24) & 0xFF000000);
            return addr;
        }

        /// <summary>
        /// byte����ת��ΪString
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="start"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string BytesToString(byte[] bytes, int start, int length)
        {
            string ret = "";
            for (int i = start; i < start + length; i++)
            {
                ret = ret + (char)bytes[i];
            }
            return ret;
        }

        /// <summary>
        /// byte����ת��Ϊ��ͨString RollID
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="start"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string BytesToCString(byte[] bytes, int start, int length)
        {
            string ret = "";
            for (int i = start; i < start + length; i++)
            {
                char c = (char)bytes[i];
                if (Char.IsLetterOrDigit(c))
                    ret = ret + c;
                else
                {
                    break;
                }
            }
            return ret;
        }



        public static bool BitOfByte(Byte byte0, Int16 index)
        {
            return ((byte0 >> (index)) & 1) == 1 ? true : false;
            // return ((byte0 >> (7 - index)) & 1) == 1 ? true : false; 
        }

        public static bool BitOfBuffer(Byte[] Buffer, int Byteindex, Int16 Bitindex)
        {
            return ((Buffer[Byteindex] >> (Bitindex)) & 1) == 1 ? true : false;   //���Խ�� ����ȡ��
            // return ((Buffer[Byteindex] >> (7 - Bitindex)) & 1) == 1 ? true : false; 
        }


        public static bool BitOfBuffer(Byte[] Buffer, double index)
        {
            string str = index.ToString("f1");
            return BitOfBuffer(Buffer, str);

        }

        /// <summary>
        /// ͨ��������ַ�����ȡBuffer��bit  ���� ��"5.3"
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="str">������ַ��� ����"5.4"</param>
        /// <returns></returns>
        public static bool BitOfBuffer(Byte[] Buffer, string str)
        {
            string[] strs = str.Split('.');
            int Byteindex = Convert.ToInt32(strs[0]);
            Int16 Bitindex = Convert.ToInt16(strs[1]);   //���Խ�� ��λ
            return BitOfBuffer(Buffer, Byteindex, Bitindex);
        }


        #region ������Byte ��ת String �ĺ���

        public static string ByteToString(byte[] InBytes)
        {
            string StringOut = "";
            foreach (byte InByte in InBytes)
            {
                StringOut += String.Format("{0:X2}", InByte);
            }
            return StringOut;
        }

        public static string ByteToString(byte[] InBytes, string Param) ///Ϊ����ʾ���������Ҫ��ÿ���ֽں��油��һ���ո�ȣ�
        {
            string StringOut = "";
            foreach (byte InByte in InBytes)
            {
                StringOut = StringOut + String.Format("{0:X2}", InByte) + Param;
            }
            StringOut = StringOut.Substring(0, StringOut.Length - Param.Length);
            return StringOut;
        }


        public static string ByteToString(byte iIn)
        {
            string sTmp = "";
            //   sTmp = Supply00( iIn.ToString("X") , 0, "0", 2);
            sTmp = sTmp + String.Format("{0:X2}", iIn);
            return sTmp;
        }
        public static string StrToHexAsc(string inString)
        {
            char[] inStringToChar = inString.ToCharArray();
            string outString = "";
            for (int i = 0; i < inStringToChar.Length; i++)
            {
                outString += String.Format("{0:X2}", Convert.ToByte(inStringToChar[i]));
            }
            return outString;
        }


        public static byte[] StringToByte(string InString, string Param)
        {
            string[] ByteStrings;
            ByteStrings = InString.Split(Param.ToCharArray());

            byte[] ByteOut;
            ByteOut = new byte[ByteStrings.Length];
            for (int i = 0; i <= ByteStrings.Length - 1; i++)
            {
                ByteOut[i] = Convert.ToByte(ByteStrings[i], 16);
            }

            return ByteOut;
        }

        public static byte[] StringToByte(string InString)
        {
            string[] ByteStrings;
            ByteStrings = new string[InString.Length / 2];
            for (int i = 0; i < ByteStrings.Length; i++)
            {
                ByteStrings[i] = InString.Substring(i * 2, 2);
            }

            byte[] ByteOut;
            ByteOut = new byte[ByteStrings.Length];
            for (int i = 0; i <= ByteStrings.Length - 1; i++)
            {
                ByteOut[i] = Convert.ToByte(ByteStrings[i], 16);
            }

            return ByteOut;
        }



        //��00����
        public static string Supply00(string sOldStr, int SupplyWay, string SupplyChar, long SupplyNum)
        {
            int i;
            string sTmp;

            sTmp = "";
            for (i = sOldStr.Length; i < SupplyNum; i++)
                sTmp = sTmp + SupplyChar;
            switch (SupplyWay)
            {
                case 0: //ǰ����
                    sTmp = sTmp + sOldStr;
                    break;
                case 1: //�󲹳�
                    sTmp = sOldStr + sTmp;
                    break;
                default:
                    sTmp = sOldStr;
                    break;
            }
            return sTmp;
        }


        public static string HexAscToStr(string inString)
        {

            byte[] CardData;
            CardData = StringToByte(inString);

            string outString = Encoding.ASCII.GetString(CardData);
            return outString;
        }

        #endregion






    }//Class Connect_PLC


    public class Prodave6
    {
        #region ��ֵ���壨���ڼ���ֵ��
        public const int MAX_CONNECTIONS = 64; // 64 is default in PRODAVE
        public const int MAX_DEVNAME_LEN = 128;// e.g. "S7ONLINE"
        public const int MAX_BUFFERS = 64; // 64 for blk_read() and blk_write() 
        public const int MAX_BUFFER = 65536; // Transfer buffer for error text) 
        #endregion

        #region �ṹ�嶨��
        public struct CON_TABLE_TYPE//������plc��ַ���Ա�
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            //public CON_ADR_TYPE Adr; // connection address
            public byte[] Adr; // connection address
            // MPI/PB station address (2)
            // IP address (192.168.0.1)
            // MAC address (08-00-06-01-AA-BB)
            public byte AdrType; // Type of address: MPI/PB (1), IP (2), MAC (3)
            public byte SlotNr; // Slot number
            public byte RackNr; // Rack number
        }
        public enum DatType : byte//PLC��������
        {
            BYTE = 0x02,
            WORD = 0x04,
            DWORD = 0x06,
        }
        public enum FieldType : byte//PLC��������
        {
            //Value types as ASCII characters�������Ͷ�Ӧ��ASCII�ַ�
            //data byte (d/D)
            d = 100,
            D = 68,
            //input byte (e/E)
            e = 101,
            E = 69,
            //output byte (a/A)
            a = 97,
            A = 65,
            //memory byte (m/M)
            m = 109,
            M = 77,
            //timer word (t/T),
            t = 116,
            T = 84,
        }
        #endregion

        #region PLC��������

        [DllImport("Prodave6.dll")]//����PLC����
        //���������Ӻţ�0-63������ֵ"S7ONLINE"��������plc��ַ���Ա��ȣ��ֽ�Ϊ��λ����ֵ9����������plc��ַ���Ա�
        public extern static int LoadConnection_ex6(int ConNr, string pAccessPoint, int ConTableLen, ref  CON_TABLE_TYPE pConTable);

        [DllImport("Prodave6.dll")]//�Ͽ�PLC����
        //���������Ӻţ�0-63��
        public extern static int UnloadConnection_ex6(UInt16 ConNr);

        [DllImport("Prodave6.dll")]//����PLC���Ӳ���
        //���������Ӻţ�0-63��
        public extern static int SetActiveConnection_ex6(UInt16 ConNr);

        [DllImport("Prodave6.dll")]//PLC db����ȡ����
        //������data block�š�Ҫ��ȡ���������͡���ʼ��ַ�š���Ҫ��ȡ���͵����������������ȣ��ֽ�Ϊ��λ���������������������ݽ����ĳ���
        public extern static int db_read_ex6(UInt16 BlkNr, DatType DType, UInt16 StartNr, ref UInt32 pAmount, UInt32 BufLen,
            byte[] pBuffer, ref UInt32 pDatLen);

        [DllImport("Prodave6.dll")]//PLC db��д�����
        //������data block�š�Ҫд����������͡���ʼ��ַ�š���Ҫд�����͵����������������ȣ��ֽ�Ϊ��λ����������
        public extern static int db_write_ex6(UInt16 BlkNr, DatType Type, UInt16 StartNr, ref UInt32 pAmount, UInt32 BufLen,
            byte[] pBuffer);

        [DllImport("Prodave6.dll")]//PLC ��������ȡ����
        //������Ҫ��ȡ�������͡�data block��(DB�����У�Ĭ��Ϊ0)����ʼ��ַ�š���Ҫ��ȡ���͵�������
        //���������ȣ��ֽ�Ϊ��λ���������������������ݽ����ĳ���
        public extern static int field_read_ex6(FieldType FType, UInt16 BlkNr, UInt16 StartNr, UInt32 pAmount, UInt32 BufLen,
            byte[] pBuffer, ref UInt32 pDatLen);

        [DllImport("Prodave6.dll")]//PLC ������д�����
        //������Ҫд��������͡�data block��(DB�����У�Ĭ��Ϊ0)����ʼ��ַ�š���Ҫд�����͵�������
        //���������ȣ��ֽ�Ϊ��λ����������
        public extern static int field_write_ex6(FieldType FType, UInt16 BlkNr, UInt16 StartNr, UInt32 pAmount, UInt32 BufLen,
            byte[] pBuffer);

        [DllImport("Prodave6.dll")]//PLC M��ĳ�ֽڵ�ĳλ��ȡ����
        //������M���ֽںš�λ�š���ǰ��ֵ(0/1)
        public extern static int mb_bittest_ex6(UInt16 MbNr, UInt16 BitNr, ref int pValue);

        [DllImport("Prodave6.dll")]//PLC M��ĳ�ֽڵ�ĳλд�����
        //������M���ֽںš�λ�š�Ҫд���ֵ(0/1)
        public extern static int mb_setbit_ex6(UInt16 MbNr, UInt16 BitNr, byte Value);

        #endregion

        #region PLC200�����ݴ��亯��

        [DllImport("Prodave6.dll")]//200ϵ��PLC ��������ȡ����
        //������Ҫ��ȡ�������͡�data block��(DB�����У�Ĭ��Ϊ0)����ʼ��ַ�š���Ҫ��ȡ���͵�������
        //���������ȣ��ֽ�Ϊ��λ���������������������ݽ����ĳ���
        public extern static int as200_field_read_ex6(FieldType FType, UInt16 BlkNr, UInt16 StartNr, UInt32 pAmount, UInt32 BufLen,
            byte[] pBuffer, ref UInt32 pDatLen);

        [DllImport("Prodave6.dll")]//200ϵ��PLC ������д�����
        //������Ҫд��������͡�data block��(DB�����У�Ĭ��Ϊ0)����ʼ��ַ�š���Ҫд�����͵�������
        //���������ȣ��ֽ�Ϊ��λ����������
        public extern static int as200_field_write_ex6(FieldType FType, UInt16 BlkNr, UInt16 StartNr, UInt32 pAmount, UInt32 BufLen,
            byte[] pBuffer);

        [DllImport("Prodave6.dll")]//200ϵ��PLC M��ĳ�ֽڵ�ĳλ��ȡ����
        //������M���ֽںš�λ�š���ǰ��ֵ(0/1)
        public extern static int as200_mb_bittest_ex6(UInt16 MbNr, UInt16 BitNr, ref int pValue);

        [DllImport("Prodave6.dll")]//200ϵ��PLC M��ĳ�ֽڵ�ĳλд�����
        //������M���ֽںš�λ�š�Ҫд���ֵ(0/1)
        public extern static int as200_mb_setbit_ex6(UInt16 MbNr, UInt16 BitNr, byte Value);

        #endregion

        #region PLC����ת������

        [DllImport("Prodave6.dll")]//��ϴ�����Ϣ����
        //������������š���������С���ֽ�Ϊ��λ����������
        public extern static int GetErrorMessage_ex6(int ErrorNr, UInt32 BufLen, [MarshalAs(UnmanagedType.LPStr)] StringBuilder pBuffer);

        [DllImport("Prodave6.dll")]//S7������ת����PC������
        //������S7��������PC������
        public extern static int gp_2_float_ex6(UInt32 gp, ref float pieee);

        [DllImport("Prodave6.dll")]//PC������ת����S7������
        //������PC��������S7������
        public extern static int float_2_gp_ex6(float ieee, ref UInt32 pgp);

        [DllImport("Prodave6.dll")]//���ĳ�ֽڵ�ĳλ��ֵ��0��1
        //�������ֽ�ֵ��λ��
        public extern static int testbit_ex6(byte Value, int BitNr);

        [DllImport("Prodave6.dll")]//���ĳ�ֽڵ�byteֵת����int����
        //������byteֵ��int����(����Ϊ8)
        public extern static void byte_2_bool_ex6(byte Value, int[] pBuffer);


        [DllImport("Prodave6.dll")]//���ĳ�ֽڵ�int����ת����byteֵ
        //������int����(����Ϊ8)
        public extern static byte bool_2_byte_ex6(int[] pBuffer);

        [DllImport("Prodave6.dll")]//�������ݵĸߵ��ֽڡ���16λ����
        //������������������
        public extern static UInt16 kf_2_integer_ex6(UInt16 wValue);//16λ���ݡ���WORD

        [DllImport("Prodave6.dll")]//�������ݵĸߵ��ֽڡ���32λ����
        //������������������
        public extern static UInt32 kf_2_long_ex6(UInt32 dwValue);//32λ���ݡ���DWORD

        [DllImport("Prodave6.dll")]//�������ݻ������ĵĸߵ��ֽ���������pBuffer[0]��pBuffer[1]��pBuffer[2]��pBuffer[3]����
        //�����������������ݻ�������Ҫ�������ֽ�������Amount=pBuffer.Length���򽻻�ȫ������
        public extern static void swab_buffer_ex6(byte[] pBuffer, UInt32 Amount);

        [DllImport("Prodave6.dll")]//�������ݻ�����
        //������Ŀ�����ݻ�������Դ���ݻ�������Ҫ���Ƶ��������ֽ�Ϊ��λ��
        public extern static void copy_buffer_ex6(byte[] pTargetBuffer, byte[] pSourceBuffer, UInt32 Amount);

        [DllImport("Prodave6.dll")]//�Ѷ��������鴫����BCD������顪��16λ����
        //������Ҫ��������飬Ҫ������ֽ�����ת��ǰ�Ƿ��Ƚ����ߵ��ֽڣ�ת�����Ƿ�Ҫ�����ߵ��ֽ�
        //InBytechangeΪ1��ת��BCD��֮ǰ���Ƚ����ߵ��ֽ�
        //OutBytechangeΪ1��ת��BCD��֮���ٽ����ߵ��ֽ�
        //���InBytechange��OutBytechange��û����1���򲻷����ߵ�λ�Ľ���
        //16λ����BCD��ֵ����ɷ�Χ�ǣ�+999 ���� -999
        public extern static void ushort_2_bcd_ex6(UInt16[] pwValues, UInt32 Amount, int InBytechange, int OutBytechange);//16λ���ݡ���WORD

        [DllImport("Prodave6.dll")]//�Ѷ��������鴫����BCD������顪��32λ����
        //������Ҫ��������飬Ҫ������ֽ�����ת��ǰ�Ƿ��Ƚ����ߵ��ֽڣ�ת�����Ƿ�Ҫ�����ߵ��ֽ�
        //InBytechangeΪ1��ת��BCD��֮ǰ���Ƚ����ߵ��ֽ�
        //OutBytechangeΪ1��ת��BCD��֮���ٽ����ߵ��ֽ�
        //���InBytechange��OutBytechange��û����1���򲻷����ߵ�λ�Ľ���
        //32λ����BCD��ֵ����ɷ�Χ�ǣ�+9 999 999 ���� -9 999 999
        public extern static void ulong_2_bcd_ex6(UInt32[] pdwValues, UInt32 Amount, int InBytechange, int OutBytechange);//32λ���ݡ���DWORD

        [DllImport("Prodave6.dll")]//��BCD������鴫���ɶ��������顪��16λ����
        //������Ҫ��������飬Ҫ������ֽ�����ת��ǰ�Ƿ��Ƚ����ߵ��ֽڣ�ת�����Ƿ�Ҫ�����ߵ��ֽ�
        //InBytechangeΪ1��ת��BCD��֮ǰ���Ƚ����ߵ��ֽ�
        //OutBytechangeΪ1��ת��BCD��֮���ٽ����ߵ��ֽ�
        //���InBytechange��OutBytechange��û����1���򲻷����ߵ�λ�Ľ���
        //16λ����BCD��ֵ����ɷ�Χ�ǣ�+999 ���� -999
        public extern static void bcd_2_ushort_ex6(UInt16[] pwValues, UInt32 Amount, int InBytechange, int OutBytechange);//16λ���ݡ���WORD

        [DllImport("Prodave6.dll")]//��BCD������鴫���ɶ��������顪��32λ����
        //������Ҫ��������飬Ҫ������ֽ�����ת��ǰ�Ƿ��Ƚ����ߵ��ֽڣ�ת�����Ƿ�Ҫ�����ߵ��ֽ�
        //InBytechangeΪ1��ת��BCD��֮ǰ���Ƚ����ߵ��ֽ�
        //OutBytechangeΪ1��ת��BCD��֮���ٽ����ߵ��ֽ�
        //���InBytechange��OutBytechange��û����1���򲻷����ߵ�λ�Ľ���
        //32λ����BCD��ֵ����ɷ�Χ�ǣ�+9 999 999 ���� -9 999 999
        public extern static void bcd_2_ulong_ex6(UInt32[] pdwValues, UInt32 Amount, int InBytechange, int OutBytechange);//32λ���ݡ���DWORD

        [DllImport("Prodave6.dll")]//�鿴64����������Щ��ռ�ã���Щ�Ѿ�����
        //���������仺����ֽڳ��ȣ�64λ���ȵ�����(0��1)
        public extern static void GetLoadedConnections_ex6(UInt32 BufLen, int[] pBuffer);

        #endregion

        #region �Զ��帨������

        public static UInt16 bytes_2_word(byte dbb0, byte dbb1)//���ߵ�2��byteת����1��word
        {
            UInt16 dbw0;
            dbw0 = (UInt16)(dbb0 * 256 + dbb1);
            return dbw0;
        }

        public static UInt32 bytes_2_dword(byte dbb0, byte dbb1, byte dbb2, byte dbb3)//���ߵ�4��byteת����1��dword
        {
            UInt32 dbd0;
            dbd0 = (UInt32)(dbb0 * 16777216 + dbb1 * 65536 + dbb2 * 256 + dbb3);
            return dbd0;
        }

        public static UInt32 words_2_dword(UInt16 dbw0, UInt16 dbw2)//���ߵ�2��wordת����1��dword
        {
            UInt32 dbd0;
            dbd0 = (UInt32)(dbw0 * 65536 + dbw2);
            return dbd0;
        }

        public static byte[] word_2_bytes(UInt16 dbw0)//��word���Ϊ2��byte
        {
            byte[] bytes = new byte[2];
            bytes[0] = (byte)(dbw0 / 256);
            bytes[1] = (byte)(dbw0 % 256);
            return bytes;
        }

        public static byte[] dword_2_bytes(UInt32 dbd0)//��dword���Ϊ4��byte
        {
            byte[] bytes = new byte[4];
            bytes[0] = (byte)(dbd0 / 16777216);
            dbd0 = dbd0 % 16777216;
            bytes[1] = (byte)(dbd0 / 65536);
            dbd0 = dbd0 % 65536;
            bytes[2] = (byte)(dbd0 / 256);
            bytes[3] = (byte)(dbd0 % 256);
            return bytes;
        }

        public static UInt16[] dword_2_words(UInt32 dbd0)//��dword���Ϊ2��word
        {
            UInt16[] words = new UInt16[2];
            words[0] = (UInt16)(dbd0 / 65536);
            words[1] = (UInt16)(dbd0 % 65536);
            return words;
        }

        #endregion
    }
}
