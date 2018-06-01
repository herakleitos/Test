using Google.Cloud.Dialogflow.V2;
using Google.LongRunning;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace DialogflowFrm
{
    public  static  class TrainBot
    {
        public static string tocken = string.Empty;

        public static string Train(string projectId,string authJSONPath)
        {
            if (string.IsNullOrWhiteSpace(tocken))
            {
                tocken = TokenService.GetToken(authJSONPath).Result;
            }
            string trainFormat = "https://dialogflow.googleapis.com/v2/projects/{0}/agent:train";
            string trainUrl = string.Format(trainFormat, projectId);
            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, trainUrl);
            HttpResponseMessage responseMsg = WebRequestAsync.PostAsync(message, TrainBot.tocken, 60).Result;
            var msg = responseMsg.Content.ReadAsStringAsync().Result;
            //var response =
            //    JsonConvert.DeserializeObject<Operation>(msg);
            return msg;
        }
    }
}
