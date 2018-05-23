using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json;
namespace DialogflowTest
{
    public  static class TestApi
    {
        public static string SendRequest(string url, HttpMethod method)
        {
            HttpRequestMessage request = new HttpRequestMessage();
            HttpRequestMessage message = new HttpRequestMessage(method, url);
            People p = new People();
            p.FirstName = "aaaa";
            p.LastName = "bbbb";
            //string json = JsonConvert.SerializeObject(p);
            string json = JsonConvert.SerializeObject("this is a test");
            message.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage responseMsg = WebRequestAsync.SendAsync(message, 60).Result;
            var msg = responseMsg.Content.ReadAsStringAsync().Result;
            return msg;
        }
    }
    public class People
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
