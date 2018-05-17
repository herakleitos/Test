# Cloud Storage Sample

A sample demonstrating how to invoke [Google Cloud Storage](
https://cloud.google.com/storage/docs/) from C#.

This sample requires [.NET Core 2.0](
    https://www.microsoft.com/net/core) or later.  That means using
[Visual Studio 2017](
    https://www.visualstudio.com/), or the command line.  Visual Studio 2015 users
can use [this older sample](
    https://github.com/GoogleCloudPlatform/dotnet-docs-samples/tree/vs2015/storage/api).

## Links

- [Cloud Storage Client Libraries](https://cloud.google.com/storage/docs/reference/libraries#client-libraries-install-csharp)

## Build and Run

1.  **Follow the instructions in the [root README](../../README.md)**.

4.  Enable APIs for your project.
    [Click here](https://console.cloud.google.com/flows/enableapi?apiid=storage_api&showconfirmation=true)
    to visit Cloud Platform Console and enable the Google Cloud Speech API.

7.  Edit `QuickStart\Program.cs`, and replace YOUR-PROJECT-ID with id
    of the project you created in step 1.

7.  Edit `Storage\Program.cs`, and replace YOUR-PROJECT-ID with id
    of the project you created in step 1.

8.  Build:

    ```ps1
    PS > dotnet restore
    PS > dotnet build
    ```

9.  From the command line, run QuickStart to create a bucket:

    ```ps1
    PS > dotnet run --project .\QuickStart\QuickStart.csproj
    You already own this bucket. Please select another name.
    ```

10. From the command line, run Storage to see a list of commands:
    
    ```ps1
    PS > dotnet run --project .\Storage\Storage.csproj
    Usage:
    Storage create [new-bucket-name]
    Storage list
    Storage list bucket-name [prefix] [delimiter]
    Storage get-metadata bucket-name object-name
    Storage make-public bucket-name object-name
    Storage upload [-key encryption-key] bucket-name local-file-path [object-name]
    Storage copy source-bucket-name source-object-name dest-bucket-name dest-object-name
    Storage move bucket-name source-object-name dest-object-name
    Storage download [-key encryption-key] bucket-name object-name [local-file-path]
    Storage download-byte-range bucket-name object-name range-begin range-end [local-file-path]
    Storage generate-signed-url bucket-name object-name
    Storage view-bucket-iam-members bucket-name
    Storage add-bucket-iam-member bucket-name member
    Storage remove-bucket-iam-member bucket-name role member
    Storage print-acl bucket-name
    Storage print-acl bucket-name object-name
    Storage add-owner bucket-name user-email
    Storage add-owner bucket-name object-name user-email
    Storage add-default-owner bucket-name user-email
    Storage remove-owner bucket-name user-email
    Storage remove-owner bucket-name object-name user-email
    Storage remove-default-owner bucket-name user-email
    Storage delete bucket-name
    Storage delete bucket-name object-name [object-name]
    Storage enable-requester-pays bucket-name
    Storage disable-requester-pays bucket-name
    Storage get-requester-pays bucket-name
    Storage generate-encryption-key
    ```

## Contributing changes

* See [CONTRIBUTING.md](../../CONTRIBUTING.md)

## Licensing

* See [LICENSE](../../LICENSE)
