using System;
using Google.Cloud.Dialogflow.V2;
using Google.Protobuf.Collections;
using Google.Rpc;
using Google.LongRunning;
using Google.Apis.Auth.OAuth2;
using Grpc.Auth;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Linq;

namespace NetCoreTest
{
    class Program
    {
        static void Main(string[] args)
        {
            test bbb = new test();
            string jsonPath = "..\\NetCoreTest\\My Project-2c0ddc26d37d.json";
            string projectId = "regal-sled-204110";
            ////string result  = EntityTypesOperate.Create(jsonPath, projectId);
            //string createEntityTypesFormat = "https://dialogflow.googleapis.com/v2/projects/{0}/agent/entityTypes";
            //string createEntityTypesUrl = string.Format(createEntityTypesFormat, projectId);

            string sessionId = Guid.NewGuid().ToString();
            string detectInternFormat = "https://dialogflow.googleapis.com/v2/projects/{0}/agent/sessions/{1}:detectIntent";
            string detectInternUrl = string.Format(detectInternFormat, projectId, sessionId);

            //string trainFormat = "https://dialogflow.googleapis.com/v2/projects/{0}/agent:train";


            //HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, createEntityTypesUrl);
            //System.IO.FileStream fs = System.IO.File.OpenRead("..\\NetCoreTest\\createEntityTypes.txt");
            //StreamReader sr = new StreamReader(fs, Encoding.UTF8);
            //string content = sr.ReadToEnd();
            //message.Content = new StringContent(content, System.Text.Encoding.UTF8, "application/json");

            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, detectInternUrl);
            System.IO.FileStream fs = System.IO.File.OpenRead("..\\NetCoreTest\\detectIntern.txt");
            StreamReader sr = new StreamReader(fs, Encoding.UTF8);
            string content = sr.ReadToEnd();
            message.Content = new StringContent(content, System.Text.Encoding.UTF8, "application/json");



            //string trainUrl = string.Format(trainFormat, projectId);
            string token = TokenService.GetToken(jsonPath).Result;

            HttpResponseMessage responseMsg = WebRequestAsync.PostAsync(message, token, 60).Result;

            var msg = responseMsg.Content.ReadAsStringAsync().Result;

            var  response = 
                JsonConvert.DeserializeObject<DetectIntentResponse>(msg);
            JObject intent = JObject.FromObject(response.QueryResult.Intent);
            JObject parameters = JObject.FromObject(response.QueryResult.Parameters);
           
            Console.WriteLine(msg);

            Console.WriteLine("**************************************************************");

            Console.WriteLine(intent.GetStringValue("Name"));

            string[] splitName = intent.GetStringValue("Name").Split('/');

            for (int i = 0; i < splitName.Length; i++)
            {
                Console.WriteLine(string.Format("{0}----{1}",i,splitName[i]));
            }
            //textContent tc = new textContent();
            //tc.language = "en";
            //tc.text = "hello";
            //DetectIntentDto content = new DetectIntentDto();
            //queryInput qi = new queryInput();
            //qi.text = tc;
            //content.queryInput = qi;
            //string json = JsonConvert.SerializeObject(content, Formatting.None);
            //Console.Write(json);
            Console.Read();
        }

    }
    public static class tools
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

    public class test
    {
        public JObject aaa { get; set; }
    }
    public class DetectIntentDto
    {
        public queryInput queryInput { get;set;}
    }
    public class queryInput
    {
        public textContent text { get; set; }
    }
    public class textContent
    {
        public string text { get; set; }
        public string language { get; set; }
    }
}