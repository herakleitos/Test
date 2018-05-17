﻿// Copyright(c) 2018 Google Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License"); you may not
// use this file except in compliance with the License. You may obtain a copy of
// the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
// WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the
// License for the specific language governing permissions and limitations under
// the License.
using Google.Cloud.Diagnostics.AspNetCore;
using Google.Cloud.Diagnostics.Common;
using GoogleHomeAspNetCoreDemoServer.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoogleHomeAspNetCoreDemoServer.Dialogflow
{
    public class DialogflowApp
    {
        // Conversations keyed by the DialogFlow conversation sessionID.
        private static readonly Dictionary<string, Conversation> conversations = new Dictionary<string, Conversation>();

        private readonly IExceptionLogger _exceptionLogger;
        private readonly ILogger<ConversationController> _logger;
        private readonly IManagedTracer _tracer;

        /// <summary>
        /// Creates a Dialogflow.
        /// </summary>
        /// <param name="exceptionLogger">Exception logger</param>
        /// <param name="logger">Regular logger</param>
        /// <param name="tracer">Tracer</param>
        public DialogflowApp(IExceptionLogger exceptionLogger, ILogger<ConversationController> logger, IManagedTracer tracer)
        {
            _exceptionLogger = exceptionLogger;
            _logger = logger;
            _tracer = tracer;
        }

        /// <summary>
        /// Given a snippet of HTML, it shows in the browser. 
        /// </summary>
        /// <param name="html">HTML to show</param>
        public static void Show(string html) => HomeController.SetPage(html);

        /// <summary>
        /// Renders a spech response.
        /// </summary>
        /// <param name="speech">Speech to render</param>
        /// <returns>The response sent back to the assistant</returns>
        public static string Tell(string speech)
        {
            var jobject = new JObject
            {
                ["fulfillmentText"] = speech
            };
            return JsonConvert.SerializeObject(jobject);
        }

        /// <summary>
        /// Handles received HTTP request. For details of the expected request, 
        /// please see Dialogflow fulfillment doc: https://dialogflow.com/docs/fulfillment
        /// </summary>
        /// <param name="httpRequest">HTTP request</param>
        /// <returns>A response to the request which usually includes a spoken fulfillment</returns>
        public async Task<string> HandleRequest(HttpRequest httpRequest)
        {
            using (_tracer.StartSpan(nameof(DialogflowApp)))
            {
                var request = await ConvRequest.ParseAsync(httpRequest);

                _logger.LogInformation($"Intent: '{request.IntentName}',  QueryText: '{request.QueryText}'");

                var conversation = GetOrCreateConversation(request);

                using (_tracer.StartSpan("Conversation"))
                {
                    return await conversation.HandleAsync(request);
                }
            }
        }

        /// <summary>
        /// Given a conversation request with a session id, either get the existing
        /// conversation or create a new one.
        /// </summary>
        /// <param name="convRequest">Conversation request</param>
        /// <returns>Conversation</returns>
        private Conversation GetOrCreateConversation(ConvRequest convRequest)
        {
            Conversation conversation;
            lock (conversations)
            {
                var sessionId = convRequest.SessionId;
                if (!conversations.TryGetValue(sessionId, out conversation))
                {
                    _logger.LogInformation($"Creating new conversation with sessionId: {sessionId}");
                    conversation = new Conversation(_exceptionLogger, _tracer);
                    conversations.Add(sessionId, conversation);
                }
            }

            return conversation;
        }
    }
}
