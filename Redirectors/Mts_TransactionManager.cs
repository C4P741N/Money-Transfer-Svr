using money_transfer_server_side.DataServer;
using money_transfer_server_side.EnumsFactory;
using money_transfer_server_side.JsonExtractors;
using money_transfer_server_side.Models;
using System.Net;

namespace money_transfer_server_side.Redirectors
{
    public class Mts_TransactionManager : IMts_TransactionManager
    {
        public HttpStatusCode Begin(
            TransactionsModel transactions,
            IConfiguration config)
        {
            ITrans ts = new DataServerFactory().GetDataServer(EnumsAtLarge.Server.Mssql, config);

            switch (transactions.trasactionType)
            {
                case EnumsAtLarge.TransactionTypes.Withdraw:
                    return ts.Withdraw(transactions);
                case EnumsAtLarge.TransactionTypes.Deposit:
                    return ts.Deposit(transactions);
                case EnumsAtLarge.TransactionTypes.GetDashboardValues:
                    return ts.GetUserStatements((TransactionDetailsModel)transactions);
                case EnumsAtLarge.TransactionTypes.CreditTransfer:
                    return ts.CreditTransfer(transactions);
                default:
                    throw new NotSupportedException($"Transaction type {transactions.trasactionType} is not supported.");
            }
        }
    }
}
