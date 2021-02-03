using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace GmailChallenge.Repository
{
    public class EMailRepository : IEMailRepository
    {
        private readonly ApplicationDbContext _appDbContext;

        public EMailRepository(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public bool AddEMail(EMail email)
        {
            _appDbContext.EMails.Add(email);
            return _appDbContext.SaveChanges() == 1;
        }

        public IEnumerable<EMail> GetEMails()
        {
            return _appDbContext.EMails.ToList();
        }
    }
}
