using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Collections;
using CTSocket;
using System.Net;
using DataModel;
using CTWH.Common.PLC;
namespace CTWH.Common
{
   
    public  class PubFunction
    {
        public PubFunction()
        {            
        }
        //public PLC_Operate plc = new PLC_Operate();
        public Prodave6_Operate plc = new Prodave6_Operate();

        public bool RefreashDB(ref byte[] buffer)
        {
            buffer = new byte[Utils.PLCReadCount];
            if (!plc.ConnectionsIsConnect[63])   //读取失败则连接
            {
                //尝试连接  //请求尝试连接
            
               // plc.ReConnect_PLC(Utils.PLCAddress, Utils.PLCSlot, Utils.PLCRack);
                plc.Connect_PLC(63, Utils.PLCAddress);
               
                //重新连接后 等待几秒 防止错误
                System.Threading.Thread.Sleep(3000);

                //尝试继续读取。。。
                //plc.PLC_ReadData(Utils.PLCDBNO, Utils.PLCReadBegin, Utils.PLCReadCount, buffer);//Utils.ReadBuffer);  
                plc.PLC_ReadData(63,  (ushort)Utils.PLCDBNO, (ushort)Utils.PLCReadBegin, (uint)Utils.PLCReadCount, buffer);//Utils.ReadBuffer);  

                return false;
            }
            else//读取信息  读取失败则设置 连接失败 重连
            {
                // Utils.ReadBuffer = new Byte[ReadAount]; 

                //plc.PLC_ReadData(Utils.PLCDBNO, Utils.PLCReadBegin, Utils.PLCReadCount, buffer); //Utils.ReadBuffer);  
                plc.PLC_ReadData(63,(ushort)Utils.PLCDBNO, (ushort)Utils.PLCReadBegin, (uint)Utils.PLCReadCount, buffer);//Utils.ReadBuffer);  

                return true;
            }
        } 

        public bool RefreashDB(ref  DefineDS.PLCBufferDataTable PLCBufferTB)
        {
               
           // buffer = new byte[Utils.PLCReadCount];
            if (!plc.ConnectionsIsConnect[63])   //读取失败则连接
            {
                //尝试连接  //请求尝试连接
              
                //plc.ReConnect_PLC(Utils.PLCAddress, Utils.PLCSlot, Utils.PLCRack);
                plc.Connect_PLC(63, Utils.PLCAddress);
                //保存日志
             
                //重新连接后 等待几秒 防止错误
                System.Threading.Thread.Sleep(3000);
                //尝试继续读取。。。               

                foreach (DefineDS.PLCBufferRow row in PLCBufferTB.Rows)
                {
                    row.ReadBuffer = new byte[row.DBReadCount];

                    //plc.PLC_ReadData(row.DBNO, row.DBReadBegin, row.DBReadCount, row.ReadBuffer);
                }


                return false;
            }
            else//读取信息  读取失败则设置 连接失败 重连
            {
                // Utils.ReadBuffer = new Byte[ReadAount]; 
                //plc.PLC_ReadData(Utils.PLCDBNO, Utils.PLCReadBegin, Utils.PLCReadCount, buffer); //Utils.ReadBuffer);  
                foreach (DefineDS.PLCBufferRow row in PLCBufferTB.Rows)
                {
                    row.ReadBuffer = new byte[row.DBReadCount];

                    //plc.PLC_ReadData(row.DBNO, row.DBReadBegin, row.DBReadCount, row.ReadBuffer);
                }
             
                //Utils.PLCConnect = true;
                return true;
            }
        }

        public bool RefreashDB(ushort ConNr, PLCAddress plcAdress,ref byte[] buffer, ushort DBNO, ushort ReadBegin, uint ReadCount)
        {
            buffer = new byte[ReadCount];
            if (!plc.ConnectionsIsConnect[ConNr])   //读取失败则连接
            {
                //尝试连接  //请求尝试连接 
                plc.Connect_PLC(ConNr, plcAdress);

                //重新连接后 等待几秒 防止错误
                System.Threading.Thread.Sleep(3000);

                //尝试继续读取。。。
                plc.PLC_ReadData(ConNr,DBNO, ReadBegin, ReadCount, buffer);//Utils.ReadBuffer);  
                return false;
            }
            else//读取信息  读取失败则设置 连接失败 重连
            {

                plc.PLC_ReadData(ConNr,DBNO, ReadBegin, ReadCount, buffer);//Utils.ReadBuffer);  
                return true;
            }
        }
        
        public SocketClientSync CTclient = new SocketClientSync(null);
        //Socket发送数据
        public void RequestRollData(string OnlyID)
        {
            //NNNN|0000000000|AA|RollID| >
            string data = Utils.ToMESJoinData("0000000000", "AA|" + OnlyID + "|");

            if (CTclient.Connected)
            {
                CTclient.Write(Utils.SocketParaChaintServer.SocketEncoding.GetBytes(data));
            }
            else
            {
                CTclient.RemoteEndPoint = new System.Net.IPEndPoint(System.Net.IPAddress.Parse(Utils.SocketParaChaintServer.IPAddress), Utils.SocketParaChaintServer.SocketPort);
              
                CTclient.DelimiterType = DelimiterType.dtMessageTailExcludeOnReceive;
                CTclient.Delimiter = Utils.SocketParaChaintServer.SocketEncoding.GetBytes(new char[] { '>' });

                CTclient.EncryptType = EncryptType.etNone;
                CTclient.CompressionType = CompressionType.ctNone;

                CTclient.SocketBufferSize = 1024;
                CTclient.MessageBufferSize = 2048;
                CTclient.Connect();
                if (CTclient.Connected)
                {
                    CTclient.Write(Utils.SocketParaPLCReadServer.SocketEncoding.GetBytes(data));
                }
                else
                {

                }
            }
        }
        
        [StructLayout(LayoutKind.Sequential)]
        public struct SystemTime
        {
            public ushort wYear;
            public ushort wMonth;
            public ushort wDayOfWeek;
            public ushort wDay;
            public ushort wHour;
            public ushort wMinute;
            public ushort wSecond;
            public ushort wMiliseconds;
        }


        [DllImport("Kernel32.dll")]
        public static extern bool SetSystemTime(ref SystemTime sysTime);
        [DllImport("Kernel32.dll")]
        public static extern bool SetLocalTime(ref SystemTime sysTime);
        [DllImport("Kernel32.dll")]
        public static extern void GetSystemTime(ref SystemTime sysTime);
        [DllImport("Kernel32.dll")]
        public static extern void GetLocalTime(ref SystemTime sysTime);


        public bool SetLocalTime(DateTime time)
        {
            bool flag = false;
            SystemTime sysTime = new SystemTime();

            sysTime.wYear = Convert.ToUInt16(time.Year);//Convert.ToUInt16(SysTime.Substring(0,4));
            sysTime.wMonth = Convert.ToUInt16(time.Month);// Convert.ToUInt16(SysTime.Substring(4, 2));
            sysTime.wDay = Convert.ToUInt16(time.Day);// Convert.ToUInt16(SysTime.Substring(6, 2));
            sysTime.wHour = Convert.ToUInt16(time.Hour);// Convert.ToUInt16(SysTime.Substring(8, 2));
            sysTime.wMinute = Convert.ToUInt16(time.Minute);// Convert.ToUInt16(SysTime.Substring(10, 2));
            sysTime.wSecond = Convert.ToUInt16(time.Second);// Convert.ToUInt16(SysTime.Substring(12, 2));
            //注意：
            //结构体的wDayOfWeek属性一般不用赋值，函数会自动计算，写了如果不对应反而会出错
            //wMiliseconds属性默认值为一，可以赋值
            try
            {
                flag = SetLocalTime(ref sysTime);
            }
            //由于不是C#本身的函数，很多异常无法捕获
            //函数执行成功则返回true，函数执行失败返回false
            //经常不返回异常，不提示错误，但是函数返回false，给查找错误带来了一定的困难
            catch// (Exception ex1)
            {
                //MessageBox.Show("SetLocalTime函数执行异常" + ex1.Message);
                return false;
            }
            return flag;
        }
    } 
}
