using System;
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

namespace DialogflowFrm
{
    public class DetectIntentTexts
    {

        public static string DetectIntentFromTexts(string projectId, string sessionId, string authJSONPath,string question,string languageCode,bool isDelContext)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                //string token = "ya29.c.ElrHBatAIAlNZEjrQzyELWz2UGTz8gg8rm7nqRSJ0fupV8pWwLJZGDSpwfNeVLt0HTa6sh8WpQ3WYvlVTbE9EmLAB6cpZDPCj08bwq5jAiQvoNt_3LcAMjy5cS4";
                string token = TokenService.GetToken(authJSONPath).Result;
                string detectInternFormat = "https://dialogflow.googleapis.com/v2/projects/{0}/agent/sessions/{1}:detectIntent";
                string detectInternUrl = string.Format(detectInternFormat, projectId, sessionId);
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, detectInternUrl);
                System.IO.FileStream fs = System.IO.File.OpenRead("..\\..\\json\\detectIntern.json");
                StreamReader sr = new StreamReader(fs, Encoding.UTF8);
                string content = sr.ReadToEnd();
                content = content.Replace("***", question).Replace("###", languageCode);

                message.Content = new StringContent(content, System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage responseMsg = WebRequestAsync.PostAsync(message, token, 60).Result;
                var msg = responseMsg.Content.ReadAsStringAsync().Result;
                var response =
                    JsonConvert.DeserializeObject<DetectIntentResponse>(msg);
                sb.AppendLine(msg);
                if (response.QueryResult.Intent != null)
                {
                    JObject intent = JObject.FromObject(response.QueryResult.Intent);
                    sb.AppendLine("**************************************************************");
                    sb.AppendLine("Name : " + intent.GetStringValue("Name"));
                    sb.AppendLine("Displayname : " + intent.GetStringValue("DisplayName"));
                    string[] splitName = intent.GetStringValue("Name").Split('/');
                    for (int i = 0; i < splitName.Length; i++)
                    {
                        sb.AppendLine(string.Format("{0}----{1}", i, splitName[i]));
                    }
                }
                if (isDelContext)
                {
                    if (response != null && response.QueryResult != null && response.QueryResult.OutputContexts != null && response.QueryResult.OutputContexts.Count > 0)
                    {
                        foreach (var context in response.QueryResult.OutputContexts)
                        {
                            string deleteFormat = "https://dialogflow.googleapis.com/v2/{0}";
                            string deleteUrl = string.Format(deleteFormat, context.Name);
                            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, deleteUrl);
                            message.Content = new StringContent(question, System.Text.Encoding.UTF8, "application/json");
                            HttpResponseMessage delteResponse = WebRequestAsync.DeleteAsync(requestMessage, token, 60).Result;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                sb.AppendLine(ex.ToString());
            }
            return sb.ToString();
        }
    }
}
