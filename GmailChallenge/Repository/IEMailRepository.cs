using System.Collections.Generic;

namespace GmailChallenge.Repository
{
    public interface IEmailRepository
    {
        public bool AddEmail(Email email);
        public IEnumerable<Email> GetEmails();
        public Email GetEmail(int emailId);
        public bool DeleteEmail(Email email);
    }
}
