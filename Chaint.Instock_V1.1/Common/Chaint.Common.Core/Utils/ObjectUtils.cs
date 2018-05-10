using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chaint.Common.Core.Utils
{
    public static class ObjectUtils
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="showZero">为0显示</param>
        /// <param name="isDB">是否是BD类型</param>
        /// <returns></returns>
        public static object TrimEndZero(object value,bool showZero=false,bool isDB=false)
        {
            string str = Convert.ToString(value).TrimEnd('0').TrimEnd('.');
            if (str == "0"&&!showZero&&!isDB) return string.Empty;
            if (str == "0" && !showZero && isDB) return DBNull.Value;
            return str;
        } 
    }
}
