using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;




/*-----------------------------------------------------------------------------------
 * 作者:Chaint.IT
 * 
 * 创建时间: 2013-09-06
 * 
 * 功能描述: 
 *      (单)双侧激光测距仪，开始使用时，需要输入定义的左侧参数COSA、右侧参数COSB、以及常量参数C,
 * 此三个参数需经过校正程序获取
 * Y=COSA.X1+COSB.X2+C
 * 
 * 如为单激光测距，则上述公式变为：
 * Y=COSA.X1+C,其中X2=0,COSB=1  (可以理解为测量物体与某一激光位置重叠）
 * 
 * 用法：
 *          Param_Base lparam=new Param_SerialPort("COM1");
            Param_Base rparam=new Param_SerialPort("COM2");
            AdjustParam adjustParam=new AdjustParam(cosa,cosb,c);
            RangeFinderHelper rangeFinder = new RangeFinderHelper(lparam, rparam, adjustParam);

            rangeFinder.OnRunMessage+=rangeFinder_OnRunMessage;
            rangeFinder.OnX1Changed+=rangeFinder_OnX1Changed;
            rangeFinder.OnX2Changed+=rangeFinder_OnX2Changed;
            rangeFinder.OnValueChanged+=rangeFinder_OnValueChanged;
            rangeFinder.OpenRangeFinder();
 * 
 ------------------------------------------------------------------------------------*/
namespace Chaint.Common.Devices.Devices
{
    public class RangeFinderHelper
    {
        private RangeFinderType m_RangeFinderType = RangeFinderType.Leuze;

        string m_LeftMeaureValue = "0";
        string m_RightMeasureValue = "0";

        RangeFinder m_LeftRangeFinderHelper = null;
        RangeFinder m_RightRangeFinderHelper = null;

        Param_Base m_LeftDeviceParam = null;
        Param_Base m_RightDevicePram = null;
        AdjustParam m_AdjustParams = null;
      
        public event RunMessageEventHandler OnRunMessage;
        public event ReadStringArrivedHandler OnValueChanged;               //估计值
        public event ReadStringArrivedHandler OnX1Changed;                  //左侧激光值                     
        public event ReadStringArrivedHandler OnX2Changed;                  //右侧激光值


        /// <summary>
        /// 左侧激光余弦值
        /// </summary>
        public double COSA
        {
            get { return m_AdjustParams.ParamA; }
            set 
            {

                if (m_AdjustParams.ParamA != value)
                {
                    m_AdjustParams.ParamA = value;
                    UpdateMeasureValue();
                }
            }
        }

        /// <summary>
        /// 右侧激光余弦值
        /// </summary>
        public double COSB
        {
            get { return m_AdjustParams.ParamB; }
            set 
            {
                if (m_AdjustParams.ParamB != value)
                {
                    m_AdjustParams.ParamB = value;
                    UpdateMeasureValue();
                }
                
            }
        }

        /// <summary>
        /// 左右侧激光水平距离(mm)
        /// </summary>
        public double LengthAB
        {
            get { return m_AdjustParams.ParamC; }
            set 
            {
                if (m_AdjustParams.ParamC != value)
                {
                    m_AdjustParams.ParamC = value;
                    UpdateMeasureValue();
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="leftDeviceParam">左侧设备参数</param>
        /// <param name="rightDeviceParam">右侧设备参数</param>
        /// <param name="adjustParam">调节参数 cosa,cosb,c</param>
        public RangeFinderHelper(Param_Base leftDeviceParam,Param_Base rightDeviceParam,AdjustParam adjustParam):this(RangeFinderType.Leuze,leftDeviceParam,rightDeviceParam,adjustParam)
        { }

        public RangeFinderHelper(RangeFinderType deviceType, Param_Base leftDeviceParam, Param_Base rightDeviceParam, AdjustParam adjustParam)
        {
            m_RangeFinderType=deviceType;
            m_LeftDeviceParam = leftDeviceParam;
            m_RightDevicePram = rightDeviceParam;
            m_AdjustParams = adjustParam;
        }

        /// <summary>
        /// 打开激光测距仪
        /// </summary>
        public void OpenRangeFinder()
        {
            if (m_LeftRangeFinderHelper == null && m_LeftDeviceParam!=null )
            {
                m_LeftRangeFinderHelper = RangeFinderFactory.CreateDevice(m_RangeFinderType, m_LeftDeviceParam);
                m_LeftRangeFinderHelper.OnRunMessage+=RangeFinderHelper_OnRunMessage;
                m_LeftRangeFinderHelper.OnRetMeasureValue+=m_LeftRangeFinderHelper_OnRetMeasureValue;
                m_LeftRangeFinderHelper.Connect();
            }

            if (m_RightRangeFinderHelper == null && m_RightDevicePram!=null)
            {
                m_RightRangeFinderHelper=RangeFinderFactory.CreateDevice(m_RangeFinderType,m_RightDevicePram);
                m_RightRangeFinderHelper.OnRunMessage+=RangeFinderHelper_OnRunMessage;
                m_RightRangeFinderHelper.OnRetMeasureValue+=m_RightRangeFinderHelper_OnRetMeasureValue;
                m_RightRangeFinderHelper.Connect();
            }
        }

        /// <summary>
        /// 关闭激光，并释放资源
        /// </summary>
        public void CloseRangeFinder()
        {
            if (m_LeftRangeFinderHelper != null )
            {
                m_LeftRangeFinderHelper.OnRunMessage-=RangeFinderHelper_OnRunMessage;
                m_LeftRangeFinderHelper.OnRetMeasureValue-=m_LeftRangeFinderHelper_OnRetMeasureValue;
                m_LeftRangeFinderHelper.Disconnect();
            }

            if (m_RightRangeFinderHelper != null)
            {
                m_RightRangeFinderHelper.OnRunMessage-=RangeFinderHelper_OnRunMessage;
                m_RightRangeFinderHelper.OnRetMeasureValue-=m_RightRangeFinderHelper_OnRetMeasureValue;
                m_RightRangeFinderHelper.Disconnect();
            }
        }

        private void m_LeftRangeFinderHelper_OnRetMeasureValue(string strReadValue)
        {
            m_LeftMeaureValue=strReadValue;
 	        if(OnX1Changed!=null) OnX1Changed(strReadValue);
            UpdateMeasureValue();
        }

        private void RangeFinderHelper_OnRunMessage(object sender, string strMsg)
        {
 	        if (OnRunMessage != null) OnRunMessage(sender,strMsg);
        }

        private void m_RightRangeFinderHelper_OnRetMeasureValue(string strReadValue)
        {
            m_RightMeasureValue=strReadValue;
 	        if(OnX2Changed!=null) OnX2Changed(strReadValue);
            UpdateMeasureValue();
        }

        private void UpdateMeasureValue()
        {
            double leftvalue = 0;
            double rightvalue = 0;
            int intValue = -1;

            try
            {
                if (!double.TryParse(m_LeftMeaureValue, out leftvalue))
                {
                    RangeFinderHelper_OnRunMessage(this,string.Format("左侧激光测量值有误<{0}>", m_LeftMeaureValue));
                    intValue = -1;
                }
                else if (!double.TryParse(m_RightMeasureValue, out rightvalue))
                {
                    RangeFinderHelper_OnRunMessage(this,string.Format("右侧激光测量值有误<{0}>", m_RightMeasureValue));
                    intValue = -1;
                }
                else
                {
                    intValue = Convert.ToInt32(m_AdjustParams.ParamC - leftvalue * m_AdjustParams.ParamA - rightvalue * m_AdjustParams.ParamB + 0.5f);
                    if (intValue < 0) intValue = -1;
                }
                if (OnValueChanged != null) OnValueChanged(intValue.ToString());
            }
            catch (System.Exception ex)
            {
                if (OnValueChanged != null) OnValueChanged("-99");
                RangeFinderHelper_OnRunMessage(this,string.Format("更新出错<{0}>|<{1}>|<{2}>,原因:{3}", leftvalue, rightvalue, m_AdjustParams.ParamC, ex.Message));
            }
        }
    }


   



}
