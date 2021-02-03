# GmailChallenge

## Running app
- Make sure u have installed .net core 3.1 sdk and have a way to send request (Like Postman)

- Go to the root of the project and Run the following commands
```
dotnet restore
dotnet build
dotnet ef migrations add CreateGmailChallengeDB --project GmailChallenge
dotnet ef database update --project GmailChallenge
dotnet run --project GmailChallenge
```

- Open Postman and import the "GMailChallenge.postman_collection.json" collection to start using the endpoints

- Have Fun
