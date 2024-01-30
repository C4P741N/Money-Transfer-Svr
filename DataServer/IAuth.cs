using money_transfer_server_side.Models;
using System.Net;

namespace money_transfer_server_side.DataServer
{
    public interface IAuth: ITrans
    {
        Task<HttpStatusCode> Authenticate(UserLogin userDetails);
        Task<HttpStatusCode> Register(UserLogin userDetails);
        Task<HttpStatusCode> Unregister(UserLogin userDetails);
    }
}
