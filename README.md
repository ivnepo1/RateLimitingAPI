# ApiRandomBytes by Ivan Nepomuceno

## Build and Run **with** Visual Studio

#### Top-Level Packages installed from NuGet:
```
...
  <ItemGroup>
    <PackageReference Include="Microsoft.AspNetCore.Authentication" Version="2.2.0" />
    <PackageReference Include="Microsoft.AspNetCore.Mvc.Core" Version="2.2.5" />
    <PackageReference Include="Microsoft.AspNetCore.OpenApi" Version="7.0.5" />
    <PackageReference Include="Swashbuckle.AspNetCore" Version="6.4.0" />
  </ItemGroup>
...
```

## Publishing the executable to an .exe file for Windows with Visual Studio
https://learn.microsoft.com/en-us/dotnet/core/tutorials/publishing-with-visual-studio?pivots=dotnet-7-0

## Summary of steps to publish 
1.   Open the project in Visual Studio.
2.   Right-click on the project in the Solution Explorer and select "Publish".
3.   In the "Publish" dialog, select "Folder" as the publish target and choose a folder where the published files will be stored.
4.   Click the "Publish" button to publish the files to the selected folder.
5.   Navigate to the published folder and locate the .exe file for the project.

## Build and Run **without** Visual Studio

### Prerequisites:

1.  .NET Core SDK installed on the machine.
2.   https://github.com/ivnepo1/RateLimitingAPI clone and setup in the local or target machine.
3.   Open a command prompt or terminal window and navigate to the root directory of the RateLimitingAPI github project.
4.   Install the Microsoft.AspNetCore.Authentication package by running the following command: `dotnet add package Microsoft.AspNetCore.Authentication`
5.   Install the Microsoft.AspNetCore.Mvc.Core package by running the following command: `dotnet add package Microsoft.AspNetCore.Mvc.Core`
6.   Install the Microsoft.AspNetCore.OpenApi package by running the following command: `dotnet add package Microsoft.AspNetCore.OpenApi`
7.   Install the Swashbuckle.AspNetCore package by running the following command: `dotnet add package Swashbuckle.AspNetCore`
8.   Restore any required NuGet packages by running the following command: `dotnet restore`
9.   Build the project by running the following command: `dotnet build`
10.  Run the project by running the following command: `dotnet run`

## Assumptions
GET /random - > means the endpoint must have the **last** part of the URL and not be the whole URL (localhost:5000/random vs localhost:5000/api/something/more/random)
HTTP Basic Auth - > No credential/password policy is needed as long as Basic Auth is implemented
Client can be identified with numbers and have individual rate limits - > not implemented
Returned object is simply JSON object with key-pair of "random" and base64 encoded string
*Extend the API with an Admin endpoint that can reset and/or modify the rate limit for a client.* -> this does **not** include the time window (10 seconds) and only the limit of bytes


## Features
- Extend the API with an Admin endpoint that can reset and/or modify the rate limit for a client.
- Limit can go up to 1048576 and as low as 100
- Postman Collection included for testing - endpoint is set to http://localhost:8888/api/random

## Notes
- Testing was done manually (Console App + Swagger included by Visual Studio + Postman):
    - GetRandomBytes
    - Base64Encode <-> Base64Decode
    - Rate limitting (with reset and modify) working based on returned "X-Rate-Limit" header
- Rate Limiting implemented with Fixed Window algorithm - https://devblogs.microsoft.com/dotnet/announcing-rate-limiting-for-dotnet/
- Lock utilized to prevent multiple requests from interfering with the rate limiting - https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/statements/lock?redirectedfrom=MSDN
- This is my first C# project with Visual Studio :D 