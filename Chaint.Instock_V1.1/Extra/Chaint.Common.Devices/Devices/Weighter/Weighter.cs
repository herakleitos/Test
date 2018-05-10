using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chaint.Common.Devices.Devices
{
    public abstract class Weighter
    {
        public abstract event RunMessageEventHandler OnRunMessage;
        public abstract event ReadStringArrivedHandler OnWeightValue;


        public abstract WeighterType DeviceType { get; }


        public abstract bool Connect();
        public abstract bool Disconnect();
       // public abstract string Read();

        protected void SendMessage(RunMessageEventHandler OnMessageHandler, string strMsg)
        {
            if (OnMessageHandler != null) OnMessageHandler(this, strMsg);
        }
    }






}
