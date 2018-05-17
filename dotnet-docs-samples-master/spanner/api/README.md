# .NET Cloud Spanner Samples

A collection of samples that demonstrate how to call the
[Google Cloud Spanner API](https://cloud.google.com/spanner/docs/) from C#.

This sample requires [.NET Core 2.0](
    https://www.microsoft.com/net/core) or later.  That means using
[Visual Studio 2017](
    https://www.visualstudio.com/), or the command line.  Visual Studio 2015 users
can use [this older sample](
    https://github.com/GoogleCloudPlatform/dotnet-docs-samples/tree/vs2015/spanner/api).

## Build and Run

1.  **Follow the instructions in the [root README](../../README.md)**.

4.  Enable APIs for your project.
    [Click here](https://console.cloud.google.com/flows/enableapi?apiid=spanner.googleapis.com&showconfirmation=true)
    to visit Cloud Platform Console and enable the Google Cloud Spanner API.

7.  Edit `QuickStart\Program.cs`, and replace YOUR-PROJECT-ID with id
    of the project you created in step 1.

9.  From a Powershell command line, run the QuickStart sample:
    ```
    PS C:\...\dotnet-docs-samples\spanner\api\QuickStart> dotnet run
    Hello World
    ```

10. And run the Spanner sample to see a list of subcommands:
    ```
    PS C:\...\dotnet-docs-samples\spanner\api\Spanner> dotnet run
    Spanner 1.0.0.0
    Copyright c  2017

    ERROR(S):
      No verb selected.

      createSampleDatabase         Create a sample Cloud Spanner database along with sample tables in your project.

      createDatabase               Create a Cloud Spanner database in your project.

      deleteSampleData             Delete sample data from sample Cloud Spanner database table.

      insertSampleData             Insert sample data into sample Cloud Spanner database table.

      querySampleData              Query sample data from sample Cloud Spanner database table.

      addColumn                    Add a column to the sample Cloud Spanner database table.

      writeDataToNewColumn         Write data to a newly added column in the sample Cloud Spanner database table.

      queryNewColumn               Query data from a newly added column in the sample Cloud Spanner database table.

      queryDataWithTransaction     Query the sample Cloud Spanner database table using a transaction.

      readWriteWithTransaction     Update data in the sample Cloud Spanner database table using a read-write transaction.

      addIndex                     Add an index to the sample Cloud Spanner database table.

      addStoringIndex              Add a storing index to the sample Cloud Spanner database table.

      queryDataWithIndex           Query the sample Cloud Spanner database table using an index.

      queryDataWithStoringIndex    Query the sample Cloud Spanner database table using an storing index.

      readStaleData                Read data that is ten seconds old.

      batchInsertRecords           Batch insert sample records into the database.

      batchReadRecords             Batch read sample records from the database.

      addCommitTimestamp            Add a commit timestamp column to the sample Cloud Spanner database table.

      updateDataWithTimestamp       Update data with a newly added commit timestamp column in the sample Cloud Spanner database table.

      queryDataWithTimestamp        Query data with a newly added commit timestamp column in the sample Cloud Spanner database table.

      createTableWithTimestamp      Create a new table with a commit timestamp column in the sample Cloud Spanner database table.

      writeDataWithTimestamp        Write data into table with a commit timestamp column in the sample Cloud Spanner database table.

      queryNewTableWithTimestamp    Query data from table with a commit timestamp column in the sample Cloud Spanner database table.

      listDatabaseTables           List all the user-defined tables in the database.

      deleteDatabase               Delete a Spanner database.

      dropSampleTables             Drops the tables created by createSampleDatabase.

      help                         Display more information on a specific command.

      version                      Display version information.
    ```

    ```
    PS > dotnet run createSampleDatabase your-project-id my-instance my-database
    Waiting for operation to complete...
    Database create operation state: Ready
    Operation status: RanToCompletion
    Created sample database my-database on instance my-instance
    ```

## Contributing changes

* See [CONTRIBUTING.md](../../CONTRIBUTING.md)

## Licensing

* See [LICENSE](../../LICENSE)

## Testing

* See [TESTING.md](../../TESTING.md)
