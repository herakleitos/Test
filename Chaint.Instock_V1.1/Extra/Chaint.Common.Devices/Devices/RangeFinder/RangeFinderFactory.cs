using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chaint.Common.Devices.Devices
{
    public enum RangeFinderType { Leuze, SickOD };
   

    public class RangeFinderFactory
    {
        public static RangeFinder CreateDevice(RangeFinderType deviceType, Param_Base deviceParam)
        {
            RangeFinder deviceObj = null;
            switch (deviceType)
            {
                case RangeFinderType.Leuze:
                    deviceObj = new RangeFinder_Leuze(deviceParam);
                    break;
                case RangeFinderType.SickOD:
                    deviceObj = new RangeFinder_SickOD(deviceParam);
                    break;
            }
            return deviceObj;
        }

    }


}
