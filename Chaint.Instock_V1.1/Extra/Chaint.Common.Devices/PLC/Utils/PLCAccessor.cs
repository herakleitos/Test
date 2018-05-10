using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Chaint.Common.Devices.PLC.Utils
{
/*-----------------------------------------------------------------------------------
 * 作者: Automation&IT Dept. 
 * 
 * 创建时间: 2015-05-21
 * 
 * 功能描述: 
 *      PLC信号访问器
 * 
 ------------------------------------------------------------------------------------*/
    public class PLCAccessor
    {
        public event PLCMessageEventHandler OnPLCMessageChanged;

        private ThreadSafeQueue<PLCObject> m_lstPLCDataToWrited = new ThreadSafeQueue<PLCObject>();

        private PLCSignalCollection m_PLCSignals = new PLCSignalCollection();                  //PLC信号地址表
        private PLCConnectCollection m_PLCConnects = new PLCConnectCollection();                         //PLC连接集合

        private Semaphore m_PlcSemphore = new Semaphore(0, 1);          //定义01信号量,仅允许一个线程使用 用于同步

        private enum LogStatus { Inform, Error, Warn };

        private Thread thdPLCAccess = null;
        private bool m_IsStarted = false;


        /// <summary>
        /// PLC信号集合
        /// </summary>
        public PLCSignalCollection PLCSignals
        {
            get { return m_PLCSignals; }
        }

        /// <summary>
        ///  PLC连接器集合
        /// </summary>
        public PLCConnectCollection PLCConnects
        {
            get { return m_PLCConnects; }
        }

     
        /// <summary>
        /// 初始化PLC连接参数以及PLC信号量
        /// </summary>
        /// <param name="dtPlcConnects">PLC_Connect</param>
        /// <param name="dtPlcSiganls">PLC_Config</param>
        public void InitalPLCParams(DataTable dtPlcConnects, DataTable dtPlcSiganls)
        {
            try
            {
                //准备PLC通讯地址
                foreach (DataRow currRow in dtPlcSiganls.Rows)
                {
                    PLCSignalItem plcSignal = new PLCSignalItem();
                    plcSignal.PLCStationName = currRow["GongweiType"].ToString();   //注意PLC_Config数据表中的字段名称
                    plcSignal.PLCTemplateName = currRow["MobanNO"].ToString();
                    plcSignal.SignalName = currRow["SignalName"].ToString();
                    plcSignal.DBUnit = int.Parse(currRow["DBUnit"].ToString());
                    plcSignal.DWNO = int.Parse(currRow["DWNO"].ToString());
                    plcSignal.Amount = int.Parse(currRow["Amount"].ToString());
                    m_PLCSignals.AddPLCSignalItem(plcSignal);
                }

                //准备PLC连接
                foreach (DataRow currRow in dtPlcConnects.Rows)
                {
                    PLCConnItem plcItem = new PLCConnItem();
                    plcItem.PLCStationName = currRow["GongweiType"].ToString();
                    plcItem.PLCTemplateName = currRow["MobanNO"].ToString();

                    if (currRow["PLCType"].ToString().Trim().Length > 0)
                    {
                        plcItem.PlcType = (PLCType)Enum.Parse(typeof(PLCType), currRow["PLCType"].ToString().Trim());
                    }
                    else
                    {
                        SendMessage("PLC类型未定义，系统默认类型为<PLC_6D0>", LogStatus.Error);
                        plcItem.PlcType = PLCType.PLC_6D0;
                    }

                    plcItem.Address = currRow["Address"].ToString();
                    if (plcItem.PlcType == PLCType.PLC_6D0)
                    {
                        plcItem.Address = currRow["IPAddress"].ToString();
                    }
                    plcItem.SegmentNO = Int16.Parse(currRow["SegmentID"].ToString());
                    plcItem.ConnNr = ushort.Parse(currRow["ConnNr"].ToString());
                    plcItem.SlotNO = byte.Parse(currRow["SlotNo"].ToString());
                    plcItem.RackNO = byte.Parse(currRow["RackNO"].ToString());
                    plcItem.DWNO = int.Parse(currRow["DWNO"].ToString());
                    plcItem.Amount = int.Parse(currRow["MaxBytesCount"].ToString());
                    plcItem.ByteBuffer = new byte[plcItem.Amount];
                    plcItem.DBUnit = ushort.Parse(currRow["DBUnit"].ToString());

                    IPLCOperation plcHelper = PLCFactory.CreatePLC(plcItem);
                    plcHelper.OnPLCMessageChanged += new PLCMessageEventHandler(plcHelper_OnPLCMessageChanged);
                    plcHelper.ConnectPLC();
                    plcItem.PLCHelper = plcHelper;
                    m_PLCConnects.AddPLCConnItem(plcItem);

                    
                }
            }
            catch (System.Exception ex)
            {
                SendMessage("PLC初始化出错,原因:"+ex.Message,LogStatus.Error);
            }
        }

        public void InitalPLCParams(PLCConnectCollection plcConnects, PLCSignalCollection plcSignals,Chaint.Common.Devices.Data.DBAccessFunc dbAccessor)
        {
            try
            {
                //准备PLC通讯地址
                m_PLCSignals = plcSignals;

                if(plcConnects==null || plcConnects.Count==0)
                {
                    throw new Exception("PLC连接为空或者未初始化!");
                }

                //准备PLC连接
                foreach (PLCConnItem plcItem in plcConnects)
                {
                    //IPLCOperation plcHelper = PLCFactory.CreatePLC(plcItem);
                    IPLCOperation plcHelper = PLCFactory.CreatePLC(plcItem,dbAccessor);     //用于测试
                    plcHelper.OnPLCMessageChanged += new PLCMessageEventHandler(plcHelper_OnPLCMessageChanged);
                    plcHelper.ConnectPLC();
                    plcItem.PLCHelper = plcHelper;
                    m_PLCConnects.AddPLCConnItem(plcItem);
                }
            }
            catch (System.Exception ex)
            {
                SendMessage("PLC初始化出错,原因:" + ex.Message, LogStatus.Error);
            }
        }

        private void plcHelper_OnPLCMessageChanged(object sender, PLCEventArgument args)
        {
            SendMessage(args.RetMessage, LogStatus.Inform);
        }

        /// <summary>
        /// 开始PLC访问
        /// </summary>
        public void StartPLCAccess()
        {
            if (!m_IsStarted)
            {
                m_IsStarted = true;

                thdPLCAccess = new Thread(new System.Threading.ThreadStart(ExecutePLCReadAndWrite));
                thdPLCAccess.Start();

                m_PlcSemphore.Release();    //初始值
            }
        }


        /// <summary>
        /// 结束PLC访问
        /// </summary>
        public void StopPLCAccess()
        {
            if (thdPLCAccess != null && thdPLCAccess.IsAlive)
            {
                thdPLCAccess.Join(500);
                thdPLCAccess.Abort();
                thdPLCAccess = null;
            }
            m_IsStarted = false;
        }


        /// <summary>
        /// 执行PLC的读取操作
        /// </summary>
        private void ExecutePLCReadAndWrite()
        {
            while (thdPLCAccess != null && thdPLCAccess.IsAlive)
            {
                try
                {   
                    m_PlcSemphore.WaitOne();        //等待获取

                    ReadPLCData();

                    WritePLCData();
                }
                catch (Exception ex)
                {
                    SendMessage(ex.Message,LogStatus.Error);
                }

                m_PlcSemphore.Release();        //释放

                Thread.Sleep(300);
            }
        }

        /// <summary>
        /// 读取PLC数据
        /// </summary>
        private void ReadPLCData()
        {
            if (PLCConnects == null || PLCConnects.Count == 0) return;

            byte[] bytBuffer;
            foreach (PLCConnItem plcItem in PLCConnects)
            {
                if (plcItem == null || plcItem.PLCHelper == null)
                {
                    continue;
                }
                if (plcItem.PLCHelper.Connected)
                {
                    bytBuffer = new byte[plcItem.ByteBuffer.Length];
                    bool blnSucc = plcItem.PLCHelper.ReadData(plcItem.DBUnit, plcItem.DWNO, plcItem.ByteBuffer.Length, ref bytBuffer);
                    if (blnSucc) plcItem.ByteBuffer = bytBuffer;
                }
                else
                {
                    System.Threading.Thread.Sleep(1000);
                    SendMessage("PLC正在重连", LogStatus.Warn);
                    plcItem.PLCHelper.ReConnectPLC();
                }
            }
        }

        /// <summary>
        /// 写PLC数据
        /// </summary>
        private void WritePLCData()
        {
            if (PLCConnects == null || PLCConnects.Count == 0) return;

            try
            {
                int itemCount = m_lstPLCDataToWrited.Count;
                if (itemCount == 0) return;

                PLCObject currPlcObj = null;
                IPLCOperation currPlcConnector = null;

                for (int i = 0; i < itemCount; i++)
                {
                    currPlcObj = m_lstPLCDataToWrited.PopObject();
                    if (currPlcObj == null) continue;

                    //当没有涉及到PLC工位或者模板值时，系统默认取第一个连接  此处一定要注意如加载多个PLC时一定要用工位与模板区分,如只有一个CPU就无关仅要了
                    if (currPlcObj.PLCStationName == "" && currPlcObj.PLCTemplateName == "")
                        currPlcConnector = m_PLCConnects[0].PLCHelper;
                    else
                        currPlcConnector=PLCConnects[currPlcObj.PLCStationName, currPlcObj.PLCTemplateName].PLCHelper;
                   
                    if (currPlcConnector == null)
                    {
                        SendMessage(string.Format("◆ 不存在对应站类型与模板名的PLC连接<{0}:{1}>", currPlcObj.PLCStationName, currPlcObj.PLCTemplateName), LogStatus.Error);
                        continue;
                    }

                    bool blnSucc = currPlcConnector.WriteData(currPlcObj.DBUnit, currPlcObj.DWNO, currPlcObj.Amount, currPlcObj.BytsData);
                    if (blnSucc)
                    {
                        SendMessage(string.Format("【Snd】: {0}/{1}/{2},Value:{3}", 
                            currPlcObj.DBUnit, currPlcObj.DWNO, currPlcObj.Amount,
                            DataConverter.FormatByts(currPlcObj.BytsData, 0, currPlcObj.Amount)), 
                            LogStatus.Inform);
                    }
                    else
                    {
                        SendMessage(string.Format("◆ PLC信号<{0}/{1}/{2},Value:{3}>写入失败,正在等待PLC重连…",
                            currPlcObj.DBUnit, currPlcObj.DWNO, currPlcObj.Amount,
                            DataConverter.FormatByts(currPlcObj.BytsData, 0, currPlcObj.Amount)), 
                            LogStatus.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                SendMessage("WritePLCData 出错,原因:" + ex.Message, LogStatus.Error);
            }
        }

        /// <summary>
        /// 日志信息发送
        /// </summary>
        /// <param name="strMsg"></param>
        /// <param name="LogStatus">日志状态:</param>
        private void SendMessage(string strMsg, LogStatus status)
        {
            if (OnPLCMessageChanged != null) OnPLCMessageChanged(this, new PLCEventArgument(status == LogStatus.Inform ? true : false, strMsg));
        }


        #region 读取PLC数据,从已获取到的字节数组中
        /// <summary>
        /// 读取某个PLC值
        /// </summary>
        /// <param name="plcSignal">plc信号实体</param>
        /// <returns></returns>
        public static string ReadPLCSignalValue(PLCSignalItem plcSignal,byte[] plcBytsData)
        {
            if (plcSignal == null || plcBytsData == null || plcBytsData.Length < 1) return "";

            return ReadPLCSignalValue(plcSignal.DWNO, plcSignal.Amount, plcBytsData);
        }

        /// <summary>
        /// 从PLC字节数组中截取某个数据
        /// </summary>
        /// <param name="dwno">初始地址 索引</param>
        /// <param name="amount">字节数量</param>
        /// <param name="plcBytsData">对应字节数组</param>
        /// <returns></returns>
        public static string ReadPLCSignalValue(int dwno,int amount, byte[] plcBytsData)
        {
            if (plcBytsData.Length < 1) return "";

            string strRetValue = "";
            if (amount <= 4)
                strRetValue = string.Format("{0}", DataConverter.ByteToLong(plcBytsData, dwno, dwno + amount - 1));
            else
            {
                //如读取扫描条码的情况,开始与结束字符在控件已做处理
                strRetValue = DataConverter.BytesToString(plcBytsData, 0, amount);

                strRetValue = strRetValue.Trim(new char[] { (char)2, (char)3, (char)4, (char)5 });//去掉开始与结束字符
            }
            return strRetValue;
        }

        #endregion

        #region 写PLC数据

        /// <summary>
        /// 发送PLC信号(单个PLC信号值)
        /// </summary>
        /// <param name="plcSignal">plc信号地址</param>
        /// <param name="intValue">对应的值</param>
        /// <returns></returns>
        public bool WritePLCSignalValue(PLCSignalItem plcSignal,int intValue)
        {
            if (plcSignal == null) return false;

            byte[] plcBytsData = DataConverter.IntToByte(intValue, plcSignal.Amount);
            return WritePLCSignalValue(plcSignal.PLCStationName, 
                                        plcSignal.PLCTemplateName, 
                                        plcSignal.DBUnit, 
                                        plcSignal.DWNO, 
                                        plcSignal.Amount, 
                                        plcBytsData);
        }

        /// <summary>
        /// 当对应只加载一个CPU时，不需要工位名称与工位模板,
        /// 但涉及多个CPU时，请注意不要用此函数
        /// </summary>
        /// <param name="dbunit"></param>
        /// <param name="dwno"></param>
        /// <param name="amount"></param>
        /// <param name="plcBytsData"></param>
        /// <returns></returns>
        public bool WritePLCSignalValue(int dbunit, int dwno, int amount, byte[] plcBytsData)
        {
            return WritePLCSignalValue("", "", dbunit, dwno, amount, plcBytsData);
        }

        /// <summary>
        /// 发送PLC信号  可以对应连续的PLC字节数组
        /// </summary>
        /// <param name="strPlcStationName">PLC工位</param>
        /// <param name="strPlcTemplateName">PLC模板</param>
        /// <param name="dbunit">db单元</param>
        /// <param name="dwno">初始地址</param>
        /// <param name="amount">字节数量</param>
        /// <param name="plcBytsData">字节数组</param>
        /// <returns></returns>
        public bool WritePLCSignalValue(string strPlcStationName,string strPlcTemplateName,int dbunit,int dwno,int amount,byte[] plcBytsData)
        {
            PLCObject plcObj = new PLCObject(strPlcStationName, strPlcTemplateName, dbunit, dwno, amount, plcBytsData);
            bool blnSucc=m_lstPLCDataToWrited.PushObject(plcObj);
            return blnSucc;
        }

        /// <summary>
        /// 向PLC一次性发送连续的多个PLC数据,其中plcObject中的PLC对象的信号值在数组中
        /// 注意：此列表中的多个对象的PLC地址是连续的,如不连续，系统自动分成多次发送
        /// </summary>
        /// <param name="plcObjects">PLC对象，注意：此列表中的多个对象的PLC地址是连续的</param>
        /// <returns></returns>
        public bool WritePLCSignalValue(List<PLCObject> plcObjects)
        {
            if (plcObjects == null || plcObjects.Count == 0) return false;

            //(1) 先获取PLC分组
            Dictionary<int, PLCObject[]> plcSignalGroups = GetPlcSignalGroups(plcObjects);

            //(2) 根据分组再发送PLC数据
           
            PLCObject plcSndSignals = new PLCObject(); //临时变量
            for (int i = 0; i < plcSignalGroups.Count;i++ )
            {
                //获取每组第一个对象的地址
                plcSndSignals.PLCStationName = plcSignalGroups[i][0].PLCStationName;
                plcSndSignals.PLCTemplateName = plcSignalGroups[i][0].PLCTemplateName;
                plcSndSignals.DBUnit = plcSignalGroups[i][0].DBUnit;
                plcSndSignals.DWNO = plcSignalGroups[i][0].DWNO;
                plcSndSignals.Amount = GetTotalBytsCount(plcSignalGroups[i]);
                plcSndSignals.BytsData = GetTotalBytBuffer(plcSignalGroups[i]);

                //发送某个分组的PLC数据
                WritePLCSignalValue(plcSndSignals.PLCStationName,
                                    plcSndSignals.PLCTemplateName,
                                    plcSndSignals.DBUnit,
                                    plcSndSignals.DWNO,
                                    plcSndSignals.Amount, 
                                    plcSndSignals.BytsData);
            }

            return true;
        }

        public bool WritePLCSignalValue(List<PLCSignalItem> plcSignalItems)
        {
            if (plcSignalItems == null || plcSignalItems.Count == 0) return false;
            List<PLCObject> plcObjects = new List<PLCObject>();

            for(int i=0;i<plcSignalItems.Count;i++)
            {
                PLCObject plcObj = new PLCObject(
                                    plcSignalItems[i].PLCStationName,
                                    plcSignalItems[i].PLCTemplateName, 
                                    plcSignalItems[i].DBUnit, 
                                    plcSignalItems[i].DWNO, 
                                    plcSignalItems[i].Amount,
                                    DataConverter.IntToByte(int.Parse(plcSignalItems[i].SignalValue), plcSignalItems[i].Amount));

                plcObjects.Add(plcObj);
            }

            return WritePLCSignalValue(plcObjects); 
        }

        public PLCSignalItem ParsePLCSignalItem(string strPLCStationName,string strPLCTemplateName,string strSignalName,string strSignalValue)
        {
            PLCSignalItem plcItem = m_PLCSignals[strPLCStationName, strPLCTemplateName, strSignalName];
            if (plcItem == null)
                return null;
            else
                plcItem.SignalValue = strSignalValue;
            return plcItem;
        }

        /// <summary>
        /// 将当前PLC信号转换成PLCObject对象
        /// </summary>
        /// <param name="strPLCStationName">工位名称</param>
        /// <param name="strPLCTemplateName">模板名称</param>
        /// <param name="strSignalName">信号名称</param>
        /// <param name="strSignalValue">信号值</param>
        /// <returns></returns>
        public PLCObject ParsePLCObject(string strPLCStationName,string strPLCTemplateName,string strSignalName,string strSignalValue)
        {
            PLCSignalItem plcItem = ParsePLCSignalItem(strPLCStationName, strPLCTemplateName, strSignalName, strSignalValue);
            if (plcItem == null) 
                return null;
            else
            {
                return new PLCObject(plcItem.PLCStationName, plcItem.PLCTemplateName,
                                     plcItem.DBUnit, plcItem.DWNO, plcItem.Amount,
                                     DataConverter.IntToByte(int.Parse(plcItem.SignalValue), plcItem.Amount));
            }
        }


        /// <summary>
        /// 获取PLC信号的所有分组  已测试
        /// </summary>
        /// <param name="plcObjects"> PLC信号对象集合</param>
        /// <returns></returns>
        private Dictionary<int, PLCObject[]> GetPlcSignalGroups(List<PLCObject> plcObjects)
        {
            plcObjects.Sort(new PlcObjectCompare());

            Dictionary<int, PLCObject[]> plcSignalGroups = new Dictionary<int, PLCObject[]>();

            int groupID = 0;
            int currDwno = 0;
            bool isNeedNewGroup = false;



            List<PLCObject> currPlcSignalGroup = new List<PLCObject>();
            for (int i = 0; i < plcObjects.Count; i++)
            {
                isNeedNewGroup = false;

                if (i == 0)
                {
                    currPlcSignalGroup.Add(plcObjects[i]);

                    if (i == plcObjects.Count - 1) isNeedNewGroup = true;

                }
                else
                {
                    currDwno = plcObjects[i - 1].DWNO + plcObjects[i - 1].Amount;
                    if (currDwno == plcObjects[i].DWNO)
                    {
                        currPlcSignalGroup.Add(plcObjects[i]);

                        if (i == plcObjects.Count - 1) isNeedNewGroup = true;
                    }
                    else
                    {

                        plcSignalGroups.Add(groupID, currPlcSignalGroup.ToArray());
                        groupID++;


                        currPlcSignalGroup.Clear();
                        currPlcSignalGroup.Add(plcObjects[i]);

                        if (i == plcObjects.Count - 1) isNeedNewGroup = true;
                    }
                }

                if (isNeedNewGroup)
                {

                    plcSignalGroups.Add(groupID, currPlcSignalGroup.ToArray());
                    groupID++;
                    currPlcSignalGroup.Clear();
                }
            }
            return plcSignalGroups;
        }


        /// <summary>
        /// 获取某一组PLC信号的总的字节数量
        /// </summary>
        /// <param name="plcObjects"></param>
        /// <returns></returns>
        private int GetTotalBytsCount(PLCObject[] plcObjects)
        {
            if (plcObjects.Length == 0) return 0;

            return (plcObjects[plcObjects.Length - 1].DWNO + plcObjects[plcObjects.Length - 1].Amount - plcObjects[0].DWNO);
        }
   
        /// <summary>
        /// 获取每个分组对应的字节数组(注意PLCObject中取的是每个信号的数组值)
        /// </summary>
        /// <param name="plcObjects"></param>
        /// <returns></returns>
        private byte[] GetTotalBytBuffer(PLCObject[] plcObjects)
        {
            int bytCount = GetTotalBytsCount(plcObjects);

            if (bytCount == 0) return null;

            IList<byte> lstBuffers = new List<byte>();

            byte[] bytRetValues = new byte[bytCount];

            for(int i=0;i<plcObjects.Length;i++)
            {
                for(int j=0;j<plcObjects[i].BytsData.Length;j++)
                {
                    lstBuffers.Add(plcObjects[i].BytsData[j]);
                }
            }

            lstBuffers.CopyTo(bytRetValues, 0);
            return bytRetValues;
        }


        #endregion


    }
}
