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
namespace DialogflowTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string a = string.Empty;
            for (int k = 0; k < 100; k++)
            {
                a += k.ToString() + ",";
            }


            int i = 1;
            //while (true)
            //{
                string jsonPath = "..\\DialogflowTest\\json\\My Project-2c0ddc26d37d.json";
            string projectId = "geman-testbot";
            string sessionId = Guid.NewGuid().ToString();
            string operationName = "fd1ed6be-49d0-4a4d-b559-452f423e58c4";
            //string result = Intents.GetList(projectId, jsonPath);
            //string result = DetectIntentTexts.DetectIntentFromTexts(projectId,sessionId,jsonPath);
            //string result = Intents.Create(projectId, jsonPath);
            string result = EntityTypes.Create(projectId, jsonPath);
            //string result = TrainBot.Train(projectId, jsonPath);
            //string result = Operation.GetOperation(projectId, operationName, jsonPath);
            //string result = TestApi.SendRequest("http://localhost:61973/api/values/test/123",HttpMethod.Post);
            //string result = Agents.Export(projectId, jsonPath);

            #region automapper
            //AutoMapper.Mapper.Initialize(config =>
            //{
            //    //config.CreateMap<humanDto, human>().ForMember(dest => dest.id,opt=>opt.MapFrom(src=>0));
            //    config.CreateMap<humanDto, human>().ForMember("id", dest => dest.MapFrom(src => 12));
            //});
            //humanDto hmDto = new humanDto();
            //hmDto.name = "lily";
            //hmDto.sex = "female";
            //hmDto.birthday = DateTime.UtcNow;
            //var hm = AutoMapper.Mapper.Map<human>(hmDto);
            //Console.Clear();
            #endregion
            Console.WriteLine(DateTime.Now);
                Console.WriteLine(string.Format("{0}{1}{2}{3}", i, i, i, i));
                Console.WriteLine(result);
                i++;
                //Console.Read();
                Thread.Sleep(1000);
            //}
        }

    }
    public class humanDto
    {
        [Required]
        public string name { get; set; }
        [Required]
        public string sex { get; set; }
        [Required]
        public DateTime birthday { get; set; }
    }
    [Table("t_human")]
    public class human
    {
        [Required]
        public int id { get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        public string sex { get; set; }
        [Required]
        public DateTime birthday { get; set; }
    }
}