using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace GmailChallenge.Model
{
    public class GMailService : IGMailService
    {
        private readonly IFileProvider _fileProvider;
        private GmailService gMailService;

        public GMailService(IFileProvider fileProvider)
        {
            _fileProvider = fileProvider;
        }

        public List<Message> getMessages(string body, string subject)
        {
            var messageRequest = gMailService.Users.Messages.List("me");
            messageRequest.Q = $"\"{body}\" OR subject:{subject}";

            return (List<Message>)messageRequest.Execute().Messages;
        }

        public Message getMessage(string Id)
        {
            return gMailService.Users.Messages.Get("me", Id).Execute();
        }

        public void setGmailService(string emailToQuery)
        {
            UserCredential credential;
            var baseRepository = $"{Path.DirectorySeparatorChar}Resources{Path.DirectorySeparatorChar}";
            using (var stream =
                _fileProvider.GetFileInfo($"{baseRepository}credentials.json").CreateReadStream())
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string credPath = $"{baseRepository}token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    new List<string> { GmailService.Scope.GmailReadonly },
                    emailToQuery,
                    CancellationToken.None).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            // Create Gmail API service.
            gMailService =  new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "GmailChallenge",
            });
        }
    }
}
