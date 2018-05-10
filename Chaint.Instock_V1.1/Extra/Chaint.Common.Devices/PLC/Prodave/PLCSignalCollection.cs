using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Chaint.Common.Devices.PLC
{
  
    public class PLCSignalItem : IEnumerable
    {
        private string m_PLCStationName = "";
        private string m_PLCTemplateName = "";
        private string m_SignalName = "";
        private string m_SignalValue = "";
        private int m_DBUnit = 0;
        private int m_DWNO = 0;
        private int m_Amount = 0;


        #region 属性定义
        public string PLCStationName
        {
            set { m_PLCStationName = value; }
            get { return m_PLCStationName; }
        }
        public string PLCTemplateName
        {
            set { m_PLCTemplateName = value; }
            get { return m_PLCTemplateName; }
        }
        public string SignalName
        {
            set { m_SignalName = value; }
            get { return m_SignalName; }
        }
        public string SignalValue
        {
            set { m_SignalValue = value; }
            get { return m_SignalValue; }
        }
        public int DBUnit
        {
            set { m_DBUnit = value; }
            get { return m_DBUnit; }
        }
        public int DWNO
        {
            set { m_DWNO = value; }
            get { return m_DWNO; }
        }
        public int Amount
        {
            set { m_Amount = value; }
            get { return m_Amount; }
        }
        #endregion

        public PLCSignalItem()
        {

        }

        public PLCSignalItem(string plcStationName, string plcTemplateName, string strSignalName, int dbunit, int dwno, int amount)
        {
            m_PLCStationName = plcStationName;
            m_PLCTemplateName = plcTemplateName;
            m_SignalName = strSignalName;
            m_DBUnit = dbunit;
            m_DWNO = dwno;
            m_Amount = amount;
        }

        private PLCSignalItem[] m_PLCSignalItems = null;
        public PLCSignalItem(PLCSignalItem[] plcSignalItems)
        {
            m_PLCSignalItems = new PLCSignalItem[plcSignalItems.Length];
            for (int i = 0; i < plcSignalItems.Length; i++)
            {
                m_PLCSignalItems[i] = plcSignalItems[i];
            }
        }

        public IEnumerator GetEnumerator()//实现GetEnumerator()接口
        {
            return new PLCSignalCollection(m_PLCSignalItems);
        }
    }

    public class PLCSignalCollection : IEnumerator
    {
        private IList<PLCSignalItem> m_lstPLCSignalCollect = new List<PLCSignalItem>();
        private int m_posIndex = -1;                                      //位置信息

        /// <summary>
        /// 实现Foreach语句遍历
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()//实现GetEnumerator()接口
        {
            return new PLCSignalCollection(m_lstPLCSignalCollect);
        }

        /// <summary>
        /// 实现空构造函数
        /// </summary>
        public PLCSignalCollection()
        {

        }

        public PLCSignalItem this[string strPlcStationName, string strPlcTemplateName, string strSignalName]
        {
            get
            {
                PLCSignalItem currSignalItem = null;
                foreach (PLCSignalItem signalItem in m_lstPLCSignalCollect)
                {
                    if (signalItem.PLCStationName.ToLower() == strPlcStationName.ToLower() &&
                        signalItem.PLCTemplateName.ToLower() == strPlcTemplateName.ToLower() &&
                        signalItem.SignalName.ToLower() == strSignalName.ToLower())
                    {
                        currSignalItem = signalItem;
                        break;
                    }
                }
                return currSignalItem;
            }

        }

        public PLCSignalItem this[int index]
        {
            get
            {
                if (index < 0 || index > m_lstPLCSignalCollect.Count || m_lstPLCSignalCollect == null)
                    return null;
                else
                    return m_lstPLCSignalCollect[index];
            }
            set
            {
                if (index < m_lstPLCSignalCollect.Count && index > -1 && m_lstPLCSignalCollect != null)
                    m_lstPLCSignalCollect[index] = value;
            }
        }

        public PLCSignalCollection(IList<PLCSignalItem> plcSignalItems)
        {
            m_lstPLCSignalCollect = plcSignalItems;
        }

        public PLCSignalCollection(PLCSignalItem[] plcSignalItems)
        {
            if (plcSignalItems != null && plcSignalItems.Length > 0)
            {
                m_lstPLCSignalCollect.Clear();
                for (int i = 0; i < plcSignalItems.Length; i++)
                {
                    m_lstPLCSignalCollect.Add(plcSignalItems[i]);
                }
            }
        }

        public bool MoveNext()
        {
            m_posIndex++;
            return (m_posIndex < m_lstPLCSignalCollect.Count);
        }

        public void Reset()
        {
            m_posIndex = -1;
        }

        public object Current
        {
            get
            {
                try
                {
                    if (m_lstPLCSignalCollect.Count == 0)
                    {
                        return null;
                    }
                    else if (m_lstPLCSignalCollect.Count == 1)
                    {
                        m_posIndex = 0;
                        return m_lstPLCSignalCollect[m_posIndex];
                    }
                    else if (m_posIndex >= 0 && m_posIndex < m_lstPLCSignalCollect.Count)
                    {
                        return m_lstPLCSignalCollect[m_posIndex];
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

        public int Count
        {
            get
            {
                if (m_lstPLCSignalCollect != null)
                    return m_lstPLCSignalCollect.Count;
                else
                    return 0;
            }
        }

        public void AddPLCSignalItem(PLCSignalItem plcSignalItem)
        {
            bool blnContained = IsContainPLCSignalItem(plcSignalItem);
            if (!blnContained)
                m_lstPLCSignalCollect.Add(plcSignalItem);
            else
            {
                //如果相同，更新数据
                foreach (PLCSignalItem currItem in m_lstPLCSignalCollect)
                {
                    if (currItem.PLCStationName.ToLower() == plcSignalItem.PLCStationName.ToLower() &&
                        currItem.PLCTemplateName.ToLower() == plcSignalItem.PLCTemplateName.ToLower() &&
                        currItem.SignalName.ToLower() == plcSignalItem.SignalName.ToLower())
                    {
                        currItem.DBUnit = plcSignalItem.DBUnit;
                        currItem.DWNO = plcSignalItem.DWNO;
                        currItem.Amount = plcSignalItem.Amount;
                        currItem.SignalValue = plcSignalItem.SignalValue;
                        break;
                    }
                }
            }
        }

        public bool IsContainPLCSignalItem(PLCSignalItem plcSignalItem)
        {
            bool blnContained = false;
            foreach (PLCSignalItem signalItem in m_lstPLCSignalCollect)
            {
                if (plcSignalItem.PLCStationName.ToLower() == signalItem.PLCStationName.ToLower() &&
                    plcSignalItem.PLCTemplateName.ToLower() == signalItem.PLCTemplateName.ToLower() &&
                    plcSignalItem.SignalName.ToLower() == signalItem.SignalName.ToLower())
                {
                    blnContained = true;
                    break;
                }
            }

            return blnContained;
        }

        /// <summary>
        /// 移除某个PLCSignal Item
        /// </summary>
        /// <param name="plcItem"></param>
        public void RemovePLCSignalItem(PLCSignalItem plcSignalItem)
        {
            foreach (PLCSignalItem item in m_lstPLCSignalCollect)
            {
                if (item.PLCStationName.ToLower() == plcSignalItem.PLCStationName.ToLower() &&
                    item.PLCTemplateName.ToLower() == plcSignalItem.PLCTemplateName.ToLower() &&
                    item.SignalName.ToLower() == plcSignalItem.SignalName.ToLower())
                {
                    m_lstPLCSignalCollect.Remove(item);
                    break;
                }
            }
        }

        /// <summary>
        /// 按索引移除某个PLCSignal Item
        /// </summary>
        /// <param name="index"></param>
        public void RemovePLCSignalItemAt(int index)
        {
            if (m_lstPLCSignalCollect.Count == 0 || m_lstPLCSignalCollect.Count < index) return;
            m_lstPLCSignalCollect.RemoveAt(index);
        }

        /// <summary>
        /// 移除所有元素
        /// </summary>
        public void Clear()
        {
            if (m_lstPLCSignalCollect != null)
            {
                m_lstPLCSignalCollect.Clear();
            }
        }

    }
}
