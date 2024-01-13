using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using money_transfer_server_side.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace money_transfer_server_side.Utils
{
    public class JwtGenerator : IJwtGenerator
    {
        public string AttachSuccessToken(
            UserLogin userLogin,
            IConfiguration config)
        {
            return Generate(userLogin, config);

            //var token = Generate(userLogin, config);

            //var response = new HttpResponseMessage(HttpStatusCode.Found);
            //response.Headers.Add("Authorization", "Bearer " + token);

            //return response;
        }
        private string Generate(
            UserLogin user,
            IConfiguration config)
        {
            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(config["Jwt:Key"] ?? "wrong JWT Key string assignment"));
            SigningCredentials credentials = new(securityKey, SecurityAlgorithms.HmacSha256);
            Claim[] claims =
            [
                new Claim(ClaimTypes.NameIdentifier, user.user),
                new Claim(ClaimTypes.Role, "User") //User Roles
            ];
            JwtSecurityToken token = new(config["Jwt:Issuer"],
                config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddSeconds(30),
                signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
