using money_transfer_server_side.EnumsFactory;

namespace money_transfer_server_side.DataServer
{
    public class DataServerFactory
    {
        public IDataServer GetDataServer(EnumsAtLarge.Server server)
        {
            switch (server )
            {
                case EnumsAtLarge.Server.Mssql:
                    return new DS_MSqlServer();
                case EnumsAtLarge.Server.Mongo:
                    return new DS_MongoCollector();
                default:
                    throw new NotSupportedException($"Server type {server} is not supported.");
            }
        }
    }
}
