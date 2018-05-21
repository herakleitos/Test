using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace DialogflowTest
{
    public static class Tools
    {
        public static string GetStringValue(this JObject jo, string key)
        {
            JToken value = null;
            if (jo.TryGetValue(key, out value))
            {
                return Convert.ToString(value);
            }
            return String.Empty;
        }
    }
}
