using money_transfer_server_side.Models;
using System.Net;

namespace money_transfer_server_side.DataServer
{
    public interface IAuth: ITrans
    {
        HttpStatusCode Authenticate(UserLogin userDetails);
        HttpStatusCode Register(UserLogin userDetails);
        HttpStatusCode Unregister(UserLogin userDetails);
    }
}
