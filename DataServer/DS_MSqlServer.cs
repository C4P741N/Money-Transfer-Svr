using money_transfer_server_side.Models;
using System.Net;

namespace money_transfer_server_side.DataServer
{
    public class DS_MSqlServer(IConfiguration config) : IAuth
    {
        private readonly MsqlDataAdapter _msqlDataAdapter = new(config);

        public HttpStatusCode Withdraw(TransactionsModel transactions) =>
            _msqlDataAdapter.AddWithdrawTransaction(transactions);
        
        public HttpStatusCode GetBalance(TransactionsModel transactions) =>
            _msqlDataAdapter.GetSumAmount(transactions);
        
        public HttpStatusCode Deposit(TransactionsModel transactions) =>
            _msqlDataAdapter.AddDepositTransaction(transactions);
        
        public HttpStatusCode Authenticate(UserLogin userDetails) =>
            _msqlDataAdapter.ValidateUser(userDetails);
        
        public HttpStatusCode Register(UserLogin userDetails) =>
            _msqlDataAdapter.AddUser(userDetails);
        
        public HttpStatusCode Unregister(UserLogin userDetails) =>
            HttpStatusCode.NotImplemented;
        
        public HttpStatusCode CreditTransfer(TransactionsModel transactions) =>
            throw new NotImplementedException();
        public HttpStatusCode GetUserStatements(TransactionDetailsModel transactions) =>
            _msqlDataAdapter.GetTransactionStatements(transactions);
    }
}
