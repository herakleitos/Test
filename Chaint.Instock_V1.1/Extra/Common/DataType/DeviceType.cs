using System;
using System.Collections.Generic;
using System.Text;

namespace CTWH.Common.DataType
{
    public enum DeviceType
    {
        Scanner = 0x0,
        InkJet = 0x1,
        Weight = 0x2,
        WidthMeasure = 0x3,
        Computer = 0x4,
        PLC = 0x5,
        SQL = 0x6,
        DEMAGSocket = 0x7,

        SAPOracle = 0x8,

        MES = 0x9,
        Storage= 0x10,
        Gripper = 0x11,
        
        Other = 0x12

        
    }
}
