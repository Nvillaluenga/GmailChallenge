using System.Collections.Generic;

namespace GmailChallenge.Model
{
    public interface IEMailService
    {
        public IEnumerable<EMail> GetEmails();
        public bool AddEmail(EMail eMail);
        public int AddDevOpsEmails(string user);
    }
}
