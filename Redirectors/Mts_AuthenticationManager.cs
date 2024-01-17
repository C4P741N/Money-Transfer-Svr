using money_transfer_server_side.DataServer;
using money_transfer_server_side.EnumsFactory;
using money_transfer_server_side.Models;
using money_transfer_server_side.Utils;
using System.Net;
using static money_transfer_server_side.EnumsFactory.EnumsAtLarge;

namespace money_transfer_server_side.JsonExtractors
{
    public class Mts_AuthenticationManager : IMts_AuthenticationManager
    {
        public HttpStatusCode Begin(
            UserLogin userDetails,
            IConfiguration config)
        {
            IAuth _ds = new DataServerFactory().GetDataServer(EnumsAtLarge.Server.Mssql, config);

            switch (userDetails.AuthType)
            {
                case AuthTypes.Registration:
                    return _ds.Register(userDetails);
                case AuthTypes.Authentication:
                    return _ds.Authenticate(userDetails);
                case AuthTypes.Unregister:
                    return _ds.Unregister(userDetails);
                default:
                    throw new NotSupportedException($"Authentication type {userDetails.AuthType} is not supported.");
            }
        }
    }
}
