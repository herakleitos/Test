using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSync
{
    public static class DataOperatorFactory
    {
        public static DataOperator Create(string tableName, DataAccessor dataAccessor)
        {
            DataOperator controller;
            switch (tableName.ToUpperInvariant())
            {
                case "SPGL_XMDWXXB":
                    controller = new XMDWXXBOperator(dataAccessor);
                    break;
                case "SPGL_XMJBXXB":
                    controller = new XMJBXXBOperator(dataAccessor);
                    break;
                case "SPGL_XMSPSXBLXXB":
                    controller = new XMSPSXBLXXBOperator(dataAccessor);
                    break;
                case "SPGL_XMSPSXBLXXXXB":
                    controller = new XMSPSXBLXXXXBOperator(dataAccessor);
                    break;
                case "SPGL_DFXMSPLCXXB":
                    controller = new DFXMSPLCXXBOperator(dataAccessor);
                    break;
                case "SPGL_DFXMSPLCJDXXB":
                    controller = new DFXMSPLCJDXXBOperator(dataAccessor);
                    break;
                case "SPGL_DFXMSPLCJDSXXXB":
                    controller = new DFXMSPLCJDSXXXBOperator(dataAccessor);
                    break;
                default:
                    controller =null;
                    break;
            }
            return controller;
        }

        public static Dictionary<string,DataOperator> CreateAll(DataAccessor dataAccessor)
        {
            Dictionary<string, DataOperator> result = new Dictionary<string, DataOperator>();
            //项目
            result.Add("SPGL_XMDWXXB", new XMDWXXBOperator(dataAccessor));
            result.Add("SPGL_XMJBXXB", new XMJBXXBOperator(dataAccessor));
            result.Add("SPGL_XMSPSXBLXXB", new XMSPSXBLXXBOperator(dataAccessor));
            result.Add("SPGL_XMSPSXBLXXXXB", new XMSPSXBLXXXXBOperator(dataAccessor));

            //流程
            result.Add("SPGL_DFXMSPLCXXB", new DFXMSPLCXXBOperator(dataAccessor));
            result.Add("SPGL_DFXMSPLCJDXXB", new DFXMSPLCJDXXBOperator(dataAccessor));
            result.Add("SPGL_DFXMSPLCJDSXXXB", new DFXMSPLCJDSXXXBOperator(dataAccessor));

            return result;
        }
    }
}
