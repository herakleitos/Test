using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace DialogflowFrm
{
    public static class Operation
    {
        public static string GetOperation(string projectId, string operateName, string authJSONPath)
        {
            string token = TokenService.GetToken(authJSONPath).Result;
            string trainFormat = "https://dialogflow.googleapis.com/v2/projects/{0}/operations/{1}";
            string trainUrl = string.Format(trainFormat, projectId, operateName);
            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, trainUrl);
            HttpResponseMessage responseMsg = WebRequestAsync.GetAsync(message, token, 60).Result;
            var msg = responseMsg.Content.ReadAsStringAsync().Result;
            //var response =
            //    JsonConvert.DeserializeObject<Operation>(msg);
            return msg;
        }
    }
}
