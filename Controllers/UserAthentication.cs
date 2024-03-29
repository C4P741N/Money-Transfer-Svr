﻿using Microsoft.AspNetCore.Authorization;
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
    public class UserAthentication(
        IConfiguration config,
        IJwtGenerator jwtGenerator,
        IUserRepository authenticationManager) : ControllerBase
    {
        private readonly IConfiguration _config = config;
        private readonly IJwtGenerator _jwtGenerator = jwtGenerator;
        private readonly IUserRepository _authenticationManager = authenticationManager;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserLogin model)
        {
            return await ProcessRequest(model);
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] UserLogin model)
        {
            return await ProcessRequest(model);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok();
        }
        private async Task<IActionResult> ProcessRequest(
            UserLogin detailsModel)
        {
            try
            {
                if (detailsModel.authType is AuthTypes.None) return BadRequest();

                HttpStatusCode status = await _authenticationManager.Begin(detailsModel, config);

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
