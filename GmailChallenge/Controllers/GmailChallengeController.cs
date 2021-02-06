using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using GmailChallenge.Model;
using Google.Apis.Auth.AspNetCore3;
using Google.Apis.Gmail.v1;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GmailChallenge.Controllers
{
    [ApiController]
    [Route("Api/V1/GmailChallenge/Email")]
    public class GmailChallengeController : ControllerBase
    {
        private readonly ILogger<GmailChallengeController> _logger;
        private readonly IEmailService _emailService;

        public GmailChallengeController(ILogger<GmailChallengeController> logger, IEmailService emailService)
        {
            _logger = logger;
            _emailService = emailService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Email>> GetEmails()
        {
            try
            {
                return Ok(_emailService.GetEmails());
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Something went wrong when retrieving Emails.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [Route("{emailId:int}")]
        public ActionResult<IEnumerable<Email>> GetEmail(int emailId)
        {
            try
            {
                return Ok(_emailService.GetEmail(emailId));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Something went wrong when retrieving Email.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        [HttpPost]
        public ActionResult PostEmails([FromBody] Email email)
        {
            try
            {
                if (!_emailService.AddEmail(email))
                    throw new Exception($"The Email: {JsonSerializer.Serialize(email)} Could not be added.");
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Something went wrong when Adding an Email.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete]
        [Route("{emailId:int}")]
        public ActionResult<IEnumerable<Email>> DeleteEmail(int emailId)
        {
            try
            {
                var email = _emailService.GetEmail(emailId);
                if (email == null)
                    return Ok(true); // idempotencia
                return Ok(_emailService.DeleteEmail(email));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Something went wrong when retrieving Email.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete]
        [Route("BlackHole")]
        public ActionResult BlackHoleEmails()
        {
            try
            {
                _emailService.BlackHoleEmails();
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Something went wrong when retrieving Email.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [Route("ReadEmails")]
        [GoogleScopedAuthorize(GmailService.ScopeConstants.GmailReadonly)]
        public ActionResult ReadEmails()
        {
            if (User.Identity.IsAuthenticated)
            {
                HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).Wait();
            }
            _emailService.AddDevOpsEmails();
            return Ok();
        }
    }
}
