namespace money_transfer_server_side.EnumsFactory
{
    public class EnumsAtLarge
    {
        public enum AuthTypes
        {
            Registration,
            Authentication,
            Unregister
        }
        public enum Server
        {
            Mssql,
            Mongo
        }
        public enum TransactionTypes
        {
            Withdraw,
            Deposit,
            CheckBalance
        }
    }
}
