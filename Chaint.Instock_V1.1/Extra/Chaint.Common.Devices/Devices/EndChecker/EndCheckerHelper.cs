using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/*-----------------------------------------------------------------------------------
 * 作者:Chaint.IT
 * 
 * 创建时间: 2014-09-06
 * 
 * 功能描述: 
 *     端面检测功能模块
 * 
 * 用法：
 *          Param_Base deviceParam=new Param_SerialPort("COM1");
            EndCheckerHelper checkerHelper=new EndCheckerHelper(CheckerType.SickOD,deviceParam);
 *          checkerHelper.MinSampleValue=0;
 *          checkerHelper.MaxSampleValue=180;
            checkerHelper.OnRunMessage+= EndCheckerHelper_OnRunMessage;
 *          checkerHelper.OnValueChanged+= EndCheckerHelper_OnValueChanged;
            checkerHelper.OpenEndChecker(); //打开端面检测
 * 
 *          checkerHelper.StartMeasure();   //开始测量　
 *          
 *          checkerHelper.StopMeasure();    //停止测量　
 *          
 *          checkerHelper.CloseEndChecker();  //关闭端面检测
 ------------------------------------------------------------------------------------*/
namespace Chaint.Common.Devices.Devices
{
    public class EndCheckerHelper
    {
        private CheckerType m_DeviceType = CheckerType.SickOD;

        private double m_MeasureValue = 0;

        private IList<double> m_lstSampleValues=new List<double>();
        
        private EndChecker m_EndChecker = null;
        private Param_Base m_DeviceParam = null;
        
        
        public event RunMessageEventHandler OnRunMessage;
        public event ReadDoubleArrivedHandler OnValueChanged;               //激光实时值  

        /// <summary>
        /// 采样的最小距离值,默认值为0
        /// </summary>
        [DefaultValue(0)]
        public double MinSampleValue { get; set; }

        /// <summary>
        /// 采样的最大距离值，默认值为180
        /// </summary>
        [DefaultValue(180)]
        public double MaxSampleValue { get; set; }

        /// <summary>
        /// 采样集合
        /// </summary>
        public IList<double> SampleValues { get { return m_lstSampleValues; } }

        public EndCheckerHelper(CheckerType deviceType, Param_Base deviceParam)
        {
            m_DeviceType = deviceType;
            m_DeviceParam = deviceParam;
        }


        /// <summary>
        /// 打开端面检测 设备
        /// </summary>
        public void OpenEndChecker()
        {
            if (m_EndChecker == null && m_DeviceParam != null)
            {
                m_EndChecker = EndCheckerFactory.CreateDevice(m_DeviceType, m_DeviceParam);
                m_EndChecker.OnRunMessage += EndChecker_OnRunMessage;
                m_EndChecker.OnRetMeasureValue += EndChecker_OnRetMeasureValue;
                m_EndChecker.Connect();
            }
        }

        /// <summary>
        /// 关闭端面检测，并释放资源
        /// </summary>
        public void CloseEndChecker()
        {
            if (m_EndChecker != null)
            {
                m_EndChecker.OnRunMessage -= EndChecker_OnRunMessage;
                m_EndChecker.OnRetMeasureValue -= EndChecker_OnRetMeasureValue;
                m_EndChecker.Disconnect();
            }
        }

        private void EndChecker_OnRetMeasureValue(string strReadValue)
        {
            m_MeasureValue = double.Parse(strReadValue);
            if (m_MeasureValue < MaxSampleValue && m_MeasureValue > MinSampleValue) m_lstSampleValues.Add(m_MeasureValue);
            if (OnValueChanged != null) OnValueChanged(m_MeasureValue);
        }

        private void EndChecker_OnRunMessage(object sender, string strMsg)
        {
            if (OnRunMessage != null) OnRunMessage(sender, strMsg);
        }

       
        /// <summary>
        /// 开始连续测量
        /// </summary>
        public void StartMeasure()
        {
            if (m_EndChecker != null) m_EndChecker.Write("START_MEASURE");
        }

        /// <summary>
        /// 停止连续测量
        /// </summary>
        public void StopMeasure()
        {
            if (m_EndChecker != null) m_EndChecker.Write("STOP_MEASURE");
        }

        /// <summary>
        /// 设置采样频率
        /// </summary>
        public void Send_AVGToMedium_ToDevice()
        {
            if (m_EndChecker != null) m_EndChecker.Write("AVG MEDIUM");
        }


        #region 以下为样本值与内凹与外凸值计算
        public void GetInnerAndOutterValue(ref string dInnerValue, ref string dOutterValue)
        {
            dInnerValue = "0";
            dOutterValue = "0";
            double excludeValue = -100;
            double avg = GetAverage(m_lstSampleValues);
            double stdev = GetStdev(m_lstSampleValues);
            double minSampleValue = 0d;
            double maxSampleValue = 0d;
            ComputeMaxAndMinSampleValue(m_lstSampleValues, excludeValue, ref minSampleValue, ref maxSampleValue);
            SampleFilter(m_lstSampleValues, excludeValue, avg, stdev);

            dInnerValue = System.Math.Abs(maxSampleValue - avg).ToString("F3");
            dOutterValue = System.Math.Abs(avg - minSampleValue).ToString("F3");
        }

        /// <summary>
        /// 根据样本集合，获取端面凹和凸值
        /// </summary>
        /// <param name="lstCollects">样本集合</param>
        /// <param name="dInnerValue">内凹值</param>
        /// <param name="dOutterValue">外凸值</param>
        public void GetInnerAndOutterValue(IList<double> lstCollects, ref string dInnerValue, ref string dOutterValue)
        {
            dInnerValue = "0";
            dOutterValue = "0";
            double excludeValue = -100;
            double avg = GetAverage(lstCollects);
            double stdev = GetStdev(lstCollects);
            double minSampleValue = 0d;
            double maxSampleValue = 0d;
            ComputeMaxAndMinSampleValue(lstCollects, excludeValue, ref minSampleValue, ref maxSampleValue);
            SampleFilter(lstCollects, excludeValue, avg, stdev);

            dInnerValue = System.Math.Abs(maxSampleValue - avg).ToString("F3");
            dOutterValue = System.Math.Abs(avg - minSampleValue).ToString("F3");
        }

        /// <summary>
        /// 根据样本集合，获取端面凹和凸值
        /// </summary>
        /// <param name="lstCollects">样本集合</param>
        /// <param name="dInnerValue">内凹值</param>
        /// <param name="dOutterValue">外凸值</param>
        /// <param name="dAvgValue">平均值</param>
        public void GetInnerAndOutterValue(IList<double> lstCollects, ref string dInnerValue, ref string dOutterValue, ref string dAvgValue)
        {
            dInnerValue = "0";
            dOutterValue = "0";
            dAvgValue = "0";
            double excludeValue = -100;
            double avg = GetAverage(lstCollects);
            double stdev = GetStdev(lstCollects);
            double minSampleValue = 0d;
            double maxSampleValue = 0d;
            ComputeMaxAndMinSampleValue(lstCollects, excludeValue, ref minSampleValue, ref maxSampleValue);
            SampleFilter(lstCollects, excludeValue, avg, stdev);

            dInnerValue = System.Math.Abs(maxSampleValue - avg).ToString("F3");
            dOutterValue = System.Math.Abs(avg - minSampleValue).ToString("F3");
            dAvgValue = avg.ToString("F3");
        }

        /// <summary>
        /// 计算平均值
        /// </summary>
        /// <param name="lstCollects"></param>
        /// <returns></returns>
        private double GetAverage(IList<double> lstCollects)
        {
            if (lstCollects == null || lstCollects.Count == 0) return 0d;
            double sum = 0;
            foreach (double d in lstCollects)
            {
                sum += d;
            }
            return sum / lstCollects.Count;
        }

        /// <summary>
        /// 计算标准差
        /// 
        /// stdev=1/(n-1) sqrt(xi-avg)2
        /// 所有数减去其平均值的平方和，所得结果除以该组数之个数（或个数减一，即变异数），再把所得值开根号，所得之数就是这组数据的标准差
        /// 
        /// 在计算方法上的差异是：样本标准差^2=（样本方差/（数据个数-1））；总体标准差^2=（总体方差/（数据个数））。
        /// </summary>
        /// <param name="lstCollects"></param>
        /// <returns></returns>
        private double GetStdev(IList<double> lstCollects)
        {
            if (lstCollects == null || lstCollects.Count == 0) return 0d;

            double avg = GetAverage(lstCollects);
            double sum = 0;
            foreach (double d in lstCollects)
            {
                sum += (d - avg) * (d - avg);
            }
            return System.Math.Sqrt(sum / (lstCollects.Count));
        }

        /// <summary>
        /// 计算最小值与最大值,此处需要排除某一种值
        /// </summary>
        /// <param name="lstCollects"></param>
        /// <param name="excludeValue">需要排除的值</param>
        /// <param name="minValue">返回最小值</param>
        /// <param name="maxValue">返回最大值</param>
        private void ComputeMaxAndMinSampleValue(IList<double> lstCollects, double excludeValue, ref double minValue, ref double maxValue)
        {
            minValue = 0d;
            maxValue = 0d;
            for (int i = 0; i < lstCollects.Count; i++)
            {
                if (lstCollects[i] > maxValue && lstCollects[i] != excludeValue) maxValue = lstCollects[i];
                if (lstCollects[i] < minValue && lstCollects[i] != excludeValue) minValue = lstCollects[i];
            }
        }

        /// <summary>
        /// 样本过滤(排除噪声点)，只保留样本中聚集在一起的95%
        /// </summary>
        /// <param name="lstCollects">样本集合</param>
        /// <returns></returns>
        private void SampleFilter(IList<double> lstCollects, double excludeValue)
        {
            double avg = GetAverage(lstCollects);   //计算平均值
            double stdev = GetStdev(lstCollects);   //计算标准差

            //只取合乎要求的95%的样本值
            for (int i = 0; i < lstCollects.Count; i++)
            {
                if (System.Math.Abs(lstCollects[i] - avg) - 2 * stdev > 0)
                    lstCollects[i] = excludeValue;
            }
        }

        /// <summary>
        /// 样本过滤(排除噪声点)，只保留样本中聚集在一起的95%
        /// </summary>
        /// <param name="lstCollects">集合</param>
        /// <param name="excludeValue">排除的值</param>
        /// <param name="avg">样本平均值</param>
        /// <param name="stdev">样本标准差</param>
        private void SampleFilter(IList<double> lstCollects, double excludeValue, double avg, double stdev)
        {
            //只取满足要求的95%的样本值
            for (int i = 0; i < lstCollects.Count; i++)
            {
                if (System.Math.Abs(lstCollects[i] - avg) - 2 * stdev > 0)
                    lstCollects[i] = excludeValue;
            }
        }

        #endregion

    }
}
