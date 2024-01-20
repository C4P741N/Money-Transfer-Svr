using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using money_transfer_server_side.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace money_transfer_server_side.Utils
{
    public interface IJwtGenerator
    {
        //HttpResponseMessage AttachSuccessToken(UserLogin user, IConfiguration config);
        JwtModel CreateToken(UserLogin user);
        string AttachSuccessToken(UserLogin user);
    }
}