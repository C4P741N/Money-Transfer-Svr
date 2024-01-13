using money_transfer_server_side.Models;
using System.Net;

namespace money_transfer_server_side.DataServer
{
    public interface IDataServer
    {
        HttpStatusCode Authenticate(UserDetailsModel userDetails);
        HttpStatusCode Register(UserDetailsModel userDetails);
        HttpStatusCode Unregister(UserDetailsModel userDetails);
    }
}
