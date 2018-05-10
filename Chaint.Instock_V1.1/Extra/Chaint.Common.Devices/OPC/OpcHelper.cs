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


/*��OPC  �ͻ�������OPC���������õ�����Դ�������У�
 * (1)�������˶�̬���ӿ� OPCNetApiChs.Dll;
 * (2)  ����Ĺ����У�����OPC���������Ͽ���OPC�����������ӡ���ȡָ��Item��ֵ����ֵд��ָ��Item;
 * (3)  ������Ϣ�ķ������������OnErrorMessage �¼���ʹ�ô�����û�ֻ��Ҫ���ƴ��¼�����;
 * (4)  ��OPC������ͨ�ŵ�״̬��Ϣ���������OnRetMessage,ʹ�ô�����û�ֻ��Ҫ���ƴ��¼�����;
 * (5)  ������¼�OnOPCRetSignalVal��Ҫ���ڷ��ص�ǰItemֵ�����仯����Ϣ���˴��ò��ϣ����Բ��ö���;
 * 
 * (6)  ���ڲ�ͬ��OPC������������ʹ�õ�Item�������Ƶķ�����Щ��ͬ�����磺����KEPware.KEPServerEx.V4,
 *      �䶨�巽ʽΪ��Channel_1.Device_1.Chaint.ItemName(ͨ����.�豸��.����.��Ŀ��)
 *      ��OPC.SimaticNET ������Ϊ��S7:[Connection_1]:ItemName
 * (7)  ��������ڵ��ź����ƣ���Ҫ������PLC_Config�ж���;
 * */

namespace Chaint.Common.Devices.OPC
{
    public class OpcHelper
    {
        #region ��������
        private string m_ItemNamePrefix = "S7:[S7 Connection_2]";
        private string m_GroupName = "Chaint";

        private string m_OPCServerName = "";        //OPC����������
        private Host m_OPCSrvHostInform = null;       //OPC����������PC��Ӧ��������Ϣ

        private string m_RemoteServerIP = "";
        private string m_HostName = "localhost";    //OPC����������PC��������
        private string m_UserName = "";             //OPC����������PC�ĵ�¼�û���
        private string m_UserPWD = "";              //OPC����������PC�ĵ�¼�û�����

        private OpcServer m_OPCServer = null;
        private SyncIOGroup m_SynGroup = null;
        private int m_RefreshRate = 500;
        private bool m_OPCConnected = false;

        private Hashtable m_htOPCItemName = new Hashtable();        //�ɵ��ô��������OPC��������ָ�����Item������Ϣ,Key:����(ItemName),Value:��Ӧֵ(ItemValue)
        private Hashtable m_htOPCLatestItemInform = new Hashtable();    //������µ�ItemID��ItemValueֵ�Ķ��ձ�
        private Timer tmrMonitor = new Timer();
        private System.ComponentModel.BackgroundWorker ReadOPCInformWorker = new System.ComponentModel.BackgroundWorker();
        #endregion

        #region ���Զ���
        public string OPCServerName
        {
            set { m_OPCServerName = value; }
        }

        /// <summary>
        /// ��Ҫ��OPC��������ӵ����������
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
        /// OPC �����������ĸ�����
        /// </summary>
        public int UpdateRate
        {
            set { m_RefreshRate = value; }
        }
        /// <summary>
        /// ����PLC����
        /// </summary>
        public Hashtable OPCLatestItemInform
        {
            set { m_htOPCLatestItemInform = value; }
            get { return m_htOPCLatestItemInform; }
        }

        #endregion

        #region �Զ����¼�
        public delegate void MsgEventHandler(object sender, MsgEventArgs e);
        public delegate void SignalInformHandler(object sender, CItemInformEventArgs e);

        public event MsgEventHandler OnErrorMessage;        //������Ϣ��
        public event MsgEventHandler OnRetOPCStatus;          //������Ϣ
        public event SignalInformHandler OnRetOPCItemInform; //OPC�������˷��ص��ź�ֵ

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

        #region ���巵�ص�Item��Ϣ
        public class CItemInformEventArgs : EventArgs
        {
            public CItemInformEventArgs()
            { }
            public string GroupName = "";
            public string ItemName = "";            //����
            public string ItemValue = "";          //��ֵ
            public string ItemQuality = "";         //������
            public string ItemTimeStamp = "";            //ʱ�����Ӧ�ľ���ʱ��
            public string ItemClientHandler = "";        //���Ӧ��ClientHandler
            public string ItemServerHandler = "";       //���ӦServerHandler 
            public string ItemID = "";           //��ȫ��
        }
        #endregion

        /// <summary>
        /// ���м���ʽ����Item��Ϣ
        /// </summary>
        /// <param name="drItemInformation"></param>
        public OpcHelper(DataTable dtItemInformation)
        {
            ReadOPCInformWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(ReadOPCInformWorker_DoWork);
            ReadOPCInformWorker.WorkerSupportsCancellation = true;

            //����ʵʱ���OPC��ItemIDֵ�ı仯���
//             tmrMonitor.Interval = 800;
//             tmrMonitor.Tick += new EventHandler(tmrMonitor_Tick);

            //��ȡ���ص�IP��ַ
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
                    SendErrMessage("�������ϢΪ�գ��������ݱ�PLC_Config�����!");
                }
            }
            catch (Exception ex)
            {
                SendErrMessage("����OPC�ͻ��������ԭ��:" + ex.Message);
            }
        }

        /// <summary>
        /// �Թ�ϣ����ʽ����Item��Ϣ,Item������Ϣ,Key:����(ItemName),Value:��Ӧֵ(ItemValue)
        /// </summary>
        /// <param name="ItemInformation"></param>
        public OpcHelper(Hashtable ItemInformation)
        {
            ReadOPCInformWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(ReadOPCInformWorker_DoWork);
            ReadOPCInformWorker.WorkerSupportsCancellation = true;

            //����ʵʱ���OPC��ItemIDֵ�ı仯���
//             tmrMonitor.Interval = 800;
//             tmrMonitor.Tick += new EventHandler(tmrMonitor_Tick);

            //��ȡ���ص�IP��ַ
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
                    SendErrMessage("�������ϢΪ�գ��������ݱ�PLC_Config�����!");
                }

            }
            catch (Exception ex)
            {
                SendErrMessage("����OPC�ͻ��������ԭ��:" + ex.Message);
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
                //Console.WriteLine(string.Format("����ź�<{0}> ����,ԭ��:{1}", strItemID,ex.Message));
                SendErrMessage(String.Format("����ź�<{0}> ����,ԭ��:{1}", strItemID, ex.Message));
            }
        }

        /// <summary>
        /// ����OPC������
        /// </summary>
        /// <param name="strOPCServerName">OPC����������</param>
        /// <returns></returns>
        public bool ConnectOPCServer(string strOPCServerName)
        { 
            m_OPCConnected = false;
            OpcServerBrowser opcbrowser = new OpcServerBrowser(m_HostName);
            Guid srvGuid;
            try
            {
                //����OPC������
                SendOPCStatusInform("��������<OPC>��������");
                opcbrowser.CLSIDFromProgID(strOPCServerName, out srvGuid);
                m_OPCServer = new OpcServer();
                int retID = m_OPCServer.Connect(m_OPCSrvHostInform, srvGuid);
                if (HRESULTS.Failed(retID))
                {
                    SendErrMessage("����OPC����������" + retID.ToString());
                    m_OPCServer = null;
                    m_OPCConnected = false;
                    return false;
                }
                // ��ȡ������״̬
                SERVERSTATUS stat = null;
                int rtc = m_OPCServer.GetStatus(out stat);
                if (HRESULTS.Succeeded(rtc))
                {
                    StringBuilder sb = new StringBuilder(stat.szVendorInfo, 200);
                    sb.AppendFormat(" Version:{0}.{1}.{2}", stat.wMajorVersion, stat.wMinorVersion, stat.wBuildNumber);
                    SendOPCStatusInform(sb.ToString());

                    if (InitialGroup(m_GroupName, m_htOPCItemName)) //��ʼ�������
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
                    SendErrMessage("��ȡ״̬ʱ����,״̬��:" + rtc.ToString());
                    m_OPCServer = null;
                    m_OPCConnected = false;
                    return false;
                }
                m_OPCConnected = true;
                return true;
            }
            catch (Exception ex)
            {
                SendErrMessage("����OPC����������,ԭ��:" + ex.Message);
                m_OPCConnected = false;
                return false;
            }
        }

        /// <summary>
        /// �Ͽ�OPC������������
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
                    m_OPCServer.Disconnect();				// �Ͽ���OPC������������
                    m_OPCServer = null;
                }
                m_OPCConnected = false;
                SendOPCStatusInform("�ѶϿ���OPC������������!");
                return true;
            }
            catch (Exception ex)
            {
                SendErrMessage("�Ͽ���OPC���������ӳ���,ԭ��:" + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// ��ʼ�������,����Item ��Ϣ��Ӧ�����ݱ�,��Ҫ���ڶ�д����
        /// </summary>
        /// <param name="strGroupName">����</param>
        /// <param name="htItemInformation">һ���й�Item��Ϣ�Ĺ�ϣ��,Key:Item����,Value:��Ӧ������</param>
        /// <returns></returns>
        private bool InitialGroup(string strGroupName, Hashtable htItemInformation)
        {
            if (m_OPCServer == null)
            {
                SendErrMessage("��ʼ��<OPC��>��Ϣʱ,<OPC����������>Ϊ��!");
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
                //��ȡItemDef��Ϣ
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
                SendErrMessage("��ʼ����������ԭ��:" + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// ��ȡItem��ֵ
        /// </summary>
        /// <param name="strItemName">Item����</param>
        /// <param name="retValue">��ȡItem��ķ���ֵ</param>
        /// <returns>��ȡ�ɹ�Ϊtrue</returns>
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
                    SendErrMessage(string.Format("��ȡ�źš�{0}������,ԭ��:��{1}��", strItemID, m_SynGroup.GetErrorString(rtc)));
                    return false;
                }
                if (rslt != null)
                {
                    if (HRESULTS.Succeeded(rslt.Error))
                    {
                        if (rslt.DataValue is System.Byte[])//���Ϊ�ֽ�����char[]
                        {
                            System.Byte[] rollIDdata = (rslt.DataValue) as System.Byte[];
                            retValue = "";
                            for (int i = 0; i < rollIDdata.Length; i++)
                            {
                                //*****�˴���Ҫ�Զ�ȡ�������ݽ���ת��*******
                                retValue += Convert.ToString((char)rollIDdata[i]);
                            }
                        }
                        else
                        {
                            //��������(��OPC�ж���ı���ֻ��1λ�������Int,Uint��)
                            retValue = rslt.DataValue.ToString();
                        }
                        Console.WriteLine("Quality={0}", m_SynGroup.GetQualityString(rslt.Quality));// ��ʾ������ʱ���
                        Console.WriteLine("TiemStamp={0}", DateTime.FromFileTime(rslt.TimeStamp));
                        return true;
                    }
                    else
                    {
                        retValue = "";
                        SendErrMessage(string.Format("��ȡ�źš�{0}������,ԭ��:��{1}��", strItemID, m_SynGroup.GetErrorString(rtc)));
                        return false;
                    }
                }
                else
                {
                    SendErrMessage(string.Format("�źš�{0}��û������!", strItemID));
                    return false;
                }
            }
            catch (System.Exception ex)
            {
                SendErrMessage(string.Format("��ȡ�źš�{0}������,ԭ��:��{1}��", strItemID, ex.Message));
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
                    SendErrMessage(string.Format("��ȡ�źš�{0}������,ԭ��:��{1}��", strItemID, m_SynGroup.GetErrorString(rtc)));
                    return false;
                }
                if (rslt != null)
                {
                    if (HRESULTS.Succeeded(rslt.Error))
                    {
                        itemInform.ItemID = strItemID;
                        if (rslt.DataValue is System.Byte[])//���Ϊ�ֽ�����char[]
                        {
                            System.Byte[] rollIDdata = (rslt.DataValue) as System.Byte[];
                            itemInform.ItemValue = "";
                            for (int i = 0; i < rollIDdata.Length; i++)
                            {
                                //*****�˴���Ҫ�Զ�ȡ�������ݽ���ת��*******
                                itemInform.ItemValue += Convert.ToString((char)rollIDdata[i]);
                            }
                            itemInform.ItemValue = itemInform.ItemValue.Split(new char[] { '\0' }, StringSplitOptions.RemoveEmptyEntries)[0];
                        }
                        else
                        {
                            //��������(��OPC�ж���ı���ֻ��1λ�������Int,Uint��)
                            itemInform.ItemValue = rslt.DataValue.ToString();
                        }
                        itemInform.ItemQuality = m_SynGroup.GetQualityString(rslt.Quality);
                        itemInform.ItemTimeStamp = string.Format("{0}", DateTime.FromFileTime(rslt.TimeStamp));
                        return true;
                    }
                    else
                    {
                        SendErrMessage(string.Format("��ȡ�źš�{0}������,ԭ��:��{1}��", strItemID, m_SynGroup.GetErrorString(rtc)));
                        return false;
                    }
                }
                else
                {
                    SendErrMessage(string.Format("�źš�{0}��û������!", strItemID));
                    return false;
                }
            }
            catch (System.Exception ex)
            {
                SendErrMessage(string.Format("��ȡ�źš�{0}������,ԭ��:��{1}��", strItemID, ex.Message));
                return false;
            }
        }

        /// <summary>
        /// ���ݲ����Item��Ӧ�����Ƽ�ƣ�д���Ӧ������ֵ
        /// </summary>
        /// <param name="strItemName">Item�ļ��</param>
        /// <param name="strItemValue">Item��Ӧ��ֵ</param>
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
                //�����д���ֵ�ĳ��ȴ���14���ַ���������ת��Ϊ�ֽ���������д��
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
                    SendErrMessage(string.Format("д���źš�{0}������,ԭ��:��{1}��", strItemID, m_SynGroup.GetErrorString(rtc)));
                    return false;
                }
                return true;
            }
            catch (System.Exception ex)
            {
                SendErrMessage(string.Format("д���źš�{0}������,ԭ��:��{1}��", strItemID, ex.Message));
                return false;
            }
        }

        /// <summary>
        /// ��ȡItemID��ֵ
        /// </summary>
        /// <param name="strItemName">Item����</param>
        /// <param name="retValue">��ȡItem��ķ���ֵ</param>
        /// <returns>��ȡ�ɹ�Ϊtrue</returns>
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
                        if (rslt.DataValue is System.Byte[])//���Ϊ�ֽ�����char[]
                        {
                            System.Byte[] rollIDdata = (rslt.DataValue) as System.Byte[];
                            retValue = "";
                            for (int i = 0; i < rollIDdata.Length; i++)
                            {
                                //*****�˴���Ҫ�Զ�ȡ�������ݽ���ת��*******
                                retValue += Convert.ToString((char)rollIDdata[i]);
                            }
                            retValue = retValue.Split(new char[] { (char)13 })[0];
                        }
                        else
                        {
                            //��������(��OPC�ж���ı���ֻ��1λ�������Int,Uint��)
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
        /// ���ݡ�m_htOPCItemName ��ϣ���б����ItemName��ItemValueֵ�Ķ�Ӧ�Ĺ�ϵ��
        /// �����µ�ItemValueֵ�뱣���ItemValue���бȽϣ�����Ѹ��£������ OPCGroup_DataChange
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
                Console.WriteLine("TimeMonitor����,ԭ��:{0}",ex.Message);
            }
        }

        //���Simatic NET��������ItemID��ʽ: "S7:[S7 Connection_1]AllowScan";
        private string GetItemID(object strItemName)
        {
            string actItemName = string.Format("{0}{1}", m_ItemNamePrefix, strItemName);//���SimaticNet������
            return actItemName;
        }

        /// <summary>
        /// ���¼���ʽ����������ʱ���ֵĸ��ִ�����Ϣ
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
        /// ���¼���ʽ����������ʱOPC�������ĸ�������״̬
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
