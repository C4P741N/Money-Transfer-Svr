using System;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Transactions;
using Microsoft.Extensions.Configuration;
using money_transfer_server_side.Models;
using Microsoft.AspNetCore.Http;
using money_transfer_server_side.EnumsFactory;

namespace money_transfer_server_side.DataServer
{
    public class MsqlDataAdapter(IConfiguration config)
    {
        private readonly string _connectionString = config["ConnectionStrings:MSSQL"] ?? throw new ArgumentException("Invalid connection string");

        private async Task<SqlConnection> CreateOpenConnectionAsync()
        {
            SqlConnection connection = new(_connectionString);
            await connection.OpenAsync();
            return connection;
        }
        private SqlCommand CreateStoredProcedureCommand(string query, SqlConnection connection)
        {
            //new(query, connection);

            return new SqlCommand(query, connection)
            {
                CommandType = CommandType.StoredProcedure
            };
        }
        
        private async Task<int> ExecuteNonQueryAsync(SqlCommand command)
        {
            //command.CommandType = CommandType.StoredProcedure;
            //command.Connection.Open();
            //int rowsAffected = command.ExecuteNonQuery();
            //command.Connection.Close();
            //return rowsAffected;

            using (command)
            {
                return await command.ExecuteNonQueryAsync();
            }
        }
        private async Task<double> ExecuteScalarAsync(SqlCommand command)
        {
            //command.CommandType = CommandType.StoredProcedure;
            //command.Connection.Open();
            //double result = Convert.ToDouble(command.ExecuteScalar());
            //command.Connection.Close();
            //return result;

            using (command)
            {
                object result = await command.ExecuteScalarAsync() ?? 0;
                return Convert.ToDouble(result);
            }
        }
        private async Task<string> LoadStringDataAsync(SqlCommand command)
        {
            //if (command == null) return null;

            //using (command)
            //{
            //    command.CommandType = CommandType.StoredProcedure;
            //    command.Connection.Open();
            //    using var reader = command.ExecuteReader();
            //    return reader.Read() ? reader[0].ToString() : null;
            //}

            using (command)
            {
                using var reader = await command.ExecuteReaderAsync();
                return reader.Read() ? reader[0].ToString() : null;
            }
        }
        public async Task<HttpStatusCode> AddTransferFunds(TransactionsModel transactions)
        {
            return await AddWithdrawTransaction(transactions);
        }
        public async Task<HttpStatusCode> AddWithdrawTransaction(TransactionsModel transactions)
        {
            if (await GetSumAmount(transactions) < transactions.amount)
            {
                return HttpStatusCode.BadRequest;
            }

            transactions.amount = -transactions.amount;
            return await AddTransaction(transactions);
        }

        public async Task<HttpStatusCode> AddDepositTransaction(TransactionsModel transactions)
        {
            return await AddTransaction(transactions);
        }
        private async Task<HttpStatusCode> UpdateAcounts(TransactionsModel transactions)
        {
            using SqlConnection connection = await CreateOpenConnectionAsync();
            using SqlCommand command = CreateStoredProcedureCommand("UpdateAccounts", connection);

            command.Parameters.AddWithValue("@userId", transactions.userId);
            command.Parameters.AddWithValue("@deposit", (int)EnumsAtLarge.TransactionTypes.Deposit);
            command.Parameters.AddWithValue("@transfer", (int)EnumsAtLarge.TransactionTypes.CreditTransfer);
            command.Parameters.AddWithValue("@withdraw", (int)EnumsAtLarge.TransactionTypes.Withdraw);

            return await ExecuteNonQueryAsync(command) > 0 ? HttpStatusCode.Accepted : HttpStatusCode.NotModified;
        }
        private async Task<HttpStatusCode> AddTransaction(TransactionsModel transactions)
        {
            using SqlConnection connection = await CreateOpenConnectionAsync();
            using SqlCommand command = CreateStoredProcedureCommand("AddTransaction", connection);

            command.Parameters.AddWithValue("@userId", transactions.userId);
            command.Parameters.AddWithValue("@amount", transactions.amount);
            command.Parameters.AddWithValue("@timeStamp", DateTime.Now);
            command.Parameters.AddWithValue("@type", transactions.trasactionType);
            command.Parameters.AddWithValue("@recepient", transactions.recepient);

            return await ExecuteNonQueryAsync(command) > 0 ? await UpdateAcounts(transactions) : HttpStatusCode.NotModified;
        }
        public async Task<double> GetSumAmount(TransactionsModel transactions)
        {
            using SqlConnection connection = await CreateOpenConnectionAsync();
            using SqlCommand command = CreateStoredProcedureCommand("GetSumAmount", connection);

            command.Parameters.AddWithValue("@userId", transactions.userId);

            return await ExecuteScalarAsync(command);
        }

        public async Task<HttpStatusCode> AddUser(UserLogin userDetails)
        {
            if (await CheckUserExists(userDetails) is HttpStatusCode.NotFound)
            {
                using SqlConnection connection = await CreateOpenConnectionAsync();
                using SqlCommand command = CreateStoredProcedureCommand("AddUser", connection);

                command.Parameters.AddWithValue("@userId", userDetails.user);
                command.Parameters.AddWithValue("@pwd", userDetails.pwd);
                command.Parameters.AddWithValue("@email", userDetails.email);

                return await ExecuteNonQueryAsync(command) > 0 ? HttpStatusCode.Accepted : HttpStatusCode.InternalServerError;
            }

            return HttpStatusCode.Conflict;
        }
        public async Task<HttpStatusCode> CheckUserExists(UserLogin userDetails)
        {
            using SqlConnection connection = await CreateOpenConnectionAsync();
            using SqlCommand command = CreateStoredProcedureCommand("CheckValueExists", connection);

            command.Parameters.AddWithValue("@email", userDetails.email);
            command.Parameters.AddWithValue("@pwd", userDetails.pwd);

            return await ExecuteScalarAsync(command) > 0 ? HttpStatusCode.Found : HttpStatusCode.NotFound;
        }
        public async Task<HttpStatusCode> ValidateUser(UserLogin userDetails)
        {
            if (await CheckUserExists(userDetails) is HttpStatusCode.Found)
            {
                using SqlConnection connection = await CreateOpenConnectionAsync();
                using SqlCommand command = CreateStoredProcedureCommand("GetUserName", connection);

                command.Parameters.AddWithValue("@email", userDetails.email);

                string userId = await LoadStringDataAsync(command);

                if (!string.IsNullOrEmpty(userId))
                {
                    userDetails.user = userId;
                    return HttpStatusCode.Found;
                }
            }

            return HttpStatusCode.NotFound;
        }
        public async Task<HttpStatusCode> GetTransactionStatements(TransactionDetailsModel transactions)
        {
            try
            {
                using SqlConnection connection = await CreateOpenConnectionAsync();
                using SqlCommand command = CreateStoredProcedureCommand("GetAccountValues", connection);

                command.Parameters.AddWithValue("@userId", transactions.userId);

                transactions = await LoadToTransactionDetailsModel(command, transactions);

                string queryString = $"EXEC [dbo].[GetStatements] {transactions.userId}";

                transactions.Statements = LoadBatchData<StatementModel>(queryString);

                return HttpStatusCode.OK;
            }
            catch (Exception)
            {
                return HttpStatusCode.InternalServerError;
            }
        }
        private async Task<TransactionDetailsModel> LoadToTransactionDetailsModel(
            SqlCommand command,
            TransactionDetailsModel transactions)
        {
            if (command != null)
            {
                using (command)
                {
                    //command.CommandType = CommandType.StoredProcedure;
                    //command.Connection.Open();
                    using var reader = await command.ExecuteReaderAsync();
                    if (reader.Read()) // Check if there are rows
                    {
                        transactions.Balance = Convert.ToDouble(reader["Sum_Balance"]);
                        transactions.AmountReceived = Convert.ToDouble(reader["Sum_Received"]);
                        transactions.AmountSent = Convert.ToDouble(reader["Sum_Sent"]);
                    }
                }
            }

            return transactions;
        }
        public List<T> LoadBatchData<T>(string sql)
        {
            using IDbConnection IDbCn = new SqlConnection(_connectionString);
            return IDbCn.Query<T>(sql).ToList();
        }
    }
}
