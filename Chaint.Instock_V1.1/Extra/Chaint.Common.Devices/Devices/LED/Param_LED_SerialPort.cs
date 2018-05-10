using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chaint.Common.Devices.Devices.LED
{
    public class Param_LED_SerialPort:Param_Base
    {
        byte m_ScreenNO = 1;
        uint m_PortNO = 1;      //对应 COM1
        BaudNo m_BaudRate = BaudNo.NO_115200;
        int m_MinScreenNO = 1;                  //最小屏号
        int m_MaxScreenNO = 253;                //最大屏号

        public Param_LED_SerialPort(byte screenNO, uint portNO):this(screenNO,portNO,BaudNo.NO_115200)
        {
           
        }

        public Param_LED_SerialPort(byte screenNO,uint portNO,BaudNo baudRate)
        {
            m_ScreenNO = screenNO;
            m_PortNO = portNO;
            m_BaudRate = baudRate;
        }

        /// <summary>
        /// 波特率 默认为115200，注意与LED屏中设置一一对应
        /// </summary>
        public BaudNo BaudRate
        {
            get { return m_BaudRate; }
            set { m_BaudRate = value; }
        }


        /// <summary>
        /// 端口号，注意需要去掉字母COM
        /// </summary>
        public uint PortNO
        {
            get { return m_PortNO; }
            set { m_PortNO = value; }
        }

        /// <summary>
        /// 屏号 默认为1，与屏中设置的编号要一一对应
        /// </summary>
        public byte ScreenNO
        {
            get { return m_ScreenNO; }
            set { m_ScreenNO = value; }
        }
    }
}
