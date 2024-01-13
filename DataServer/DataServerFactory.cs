using money_transfer_server_side.EnumsFactory;
using money_transfer_server_side.Utils;

namespace money_transfer_server_side.DataServer
{
    public class DataServerFactory
    {
        public IDataServer GetDataServer(
            EnumsAtLarge.Server server,
            IConfiguration config)
        {
            switch (server )
            {
                case EnumsAtLarge.Server.Mssql:
                    return new DS_MSqlServer(config);
                case EnumsAtLarge.Server.Mongo:
                    return new DS_MongoCollector(config);
                default:
                    throw new NotSupportedException($"Server type {server} is not supported.");
            }
        }
    }
}
