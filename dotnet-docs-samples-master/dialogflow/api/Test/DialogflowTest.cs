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

using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Xunit;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace GoogleCloudSamples
{
    public class DialogflowTest
    {
        protected RetryRobot _retryRobot = new RetryRobot();
        public readonly string ProjectId = Environment.GetEnvironmentVariable("GOOGLE_PROJECT_ID");
        public readonly string SessionId = TestUtil.RandomName();

        public ConsoleOutput Output { get; set; }
        public string Stdout => Output.Stdout;
        public int ExitCode => Output.ExitCode;

        // Multiple tests depend on existing EntityTypes.
        //
        // This helper method creates an EntityType via `entity-types:create`
        // and returns the EntityType's ID.
        public string CreateEntityType(string displayName = null, string kindName = "Map")
        {
            if (string.IsNullOrEmpty(displayName))
                displayName = TestUtil.RandomName();
            Run("entity-types:create", displayName, kindName);
            var outputPattern = new Regex(
                $"Created EntityType: projects/{ProjectId}/agent/entityTypes/(?<entityTypeId>.*)"
            );
            return outputPattern.Match(Stdout).Groups["entityTypeId"].Value;
        }

        public readonly CommandLineRunner _dialogflow = new CommandLineRunner()
        {
            Main = DialogflowSamples.Main,
            Command = "Dialogflow"
        };

        // Dialogflow enforces no more than 60 requests per minute per project.
        // Many agents may be running the test at the same time, so limit
        // our requests to 10 per minute.
        static readonly ThrottleTokenPool s_throttleTokenPool =
            new ThrottleTokenPool(10, TimeSpan.FromSeconds(61));

        // Run command and return output.
        // Project ID argument is always set.
        // Session ID argument available as a parameter.
        // Sets helper properties to last console output.
        public ConsoleOutput Run(string command, params object[] args)
        {
            using (var thottleToken = s_throttleTokenPool.Acquire())
            {
                var arguments = args.Select((arg) => arg.ToString()).ToList();
                arguments.Insert(0, command);
                arguments.AddRange(new[] { "--projectId", ProjectId });

                Output = _dialogflow.Run(arguments.ToArray());

                return Output;
            }
        }

        public ConsoleOutput RunWithSessionId(string command, params object[] args)
        {
            var arguments = args.ToList();
            arguments.AddRange(new[] { "--sessionId", SessionId });
            return Run(command, arguments.ToArray());
        }
    }

    // TODO: Move this class into test helpers.
    /// <summary>
    /// Schedules throttling.
    /// </summary>
    class ThrottleTokenPool
    {
        private readonly TimeSpan _timeSpan;
        private readonly BlockingCollection<ThrottleToken> _pool =
            new BlockingCollection<ThrottleToken>();
        /// <summary>
        /// Creates a throttle token pool.
        /// </summary>
        /// <example>
        /// Throttle number of tokens that can be acquired to 20 per minute.
        /// new ThrottleTokenPool(20, TimeSpan.FromMinutes(1))
        /// </example>
        /// <param name="tokenCount">Number of tokens.  Controls number
        /// of simultaneous operations than can be executing.</param>
        /// <param name="timeSpan">Every given timeSpan, a new set
        /// of tokens becomes available.</param>
        public ThrottleTokenPool(int tokenCount, TimeSpan timeSpan)
        {
            for (int i = 0; i < tokenCount; ++i)
            {
                _pool.Add(new ThrottleToken(this));
            }

            _timeSpan = timeSpan;
        }

        /// <summary>
        /// Acquires a token.  Blocks until a token is available.
        /// </summary>
        /// <returns>The token.</returns>
        public IDisposable Acquire() => _pool.Take();

        internal void Release(ThrottleToken token)
        {
            Task.Run(async () =>
            {
                await Task.Delay(_timeSpan);
                _pool.Add(token);
            });
        }
    }

    class ThrottleToken : IDisposable
    {
        readonly ThrottleTokenPool _pool;

        public ThrottleToken(ThrottleTokenPool pool)
        {
            _pool = pool;
        }

        public void Dispose()
        {
            _pool.Release(this);
        }
    }
}
