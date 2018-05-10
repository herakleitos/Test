using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chaint.Common.Devices.Devices.LED
{
    public class LED_ActionSytle
    {
        private InMode m_InMode = InMode.DIRECT;    //进入方式
        private Speed m_InSpeed = Speed.MID;
        private ushort m_StayTime = 4 * 10;         //持续时间

        private OutMode m_OutMode = OutMode.KEEP;    //保持不动
        private Speed m_OutSpeed = Speed.MID;
        private PlayCon m_PlayControl = PlayCon.ALWAYS; //播放不自动返回
        private ushort m_TimeOrLoops = 0;

        private bool m_IsWaitSend = true;

        public LED_ActionSytle()
        {

        }

        public LED_ActionSytle(InMode inMode, Speed inSpeed) :
            this(inMode, inSpeed, 4 * 10, OutMode.KEEP, Speed.MID, PlayCon.ALWAYS, 0, true)
        { }

        public LED_ActionSytle(InMode inMode, Speed inSpeed, OutMode outMode, Speed outSpeed) :
            this(inMode, inSpeed, 4 * 10, outMode, outSpeed, PlayCon.ALWAYS, 0, true)
        { }

        public LED_ActionSytle(InMode inMode,Speed inSpeed,OutMode outMode,Speed outSpeed,PlayCon playCon):
            this(inMode, inSpeed, 4 * 10, outMode, outSpeed, playCon, 0, true)
        { }

        public LED_ActionSytle(InMode inMode,Speed inSpeed,ushort stayTime,OutMode outMode,Speed outSpeed,PlayCon playCon,ushort timeOrLoop,bool isWaitSend)
        {
            m_InMode = InMode;
            m_InSpeed = inSpeed;
            m_StayTime = stayTime;
            m_OutMode = outMode;
            m_OutSpeed = outSpeed;
            m_PlayControl = playCon;
            m_TimeOrLoops = timeOrLoop;
            m_IsWaitSend = isWaitSend;

        }


        public bool IsWaitSend
        {
            get { return m_IsWaitSend; }
            set { m_IsWaitSend = value; }
        }


        public ushort TimeOrLoops
        {
            get { return m_TimeOrLoops; }
            set { m_TimeOrLoops = value; }
        }

        /// <summary>
        /// 即时消息控制 默认：播放不自动返回
        /// </summary>
        public PlayCon PlayControl
        {
            get { return m_PlayControl; }
            set { m_PlayControl = value; }
        }
        


        /// <summary>
        /// 持续时间
        /// </summary>

        public ushort StayTime
        {
            get { return m_StayTime; }
            set { m_StayTime = value; }
        }

        /// <summary>
        /// 进入速度
        /// </summary>
        public Speed InSpeed
        {
            get { return m_InSpeed; }
            set { m_InSpeed = value; }
        }


        /// <summary>
        /// 文本进入方式
        /// </summary>
        public InMode InMode
        {
            get { return m_InMode; }
            set { m_InMode = value; }
        }


        /// <summary>
        /// 消失速度
        /// </summary>
        public Speed OutSpeed
        {
            get { return m_OutSpeed; }
            set { m_OutSpeed = value; }
        }


        /// <summary>
        /// 文本消失方式
        /// </summary>
        public OutMode OutMode
        {
            get { return m_OutMode; }
            set { m_OutMode = value; }
        }



    }
}
