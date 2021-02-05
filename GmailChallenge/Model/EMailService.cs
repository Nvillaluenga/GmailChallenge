using GmailChallenge.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GmailChallenge.Model
{
    public class EmailService : IEmailService
    {
        private readonly IEmailRepository _emailRepository;
        private readonly IGMailService _gMailService;

        public EmailService(IEmailRepository emailRepository, IGMailService gMailServiceProvider)
        {
            _emailRepository = emailRepository;
            _gMailService = gMailServiceProvider;
        }

        public int AddDevOpsEmails(string user)
        {
           _gMailService.setGmailService(user);

            var messages = _gMailService.getMessages("DevOps", "DevOps");

            var messageCount = messages?.Count ?? 0;

            List<Email> emails = new List<Email> { };
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
                        var messagePartDate = (long)message.InternalDate;
                        emails.Add(new Email
                        {
                            Fecha = DateTimeOffset.FromUnixTimeMilliseconds(messagePartDate).UtcDateTime,
                            From = messagePartFrom.Value,
                            Subject = messagePartSubject.Value
                        });
                    });
                    tasks.Add(task);
                }
                try
                {
                    Task.WaitAll(tasks.ToArray());
                    emails.ForEach(email => _emailRepository.AddEmail(email));
                }
                catch (AggregateException exceptions)
                {
                    exceptions.InnerExceptions.ToList().ForEach(e =>
                       Console.WriteLine($"Something failed when reading/writting the emails {e.Message}"));
                }
            }

            return emails.Count;
        }

        public bool AddEmail(Email email)
        {
            return _emailRepository.AddEmail(email);
        }
        public bool DeleteEmail(Email email)
        {
            return _emailRepository.DeleteEmail(email);
        }

        public IEnumerable<Email> GetEmails()
        {
            return _emailRepository.GetEmails();
        }
        public Email GetEmail(int emailId)
        {
            return _emailRepository.GetEmail(emailId);
        }
        public void BlackHoleEmails()
        {
            _emailRepository.GetEmails().ToList()
                .ForEach(email => _emailRepository.DeleteEmail(email));
        }
    }
}
