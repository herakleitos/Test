using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;

namespace CTWH.Common.DataType
{
    public struct SerialPortParameter
    {
        public string PortName;
        public int BaudRate;
        public Parity Parity;
        public int DataBits;
        public StopBits StopBits;
        public bool DtrEnable;
        public SerialPortParameter(string _PortName,int _BaudRate,Parity _Parity,int _DataBits,StopBits _StopBits,bool _DtrEnable)
        {
            PortName = _PortName;
            BaudRate = _BaudRate;
            Parity = _Parity;
            DataBits= _DataBits;
            StopBits=_StopBits;
            DtrEnable = _DtrEnable;
        }

        //public SerialPortParameter(string _PortName, int _BaudRate, Parity _Parity, int _DataBits, StopBits _StopBits)
        //{
        //    PortName = _PortName;
        //    BaudRate = _BaudRate;
        //    Parity = _Parity;
        //    DataBits = _DataBits;
        //    StopBits = _StopBits;
        //    DtrEnable = false; //默认为false
        //}

        public SerialPortParameter(string strPara)
        {
            string[] strs = strPara.Split(',');

            PortName = strs[0];
            BaudRate = Convert.ToInt32(strs[1]);
            Parity = (Parity)Enum.Parse(typeof(Parity), strs[2]);
            DataBits = Convert.ToInt32( strs[3]);
            StopBits = (StopBits)Enum.Parse(typeof(StopBits), strs[4]);
            if (strs.Length > 5)
            {
                DtrEnable = strs[5].ToLower().Equals("true");
            }
            else
            {
                DtrEnable = false;
            }
        }

        public string GetParameterString()
        {
            return String.Join(",", PortName, BaudRate, Parity, DataBits, StopBits, DtrEnable);
        }
    }

 
}
