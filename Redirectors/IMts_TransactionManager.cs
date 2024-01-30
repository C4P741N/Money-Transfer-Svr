using Microsoft.AspNetCore.Mvc;
using money_transfer_server_side.Models;
using System.Net;

namespace money_transfer_server_side.Redirectors
{
    public interface IMts_TransactionManager
    {
        Task<TransactionsModel> Begin(TransactionsModel transactions, IConfiguration config);
    }
}