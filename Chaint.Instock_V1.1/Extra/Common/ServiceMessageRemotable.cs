using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;
using System.Runtime.Remoting.Channels.Tcp;
namespace CTWH.Common
{
    [Serializable]
    public class ServiceMessage
    {
        String strMessage;
        public ServiceMessage()
        {
        }
        public ServiceMessage(string str)
        {
            strMessage = str;
        }
        public string Message
        {
            get
            {
                return strMessage;
            }
            set
            {
                strMessage = value;
            }
        }
    }
    public class ServiceMessageRemotable : MarshalByRefObject
    {
        public ServiceMessage serviceMessage =
            new ServiceMessage();
        public ServiceMessageRemotable(string strMessage)
        {
            serviceMessage.Message = strMessage;
        }
        public ServiceMessageRemotable()
        {
           
        }

        public override Object InitializeLifetimeService()
        {
            return null;
        }
        public string Message
        {
            set
            {
                serviceMessage.Message = value;
            }
        }
    }
}
