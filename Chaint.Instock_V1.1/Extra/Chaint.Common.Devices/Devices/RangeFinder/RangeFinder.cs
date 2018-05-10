using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chaint.Common.Devices.Devices
{
    public abstract class RangeFinder
    {
        public abstract event RunMessageEventHandler OnRunMessage;
        public abstract event ReadStringArrivedHandler OnRetMeasureValue;

        public abstract RangeFinderType DeviceType { get; }

        public abstract bool Connect();
        public abstract bool Disconnect();
      
        protected void SendMessage(RunMessageEventHandler OnMessageHandler, string strMsg)
        {
            if (OnMessageHandler != null) OnMessageHandler(this, strMsg);
        }


    }
}
