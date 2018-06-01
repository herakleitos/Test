using System;
using System.Collections.Generic;
using System.Text;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Dialogflow.V2;
using System.Threading.Tasks;
using System.IO;

namespace DialogflowFrm
{
    public static class TokenService
    {
        public static async Task<string> GetToken(string jsonPath)
        {
            System.IO.FileStream fs = System.IO.File.OpenRead(jsonPath);
            StreamReader sr = new StreamReader(fs, Encoding.UTF8);
            string content = sr.ReadToEnd();
            var credential = GoogleCredential.FromJson(content).CreateScoped(EntityTypesClient.DefaultScopes);
            return await credential.UnderlyingCredential.GetAccessTokenForRequestAsync();
        }
    }
}
