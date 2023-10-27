# Overview

Forum-io-backend is a backend service for for [forum-io frontend (Github repo)](https://github.com/topiaspeiponen/forum-io). This project was created with ASP.NET Core and largely follows the instructions provided in the [minimal API with ASP.NET Core tutorial](https://learn.microsoft.com/en-us/aspnet/core/tutorials/min-web-api?view=aspnetcore-7.0&tabs=visual-studio-code).

## Local development

You can either run the application through the command line or using Docker.

1. To run the application through the command line:
**Development environment**
```dotnet run --launch-profile Dev```
**Production environment**
```dotnet run --launch-profile Production```

Note that the URL of the application changes depending on the environment used. URLs can be found in the Properties/launchSettings.json configuration file.

2. To run the application using Docker. Note that the container builds in the production environment by default.

```docker build -t forum-io-backend-image .```
```docker run -d -p 8080:80 --name forum-io-backend forum-io-backend-image```