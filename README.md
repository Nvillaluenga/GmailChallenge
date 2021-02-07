# GmailChallenge

## Running app
- Make sure you have installed docker and docker-compose and have a way to send http request (Like Postman or firefox)

- Go to the root of the project and Run the following commands
```
docker-compose up
```

## Using the app

- Open Firefox and try the following URIs (We recommend you try in order to test the functionallity):
  1. http://localhost:5001/Api/V1/GmailChallenge/Email/ReadEmails (Read Emails with "DevOps" in the body and/or subject and returns a Json with all the emails (Will save the same email multiple times if called more than once))
  2. http://localhost:5001/Api/V1/GmailChallenge/Email (Returns a Json with all the emails)
  3. http://localhost:5001/Api/V1/GmailChallenge/Email/{Id:int} (Returns a Json with corresponding email)

- Open Postman desktop and import the "GMailChallenge.postman_collection.json" collection to start using the endpoints
  - You will see, there is a few more endpoints you can play with, but do not use http://localhost:5001/Api/V1/GmailChallenge/Email/ReadEmails as Postman doesn't redirect properly to google login

- Have Fun