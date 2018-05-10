using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chaint.Common.Devices.Devices
{
    public class RangeFinder_SickOD:RangeFinder_Leuze
    {
        private Param_Base m_ConnParam = null;

        public override RangeFinderType DeviceType
        {
            get { return RangeFinderType.SickOD; }
        }

        public RangeFinder_SickOD(Param_Base deviceParam):base(deviceParam)
        {
            
        }
    }
}
