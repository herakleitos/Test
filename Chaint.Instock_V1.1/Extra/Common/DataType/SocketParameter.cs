using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CTWH.Common.DataType
{
    public struct SocketParameter
    {
        public string[] IPAddresss;
        public int[] SocketPorts;
        public Encoding SocketEncoding;

        public SocketParameter(string[] _IPAddresss, int[] _SocketPort, Encoding _SocketEncoding)
        {
            IPAddresss = _IPAddresss;
            SocketPorts = _SocketPort;
            SocketEncoding = _SocketEncoding;  
        }

        public int SocketPort
        {
            get
            {
                return SocketPorts[0];
            }
        }

        public string IPAddress
        {
            get
            {
                return IPAddresss[0];
            }        
        }

        public SocketParameter(string strPara)
        {
            string[] strs = strPara.Split(',');
            //IPAddress = strs[0];
            string[] strIPs = strs[0].Split('/');
            IPAddresss = new string[strIPs.Length];
            for (byte c = 0; c < strIPs.Length; c++)
            {
                IPAddresss[c] =strIPs[c];
            }


            string[] strPorts = strs[1].Split('/');
            SocketPorts = new int[strPorts.Length];
            for (byte c = 0; c < strPorts.Length; c++)
            {
                SocketPorts[c] = Convert.ToInt32(strPorts[c]);
            }

            SocketEncoding = Encoding.GetEncoding(strs[2]);
        }

        public string GetParameterString()
        {
            return String.Join(",", String.Join("/", IPAddresss), String.Join("/",SocketPorts), SocketEncoding.BodyName);
        }
    }
}
