using money_transfer_server_side.Models;
using System.Net;

namespace money_transfer_server_side.JsonExtractors
{
    public interface IMts_AuthenticationManager
    {
        HttpStatusCode Begin(UserDetailsModel userDetails);
    }
}