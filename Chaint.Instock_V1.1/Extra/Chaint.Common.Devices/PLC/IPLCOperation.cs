using System;
using System.Collections.Generic;
using System.Text;
/*-----------------------------------------------------------------------------------
 * 作者: Automation&IT Dept. 
 * 
 * 创建时间: 2014-02-17
 * 
 * 功能描述: 
 *      PLC 操作类接口
 * 
 ------------------------------------------------------------------------------------*/
namespace Chaint.Common.Devices.PLC
{
    public interface IPLCOperation
    {
        event PLCMessageEventHandler OnPLCMessageChanged;

        bool Connected  {get;}
        PLCType CurrPLCType { get; }
           
        bool ConnectPLC();
        bool CloseConnectPLC();
        bool ReConnectPLC();

        bool ReadData(int dbno, string strAddr, ref string retBoolValue);
        bool ReadData(int dbno, int dwno, int amount, ref string retValue);
        bool ReadData(int dbno, int dwno, int amount, ref byte[] buffer);

        bool WriteData(int dbno, string strAddr);
        bool WriteData(int dbno, string strAddr, bool blnValue);
        bool WriteData(int dbno, int dwno, int amount, string strData);
        bool WriteData(int dbno, int dwno, int amount, ulong data);
        bool WriteData(int dbno, int dwno, int amount, byte[] buffer);
    }
}
