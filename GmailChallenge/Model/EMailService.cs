using GmailChallenge.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GmailChallenge.Model
{
    public class EMailService : IEMailService
    {
        private readonly IEMailRepository _eMailRepository;
        private readonly IGMailService _gMailService;

        public EMailService(IEMailRepository eMailRepository, IGMailService gMailServiceProvider)
        {
            _eMailRepository = eMailRepository;
            _gMailService = gMailServiceProvider;
        }

        public int AddDevOpsEmails(string user)
        {
           _gMailService.setGmailService(user);

            var messages = _gMailService.getMessages("DevOps");

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
                        var message = _gMailService.getMessage(messageId);
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
    }
}
