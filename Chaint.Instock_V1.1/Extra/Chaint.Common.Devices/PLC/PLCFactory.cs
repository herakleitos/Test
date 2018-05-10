using System;
using System.Collections.Generic;
using System.Text;
/*-----------------------------------------------------------------------------------
 * 作者: Automation&IT Dept. 
 * 
 * 创建时间: 2014-02-18
 * 
 * 功能描述: 
 *      PLC 操作创建者
 * 
 ------------------------------------------------------------------------------------*/
namespace Chaint.Common.Devices.PLC
{
    public class PLCFactory
    {
        public static IPLCOperation CreatePLC(PLCConnItem plcConnParams)
        {
            IPLCOperation currPLCOperation;
            switch (plcConnParams.PlcType)
            {
                case PLCType.PLC_5D0:
                    currPLCOperation = new PLCOperation5D0(plcConnParams);
                    break;
                case PLCType.PLC_5D6:
                    currPLCOperation= new PLCOperation5D6(plcConnParams);
                    break;
                case PLCType.PLC_6D0:
                    currPLCOperation = new PLCOperation6D0(plcConnParams);
                    break;
                case PLCType.PLC_LibNoDave:
                    currPLCOperation = new PLCLibNoDave(plcConnParams);
                    break;
                default:
                    currPLCOperation = new PLCOperation6D0(plcConnParams);
                    break;
            }
            return currPLCOperation;
        }

        public static IPLCOperation CreatePLC(PLCConnItem plcConnParams,Chaint.Common.Devices.Data.DBAccessFunc dbAccessor)
        {
            IPLCOperation currPLCOperation;
            switch (plcConnParams.PlcType)
            {
                case PLCType.PLC_5D0:
                    currPLCOperation = new PLCOperation5D0(plcConnParams);
                    break;
                case PLCType.PLC_5D6:
                    currPLCOperation = new PLCOperation5D6(plcConnParams);
                    break;
                case PLCType.PLC_6D0:
                    currPLCOperation = new PLCOperation6D0(plcConnParams);
                    break;
                case PLCType.PLC_LibNoDave:
                    currPLCOperation = new PLCLibNoDave(plcConnParams);
                    break;
                case PLCType.PLC_Test:      //用来测试
                    currPLCOperation = new PLCOperationTest(dbAccessor,plcConnParams.PLCStationName,plcConnParams.PLCTemplateName);
                    break;
                default:
                    currPLCOperation = new PLCOperation6D0(plcConnParams);
                    break;
            }
            return currPLCOperation;
        }



    }
}
