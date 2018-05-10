using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

/*-----------------------------------------------------------------------------------
 * ����: Automation&IT Dept. 
 * 
 * ����ʱ��: 2014-07-03
 * 
 * ��������: 
 *      PLC Prodave5.0 ������
 * 
 ------------------------------------------------------------------------------------*/
namespace Chaint.Common.Devices.PLC
{
    public class PLCProdave5D0
    {
        /// 
        /// ����MPI���Ӳ���
        /// 
        protected struct PLCAddrType
        {
            public byte Addres;  // ����CPU��MPI��ַ һ��Ϊ2
            public byte Segment; // ����ε�ַ һ��Ϊ0
            public byte Rack;    // ����CPU�Ļ��ܺ� һ��Ϊ2
            public byte Slot;    // ����CPU�Ĳۺ� һ��Ϊ0
        }


        //��ȡ��д���PLC��ַ����
        protected struct PLCAddress
        {
            public int dbno; //�����ַdb��(��:200)
            public int dwno; //����ȡ��д��ĳһ��db���µ������׵�ַ 
            public int amount;//д����ȡ���ֽڸ���(��2,4)
        }

        [DllImport("w95_s7.dll")]
        protected extern static int load_tool(byte nr, string device, byte[,] adr_table);

        //����һ������
        [DllImport("w95_s7.dll")]
        protected extern static int new_ss(byte no);

        //ȡ������
        [DllImport("w95_s7.dll")]
        protected extern static int unload_tool();


        [DllImport("w95_s7.dll")]
        protected extern static int d_field_read(int dbno, int dwno, int amount, byte[] buffer);

        /// 
        /// ��DB��д������(Byte)
        /// 
        /// dbno:ָ��DB���
        /// dwno:ָ��д�����ʼ�ֽ���ţ�=0��ʾDB0,=1��ʾDB1
        /// amount:ָ��д��Ķ����ж��ٸ���
        /// buffer:д��ֵ
        /// 
        [DllImport("w95_s7.dll")]
        protected extern static int d_field_write(int dbno, int dwno, int amount, byte[] buffer);

    }
}
