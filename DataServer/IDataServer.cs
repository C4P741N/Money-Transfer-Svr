using money_transfer_server_side.Models;
using System.Net;

namespace money_transfer_server_side.DataServer
{
    public interface IDataServer
    {
        HttpResponseMessage Authenticate(UserLogin userDetails);
        HttpResponseMessage Register(UserLogin userDetails);
        HttpResponseMessage Unregister(UserLogin userDetails);
    }
}
