using System;
using System.Collections.Generic;
using System.Text;

namespace Chaint.Common.Devices.Data
{
    //���ݿ����� �Ͽ� �¼�
    public class SqlStateEventArgs : EventArgs
    {
        private bool m_IsConnect;
        private string m_Info;

        public SqlStateEventArgs(bool isConnect, string msg)
        {
            this.m_IsConnect = isConnect;
            this.m_Info = msg;

        }
        public bool IsConnect
        {
            get
            {
                return m_IsConnect;
            }
        }
        public string Info
        {
            get
            {
                return m_Info;
            }
        }
    }
}
