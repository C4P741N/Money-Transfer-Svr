﻿using money_transfer_server_side.Models;
using System.Net;

namespace money_transfer_server_side.Redirectors
{
    public interface IMts_TransactionManager
    {
        HttpStatusCode Begin(TransactionsModel transactions, IConfiguration config);
    }
}