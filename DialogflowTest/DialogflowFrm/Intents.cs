using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using Google.Cloud.Dialogflow.V2;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace DialogflowFrm
{
    public static class Intents
    {
        public static string Create(string projectId,string authJSONPath, string languageCode)
        {
            string token = TokenService.GetToken(authJSONPath).Result;
            string createEntityTypesFormat = "https://dialogflow.googleapis.com/v2/projects/{0}/agent/intents?languageCode={1}";
            string createEntityTypesUrl = string.Format(createEntityTypesFormat, projectId,languageCode);
            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, createEntityTypesUrl);
            System.IO.FileStream fs = System.IO.File.OpenRead("..\\..\\json\\createIntent.json");
            StreamReader sr = new StreamReader(fs, Encoding.UTF8);
            string content = sr.ReadToEnd();

            //Intent result = JsonConvert.DeserializeObject<Intent>(content);
            //Intent.Types.Parameter item = new Intent.Types.Parameter()
            //{
            //    DisplayName = "1111",
            //    EntityTypeDisplayName="@1111",
            //    Value ="$1111"
            //};
            //result.Parameters.Add(item);
            message.Content = new StringContent(content, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage responseMsg = WebRequestAsync.PostAsync(message,token, 60).Result;
            var msg = responseMsg.Content.ReadAsStringAsync().Result;
            return msg;
        }

        public static string GetList(string projectId, string authJSONPath,string languageCode)
        {
            string token = TokenService.GetToken(authJSONPath).Result;
            string getIntentListFormat = "https://dialogflow.googleapis.com/v2/projects/{0}/agent/intents?languageCode={1}";
            string getIntentListUrl = string.Format(getIntentListFormat, projectId, languageCode);
            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, getIntentListUrl);
            HttpResponseMessage responseMsg = WebRequestAsync.GetAsync(message, token, 60).Result;
            var msg = responseMsg.Content.ReadAsStringAsync().Result;
            return msg;
        }
    }
}
