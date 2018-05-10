using System;
using System.Collections.Generic;
using System.Text;

namespace CTWH.Common
{
    //数据库连接 断开 事件
    public class SqlStateEventArgs : EventArgs
    {
        private bool _IsConnect;
        private string _Info;
        public SqlStateEventArgs(bool isConnect, string msg)
        {
            this._IsConnect = isConnect;
            this._Info = msg;

        }
        public bool IsConnect
        {
            get
            {
                return _IsConnect;
            }
        }
        public string Info
        {
            get
            {
                return _Info;
            }
        }
    }
}
