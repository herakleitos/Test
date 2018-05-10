using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chaint.Common.Devices.Devices
{
    public abstract class InkJetter
    {

        public abstract event RunMessageEventHandler OnRunMessage;
        public abstract event ReadStringArrivedHandler OnInkJetRetValue;

        public abstract InkJetterType DeviceType { get; }

        public abstract bool Connect();
        public abstract bool Disconnect();
        public abstract bool Write(string[] strLines);

        protected void SendMessage(RunMessageEventHandler OnMessageHandler, string strMsg)
        {
            if (OnMessageHandler != null) OnMessageHandler(this, strMsg);
        }


    }
}
