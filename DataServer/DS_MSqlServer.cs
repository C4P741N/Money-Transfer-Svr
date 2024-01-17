using money_transfer_server_side.Models;
using System.Net;

namespace money_transfer_server_side.DataServer
{
    public class DS_MSqlServer(IConfiguration config) : IAuth
    {
        private readonly MsqlDataAdapter _msqlDataAdapter = new(config);

        public HttpStatusCode Withdraw(TransactionsModel transactions)
        {
            return _msqlDataAdapter.AddWithdrawTransaction(transactions);
        }
        public HttpStatusCode GetBalance(TransactionsModel transactions)
        {
            return _msqlDataAdapter.GetSumAmount(transactions);
        }
        public HttpStatusCode Deposit(TransactionsModel transactions)
        {
            return _msqlDataAdapter.AddDepositTransaction(transactions);
        }
        public HttpStatusCode Authenticate(UserLogin userDetails)
        {
            return _msqlDataAdapter.CheckValueExists(userDetails.user, userDetails.pwd);
        }
        public HttpStatusCode Register(UserLogin userDetails)
        {
            return _msqlDataAdapter.AddUser(userDetails.user, userDetails.pwd);
        }

        public HttpStatusCode Unregister(UserLogin userDetails)
        {
            return HttpStatusCode.NotImplemented;
        }
    }
}
