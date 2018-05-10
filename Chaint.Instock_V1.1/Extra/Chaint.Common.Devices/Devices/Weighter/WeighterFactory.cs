using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chaint.Common.Devices.Devices
{
    public enum WeighterType { Toledo, Vishay };

    public class WeighterFactory
    {
        public static Weighter CreateDevice(WeighterType weighterType, Param_Base deviceParam)
        {
            Weighter deviceObj = null;
            switch (weighterType)
            {
                case WeighterType.Toledo:
                    deviceObj = new Weighter_Toledo(deviceParam);
                    break;
                case WeighterType.Vishay:
                    deviceObj = new Weighter_Vishay(deviceParam);
                    break;
            }
            return deviceObj;
        }
    }



}
