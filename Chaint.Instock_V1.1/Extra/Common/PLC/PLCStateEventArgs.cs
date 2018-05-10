using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CTWH.Common.PLC
{
    public class PLCStateEventArgs : EventArgs
    {
        //private bool _IsConnect;
        private ushort _CurrenConNr;
        private Dictionary<ushort, bool> _ConnectionsIsConnect;
        private Dictionary<ushort, bool> _ConnectionsIsLastConnect;
        private string _Info;
        private PLCInfoType _Infotype;
        private bool _IsOK;//区分消息是异常还是成功
        public PLCStateEventArgs(PLCInfoType infoType, ushort currenConNr, Dictionary<ushort, bool> isConnect, Dictionary<ushort, bool> isLastConnect, bool isok, string msg)
        {
            this._Infotype = infoType;
            this._CurrenConNr = currenConNr;
            this._ConnectionsIsConnect = isConnect;
            this._ConnectionsIsLastConnect = isLastConnect;
            this._IsOK = isok;
            this._Info = msg;

        }

        public ushort CurrenConNr
        {
            get
            {
                return _CurrenConNr;
            }
        }

        public Dictionary<ushort, bool> IsConnect
        {
            get
            {
                return _ConnectionsIsConnect;
            }
        }
        public Dictionary<ushort, bool> IsLastConnect
        {
            get
            {
                return _ConnectionsIsLastConnect;
            }
        }
        public string Info
        {
            get
            {
                return _Info;
            }
        }
        public PLCInfoType Infotype
        {
            get
            {
                return _Infotype;
            }        
        }

        /// <summary>
        /// 区分消息是正常还是异常
        /// </summary>
        public bool IsOK
        {
            get
            {
                return _IsOK;
            }
        }
    }


    public enum PLCInfoType
    {
        None = 0x0,
        PLCRead = 0x1,
        PLCWrite = 0x2,
        PLCConnect = 0x4,
        PLCReConnect = 0x8,
        PLCSetActive= 0x16,
        PLCUnloadConnection = 0x32,
    }
}
