using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Intermec.DataCollection.RFID;

/*-----------------------------------------------------------------------------------
 * 作者: Automation&IT Dept. 
 * 
 * 创建时间: 2014-03-07
 * 
 * 功能描述: 
 *      (1) 注：仅对Intermec 类型的RFID　进行测试,具体使用的类还有待重构!!!!
 *      
 * 
 ------------------------------------------------------------------------------------*/
namespace Chaint.Common.Devices.RFID
{
    public delegate void EventHandlerConnectStatus(object sender,RFIDEventArgs e);
    public delegate void EventHandlerRFIDTagData(object sender,TagEventArgs e);
    public delegate void EventHandlerCurrConfigInfo(string currInfo);


    public enum ConnType { TCP, SerialPort };
    public enum TagReportType { EventAll,Event};
    
    public class RFIDEventArgs : EventArgs
    {
        private string m_strMsg = "";
        private bool m_blnConnected = false;
        
        public RFIDEventArgs(string strMsg, bool blnConnected)
        {
            m_strMsg = strMsg;
            m_blnConnected = blnConnected;
        }
        public string RetMessage
        {
            get { return m_strMsg; }
        }

        public bool Connected
        {
            get { return m_blnConnected; }
        }
    }

    /// <summary>
    /// 标签参数
    /// </summary>
    public class TagEventArgs : EventArgs
    {
        private string m_strDataString = "";            //返回所有数据(包含标签、天线、RSSI值,用逗号分隔)
        private string m_strTagData="";                 //标签数据
        private long m_lngANTID = 1;                  //天线索引值 1--4
        private float m_fltRSSI = -128;                 //RSSI值 0----128db 有小数
        private string m_TimeStamp = "";                //读取时间

        /// <summary>
        /// 返回数据字符串,以逗号分隔 TagData,AntIndex,RSSI
        /// </summary>
        public string DataString
        {
            get { return m_strDataString; }
            set { m_strDataString = value; }
        }

        /// <summary>
        /// 纯标签数据
        /// </summary>
        public string TagData
        {
            get { return m_strTagData; }
            set { m_strTagData = value; }
        }

        /// <summary>
        /// 天线索引值 1--4
        /// </summary>
        public long AntID
        {
            get { return m_lngANTID; }
            set { m_lngANTID = value; }
        }

        /// <summary>
        /// RSSI值 0-- -128db,值越大，标签离天线越近
        /// </summary>
        public float RSSIValue
        {
            get { return m_fltRSSI; }
            set { m_fltRSSI = value; }
        }

        /// <summary>
        /// 读取时间戳
        /// </summary>
        public string TimeStamp
        {
             get { return m_TimeStamp; }
            set { m_TimeStamp = value; }
        }
    }

    public class RFIDHepler
    {
        public event EventHandlerConnectStatus OnRFIDConnectedStatus;
        public event EventHandlerCurrConfigInfo OnRFIDConfigInfo;        
        public event EventHandlerRFIDTagData OnRFIDTagData;

        private bool m_blnConnected = false;
        private ConnType m_ConnType = ConnType.TCP;
        private TagReportType m_TagReportType = TagReportType.EventAll;

        private string m_ConnPort = "172.16.5.230";
        private BRIReader m_RFIDReader = null;
      
        
        public bool IsConnected
        {
            get { return m_blnConnected; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strConnAddr"></param>
        /// <param name="strAnts">1,2,3,4</param>
        public RFIDHepler(ConnType enmConnType, string strConnPort)
        {
            m_ConnType = enmConnType;
            if (enmConnType == ConnType.TCP)
                m_ConnPort = string.Format("TCP://{0}",strConnPort);
            else
                m_ConnPort = string.Format("SERIAL://{0}", strConnPort.ToUpper());
        }

        public RFIDHepler(ConnType enmConnType, string strConnPort,TagReportType enmTagReportType)
        {
            m_TagReportType = enmTagReportType;
            m_ConnType = enmConnType;
            if (enmConnType == ConnType.TCP)
                m_ConnPort = string.Format("TCP://{0}", strConnPort);
            else
                m_ConnPort = string.Format("SERIAL://{0}", strConnPort.ToUpper());
        }

        /// <summary>
        /// 测试与RFID的连接
        /// </summary>
        /// <returns></returns>
        public bool TestConnect()
        {
            string strMsg = "";
            bool blnConnected = false;
            if (m_RFIDReader == null) return false;
            string retMsg = m_RFIDReader.Execute("PING");
            if (retMsg.Equals("OK>"))
            {
                blnConnected = true;
                strMsg = "Successful Connected!";
            }
            else
            {
                blnConnected = false;
                strMsg = retMsg;
            }
            SendMessage(strMsg, m_blnConnected);
            return blnConnected;
        }

        /// <summary>
        /// 打开RFID 设备
        /// </summary>
        /// <returns></returns>
        public bool OpenReader(System.Windows.Forms.Control owner)
        {
            string strMsg = "";
            m_blnConnected = false;
            try
            {
                m_RFIDReader = new BRIReader(owner, m_ConnPort);
                if (m_RFIDReader == null)
                {
                    strMsg = string.Format("不能初始化RFID阅读器<{0}>", m_ConnPort);
                    m_blnConnected = false;
                }
                else
                {
                    //校验当前连接是否正常
                    string retMsg = m_RFIDReader.Execute("PING");
                    if (retMsg != null)
                    {
                        if (retMsg.Equals("OK>"))
                        {
                            m_RFIDReader.EventHandlerDeviceConnectState += new DCE_DeviceConnectStateEventHandlerAdv(m_RFIDReader_EventHandlerDeviceConnectState);
                            m_RFIDReader.EventHandlerTag += new Tag_EventHandlerAdv(m_RFIDReader_EventHandlerTag);
                            m_blnConnected = true;
                            strMsg = "RFID阅读器连接成功！";
                        }
                        else
                        {
                            m_blnConnected = false;
                            strMsg = retMsg;
                        }
                    }
                    else
                    {
                        m_blnConnected = false;
                        strMsg = "PING命令错误,原因:" + retMsg;
                    }
                }
            }
            catch (System.Exception ex)
            {
                m_blnConnected = false;
                strMsg = "连接错误,原因:" + ex.Message;
            }
            SendMessage(strMsg, m_blnConnected);
            return m_blnConnected;
        }

        /// <summary>
        /// 开始读取标签
        /// </summary>
        public void SartReadTags()
        {
            if (m_TagReportType == TagReportType.EventAll)
                StartReadTagsByEventALL();
            else
                StartReadTagsByEvent();
        }

        /// <summary>
        /// 在打开阅读器，并设置属性后开始读取标签 
        /// 一直读取标签
        /// </summary>
        /// <returns></returns>
        private void StartReadTagsByEventALL()
        {
            if (m_RFIDReader != null && m_RFIDReader.IsConnected)
            {
                m_RFIDReader.StartReadingTags(null, "ANT,RSSI,TIME", BRIReader.TagReportOptions.EVENTALL);
            }
            else
            {
                SendMessage("类未初始化或者RFID阅读器未连接,无法开始读取",false);
            }
        }

        /// <summary>
        /// 相同标签仅读取一个后，不再重复读取
        /// </summary>
        private void StartReadTagsByEvent()
        {
            if (m_RFIDReader != null && m_RFIDReader.IsConnected)
            {
                m_RFIDReader.StartReadingTags(null, "ANT,RSSI,TIME", BRIReader.TagReportOptions.EVENT);
            }
            else
            {
                SendMessage("类未初始化或者RFID阅读器未连接,无法开始读取", false);
            }
        }

        /// <summary>
        /// 可人工执行命令
        /// </summary>
        /// <param name="strCommands"></param>
        /// <returns></returns>
        public string ExecuteCommand(string strCommands)
        {
            try
            {
                if (m_RFIDReader != null && m_RFIDReader.IsConnected)
                    return m_RFIDReader.Execute(strCommands);
                else
                    return "";
            }
            catch (System.Exception ex)
            {
                SendMessage(ex.Message, true);
                return "";
            }
        }

        /// <summary>
        /// 关闭RFID 读取器
        /// </summary>
        /// <returns></returns>
        public bool CloseReader()
        {
            if (m_RFIDReader != null)
            {
                SendMessage("连接已关闭", false);

                m_RFIDReader.StopReadingTags();
                m_RFIDReader.EventHandlerDeviceConnectState -= new DCE_DeviceConnectStateEventHandlerAdv(m_RFIDReader_EventHandlerDeviceConnectState);
                m_RFIDReader.EventHandlerTag -= new Tag_EventHandlerAdv(m_RFIDReader_EventHandlerTag);
                m_RFIDReader.Dispose();
                m_RFIDReader = null;
            }
            m_blnConnected = false;
            return true;
        }

        /// <summary>
        /// 停止读取标签(相当于暂停)
        /// </summary>
        public void StopReadingTags()
        {
            if (m_RFIDReader != null)
                m_RFIDReader.StopReadingTags();
        }

        /// <summary>
        /// 重连阅读器
        /// </summary>
        /// <param name="owner"></param>
        /// <returns></returns>
        public bool ReConnectReader(System.Windows.Forms.Control owner)
        {
            CloseReader();
            System.Threading.Thread.Sleep(200);
            bool bln = OpenReader(owner);
            return bln;
        }

        private void m_RFIDReader_EventHandlerTag(object sender, EVTADV_Tag_EventArgs e)
        {
            if (OnRFIDTagData != null)
            {
                //OnRFIDTagData(EvtArgs.DataString);
                TagEventArgs tagInfo = new TagEventArgs();
                tagInfo.DataString = e.DataString;
                tagInfo.TagData = e.Tag.ToString();
                tagInfo.AntID = e.Tag.TagFields.GetField(0).DataInt;
                tagInfo.RSSIValue = float.Parse(e.Tag.TagFields.GetField(1).DataString);
                tagInfo.TimeStamp = e.Tag.TagFields.GetField(2).DataString;
                OnRFIDTagData(this, tagInfo);
            }
        }

        private void m_RFIDReader_EventHandlerDeviceConnectState(object sender, EVTADV_DeviceConnectStateEventArgs EvtArgs)
        {
            string strMsg="";
            //EVTADV_DeviceConnectStateEventArgs.States
            if(EvtArgs.DeviceConnectState==EVTADV_DeviceConnectStateEventArgs.States.CONNECTED)
            {
                m_blnConnected=true;
                strMsg="Connected";
            }
            else
            {
                m_blnConnected=false;
                strMsg=EvtArgs.DeviceConnectState.ToString();
            }
            SendMessage(strMsg, m_blnConnected);
        }

        /// <summary>
        /// 设置属性列表
        /// </summary>
        /// <param name="strCommands"></param>
        public void SetAttributes(string[] strCommands)
        {
            if (m_RFIDReader == null || m_RFIDReader.IsConnected == false) return;
            for (int i = 0; i < strCommands.Length;i++ )
            {
                if (strCommands[i].Trim().Length == 0)
                    continue;
                m_RFIDReader.Execute(strCommands[i]);
            }
            
        }

        public void SetAttributes(ArrayList arCommands)
        {
            if (m_RFIDReader == null || m_RFIDReader.IsConnected == false || arCommands.Count==0) return;

            for (int i = 0; i < arCommands.Count; i++)
            {
                if (arCommands[i].ToString().Trim().Length == 0)
                    continue;
                m_RFIDReader.Execute(arCommands[i].ToString().Trim());
            }
            if (OnRFIDConfigInfo != null)
            {
                string strAttribs = m_RFIDReader.Execute("ATTRIB");
                OnRFIDConfigInfo(strAttribs);
            }
        }

        /// <summary>
        /// 设置属性值
        /// </summary>
        /// <param name="strAttributeName"></param>
        /// <param name="strValue"></param>
        private void SetReaderAttribute(string strAttributeName, string strValue)
        {
            string retMsg = null;
            try
            {
                if (strValue == "")
                {
                    retMsg = m_RFIDReader.Execute(strAttributeName.ToUpper());
                }
                else
                {
                    retMsg = m_RFIDReader.Execute(string.Format("ATTRIB {0}={1}", strAttributeName.ToUpper(), strValue.ToUpper()));
                }
                if (OnRFIDConfigInfo != null) OnRFIDConfigInfo(retMsg);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 设置简单模式
        /// </summary>
        public void SetSimpleReaderAttributes()
        {
            string retMsg = null;
            try
            {
                m_RFIDReader.Execute(string.Format("ATTRIB ANTS=1,2,3,4"));          //天线选择1,2,3,4
                m_RFIDReader.Execute("ATTRIB IDTRIES=1");        //标签读取次数
                m_RFIDReader.Execute("ATTRIB ANTTRIES=1");      //
                m_RFIDReader.Execute("ATTRIB TIMEOUTMODE=OFF");      //关闭超时模式
                m_RFIDReader.Execute("ATTRIB IDTIMEOUT=0");       
                m_RFIDReader.Execute("ATTRIB ANTTIMEOUT=0");     
                m_RFIDReader.Execute("ATTRIB FIELDSTRENGTH=30db,30db,30db,30db");
                if (OnRFIDConfigInfo != null)
                {
                    retMsg = m_RFIDReader.Execute("ATTRIB");
                    OnRFIDConfigInfo(retMsg);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void SendMessage(string strMsg, bool blnConnected)
        {
            if (OnRFIDConnectedStatus != null)
            {
                OnRFIDConnectedStatus(this, new RFIDEventArgs(strMsg, blnConnected));
            }
        }

    }
}
