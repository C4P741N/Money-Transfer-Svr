using Microsoft.AspNetCore.Mvc;
using money_transfer_server_side.Models;

namespace money_transfer_server_side.Utils
{
    public interface IJwtGenerator
    {
        //HttpResponseMessage AttachSuccessToken(UserLogin user, IConfiguration config);
        string AttachSuccessToken(UserLogin user, IConfiguration config);
    }
}