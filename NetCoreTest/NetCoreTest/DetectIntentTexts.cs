using System;
using System.Collections.Generic;
using System.Text;
using Google.Cloud.Dialogflow.V2;
using Google.Protobuf.Collections;
using Google.Rpc;
using Google.Apis.Auth.OAuth2;
using Grpc.Auth;
namespace NetCoreTest
{
    public class DetectIntentTexts
    {

        public static int DetectIntentFromTexts(string jsonPath, string projectId,
                                        string sessionId,
                                        string[] texts,
                                        string languageCode = "en-US")
        {
            var credential = GoogleCredential.FromFile(jsonPath)
                .CreateScoped(SessionsClient.DefaultScopes);
            Grpc.Core.Channel channel =
                new Grpc.Core.Channel(SessionsClient.DefaultEndpoint.ToString(),
                credential.ToChannelCredentials());
            var client = SessionsClient.Create(channel);
            foreach (var text in texts)
            {
                var response = client.DetectIntent(
                    session: new SessionName(projectId, sessionId),
                    queryInput: new QueryInput()
                    {
                        Text = new TextInput()
                        {
                            Text = text,
                            LanguageCode = languageCode
                        }
                    }
                );

                var queryResult = response.QueryResult;
                string responseId  = response.ResponseId;
                Status webhootStatus  =  response.WebhookStatus;

                RepeatedField<Context> oContext  =  response.QueryResult.OutputContexts;

                Console.WriteLine($"Query text: {queryResult.QueryText}");
                Console.WriteLine($"Intent detected: {queryResult.Intent.DisplayName}");
                Console.WriteLine($"Intent confidence: {queryResult.IntentDetectionConfidence}");
                Console.WriteLine($"Fulfillment text: {queryResult.FulfillmentText}");
                Console.WriteLine();
            }
            return 0;
        }
    }
}
