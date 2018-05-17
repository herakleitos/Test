﻿/*
 * Copyright (c) 2017 Google Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not
 * use this file except in compliance with the License. You may obtain a copy of
 * the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the
 * License for the specific language governing permissions and limitations under
 * the License.
 */

using System;
using System.Threading;
using System.Threading.Tasks;
using Google.Cloud.Spanner.Data;
using CommandLine;
using System.Transactions;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using System.Collections.Generic;
using log4net;
using System.Linq;
using System.Collections.Specialized;

namespace GoogleCloudSamples.Spanner
{
    [Verb("createDatabase", HelpText = "Create a Cloud Spanner database in your project.")]
    class CreateDatabaseOptions
    {
        [Value(0, HelpText = "The project ID of the project to use when creating Cloud Spanner resources.", Required = true)]
        public string projectId { get; set; }
        [Value(1, HelpText = "The ID of the instance where the database will be created.", Required = true)]
        public string instanceId { get; set; }
        [Value(2, HelpText = "The ID of the database to create.", Required = true)]
        public string databaseId { get; set; }
    }

    [Verb("createSampleDatabase", HelpText = "Create a sample Cloud Spanner database along with sample tables in your project.")]
    class CreateSampleDatabaseOptions
    {
        [Value(0, HelpText = "The project ID of the project to use when creating Cloud Spanner resources.", Required = true)]
        public string projectId { get; set; }
        [Value(1, HelpText = "The ID of the instance where the sample database will be created.", Required = true)]
        public string instanceId { get; set; }
        [Value(2, HelpText = "The ID of the sample database to create.", Required = true)]
        public string databaseId { get; set; }
    }

    [Verb("createTable", HelpText = "Create a table in your project's Cloud Spanner database.")]
    class CreateTableOptions
    {
        [Value(0, HelpText = "The project ID of the project to use when creating Cloud Spanner resources.", Required = true)]
        public string projectId { get; set; }
        [Value(1, HelpText = "The ID of the instance where the table will be created.", Required = true)]
        public string instanceId { get; set; }
        [Value(2, HelpText = "The ID of the database where the table will be created.", Required = true)]
        public string databaseId { get; set; }
        [Value(3, HelpText = "The name of the table to create.", Required = true)]
        public string tableName { get; set; }
    }

    [Verb("dropSampleTables", HelpText = "Drops the tables created by createSampleDatabase.")]
    class DropSampleTablesOptions
    {
        [Value(0, HelpText = "The project ID of the project to use when creating Cloud Spanner resources.", Required = true)]
        public string projectId { get; set; }
        [Value(1, HelpText = "The ID of the instance where the tables will be dropped.", Required = true)]
        public string instanceId { get; set; }
        [Value(2, HelpText = "The ID of the database where the tables will be dropped.", Required = true)]
        public string databaseId { get; set; }
    }

    [Verb("insertSampleData", HelpText = "Insert sample data into sample Cloud Spanner database table.")]
    class InsertSampleDataOptions
    {
        [Value(0, HelpText = "The project ID of the project to use when managing Cloud Spanner resources.", Required = true)]
        public string projectId { get; set; }
        [Value(1, HelpText = "The ID of the instance where the sample data will be inserted.", Required = true)]
        public string instanceId { get; set; }
        [Value(2, HelpText = "The ID of the database where the sample data will be inserted.", Required = true)]
        public string databaseId { get; set; }
    }

    [Verb("deleteSampleData", HelpText = "Delete sample data from sample Cloud Spanner database table.")]
    class DeleteSampleDataOptions
    {
        [Value(0, HelpText = "The project ID of the project to use when managing Cloud Spanner resources.", Required = true)]
        public string projectId { get; set; }
        [Value(1, HelpText = "The ID of the instance where the sample data will be removed.", Required = true)]
        public string instanceId { get; set; }
        [Value(2, HelpText = "The ID of the database where the sample data will be removed.", Required = true)]
        public string databaseId { get; set; }
    }

    [Verb("querySampleData", HelpText = "Query sample data from sample Cloud Spanner database table.")]
    class QuerySampleDataOptions
    {
        [Value(0, HelpText = "The project ID of the project to use when managing Cloud Spanner resources.", Required = true)]
        public string projectId { get; set; }
        [Value(1, HelpText = "The ID of the instance where the sample data resides.", Required = true)]
        public string instanceId { get; set; }
        [Value(2, HelpText = "The ID of the database where the sample data resides.", Required = true)]
        public string databaseId { get; set; }
    }

    [Verb("addIndex", HelpText = "Add an index to the sample Cloud Spanner database table.")]
    class AddIndexOptions
    {
        [Value(0, HelpText = "The project ID of the project to use when managing Cloud Spanner resources.", Required = true)]
        public string projectId { get; set; }
        [Value(1, HelpText = "The ID of the instance where the sample data resides.", Required = true)]
        public string instanceId { get; set; }
        [Value(2, HelpText = "The ID of the database where the sample data resides.", Required = true)]
        public string databaseId { get; set; }
    }

    [Verb("addStoringIndex", HelpText = "Add a storing index to the sample Cloud Spanner database table.")]
    class AddStoringIndexOptions
    {
        [Value(0, HelpText = "The project ID of the project to use when managing Cloud Spanner resources.", Required = true)]
        public string projectId { get; set; }
        [Value(1, HelpText = "The ID of the instance where the sample data resides.", Required = true)]
        public string instanceId { get; set; }
        [Value(2, HelpText = "The ID of the database where the sample data resides.", Required = true)]
        public string databaseId { get; set; }
    }

    [Verb("queryDataWithIndex", HelpText = "Query the sample Cloud Spanner database table using an index.")]
    class QueryDataWithIndexOptions
    {
        [Value(0, HelpText = "The project ID of the project to use when managing Cloud Spanner resources.", Required = true)]
        public string projectId { get; set; }
        [Value(1, HelpText = "The ID of the instance where the sample data resides.", Required = true)]
        public string instanceId { get; set; }
        [Value(2, HelpText = "The ID of the database where the sample data resides.", Required = true)]
        public string databaseId { get; set; }
        [Value(3, HelpText = "The start of the title index.", Required = false)]
        public string startTitle { get; set; }
        [Value(4, HelpText = "The end of the title index.", Required = false)]
        public string endTitle { get; set; }
    }

    [Verb("queryDataWithStoringIndex", HelpText = "Query the sample Cloud Spanner database table using an storing index.")]
    class QueryDataWithStoringIndexOptions
    {
        [Value(0, HelpText = "The project ID of the project to use when managing Cloud Spanner resources.", Required = true)]
        public string projectId { get; set; }
        [Value(1, HelpText = "The ID of the instance where the sample data resides.", Required = true)]
        public string instanceId { get; set; }
        [Value(2, HelpText = "The ID of the database where the sample data resides.", Required = true)]
        public string databaseId { get; set; }
        [Value(3, HelpText = "The start of the title index.", Required = false)]
        public string startTitle { get; set; }
        [Value(4, HelpText = "The end of the title index.", Required = false)]
        public string endTitle { get; set; }
    }

    [Verb("addColumn", HelpText = "Add a column to the sample Cloud Spanner database table.")]
    class AddColumnOptions
    {
        [Value(0, HelpText = "The project ID of the project to use when managing Cloud Spanner resources.", Required = true)]
        public string projectId { get; set; }
        [Value(1, HelpText = "The ID of the instance where the sample database resides.", Required = true)]
        public string instanceId { get; set; }
        [Value(2, HelpText = "The ID of the database where the sample database resides.", Required = true)]
        public string databaseId { get; set; }
    }

    [Verb("writeDataToNewColumn", HelpText = "Write data to a newly added column in the sample Cloud Spanner database table.")]
    class WriteDataToNewColumnOptions
    {
        [Value(0, HelpText = "The project ID of the project to use when managing Cloud Spanner resources.", Required = true)]
        public string projectId { get; set; }
        [Value(1, HelpText = "The ID of the instance where the sample database resides.", Required = true)]
        public string instanceId { get; set; }
        [Value(2, HelpText = "The ID of the database where the sample database resides.", Required = true)]
        public string databaseId { get; set; }
    }

    [Verb("queryNewColumn", HelpText = "Query data from a newly added column in the sample Cloud Spanner database table.")]
    class QueryNewColumnOptions
    {
        [Value(0, HelpText = "The project ID of the project to use when managing Cloud Spanner resources.", Required = true)]
        public string projectId { get; set; }
        [Value(1, HelpText = "The ID of the instance where the sample database resides.", Required = true)]
        public string instanceId { get; set; }
        [Value(2, HelpText = "The ID of the database where the sample database resides.", Required = true)]
        public string databaseId { get; set; }
    }

    [Verb("queryDataWithTransaction", HelpText = "Query the sample Cloud Spanner database table using a transaction.")]
    class QueryDataWithTransactionOptions
    {
        [Value(0, HelpText = "The project ID of the project to use when managing Cloud Spanner resources.", Required = true)]
        public string projectId { get; set; }
        [Value(1, HelpText = "The ID of the instance where the sample database resides.", Required = true)]
        public string instanceId { get; set; }
        [Value(2, HelpText = "The ID of the database where the sample database resides.", Required = true)]
        public string databaseId { get; set; }

        [Value(3, HelpText = "The platform code to execute. This should be 'netcore' or 'net45' (the default).", Required = false)]
        public string platform { get; set; } = "net45";
    }

    [Verb("readWriteWithTransaction", HelpText = "Update data in the sample Cloud Spanner database table using a read-write transaction.")]
    class ReadWriteWithTransactionOptions
    {
        [Value(0, HelpText = "The project ID of the project to use when managing Cloud Spanner resources.", Required = true)]
        public string projectId { get; set; }
        [Value(1, HelpText = "The ID of the instance where the sample database resides.", Required = true)]
        public string instanceId { get; set; }
        [Value(2, HelpText = "The ID of the database where the sample database resides.", Required = true)]
        public string databaseId { get; set; }

        [Value(3, HelpText = "The platform code to execute. This should be 'netcore' or 'net45' (the default).", Required = false)]
        public string platform { get; set; } = "net45";
    }

    [Verb("readStaleData", HelpText = "Read data that is ten seconds old.")]
    class ReadStaleDataOptions
    {
        [Value(0, HelpText = "The project ID of the project to use when managing Cloud Spanner resources.", Required = true)]
        public string projectId { get; set; }
        [Value(1, HelpText = "The ID of the instance where the sample database resides.", Required = true)]
        public string instanceId { get; set; }
        [Value(2, HelpText = "The ID of the database where the sample database resides.", Required = true)]
        public string databaseId { get; set; }
    }

    [Verb("batchInsertRecords", HelpText = "Batch insert sample records into the database.")]
    class BatchInsertOptions
    {
        [Value(0, HelpText = "The project ID of the project to use when managing Cloud Spanner resources.", Required = true)]
        public string projectId { get; set; }
        [Value(1, HelpText = "The ID of the instance where the sample database resides.", Required = true)]
        public string instanceId { get; set; }
        [Value(2, HelpText = "The ID of the database where the sample database resides.", Required = true)]
        public string databaseId { get; set; }
    }

    [Verb("batchReadRecords", HelpText = "Batch read sample records from the database.")]
    class BatchReadOptions
    {
        [Value(0, HelpText = "The project ID of the project to use when managing Cloud Spanner resources.", Required = true)]
        public string projectId { get; set; }
        [Value(1, HelpText = "The ID of the instance where the sample database resides.", Required = true)]
        public string instanceId { get; set; }
        [Value(2, HelpText = "The ID of the database where the sample database resides.", Required = true)]
        public string databaseId { get; set; }
    }

    [Verb("addCommitTimestamp", HelpText = "Add a commit timestamp column to the sample Cloud Spanner database table.")]
    class AddCommitTimestampOptions
    {
        [Value(0, HelpText = "The project ID of the project to use when managing Cloud Spanner resources.", Required = true)]
        public string projectId { get; set; }
        [Value(1, HelpText = "The ID of the instance where the sample database resides.", Required = true)]
        public string instanceId { get; set; }
        [Value(2, HelpText = "The ID of the database where the sample database resides.", Required = true)]
        public string databaseId { get; set; }
    }

    [Verb("updateDataWithTimestamp", HelpText = "Update data with a newly added commit timestamp column in the sample Cloud Spanner database table.")]
    class UpdateDataWithTimestampOptions
    {
        [Value(0, HelpText = "The project ID of the project to use when managing Cloud Spanner resources.", Required = true)]
        public string projectId { get; set; }
        [Value(1, HelpText = "The ID of the instance where the sample database resides.", Required = true)]
        public string instanceId { get; set; }
        [Value(2, HelpText = "The ID of the database where the sample database resides.", Required = true)]
        public string databaseId { get; set; }
    }

    [Verb("queryDataWithTimestamp", HelpText = "Query data with a newly added commit timestamp column in the sample Cloud Spanner database table.")]
    class QueryDataWithTimestampOptions
    {
        [Value(0, HelpText = "The project ID of the project to use when managing Cloud Spanner resources.", Required = true)]
        public string projectId { get; set; }
        [Value(1, HelpText = "The ID of the instance where the sample database resides.", Required = true)]
        public string instanceId { get; set; }
        [Value(2, HelpText = "The ID of the database where the sample database resides.", Required = true)]
        public string databaseId { get; set; }
    }

    [Verb("createTableWithTimestamp", HelpText = "Create a new table with a commit timestamp column in the sample Cloud Spanner database table.")]
    class CreateTableWithTimestampOptions
    {
        [Value(0, HelpText = "The project ID of the project to use when managing Cloud Spanner resources.", Required = true)]
        public string projectId { get; set; }
        [Value(1, HelpText = "The ID of the instance where the sample database resides.", Required = true)]
        public string instanceId { get; set; }
        [Value(2, HelpText = "The ID of the database where the sample database resides.", Required = true)]
        public string databaseId { get; set; }
    }

    [Verb("writeDataWithTimestamp", HelpText = "Write data into table with a commit timestamp column in the sample Cloud Spanner database table.")]
    class WriteDataWithTimestampOptions
    {
        [Value(0, HelpText = "The project ID of the project to use when managing Cloud Spanner resources.", Required = true)]
        public string projectId { get; set; }
        [Value(1, HelpText = "The ID of the instance where the sample database resides.", Required = true)]
        public string instanceId { get; set; }
        [Value(2, HelpText = "The ID of the database where the sample database resides.", Required = true)]
        public string databaseId { get; set; }
    }

    //QueryDataWithTimestampAsync
    [Verb("queryNewTableWithTimestamp", HelpText = "Query data from table with a commit timestamp column in the sample Cloud Spanner database table.")]
    class QueryNewTableWithTimestampOptions
    {
        [Value(0, HelpText = "The project ID of the project to use when managing Cloud Spanner resources.", Required = true)]
        public string projectId { get; set; }
        [Value(1, HelpText = "The ID of the instance where the sample database resides.", Required = true)]
        public string instanceId { get; set; }
        [Value(2, HelpText = "The ID of the database where the sample database resides.", Required = true)]
        public string databaseId { get; set; }
    }

    [Verb("listDatabaseTables", HelpText = "List all the user-defined tables in the database.")]
    class ListDatabaseTablesOptions
    {
        [Value(0, HelpText = "The project ID of the project to use when managing Cloud Spanner resources.", Required = true)]
        public string projectId { get; set; }
        [Value(1, HelpText = "The ID of the instance where the sample database resides.", Required = true)]
        public string instanceId { get; set; }
        [Value(2, HelpText = "The ID of the database where the sample database resides.", Required = true)]
        public string databaseId { get; set; }
    }

    [Verb("deleteDatabase", HelpText = "Delete a Spanner database.")]
    class DeleteOptions
    {
        [Value(0, HelpText = "The project ID of the project to use when managing Cloud Spanner resources.", Required = true)]
        public string projectId { get; set; }
        [Value(1, HelpText = "The ID of the instance where the sample database resides.", Required = true)]
        public string instanceId { get; set; }
        [Value(2, HelpText = "The ID of the database to delete.", Required = true)]
        public string databaseId { get; set; }
    }


    public class Program
    {
        static readonly ILog s_logger = LogManager.GetLogger(typeof(Program));
        private static readonly string s_netCorePlatform = "netcore";

        enum ExitCode : int
        {
            Success = 0,
            InvalidParameter = 1,
        }

        // [START spanner_insert_data]
        public class Singer
        {
            public int singerId { get; set; }
            public string firstName { get; set; }
            public string lastName { get; set; }
        }

        public class Album
        {
            public int singerId { get; set; }
            public int albumId { get; set; }
            public string albumTitle { get; set; }
        }
        // [END spanner_insert_data]

        public class Performance
        {
            public int singerId { get; set; }
            public int venueId { get; set; }
            public DateTime eventDate { get; set; }
            public long revenue { get; set; }
        }

        public static async Task CreateSampleDatabaseAsync(
            string projectId, string instanceId, string databaseId)
        {
            // [START spanner_create_database]
            // Initialize request connection string for database creation.
            string connectionString =
                $"Data Source=projects/{projectId}/instances/{instanceId}";
            // Make the request.
            using (var connection = new SpannerConnection(connectionString))
            {
                string createStatement = $"CREATE DATABASE `{databaseId}`";
                var cmd = connection.CreateDdlCommand(createStatement);
                try
                {
                    await cmd.ExecuteNonQueryAsync();
                }
                catch (SpannerException e) when (e.ErrorCode == ErrorCode.AlreadyExists)
                {
                    // OK.
                }
            }
            // Update connection string with Database ID for table creation.
            connectionString = connectionString + $"/databases/{databaseId}";
            using (var connection = new SpannerConnection(connectionString))
            {
                // Define create table statement for table #1.
                string createTableStatement =
               @"CREATE TABLE Singers (
                     SingerId INT64 NOT NULL,
                     FirstName    STRING(1024),
                     LastName STRING(1024),
                     ComposerInfo   BYTES(MAX)
                 ) PRIMARY KEY (SingerId)";
                // Make the request.
                var cmd = connection.CreateDdlCommand(createTableStatement);
                await cmd.ExecuteNonQueryAsync();
                // Define create table statement for table #2.
                createTableStatement =
                @"CREATE TABLE Albums (
                     SingerId     INT64 NOT NULL,
                     AlbumId      INT64 NOT NULL,
                     AlbumTitle   STRING(MAX)
                 ) PRIMARY KEY (SingerId, AlbumId),
                 INTERLEAVE IN PARENT Singers ON DELETE CASCADE";
                // Make the request.
                cmd = connection.CreateDdlCommand(createTableStatement);
                await cmd.ExecuteNonQueryAsync();
            }
            // [END spanner_create_database]
        }

        public static async Task AddIndexAsync(
            string projectId, string instanceId, string databaseId)
        {
            // [START spanner_create_index]
            // Initialize request argument(s).
            string connectionString =
                $"Data Source=projects/{projectId}/instances/"
                + $"{instanceId}/databases/{databaseId}";
            string createStatement =
                "CREATE INDEX AlbumsByAlbumTitle ON Albums(AlbumTitle)";
            // Make the request.
            using (var connection = new SpannerConnection(connectionString))
            {
                var createCmd = connection.CreateDdlCommand(createStatement);
                await createCmd.ExecuteNonQueryAsync();
            }
            Console.WriteLine("Added the AlbumsByAlbumTitle index.");
            // [END spanner_create_index]
        }

        public static async Task AddStoringIndexAsync(
            string projectId, string instanceId, string databaseId)
        {
            // [START spanner_create_storing_index]
            // Initialize request argument(s).
            string connectionString =
                $"Data Source=projects/{projectId}/instances/"
                + $"{instanceId}/databases/{databaseId}";
            string createStatement =
                "CREATE INDEX AlbumsByAlbumTitle2 ON Albums(AlbumTitle) "
                + "STORING (MarketingBudget)";
            // Make the request.
            using (var connection = new SpannerConnection(connectionString))
            {
                var createCmd = connection.CreateDdlCommand(createStatement);
                await createCmd.ExecuteNonQueryAsync();
            }
            Console.WriteLine("Added the AlbumsByAlbumTitle2 index.");
            // [END spanner_create_storing_index]
        }

        public static async Task AddColumnAsync(
            string projectId, string instanceId, string databaseId)
        {
            // [START spanner_add_column]
            // Initialize request argument(s).
            string connectionString =
                $"Data Source=projects/{projectId}/instances/"
                + $"{instanceId}/databases/{databaseId}";
            string alterStatement =
                "ALTER TABLE Albums ADD COLUMN MarketingBudget INT64";
            // Make the request.
            using (var connection = new SpannerConnection(connectionString))
            {
                var updateCmd = connection.CreateDdlCommand(alterStatement);
                await updateCmd.ExecuteNonQueryAsync();
            }
            Console.WriteLine("Added the MarketingBudget column.");
            // [END spanner_add_column]
        }

        public static async Task QuerySampleDataAsync(
            string projectId, string instanceId, string databaseId)
        {
            // [START spanner_query_data]
            string connectionString =
            $"Data Source=projects/{projectId}/instances/"
            + $"{instanceId}/databases/{databaseId}";
            // Create connection to Cloud Spanner.
            using (var connection = new SpannerConnection(connectionString))
            {
                var cmd = connection.CreateSelectCommand(
                    "SELECT SingerId, AlbumId, AlbumTitle FROM Albums");
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        Console.WriteLine("SingerId : "
                            + reader.GetFieldValue<string>("SingerId")
                            + " AlbumId : "
                            + reader.GetFieldValue<string>("AlbumId")
                            + " AlbumTitle : "
                            + reader.GetFieldValue<string>("AlbumTitle"));
                    }
                }
            }
            // [END spanner_query_data]
        }

        public static async Task QueryDataWithIndexAsync(
            string projectId, string instanceId, string databaseId,
            string startTitle = "Aardvark", string endTitle = "Goo")
        {
            // [START spanner_query_data_with_index]
            // [START spanner_read_data_with_index]
            string connectionString =
            $"Data Source=projects/{projectId}/instances/{instanceId}"
            + $"/databases/{databaseId}";
            // Create connection to Cloud Spanner.
            using (var connection = new SpannerConnection(connectionString))
            {
                var cmd = connection.CreateSelectCommand(
                    "SELECT AlbumId, AlbumTitle, MarketingBudget FROM Albums@ "
                    + "{FORCE_INDEX=AlbumsByAlbumTitle} "
                    + $"WHERE AlbumTitle >= @startTitle "
                    + $"AND AlbumTitle < @endTitle",
                    new SpannerParameterCollection {
                        {"startTitle", SpannerDbType.String},
                        {"endTitle", SpannerDbType.String} });
                cmd.Parameters["startTitle"].Value = startTitle;
                cmd.Parameters["endTitle"].Value = endTitle;
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        Console.WriteLine("AlbumId : "
                        + reader.GetFieldValue<string>("AlbumId")
                        + " AlbumTitle : "
                        + reader.GetFieldValue<string>("AlbumTitle")
                        + " MarketingBudget : "
                        + reader.GetFieldValue<string>("MarketingBudget"));
                    }
                }
            }
            // [END spanner_read_data_with_index]
            // [END spanner_query_data_with_index]
        }

        public static async Task QueryDataWithStoringIndexAsync(
            string projectId, string instanceId, string databaseId,
            string startTitle = "Aardvark", string endTitle = "Goo")
        {
            // [START spanner_read_data_with_storing_index]
            string connectionString =
            $"Data Source=projects/{projectId}/instances/{instanceId}"
            + $"/databases/{databaseId}";
            // Create connection to Cloud Spanner.
            using (var connection = new SpannerConnection(connectionString))
            {
                var cmd = connection.CreateSelectCommand(
                    "SELECT AlbumId, AlbumTitle, MarketingBudget FROM Albums@ "
                    + "{FORCE_INDEX=AlbumsByAlbumTitle2} "
                    + $"WHERE AlbumTitle >= @startTitle "
                    + $"AND AlbumTitle < @endTitle",
                    new SpannerParameterCollection {
                        {"startTitle", SpannerDbType.String},
                        {"endTitle", SpannerDbType.String} });
                cmd.Parameters["startTitle"].Value = startTitle;
                cmd.Parameters["endTitle"].Value = endTitle;
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        Console.WriteLine("AlbumId : "
                        + reader.GetFieldValue<string>("AlbumId")
                        + " AlbumTitle : "
                        + reader.GetFieldValue<string>("AlbumTitle")
                        + " MarketingBudget : "
                        + reader.GetFieldValue<string>("MarketingBudget"));
                    }
                }
            }
            // [END spanner_read_data_with_storing_index]
        }

        public static async Task QueryDataWithTransactionCoreAsync(
            string projectId, string instanceId, string databaseId)
        {
            Console.WriteLine(".NetCore API sample.");

            // [START spanner_read_only_transaction_core]
            string connectionString =
                $"Data Source=projects/{projectId}/instances/{instanceId}"
                + $"/databases/{databaseId}";

            // Create connection to Cloud Spanner.
            using (var connection = new SpannerConnection(connectionString))
            {
                await connection.OpenAsync();

                // Open a new read only transaction.
                using (var transaction =
                    await connection.BeginReadOnlyTransactionAsync())
                {
                    var cmd = connection.CreateSelectCommand(
                        "SELECT SingerId, AlbumId, AlbumTitle FROM Albums");
                    cmd.Transaction = transaction;

                    // Read #1.
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Console.WriteLine("SingerId : "
                                + reader.GetFieldValue<string>("SingerId")
                                + " AlbumId : "
                                + reader.GetFieldValue<string>("AlbumId")
                                + " AlbumTitle : "
                                + reader.GetFieldValue<string>("AlbumTitle"));
                        }
                    }
                    // Read #2. Even if changes occur in-between the reads,
                    // the transaction ensures that Read #1 and Read #2
                    // return the same data.
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Console.WriteLine("SingerId : "
                                + reader.GetFieldValue<string>("SingerId")
                                + " AlbumId : "
                                + reader.GetFieldValue<string>("AlbumId")
                                + " AlbumTitle : "
                                + reader.GetFieldValue<string>("AlbumTitle"));
                        }
                    }
                }
            }
            Console.WriteLine("Transaction complete.");
            // [END spanner_read_only_transaction_core]
        }

        public static async Task<object> ReadStaleDataAsync(
            string projectId, string instanceId, string databaseId)
        {
            Console.WriteLine(".NetCore API sample.");

            // [START spanner_read_stale_data]
            string connectionString =
                $"Data Source=projects/{projectId}/instances/{instanceId}"
                + $"/databases/{databaseId}";

            // Create connection to Cloud Spanner.
            using (var connection = new SpannerConnection(connectionString))
            {
                await connection.OpenAsync();

                // Open a new read only transaction.
                var staleness = TimestampBound.OfExactStaleness(
                    TimeSpan.FromSeconds(15));
                using (var transaction =
                    await connection.BeginReadOnlyTransactionAsync(staleness))
                {
                    var cmd = connection.CreateSelectCommand(
                        "SELECT SingerId, AlbumId, AlbumTitle FROM Albums");
                    cmd.Transaction = transaction;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Console.WriteLine("SingerId : "
                                + reader.GetFieldValue<string>("SingerId")
                                + " AlbumId : "
                                + reader.GetFieldValue<string>("AlbumId")
                                + " AlbumTitle : "
                                + reader.GetFieldValue<string>("AlbumTitle"));
                        }
                    }
                }
            }
            // [END spanner_read_stale_data]
            return 0;
        }


        public static async Task QueryDataWithTransactionAsync(
            string projectId, string instanceId, string databaseId)
        {
            // [START spanner_read_only_transaction]
            string connectionString =
            $"Data Source=projects/{projectId}/instances/{instanceId}"
            + $"/databases/{databaseId}";
            // Gets a transaction object that captures the database state
            // at a specific point in time.
            using (TransactionScope scope = new TransactionScope(
                TransactionScopeAsyncFlowOption.Enabled))
            {
                // Create connection to Cloud Spanner.
                using (var connection = new SpannerConnection(connectionString))
                {
                    // Open the connection, making the implicitly created
                    // transaction read only when it connects to the outer
                    // transaction scope.
                    await connection.OpenAsReadOnlyAsync()
                        .ConfigureAwait(false);
                    var cmd = connection.CreateSelectCommand(
                        "SELECT SingerId, AlbumId, AlbumTitle FROM Albums");
                    // Read #1.
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Console.WriteLine("SingerId : "
                                + reader.GetFieldValue<string>("SingerId")
                                + " AlbumId : "
                                + reader.GetFieldValue<string>("AlbumId")
                                + " AlbumTitle : "
                                + reader.GetFieldValue<string>("AlbumTitle"));
                        }
                    }
                    // Read #2. Even if changes occur in-between the reads,
                    // the transaction ensures that Read #1 and Read #2
                    // return the same data.
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Console.WriteLine("SingerId : "
                                + reader.GetFieldValue<string>("SingerId")
                                + " AlbumId : "
                                + reader.GetFieldValue<string>("AlbumId")
                                + " AlbumTitle : "
                                + reader.GetFieldValue<string>("AlbumTitle"));
                        }
                    }
                }
                scope.Complete();
                Console.WriteLine("Transaction complete.");
            }
            // [END spanner_read_only_transaction]
        }

        public static async Task WriteDataToNewColumnAsync(
            string projectId, string instanceId, string databaseId)
        {
            // [START spanner_update_data]
            string connectionString =
            $"Data Source=projects/{projectId}/instances/{instanceId}"
            + $"/databases/{databaseId}";
            // Create connection to Cloud Spanner.
            using (var connection = new SpannerConnection(connectionString))
            {
                var cmd = connection.CreateUpdateCommand("Albums",
                    new SpannerParameterCollection {
                        {"SingerId", SpannerDbType.Int64},
                        {"AlbumId", SpannerDbType.Int64},
                        {"MarketingBudget", SpannerDbType.Int64},
                    });
                var cmdLookup =
                    connection.CreateSelectCommand("SELECT * FROM Albums");
                using (var reader = await cmdLookup.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        if (reader.GetFieldValue<int>("SingerId") == 1
                            && reader.GetFieldValue<int>("AlbumId") == 1)
                        {
                            cmd.Parameters["SingerId"].Value =
                                reader.GetFieldValue<int>("SingerId");
                            cmd.Parameters["AlbumId"].Value =
                                reader.GetFieldValue<int>("AlbumId");
                            cmd.Parameters["MarketingBudget"].Value = 100000;
                            await cmd.ExecuteNonQueryAsync();
                        }
                        if (reader.GetInt64(0) == 2 && reader.GetInt64(1) == 2)
                        {
                            cmd.Parameters["SingerId"].Value =
                                reader.GetFieldValue<int>("SingerId");
                            cmd.Parameters["AlbumId"].Value =
                                reader.GetFieldValue<int>("AlbumId");
                            cmd.Parameters["MarketingBudget"].Value = 500000;
                            await cmd.ExecuteNonQueryAsync();
                        }
                    }
                }
            }
            Console.WriteLine("Updated data.");
            // [END spanner_update_data]
        }

        public static async Task QueryNewColumnAsync(
            string projectId, string instanceId, string databaseId)
        {
            // [START spanner_query_data_with_new_column]
            string connectionString =
            $"Data Source=projects/{projectId}/instances/{instanceId}"
            + $"/databases/{databaseId}";
            // Create connection to Cloud Spanner.
            using (var connection = new SpannerConnection(connectionString))
            {
                var cmd =
                    connection.CreateSelectCommand("SELECT * FROM Albums");
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        string budget = string.Empty;
                        if (reader["MarketingBudget"] != DBNull.Value)
                        {
                            budget = reader.GetFieldValue<string>("MarketingBudget");
                        }
                        Console.WriteLine("SingerId : "
                        + reader.GetFieldValue<string>("SingerId")
                        + " AlbumId : "
                        + reader.GetFieldValue<string>("AlbumId")
                        + $" MarketingBudget : {budget}");
                    }
                }
            }
            // [END spanner_query_data_with_new_column]
        }

        public static async Task ListDatabaseTablesAsync(
            string projectId, string instanceId, string databaseId)
        {
            // [START spanner_list_database_tables]
            string connectionString =
            $"Data Source=projects/{projectId}/instances/{instanceId}"
            + $"/databases/{databaseId}";
            // Create connection to Cloud Spanner.
            using (var connection = new SpannerConnection(connectionString))
            {
                var cmd =
                    connection.CreateSelectCommand(
                    "SELECT t.table_name FROM information_schema.tables AS t "
                    + "WHERE t.table_catalog = '' and t.table_schema = ''");
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        Console.WriteLine(
                            reader.GetFieldValue<string>("table_name"));
                    }
                }
            }
            // [END spanner_list_database_tables]
        }

        // [START spanner_topaz_strategy]
        internal class CustomTransientErrorDetectionStrategy
            : ITransientErrorDetectionStrategy
        {
            public bool IsTransient(Exception ex) =>
                ex.IsTransientSpannerFault();
        }
        // [END spanner_topaz_strategy]

        // [START spanner_read_write_transaction_core]
        public static async Task ReadWriteWithTransactionCoreAsync(
            string projectId,
            string instanceId,
            string databaseId)
        {
            // This sample transfers 200,000 from the MarketingBudget
            // field of the second Album to the first Album. Make sure to run
            // the addColumn and writeDataToNewColumn samples first,
            // in that order.
            string connectionString =
                $"Data Source=projects/{projectId}/instances/{instanceId}"
                + $"/databases/{databaseId}";

            decimal transferAmount = 200000;
            decimal minimumAmountToTransfer = 300000;
            decimal secondBudget = 0;
            decimal firstBudget = 0;

            Console.WriteLine(".NetCore API sample.");

            // Create connection to Cloud Spanner.
            using (var connection =
                new SpannerConnection(connectionString))
            {
                await connection.OpenAsync();

                // Create a readwrite transaction that we'll assign
                // to each SpannerCommand.
                using (var transaction =
                        await connection.BeginTransactionAsync())
                {
                    // Create statement to select the second album's data.
                    var cmdLookup = connection.CreateSelectCommand(
                     "SELECT * FROM Albums WHERE SingerId = 2 AND AlbumId = 2");
                    cmdLookup.Transaction = transaction;
                    // Excecute the select query.
                    using (var reader = await cmdLookup.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            // Read the second album's budget.
                            secondBudget =
                               reader.GetFieldValue<decimal>("MarketingBudget");
                            // Confirm second Album's budget is sufficient and
                            // if not raise an exception. Raising an exception
                            // will automatically roll back the transaction.
                            if (secondBudget < minimumAmountToTransfer)
                            {
                                throw new Exception("The second album's "
                                        + $"budget {secondBudget} "
                                        + "is less than the minimum required "
                                        + "amount to transfer.");
                            }
                        }
                    }
                    // Read the first album's budget.
                    cmdLookup = connection.CreateSelectCommand(
                     "SELECT * FROM Albums WHERE SingerId = 1 and AlbumId = 1");
                    cmdLookup.Transaction = transaction;
                    using (var reader = await cmdLookup.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            firstBudget =
                              reader.GetFieldValue<decimal>("MarketingBudget");
                        }
                    }

                    // Specify update command parameters.
                    var cmd = connection.CreateUpdateCommand("Albums",
                        new SpannerParameterCollection
                        {
                            {"SingerId", SpannerDbType.Int64},
                            {"AlbumId", SpannerDbType.Int64},
                            {"MarketingBudget", SpannerDbType.Int64},
                        });
                    cmd.Transaction = transaction;
                    // Update second album to remove the transfer amount.
                    secondBudget -= transferAmount;
                    cmd.Parameters["SingerId"].Value = 2;
                    cmd.Parameters["AlbumId"].Value = 2;
                    cmd.Parameters["MarketingBudget"].Value = secondBudget;
                    await cmd.ExecuteNonQueryAsync();
                    // Update first album to add the transfer amount.
                    firstBudget += transferAmount;
                    cmd.Parameters["SingerId"].Value = 1;
                    cmd.Parameters["AlbumId"].Value = 1;
                    cmd.Parameters["MarketingBudget"].Value = firstBudget;
                    await cmd.ExecuteNonQueryAsync();

                    await transaction.CommitAsync();
                }
                Console.WriteLine("Transaction complete.");
            }
        }
        // [END spanner_read_write_transaction_core]

        // [START spanner_read_write_transaction]
        public static async Task ReadWriteWithTransactionAsync(
            string projectId,
            string instanceId,
            string databaseId)
        {
            // This sample transfers 200,000 from the MarketingBudget
            // field of the second Album to the first Album. Make sure to run
            // the addColumn and writeDataToNewColumn samples first,
            // in that order.

            string connectionString =
            $"Data Source=projects/{projectId}/instances/{instanceId}"
            + $"/databases/{databaseId}";

            using (TransactionScope scope = new TransactionScope(
                TransactionScopeAsyncFlowOption.Enabled))
            {
                decimal transferAmount = 200000;
                decimal minimumAmountToTransfer = 300000;
                decimal secondBudget = 0;
                decimal firstBudget = 0;

                // Create connection to Cloud Spanner.
                using (var connection =
                    new SpannerConnection(connectionString))
                {
                    // Create statement to select the second album's data.
                    var cmdLookup = connection.CreateSelectCommand(
                    "SELECT * FROM Albums WHERE SingerId = 2 AND AlbumId = 2");
                    // Excecute the select query.
                    using (var reader = await cmdLookup.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            // Read the second album's budget.
                            secondBudget =
                              reader.GetFieldValue<decimal>("MarketingBudget");
                            // Confirm second Album's budget is sufficient and
                            // if not raise an exception. Raising an exception
                            // will automatically roll back the transaction.
                            if (secondBudget < minimumAmountToTransfer)
                            {
                                throw new Exception("The second album's "
                                    + $"budget {secondBudget} "
                                    + "is less than the minimum required "
                                    + "amount to transfer.");
                            }
                        }
                    }
                    // Read the first album's budget.
                    cmdLookup = connection.CreateSelectCommand(
                    "SELECT * FROM Albums WHERE SingerId = 1 and AlbumId = 1");
                    using (var reader = await cmdLookup.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            firstBudget =
                              reader.GetFieldValue<decimal>("MarketingBudget");
                        }
                    }

                    // Specify update command parameters.
                    var cmd = connection.CreateUpdateCommand("Albums",
                        new SpannerParameterCollection {
                        {"SingerId", SpannerDbType.Int64},
                        {"AlbumId", SpannerDbType.Int64},
                        {"MarketingBudget", SpannerDbType.Int64},
                    });
                    // Update second album to remove the transfer amount.
                    secondBudget -= transferAmount;
                    cmd.Parameters["SingerId"].Value = 2;
                    cmd.Parameters["AlbumId"].Value = 2;
                    cmd.Parameters["MarketingBudget"].Value = secondBudget;
                    await cmd.ExecuteNonQueryAsync();
                    // Update first album to add the transfer amount.
                    firstBudget += transferAmount;
                    cmd.Parameters["SingerId"].Value = 1;
                    cmd.Parameters["AlbumId"].Value = 1;
                    cmd.Parameters["MarketingBudget"].Value = firstBudget;
                    await cmd.ExecuteNonQueryAsync();
                    scope.Complete();
                    Console.WriteLine("Transaction complete.");
                }
            }
        }
        // [END spanner_read_write_transaction]

        public static async Task DeleteSampleDataAsync(
            string projectId, string instanceId, string databaseId)
        {
            const int firstSingerId = 1;
            const int secondSingerId = 2;
            string connectionString =
                $"Data Source=projects/{projectId}/instances/{instanceId}"
                + $"/databases/{databaseId}";
            List<Singer> singers = new List<Singer> {
                new Singer {singerId = firstSingerId, firstName = "Marc",
                    lastName = "Richards"},
                new Singer {singerId = secondSingerId, firstName = "Catalina",
                    lastName = "Smith"},
                new Singer {singerId = 3, firstName = "Alice",
                    lastName = "Trentor"},
                new Singer {singerId = 4, firstName = "Lea",
                    lastName = "Martin"},
                new Singer {singerId = 5, firstName = "David",
                    lastName = "Lomond"},
            };
            // Create connection to Cloud Spanner.
            using (var connection = new SpannerConnection(connectionString))
            {
                await connection.OpenAsync();

                // Insert rows into the Singers table.
                var cmd = connection.CreateDeleteCommand("Singers",
                    new SpannerParameterCollection {
                        {"SingerId", SpannerDbType.Int64}
                    });
                await Task.WhenAll(singers.Select(singer =>
                {
                    cmd.Parameters["SingerId"].Value = singer.singerId;
                    return cmd.ExecuteNonQueryAsync();
                }));

                Console.WriteLine("Deleted data.");
            }
        }
        // [START spanner_insert_data]
        public static async Task InsertSampleDataAsync(
            string projectId, string instanceId, string databaseId)
        {
            const int firstSingerId = 1;
            const int secondSingerId = 2;
            string connectionString =
            $"Data Source=projects/{projectId}/instances/{instanceId}"
            + $"/databases/{databaseId}";
            List<Singer> singers = new List<Singer> {
                new Singer {singerId = firstSingerId, firstName = "Marc",
                    lastName = "Richards"},
                new Singer {singerId = secondSingerId, firstName = "Catalina",
                    lastName = "Smith"},
                new Singer {singerId = 3, firstName = "Alice",
                    lastName = "Trentor"},
                new Singer {singerId = 4, firstName = "Lea",
                    lastName = "Martin"},
                new Singer {singerId = 5, firstName = "David",
                    lastName = "Lomond"},
            };
            List<Album> albums = new List<Album> {
                new Album {singerId = firstSingerId, albumId = 1,
                    albumTitle = "Go, Go, Go"},
                new Album {singerId = firstSingerId, albumId = 2,
                    albumTitle = "Total Junk"},
                new Album {singerId = secondSingerId, albumId = 1,
                    albumTitle = "Green"},
                new Album {singerId = secondSingerId, albumId = 2,
                    albumTitle = "Forever Hold your Peace"},
                new Album {singerId = secondSingerId, albumId = 3,
                    albumTitle = "Terrified"},
            };
            // Create connection to Cloud Spanner.
            using (var connection = new SpannerConnection(connectionString))
            {
                await connection.OpenAsync();

                // Insert rows into the Singers table.
                var cmd = connection.CreateInsertCommand("Singers",
                    new SpannerParameterCollection {
                        {"SingerId", SpannerDbType.Int64},
                        {"FirstName", SpannerDbType.String},
                        {"LastName", SpannerDbType.String}
                });
                await Task.WhenAll(singers.Select(singer =>
                {
                    cmd.Parameters["SingerId"].Value = singer.singerId;
                    cmd.Parameters["FirstName"].Value = singer.firstName;
                    cmd.Parameters["LastName"].Value = singer.lastName;
                    return cmd.ExecuteNonQueryAsync();
                }));

                // Insert rows into the Albums table.
                cmd = connection.CreateInsertCommand("Albums",
                    new SpannerParameterCollection {
                        {"SingerId", SpannerDbType.Int64},
                        {"AlbumId", SpannerDbType.Int64},
                        {"AlbumTitle", SpannerDbType.String}
                });
                await Task.WhenAll(albums.Select(album =>
                {
                    cmd.Parameters["SingerId"].Value = album.singerId;
                    cmd.Parameters["AlbumId"].Value = album.albumId;
                    cmd.Parameters["AlbumTitle"].Value = album.albumTitle;
                    return cmd.ExecuteNonQueryAsync();
                }));
                Console.WriteLine("Inserted data.");
            }
        }
        // [END spanner_insert_data]

        public static async Task CreateDatabaseAsync(
            string projectId, string instanceId, string databaseId)
        {
            // [START spanner_create_custom_database]
            // Initialize request connection string for database creation.
            string connectionString =
                $"Data Source=projects/{projectId}/instances/{instanceId}";
            // Make the request.
            using (var connection = new SpannerConnection(connectionString))
            {
                string createStatement = $"CREATE DATABASE `{databaseId}`";
                var cmd = connection.CreateDdlCommand(createStatement);
                await cmd.ExecuteNonQueryAsync();
            }
            // [END spanner_create_custom_database]
        }

        public static async Task<object> DropSampleTables(
            string projectId, string instanceId, string databaseId)
        {
            string connectionString =
                $"Data Source=projects/{projectId}/instances/{instanceId}"
                + $"/databases/{databaseId}";
            // Make the request.
            using (var connection = new SpannerConnection(connectionString))
            {
                foreach (string table in new[] { "Albums", "Singers" })
                {
                    try
                    {
                        await connection.CreateDdlCommand($"DROP TABLE {table}")
                            .ExecuteNonQueryAsync();
                    }
                    catch (SpannerException e) when (e.ErrorCode == ErrorCode.NotFound)
                    {
                        // Table does not exist.  Not a problem.
                    }
                }
                return 0;
            }
        }

        public static object CreateSampleDatabase(string projectId,
            string instanceId, string databaseId)
        {
            var response =
                CreateSampleDatabaseAsync(projectId, instanceId, databaseId);
            s_logger.Info("Waiting for operation to complete...");
            response.Wait();
            s_logger.Info($"Operation status: {response.Status}");
            Console.WriteLine($"Created sample database {databaseId} on "
                + $"instance {instanceId}");
            return ExitCode.Success;
        }

        public static object InsertSampleData(string projectId,
            string instanceId, string databaseId)
        {
            var response = InsertSampleDataAsync(
                projectId, instanceId, databaseId);
            s_logger.Info("Waiting for operation to complete...");
            response.Wait();
            s_logger.Info($"Operation status: {response.Status}");
            return ExitCode.Success;
        }

        public static object DeleteSampleData(string projectId,
            string instanceId, string databaseId)
        {
            var response = DeleteSampleDataAsync(
                projectId, instanceId, databaseId);
            s_logger.Info("Waiting for operation to complete...");
            response.Wait();
            s_logger.Info($"Operation status: {response.Status}");
            return ExitCode.Success;
        }

        public static object QuerySampleData(string projectId,
            string instanceId, string databaseId)
        {
            var response = QuerySampleDataAsync(
                projectId, instanceId, databaseId);
            s_logger.Info("Waiting for operation to complete...");
            response.Wait();
            s_logger.Info($"Operation status: {response.Status}");
            return ExitCode.Success;
        }

        public static object QueryDataWithIndex(string projectId,
            string instanceId, string databaseId,
            string startTitle, string endTitle)
        {
            if (string.IsNullOrEmpty(startTitle) ^ string.IsNullOrWhiteSpace(endTitle))
            {
                // Only one Title provided: show message that user must
                // provide both startTitle and endTitle or exclude them
                // from their command.
                Console.WriteLine("Please provide values for both the start "
                    + "of the title index and the end of the title index.");
                Console.WriteLine("Or you can exclude both to run the "
                    + "query with default values.");
                return ExitCode.InvalidParameter;
            }
            // Call method QueryDataWithIndexAsync() based on whether
            // user provided startTitle and endTitle values are empty or null.
            Task response = string.IsNullOrEmpty(startTitle) ?
                // startTitle not provided, exclude startTitle and endTitle from
                // method call to use default values for both.
                QueryDataWithIndexAsync(projectId, instanceId, databaseId) :
                // Both startTitle and endTitle provided, include them both in
                // the method call to QueryDataWithIndexAsync.
                QueryDataWithIndexAsync(projectId, instanceId, databaseId,
                        startTitle, endTitle);
            s_logger.Info("Waiting for operation to complete...");
            response.Wait();
            s_logger.Info($"Operation status: {response.Status}");
            return ExitCode.Success;
        }

        public static object CreateDatabase(string projectId,
            string instanceId, string databaseId)
        {
            var response = CreateDatabaseAsync(
                projectId, instanceId, databaseId);
            s_logger.Info("Waiting for operation to complete...");
            response.Wait();
            s_logger.Info($"Response status: {response.Status}");
            Console.WriteLine($"Created database {databaseId} on instance "
                + $"{instanceId}");
            return ExitCode.Success;
        }

        public static object AddIndex(string projectId,
            string instanceId, string databaseId)
        {
            var response = AddIndexAsync(
                projectId, instanceId, databaseId);
            s_logger.Info("Waiting for operation to complete...");
            response.Wait();
            s_logger.Info($"Response status: {response.Status}");
            return ExitCode.Success;
        }
        public static object AddStoringIndex(string projectId,
            string instanceId, string databaseId)
        {
            var response = AddStoringIndexAsync(
                projectId, instanceId, databaseId);
            s_logger.Info("Waiting for operation to complete...");
            response.Wait();
            s_logger.Info($"Response status: {response.Status}");
            return ExitCode.Success;
        }

        public static object QueryDataWithStoringIndex(
            string projectId, string instanceId,
            string databaseId, string startTitle, string endTitle)
        {
            var response = new Task(() => { });
            // Call method QueryDataWithStoringIndexAsync() based on whether
            // user provided startTitle and endTitle values are empty or null.
            if (string.IsNullOrEmpty(startTitle))
            {
                // startTitle not provided, exclude startTitle and endTitle from
                // method call to use default values for both.
                response = QueryDataWithStoringIndexAsync(
                        projectId, instanceId, databaseId);
            }
            else if (!string.IsNullOrEmpty(startTitle)
                && string.IsNullOrEmpty(endTitle))
            {
                // Only startTitle provided, show message that user must
                // provide both startTitle and endTitle or exclude them
                // from their command.
                Console.WriteLine("Please provide values for both the start "
                    + "of the title index and the end of the title index.");
                Console.WriteLine("Or you can exclude both to run the "
                    + "query with default values.");
                return ExitCode.InvalidParameter;
            }
            else if (!string.IsNullOrEmpty(startTitle)
                && !string.IsNullOrEmpty(endTitle))
            {
                // Both startTitle and endTitle provided, include them both in
                // the method call to QueryDataWithIndexAsync.
                response = QueryDataWithStoringIndexAsync(
                        projectId, instanceId, databaseId,
                        startTitle, endTitle);
            }
            s_logger.Info("Waiting for operation to complete...");
            response.Wait();
            s_logger.Info($"Operation status: {response.Status}");
            return ExitCode.Success;
        }

        public static object AddColumn(string projectId,
            string instanceId, string databaseId)
        {
            var response = AddColumnAsync(
                projectId, instanceId, databaseId);
            s_logger.Info("Waiting for operation to complete...");
            response.Wait();
            s_logger.Info($"Response status: {response.Status}");
            return ExitCode.Success;
        }

        public static object WriteDataToNewColumn(string projectId,
            string instanceId, string databaseId)
        {
            var response = WriteDataToNewColumnAsync(
                projectId, instanceId, databaseId);
            s_logger.Info("Waiting for operation to complete...");
            response.Wait();
            s_logger.Info($"Response status: {response.Status}");
            return ExitCode.Success;
        }

        public static object QueryNewColumn(string projectId,
            string instanceId, string databaseId)
        {
            var response = QueryNewColumnAsync(projectId,
                                instanceId, databaseId);
            s_logger.Info("Waiting for operation to complete...");
            response.Wait();
            s_logger.Info($"Response status: {response.Status}");
            return ExitCode.Success;
        }

        public static object ListDatabaseTables(string projectId,
            string instanceId, string databaseId)
        {
            var response = ListDatabaseTablesAsync(projectId,
                    instanceId, databaseId);
            s_logger.Info("Waiting for operation to complete...");
            response.Wait();
            s_logger.Info($"Response status: {response.Status}");
            return ExitCode.Success;
        }

        public static object QueryDataWithTransaction(
            string projectId, string instanceId,
            string databaseId, string platform)
        {
            var retryPolicy =
                new RetryPolicy<CustomTransientErrorDetectionStrategy>
                    (RetryStrategy.DefaultExponential);

            var response = platform == s_netCorePlatform
                ? retryPolicy.ExecuteAsync(() =>
                    QueryDataWithTransactionCoreAsync(projectId,
                        instanceId, databaseId))
                : retryPolicy.ExecuteAsync(() =>
                    QueryDataWithTransactionAsync(projectId,
                        instanceId, databaseId));

            s_logger.Info("Waiting for operation to complete...");
            response.Wait();
            s_logger.Info($"Operation status: {response.Status}");
            return ExitCode.Success;
        }

        public static object ReadWriteWithTransaction(
            string projectId, string instanceId,
            string databaseId, string platform)
        {
            s_logger.Info("Waiting for operation to complete...");
            var response = platform == s_netCorePlatform
                ? Task.Run(async () =>
                {
                    var retryPolicy = new
                        RetryPolicy<CustomTransientErrorDetectionStrategy>
                        (RetryStrategy.DefaultExponential);

                    await retryPolicy.ExecuteAsync(
                        () => ReadWriteWithTransactionCoreAsync(
                            projectId, instanceId, databaseId));
                })
                : Task.Run(async () =>
                {
                    // [START spanner_read_write_retry]
                    var retryPolicy = new
                        RetryPolicy<CustomTransientErrorDetectionStrategy>
                            (RetryStrategy.DefaultExponential);

                    await retryPolicy.ExecuteAsync(() =>
                        ReadWriteWithTransactionAsync(
                                projectId, instanceId, databaseId));
                    // [END spanner_read_write_retry]
                });
            response.Wait();
            s_logger.Info($"Response status: {response.Status}");
            return ExitCode.Success;
        }

        public static object BatchInsertRecords(string projectId,
            string instanceId, string databaseId)
        {
            var responseTask =
                BatchInsertRecordsAsync(projectId, instanceId, databaseId);
            Console.WriteLine("Waiting for operation to complete...");
            responseTask.Wait();
            Console.WriteLine($"Operation status: {responseTask.Status}");
            Console.WriteLine($"Inserted records into sample database "
                + $"{databaseId} on instance {instanceId}");
            return ExitCode.Success;
        }

        private static async Task BatchInsertRecordsAsync(string projectId,
            string instanceId, string databaseId)
        {
            string connectionString =
                $"Data Source=projects/{projectId}/instances/{instanceId}"
                + $"/databases/{databaseId}";

            // Get current max SingerId.
            Int64 maxSingerId = 0;
            using (var connection = new SpannerConnection(connectionString))
            {
                // Execute a SQL statement to get current MAX() of SingerId.
                var cmd = connection.CreateSelectCommand(
                    @"SELECT MAX(SingerId) as SingerId FROM Singers");
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        long parsedValue;
                        bool result = Int64.TryParse(
                            reader.GetFieldValue<string>("SingerId"), out parsedValue);
                        if (result)
                        {
                            maxSingerId = parsedValue;
                        }
                    }
                }
            }

            // Batch insert 249,900 singer records into the Singers table.
            using (var connection = new SpannerConnection(connectionString))
            {
                await connection.OpenAsync();

                for (int i = 0; i < 100; i++)
                {
                    // For details on transaction isolation, see the "Isolation" section in:
                    // https://cloud.google.com/spanner/docs/transactions#read-write_transactions
                    using (var tx = await connection.BeginTransactionAsync())
                    using (var cmd = connection.CreateInsertCommand("Singers", new SpannerParameterCollection
                    {
                        { "SingerId", SpannerDbType.String },
                        { "FirstName", SpannerDbType.String },
                        { "LastName", SpannerDbType.String }
                    }))
                    {
                        cmd.Transaction = tx;
                        for (var x = 1; x < 2500; x++)
                        {
                            maxSingerId++;
                            string nameSuffix = Guid.NewGuid().ToString().Substring(0, 8);
                            cmd.Parameters["SingerId"].Value = maxSingerId;
                            cmd.Parameters["FirstName"].Value = $"FirstName-{nameSuffix}";
                            cmd.Parameters["LastName"].Value = $"LastName-{nameSuffix}";
                            cmd.ExecuteNonQuery();
                        }
                        await tx.CommitAsync();
                    }
                }
            }
            Console.WriteLine("Done inserting sample records...");
        }

        // [START spanner_batch_client]
        private static int s_partitionId;
        private static int s_rowsRead;
        public static object BatchReadRecords(string projectId,
             string instanceId, string databaseId)
        {
            var responseTask =
                DistributedReadAsync(projectId, instanceId, databaseId);
            Console.WriteLine("Waiting for operation to complete...");
            responseTask.Wait();
            Console.WriteLine($"Operation status: {responseTask.Status}");
            return ExitCode.Success;
        }

        private static async Task DistributedReadAsync(string projectId,
            string instanceId, string databaseId)
        {
            string connectionString =
                $"Data Source=projects/{projectId}/instances/{instanceId}"
                + $"/databases/{databaseId}";
            using (var connection = new SpannerConnection(connectionString))
            {
                await connection.OpenAsync();

                using (var transaction =
                    await connection.BeginReadOnlyTransactionAsync())
                using (var cmd =
                    connection.CreateSelectCommand(
                        "SELECT SingerId, FirstName, LastName FROM Singers"))
                {
                    transaction.DisposeBehavior =
                        DisposeBehavior.CloseResources;
                    cmd.Transaction = transaction;
                    var partitions = await cmd.GetReaderPartitionsAsync();
                    var transactionId = transaction.TransactionId;
                    await Task.WhenAll(partitions.Select(
                            x => DistributedReadWorkerAsync(x, transactionId)))
                                .ConfigureAwait(false);
                }
                Console.WriteLine($"Done reading!  Total rows read: "
                    + $"{s_rowsRead:N0} with {s_partitionId} partition(s)");
            }
        }

        private static async Task DistributedReadWorkerAsync(
            CommandPartition readPartition, TransactionId id)
        {
            var localId = Interlocked.Increment(ref s_partitionId);
            using (var connection = new SpannerConnection(id.ConnectionString))
            using (var transaction = connection.BeginReadOnlyTransaction(id))
            {
                using (var cmd = connection.CreateCommandWithPartition(
                    readPartition, transaction))
                {
                    using (var reader =
                        await cmd.ExecuteReaderAsync().ConfigureAwait(false))
                    {
                        while (await reader.ReadAsync())
                        {
                            Interlocked.Increment(ref s_rowsRead);
                            Console.WriteLine($"Partition ({localId}) "
                                + $"{reader.GetFieldValue<string>("SingerId")}"
                                + $" {reader.GetFieldValue<string>("FirstName")}"
                                + $" {reader.GetFieldValue<string>("LastName")}");
                        }
                    }
                }
                Console.WriteLine($"Done with single reader {localId}.");
            }
        }
        // [END spanner_batch_client]

        public static object AddCommitTimestamp(string projectId,
              string instanceId, string databaseId)
        {
            var response = AddCommitTimestampAsync(
                projectId, instanceId, databaseId);
            s_logger.Info("Waiting for operation to complete...");
            response.Wait();
            s_logger.Info($"Response status: {response.Status}");
            return ExitCode.Success;
        }

        public static async Task AddCommitTimestampAsync(
            string projectId, string instanceId, string databaseId)
        {
            // [START spanner_add_timestamp_column]
            // Initialize request argument(s).
            string connectionString =
                $"Data Source=projects/{projectId}/instances/"
                + $"{instanceId}/databases/{databaseId}";
            string alterStatement =
                "ALTER TABLE Albums ADD COLUMN LastUpdateTime TIMESTAMP "
                + " OPTIONS (allow_commit_timestamp=true)";
            // Make the request.
            using (var connection = new SpannerConnection(connectionString))
            {
                var updateCmd = connection.CreateDdlCommand(alterStatement);
                await updateCmd.ExecuteNonQueryAsync();
            }
            Console.WriteLine("Added LastUpdateTime as a commit timestamp column in Albums table.");
            // [END spanner_add_timestamp_column]
        }

        public static object UpdateDataWithTimestampColumn(string projectId,
            string instanceId, string databaseId)
        {
            var response = UpdateDataWithTimestampColumnAsync(
                projectId, instanceId, databaseId);
            s_logger.Info("Waiting for operation to complete...");
            response.Wait();
            s_logger.Info($"Response status: {response.Status}");
            return ExitCode.Success;
        }

        public static async Task UpdateDataWithTimestampColumnAsync(
            string projectId, string instanceId, string databaseId)
        {
            // [START spanner_update_data_with_timestamp_column]
            string connectionString =
            $"Data Source=projects/{projectId}/instances/{instanceId}"
            + $"/databases/{databaseId}";
            // Create connection to Cloud Spanner.
            using (var connection = new SpannerConnection(connectionString))
            {
                var cmd = connection.CreateUpdateCommand("Albums",
                    new SpannerParameterCollection {
                        {"SingerId", SpannerDbType.Int64},
                        {"AlbumId", SpannerDbType.Int64},
                        {"MarketingBudget", SpannerDbType.Int64},
                        {"LastUpdateTime", SpannerDbType.Timestamp},
                    });
                var cmdLookup =
                    connection.CreateSelectCommand("SELECT * FROM Albums");
                using (var reader = await cmdLookup.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        if (reader.GetFieldValue<int>("SingerId") == 1
                            && reader.GetFieldValue<int>("AlbumId") == 1)
                        {
                            cmd.Parameters["SingerId"].Value =
                                reader.GetFieldValue<int>("SingerId");
                            cmd.Parameters["AlbumId"].Value =
                                reader.GetFieldValue<int>("AlbumId");
                            cmd.Parameters["MarketingBudget"].Value = 1000000;
                            cmd.Parameters["LastUpdateTime"].Value =
                                SpannerParameter.CommitTimestamp;
                            await cmd.ExecuteNonQueryAsync();
                        }
                        if (reader.GetInt64(0) == 2 && reader.GetInt64(1) == 2)
                        {
                            cmd.Parameters["SingerId"].Value =
                                reader.GetFieldValue<int>("SingerId");
                            cmd.Parameters["AlbumId"].Value =
                                reader.GetFieldValue<int>("AlbumId");
                            cmd.Parameters["MarketingBudget"].Value = 750000;
                            cmd.Parameters["LastUpdateTime"].Value =
                                SpannerParameter.CommitTimestamp;
                            await cmd.ExecuteNonQueryAsync();
                        }
                    }
                }
            }
            Console.WriteLine("Updated data.");
            // [END spanner_update_data_with_timestamp_column]
        }

        public static object QueryDataWithTimestampColumn(string projectId,
            string instanceId, string databaseId)
        {
            var response = QueryDataWithTimestampColumnAsync(
                projectId, instanceId, databaseId);
            s_logger.Info("Waiting for operation to complete...");
            response.Wait();
            s_logger.Info($"Response status: {response.Status}");
            return ExitCode.Success;
        }

        public static async Task QueryDataWithTimestampColumnAsync(
            string projectId, string instanceId, string databaseId)
        {
            // [START spanner_query_data_with_timestamp_column]
            string connectionString =
            $"Data Source=projects/{projectId}/instances/{instanceId}"
            + $"/databases/{databaseId}";
            // Create connection to Cloud Spanner.
            using (var connection = new SpannerConnection(connectionString))
            {
                var cmd =
                    connection.CreateSelectCommand(
                        "SELECT SingerId, AlbumId, "
                        + "MarketingBudget, LastUpdateTime FROM Albums");
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        string budget = string.Empty;
                        if (reader["MarketingBudget"] != DBNull.Value)
                        {
                            budget = reader.GetFieldValue<string>("MarketingBudget");
                        }
                        string timestamp = string.Empty;
                        if (reader["LastUpdateTime"] != DBNull.Value)
                        {
                            timestamp = reader.GetFieldValue<string>("LastUpdateTime");
                        }
                        Console.WriteLine("SingerId : "
                        + reader.GetFieldValue<string>("SingerId")
                        + " AlbumId : "
                        + reader.GetFieldValue<string>("AlbumId")
                        + $" MarketingBudget : {budget}"
                        + $" LastUpdateTime : {timestamp}");
                    }
                }
            }
            // [END spanner_query_data_with_timestamp_column]
        }

        public static object CreateTableWithTimestampColumn(string projectId,
            string instanceId, string databaseId)
        {
            var response = CreateTableWithTimestampColumnAsync(
                projectId, instanceId, databaseId);
            s_logger.Info("Waiting for operation to complete...");
            response.Wait();
            s_logger.Info($"Response status: {response.Status}");
            return ExitCode.Success;
        }

        public static async Task CreateTableWithTimestampColumnAsync(
            string projectId, string instanceId, string databaseId)
        {
            // [START spanner_create_table_with_timestamp_column]
            // Initialize request connection string for database creation.
            string connectionString =
                $"Data Source=projects/{projectId}/instances/{instanceId}"
                + $"/databases/{databaseId}";
            using (var connection = new SpannerConnection(connectionString))
            {
                // Define create table statement for table with 
                // commit timestamp column.
                string createTableStatement =
               @"CREATE TABLE Performances (
				SingerId	INT64 NOT NULL,
				VenueId		INT64 NOT NULL,
				EventDate	Date,
				Revenue   INT64,
				LastUpdateTime TIMESTAMP NOT NULL OPTIONS (allow_commit_timestamp=true)
			    ) PRIMARY KEY (SingerId, VenueId, EventDate),
			    INTERLEAVE IN PARENT Singers ON DELETE CASCADE";
                // Make the request.
                var cmd = connection.CreateDdlCommand(createTableStatement);
                await cmd.ExecuteNonQueryAsync();
            }
            // [END spanner_create_table_with_timestamp_column]
        }

        public static object WriteDataWithTimestampColumn(string projectId,
            string instanceId, string databaseId)
        {
            var response = WriteDataWithTimestampAsync(
                projectId, instanceId, databaseId);
            s_logger.Info("Waiting for operation to complete...");
            response.Wait();
            s_logger.Info($"Response status: {response.Status}");
            return ExitCode.Success;
        }

        public static async Task WriteDataWithTimestampAsync(
            string projectId, string instanceId, string databaseId)
        {
            // [START spanner_insert_data_with_timestamp_column]
            string connectionString =
            $"Data Source=projects/{projectId}/instances/{instanceId}"
            + $"/databases/{databaseId}";
            List<Performance> performances = new List<Performance> {
                new Performance {singerId = 1, venueId = 4, eventDate = DateTime.Parse("2017-10-05"),
                    revenue = 11000},
                new Performance {singerId = 1, venueId = 19, eventDate = DateTime.Parse("2017-11-02"),
                    revenue = 15000},
                new Performance {singerId = 2, venueId = 42, eventDate = DateTime.Parse("2017-12-23"),
                    revenue = 7000},
            };
            // Create connection to Cloud Spanner.
            using (var connection = new SpannerConnection(connectionString))
            {
                await connection.OpenAsync();
                // Insert rows into the Performances table.
                var cmd = connection.CreateInsertCommand("Performances",
                    new SpannerParameterCollection {
                        {"SingerId", SpannerDbType.Int64},
                        {"VenueId", SpannerDbType.Int64},
                        {"EventDate", SpannerDbType.Date},
                        {"Revenue", SpannerDbType.Int64},
                        {"LastUpdateTime", SpannerDbType.Timestamp},
                });
                await Task.WhenAll(performances.Select(performance =>
                {
                    cmd.Parameters["SingerId"].Value = performance.singerId;
                    cmd.Parameters["VenueId"].Value = performance.venueId;
                    cmd.Parameters["EventDate"].Value = performance.eventDate;
                    cmd.Parameters["Revenue"].Value = performance.revenue;
                    cmd.Parameters["LastUpdateTime"].Value = SpannerParameter.CommitTimestamp;
                    return cmd.ExecuteNonQueryAsync();
                }));
                Console.WriteLine("Inserted data.");
            }
            // [END spanner_insert_data_with_timestamp_column]
        }

        public static object QueryNewTableWithTimestampColumn(string projectId,
            string instanceId, string databaseId)
        {
            var response = QueryDataWithTimestampAsync(
                projectId, instanceId, databaseId);
            s_logger.Info("Waiting for operation to complete...");
            response.Wait();
            s_logger.Info($"Response status: {response.Status}");
            return ExitCode.Success;
        }

        public static async Task QueryDataWithTimestampAsync(
            string projectId, string instanceId, string databaseId)
        {
            string connectionString =
            $"Data Source=projects/{projectId}/instances/"
            + $"{instanceId}/databases/{databaseId}";
            // Create connection to Cloud Spanner.
            using (var connection = new SpannerConnection(connectionString))
            {
                var cmd = connection.CreateSelectCommand(
                    "SELECT SingerId, VenueId, EventDate, Revenue, "
                    + "LastUpdateTime FROM Performances "
                    + "ORDER BY LastUpdateTime DESC");
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        Console.WriteLine("SingerId : "
                            + reader.GetFieldValue<string>("SingerId")
                            + " VenueId : "
                            + reader.GetFieldValue<string>("VenueId")
                            + " EventDate : "
                            + reader.GetFieldValue<string>("EventDate")
                            + " Revenue : "
                            + reader.GetFieldValue<string>("Revenue")
                            + " LastUpdateTime : "
                            + reader.GetFieldValue<string>("LastUpdateTime")
                            );
                    }
                }
            }
        }


        public static object DeleteDatabase(string projectId,
            string instanceId, string databaseId)
        {
            var response =
                DeleteDatabaseAsync(projectId, instanceId, databaseId);
            Console.WriteLine("Waiting for operation to complete...");
            response.Wait();
            Console.WriteLine($"Operation status: {response.Status}");
            Console.WriteLine($"Deleted sample database {databaseId} on "
                + $"instance {instanceId}");
            return ExitCode.Success;
        }

        private static async Task DeleteDatabaseAsync(string projectId,
            string instanceId, string databaseId)
        {
            // The IAM Role "Cloud Spanner Database Admin" is required for 
            // performing Administration operations on the database.
            // For more info see https://cloud.google.com/spanner/docs/iam#roles
            string AdminConnectionString = $"Data Source=projects/{projectId}/"
                + $"instances/{instanceId}";
            using (var connection = new SpannerConnection(AdminConnectionString))
            using (var cmd = connection.CreateDdlCommand(
                $@"DROP DATABASE {databaseId}"))
            {
                await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
            }
            Console.WriteLine($"Done deleting database: {databaseId}");
        }

        public static int Main(string[] args)
        {
            var verbMap = new VerbMap<object>();
            verbMap
                .Add((CreateSampleDatabaseOptions opts) =>
                    CreateSampleDatabase(opts.projectId, opts.instanceId,
                        opts.databaseId))
                .Add((CreateDatabaseOptions opts) => CreateDatabase(
                    opts.projectId, opts.instanceId, opts.databaseId))
                .Add((DeleteSampleDataOptions opts) => DeleteSampleData(
                        opts.projectId, opts.instanceId, opts.databaseId))
                .Add((InsertSampleDataOptions opts) => InsertSampleData(
                    opts.projectId, opts.instanceId, opts.databaseId))
                .Add((QuerySampleDataOptions opts) => QuerySampleData(
                    opts.projectId, opts.instanceId, opts.databaseId))
                .Add((AddColumnOptions opts) => AddColumn(
                    opts.projectId, opts.instanceId, opts.databaseId))
                .Add((WriteDataToNewColumnOptions opts) => WriteDataToNewColumn(
                    opts.projectId, opts.instanceId, opts.databaseId))
                .Add((QueryNewColumnOptions opts) => QueryNewColumn(
                    opts.projectId, opts.instanceId, opts.databaseId))
                .Add((QueryDataWithTransactionOptions opts) =>
                    QueryDataWithTransaction(
                        opts.projectId, opts.instanceId, opts.databaseId,
                        opts.platform))
                .Add((ReadWriteWithTransactionOptions opts) =>
                    ReadWriteWithTransaction(
                        opts.projectId, opts.instanceId, opts.databaseId,
                        opts.platform))
                .Add((AddIndexOptions opts) => AddIndex(
                    opts.projectId, opts.instanceId, opts.databaseId))
                .Add((AddStoringIndexOptions opts) => AddStoringIndex(
                    opts.projectId, opts.instanceId, opts.databaseId))
                .Add((QueryDataWithIndexOptions opts) => QueryDataWithIndex(
                    opts.projectId, opts.instanceId, opts.databaseId,
                    opts.startTitle, opts.endTitle))
                .Add((QueryDataWithStoringIndexOptions opts) =>
                    QueryDataWithStoringIndex(
                        opts.projectId, opts.instanceId, opts.databaseId,
                        opts.startTitle, opts.endTitle))
                .Add((ReadStaleDataOptions opts) =>
                    ReadStaleDataAsync(opts.projectId, opts.instanceId,
                        opts.databaseId).Result)
                .Add((BatchInsertOptions opts) =>
                    BatchInsertRecords(opts.projectId, opts.instanceId,
                        opts.databaseId))
                .Add((BatchReadOptions opts) =>
                    BatchReadRecords(opts.projectId, opts.instanceId,
                        opts.databaseId))
                .Add((AddCommitTimestampOptions opts) =>
                    AddCommitTimestamp(opts.projectId, opts.instanceId,
                        opts.databaseId))
                .Add((UpdateDataWithTimestampOptions opts) =>
                    UpdateDataWithTimestampColumn(opts.projectId,
                        opts.instanceId, opts.databaseId))
                .Add((QueryDataWithTimestampOptions opts) =>
                    QueryDataWithTimestampColumn(opts.projectId,
                        opts.instanceId, opts.databaseId))
                .Add((CreateTableWithTimestampOptions opts) =>
                    CreateTableWithTimestampColumn(opts.projectId,
                        opts.instanceId, opts.databaseId))
                .Add((WriteDataWithTimestampOptions opts) =>
                    WriteDataWithTimestampColumn(opts.projectId,
                        opts.instanceId, opts.databaseId))
                .Add((QueryNewTableWithTimestampOptions opts) =>
                    QueryNewTableWithTimestampColumn(opts.projectId,
                        opts.instanceId, opts.databaseId))
                .Add((ListDatabaseTablesOptions opts) =>
                    ListDatabaseTables(opts.projectId, opts.instanceId,
                        opts.databaseId))
                .Add((DeleteOptions opts) =>
                    DeleteDatabase(opts.projectId, opts.instanceId,
                        opts.databaseId))
                .Add((DropSampleTablesOptions opts) =>
                    DropSampleTables(opts.projectId, opts.instanceId,
                    opts.databaseId).Result)
                .NotParsedFunc = (err) => 1;
            return (int)verbMap.Run(args);
        }

        /// <summary>
        /// Returns true if an AggregateException contains a SpannerException
        /// with the given error code.
        /// </summary>
        /// <param name="e">The exception to examine.</param>
        /// <param name="errorCode">The error code to look for.</param>
        /// <returns></returns>
        public static bool ContainsError(AggregateException e, params ErrorCode[] errorCode)
        {
            foreach (var innerException in e.InnerExceptions)
            {
                SpannerException spannerException = innerException as SpannerException;
                if (spannerException != null && errorCode.Contains(spannerException.ErrorCode))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Returns true if an AggregateException contains a Grpc.Core.RpcException
        /// with the given error code.
        /// </summary>
        /// <param name="e">The exception to examine.</param>
        /// <param name="errorCode">The error code to look for.</param>
        /// <returns></returns>
        public static bool ContainsGrpcError(AggregateException e,
            Grpc.Core.StatusCode errorCode)
        {
            foreach (var innerException in e.InnerExceptions)
            {
                Grpc.Core.RpcException grpcException = innerException
                    as Grpc.Core.RpcException;
                if (grpcException != null &&
                    grpcException.Status.StatusCode == errorCode)
                    return true;
            }
            return false;
        }
    }
}
