using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using money_transfer_server_side.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using MongoDB.Driver.Linq;

namespace money_transfer_server_side.Utils
{
    public class JwtGenerator(IConfiguration config) : IJwtGenerator
    {
        private readonly SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(config["Jwt:Key"] ?? throw new ArgumentException("Invalid JWT string")));
        public JwtModel CreateRefreshToken(UserLogin user)
        {
            _ = int.TryParse(config["JWT:TokenValidityInMinutes"], out int tokenValidityInMinutes);
            SigningCredentials credentials = new(securityKey, SecurityAlgorithms.HmacSha256);

            Claim[] authClaims =
            [
                new Claim("userName", user.user),
                //new Claim(ClaimTypes.Role, "User") //User Roles
            ];

            JwtSecurityToken token = new(
                issuer: config["Jwt:Issuer"],
                audience: config["Jwt:Audience"],
                claims: authClaims,
                expires: DateTime.UtcNow.AddMinutes(tokenValidityInMinutes),//.AddHours(3),
                signingCredentials: credentials);

            ClaimsIdentity identity = new(authClaims, CookieAuthenticationDefaults.AuthenticationScheme);

            AuthenticationProperties props = new();
            props.Parameters.Add("token", token);

            return new JwtModel
            {
                authenticationProperties = props,
                jwtSecurityToken = token,
                claimsPrincipal = new ClaimsPrincipal(identity),
                cookieAuth = CookieAuthenticationDefaults.AuthenticationScheme
            };
        }
    }
}
