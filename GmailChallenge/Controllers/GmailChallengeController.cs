using System;
using System.Collections.Generic;
using System.Text.Json;
using GmailChallenge.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GmailChallenge.Controllers
{
    [ApiController]
    [Route("Api/V1/GmailChallenge")]
    public class GmailChallengeController : ControllerBase
    {
        private readonly ILogger<GmailChallengeController> _logger;
        private readonly IEMailService _eMailService;

        public GmailChallengeController(ILogger<GmailChallengeController> logger, IEMailService eMailService)
        {
            _logger = logger;
            _eMailService = eMailService;
        }

        [HttpGet]
        [Route("EMails")]
        public ActionResult<IEnumerable<EMail>> GetEMails()
        {
            try
            {
                return Ok(_eMailService.GetEmails());
            } 
            catch(Exception e)
            {
                _logger.LogError(e, "Something went wrong when retrieving EMails.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [Route("EMails")]
        public ActionResult PostEMails([FromBody] EMail eMail)
        {
            try
            {
                if (!_eMailService.AddEmail(eMail))
                    throw new Exception($"The Email: {JsonSerializer.Serialize(eMail)} Could not be added.");
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Something went wrong when Adding an EMail.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [Route("ReadEmails")]
        public ActionResult ReadEmails([FromQuery] string user = "defaultUser")
        {
            _eMailService.AddDevOpsEmails(user);
            return Ok();
        }
    }
}
