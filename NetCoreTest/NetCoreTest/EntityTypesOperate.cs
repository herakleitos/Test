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
    public class EntityTypesOperate
    {
        public static string Create(string jsonPath,string projectId)
        {
            var credential = GoogleCredential.FromFile(jsonPath)
                .CreateScoped(EntityTypesClient.DefaultScopes);
            Grpc.Core.Channel channel =
                new Grpc.Core.Channel(SessionsClient.DefaultEndpoint.ToString(),
                credential.ToChannelCredentials());

            EntityTypesSettings settings = new EntityTypesSettings();
            var client = EntityTypesClient.Create(channel, settings);
            var entityType = new EntityType();
            entityType.DisplayName = "electrical equipment";
            entityType.Entities.Add(new EntityType.Types.Entity() { Value = "freezer" });
            entityType.Entities.Add(new EntityType.Types.Entity() { Value = "washing machine" });
            entityType.Entities.Add(new EntityType.Types.Entity() { Value = "air conditioner" });
            entityType.Kind = EntityType.Types.Kind.List;
            var createdEntityType = client.CreateEntityType(
                parent: new ProjectAgentName(projectId),
                entityType: entityType
            );
            return createdEntityType.Name;
        }
    }
}
