using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using money_transfer_server_side.JsonExtractors;
using money_transfer_server_side.Models;
using static money_transfer_server_side.EnumsFactory.EnumsAtLarge;
using System.Text.Json;
using money_transfer_server_side.Utils;
using System.Net;

namespace money_transfer_server_side.Controllers
{
    [Route("auth")]
    [ApiController]
    public class LoginController(
        IConfiguration config,
        IJwtGenerator jwtGenerator,
        IMts_AuthenticationManager authenticationManager) : ControllerBase
    {
        private readonly IConfiguration _config = config;
        private readonly IJwtGenerator _jwtGenerator = jwtGenerator;
        private IMts_AuthenticationManager _authenticationManager = authenticationManager;

        [HttpPost("register")]
        public ActionResult Register([FromBody] string registrationDetails) =>
             ProcessRequest(registrationDetails, AuthTypes.Registration);

        [HttpPost("authenticate")]
        public ActionResult Authenticate([FromBody] string auth) =>
             ProcessRequest(auth, AuthTypes.Authentication);

        private ActionResult ProcessRequest(
            string jsonString,
            AuthTypes type)
        {
            try
            {
                UserLogin detailsModel = JsonSerializer.Deserialize<UserLogin>(jsonString);

                if (detailsModel is null) return BadRequest();

                detailsModel.AuthType = type;

                HttpStatusCode status = _authenticationManager.Begin(detailsModel, config);

                if (status is HttpStatusCode.Found)
                {
                    return new OkObjectResult(new { Token = _jwtGenerator.AttachSuccessToken(detailsModel, _config) });
                }

                return new ObjectResult(status)
                {
                    StatusCode = (int)status
                };
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
