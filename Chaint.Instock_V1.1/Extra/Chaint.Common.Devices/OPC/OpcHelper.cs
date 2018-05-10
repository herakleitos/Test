using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Collections;
using System.Data;
using System.Windows.Forms;

using HaiGrang.Package.OpcNetApiChs;
using HaiGrang.Package.OpcNetApiChs.Opc;
using HaiGrang.Package.OpcNetApiChs.Da;
using HaiGrang.Package.OpcNetApiChs.DaNet;
using HaiGrang.Package.OpcNetApiChs.Common;


/*此OPC  客户端连接OPC服务器类用到的资源及功能有：
 * (1)　引用了动态链接库 OPCNetApiChs.Dll;
 * (2)  定义的功能有：连接OPC服务器、断开与OPC服务器的连接、读取指定Item的值、将值写入指定Item;
 * (3)  错误信息的返回依靠定义的OnErrorMessage 事件，使用此类的用户只需要订制此事件即可;
 * (4)  与OPC服务器通信的状态信息依靠定义的OnRetMessage,使用此类的用户只需要订制此事件即可;
 * (5)  定义的事件OnOPCRetSignalVal主要用于返回当前Item值发生变化的信息，此处用不上，可以不用订制;
 * 
 * (6)  对于不同的OPC服务器，可能使用的Item定义名称的方法有些不同，比如：对于KEPware.KEPServerEx.V4,
 *      其定义方式为：Channel_1.Device_1.Chaint.ItemName(通道名.设备名.组名.项目名)
 *      而OPC.SimaticNET 的名称为：S7:[Connection_1]:ItemName
 * (7)  如果不存在的信号名称，需要事先在PLC_Config中定义;
 * */

namespace Chaint.Common.Devices.OPC
{
    public class OpcHelper
    {
        #region 变量定义
        private string m_ItemNamePrefix = "S7:[S7 Connection_2]";
        private string m_GroupName = "Chaint";

        private string m_OPCServerName = "";        //OPC服务器名称
        private Host m_OPCSrvHostInform = null;       //OPC服务器所在PC对应的主机信息

        private string m_RemoteServerIP = "";
        private string m_HostName = "localhost";    //OPC服务器所在PC的主机名
        private string m_UserName = "";             //OPC服务器所在PC的登录用户名
        private string m_UserPWD = "";              //OPC服务器所在PC的登录用户密码

        private OpcServer m_OPCServer = null;
        private SyncIOGroup m_SynGroup = null;
        private int m_RefreshRate = 500;
        private bool m_OPCConnected = false;

        private Hashtable m_htOPCItemName = new Hashtable();        //由调用此类程序传入OPC服务器中指定组的Item名称信息,Key:名称(ItemName),Value:对应值(ItemValue)
        private Hashtable m_htOPCLatestItemInform = new Hashtable();    //存放最新的ItemID与ItemValue值的对照表
        private Timer tmrMonitor = new Timer();
        private System.ComponentModel.BackgroundWorker ReadOPCInformWorker = new System.ComponentModel.BackgroundWorker();
        #endregion

        #region 属性定义
        public string OPCServerName
        {
            set { m_OPCServerName = value; }
        }

        /// <summary>
        /// 需要在OPC服务器添加的组对象名称
        /// </summary>
        public string OPCGroupName
        {
            set { m_GroupName = value; }
            get { return m_GroupName; }
        }

        public bool Connected
        {
            get { return m_OPCConnected; }
        }

        /// <summary>
        /// OPC 服务器组对象的更新率
        /// </summary>
        public int UpdateRate
        {
            set { m_RefreshRate = value; }
        }
        /// <summary>
        /// 最新PLC数据
        /// </summary>
        public Hashtable OPCLatestItemInform
        {
            set { m_htOPCLatestItemInform = value; }
            get { return m_htOPCLatestItemInform; }
        }

        #endregion

        #region 自定义事件
        public delegate void MsgEventHandler(object sender, MsgEventArgs e);
        public delegate void SignalInformHandler(object sender, CItemInformEventArgs e);

        public event MsgEventHandler OnErrorMessage;        //错误消息　
        public event MsgEventHandler OnRetOPCStatus;          //返回信息
        public event SignalInformHandler OnRetOPCItemInform; //OPC服务器端返回的信号值

        public class MsgEventArgs : EventArgs
        {
            public MsgEventArgs(string strMsg)
            {
                m_msg = strMsg;
            }
            private readonly string m_msg = "";
            public string RetMsg
            {
                get { return m_msg; }
            }
        }
        #endregion

        #region 定义返回的Item信息
        public class CItemInformEventArgs : EventArgs
        {
            public CItemInformEventArgs()
            { }
            public string GroupName = "";
            public string ItemName = "";            //项简称
            public string ItemValue = "";          //项值
            public string ItemQuality = "";         //项质量
            public string ItemTimeStamp = "";            //时间戳对应的具体时间
            public string ItemClientHandler = "";        //项对应的ClientHandler
            public string ItemServerHandler = "";       //项对应ServerHandler 
            public string ItemID = "";           //项全称
        }
        #endregion

        /// <summary>
        /// 以行集形式接收Item信息
        /// </summary>
        /// <param name="drItemInformation"></param>
        public OpcHelper(DataTable dtItemInformation)
        {
            ReadOPCInformWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(ReadOPCInformWorker_DoWork);
            ReadOPCInformWorker.WorkerSupportsCancellation = true;

            //定义实时监控OPC中ItemID值的变化情况
//             tmrMonitor.Interval = 800;
//             tmrMonitor.Tick += new EventHandler(tmrMonitor_Tick);

            //读取本地的IP地址
            IPHostEntry iphost = Dns.GetHostEntry(Environment.MachineName);
            try
            {
                if (iphost.AddressList.Length > 0)
                {
                    m_RemoteServerIP = iphost.AddressList[0].ToString();
                    m_HostName = Dns.GetHostEntry(m_RemoteServerIP).HostName;
                    Host host = new Host(m_HostName);
                    host.Domain = m_HostName.ToUpper();
                    host.HostName = m_HostName;
                    host.UserName = m_UserName;
                    host.Password = m_UserPWD;
                    m_OPCSrvHostInform = host;
                }
                if (dtItemInformation.Rows.Count > 0)
                {
                    foreach (DataRow row in dtItemInformation.Rows)
                    {
                        m_htOPCItemName.Add(row["ItemID"], "");
                        m_htOPCLatestItemInform.Add(row["ItemID"], "");
                    }
                }
                else
                {
                    SendErrMessage("测点项信息为空，请在数据表PLC_Config中添加!");
                }
            }
            catch (Exception ex)
            {
                SendErrMessage("创建OPC客户端类出错，原因:" + ex.Message);
            }
        }

        /// <summary>
        /// 以哈希表形式接收Item信息,Item名称信息,Key:名称(ItemName),Value:对应值(ItemValue)
        /// </summary>
        /// <param name="ItemInformation"></param>
        public OpcHelper(Hashtable ItemInformation)
        {
            ReadOPCInformWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(ReadOPCInformWorker_DoWork);
            ReadOPCInformWorker.WorkerSupportsCancellation = true;

            //定义实时监控OPC中ItemID值的变化情况
//             tmrMonitor.Interval = 800;
//             tmrMonitor.Tick += new EventHandler(tmrMonitor_Tick);

            //读取本地的IP地址
            IPHostEntry iphost = Dns.GetHostEntry(Environment.MachineName);
            try
            {
                if (iphost.AddressList.Length > 0)
                {
                    m_RemoteServerIP = iphost.AddressList[0].ToString();
                    m_HostName = Dns.GetHostEntry(m_RemoteServerIP).HostName;
                    Host host = new Host(m_HostName);
                    host.Domain = m_HostName.ToUpper();
                    host.HostName = m_HostName;
                    host.UserName = m_UserName;
                    host.Password = m_UserPWD;
                    m_OPCSrvHostInform = host;
                }
                m_htOPCItemName = ItemInformation;
                m_htOPCLatestItemInform = ItemInformation;
                if (m_htOPCItemName.Count<1)
                {
                    SendErrMessage("测点项信息为空，请在数据表PLC_Config中添加!");
                }

            }
            catch (Exception ex)
            {
                SendErrMessage("创建OPC客户端类出错，原因:" + ex.Message);
            }
        }

        private void ReadOPCInformWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            while (!ReadOPCInformWorker.CancellationPending)
            {
                MonitorAllItemIDValues();
                System.Threading.Thread.Sleep(500);
            }
        }

        private void MonitorAllItemIDValues()
        {
            string strItemID = "";
            string strItemValue = "";
            bool blnRet = false;
            try
            {
                foreach (DictionaryEntry obj in m_htOPCItemName)
                {
                    strItemID = string.Format("{0}", obj.Key);
                    blnRet = ReadItemIDValue(strItemID, ref strItemValue);
                    if (blnRet)
                    {
                        if (strItemValue != m_htOPCLatestItemInform[obj.Key].ToString())
                        {
                            m_htOPCLatestItemInform[obj.Key] = strItemValue;
                            CItemInformEventArgs args = new CItemInformEventArgs();
                            args.GroupName = m_GroupName;
                            args.ItemName = strItemID.Split(']')[1];
                            args.ItemID = strItemID;
                            args.ItemClientHandler = m_SynGroup.ClientHandle.ToString();
                            args.ItemServerHandler = "0";
                            args.ItemValue = strItemValue;
                            args.ItemQuality = "Good";
                            args.ItemTimeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            SendOPCItemInform(args);
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            catch (System.Exception ex)
            {
                //Console.WriteLine(string.Format("监控信号<{0}> 出错,原因:{1}", strItemID,ex.Message));
                SendErrMessage(String.Format("监控信号<{0}> 出错,原因:{1}", strItemID, ex.Message));
            }
        }

        /// <summary>
        /// 连接OPC服务器
        /// </summary>
        /// <param name="strOPCServerName">OPC服务器名称</param>
        /// <returns></returns>
        public bool ConnectOPCServer(string strOPCServerName)
        { 
            m_OPCConnected = false;
            OpcServerBrowser opcbrowser = new OpcServerBrowser(m_HostName);
            Guid srvGuid;
            try
            {
                //连接OPC服务器
                SendOPCStatusInform("正在连接<OPC>服务器…");
                opcbrowser.CLSIDFromProgID(strOPCServerName, out srvGuid);
                m_OPCServer = new OpcServer();
                int retID = m_OPCServer.Connect(m_OPCSrvHostInform, srvGuid);
                if (HRESULTS.Failed(retID))
                {
                    SendErrMessage("连接OPC服务器错误：" + retID.ToString());
                    m_OPCServer = null;
                    m_OPCConnected = false;
                    return false;
                }
                // 获取服务器状态
                SERVERSTATUS stat = null;
                int rtc = m_OPCServer.GetStatus(out stat);
                if (HRESULTS.Succeeded(rtc))
                {
                    StringBuilder sb = new StringBuilder(stat.szVendorInfo, 200);
                    sb.AppendFormat(" Version:{0}.{1}.{2}", stat.wMajorVersion, stat.wMinorVersion, stat.wBuildNumber);
                    SendOPCStatusInform(sb.ToString());

                    if (InitialGroup(m_GroupName, m_htOPCItemName)) //初始化组对象
                    {
                        m_OPCConnected = true;
                        //opcThread.Start();
                    }
                    else
                    {
                        m_OPCConnected = false;
                        return m_OPCConnected;
                    }
                }
                else
                {
                    SendErrMessage("获取状态时出错,状态码:" + rtc.ToString());
                    m_OPCServer = null;
                    m_OPCConnected = false;
                    return false;
                }
                m_OPCConnected = true;
                return true;
            }
            catch (Exception ex)
            {
                SendErrMessage("连接OPC服务器错误,原因:" + ex.Message);
                m_OPCConnected = false;
                return false;
            }
        }

        /// <summary>
        /// 断开OPC服务器的连接
        /// </summary>
        /// <returns></returns>
        public bool DisConnectOPCServer()
        {
            if (!m_OPCConnected)
            {
              //  if (tmrMonitor.Enabled) tmrMonitor.Stop();
                if (ReadOPCInformWorker.WorkerSupportsCancellation) ReadOPCInformWorker.CancelAsync();
                return true;
            }
            try
            {
               //  tmrMonitor.Stop();
                if (ReadOPCInformWorker.WorkerSupportsCancellation) ReadOPCInformWorker.CancelAsync();
                if (m_OPCServer != null)
                {
                    m_OPCServer.Disconnect();				// 断开与OPC服务器的连接
                    m_OPCServer = null;
                }
                m_OPCConnected = false;
                SendOPCStatusInform("已断开与OPC服务器的连接!");
                return true;
            }
            catch (Exception ex)
            {
                SendErrMessage("断开与OPC服务器连接出错,原因:" + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 初始化组操作,创建Item 信息相应的数据表,主要用于读写操作
        /// </summary>
        /// <param name="strGroupName">组名</param>
        /// <param name="htItemInformation">一个有关Item信息的哈希表,Key:Item名称,Value:对应的类型</param>
        /// <returns></returns>
        private bool InitialGroup(string strGroupName, Hashtable htItemInformation)
        {
            if (m_OPCServer == null)
            {
                SendErrMessage("初始化<OPC组>信息时,<OPC服务器对象>为空!");
                return false;
            }

           // DataRow itemRow = null;
            int itemCount = htItemInformation.Count;  
            string strItemName = "";
            string strItemValue="";
            int itemIndex = 0;
            try
            {
               // CreateItemInformTable(ref m_dtItemInform);
                //获取ItemDef信息
                m_SynGroup = new SyncIOGroup(m_OPCServer); 
                foreach (DictionaryEntry obj in htItemInformation)
                {
                    strItemName = obj.Key.ToString();
                    strItemValue = obj.Value.ToString();
                    m_SynGroup.Add(strItemName);
                    itemIndex++;
                }
               // tmrMonitor.Start();
                ReadOPCInformWorker.RunWorkerAsync();   //Modified by hychong 2012-03-19
                
                return true;
            }
            catch (System.Exception ex)
            {
                SendErrMessage("初始化组对象出错，原因:" + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 读取Item的值
        /// </summary>
        /// <param name="strItemName">Item名称</param>
        /// <param name="retValue">读取Item后的返回值</param>
        /// <returns>读取成功为true</returns>
        public bool ReadItemValue(string strItemID, ref string retValue)
        {
            OPCItemState rslt = null;
            OPCDATASOURCE dsrc = OPCDATASOURCE.OPC_DS_DEVICE;
            //string strItemID = string.Format("{0}{1}", strItemNamePrefix, strItemName);
            ItemDef ItemData = null;
            if (m_SynGroup == null)
                return false;
            try
            {
                ItemData = m_SynGroup.Item(strItemID);
                if (ItemData == null)
                {
                    m_SynGroup.Add(strItemID);
                    ItemData = m_SynGroup.Item(strItemID);
                }
                int rtc = m_SynGroup.Read(dsrc, ItemData, out rslt);
                if (HRESULTS.Failed(rtc))
                {
                    SendErrMessage(string.Format("读取信号【{0}】出错,原因:【{1}】", strItemID, m_SynGroup.GetErrorString(rtc)));
                    return false;
                }
                if (rslt != null)
                {
                    if (HRESULTS.Succeeded(rslt.Error))
                    {
                        if (rslt.DataValue is System.Byte[])//如果为字节数据char[]
                        {
                            System.Byte[] rollIDdata = (rslt.DataValue) as System.Byte[];
                            retValue = "";
                            for (int i = 0; i < rollIDdata.Length; i++)
                            {
                                //*****此处需要对读取到的数据进行转化*******
                                retValue += Convert.ToString((char)rollIDdata[i]);
                            }
                        }
                        else
                        {
                            //其他类型(在OPC中定义的变量只有1位的情况如Int,Uint型)
                            retValue = rslt.DataValue.ToString();
                        }
                        Console.WriteLine("Quality={0}", m_SynGroup.GetQualityString(rslt.Quality));// 显示质量和时间戳
                        Console.WriteLine("TiemStamp={0}", DateTime.FromFileTime(rslt.TimeStamp));
                        return true;
                    }
                    else
                    {
                        retValue = "";
                        SendErrMessage(string.Format("读取信号【{0}】出错,原因:【{1}】", strItemID, m_SynGroup.GetErrorString(rtc)));
                        return false;
                    }
                }
                else
                {
                    SendErrMessage(string.Format("信号【{0}】没有数据!", strItemID));
                    return false;
                }
            }
            catch (System.Exception ex)
            {
                SendErrMessage(string.Format("读取信号【{0}】出错,原因:【{1}】", strItemID, ex.Message));
                return false;
            }
        }

        public bool ReadItemValue(string strItemID, ref CItemInformEventArgs itemInform)
        {
            OPCItemState rslt = null;
            OPCDATASOURCE dsrc = OPCDATASOURCE.OPC_DS_DEVICE;
            itemInform = new CItemInformEventArgs();
           // string strItemID = string.Format("{0}{1}", strItemNamePrefix, strItemName);
            ItemDef ItemData = null;
            if (m_SynGroup == null)
                return false;
            try
            {
                ItemData = m_SynGroup.Item(strItemID);
                if (ItemData == null)
                {
                    m_SynGroup.Add(strItemID);
                    ItemData = m_SynGroup.Item(strItemID);
                }
                int rtc = m_SynGroup.Read(dsrc, ItemData, out rslt);
                if (HRESULTS.Failed(rtc))
                {
                    SendErrMessage(string.Format("读取信号【{0}】出错,原因:【{1}】", strItemID, m_SynGroup.GetErrorString(rtc)));
                    return false;
                }
                if (rslt != null)
                {
                    if (HRESULTS.Succeeded(rslt.Error))
                    {
                        itemInform.ItemID = strItemID;
                        if (rslt.DataValue is System.Byte[])//如果为字节数据char[]
                        {
                            System.Byte[] rollIDdata = (rslt.DataValue) as System.Byte[];
                            itemInform.ItemValue = "";
                            for (int i = 0; i < rollIDdata.Length; i++)
                            {
                                //*****此处需要对读取到的数据进行转化*******
                                itemInform.ItemValue += Convert.ToString((char)rollIDdata[i]);
                            }
                            itemInform.ItemValue = itemInform.ItemValue.Split(new char[] { '\0' }, StringSplitOptions.RemoveEmptyEntries)[0];
                        }
                        else
                        {
                            //其他类型(在OPC中定义的变量只有1位的情况如Int,Uint型)
                            itemInform.ItemValue = rslt.DataValue.ToString();
                        }
                        itemInform.ItemQuality = m_SynGroup.GetQualityString(rslt.Quality);
                        itemInform.ItemTimeStamp = string.Format("{0}", DateTime.FromFileTime(rslt.TimeStamp));
                        return true;
                    }
                    else
                    {
                        SendErrMessage(string.Format("读取信号【{0}】出错,原因:【{1}】", strItemID, m_SynGroup.GetErrorString(rtc)));
                        return false;
                    }
                }
                else
                {
                    SendErrMessage(string.Format("信号【{0}】没有数据!", strItemID));
                    return false;
                }
            }
            catch (System.Exception ex)
            {
                SendErrMessage(string.Format("读取信号【{0}】出错,原因:【{1}】", strItemID, ex.Message));
                return false;
            }
        }

        /// <summary>
        /// 根据测点项Item对应的名称简称，写入对应测点项的值
        /// </summary>
        /// <param name="strItemName">Item的简称</param>
        /// <param name="strItemValue">Item对应的值</param>
        /// <returns></returns>
        public bool WriteItemValue(string strItemID, string strItemValue)
        {
            int rtc = -1;
            ItemDef itemData = null;
            if (m_SynGroup == null)
                return false;
            try
            {
                itemData = m_SynGroup.Item(strItemID);
                if (itemData == null)
                {
                    m_SynGroup.Add(strItemID);
                    itemData = m_SynGroup.Item(strItemID);
                }
                //如果将写入的值的长度大于14的字符串，则将其转化为字节数组依次写入
                if (strItemValue.Trim().Length>=14)
                {
                    char[] bytBuffer = null;
                    if (strItemValue.Trim().Length == 14 || strItemValue.Trim().Length==18)
                    {
                        bytBuffer = new char[strItemValue.Trim().Length];
                        for (int i = 0; i < bytBuffer.Length; i++)
                        {
                            bytBuffer[i] = Convert.ToChar(strItemValue.Substring(i, 1));
                        }
                    }
                    else
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append(strItemValue.Trim());
                        while (sb.Length < 95)
                        {
                            sb.Append(" ");
                        }
                        bytBuffer = new char[sb.Length];
                        for (int i = 0; i < bytBuffer.Length; i++)
                        {
                            bytBuffer[i] = Convert.ToChar(sb.ToString().Substring(i, 1));
                        }
                    }
                    rtc = m_SynGroup.Write(itemData, bytBuffer);
                }
                else
                    rtc = m_SynGroup.Write(itemData, strItemValue);
                if (HRESULTS.Failed(rtc))
                {
                    SendErrMessage(string.Format("写入信号【{0}】出错,原因:【{1}】", strItemID, m_SynGroup.GetErrorString(rtc)));
                    return false;
                }
                return true;
            }
            catch (System.Exception ex)
            {
                SendErrMessage(string.Format("写入信号【{0}】出错,原因:【{1}】", strItemID, ex.Message));
                return false;
            }
        }

        /// <summary>
        /// 读取ItemID的值
        /// </summary>
        /// <param name="strItemName">Item名称</param>
        /// <param name="retValue">读取Item后的返回值</param>
        /// <returns>读取成功为true</returns>
        private bool ReadItemIDValue(string strItemID, ref string retValue)
        {
            OPCItemState rslt = null;
            OPCDATASOURCE dsrc = OPCDATASOURCE.OPC_DS_DEVICE;
            ItemDef ItemData = null;
            if (m_SynGroup == null)
                return false;
            try
            {

                ItemData = m_SynGroup.Item(strItemID);
                if (ItemData == null)
                {
                    m_SynGroup.Add(strItemID);
                    ItemData = m_SynGroup.Item(strItemID);
                }
                int rtc = m_SynGroup.Read(dsrc, ItemData, out rslt);
                if (HRESULTS.Failed(rtc))
                {
                    return false;
                }
                if (rslt != null)
                {
                    if (HRESULTS.Succeeded(rslt.Error))
                    {
                        if (rslt.DataValue is System.Byte[])//如果为字节数据char[]
                        {
                            System.Byte[] rollIDdata = (rslt.DataValue) as System.Byte[];
                            retValue = "";
                            for (int i = 0; i < rollIDdata.Length; i++)
                            {
                                //*****此处需要对读取到的数据进行转化*******
                                retValue += Convert.ToString((char)rollIDdata[i]);
                            }
                            retValue = retValue.Split(new char[] { (char)13 })[0];
                        }
                        else
                        {
                            //其他类型(在OPC中定义的变量只有1位的情况如Int,Uint型)
                            retValue = rslt.DataValue.ToString();
                        }
                        return true;
                    }
                    else
                    {
                        retValue = "";
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 根据　m_htOPCItemName 哈希表中保存的ItemName与ItemValue值的对应的关系，
        /// 将最新的ItemValue值与保存的ItemValue进行比较，如果已更新，则调用 OPCGroup_DataChange
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tmrMonitor_Tick(object sender, EventArgs e)
        {
            string strItemID = "";
            string strItemValue="";
            bool blnRet = false;
            try
            {
                foreach (DictionaryEntry obj in m_htOPCItemName)
                {
                    strItemID = string.Format("{0}", obj.Key);
                    blnRet = ReadItemIDValue(strItemID, ref strItemValue);
                    if (blnRet)
                    {
                        if (strItemValue != m_htOPCLatestItemInform[obj.Key].ToString())
                        {
                            m_htOPCLatestItemInform[obj.Key] = strItemValue;
                            CItemInformEventArgs args = new CItemInformEventArgs();
                            args.GroupName = m_GroupName;
                            args.ItemName = strItemID.Split(']')[1];
                            args.ItemID = strItemID;
                            args.ItemClientHandler = m_SynGroup.ClientHandle.ToString();
                            args.ItemServerHandler = "0";
                            args.ItemValue = strItemValue;
                            args.ItemQuality ="Good";
                            args.ItemTimeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            SendOPCItemInform(args);
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("TimeMonitor出错,原因:{0}",ex.Message);
            }
        }

        //针对Simatic NET服务器的ItemID方式: "S7:[S7 Connection_1]AllowScan";
        private string GetItemID(object strItemName)
        {
            string actItemName = string.Format("{0}{1}", m_ItemNamePrefix, strItemName);//针对SimaticNet服务器
            return actItemName;
        }

        /// <summary>
        /// 以事件形式返回类运行时出现的各种错误信息
        /// </summary>
        /// <param name="strMsg"></param>
        private void SendErrMessage(string strMsg)
        {
            if (!string.IsNullOrEmpty(strMsg))
            {
                MsgEventArgs args = new MsgEventArgs(strMsg);
                if (OnErrorMessage != null)
                    OnErrorMessage(this, args);
            }
        }

        /// <summary>
        /// 以事件形式返回类运行时OPC服务器的各种运行状态
        /// </summary>
        /// <param name="strMsg"></param>
        private void SendOPCStatusInform(string strMsg)
        {
            if (!string.IsNullOrEmpty(strMsg))
            {
                MsgEventArgs args = new MsgEventArgs(strMsg);
                if (OnRetOPCStatus != null)
                    OnRetOPCStatus(this, args);
            }
        }

        private void SendOPCItemInform(CItemInformEventArgs itemArgs)
        {
            if (itemArgs != null && OnRetOPCItemInform != null)
                OnRetOPCItemInform(this, itemArgs);
        }

    }
}
