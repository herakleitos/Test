﻿using System;
using System.Collections.Generic;
using System.Text;
using Google.Cloud.Dialogflow.V2;
using Google.Protobuf.Collections;
using Google.Rpc;
using Google.Apis.Auth.OAuth2;
using Grpc.Auth;
using System.Net.Http;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NetCoreTest
{
    public class DetectIntentTexts
    {

        public static string DetectIntentFromTexts(string projectId, string sessionId, string authJSONPath)
        {
            string token = TokenService.GetToken(authJSONPath).Result;
            string detectInternFormat = "https://dialogflow.googleapis.com/v2/projects/{0}/agent/sessions/{1}:detectIntent";
            string detectInternUrl = string.Format(detectInternFormat, projectId, sessionId);
            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, detectInternUrl);
            System.IO.FileStream fs = System.IO.File.OpenRead("..\\NetCoreTest\\json\\detectIntern.json");
            StreamReader sr = new StreamReader(fs, Encoding.UTF8);
            string content = sr.ReadToEnd();
            message.Content = new StringContent(content, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage responseMsg = WebRequestAsync.PostAsync(message, token, 60).Result;
            var msg = responseMsg.Content.ReadAsStringAsync().Result;
            var response =
                JsonConvert.DeserializeObject<DetectIntentResponse>(msg);
            StringBuilder sb = new StringBuilder();
            if (response.QueryResult.Intent != null)
            {
                JObject intent = JObject.FromObject(response.QueryResult.Intent);
                sb.AppendLine(msg);
                sb.AppendLine("**************************************************************");
                sb.AppendLine("Name : " + intent.GetStringValue("Name"));
                sb.AppendLine("Displayname : " + intent.GetStringValue("DisplayName"));
                string[] splitName = intent.GetStringValue("Name").Split('/');
                for (int i = 0; i < splitName.Length; i++)
                {
                    sb.AppendLine(string.Format("{0}----{1}", i, splitName[i]));
                }
            }
            else
            {
                return "没有内容";
            }
            return sb.ToString();
        }
    }
}
