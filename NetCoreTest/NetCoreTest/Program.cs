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
            string jsonPath = "..\\NetCoreTest\\json\\My Project-2c0ddc26d37d.json";
            string projectId = "regal-sled-204110";
            string sessionId = Guid.NewGuid().ToString();

            //string result = DetectIntentTexts.DetectIntentFromTexts(projectId,sessionId,jsonPath);
            string result = Intents.Create(projectId, jsonPath);
            Console.WriteLine(result);
            Console.Read();
        }

    }
}