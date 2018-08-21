using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
namespace DialogflowFrm
{
    public static class WebRequestAsync
    {
        public static async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, string token, int timeOut)
        {
            HttpResponseMessage responseMessage = null;
            using (var client = new HttpClient())
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                client.Timeout = TimeSpan.FromSeconds(timeOut);
                responseMessage = await client.SendAsync(request);
            }
            return responseMessage;
        }
        public static async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, int timeOut)
        {
            HttpResponseMessage responseMessage = null;
            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(timeOut);
                responseMessage = await client.SendAsync(request);
            }
            return responseMessage;
        }
        public static async Task<HttpResponseMessage> PostAsync(HttpRequestMessage request, string token, int timeOut)
        {
            HttpResponseMessage responseMessage = null;
            request.Method = HttpMethod.Post;
            responseMessage = await SendAsync(request,token, timeOut);
            return responseMessage;
        }
        public static async Task<HttpResponseMessage> DeleteAsync(HttpRequestMessage request, string token, int timeOut)
        {
            HttpResponseMessage responseMessage = null;
            request.Method = HttpMethod.Delete;
            responseMessage = await SendAsync(request, token, timeOut);
            return responseMessage;
        }
        public static async Task<HttpResponseMessage> PostAsync(HttpRequestMessage request, int timeOut)
        {
            HttpResponseMessage responseMessage = null;
            request.Method = HttpMethod.Post;
            responseMessage = await SendAsync(request, timeOut);
            return responseMessage;
        }
        public static async Task<HttpResponseMessage> GetAsync(HttpRequestMessage request, int timeOut)
        {
            HttpResponseMessage responseMessage = null;
            request.Method = HttpMethod.Get;
            responseMessage = await SendAsync(request, timeOut);
            return responseMessage;
        }
    }
}
