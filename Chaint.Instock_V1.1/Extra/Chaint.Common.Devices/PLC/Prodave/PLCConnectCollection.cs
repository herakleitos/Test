using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;


/*-----------------------------------------------------------------------------------
 * 作者: Automation&IT Dept. 
 * 
 * 创建时间: 2014-02-17
 * 
 * 功能描述: 
 *      (1) PLC 连接集合,此连接集合可同时包含各种通讯方式(Prodave5.6,Prodave6.0等)的PLC连接
 *      (2) 连接项依赖于此连接项传进来的GongweiType与MobanName两个参数联合决定,
 *          其中GongweiType=RHS(输送系统)/AM(报警监控)/TS(触摸屏操作)/.....
 *              MobanName=CPU1|CPU2|....
 *      (3) 
 * 
 ------------------------------------------------------------------------------------*/
namespace Chaint.Common.Devices.PLC
{
    public class PLCConnectCollection : IEnumerator
    {
        private IList<PLCConnItem> m_plcConnHelpers = new List<PLCConnItem>();
        private int m_posIndex=-1;                                      //位置信息

        /// <summary>
        /// 实现按站点类型、模板名称索引
        /// </summary>
        /// <param name="strStationName"></param>
        /// <param name="strTemplateName"></param>
        /// <returns></returns>
        public PLCConnItem this[string strStationName, string strTemplateName]
        {
            get 
            {
                PLCConnItem currPLCItem = null;
                foreach (PLCConnItem plc in m_plcConnHelpers)
                {
                    if (plc.PLCStationName.ToLower() == strStationName.ToLower() &&
                        plc.PLCTemplateName.ToLower() == strTemplateName.ToLower())
                    {
                        currPLCItem = plc;
                        break;
                    }
                }
                return currPLCItem;
            }
            set
            {
                AddPLCConnItem(strStationName, strTemplateName);
            }
        }

        /// <summary>
        /// 实现按索引值取值
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public PLCConnItem this[int index]
        {
            get
            {
                if (index < 0 || index > m_plcConnHelpers.Count || m_plcConnHelpers==null)
                    return null;
                else
                    return m_plcConnHelpers[index];
            }
            set
            {
                if(index<m_plcConnHelpers.Count && index>-1 && m_plcConnHelpers!=null)
                    m_plcConnHelpers[index] = value;
            }
        }

        /// <summary>
        /// 当前PLCHelper数量
        /// </summary>
        public int Count
        {
            get 
            {
                if (m_plcConnHelpers==null)
                    return 0;
                else
                    return m_plcConnHelpers.Count; 
            }
        }

        /// <summary>
        /// 实现Foreach语句遍历
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()//实现GetEnumerator()接口
        {
            return new PLCConnectCollection(m_plcConnHelpers);
        }

        /// <summary>
        /// 实现空构造函数
        /// </summary>
        public PLCConnectCollection()
        {

        }

        public PLCConnectCollection(IList<PLCConnItem> plcItems)
        {
            m_plcConnHelpers = plcItems;
        }

        public PLCConnectCollection(PLCConnItem[] plcItems)
        {
            if (plcItems != null && plcItems.Length > 0)
            {
                m_plcConnHelpers.Clear();
                for (int i = 0; i < plcItems.Length;i++ )
                {
                    m_plcConnHelpers.Add(plcItems[i]);
                }
            }
        }

        public bool MoveNext()
        {
            m_posIndex++;
            return (m_posIndex < m_plcConnHelpers.Count);
        }

        public void Reset()
        {
            m_posIndex= - 1;
        }

        public object Current
        {
            get
            {
                try
                {
                    if (m_plcConnHelpers.Count == 0)
                    {
                        return null;
                    }
                    else if (m_plcConnHelpers.Count == 1)
                    {
                        m_posIndex = 0;
                        return m_plcConnHelpers[m_posIndex];
                    }
                    else if (m_posIndex >= 0 && m_posIndex < m_plcConnHelpers.Count)
                    {
                        return m_plcConnHelpers[m_posIndex];
                    }
                    else
                        return null;
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }

        public void AddPLCConnItem(string strGongweiType, string strMobanNO)
        {
            PLCConnItem plcItem = new PLCConnItem(strGongweiType, strMobanNO);
            AddPLCConnItem(plcItem);
        }

        /// <summary>
        /// 增加某个PLC Item
        /// </summary>
        /// <param name="plcItem"></param>
        public void AddPLCConnItem(PLCConnItem plcItem)
        {
            bool blnExists = IsContainPLCConnItem(plcItem);
            if (!blnExists)
                m_plcConnHelpers.Add(plcItem);
        }

        /// <summary>
        /// 移除某个PLC Item
        /// </summary>
        /// <param name="plcItem"></param>
        public void RemovePLCConnItem(PLCConnItem plcItem)
        {
            foreach (PLCConnItem plc in m_plcConnHelpers)
            {
                if (plc.PLCStationName.ToLower() == plcItem.PLCStationName.ToLower() &&
                    plc.PLCTemplateName.ToLower() == plcItem.PLCTemplateName.ToLower())
                {
                    plc.PLCHelper.CloseConnectPLC();
                    m_plcConnHelpers.Remove(plc);
                    break;
                }
            }
        }

        /// <summary>
        /// 按索引移除某个PLC Conn Item
        /// </summary>
        /// <param name="index"></param>
        public void RemovePLCConnItemAt(int index)
        {
            if (m_plcConnHelpers.Count == 0 || m_plcConnHelpers.Count<index) return;
            m_plcConnHelpers[index].PLCHelper.CloseConnectPLC();
            m_plcConnHelpers.RemoveAt(index);
        }

        /// <summary>
        /// 移除所有元素
        /// </summary>
        public void Clear()
        {
            if (m_plcConnHelpers != null)
            {
                for (int i = 0; i < m_plcConnHelpers.Count; i++)
                {
                    RemovePLCConnItemAt(i);
                }
                m_plcConnHelpers.Clear();
            }
        }

        /// <summary>
        /// 是否包含PLC Item
        /// </summary>
        /// <param name="plcItem"></param>
        /// <returns></returns>
        public bool IsContainPLCConnItem(PLCConnItem plcItem)
        {
            bool blnContained = false;
            foreach (PLCConnItem plc in m_plcConnHelpers)
            {
                if (plc.PLCStationName.ToLower() == plcItem.PLCStationName.ToLower() &&
                    plc.PLCTemplateName.ToLower() == plcItem.PLCTemplateName.ToLower())
                {
                    blnContained = true;
                    break;
                }
            }

            return blnContained;
        }

        /// <summary>
        /// 按GongweiType和MobanNO判断是否包含PLCItem
        /// </summary>
        /// <param name="plcStationName">工作站类型</param>
        /// <param name="plcTemplateName">模板名称</param>
        /// <returns></returns>
        public bool IsContainPLCConnItem(string plcStationName, string plcTemplateName)
        {
            bool blnContained = false;
            foreach (PLCConnItem plc in m_plcConnHelpers)
            {
                if (plc.PLCStationName.ToLower() == plcStationName.ToLower() &&
                    plc.PLCTemplateName.ToLower() == plcTemplateName.ToLower())
                {
                    blnContained = true;
                    break;
                }
            }

            return blnContained;
        }
    }

    public class PLCConnItem : IEnumerable
    {
        #region 变量定义
        private IPLCOperation m_plcHelper = null;
        private byte[] m_bytBuffer = new byte[2];
        private string m_plcStationName = "";
        private string m_plcTemplateName = "";
        private PLCType m_plcType = PLCType.PLC_5D6;

        private ushort m_dBUnit = 100;
        private string m_address = "192.168.0.1";
        private short m_segmentNO = 0;
        private byte m_rackNO = 2;
        private byte m_slotNO = 0;
        private ushort m_connNr = 63;
        private int m_dwno = 0;
        private int m_amount = 2;
        #endregion

        #region 属性定义
        public IPLCOperation PLCHelper
        {
            get { return m_plcHelper; }
            set { m_plcHelper = value; }
        }

        public byte[] ByteBuffer
        {
            get { return m_bytBuffer; }
            set { m_bytBuffer = value; }
        }

        public string PLCStationName
        {
            get { return m_plcStationName; }
            set { m_plcStationName = value; }
        }

        public string PLCTemplateName
        {
            get { return m_plcTemplateName; }
            set { m_plcTemplateName = value; }
        }

        public ushort DBUnit
        {
            get { return m_dBUnit; }
            set { m_dBUnit = value; }
        }

        /// <summary>
        /// 针对Prodave5.6,请输入整数,一般为2
        /// 针对Prodave 6.0 IE,请输入IP地址
        /// </summary>
        public string Address
        {
            get { return m_address; }
            set { m_address = value; }
        }

        /// <summary>
        /// 针对Prodave5.6中的参数,段地址
        /// </summary>
        public short SegmentNO
        {
            get { return m_segmentNO; }
            set { m_segmentNO = value; }
        }

        /// <summary>
        /// 槽号
        /// </summary>
        public byte SlotNO
        {
            get { return m_slotNO; }
            set { m_slotNO = value; }
        }

        /// <summary>
        /// 机架号
        /// </summary>
        public byte RackNO
        {
            get { return m_rackNO; }
            set { m_rackNO = value; }
        }

        /// <summary>
        /// 针对Prodave 5.6,连接号(1-4)
        /// 针对Prodave 6.0 IE,连接号(0--63)
        /// </summary>
        public ushort ConnNr
        {
            get { return m_connNr; }
            set { m_connNr = value; }
        }

        /// <summary>
        /// 初始地址,访问字节数组的初始字节地址
        /// </summary>
        public int DWNO
        {
            get { return m_dwno; }
            set { m_dwno = value; }
        }

        /// <summary>
        /// 访问的最大字节数
        /// </summary>
        public int Amount
        {
            get { return m_amount; }
            set { m_amount = value; }
        }


        public PLCType PlcType
        {
            get { return m_plcType; }
            set { m_plcType = value; }
        }
        #endregion

        public PLCConnItem()
        {

        }

        public PLCConnItem(string plcStationName, string plcTemplateName)
        {
            m_plcStationName = plcStationName;
            m_plcTemplateName = plcTemplateName;
        }

        private PLCConnItem[] m_plcConnItems = null;

        public PLCConnItem(PLCConnItem[] plcConnItems)
        {
            m_plcConnItems = new PLCConnItem[plcConnItems.Length];
            for (int i = 0; i < plcConnItems.Length; i++)
            {
                m_plcConnItems[i] = plcConnItems[i];
            }
        }

        public IEnumerator GetEnumerator()//实现GetEnumerator()接口
        {
            return new PLCConnectCollection(m_plcConnItems);
        }
    }

}
