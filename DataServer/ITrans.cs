using money_transfer_server_side.Models;
using System.Net;

namespace money_transfer_server_side.DataServer
{
    public interface ITrans
    {
        Task<HttpStatusCode> GetContacts(ContactsModel contacts);
        Task<HttpStatusCode> Withdraw(TransactionsModel transactions);
        Task<HttpStatusCode> Deposit(TransactionsModel transactions);
        //HttpStatusCode GetBalance(TransactionsModel transactions);
        Task<HttpStatusCode> CreditTransfer(TransactionsModel transactions);
        Task<HttpStatusCode> GetUserStatements(TransactionDetailsModel transactions);
    }
}
