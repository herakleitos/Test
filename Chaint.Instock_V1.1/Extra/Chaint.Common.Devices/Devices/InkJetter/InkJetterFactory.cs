using System;
using System.Collections.Generic;
using System.Text;

/*-----------------------------------------------------------------------------------
 * 作者: Automation&IT Dept. 
 * 
 * 创建时间: 2016-05-03
 * 作者：
 * 用法：
 * 功能描述: 
 *      (1) 喷码机类型可以包含 伟迪捷,多米诺，Imaje4020,5200,5400
 *      (2) Imaje5200/5400 利用串口服务器作串口延长功能时，使用的通讯协议仍是ASCII Comms V2.3 但需要使用TCP Client连接串口服务器
 *      (3) Imaje5200/5400 利用串口服务器时，设备参数中
 * 
 ------------------------------------------------------------------------------------*/

namespace Chaint.Common.Devices.Devices
{
    public enum InkJetterType { Marsh, Domino, Imaje4020, Imaje5200,Imaje5400,SK3000 };

    public class InkJetterFactory
    {
        public static InkJetter CreateDevice(InkJetterType inkJetterType, Param_Base deviceParam)
        {
            InkJetter deviceObj = null;
            switch (inkJetterType)
            {
                case InkJetterType.Marsh:
                    deviceObj = new InkJetter_Marsh(deviceParam);
                    break;
                case InkJetterType.Domino:
                    deviceObj = new InkJetter_Domino(deviceParam);
                    break;
                case InkJetterType.Imaje4020:
                    deviceObj = new InkJetter_Imaje4020(deviceParam);
                    break;
                case InkJetterType.Imaje5200:
                case InkJetterType.Imaje5400:
                    deviceObj = new InkJetter_Imaje5200(deviceParam);
                    break;
                case InkJetterType.SK3000:      //国内杰特
                    deviceObj = new InkJetter_SK3000(deviceParam);
                    break;
            }
            return deviceObj;
        }
    }

}
