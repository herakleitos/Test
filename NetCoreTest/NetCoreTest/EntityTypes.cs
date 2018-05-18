using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;

namespace NetCoreTest
{
    public static class EntityTypes
    {
        public static string Create(string projectId, string authJSONPath)
        {
            string token = TokenService.GetToken(authJSONPath).Result;
            string createEntityTypesFormat = "https://dialogflow.googleapis.com/v2/projects/{0}/agent/entityTypes";
            string createEntityTypesUrl = string.Format(createEntityTypesFormat, projectId);
            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, createEntityTypesUrl);
            System.IO.FileStream fs = System.IO.File.OpenRead("..\\NetCoreTest\\json\\createEntityTypes.json");
            StreamReader sr = new StreamReader(fs, Encoding.UTF8);
            string content = sr.ReadToEnd();
            message.Content = new StringContent(content, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage responseMsg = WebRequestAsync.PostAsync(message, token, 60).Result;
            var msg = responseMsg.Content.ReadAsStringAsync().Result;
            return msg;
        }
    }
}
