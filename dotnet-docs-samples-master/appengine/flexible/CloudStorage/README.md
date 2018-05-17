# Google Cloud Storage and Google App Engine Flexible Environment

This sample application demonstrates store data in Google Cloud Storage
when running in Google App Engine Flexible Environment.

## Prerequisites

1.  **Follow the instructions in the [root README](../../../README.md).**
  
2.  Install the [Google Cloud SDK](https://cloud.google.com/sdk/).  The Google Cloud SDK
    is required to deploy .NET applications to App Engine.

3.  Install the [.NET Core SDK, version 1.1](https://github.com/dotnet/core/blob/master/release-notes/download-archives/1.1.4-download.md).

4.  Create a Cloud Storage bucket and make it [publicly readable](
	https://cloud.google.com/storage/docs/access-control/#applyacls).
	Use any name for the bucket name.
    ```ps1
	gsutil mb gs://[your-bucket-name]
	gsutil defacl set public-read gs://[your-bucket-name]
	```
	
5.  Edit [appsettings.json](appsettings.json).  Replace `your-google-bucket-name` with your bucket name.

## ![PowerShell](../.resources/powershell.png) Using PowerShell

### Run Locally

```psm1
PS > dotnet restore
PS > dotnet run
```

### Deploy to App Engine

```psm1
PS > dotnet restore
PS > dotnet publish
PS > gcloud beta app deploy .\bin\Debug\netcoreapp1.0\publish\app.yaml
```


## ![Visual Studio](../.resources/visual-studio.png) Using Visual Studio 2017

Visual Studio is *optional*.  An old, unmaintained branch of samples that work
with Visual Studio 2015 is 
[here](https://github.com/GoogleCloudPlatform/dotnet-docs-samples/tree/vs2015).

[Google Cloud Tools for Visual Studio](
https://marketplace.visualstudio.com/items?itemName=GoogleCloudTools.GoogleCloudPlatformExtensionforVisualStudio)
make it easy to deploy to App Engine.  Install them if you are running Visual Studio.

### Run Locally

Open **CloudStorage.csproj**, and Press **F5**.

### Deploy to App Engine

1.  In Solution Explorer, right-click the **CloudStorage** project and choose **Publish CloudStorage to Google Cloud**.

2.  Click **App Engine Flex**.

3.  Click **Publish**.
