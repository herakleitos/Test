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
namespace DialogflowTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string jsonPath = "..\\DialogflowTest\\json\\My Project-2c0ddc26d37d.json";
            string projectId = "regal-sled-204110";
            string sessionId = Guid.NewGuid().ToString();

            //string result = DetectIntentTexts.DetectIntentFromTexts(projectId,sessionId,jsonPath);
            //string result = Intents.Create(projectId, jsonPath);

            //string result = TestApi.SendRequest("http://localhost:61973/api/values/test/123",HttpMethod.Post);

            AutoMapper.Mapper.Initialize(config =>
            {
                //config.CreateMap<humanDto, human>().ForMember(dest => dest.id,opt=>opt.MapFrom(src=>0));
                config.CreateMap<humanDto, human>().ForMember("id", dest => dest.MapFrom(src => 12));
            });

            humanDto hmDto = new humanDto();
            hmDto.name = "lily";
            hmDto.sex = "female";

            var hm = AutoMapper.Mapper.Map<human>(hmDto);

            //Console.WriteLine(result);
            Console.Read();
        }

    }
    public class humanDto
    {
        public string name { get; set; }
        public string sex { get; set; }
    }
    public class human
    {
        public int id { get; set; }
        public string name { get; set; }
        public string sex { get; set; }
    }
}