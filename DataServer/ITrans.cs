using money_transfer_server_side.Models;
using System.Net;

namespace money_transfer_server_side.DataServer
{
    public interface ITrans
    {
        HttpStatusCode Withdraw(TransactionsModel transactions);
        HttpStatusCode Deposit(TransactionsModel transactions);
        //HttpStatusCode GetBalance(TransactionsModel transactions);
        HttpStatusCode CreditTransfer(TransactionsModel transactions);
        HttpStatusCode GetUserStatements(TransactionDetailsModel transactions);
    }
}
