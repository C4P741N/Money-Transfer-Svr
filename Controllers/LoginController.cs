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
using Amazon.Runtime.Internal;
using System.IdentityModel.Tokens.Jwt;
using Amazon.Auth.AccessControlPolicy;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Security.Principal;
using money_transfer_server_side.EnumsFactory;

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
        private readonly IMts_AuthenticationManager _authenticationManager = authenticationManager;

        [HttpPost("register")]
        public IActionResult Register([FromBody] UserLogin model) => ProcessRequest(model);

        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] UserLogin model) => ProcessRequest(model);

        [HttpPost("Logout")]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok();
        }

        private IActionResult ProcessRequest(
            UserLogin detailsModel)
        {
            try
            {
                if (detailsModel.authType is AuthTypes.None) return BadRequest();

                HttpStatusCode status = _authenticationManager.Begin(detailsModel, config);

                if (status is HttpStatusCode.Found)
                {
                    JwtModel jwtProps = _jwtGenerator.CreateToken(detailsModel);

                    //HttpContext.SignInAsync(jwtProps.cookieAuth, jwtProps.claimsPrincipal, jwtProps.authenticationProperties).Wait();

                    return Ok(new
                    {
                        Token = new JwtSecurityTokenHandler().WriteToken(jwtProps.jwtSecurityToken),
                        Expiration = jwtProps.jwtSecurityToken.ValidTo
                    });
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
