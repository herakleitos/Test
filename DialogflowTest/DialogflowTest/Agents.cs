using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace DialogflowTest
{
    public static class Agents
    {
        public static string Search(string projectId, string authJSONPath)
        {
            string token = TokenService.GetToken(authJSONPath).Result;
            string getIntentListFormat = "https://dialogflow.googleapis.com/v2/projects/{0}/agent:search";
            string getIntentListUrl = string.Format(getIntentListFormat, projectId);
            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, getIntentListUrl);
            HttpResponseMessage responseMsg = WebRequestAsync.GetAsync(message, token, 60).Result;
            var msg = responseMsg.Content.ReadAsStringAsync().Result;
            return msg;
        }
        public static string Export(string projectId, string authJSONPath)
        {
            string token = TokenService.GetToken(authJSONPath).Result;
            string getIntentListFormat = "https://dialogflow.googleapis.com/v2/projects/{0}/agent:export";
            string getIntentListUrl = string.Format(getIntentListFormat, projectId);
            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, getIntentListUrl);
            HttpResponseMessage responseMsg = WebRequestAsync.PostAsync(message, token, 60).Result;
            var msg = responseMsg.Content.ReadAsStringAsync().Result;
            JObject obj = JObject.Parse(msg);
            JToken jt;
            obj.TryGetValue("response", out jt);
            if (jt != null)
            {
                string content = jt.SelectToken("agentContent").ToString();
            }
            return msg;
        }
    }
}
