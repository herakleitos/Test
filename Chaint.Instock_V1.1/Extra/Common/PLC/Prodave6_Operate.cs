using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CTWH.Common.PLC
{
    public class Prodave6_Operate
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



        //private bool m_Connect = false;
        //public bool IsConnect { get { return m_Connect; } }


        public Dictionary<ushort, bool> ConnectionsIsConnect;

        //上次的连接状态
        public Dictionary<ushort, bool> ConnectionsIsLastConnect;

        public ulong PLCReadCount = 0;
        public ulong PLCReadErrorCount = 0;
        public ulong PLCWriteCount = 0;
        public ulong PLCWriteErrorCount = 0;
        public ulong PLCConncetCount = 0;
        public ulong PLCConnectErrorCount = 0;


        /// <summary>
        ///		<remark>PLC默认构造函数</remark>   
        /// </summary>
        public Prodave6_Operate()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
            ConnectionsIsConnect = new Dictionary<ushort, bool>(64);
            for (ushort i = 0; i < 64; i++)
            {
                ConnectionsIsConnect.Add(i, false);
            }

            ConnectionsIsLastConnect = new Dictionary<ushort, bool>(64);
            for (ushort i = 0; i < 64; i++)
            {
                ConnectionsIsLastConnect.Add(i, false);
            }


        }


        /// <summary>
        /// 往PLC写入数据
        /// </summary>
        /// <param name="dbno">指定DB块号</param>
        /// <param name="dwno">指定写入的起始字节序号，=0表示DB0,=1表示DB1</param>
        /// <param name="amount">指定写入的对象有多少个字</param>
        /// <param name="buffer">buffer:写入值</param>
        public bool PLC_WriteData(ushort ConNr, ushort dbno, ushort dwno, uint amount, byte[] buffer)
        {
            bool ret = false;
            //激活连接
            SetActiveConnection(ConNr);
            int err = Prodave6.field_write_ex6(Prodave6.FieldType.D, dbno, dwno, amount, (ushort)buffer.Length, buffer);

            if (err == 0)
            {
                ConnectionsIsConnect[ConNr] = true;
                ret = true;
                //OnPLCStateChange(new PLCStateEventArgs(PLCInfoType.PLCWrite, ConNr, ConnectionsIsConnect, ret, String.Format("PLC_WriteData({0},{1},{2},{3},buffer)写入成功{4}/{5}", ConNr, dbno, dwno, amount, PLCWriteErrorCount, PLCWriteCount)));

            }
            else
            {
                ConnectionsIsConnect[ConNr] = false;
                ret = false;
                //OnPLCStateChange(new PLCStateEventArgs(PLCInfoType.PLCWrite, ConNr, ConnectionsIsConnect, ret, String.Format("PLC_WriteData({0},{1},{2},{3},buffer)写入失败{4}/{5}", ConNr, dbno, dwno, amount, PLCWriteErrorCount, PLCWriteCount)));
                PLCWriteErrorCount++;
            }
            PLCWriteCount++;
            OnPLCStateChange(new PLCStateEventArgs(PLCInfoType.PLCWrite, ConNr, ConnectionsIsConnect, ConnectionsIsLastConnect, ret, String.Format("PLC_WriteData({0},{1},{2},{3},buffer) write "+(ret?"OK":"Error")+"{4}/{5}", ConNr, dbno, dwno, amount, PLCWriteErrorCount, PLCWriteCount)));
            ConnectionsIsLastConnect[ConNr] = ret;
            return ret;
        }


        /// <summary>
        /// 写入单个Byte,SmallInt 的写入
        /// </summary>
        /// <param name="dbno"></param>
        /// <param name="dwno"></param>
        /// <param name="Num"></param>
        public bool PLC_WriteSmallInt(ushort ConNr, ushort dbno, ushort dwno, int b)
        {
            byte[] bnum = BufferOperate.SmallIntToByte(b);
            return PLC_WriteData(ConNr, dbno, dwno, 1, bnum);

            ////激活连接
            //SetActiveConnection(ConNr);
            //Prodave6.field_write_ex6(Prodave6.FieldType.D, dbno, dwno, 1, 1, bnum);
        }


        /// <summary>
        /// 写入单字节Int
        /// </summary>
        /// <param name="dbno"></param>
        /// <param name="dwno"></param>
        /// <param name="Num"></param>
        public bool PLC_WriteInt(ushort ConNr, ushort dbno, ushort dwno, int Num)
        {
            byte[] bnum = BufferOperate.IntToByte(Num);

            return PLC_WriteData(ConNr, dbno, dwno, 2, bnum);
            ////激活连接
            //SetActiveConnection(ConNr);
            //int ret = Prodave6.field_write_ex6(Prodave6.FieldType.D, dbno, dwno, 2, 2, bnum);
            //return ret == 0;
        }
        //public bool PLC_WriteInt(ushort dbno, ushort dwno, int Num)
        //{
        //    return PLC_WriteInt(63, dbno, dwno, Num);
        //}

        /// <summary>
        /// 写入双字节DInt
        /// </summary>
        /// <param name="dbno"></param>
        /// <param name="dwno"></param>
        /// <param name="DNum"></param>
        public bool PLC_WriteDInt(ushort ConNr, ushort dbno, ushort dwno, int DNum)
        {
            byte[] bnum = BufferOperate.DIntToByte(DNum);

            return PLC_WriteData(ConNr, dbno, dwno, 4, bnum);

            ////激活连接
            //SetActiveConnection(ConNr);
            //int ret = Prodave6.field_write_ex6(Prodave6.FieldType.D, dbno, dwno, 4, 4, bnum);
            //return ret == 0;

        }
        //public bool PLC_WriteDInt(ushort dbno, ushort dwno, int DNum)
        //{
        //    return PLC_WriteDInt(63, dbno, dwno, DNum);
        //}

        /// <summary>
        /// 读取PLC DB 数据
        /// </summary>
        /// <param name="dbno">DB 块号</param>
        /// <param name="dwno">读取数据的起始位置</param>
        /// <param name="amount">读取的字节数目</param>
        /// <param name="buffer">返回的读取值</param>
        public bool PLC_ReadData(ushort ConNr, ushort dbno, ushort dwno, uint amount, byte[] buffer)
        {
            bool ret = false;
            uint DataLen = 0;

            //激活连接
            SetActiveConnection(ConNr);

            //int err = Prodave6.field_read_ex6(Prodave6.FieldType.D, dbno, dwno, amount, (uint)buffer.Length, buffer, ref DataLen);
            int err = Prodave6.db_read_ex6(dbno, Prodave6.DataType.BYTE, dwno, ref amount, (uint)buffer.Length, buffer, ref DataLen);

            if (err > 0)
            {
                ConnectionsIsConnect[ConNr] = false;
                ret = false;
                //OnPLCStateChange(new PLCStateEventArgs(PLCInfoType.PLCRead, ConNr, ConnectionsIsConnect, ret, String.Format("PLC_ReadData({0},{1},{2},{3},buffer)读取失败{4}/{5}", ConNr, dbno, dwno, amount, PLCReadErrorCount, PLCReadCount)));

                PLCReadErrorCount++;
            }
            else
            {
                ConnectionsIsConnect[ConNr] = true;
                ret = true;
                //OnPLCStateChange(new PLCStateEventArgs(PLCInfoType.PLCRead, ConNr, ConnectionsIsConnect, ret, String.Format("PLC_ReadData({0},{1},{2},{3},buffer)读取成功", ConNr, dbno, dwno, amount)));

            }
            PLCReadCount++;
            OnPLCStateChange(new PLCStateEventArgs(PLCInfoType.PLCRead, ConNr, ConnectionsIsConnect, ConnectionsIsLastConnect, ret, String.Format("PLC_ReadData({0},{1},{2},{3},buffer) Read " + (ret ? "OK" : "Error") + "{4}/{5}", ConNr, dbno, dwno, amount, PLCReadErrorCount, PLCReadCount)));
            ConnectionsIsLastConnect[ConNr] = ret;
            return ret;

        }
        /// <summary>
        ///		<remark>PLC操作构造函数，用于连接PLC</remark>   
        /// </summary>
        ///     <param name="ConNr">defualt 63(first connection),  connection number；(0 ... 63)；(max. 64 connections);</param>
        ///     <param name="Address">PLC IP地址,ex:192.168.1.10</param>
        ///     <param name="Rack">机架号</param>
        ///     <param name="Slot">槽号</param>
        public Prodave6_Operate(ushort ConNr, PLC.PLCAddress plcAddress)
        {
            Connect_PLC(ConNr, plcAddress);
        }

        /// <summary>
        ///		<remark>连接PLC</remark>   
        /// </summary>
        ///     <param name="ConNr">defualt 63(first connection),  connection number；(0 ... 63)；(max. 64 connections);</param>
        ///     <param name="plcAddress">PLC IP地址,ex:192.168.1.10</param> 
        public bool Connect_PLC(ushort ConNr, PLC.PLCAddress plcAddress)
        {
            if (ConnectionsIsConnect[ConNr])
                return true;

            bool ret = false;
            if (PLCConncetCount == 0)
            {
                //初次连接
            }
            else
            {
                //重新连接，先要断开连接
                if (!UnloadPLCConnection(ConNr))//断开失败，则跳出
                    return ret;
            }

            // string[] IPstrs = Address.Split('.');
            string AccessPoint = "S7ONLINE"; // Default access point——S7ONLINE
            Prodave6.CON_TABLE_TYPE ConTable;// Connection tablE
            int ConTableLen = System.Runtime.InteropServices.Marshal.SizeOf(typeof(Prodave6.CON_TABLE_TYPE));// Length of the connection tablE
            int RetValue;
            //ConTable.Adr=new byte[]{192,168,1,200,0,0};
            //ConTable.Adr = new byte[] { Convert.ToByte(IPstrs[0]), Convert.ToByte(IPstrs[1]), Convert.ToByte(IPstrs[2]), Convert.ToByte(IPstrs[3]), 0, 0 };
            ConTable.Adr = plcAddress.Adr;//new byte[] { Convert.ToByte(Address), 0, 0, 0, 0, 0 };
            ConTable.AdrType = plcAddress.AdrType;//1; // Type of address: MPI/PB (1), IP (2), MAC (3)
            ConTable.SlotNr = plcAddress.SlotNr;// (byte)Slot; // 插槽号
            ConTable.RackNr = plcAddress.RackNr;// (byte)Rack; // 机架号
            RetValue = Prodave6.LoadConnection_ex6(ConNr, AccessPoint, ConTableLen, ref ConTable);

            if (RetValue == 0)
            {
                ConnectionsIsConnect[ConNr] = true;
                ret = true;
                //OnPLCStateChange(new PLCStateEventArgs(PLCInfoType.PLCConnect, ConNr, ConnectionsIsConnect, ret, String.Format("Connect_PLC({0},{1})连接成功", ConNr, plcAddress.GetParameterString())));
                SetActiveConnection(ConNr);

            }
            else
            {
                ConnectionsIsConnect[ConNr] = false;
                ret = false;
                //OnPLCStateChange(new PLCStateEventArgs(PLCInfoType.PLCConnect, ConNr, ConnectionsIsConnect, ret, String.Format("Connect_PLC({0},{1})连接失败{2}/{3}", ConNr, plcAddress.GetParameterString(), PLCConnectErrorCount, PLCConncetCount)));

                PLCConnectErrorCount++;
            }
            PLCConncetCount++;
            OnPLCStateChange(new PLCStateEventArgs(PLCInfoType.PLCConnect, ConNr, ConnectionsIsConnect, ConnectionsIsLastConnect, ret, String.Format("Connect_PLC({0},{1}) connect " + (ret ? "OK" : "Error") + "{2}/{3}", ConNr, plcAddress.GetParameterString(), PLCConnectErrorCount, PLCConncetCount)));
            ConnectionsIsLastConnect[ConNr] = ret;
            return ret;
        }


        private ushort CurrentConNr = 63;
        public bool SetActiveConnection(ushort ConNr)
        {
            //强行设置
            if (CurrentConNr == ConNr)
                return true;

            bool ret = false;
            //激活连接
            int RetValue = Prodave6.SetActiveConnection_ex6(ConNr);
            if (RetValue == 0)
            {
                //保存当前连接
                CurrentConNr = ConNr;
                ret = true;
                ConnectionsIsConnect[ConNr] = true;
                OnPLCStateChange(new PLCStateEventArgs(PLCInfoType.PLCSetActive, ConNr, ConnectionsIsConnect, ConnectionsIsLastConnect, ret, "SetActiveConnection(" + ConNr + ")PLC set OK"));
                ConnectionsIsLastConnect[ConNr] = ret;            
            }
            else
            {
                ConnectionsIsConnect[ConNr] = false;
                //以下测试GetErrorMessage_ex6
                int ErrorNr = RetValue; // Block boundary exceeded, correct the number
                StringBuilder Buffer = new StringBuilder(300); // Transfer buffer for error text
                UInt32 BufLen = (UInt32)Buffer.Capacity; // Buffer length　　　　 
                Prodave6.GetErrorMessage_ex6(ErrorNr, BufLen, Buffer);
                ret = false;
                OnPLCStateChange(new PLCStateEventArgs(PLCInfoType.PLCSetActive, ConNr, ConnectionsIsConnect, ConnectionsIsLastConnect, ret, "SetActiveConnection(" + ConNr + ")PLC set Error," + " error code :" + RetValue + " error msg :" + Buffer.ToString()));
                //Utils.WriteTxtLog(Utils.FilePath_txtPLCLog, " 激活失败 error code :" + RetValue + " 激活失败 error msg :" + Buffer.ToString());
                ConnectionsIsLastConnect[ConNr] = ret;
            }
            return ConnectionsIsConnect[ConNr];
        }


        //public bool ReConnect_PLC(ushort ConNr, PLC.PLCAddress plcAddress)
        //{
        //    if (ConnectionsIsConnect[ConNr])
        //        return true;

        //    //重新连接之前 断开
        //    //int ret = PLCIO.unload_tool();
        //    int RetValue = Prodave6.UnloadConnection_ex6(ConNr);
        //    System.Threading.Thread.Sleep(100);
        //    //PLCIO.PLCInfo[] Cnn = new PLCIO.PLCInfo[1];
        //    //Cnn[0].Addres = (byte)Address;
        //    //Cnn[0].Segment = 0;
        //    //Cnn[0].Slot = (byte)Slot;
        //    //Cnn[0].Rack = (byte)Rack;
        //    //PLCIO.Connect_PLC(1, Cnn);

        //    //未连接时断开的话是28693
        //    if (RetValue == 0 || RetValue == 28720 || RetValue == 28693)
        //    {//断开成功 
        //        if (Connect_PLC(ConNr, plcAddress))
        //        {
        //            ConnectionsIsConnect[ConNr] = true;
        //            OnPLCStateChange(new PLCStateEventArgs(PLCInfoType.PLCReConnect,ConNr, ConnectionsIsConnect, "PLC重新连接成功"));

        //        }
        //        else
        //        {
        //            ConnectionsIsConnect[ConNr] = false;
        //            OnPLCStateChange(new PLCStateEventArgs(PLCInfoType.PLCReConnect,ConNr, ConnectionsIsConnect, "PLC重新连接失败"));

        //        }
        //    }
        //    else
        //    {//断开失败
        //        ConnectionsIsConnect[ConNr] = false;
        //        OnPLCStateChange(new PLCStateEventArgs(PLCInfoType.PLCReConnect,ConNr, ConnectionsIsConnect, "PLC重新连接失败:断开连接失败"));

        //    }
        //    return ConnectionsIsConnect[ConNr];
        //}

        public bool UnloadPLCConnection(ushort ConNr)
        {
            int RetValue = Prodave6.UnloadConnection_ex6(ConNr);
            System.Threading.Thread.Sleep(100);

            bool ret = false;
            //未连接时断开的话是28693
            if (RetValue == 0 || RetValue == 28720 || RetValue == 28693)
            {//断开成功 
                ConnectionsIsConnect[ConNr] = false;
                ret = true;
                //OnPLCStateChange(new PLCStateEventArgs(PLCInfoType.PLCUnloadConnection, ConNr, ConnectionsIsConnect, ret, "UnloadPLCConnection(" + ConNr + ")断开成功"));

            }
            else
            {//断开失败
                ret = false;
                //OnPLCStateChange(new PLCStateEventArgs(PLCInfoType.PLCUnloadConnection, ConNr, ConnectionsIsConnect, ret, "UnloadPLCConnection(" + ConNr + ")断开失败"));

            }
            OnPLCStateChange(new PLCStateEventArgs(PLCInfoType.PLCUnloadConnection, ConNr, ConnectionsIsConnect, ConnectionsIsLastConnect, ret, "UnloadPLCConnection(" + ConNr + ") disconnect" + (ret ? "OK" : "Error")));
            ConnectionsIsLastConnect[ConNr] = ret;
            
            return ret;
        }

    }//Class Connect_PLC
}
