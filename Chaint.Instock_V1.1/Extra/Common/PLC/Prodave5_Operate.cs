using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CTWH.Common.PLC
{

    public class Prodave5_Operate
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

        private Dictionary<ushort, bool> ConnectionsIsConnect;
        //上次的连接状态
        public Dictionary<ushort, bool> ConnectionsIsLastConnect;
       // private bool m_Connect = false;
        public bool IsConnect { get { return ConnectionsIsConnect[0]; } }

        /// <summary>
        ///		<remark>PLC默认构造函数</remark>   
        /// </summary>
        public Prodave5_Operate()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //

            ConnectionsIsConnect = new Dictionary<ushort, bool>(1);
            ConnectionsIsConnect.Add(0, false);

            ConnectionsIsLastConnect = new Dictionary<ushort, bool>(1);
            ConnectionsIsLastConnect.Add(0, false);
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
            Prodave5.d_field_write(dbno, dwno, amount, buffer);
        }


        /// <summary>
        /// 写入单个Byte,SmallInt 的写入
        /// </summary>
        /// <param name="dbno"></param>
        /// <param name="dwno"></param>
        /// <param name="Num"></param>
        public void PLC_WriteSmallInt(int dbno, int dwno, int b)
        {
            byte[] bnum = BufferOperate.SmallIntToByte(b);

            Prodave5.d_field_write(dbno, dwno, 1, bnum);
        }


        /// <summary>
        /// 写入单字节Int
        /// </summary>
        /// <param name="dbno"></param>
        /// <param name="dwno"></param>
        /// <param name="Num"></param>
        public void PLC_WriteInt(int dbno, int dwno, int Num)
        {
            byte[] bnum = BufferOperate.IntToByte(Num);

            Prodave5.d_field_write(dbno, dwno, 2, bnum);

        }

        /// <summary>
        /// 写入双字节DInt
        /// </summary>
        /// <param name="dbno"></param>
        /// <param name="dwno"></param>
        /// <param name="DNum"></param>
        public void PLC_WriteDInt(int dbno, int dwno, int DNum)
        {
            byte[] bnum = BufferOperate.DIntToByte(DNum);

            Prodave5.d_field_write(dbno, dwno, 4, bnum);


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
            int err = Prodave5.d_field_read(dbno, dwno, amount, buffer);
            if (err > 0)
            {
                ConnectionsIsConnect[0] = false;
                OnPLCStateChange(new PLCStateEventArgs(PLCInfoType.PLCRead, 0, ConnectionsIsConnect, ConnectionsIsLastConnect, false, "PLC读取失败"));
                
            }
            else
            {
                ConnectionsIsConnect[0] = true;
                OnPLCStateChange(new PLCStateEventArgs(PLCInfoType.PLCRead, 0, ConnectionsIsConnect, ConnectionsIsLastConnect, true, "PLC读取成功"));
               
            }

        }
        /// <summary>
        ///		<remark>PLC操作构造函数，用于连接PLC</remark>   
        /// </summary>
        ///     <param name="Address">MPI地址</param>
        ///     <param name="Rack">机架号</param>
        ///     <param name="Slot">槽号</param>
        public Prodave5_Operate(PLC.PLCAddress plcAddress)
        {
            Connect_PLC(plcAddress);
        }

        /// <summary>
        ///		<remark>连接PLC</remark>   
        /// </summary>
        ///     <param name="Address">MPI地址</param>
        ///     <param name="Rack">机架号</param>
        ///     <param name="Slot">槽号</param>
        public bool Connect_PLC(PLC.PLCAddress plcAddress)
        {
            if (ConnectionsIsConnect[0])
                return true;

            Prodave5.PLCInfo[] Cnn = new Prodave5.PLCInfo[1];
            Cnn[0].Addres = plcAddress.Adr[0];//(byte)Address;
            Cnn[0].Segment = 0;
            Cnn[0].Slot = plcAddress.SlotNr;//(byte)Slot;
            Cnn[0].Rack = plcAddress.RackNr;// (byte)Rack;
            if (Prodave5.Connect_PLC(1, Cnn) == 0)
            {
                ConnectionsIsConnect[0] = true;
                OnPLCStateChange(new PLCStateEventArgs(PLCInfoType.PLCConnect,   0,ConnectionsIsConnect,ConnectionsIsLastConnect, true,"PLC连接成功"));

               
                Prodave5.new_ss(0);

            }
            else
            {
                ConnectionsIsConnect[0] = false;
                OnPLCStateChange(new PLCStateEventArgs(PLCInfoType.PLCConnect, 0, ConnectionsIsConnect, ConnectionsIsLastConnect, false, "PLC连接失败"));
               
            }

            return ConnectionsIsConnect[0];
        }

        /// <summary>
        ///		<remark>重新连接PLC</remark>   
        /// </summary>
        ///     <param name="Address">MPI地址</param>
        ///     <param name="Rack">机架号</param>
        ///     <param name="Slot">槽号</param>
        public bool ReConnect_PLC(PLC.PLCAddress plcAddress)
        {
            if (ConnectionsIsConnect[0])
                return true;

            //重新连接之前 断开
            int ret = Prodave5.unload_tool();

            Prodave5.PLCInfo[] Cnn = new Prodave5.PLCInfo[1];
            Cnn[0].Addres = plcAddress.Adr[0]; //(byte)Address;
            Cnn[0].Segment = 0;
            Cnn[0].Slot = plcAddress.SlotNr;//(byte)Slot;
            Cnn[0].Rack = plcAddress.RackNr;// (byte)Rack;
            Prodave5.Connect_PLC(1, Cnn);
            System.Threading.Thread.Sleep(100);
            if (Prodave5.Connect_PLC(1, Cnn) == 0)
            {
                ConnectionsIsConnect[0] = true;
                OnPLCStateChange(new PLCStateEventArgs(PLCInfoType.PLCReConnect, 0, ConnectionsIsConnect, ConnectionsIsLastConnect, true, "PLC重新连接成功"));
                
                Prodave5.new_ss(0);

            }
            else
            {
                ConnectionsIsConnect[0] = false;
                OnPLCStateChange(new PLCStateEventArgs(PLCInfoType.PLCReConnect, 0, ConnectionsIsConnect, ConnectionsIsLastConnect, false, "PLC重新连接失败"));
               
            }

            return ConnectionsIsConnect[0];
        }
    
    }//Class Connect_PLC
}
