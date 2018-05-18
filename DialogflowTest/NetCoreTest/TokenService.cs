using System;
using System.Collections.Generic;
using System.Text;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Dialogflow.V2;
using System.Threading.Tasks;
namespace NetCoreTest
{
    public static class TokenService
    {
        public static async Task<string> GetToken(string jsonPath)
        {
            var credential = GoogleCredential.FromFile(jsonPath).CreateScoped(EntityTypesClient.DefaultScopes);
            return await credential.UnderlyingCredential.GetAccessTokenForRequestAsync();
        }
    }
}
