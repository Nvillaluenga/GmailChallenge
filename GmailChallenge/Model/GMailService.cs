using Google.Apis.Auth.AspNetCore3;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace GmailChallenge.Model
{
    public class GMailService : IGMailService
    {
        private readonly IGoogleAuthProvider _googleAuthProvider;
        private GmailService gMailService;

        public GMailService(IFileProvider fileProvider, IGoogleAuthProvider googleAuthProvider)
        {
            _googleAuthProvider = googleAuthProvider;
        }

        public List<Message> getMessages(string body, string subject)
        {
            var messageRequest = gMailService.Users.Messages.List("me");
            messageRequest.Q = $"\"{body}\" OR subject:{subject}";

            return (List<Message>)messageRequest.Execute().Messages;
        }

        public Message getMessage(string Id)
        {
            return gMailService.Users.Messages.Get("me", Id).Execute();
        }

        public void setGmailService()
        {
            var task1 = _googleAuthProvider.RequireScopesAsync(GmailService.ScopeConstants.GmailReadonly);
            task1.Wait();
            if (!(task1.Result is IActionResult))
            {
                var task2 = _googleAuthProvider.GetCredentialAsync();
                task2.Wait();
                GoogleCredential cred = task2.Result;
                gMailService = new GmailService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = cred
                });
            }
        }

        //public void logOut()
        //{
        //    if (User.Identity.IsAuthenticated)
        //    {
        //        HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        //    }
        //}
    }
}
