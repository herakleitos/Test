using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceProcess;

namespace CTWH.Common.DataType
{
    public struct ServiceParameter
    {
        public string ServiceName;
        public string DisplayName;
        public string ServiceFilePath;

        public ServiceController ServiceController; 

        public ServiceParameter(string _ServiceName,string _DisplayName, string _ServiceFilePath)
        {
            ServiceName = _ServiceName;
            DisplayName = _DisplayName;
            ServiceFilePath = _ServiceFilePath;
            if (System.IO.Path.IsPathRooted(ServiceFilePath))
            {
            }
            else
            {
                ServiceFilePath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + ServiceFilePath;
            }

            ServiceController = new ServiceController(ServiceName);
            ServiceController.ServiceName = ServiceName;
            ServiceController.DisplayName = DisplayName;
        }

        public ServiceParameter(string strPara)
        {
            string[] strs = strPara.Split(',');
            ServiceName = strs[0];
            DisplayName= strs[1]; 
            ServiceFilePath = strs[2];

            if (System.IO.Path.IsPathRooted(ServiceFilePath))
            {
            }
            else
            {
                ServiceFilePath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + ServiceFilePath;
            }

            ServiceController = new ServiceController(ServiceName);
            ServiceController.ServiceName = ServiceName;
            ServiceController.DisplayName = DisplayName;
             
        }

        public string GetParameterString()
        {
            return String.Join(",", ServiceName, DisplayName, ServiceFilePath);
        }
    }
}
