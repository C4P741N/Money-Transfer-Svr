namespace money_transfer_server_side.EnumsFactory
{
    public class EnumsAtLarge
    {
        public enum AuthTypes
        {
            None = 0,
            Registration = 1,
            Authentication = 2,
            Unregister = 3
        }
        public enum Server
        {
            None = 0,
            Mssql,
            Mongo
        }
        public enum TransactionTypes
        {
            None = 0,
            Withdraw,
            Deposit,
            CheckBalance,
            CreditTransfer
        }
    }
}
