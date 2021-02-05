using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace GmailChallenge.Repository
{
    public class EmailRepository : IEmailRepository
    {
        private readonly ApplicationDbContext _appDbContext;

        public EmailRepository(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public bool AddEmail(Email email)
        {
            _appDbContext.Emails.Add(email);
            return _appDbContext.SaveChanges() == 1;
        }

        public bool DeleteEmail(Email email)
        {
            _appDbContext.Emails.Remove(email);
            return _appDbContext.SaveChanges() == 1;
        }

        public Email GetEmail(int emailId)
        {
            return _appDbContext.Emails.Find(emailId);
        }

        public IEnumerable<Email> GetEmails()
        {
            return _appDbContext.Emails.ToList();
        }
    }
}
