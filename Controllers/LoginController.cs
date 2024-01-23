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
        IUserRepository authenticationManager) : ControllerBase
    {
        private readonly IConfiguration _config = config;
        private readonly IJwtGenerator _jwtGenerator = jwtGenerator;
        private readonly IUserRepository _authenticationManager = authenticationManager;

        [HttpPost("register")]
        public IActionResult Register([FromBody] UserLogin model) => ProcessRequest(model);

        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] UserLogin model) => ProcessRequest(model);

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok();
        }
        //[HttpGet("refresh")]
        //public IActionResult RefreshToken()
        //{
        //    HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        //    return Ok();
        //}
        [HttpGet("refresh")]
        public IActionResult RefreshToken([FromServices] IHttpContextAccessor httpContextAccessor)
        {
            ClaimsIdentity user = (ClaimsIdentity)httpContextAccessor.HttpContext.User.Identity;

            var cookies = httpContextAccessor.HttpContext.Request.Cookies;
            if (!cookies.TryGetValue("token", out var refreshToken))
            {
                return StatusCode(401); // Unauthorized
            }

            //var LoggedInUserId = Convert.ToUInt32(user.FindFirst("user").Value.ToString());

            //HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok();
        }

        [HttpPost("refreshs")]
        public async Task<IActionResult> HandleRefreshToken([FromServices] IHttpContextAccessor httpContextAccessor)
        {
            var cookies = httpContextAccessor.HttpContext.Request.Cookies;
            if (!cookies.TryGetValue("jwt", out var refreshToken))
            {
                return StatusCode(401); // Unauthorized
            }

            //var foundUser = await UserRepository.FindByRefreshTokenAsync(refreshToken); // Replace with your actual user repository logic
            //if (foundUser == null)
            //{
            //    return StatusCode(403); // Forbidden
            //}

            //try
            //{
            //    var tokenHandler = new JwtSecurityTokenHandler();
            //    var key = Convert.FromBase64String(_configuration["REFRESH_TOKEN_SECRET"]);

            //    tokenHandler.ValidateToken(refreshToken, new TokenValidationParameters
            //    {
            //        IssuerSigningKey = new SymmetricSecurityKey(key),
            //        ValidateIssuerSigningKey = true,
            //        ValidateIssuer = false,
            //        ValidateAudience = false,
            //        ClockSkew = TimeSpan.Zero
            //    }, out SecurityToken validatedToken);

            //    var jwtToken = (JwtSecurityToken)validatedToken;

            //    if (foundUser.Username != jwtToken.Subject)
            //    {
            //        return StatusCode(403); // Forbidden
            //    }

            //    var roles = foundUser.Roles.ToArray();
            //    var accessToken = GenerateAccessToken(jwtToken.Subject, roles); // Replace with your actual access token generation logic

            //    return Ok(new { Roles = roles, AccessToken = accessToken });
            //}
            //catch (SecurityTokenException)
            //{
            //    return StatusCode(403); // Forbidden
            //}


            return StatusCode(403);
        }

        private string GenerateAccessToken(string username, string[] roles)
        {
            //var key = Convert.FromBase64String(_configuration["ACCESS_TOKEN_SECRET"]);
            //var tokenDescriptor = new SecurityTokenDescriptor
            //{
            //    Subject = new System.Security.Claims.ClaimsIdentity(new[]
            //    {
            //    new System.Security.Claims.Claim("UserInfo", System.Text.Json.JsonSerializer.Serialize(new
            //    {
            //        username,
            //        roles
            //    }))
            //}),
            //    Expires = DateTime.UtcNow.AddSeconds(10), // Replace with your desired expiration time
            //    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            //};

            //var tokenHandler = new JwtSecurityTokenHandler();
            //var token = tokenHandler.CreateToken(tokenDescriptor);

            //return tokenHandler.WriteToken(token);

            return "";
        }

        private IActionResult TokenGenerator()
        {
            JwtModel jwtProps = _jwtGenerator.CreateRefreshToken(new UserLogin());

            HttpContext.SignInAsync(jwtProps.cookieAuth, jwtProps.claimsPrincipal, jwtProps.authenticationProperties).Wait();

            return Ok(new
            {
                Token = new JwtSecurityTokenHandler().WriteToken(jwtProps.jwtSecurityToken),
                Expiration = jwtProps.jwtSecurityToken.ValidTo
            });
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
                    List<int> user_roles = [2001];
                    JwtModel jwtProps = _jwtGenerator.CreateRefreshToken(detailsModel);

                    //HttpContext.SignInAsync(jwtProps.cookieAuth, jwtProps.claimsPrincipal, jwtProps.authenticationProperties).Wait();

                    return Ok(new
                    {
                        Token = new JwtSecurityTokenHandler().WriteToken(jwtProps.jwtSecurityToken),
                        Expiration = jwtProps.jwtSecurityToken.ValidTo,
                        Roles = user_roles
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
