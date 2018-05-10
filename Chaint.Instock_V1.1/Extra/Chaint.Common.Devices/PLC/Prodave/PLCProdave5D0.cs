using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

/*-----------------------------------------------------------------------------------
 * 作者: Automation&IT Dept. 
 * 
 * 创建时间: 2014-07-03
 * 
 * 功能描述: 
 *      PLC Prodave5.0 操作类
 * 
 ------------------------------------------------------------------------------------*/
namespace Chaint.Common.Devices.PLC
{
    public class PLCProdave5D0
    {
        /// 
        /// 定义MPI链接参数
        /// 
        protected struct PLCAddrType
        {
            public byte Addres;  // 定义CPU的MPI地址 一般为2
            public byte Segment; // 定义段地址 一般为0
            public byte Rack;    // 定义CPU的机架号 一般为2
            public byte Slot;    // 定义CPU的槽号 一般为0
        }


        //读取或写入的PLC地址定义
        protected struct PLCAddress
        {
            public int dbno; //定义地址db块(如:200)
            public int dwno; //待读取或写入某一个db块下的数据首地址 
            public int amount;//写入或读取的字节个数(如2,4)
        }

        [DllImport("w95_s7.dll")]
        protected extern static int load_tool(byte nr, string device, byte[,] adr_table);

        //激活一个连接
        [DllImport("w95_s7.dll")]
        protected extern static int new_ss(byte no);

        //取消连接
        [DllImport("w95_s7.dll")]
        protected extern static int unload_tool();


        [DllImport("w95_s7.dll")]
        protected extern static int d_field_read(int dbno, int dwno, int amount, byte[] buffer);

        /// 
        /// 向DB中写入数据(Byte)
        /// 
        /// dbno:指定DB块号
        /// dwno:指定写入的起始字节序号，=0表示DB0,=1表示DB1
        /// amount:指定写入的对象有多少个字
        /// buffer:写入值
        /// 
        [DllImport("w95_s7.dll")]
        protected extern static int d_field_write(int dbno, int dwno, int amount, byte[] buffer);

    }
}
