using System;
using System.Collections.Generic;
using System.Text;

namespace Chaint.Common.Devices.Devices
{
    public enum ScanType { Sick, Metrologic,Symbol,Cognex};

    public class ScanFactory
    {
        public static Scanner CreateDevice(ScanType scanType, Param_Base deviceParam)
        {
            Scanner deviceObj = null;
            switch (scanType)
            {
                case ScanType.Sick:
                    deviceObj = new Scanner_Sick(deviceParam);
                    break;
                case ScanType.Metrologic:
                    deviceObj = new Scanner_Metrologic(deviceParam);
                    break;
                case ScanType.Symbol:
                    deviceObj = new Scanner_Symbol(deviceParam);
                    break;
                case ScanType.Cognex:
                    deviceObj = new Scanner_Cognex(deviceParam);
                    break;
                default:
                    deviceObj = new Scanner_Sick(deviceParam);
                    break;
            }
            return deviceObj;
        }
    }
}
