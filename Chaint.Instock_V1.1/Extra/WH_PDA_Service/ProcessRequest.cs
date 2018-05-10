using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CTWH.Common;

namespace WH_PDA_Service
{
    class ProcessRequest
    {
        CTWH.Common.MSSQL.WMSAccess access;
        public ProcessRequest() { }

        public string Process_Q08(string msg){


            string[] msgs = new string[] { "QA08","11","12","16","21","23","24","27"};
            return Utils.WMSMessage.MakeWMSSocketMsg(msgs);
        }
    }
}
