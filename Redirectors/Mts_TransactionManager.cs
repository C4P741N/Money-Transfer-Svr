using money_transfer_server_side.DataServer;
using money_transfer_server_side.EnumsFactory;
using money_transfer_server_side.JsonExtractors;
using money_transfer_server_side.Models;
using System.Net;

namespace money_transfer_server_side.Redirectors
{
    public class Mts_TransactionManager : IMts_TransactionManager
    {
        public TransactionsModel Begin(
            TransactionsModel transactions,
            IConfiguration config)
        {
            ITrans ts = new DataServerFactory().GetDataServer(EnumsAtLarge.Server.Mssql, config);

            switch (transactions.trasactionType)
            {
                case EnumsAtLarge.TransactionTypes.Withdraw:
                    transactions.StatusCode = ts.Withdraw(transactions);
                    return transactions;
                case EnumsAtLarge.TransactionTypes.Deposit:
                    transactions.StatusCode = ts.Deposit(transactions);
                    return transactions;
                case EnumsAtLarge.TransactionTypes.GetDashboardValues:
                    TransactionDetailsModel transactionDetails = new()
                    {
                        userId = transactions.userId,
                        trasactionType = transactions.trasactionType
                    };

                    transactionDetails.StatusCode = ts.GetUserStatements(transactionDetails);

                    return transactionDetails;
                case EnumsAtLarge.TransactionTypes.CreditTransfer:
                    transactions.StatusCode = ts.CreditTransfer(transactions);
                    return transactions;
                default:
                    throw new NotSupportedException($"Transaction type {transactions.trasactionType} is not supported.");
            }
        }
    }
}
