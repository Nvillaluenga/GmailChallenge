# GmailChallenge

## Running app
- Make sure you have installed docker and have a way to send http request (Like Postman or firefox)

- Go to the root of the project and Run the following commands
```
docker build -t gmailchallenge .
docker run -it -e "ASPNETCORE_URLS=http://+:80" -e "ASPNETCORE_HTTP_PORT=5001" -e "ASPNETCORE_ENVIRONMENT=Development" -e "ASPNETCORE_LOGGING__CONSOLE__DISABLECOLORS=true" -p 5001:80 --name GmailChallenge gmailchallenge
```

- Open Postman and import the "GMailChallenge.postman_collection.json" collection to start using the endpoints

- Have Fun
