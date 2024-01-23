using money_transfer_server_side.DataServer;
using money_transfer_server_side.EnumsFactory;
using money_transfer_server_side.Models;
using money_transfer_server_side.Utils;
using System.Net;
using static money_transfer_server_side.EnumsFactory.EnumsAtLarge;

namespace money_transfer_server_side.JsonExtractors
{
    public class UserRepository : IUserRepository
    {
        public HttpStatusCode Begin(
            UserLogin userDetails,
            IConfiguration config)
        {
            IAuth ds = new DataServerFactory().GetDataServer(EnumsAtLarge.Server.Mssql, config);

            switch (userDetails.authType)
            {
                case AuthTypes.Registration:
                    return ds.Register(userDetails);
                case AuthTypes.Authentication:
                    return ds.Authenticate(userDetails);
                case AuthTypes.Unregister:
                    return ds.Unregister(userDetails);
                default:
                    throw new NotSupportedException($"Authentication type {userDetails.authType} is not supported.");
            }
        }
    }
}
