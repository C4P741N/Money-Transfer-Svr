using Microsoft.AspNetCore.Authentication;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace money_transfer_server_side.Models
{
    public class JwtModel
    {
        public JwtSecurityToken jwtSecurityToken { get; set; } = new();
        public string cookieAuth { get; set; } = "";
        public ClaimsPrincipal claimsPrincipal { get; set; } = new();
        public AuthenticationProperties authenticationProperties { get; set; } = new();
    }
}
