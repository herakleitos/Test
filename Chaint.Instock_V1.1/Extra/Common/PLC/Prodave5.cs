using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace CTWH.Common.PLC
{
    public class Prodave5
    {
        public Prodave5()
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
}
