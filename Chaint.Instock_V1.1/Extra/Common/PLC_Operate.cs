using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
namespace Common
{
    /// <summary>
    /// PLC_Operate 的摘要说明。
    /// </summary>
    public class PLC_Operate
    {

        //连接断开事件
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
        ///		<remark>PLC默认构造函数</remark>   
        /// </summary>
        public PLC_Operate()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }


        /// <summary>
        /// 往PLC写入数据
        /// </summary>
        /// <param name="dbno">指定DB块号</param>
        /// <param name="dwno">指定写入的起始字节序号，=0表示DB0,=1表示DB1</param>
        /// <param name="amount">指定写入的对象有多少个字</param>
        /// <param name="buffer">buffer:写入值</param>
        public void PLC_WriteData(int dbno, int dwno, int amount, byte[] buffer)
        {
            PLCIO.d_field_write(dbno, dwno, amount, buffer);
        }


        /// <summary>
        /// 写入单个Byte,SmallInt 的写入
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
        /// 写入单字节Int
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
        /// 写入双字节DInt
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
        /// 读取PLC DB 数据
        /// </summary>
        /// <param name="dbno">DB 块号</param>
        /// <param name="dwno">读取数据的起始位置</param>
        /// <param name="amount">读取的字节数目</param>
        /// <param name="buffer">返回的读取值</param>
        public void PLC_ReadData(int dbno, int dwno, int amount, byte[] buffer)
        {
            int err = PLCIO.d_field_read(dbno, dwno, amount, buffer);
            if (err > 0)
            {
                OnPLCStateChange(new PLCStateEventArgs(false, "PLC读取失败"));
                m_Connect = false;
            }
            else
            {
                OnPLCStateChange(new PLCStateEventArgs(true, "PLC读取成功"));
                m_Connect = true;
            }

        }
        /// <summary>
        ///		<remark>PLC操作构造函数，用于连接PLC</remark>   
        /// </summary>
        ///     <param name="Address">MPI地址</param>
        ///     <param name="Rack">机架号</param>
        ///     <param name="Slot">槽号</param>
        public PLC_Operate(int Address, int Slot, int Rack)
        {
            Connect_PLC(Address, Slot, Rack);
        }

        /// <summary>
        ///		<remark>连接PLC</remark>   
        /// </summary>
        ///     <param name="Address">MPI地址</param>
        ///     <param name="Rack">机架号</param>
        ///     <param name="Slot">槽号</param>
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
                OnPLCStateChange(new PLCStateEventArgs(true, "PLC连接成功"));

                m_Connect = true;
                PLCIO.new_ss(0);

            }
            else
            {
                OnPLCStateChange(new PLCStateEventArgs(false, "PLC连接失败"));
                m_Connect = false;
            }

            return m_Connect;
        }

        /// <summary>
        ///		<remark>重新连接PLC</remark>   
        /// </summary>
        ///     <param name="Address">MPI地址</param>
        ///     <param name="Rack">机架号</param>
        ///     <param name="Slot">槽号</param>
        public bool ReConnect_PLC(int Address, int Slot, int Rack)
        {
            if (m_Connect)
                return true;

            //重新连接之前 断开
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
                OnPLCStateChange(new PLCStateEventArgs(true, "PLC重新连接成功"));
                m_Connect = true;
                PLCIO.new_ss(0);

            }
            else
            {
                OnPLCStateChange(new PLCStateEventArgs(false, "PLC重新连接失败"));
                m_Connect = false;
            }

            return m_Connect;
        }

        /// <summary>
        /// SmallInt 转化为byte数组
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
        /// Int 转化为byte数组
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
        /// DInt 转化为byte数组
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
        /// byte数组转化为Int
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
        /// byte数组转化为DInt
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
        /// byte数组转化为String
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
            return ((Buffer[Byteindex] >> (Bitindex)) & 1) == 1 ? true : false;   //测试结果 无需取反
            // return ((Buffer[Byteindex] >> (7 - Bitindex)) & 1) == 1 ? true : false; 
        }

        public static bool BitOfBuffer(Byte[] Buffer, double index)
        {
            string str = index.ToString("f1");
            return BitOfBuffer(Buffer, str);

        }

        /// <summary>
        /// 通过传入的字符串截取Buffer的bit  传入 如"5.3"
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="str">传入的字符串 例如"5.4"</param>
        /// <returns></returns>
        public static bool BitOfBuffer(Byte[] Buffer, string str)
        {
            string[] strs = str.Split('.');
            int Byteindex = Convert.ToInt32(strs[0]);
            Int16 Bitindex = Convert.ToInt16(strs[1]);   //测试结果 高位
            return BitOfBuffer(Buffer, Byteindex, Bitindex);
        }

        #region 其他的Byte 互转 String 的函数

        public static string ByteToString(byte[] InBytes)
        {
            string StringOut = "";
            foreach (byte InByte in InBytes)
            {
                StringOut += String.Format("{0:X2}", InByte);
            }
            return StringOut;
        }

        public static string ByteToString(byte[] InBytes, string Param) ///为了显示方便可能需要在每个字节后面补上一个空格等）
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



        //补00函数
        public static string Supply00(string sOldStr, int SupplyWay, string SupplyChar, long SupplyNum)
        {
            int i;
            string sTmp;

            sTmp = "";
            for (i = sOldStr.Length; i < SupplyNum; i++)
                sTmp = sTmp + SupplyChar;
            switch (SupplyWay)
            {
                case 0: //前补充
                    sTmp = sTmp + sOldStr;
                    break;
                case 1: //后补充
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
            // TODO: 在此处添加构造函数逻辑
            //
            //连接初始化
        }

        #region
        [DllImport("w95_s7m.dll")]
        public extern static int load_tool(byte nr, string device, byte[,] adr_table);
        #endregion

        #region
        //取消连接
        [DllImport("w95_s7m.dll")]
        public extern static int unload_tool();
        #endregion

        #region
        //激活一个连接
        [DllImport("w95_s7m.dll")]
        public extern static int new_ss(byte no);
        #endregion

        /// 
        /// 从DB中读取数据(Byte)
        /// 
        /// dbno:指定DB块号
        /// dwno:指定读取的起始字节序号，=0表示DB0,=1表示DB1
        /// amount:指定读取的对象有多少个字
        /// buffer:返回值
        /// 
        [DllImport("w95_s7m.dll")]
        public extern static int d_field_read(int dbno, int dwno, int amount, byte[] buffer);

        /// 
        /// 向DB中写入数据(Byte)
        /// 
        /// dbno:指定DB块号
        /// dwno:指定写入的起始字节序号，=0表示DB0,=1表示DB1
        /// amount:指定写入的对象有多少个字
        /// buffer:写入值
        /// 
        [DllImport("w95_s7m.dll")]
        public extern static int d_field_write(int dbno, int dwno, int amount, byte[] buffer);


        #region 定义与外部联系的结构变量
        /// 
        /// 定义MPI链接参数
        /// 
        public struct PLCInfo
        {
            public byte Addres;  // 定义CPU的MPI地址 一般为2
            public byte Segment; // 定义段地址 一般为0
            public byte Rack;    // 定义CPU的机架号 一般为2
            public byte Slot;    // 定义CPU的槽号 一般为0
        }
        #endregion

        #region 与动态库函数相对应的公开函数
        /// 建立连接，同一个连接只容许调用一次
        /// 连接号1-4
        /// 指定链接参数
        /// 返回10进制错误号，0表示没有错误
        public static int Connect_PLC(byte cnnNo, PLCInfo[] cnnInfo)
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
                btr[i, 2] = cnnInfo[i].Slot;
                btr[i, 3] = cnnInfo[i].Rack;
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
    }




    public class PLC6_Operate
    {

        //连接断开事件
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
        ///		<remark>PLC默认构造函数</remark>   
        /// </summary>
        public PLC6_Operate()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }


        /// <summary>
        /// 往PLC写入数据
        /// </summary>
        /// <param name="dbno">指定DB块号</param>
        /// <param name="dwno">指定写入的起始字节序号，=0表示DB0,=1表示DB1</param>
        /// <param name="amount">指定写入的对象有多少个字</param>
        /// <param name="buffer">buffer:写入值</param>
        public void PLC_WriteData(ushort dbno, ushort dwno, uint amount, byte[] buffer)
        {
            int ret = Prodave6.field_write_ex6(Prodave6.FieldType.D, dbno, dwno, amount, (ushort)buffer.Length, buffer);


        }


        /// <summary>
        /// 写入单个Byte,SmallInt 的写入
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
        /// 写入单字节Int
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
        /// 写入双字节DInt
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
        /// 读取PLC DB 数据
        /// </summary>
        /// <param name="dbno">DB 块号</param>
        /// <param name="dwno">读取数据的起始位置</param>
        /// <param name="amount">读取的字节数目</param>
        /// <param name="buffer">返回的读取值</param>
        public void PLC_ReadData(ushort dbno, ushort dwno, uint amount, byte[] buffer)
        {
            uint DataLen = 0;

            //int err = PLCIO.d_field_read(dbno, dwno, amount, buffer);
            int err = Prodave6.field_read_ex6(Prodave6.FieldType.D, dbno, dwno, amount, (uint)buffer.Length, buffer, ref DataLen);

            if (err > 0)
            {
                OnPLCStateChange(new PLCStateEventArgs(false, "PLC读取失败"));
                m_Connect = false;
            }
            else
            {
                OnPLCStateChange(new PLCStateEventArgs(true, "PLC读取成功"));
                m_Connect = true;
            }

        }
        /// <summary>
        ///		<remark>PLC操作构造函数，用于连接PLC</remark>   
        /// </summary>
        ///     <param name="ConNr">defualt 63(first connection),  connection number；(0 ... 63)；(max. 64 connections);</param>
        ///     <param name="Address">PLC IP地址,ex:192.168.1.10</param>
        ///     <param name="Rack">机架号</param>
        ///     <param name="Slot">槽号</param>
        //public PLC6_Operate(ushort ConNr, string Address, ushort Slot, ushort Rack)
        public PLC6_Operate(ushort ConNr, PLC.PLCAddress plcAddress)
        {
            Connect_PLC(ConNr, plcAddress);
        }

        /// <summary>
        ///		<remark>连接PLC</remark>   
        /// </summary>
        ///     <param name="ConNr">defualt 63(first connection),  connection number；(0 ... 63)；(max. 64 connections);</param>
        ///     <param name="Address">PLC IP地址,ex:192.168.1.10</param>
        ///     <param name="Rack">机架号</param>
        ///     <param name="Slot">槽号</param>
        //public bool Connect_PLC(ushort ConNr, string Address, ushort Slot, ushort Rack)
        public bool Connect_PLC(ushort ConNr, PLC.PLCAddress plcAddress)
        {
            if (m_Connect)
                return true;

           // string[] IPstrs = Address.Split('.');
            string AccessPoint = "S7ONLINE"; // Default access point——S7ONLINE
            Prodave6.CON_TABLE_TYPE ConTable;// Connection tablE
            int ConTableLen = System.Runtime.InteropServices.Marshal.SizeOf(typeof(Prodave6.CON_TABLE_TYPE));// Length of the connection tablE
            int RetValue;
            //ConTable.Adr=new byte[]{192,168,1,200,0,0};
            //ConTable.Adr = new byte[] { Convert.ToByte(IPstrs[0]), Convert.ToByte(IPstrs[1]), Convert.ToByte(IPstrs[2]), Convert.ToByte(IPstrs[3]), 0, 0 };
            ConTable.Adr = plcAddress.Adr;//new byte[] { Convert.ToByte(Address),0,0,0,0,0};
            ConTable.AdrType = plcAddress.AdrType;//1; // Type of address: MPI/PB (1), IP (2), MAC (3)
            ConTable.SlotNr = plcAddress.SlotNr;// (byte)Slot; // 插槽号
            ConTable.RackNr = plcAddress.RackNr;// (byte)Rack; // 机架号
            RetValue = Prodave6.LoadConnection_ex6(ConNr, AccessPoint, ConTableLen, ref ConTable);


            if (RetValue == 0)
            {
                //激活连接
                RetValue = Prodave6.SetActiveConnection_ex6(ConNr);

                if (RetValue == 0)
                {
                    m_Connect = true;
                    OnPLCStateChange(new PLCStateEventArgs(true, "PLC连接成功"));
                }
                else
                {
                    m_Connect = false;
                    //以下测试GetErrorMessage_ex6
                    int ErrorNr = RetValue; // Block boundary exceeded, correct the number
                    StringBuilder Buffer = new StringBuilder(300); // Transfer buffer for error text
                    UInt32 BufLen = (UInt32)Buffer.Capacity; // Buffer length　　　　 
                    Prodave6.GetErrorMessage_ex6(ErrorNr, BufLen, Buffer);
                    Utils.WriteTxtLog(Utils.FilePath_txtPLCLog, " 激活失败 error code :" + RetValue + " 激活失败 error msg :" + Buffer.ToString());

                }

            }
            else
            {
                OnPLCStateChange(new PLCStateEventArgs(false, "PLC连接失败"));
                m_Connect = false;
            }

            return m_Connect;
        }

        /// <summary>
        ///		<remark>重新连接PLC</remark>   
        /// </summary>
        ///     <param name="ConNr">defualt 63(first connection),  connection number；(0 ... 63)；(max. 64 connections);</param>
        ///     <param name="Address">PLC IP地址,ex:192.168.1.10</param>
        ///     <param name="Rack">机架号</param>
        ///     <param name="Slot">槽号</param>
        //public bool ReConnect_PLC(ushort ConNr, string Address, ushort Slot, ushort Rack)
        public bool ReConnect_PLC(ushort ConNr, PLC.PLCAddress plcAddress)
        {
            if (m_Connect)
                return true;

            //重新连接之前 断开
            //int ret = PLCIO.unload_tool();
            int RetValue = Prodave6.UnloadConnection_ex6(ConNr);
            System.Threading.Thread.Sleep(100);
            //PLCIO.PLCInfo[] Cnn = new PLCIO.PLCInfo[1];
            //Cnn[0].Addres = (byte)Address;
            //Cnn[0].Segment = 0;
            //Cnn[0].Slot = (byte)Slot;
            //Cnn[0].Rack = (byte)Rack;
            //PLCIO.Connect_PLC(1, Cnn);

            //未连接时断开的话是28693
            if (RetValue == 0 || RetValue == 28720 || RetValue == 28693)
            {//断开成功 
                //if (Connect_PLC(ConNr, Address, Slot, Rack))
                if (Connect_PLC(ConNr, plcAddress))   
                {
                    OnPLCStateChange(new PLCStateEventArgs(true, "PLC重新连接成功"));
                    m_Connect = true;
                }
                else
                {
                    OnPLCStateChange(new PLCStateEventArgs(false, "PLC重新连接失败"));
                    m_Connect = false;
                }
            }
            else
            {//断开失败
                OnPLCStateChange(new PLCStateEventArgs(false, "PLC重新连接失败:断开连接失败"));
                m_Connect = false;
            }

            return m_Connect;
        }

        /// <summary>
        /// SmallInt 转化为byte数组
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
        /// Int 转化为byte数组
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
        /// DInt 转化为byte数组
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
        /// byte数组转化为Int
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
        /// byte数组转化为DInt
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
        /// byte数组转化为String
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
        /// byte数组转化为普通String RollID
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
            return ((Buffer[Byteindex] >> (Bitindex)) & 1) == 1 ? true : false;   //测试结果 无需取反
            // return ((Buffer[Byteindex] >> (7 - Bitindex)) & 1) == 1 ? true : false; 
        }


        public static bool BitOfBuffer(Byte[] Buffer, double index)
        {
            string str = index.ToString("f1");
            return BitOfBuffer(Buffer, str);

        }

        /// <summary>
        /// 通过传入的字符串截取Buffer的bit  传入 如"5.3"
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="str">传入的字符串 例如"5.4"</param>
        /// <returns></returns>
        public static bool BitOfBuffer(Byte[] Buffer, string str)
        {
            string[] strs = str.Split('.');
            int Byteindex = Convert.ToInt32(strs[0]);
            Int16 Bitindex = Convert.ToInt16(strs[1]);   //测试结果 高位
            return BitOfBuffer(Buffer, Byteindex, Bitindex);
        }


        #region 其他的Byte 互转 String 的函数

        public static string ByteToString(byte[] InBytes)
        {
            string StringOut = "";
            foreach (byte InByte in InBytes)
            {
                StringOut += String.Format("{0:X2}", InByte);
            }
            return StringOut;
        }

        public static string ByteToString(byte[] InBytes, string Param) ///为了显示方便可能需要在每个字节后面补上一个空格等）
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



        //补00函数
        public static string Supply00(string sOldStr, int SupplyWay, string SupplyChar, long SupplyNum)
        {
            int i;
            string sTmp;

            sTmp = "";
            for (i = sOldStr.Length; i < SupplyNum; i++)
                sTmp = sTmp + SupplyChar;
            switch (SupplyWay)
            {
                case 0: //前补充
                    sTmp = sTmp + sOldStr;
                    break;
                case 1: //后补充
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
        #region 常值定义（用于极限值）
        public const int MAX_CONNECTIONS = 64; // 64 is default in PRODAVE
        public const int MAX_DEVNAME_LEN = 128;// e.g. "S7ONLINE"
        public const int MAX_BUFFERS = 64; // 64 for blk_read() and blk_write() 
        public const int MAX_BUFFER = 65536; // Transfer buffer for error text) 
        #endregion

        #region 结构体定义
        public struct CON_TABLE_TYPE//待连接plc地址属性表
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
        public enum DatType : byte//PLC数据类型
        {
            BYTE = 0x02,
            WORD = 0x04,
            DWORD = 0x06,
        }
        public enum FieldType : byte//PLC区域类型
        {
            //Value types as ASCII characters区域类型对应的ASCII字符
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

        #region PLC基本函数

        [DllImport("Prodave6.dll")]//连接PLC操作
        //参数：连接号（0-63）、常值"S7ONLINE"、待连接plc地址属性表长度（字节为单位，常值9）、待连接plc地址属性表
        public extern static int LoadConnection_ex6(int ConNr, string pAccessPoint, int ConTableLen, ref  CON_TABLE_TYPE pConTable);

        [DllImport("Prodave6.dll")]//断开PLC操作
        //参数：连接号（0-63）
        public extern static int UnloadConnection_ex6(UInt16 ConNr);

        [DllImport("Prodave6.dll")]//激活PLC连接操作
        //参数：连接号（0-63）
        public extern static int SetActiveConnection_ex6(UInt16 ConNr);

        [DllImport("Prodave6.dll")]//PLC db区读取操作
        //参数：data block号、要读取的数据类型、起始地址号、需要读取类型的数量、缓冲区长度（字节为单位）、缓冲区、缓冲区数据交互的长度
        public extern static int db_read_ex6(UInt16 BlkNr, DatType DType, UInt16 StartNr, ref UInt32 pAmount, UInt32 BufLen,
            byte[] pBuffer, ref UInt32 pDatLen);

        [DllImport("Prodave6.dll")]//PLC db区写入操作
        //参数：data block号、要写入的数据类型、起始地址号、需要写入类型的数量、缓冲区长度（字节为单位）、缓冲区
        public extern static int db_write_ex6(UInt16 BlkNr, DatType Type, UInt16 StartNr, ref UInt32 pAmount, UInt32 BufLen,
            byte[] pBuffer);

        [DllImport("Prodave6.dll")]//PLC 任意区读取操作
        //参数：要读取的区类型、data block号(DB区特有，默认为0)、起始地址号、需要读取类型的数量、
        //缓冲区长度（字节为单位）、缓冲区、缓冲区数据交互的长度
        public extern static int field_read_ex6(FieldType FType, UInt16 BlkNr, UInt16 StartNr, UInt32 pAmount, UInt32 BufLen,
            byte[] pBuffer, ref UInt32 pDatLen);

        [DllImport("Prodave6.dll")]//PLC 任意区写入操作
        //参数：要写入的区类型、data block号(DB区特有，默认为0)、起始地址号、需要写入类型的数量、
        //缓冲区长度（字节为单位）、缓冲区
        public extern static int field_write_ex6(FieldType FType, UInt16 BlkNr, UInt16 StartNr, UInt32 pAmount, UInt32 BufLen,
            byte[] pBuffer);

        [DllImport("Prodave6.dll")]//PLC M区某字节的某位读取操作
        //参数：M区字节号、位号、当前的值(0/1)
        public extern static int mb_bittest_ex6(UInt16 MbNr, UInt16 BitNr, ref int pValue);

        [DllImport("Prodave6.dll")]//PLC M区某字节的某位写入操作
        //参数：M区字节号、位号、要写入的值(0/1)
        public extern static int mb_setbit_ex6(UInt16 MbNr, UInt16 BitNr, byte Value);

        #endregion

        #region PLC200用数据传输函数

        [DllImport("Prodave6.dll")]//200系列PLC 任意区读取操作
        //参数：要读取的区类型、data block号(DB区特有，默认为0)、起始地址号、需要读取类型的数量、
        //缓冲区长度（字节为单位）、缓冲区、缓冲区数据交互的长度
        public extern static int as200_field_read_ex6(FieldType FType, UInt16 BlkNr, UInt16 StartNr, UInt32 pAmount, UInt32 BufLen,
            byte[] pBuffer, ref UInt32 pDatLen);

        [DllImport("Prodave6.dll")]//200系列PLC 任意区写入操作
        //参数：要写入的区类型、data block号(DB区特有，默认为0)、起始地址号、需要写入类型的数量、
        //缓冲区长度（字节为单位）、缓冲区
        public extern static int as200_field_write_ex6(FieldType FType, UInt16 BlkNr, UInt16 StartNr, UInt32 pAmount, UInt32 BufLen,
            byte[] pBuffer);

        [DllImport("Prodave6.dll")]//200系列PLC M区某字节的某位读取操作
        //参数：M区字节号、位号、当前的值(0/1)
        public extern static int as200_mb_bittest_ex6(UInt16 MbNr, UInt16 BitNr, ref int pValue);

        [DllImport("Prodave6.dll")]//200系列PLC M区某字节的某位写入操作
        //参数：M区字节号、位号、要写入的值(0/1)
        public extern static int as200_mb_setbit_ex6(UInt16 MbNr, UInt16 BitNr, byte Value);

        #endregion

        #region PLC数据转换函数

        [DllImport("Prodave6.dll")]//诊断错误信息操作
        //参数：错误代号、缓冲区大小（字节为单位）、缓冲区
        public extern static int GetErrorMessage_ex6(int ErrorNr, UInt32 BufLen, [MarshalAs(UnmanagedType.LPStr)] StringBuilder pBuffer);

        [DllImport("Prodave6.dll")]//S7浮点数转换成PC浮点数
        //参数：S7浮点数、PC浮点数
        public extern static int gp_2_float_ex6(UInt32 gp, ref float pieee);

        [DllImport("Prodave6.dll")]//PC浮点数转换成S7浮点数
        //参数：PC浮点数、S7浮点数
        public extern static int float_2_gp_ex6(float ieee, ref UInt32 pgp);

        [DllImport("Prodave6.dll")]//检测某字节的某位的值是0或1
        //参数：字节值、位号
        public extern static int testbit_ex6(byte Value, int BitNr);

        [DllImport("Prodave6.dll")]//检测某字节的byte值转换成int数组
        //参数：byte值、int数组(长度为8)
        public extern static void byte_2_bool_ex6(byte Value, int[] pBuffer);


        [DllImport("Prodave6.dll")]//检测某字节的int数组转换成byte值
        //参数：int数组(长度为8)
        public extern static byte bool_2_byte_ex6(int[] pBuffer);

        [DllImport("Prodave6.dll")]//交换数据的高低字节——16位数据
        //参数：待交换的数据
        public extern static UInt16 kf_2_integer_ex6(UInt16 wValue);//16位数据——WORD

        [DllImport("Prodave6.dll")]//交换数据的高低字节——32位数据
        //参数：待交换的数据
        public extern static UInt32 kf_2_long_ex6(UInt32 dwValue);//32位数据——DWORD

        [DllImport("Prodave6.dll")]//交换数据缓冲区的的高低字节区，例如pBuffer[0]与pBuffer[1]，pBuffer[2]与pBuffer[3]交换
        //参数：待交换的数据缓冲区，要交换的字节数，如Amount=pBuffer.Length，则交换全部缓冲
        public extern static void swab_buffer_ex6(byte[] pBuffer, UInt32 Amount);

        [DllImport("Prodave6.dll")]//复制数据缓冲区
        //参数：目的数据缓冲区，源数据缓冲区，要复制的数量（字节为单位）
        public extern static void copy_buffer_ex6(byte[] pTargetBuffer, byte[] pSourceBuffer, UInt32 Amount);

        [DllImport("Prodave6.dll")]//把二进制数组传换成BCD码的数组——16位数据
        //参数：要处理的数组，要处理的字节数，转换前是否先交换高低字节，转换后是否要交换高低字节
        //InBytechange为1则转换BCD码之前，先交换高低字节
        //OutBytechange为1则转换BCD码之后，再交换高低字节
        //如果InBytechange和OutBytechange都没有置1，则不发生高低位的交换
        //16位数据BCD码值的许可范围是：+999 —— -999
        public extern static void ushort_2_bcd_ex6(UInt16[] pwValues, UInt32 Amount, int InBytechange, int OutBytechange);//16位数据——WORD

        [DllImport("Prodave6.dll")]//把二进制数组传换成BCD码的数组——32位数据
        //参数：要处理的数组，要处理的字节数，转换前是否先交换高低字节，转换后是否要交换高低字节
        //InBytechange为1则转换BCD码之前，先交换高低字节
        //OutBytechange为1则转换BCD码之后，再交换高低字节
        //如果InBytechange和OutBytechange都没有置1，则不发生高低位的交换
        //32位数据BCD码值的许可范围是：+9 999 999 —— -9 999 999
        public extern static void ulong_2_bcd_ex6(UInt32[] pdwValues, UInt32 Amount, int InBytechange, int OutBytechange);//32位数据——DWORD

        [DllImport("Prodave6.dll")]//把BCD码的数组传换成二进制数组——16位数据
        //参数：要处理的数组，要处理的字节数，转换前是否先交换高低字节，转换后是否要交换高低字节
        //InBytechange为1则转换BCD码之前，先交换高低字节
        //OutBytechange为1则转换BCD码之后，再交换高低字节
        //如果InBytechange和OutBytechange都没有置1，则不发生高低位的交换
        //16位数据BCD码值的许可范围是：+999 —— -999
        public extern static void bcd_2_ushort_ex6(UInt16[] pwValues, UInt32 Amount, int InBytechange, int OutBytechange);//16位数据——WORD

        [DllImport("Prodave6.dll")]//把BCD码的数组传换成二进制数组——32位数据
        //参数：要处理的数组，要处理的字节数，转换前是否先交换高低字节，转换后是否要交换高低字节
        //InBytechange为1则转换BCD码之前，先交换高低字节
        //OutBytechange为1则转换BCD码之后，再交换高低字节
        //如果InBytechange和OutBytechange都没有置1，则不发生高低位的交换
        //32位数据BCD码值的许可范围是：+9 999 999 —— -9 999 999
        public extern static void bcd_2_ulong_ex6(UInt32[] pdwValues, UInt32 Amount, int InBytechange, int OutBytechange);//32位数据——DWORD

        [DllImport("Prodave6.dll")]//查看64个连接中哪些被占用，哪些已经建立
        //参数：传输缓冲的字节长度，64位长度的数组(0或1)
        public extern static void GetLoadedConnections_ex6(UInt32 BufLen, int[] pBuffer);

        #endregion

        #region 自定义辅助函数

        public static UInt16 bytes_2_word(byte dbb0, byte dbb1)//将高低2个byte转换成1个word
        {
            UInt16 dbw0;
            dbw0 = (UInt16)(dbb0 * 256 + dbb1);
            return dbw0;
        }

        public static UInt32 bytes_2_dword(byte dbb0, byte dbb1, byte dbb2, byte dbb3)//将高低4个byte转换成1个dword
        {
            UInt32 dbd0;
            dbd0 = (UInt32)(dbb0 * 16777216 + dbb1 * 65536 + dbb2 * 256 + dbb3);
            return dbd0;
        }

        public static UInt32 words_2_dword(UInt16 dbw0, UInt16 dbw2)//将高低2个word转换成1个dword
        {
            UInt32 dbd0;
            dbd0 = (UInt32)(dbw0 * 65536 + dbw2);
            return dbd0;
        }

        public static byte[] word_2_bytes(UInt16 dbw0)//将word拆分为2个byte
        {
            byte[] bytes = new byte[2];
            bytes[0] = (byte)(dbw0 / 256);
            bytes[1] = (byte)(dbw0 % 256);
            return bytes;
        }

        public static byte[] dword_2_bytes(UInt32 dbd0)//将dword拆分为4个byte
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

        public static UInt16[] dword_2_words(UInt32 dbd0)//将dword拆分为2个word
        {
            UInt16[] words = new UInt16[2];
            words[0] = (UInt16)(dbd0 / 65536);
            words[1] = (UInt16)(dbd0 % 65536);
            return words;
        }

        #endregion
    }
}
