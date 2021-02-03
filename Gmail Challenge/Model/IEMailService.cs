using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GmailChallenge.Model
{
    public interface IEMailService
    {
        public IEnumerable<EMail> GetEmails();
        public bool AddEmail(EMail eMail);
        public int AddDevOpsEmails();
    }
}
