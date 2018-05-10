using System;
using System.Collections;
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
 *      (1) 建立线程安全的队列作为缓存区模型
 * 
 ------------------------------------------------------------------------------------*/
    public class ThreadSafeQueue<T>
    {
        private Queue<T> m_Buffer = new Queue<T>();
        private Mutex m_Mutex = null;       //加入互斥信号量
        private int m_MaxCapacity = 4048;


        /// <summary>
        /// 队列中元素数量
        /// </summary>
        public int Count
        {
            get
            {
                m_Mutex.WaitOne();
                int temp = m_Buffer.Count;
                m_Mutex.ReleaseMutex();
                return temp;
            }
        }

        /// <summary>
        /// 队列是否为满
        /// </summary>
        public bool IsFull
        {
            get
            {
                bool retVal = false;
                m_Mutex.WaitOne();
                retVal = (m_Buffer.Count >= m_MaxCapacity);

                m_Mutex.ReleaseMutex();

                return retVal;
            }
        }

        /// <summary>
        /// 队列是否为空
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                bool retVal = false;
                m_Mutex.WaitOne();
                retVal = (m_Buffer.Count == 0);
                m_Mutex.ReleaseMutex();
                return retVal;
            }
        }


        public ThreadSafeQueue()
            : this("", 4048)
        {

        }

        public ThreadSafeQueue(int maxCapacity)
            : this("", maxCapacity)
        {

        }

        public ThreadSafeQueue(string strName, int maxCapacity)
        {
            m_MaxCapacity = maxCapacity;
            if (strName.Trim().Length > 0)
                m_Mutex = new Mutex(true, strName);
            else
                m_Mutex = new Mutex();
        }

        /// <summary>
        /// 对象入队列 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool PushObject(T obj)
        {
            bool blnRet = true;

            m_Mutex.WaitOne();
            if (m_Buffer.Count >= m_MaxCapacity)
            {
                blnRet = false;
            }
            else
            {
                m_Buffer.Enqueue(obj);
                blnRet = true;
            }
            m_Mutex.ReleaseMutex();

            return blnRet;
        }


        /// <summary>
        /// 对象出队列
        /// </summary>
        /// <returns></returns>
        public T PopObject()
        {
            T currObj = default(T);//返回值不一定是null

            m_Mutex.WaitOne();

            if (m_Buffer.Count > 0) currObj = m_Buffer.Dequeue();

            m_Mutex.ReleaseMutex();

            return currObj;
        }

    }
}
