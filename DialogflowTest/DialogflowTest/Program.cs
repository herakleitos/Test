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
using AutoMapper.Mappers;
using System.Threading;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using AutoMapper;

namespace DialogflowTest
{
    class Program
    {
        static void Main(string[] args)
        {

            string aaa = utils.CreateMD5Hash("123456789@#$%");
            string bbb = utils.CreateMD5Hash("123456789@#$%");
            string ccc = utils.CreateMD5Hash("123456789@#$%");

            string ddd = utils.CreateMD5Hash("qqqqqqqqqqqqqq");
            string eee = utils.CreateMD5Hash("qqqqqqqqqqqqqq");
            string fff = utils.CreateMD5Hash("book ticket ---1");

            string ggg = utils.CreateMD5Hash("&*()^%&^%$&^$&^%#$%");
            string hhh = utils.CreateMD5Hash("&*()^%&^%$&^$&^%#$%");
            string iii = utils.CreateMD5Hash("&*()^%&^%$&^$&^%#$%");

            Mapper.Initialize(ini => ini.AddProfiles(new[] { "DialogflowTest" }));

            a test = new a();
            test.a_name = "aaa";

            string json = JsonConvert.SerializeObject(test);

            b testbb = JsonConvert.DeserializeObject<b>(json);




            b testb = Mapper.Map<b>(test);



            string jsonPath = "..\\DialogflowTest\\json\\My Project-2c0ddc26d37d.json";
            string projectId = "werwe-3147f";
            string sessionId = Guid.NewGuid().ToString();
            string operationName = "fd1ed6be-49d0-4a4d-b559-452f423e58c4";

            //string result = Intents.GetList(projectId, jsonPath);
            //string result = Intents.Create(projectId, jsonPath);
            //string result = EntityTypes.Create(projectId, jsonPath);
            //string result = TrainBot.Train(projectId, jsonPath);
            //string result = Operation.GetOperation(projectId, operationName, jsonPath);
            //string result = TestApi.SendRequest("http://localhost:61973/api/values/test/123",HttpMethod.Post);
            //string result = Agents.Export(projectId, jsonPath);

            //随机数
            //byte[] randomBytes = new byte[4];
            //RNGCryptoServiceProvider rngServiceProvider = new RNGCryptoServiceProvider();
            //rngServiceProvider.GetBytes(randomBytes);
            //Int32 result = BitConverter.ToInt32(randomBytes, 0);

            //string questionsStr = Myfile.Read("E:\\", "question.txt");
            //List<string> questions = questionsStr.Split('\r').Where(w=>!string.IsNullOrWhiteSpace(w)).Select(s=>s.Replace('\n',' ').Trim()).ToList();
            //int i = 1;
            //foreach (string question in questions)
            //{
            //    string result = DetectIntentTexts.DetectIntentFromTexts(question,projectId, sessionId, jsonPath);
            //    Console.WriteLine(i);
            //    i++; 
            //}
            Console.WriteLine("completed!");
            Console.Read();
        }
    }

    public class a
    {
        public string a_name { get; set; }
        public Dictionary<int, string> a_id { get; set; }
    }
    public class b
    {
        public string a_name { get; set; }
        public Dictionary<int, string> a_id { get; set; }
    }

    public enum EnumsTest
    {
         enums_aaa,
        enums_bbb
    }

    public class CustomMapping : Profile
    {
        public CustomMapping()
        {
            CreateMap<a,b>();
        }
    }

    public static class utils
    {
        public static string CreateMD5Hash(string input)
        {
            // Use input string to calculate MD5 hash
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // Convert the byte array to hexadecimal string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
                // To force the hex string to lower-case letters instead of
                // upper-case, use he following line instead:
                // sb.Append(hashBytes[i].ToString("x2")); 
            }
            return sb.ToString();
        }
    }
}