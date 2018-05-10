using System;
using System.Collections.Generic;
using System.Text;

namespace Chaint.Common.Devices.Devices
{
    public abstract class Scanner
    {
        public abstract event RunMessageEventHandler OnRunMessage;
        public abstract event ReadStringArrivedHandler OnBarcodeValue;

        public abstract ScanType DeviceType { get;}

        public abstract bool Connect();
        public abstract bool Disconnect();
        public abstract string Read();
        public abstract bool Write(byte[] bytData);
        public abstract bool Write(string strData);

        protected void SendMessage(RunMessageEventHandler OnMessageHandler, string strMsg)
        {
            if (OnMessageHandler != null) OnMessageHandler(this,strMsg);
        }
    }


}
