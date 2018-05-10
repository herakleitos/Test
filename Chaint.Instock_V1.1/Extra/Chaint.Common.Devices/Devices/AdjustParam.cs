using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chaint.Common.Devices.Devices
{
    
    /// <summary>
    /// 调节参数 用于测距仪、端面检测或者其他
    /// </summary>
    public class AdjustParam
    {
        double m_Param1 = 0;          //左侧激光与水平面的参数值（0--1）  COSA
        double m_Param2 = 0;          //右侧激光与水平面的参数值（0--1）  COSB
        double m_Param3 = 0;          //左右激光间的水平距离               C

        public AdjustParam(double paramA, double paramB, double paramC)
        {
            m_Param1 = paramA;
            m_Param2 = paramB;
            m_Param3 = paramC;
        }

        public double ParamA
        {
            get { return m_Param1; }
            set { m_Param1 = value; }
        }

        public double ParamB
        {
            get { return m_Param2; }
            set { m_Param2 = value; }
        }

        public double ParamC
        {
            get { return m_Param3; }
            set { m_Param3 = value; }
        }

    }



}
