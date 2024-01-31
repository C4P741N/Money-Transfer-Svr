using money_transfer_server_side.Models;
using System.Net;

namespace money_transfer_server_side.DataServer
{
    public class DS_MSqlServer(IConfiguration config) : IAuth
    {
        private readonly MsqlDataAdapter _msqlDataAdapter = new(config);
        public async Task<HttpStatusCode> GetContacts(ContactsModel contacts)
        {
            return await _msqlDataAdapter.GetContacts(contacts);
        }
        public async Task<HttpStatusCode> Withdraw(TransactionsModel transactions)
        {
            return await _msqlDataAdapter.AddWithdrawTransaction(transactions); ;
        }

        //public HttpStatusCode GetBalance(TransactionsModel transactions) =>
        //    _msqlDataAdapter.GetSumAmount(transactions);

        public async Task<HttpStatusCode> Deposit(TransactionsModel transactions)
        {
            return await _msqlDataAdapter.AddDepositTransaction(transactions); ;
        }

        public async Task<HttpStatusCode> Authenticate(UserLogin userDetails)
        {
            return await _msqlDataAdapter.ValidateUser(userDetails);
        }

        public async Task<HttpStatusCode> Register(UserLogin userDetails)
        {
           return await _msqlDataAdapter.AddUser(userDetails);
        }
        
        public async Task<HttpStatusCode> Unregister(UserLogin userDetails) =>
            HttpStatusCode.NotImplemented;

        public async Task<HttpStatusCode> CreditTransfer(TransactionsModel transactions)
        {
            return await _msqlDataAdapter.AddTransferFunds(transactions);
        }
        public async Task<HttpStatusCode> GetUserStatements(TransactionDetailsModel transactions)
        {
            return await _msqlDataAdapter.GetTransactionStatements(transactions);
        }
    }
}
