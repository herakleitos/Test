using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chaint.Common.Devices.Devices
{
    public enum CheckerType { SickOD };

    public class EndCheckerFactory
    {
        public static EndChecker CreateDevice(CheckerType deviceType, Param_Base deviceParam)
        {
            EndChecker deviceObj = null;
            switch (deviceType)
            {
                case CheckerType.SickOD:
                    deviceObj = new EndChecker_SickOD(deviceParam);
                    break;
            }
            return deviceObj;
        }
    }
}
