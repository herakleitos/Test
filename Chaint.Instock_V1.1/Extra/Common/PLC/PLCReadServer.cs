using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CTWH.Common.PLC
{
    class PLCReadServer
    {
    }

    public struct PLCReadServerParameter//PLC读取信息
    {
        public string[] IPAddress;
        public uint Port;
        public ushort PLCNO;
        public ushort DBNO;
        public ushort ReadBegin;
        public ushort ReadCount;

        public PLCReadServerParameter(string[] _IPAddress,uint _Port, ushort _PLCNO, ushort _DBNO, ushort _ReadBegin, ushort _ReadCount)
        {
            IPAddress = _IPAddress;
            Port = _Port;
            PLCNO = _PLCNO;
            DBNO = _DBNO;
            ReadBegin = _ReadBegin;
            ReadCount = _ReadCount;
        }

        public PLCReadServerParameter(string strPara)
        {
            //(new string[] { "127.0.0.1", "192.168.50.117" ,"192.168.50.111", "192.168.50.112" }, 0, 100, 0, 260);
            string[] strs = strPara.Split(';');
            string[] _IPAddress = strs[0].Split(',');
            uint _Port = Convert.ToUInt32(strs[1]);
            ushort _PLCNO = Convert.ToUInt16(strs[2]);
            ushort _DBNO = Convert.ToUInt16(strs[3]); ;
            ushort _ReadBegin = Convert.ToUInt16(strs[4]); ;
            ushort _ReadCount = Convert.ToUInt16(strs[5]); ;

            IPAddress = _IPAddress;
            Port = _Port;
            PLCNO = _PLCNO;
            DBNO = _DBNO;
            ReadBegin = _ReadBegin;
            ReadCount = _ReadCount;
        }

        public string GetParameterString()
        {
            return String.Join(";", String.Join(",", IPAddress),Port, PLCNO, DBNO, ReadBegin, ReadCount);
        }




    }

}
