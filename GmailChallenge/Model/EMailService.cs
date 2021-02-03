using GmailChallenge.Repository;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace GmailChallenge.Model
{
    public class EMailService : IEMailService
    {
        private readonly IEMailRepository _eMailRepository;
        private readonly IFileProvider _fileProvider;
        private readonly GmailService _gMailService;
        public EMailService(IEMailRepository eMailRepository, IFileProvider fileProvider)
        {
            _eMailRepository = eMailRepository;
            _fileProvider = fileProvider;
            _gMailService = GetGmailService();

        }

        public object JSonSerializer { get; private set; }

        public int AddDevOpsEmails()
        {
            var messageRequest = _gMailService.Users.Messages.List("me");
            messageRequest.Q = "subject: DevOps";

            var messages = (List<Message>)messageRequest.Execute().Messages;

            var messageCount = messages?.Count ?? 0;

            List<EMail> eMails = new List<EMail> { };
            List<Task> tasks = new List<Task> { };
            Console.WriteLine("DevOps messages:");
            if (messageCount > 0)
            {
                foreach (var messageId in messages.Select(m => m.Id))
                {
                    var task = Task.Run(() =>
                    {
                        var message = _gMailService.Users.Messages.Get("me", messageId).Execute();
                        var messagePartSubject = message.Payload.Headers.FirstOrDefault(h => h.Name == "Subject");
                        var messagePartFrom = message.Payload.Headers.FirstOrDefault(h => h.Name == "From");
                        var messagePartDate = message.Payload.Headers.FirstOrDefault(h => h.Name == "Date");
                        eMails.Add(new EMail
                        {
                            Fecha = DateTime.Parse(messagePartDate.Value),
                            From = messagePartFrom.Value,
                            Subject = messagePartSubject.Value
                        });
                    });
                    tasks.Add(task);
                }
                try
                {
                    Task.WaitAll(tasks.ToArray());
                    eMails.ForEach(eMail => _eMailRepository.AddEMail(eMail));
                }
                catch (AggregateException exceptions)
                {
                    exceptions.InnerExceptions.ToList().ForEach(e =>
                       Console.WriteLine($"Something failed when reading/writting the emails {e.Message}"));
                }
            }

            return eMails.Count;
        }

        public bool AddEmail(EMail eMail)
        {
            return _eMailRepository.AddEMail(eMail);
        }

        public IEnumerable<EMail> GetEmails()
        {
            return _eMailRepository.GetEMails();
        }

        private GmailService GetGmailService()
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
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            // Create Gmail API service.
            return new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "GmailChallenge",
            });
        }
    }
}
