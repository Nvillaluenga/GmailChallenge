using System.Collections.Generic;

namespace GmailChallenge.Model
{
    public interface IEmailService
    {
        public IEnumerable<Email> GetEmails();
        public Email GetEmail(int emailId);
        public bool AddEmail(Email email);
        public bool DeleteEmail(Email email);
        public void BlackHoleEmails();
        public int AddDevOpsEmails(string user);
    }
}
