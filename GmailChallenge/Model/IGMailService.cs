using Google.Apis.Gmail.v1.Data;
using System.Collections.Generic;

namespace GmailChallenge.Model
{
    public interface IGMailService
    {
        public void setGmailService();
        public List<Message> getMessages(string body, string subject);
        public Message getMessage(string Id);
    }
}
