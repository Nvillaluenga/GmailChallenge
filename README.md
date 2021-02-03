# GmailChallenge

## Setting the environment
-Make sure u have installed .net core 3.1 sdk

- Go to the root of the project and Run the following commands
```
dotnet restore
dotnet build
dotnet tool restore
dotnet ef migrations add CreateGmailChallengeDB --project GmailChallenge
dotnet ef database update --project GmailChallenge
dotnet run --project GmailChallenge
```
