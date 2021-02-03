using System.Collections.Generic;

namespace GmailChallenge.Repository
{
    public interface IEMailRepository
    {
        public bool AddEMail(EMail email);
        public IEnumerable<EMail> GetEMails();

    }
}
