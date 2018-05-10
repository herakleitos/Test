using System;
using System.Collections.Generic;
using System.Text;

namespace CTSocket
{
    public sealed class BufferManager
    {
        /// <summary>
        /// Initializes a new instance of the BufferManager class.
        /// </summary>
        public BufferManager()
        {
            
        }



        //private byte[] m_receiveBuffer;
        //private byte[] m_sendBuffer;

        //private int m_maxSessionCount;
        //private int m_receiveBufferSize;
        //private int m_sendBufferSize;

        //private int m_bufferBlockIndex;
        //private Stack<int> m_bufferBlockIndexStack;

        //public BufferManager(int maxSessionCount, int receivevBufferSize, int sendBufferSize)
        //{
        //    m_maxSessionCount = maxSessionCount;
        //    m_receiveBufferSize = receivevBufferSize;
        //    m_sendBufferSize = sendBufferSize;

        //    m_bufferBlockIndex = 0;
        //    m_bufferBlockIndexStack = new Stack<int>();

        //    m_receiveBuffer = new byte[m_receiveBufferSize * m_maxSessionCount];
        //    m_sendBuffer = new byte[m_sendBufferSize * m_maxSessionCount];
        //}

        //public int ReceiveBufferSize
        //{
        //    get { return m_receiveBufferSize; }
        //}

        //public int SendBufferSize
        //{
        //    get { return m_sendBufferSize; }
        //}

        //public byte[] ReceiveBuffer
        //{
        //    get { return m_receiveBuffer; }
        //}

        //public byte[] SendBuffer
        //{
        //    get { return m_sendBuffer; }
        //}

        //public void FreeBufferBlockIndex(int bufferBlockIndex)
        //{
        //    if (bufferBlockIndex == -1)
        //    {
        //        return;
        //    }

        //    lock (this)
        //    {
        //        m_bufferBlockIndexStack.Push(bufferBlockIndex);
        //    }
        //}

        //public int GetBufferBlockIndex()
        //{
        //    lock (this)
        //    {
        //        int blockIndex = -1;

        //        if (m_bufferBlockIndexStack.Count > 0)  // 有用过释放的缓冲块
        //        {
        //            blockIndex = m_bufferBlockIndexStack.Pop();
        //        }
        //        else
        //        {
        //            if (m_bufferBlockIndex < m_maxSessionCount)  // 有未用缓冲区块
        //            {
        //                blockIndex = m_bufferBlockIndex++;
        //            }
        //        }

        //        return blockIndex;
        //    }
        //}

        //public int GetReceivevBufferOffset(int bufferBlockIndex)
        //{
        //    if (bufferBlockIndex == -1)  // 没有使用共享块
        //    {
        //        return 0;
        //    }

        //    return bufferBlockIndex * m_receiveBufferSize;
        //}

        //public int GetSendBufferOffset(int bufferBlockIndex)
        //{
        //    if (bufferBlockIndex == -1)  // 没有使用共享块
        //    {
        //        return 0;
        //    }

        //    return bufferBlockIndex * m_sendBufferSize;
        //}

        //public void Clear()
        //{
        //    lock (this)
        //    {
        //        m_bufferBlockIndexStack.Clear();
        //        m_receiveBuffer = null;
        //        m_sendBuffer = null;
        //    }
        //}



    }
}
