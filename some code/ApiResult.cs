using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSync
{
    public class ApiResult
    {
        public ApiResult(bool _result, errorCode _errorCode, object _data)
        {
            result = _result;
            errorCode = _errorCode;
            data = _data;
        }
        public bool result { get; set; }
        public errorCode errorCode { get; set; }
        public object data { get; set; }
    }
}
