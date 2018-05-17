# Google Cloud SQL and Google App Engine Flexible Environment

This sample application demonstrates how to store data in Google Cloud Datastore
when running in Google App Engine Flexible Environment.

## Prerequisites

1.  **Follow the instructions in the [root README](../../../README.md).**
  
2.  Install the [Google Cloud SDK](https://cloud.google.com/sdk/).  The Google Cloud SDK
    is required to deploy .NET applications to App Engine.

2.  Install the [.NET Core SDK, version 1.1](https://github.com/dotnet/core/blob/master/release-notes/download-archives/1.1.4-download.md).

4.  Edit [appsettings.json](appsettings.json).  Replace `your-project-id` with your Google project id.

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

Open **Datastore.csproj**, and Press **F5**.

### Deploy to App Engine

1.  In Solution Explorer, right-click the **Datastore** project and choose **Publish Datastore to Google Cloud**.

2.  Click **App Engine Flex**.

3.  Click **Publish**.
