using money_transfer_server_side.DataServer;
using money_transfer_server_side.EnumsFactory;
using money_transfer_server_side.Models;
using System.Net;
using static money_transfer_server_side.EnumsFactory.EnumsAtLarge;

namespace money_transfer_server_side.JsonExtractors
{
    public class Mts_AuthenticationManager : IMts_AuthenticationManager
    {
        public HttpStatusCode Begin(UserDetailsModel userDetails)
        {
            IDataServer _ds = new DataServerFactory().GetDataServer(EnumsAtLarge.Server.Mssql);

            switch (userDetails.TrasactionType)
            {
                case TrasactionTypes.Registration:
                    return _ds.Register(userDetails);
                case TrasactionTypes.Authentication:
                    return _ds.Authenticate(userDetails);
                case TrasactionTypes.Unregister:
                    return _ds.Unregister(userDetails);
                default:
                    throw new NotSupportedException($"Transaction type {userDetails.TrasactionType} is not supported.");
            }
        }
    }
}
