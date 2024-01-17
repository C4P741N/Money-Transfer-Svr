using money_transfer_server_side.Models;
using money_transfer_server_side.Utils;
using System.Net;

namespace money_transfer_server_side.JsonExtractors
{
    public interface IMts_AuthenticationManager
    {
        HttpStatusCode Begin(UserLogin userDetails, IConfiguration config);
    }
}