using System;
using System.Data;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using Chaint.Common.Devices.PLC.Utils;

/*-----------------------------------------------------------------------------------
 * 作者: Automation&IT Dept. 
 * 
 * 创建时间: 2014-02-18
 * 
 * 功能描述: 
 *      PLC 报警信息处理类
 * 注:
 * (1) 使用前需要输入PLC连接类,此类主要与数据表PLC_Alarm_Connect相关
 * (2) 需要输入PLC对应的地址的报警信息,主要与数据表PLC_Alarm_Data相关
 * 
 * 已测试 2014-02-18
 ------------------------------------------------------------------------------------*/

namespace Chaint.Common.Devices.PLC
{
    public class PLCAlarmOperation:IDisposable
    {
        private PLCConnectCollection m_PLCConnects = new PLCConnectCollection();     //报警连接集合
        private DataTable m_dtPLCAlarmData = new DataTable();                       //报警数据
        private string m_strGongweiType = "";
        private string m_strMobanNO = "";

        private Hashtable m_htPLCAlarmNewInform = new Hashtable();                          //获取到的最新PLC报警信息
        private ArrayList arPLCOldAlarmMsg = new ArrayList();     

        private BackgroundWorker bg_PLCWorker = null;

        public event PLCAlaramEventHandler OnPLCAlarmMessageChanged;
        public event PLCMessageEventHandler OnPLCLogMessageChanged;

        public PLCAlarmOperation(PLCConnectCollection plcConnects, DataTable dtPLCAlarmData)
        {
            m_PLCConnects = plcConnects;
            m_dtPLCAlarmData = dtPLCAlarmData;
        }

        /// <summary>
        /// 开始监控报警信息
        /// </summary>
        public void StartMonitorPLCAlarm()
        {
            bg_PLCWorker = new BackgroundWorker();
            bg_PLCWorker.WorkerSupportsCancellation = true;
            bg_PLCWorker.DoWork += new DoWorkEventHandler(bg_PLCWorker_DoWork);
            if (bg_PLCWorker.IsBusy)
            {
                bg_PLCWorker.CancelAsync();
                System.Threading.Thread.Sleep(1500);
            }
            bg_PLCWorker.RunWorkerAsync();           
        }

        /// <summary>
        /// 停止监控报警信息
        /// </summary>
        public void StopMonitorPLCAlarm()
        {
            if (bg_PLCWorker != null)
            {
                bg_PLCWorker.DoWork -= new DoWorkEventHandler(bg_PLCWorker_DoWork);
                bg_PLCWorker.CancelAsync();
            }
            arPLCOldAlarmMsg.Clear();
        }

        private void bg_PLCWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                try
                {
                    if (bg_PLCWorker.CancellationPending) return;
                    GetPLCAlarmInfo();
                }
                catch (System.Exception ex)
                {
                    SendPLCLogMessage("<PLCWorker_DoWork>出错,原因:" + ex.Message);
                }
                System.Threading.Thread.Sleep(500);
            }
        }

        /// <summary>
        /// 按DB块一次性获取所有报警代码,然后一次性查询
        /// </summary>
        private void GetPLCAlarmInfo()
        {
            string strBinMsg = "";
            byte[] byteBuffer = null;
            if (m_dtPLCAlarmData.Rows.Count == 0) return;
            
            try
            {

                //(1) 读取PLC数据，并将其存放至哈希表中
                foreach (PLCConnItem plcConnect in m_PLCConnects)
                {
                    strBinMsg = "";
                    m_strGongweiType = plcConnect.PLCStationName;
                    m_strMobanNO = plcConnect.PLCTemplateName;

                    if (plcConnect.PLCHelper.Connected)
                    {
                        plcConnect.PLCHelper.ReadData(plcConnect.DBUnit, plcConnect.DWNO, plcConnect.Amount, ref byteBuffer);
                        plcConnect.ByteBuffer = byteBuffer;
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(400);
                        plcConnect.PLCHelper.ReConnectPLC();
                    }

                    //获取0,1二进制串
                    foreach (byte byt in plcConnect.ByteBuffer)
                    {
                        strBinMsg = strBinMsg + DataConverter.DecToBin(byt.ToString());
                    }
                    string strKey = string.Format("{0}|{1}", m_strGongweiType, m_strMobanNO);   //以工位|模板为主键
                    if (m_htPLCAlarmNewInform.Contains(strKey))
                        m_htPLCAlarmNewInform[strKey] = strBinMsg;
                    else
                        m_htPLCAlarmNewInform.Add(strKey, strBinMsg);
                }

                //(2) 根据二进制串获取对应的报警消息
                GetPLCAlaramMsg();
            }
            catch (System.Exception ex)
            {
                SendPLCLogMessage("<GetPLCAlarmInfo>出错,原因:" + ex.Message);
            }
        }

        /// <summary>
        /// 根据0,1二进制字符串报警代码获取对应的报警信息
        /// </summary>
        private void GetPLCAlaramMsg()
        {
            ArrayList arPLCAlarmMsg = new ArrayList();
            DataRow[] currRows= null;
            string strErrNO = "",strErrMsg = "";

            arPLCAlarmMsg.Clear();
            foreach (DictionaryEntry di in m_htPLCAlarmNewInform)
            {
                //如果当前DB块中无报警信息
                if (di.Value.ToString().IndexOf('1') == -1) continue;

                //SendSysErrMessage(di.Key.ToString()+":"+  currPLCAlaramInfo);       //for test

                for (int i = 0; i < di.Value.ToString().Length; i++)
                {
                    if (di.Value.ToString().Substring(i, 1) != "1") continue;

                    strErrNO = string.Format("{0}", i.ToString().PadLeft(3, '0'));
                    currRows = m_dtPLCAlarmData.Select(string.Format("ErrorNO='{0}' AND GongweiType='{1}' AND MobanNO='{2}'",
                                    strErrNO, di.Key.ToString().Split('|')[0], di.Key.ToString().Split('|')[1]));
                    if (currRows.Length > 0)
                    {
                        strErrMsg = string.Format("{0}|{1}|{2}|{3}|{4}|{5}",
                                                currRows[0]["ErrorDesc"],
                                                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                                currRows[0]["ErrorNO"],
                                                currRows[0]["ErrorAddr"],
                                                currRows[0]["GongweiType"],
                                                currRows[0]["MobanNO"]);
                        arPLCAlarmMsg.Add(strErrMsg);
                    }
                }
            }

            //一次性发送所有信息
            SendAlarmMsg(arPLCAlarmMsg);
        }

        /// <summary>
        /// 发送所有报警消息
        /// </summary>
        /// <param name="arPLCAlarmMsg"></param>
        private void SendAlarmMsg(ArrayList arPLCAlarmMsg)
        {
            if (arPLCAlarmMsg.Count != arPLCOldAlarmMsg.Count)
            {
                arPLCOldAlarmMsg.Clear();
                arPLCOldAlarmMsg.AddRange(arPLCAlarmMsg);

                //统一发送所有报警信息
                SendPLCAlarmMessage(arPLCAlarmMsg);
            }
            else
            {
                //比较采集的PLC报警信息是否一致，如果不一致，就需要更新
                bool blnNeedUpdate = false;
                string[] strCurrInfo;
                string[] strOldInfo;

                for (int i = 0; i < arPLCAlarmMsg.Count; i++)
                {
                    //比较报警信息，编号，地址，模板
                    strCurrInfo = arPLCAlarmMsg[i].ToString().Split('|');
                    strOldInfo = arPLCOldAlarmMsg[i].ToString().Split('|');
                    if (strCurrInfo.Length > 5)
                    {
                        if ((strCurrInfo[0] != strOldInfo[0]) ||
                            (strCurrInfo[2] != strOldInfo[2]) || 
                            (strCurrInfo[3] != strOldInfo[3]) || 
                            (strCurrInfo[5] != strOldInfo[5]))
                        {
                            blnNeedUpdate = true;
                            break;
                        }
                    }
                }
                //统一发送所有报警信息
                if (blnNeedUpdate) SendPLCAlarmMessage(arPLCAlarmMsg);
                arPLCOldAlarmMsg.Clear();
                arPLCOldAlarmMsg.AddRange(arPLCAlarmMsg);
            }
        }

        /// <summary>
        /// 发送PLC报警信息
        /// </summary>
        /// <param name="strMsg"></param>
        private void SendPLCAlarmMessage(System.Collections.ArrayList arPLCAlarmMsgCollects)
        {
            if (arPLCAlarmMsgCollects != null)
            {
                PLCAlaramEventArgs args = new PLCAlaramEventArgs(arPLCAlarmMsgCollects);
                if (OnPLCAlarmMessageChanged != null)
                    OnPLCAlarmMessageChanged(this, args);
            }
        }

        /// <summary>
        /// 发送PLC运行日志信息
        /// </summary>
        /// <param name="strLogMsg"></param>
        private void SendPLCLogMessage(string strLogMsg)
        {
            if (OnPLCLogMessageChanged != null)
            {
                OnPLCLogMessageChanged(this, new PLCEventArgument(true, strLogMsg));
            }
        }

        #region 释放资源
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Part of basic design pattern for implementing Dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                StopMonitorPLCAlarm();
            }
        }
        #endregion

    }
}
