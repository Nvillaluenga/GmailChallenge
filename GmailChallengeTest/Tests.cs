using GmailChallenge;
using GmailChallenge.Model;
using GmailChallenge.Repository;
using Google.Apis.Gmail.v1.Data;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GmailChallengeTest
{
    [TestFixture]
    public class Tests
    {
        private IGMailService _gMailServiceProviderMock;
        private IEmailRepository _emailRepositoryMock;
        private string _user;

        [SetUp]
        public void Setup()
        {
            _user = "Example";
            var messagesLite = new List<Message>
            {
                new Message
                {
                    Id = "1"
                },
                new Message
                {
                    Id = "2"
                }
            };
            var messagesFull = new List<Message>
            {
                new Message
                {
                    Id = "1",
                    Payload = new MessagePart
                    {
                        Headers = new List<MessagePartHeader>
                        {
                            new MessagePartHeader
                            {
                                Name = "Subject",
                                Value = "DevOps Test_1"
                            },new MessagePartHeader
                            {
                                Name = "From",
                                Value = "Test1@Test.com"
                            },new MessagePartHeader
                            {
                                Name = "Date",
                                Value = $"{DateTime.Now.Ticks}"
                            }
                        }
                    }
                },
                new Message
                {
                    Id = "2",
                    Payload = new MessagePart
                    {
                        Headers = new List<MessagePartHeader>
                        {
                            new MessagePartHeader
                            {
                                Name = "Subject",
                                Value = "DevOps Test_2"
                            },new MessagePartHeader
                            {
                                Name = "From",
                                Value = "Test2@Test.com"
                            },new MessagePartHeader
                            {
                                Name = "Date",
                                Value = $"{DateTime.Now}"
                            }
                        }
                    }
                }
            };

            _gMailServiceProviderMock = Mock.Of<IGMailService>();
            Mock.Get(_gMailServiceProviderMock).Setup(gs => gs.getMessages("DevOps", "DevOps"))
                .Returns(messagesLite);

            Mock.Get(_gMailServiceProviderMock).Setup(gs => gs.getMessage("1"))
                .Returns(messagesFull.FirstOrDefault(m => m.Id == "1"));

            Mock.Get(_gMailServiceProviderMock).Setup(gs => gs.getMessage("2"))
                .Returns(messagesFull.FirstOrDefault(m => m.Id == "2"));


            _gMailServiceProviderMock = Mock.Of<IGMailService>();
            Mock.Get(_gMailServiceProviderMock).Setup(gr => gr.setGmailService(It.IsAny<string>()))
                .Verifiable(); ;

            _emailRepositoryMock = Mock.Of<IEmailRepository>();
            Mock.Get(_emailRepositoryMock).Setup(er => er.AddEmail(It.IsAny<Email>()));


        }

        [Test]
        public void readEmailTest()
        {
            var emailService = new EmailService(_emailRepositoryMock, _gMailServiceProviderMock);
            var output = emailService.AddDevOpsEmails("Example");
        }


    }
}