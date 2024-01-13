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
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController(
        IConfiguration config,
        IJwtGenerator jwtGenerator,
        IMts_AuthenticationManager authenticationManager) : ControllerBase
    {
        private readonly IConfiguration _config = config;
        private readonly IJwtGenerator _jwtGenerator = jwtGenerator;
        private IMts_AuthenticationManager _authenticationManager = authenticationManager;

        [HttpPost]
        [Route("/[controller]/[action]")]
        public ActionResult Register([FromBody] string registrationDetails) =>
             ProcessRequest(registrationDetails, TrasactionTypes.Registration);

        [HttpPost]
        [Route("/[controller]/[action]")]
        public ActionResult Authenticate([FromBody] string auth) =>
             ProcessRequest(auth, TrasactionTypes.Authentication);

        private ActionResult ProcessRequest(
            string jsonString,
            TrasactionTypes type)
        {
            UserLogin detailsModel = JsonSerializer.Deserialize<UserLogin>(jsonString);

            if (detailsModel == null) return BadRequest();

            detailsModel.TrasactionType = type;

            HttpResponseMessage result = _authenticationManager.Begin(detailsModel, config);

            if (result.StatusCode == HttpStatusCode.Found)
            {
                return new OkObjectResult(new { Token = _jwtGenerator.AttachSuccessToken(detailsModel, _config) });
            }

            return new ObjectResult(result)
            {
                StatusCode = (int)result.StatusCode
            };
        }
    }
}
