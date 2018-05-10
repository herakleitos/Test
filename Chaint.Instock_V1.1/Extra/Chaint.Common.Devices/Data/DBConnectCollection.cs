using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Chaint.Common.Devices.Data
{

    /// <summary>
    /// ĳ��DB����  �����SQL ACCESS
    /// </summary>
    public class DBConnect : IEnumerable
    {
        private string m_TemplateName = "";
        private string m_Connectstring = "";

        private DBAccessFunc m_DBAccessor = null;

        #region ���Զ���
        public string TemplateName
        {
            get { return m_TemplateName; }
            set { m_TemplateName = value; }
        }

        public string ConnectString
        {
            get { return m_Connectstring; }
            set { m_Connectstring = value; }
        }

        /// <summary>
        /// DB������
        /// </summary>
        public DBAccessFunc DBAccessor
        {
            get { return m_DBAccessor; }
           // set { m_DBAccessor = value; }
        }

        #endregion

        public DBConnect(string templateName, string connectString)
        {
            m_TemplateName = templateName;
            m_Connectstring = connectString;
            m_DBAccessor = DBAccessorFactory.GetDBAccessor(connectString);
        }

        private DBConnect[] m_DBConnects = null;
        public DBConnect(DBConnect[] dbconnects)
        {
            m_DBConnects = new DBConnect[dbconnects.Length];
            dbconnects.CopyTo(m_DBConnects, 0);
        }

        public IEnumerator GetEnumerator()//ʵ��GetEnumerator()�ӿ�
        {
            return new DBConnectCollection(m_DBConnects);
        }
    }

    /// <summary>
    /// DB���Ӽ���
    /// </summary>
    public class DBConnectCollection :IEnumerator
    {
        private IList<DBConnect> m_lstDBConnectCollection = new List<DBConnect>();
        private int m_posIndex = -1;                                      //λ����Ϣ

        /// <summary>
        /// ʵ��Foreach������
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()//ʵ��GetEnumerator()�ӿ�
        {
            return new DBConnectCollection(m_lstDBConnectCollection);
        }

         /// <summary>
        /// ʵ�ֿչ��캯��
        /// </summary>
        public DBConnectCollection()
        {

        }

        public DBConnect this[string strTemplateName]
        {
            get
            {
                DBConnect currDBConnect = null;
                foreach (DBConnect dbconnect in m_lstDBConnectCollection)
                {
                    if (dbconnect.TemplateName.ToLower() == strTemplateName.ToLower())
                    {
                        currDBConnect = dbconnect;
                        break;
                    }
                }
                return currDBConnect;
            }
        }

        public DBConnect this[int index]
        {
            get
            {
                if (index < 0 || index > m_lstDBConnectCollection.Count || m_lstDBConnectCollection == null)
                    return null;
                else
                    return m_lstDBConnectCollection[index];
            }
            set
            {
                if (index < m_lstDBConnectCollection.Count && index > -1 && m_lstDBConnectCollection != null)
                    m_lstDBConnectCollection[index] = value;
            }
        }

        public DBConnectCollection(IList<DBConnect> lstDBConnect)
        {
            m_lstDBConnectCollection = lstDBConnect;
        }

        public DBConnectCollection(DBConnect[] dbConnects)
        {
            if (dbConnects != null && dbConnects.Length > 0)
            {
                m_lstDBConnectCollection.Clear();
                //for (int i = 0; i < dbConnects.Length; i++)
                //{
                //    m_lstDBConnectCollection.Add(dbConnects[i]);
                //}

                m_lstDBConnectCollection.CopyTo(dbConnects, 0);
            }
        }

        public bool MoveNext()
        {
            m_posIndex++;
            return (m_posIndex < m_lstDBConnectCollection.Count);
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
                    if (m_lstDBConnectCollection.Count == 0)
                    {
                        return null;
                    }
                    else if (m_lstDBConnectCollection.Count == 1)
                    {
                        m_posIndex = 0;
                        return m_lstDBConnectCollection[m_posIndex];
                    }
                    else if (m_posIndex >= 0 && m_posIndex < m_lstDBConnectCollection.Count)
                    {
                        return m_lstDBConnectCollection[m_posIndex];
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
                if (m_lstDBConnectCollection != null)
                    return m_lstDBConnectCollection.Count;
                else
                    return 0;
            }
        }

        public void AddDBConnect(DBConnect dbConnect)
        {
            bool blnContained = IsContainDBItem(dbConnect);
            if (!blnContained)
                m_lstDBConnectCollection.Add(dbConnect);
            else
            {
                //�����ͬ����������
                foreach (DBConnect currItem in m_lstDBConnectCollection)
                {
                    if (currItem.TemplateName.ToLower() == dbConnect.TemplateName.ToLower())
                    {
                        currItem.ConnectString = dbConnect.ConnectString;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// �Ƿ��������
        /// </summary>
        /// <param name="dbConnect"></param>
        /// <returns></returns>
        public bool IsContainDBItem(DBConnect dbConnect)
        {
            bool blnContained = false;
            foreach (DBConnect currItem in m_lstDBConnectCollection)
            {
                if (currItem.TemplateName.ToLower() == dbConnect.TemplateName.ToLower())
                {
                    blnContained = true;
                    break;
                }
            }

            return blnContained;
        }

        /// <summary>
        /// �Ƴ�ĳ��DB����
        /// </summary>
        /// <param name="plcItem"></param>
        public void RemoveDBConn(DBConnect dbConnect)
        {
            foreach (DBConnect currItem in m_lstDBConnectCollection)
            {
                if (currItem.TemplateName.ToLower() == dbConnect.TemplateName.ToLower())
                {
                    m_lstDBConnectCollection.Remove(currItem);
                    break;
                }
            }
        }

        /// <summary>
        /// �Ƴ�ĳ��DB���� ������
        /// </summary>
        /// <param name="index"></param>
        public void RemoveDBConnAt(int index)
        {
            if (m_lstDBConnectCollection.Count == 0 || m_lstDBConnectCollection.Count < index) return;
            m_lstDBConnectCollection.RemoveAt(index);
        }

        /// <summary>
        /// �Ƴ�����Ԫ��
        /// </summary>
        public void Clear()
        {
            if (m_lstDBConnectCollection != null)
            {
                m_lstDBConnectCollection.Clear();
            }
        }

    }

}
