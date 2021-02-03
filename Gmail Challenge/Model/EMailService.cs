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
using System.Threading;

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
        public int AddDevOpsEmails()
        {
            UsersResource.LabelsResource.ListRequest request = _gMailService.Users.Labels.List("me");

            // List labels.
            IList<Label> labels = request.Execute().Labels;
            Console.WriteLine("Labels:");
            if (labels != null && labels.Count > 0)
            {
                foreach (var labelItem in labels)
                {
                    Console.WriteLine("{0}", labelItem.Name);
                }
            }
            else
            {
                Console.WriteLine("No labels found.");
            }
            Console.Read();
            throw new NotImplementedException();   
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
