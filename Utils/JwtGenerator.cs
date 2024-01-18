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
    public class JwtGenerator(IConfiguration config) : IJwtGenerator
    {
        private readonly SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(config["Jwt:Key"] ?? "wrong JWT Key string assignment"));
        public string VerifyToken(
            string jwtToken)
        {
            JwtSecurityTokenHandler tokenHandler = new();
            var validationParameters = GetTokenValidationParameters();

            string userId = null;

            try
            {
                ClaimsPrincipal principal = tokenHandler.ValidateToken(jwtToken, validationParameters, out SecurityToken validatedToken);

                userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            }
            catch (SecurityTokenException ex)
            {
                Console.WriteLine($"Token validation failed: {ex.Message}");
            }

            //Error 403 if Token is invalid
            //Error 401 if no AuthHeader is found

            return userId;
        }
        private TokenValidationParameters GetTokenValidationParameters()
        {
            return new TokenValidationParameters
            {
                ValidateIssuer = true, // Set to true if you want to validate the issuer
                ValidateAudience = true, // Set to true if you want to validate the audience
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = securityKey
            };
        }
        public string AttachSuccessToken(
            UserLogin userLogin,
            IConfiguration config)
        {
            //var refreshToken = GenerateRefreshToken();

            return Generate(userLogin);

            //var token = Generate(userLogin, config);

            //var response = new HttpResponseMessage(HttpStatusCode.Found);
            //response.Headers.Add("Authorization", "Bearer " + token);

            //return response;
        }
        private string Generate(
            UserLogin user)
        {
            
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

            //ACCESS_TOKEN_SECRET
            //REFRESH_TOKEN_SECRET


            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private string GenerateRefreshToken()
        {
            var refreshToken = Guid.NewGuid().ToString();
            return refreshToken;
        }
    }
}
