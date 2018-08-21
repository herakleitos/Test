using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.IO.Compression;
using ICSharpCode.SharpZipLib.Zip;
using Google.Protobuf;
using System.Net.Http;

namespace DialogflowFrm
{
    public class Agent
    {
        public static string Restore(string projectId, string authJSONPath)
        {
            string filePath = GenerateFile();
            string zipPath = string.Format("{0}.zip", filePath);
            FastZip fastZip = new FastZip();
            fastZip.CreateZip(zipPath, filePath,true,"");
            string agentContent = string.Empty;
            string msg = "";
            using (FileStream fs = new FileStream(zipPath, FileMode.Open))
            {
                ByteString byteString = ByteString.FromStream(fs);
                agentContent = byteString.ToBase64();
                string token = TokenService.GetToken(authJSONPath).Result;
                string restoreAgentFormat = "https://dialogflow.googleapis.com/v2/projects/{0}/agent:restore";
                string restoreAgent = string.Format(restoreAgentFormat, projectId);
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, restoreAgent);
                string json = JsonConvert.SerializeObject(new { agentContent});
                message.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage responseMsg = WebRequestAsync.PostAsync(message, token, 60).Result;
                msg = responseMsg.Content.ReadAsStringAsync().Result;
            }
            return msg;
        }

        public static string Val<T>(T value)
        {
            return value.GetType().ToString();
        }

        public static string GenerateFile()
        {
            string path = "E:\\agent\\UploadFiles";
            Google.Cloud.Dialogflow.V2.Agent agent = new Google.Cloud.Dialogflow.V2.Agent();
            agent.DisplayName = "testAgent";
            agent.DefaultLanguageCode = "en";
            agent.SupportedLanguageCodes.Add("en-IN");
            package newPackage = new package();
            newPackage.version = "1.0.0";
            Myfile.Write(path, "agent.json", JsonConvert.SerializeObject(agent));
            Myfile.Write(path, "package.json", JsonConvert.SerializeObject(newPackage));
            List<string> filePaths = new List<string>();
            return path;
        }
    }
    public class package
    {
        public string version { get; set; }
    }
}
