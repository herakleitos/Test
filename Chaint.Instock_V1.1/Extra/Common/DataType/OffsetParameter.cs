using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CTWH.Common.DataType
{ 
    public struct OffsetParameter
    {
        public decimal LeftInterval;
        public decimal RightInterval;

        public OffsetParameter(decimal _LeftInterval, decimal _RightInterval)
        {
            LeftInterval = _LeftInterval;
            RightInterval = _RightInterval;
        }

        public OffsetParameter(string strPara)
        {
            string[] strs = strPara.Split(',');
            LeftInterval = Convert.ToDecimal(strs[0]);
            RightInterval = Convert.ToDecimal(strs[1]);
           
        }

        public string GetParameterString()
        {
            return String.Join(",", LeftInterval, RightInterval);
        }
    }
}
