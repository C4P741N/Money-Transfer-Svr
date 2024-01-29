using System;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Transactions;
using Microsoft.Extensions.Configuration;
using money_transfer_server_side.Models;
using Microsoft.AspNetCore.Http;

namespace money_transfer_server_side.DataServer
{
    public class MsqlDataAdapter(IConfiguration config)
    {
        private readonly string _connectionString = config["ConnectionStrings:MSSQL"] ?? throw new ArgumentException("Invalid connection string");

        private SqlConnection CreateConnection() => new(_connectionString);

        private SqlCommand CreateCommand(string query, SqlConnection connection) => new(query, connection);

        private int ExecuteNonQuery(SqlCommand command)
        {
            command.CommandType = CommandType.StoredProcedure;
            command.Connection.Open();
            int rowsAffected = command.ExecuteNonQuery();
            command.Connection.Close();
            return rowsAffected;
        }
        private double ExecuteScalar(SqlCommand command)
        {
            command.CommandType = CommandType.StoredProcedure;
            command.Connection.Open();
            double result = Convert.ToDouble(command.ExecuteScalar());
            command.Connection.Close();
            return result;
        }
        private string LoadStringData(SqlCommand command)
        {
            if (command == null) return null;

            using (command)
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Connection.Open();
                using var reader = command.ExecuteReader();
                return reader.Read() ? reader[0].ToString() : null;
            }
        }
        public HttpStatusCode AddWithdrawTransaction(TransactionsModel transactions)
        {
            transactions.amount = -transactions.amount;
            return AddTransaction(transactions);
        }

        public HttpStatusCode AddDepositTransaction(TransactionsModel transactions)
        {
            return AddTransaction(transactions);
        }
        private HttpStatusCode AddTransaction(TransactionsModel transactions)
        {
            using SqlConnection connection = CreateConnection();
            using SqlCommand command = CreateCommand("AddTransaction", connection);

            command.Parameters.AddWithValue("@userId", transactions.userId);
            command.Parameters.AddWithValue("@amount", transactions.amount);
            command.Parameters.AddWithValue("@timeStamp", DateTime.Now);
            command.Parameters.AddWithValue("@type", transactions.trasactionType);

            return ExecuteNonQuery(command) > 0 ? GetSumAmount(transactions) : HttpStatusCode.NotModified;
        }
        public HttpStatusCode GetSumAmount(TransactionsModel transactions)
        {
            using SqlConnection connection = CreateConnection();
            using SqlCommand command = CreateCommand("GetSumAmount", connection);

            command.Parameters.AddWithValue("@userId", transactions.userId);

            double newAmount = ExecuteScalar(command);

            if (newAmount >= 0)
            {
                transactions.amount = newAmount;
                return HttpStatusCode.Accepted;
            }

            return HttpStatusCode.InternalServerError;
        }

        public HttpStatusCode AddUser(UserLogin userDetails)
        {
            if (CheckUserExists(userDetails) == HttpStatusCode.NotFound)
            {
                using SqlConnection connection = CreateConnection();
                using SqlCommand command = CreateCommand("AddUser", connection);

                command.Parameters.AddWithValue("@userId", userDetails.user);
                command.Parameters.AddWithValue("@pwd", userDetails.pwd);
                command.Parameters.AddWithValue("@email", userDetails.email);

                return ExecuteNonQuery(command) > 0 ? HttpStatusCode.Accepted : HttpStatusCode.InternalServerError;
            }

            return HttpStatusCode.Conflict;
        }
        public HttpStatusCode CheckUserExists(UserLogin userDetails)
        {
            using SqlConnection connection = CreateConnection();
            using SqlCommand command = CreateCommand("CheckValueExists", connection);

            command.Parameters.AddWithValue("@email", userDetails.email);
            command.Parameters.AddWithValue("@pwd", userDetails.pwd);

            return ExecuteScalar(command) > 0 ? HttpStatusCode.Found : HttpStatusCode.NotFound;
        }
        public HttpStatusCode ValidateUser(UserLogin userDetails)
        {
            if (CheckUserExists(userDetails) == HttpStatusCode.Found)
            {
                using SqlConnection connection = CreateConnection();
                using SqlCommand command = CreateCommand("GetUserName", connection);

                command.Parameters.AddWithValue("@email", userDetails.email);

                string userId = LoadStringData(command);

                if (!string.IsNullOrEmpty(userId))
                {
                    userDetails.user = userId;
                    return HttpStatusCode.Found;
                }
            }

            return HttpStatusCode.NotFound;
        }
        public HttpStatusCode GetTransactionStatements(TransactionDetailsModel transactions)
        {
            try
            {
                using SqlConnection connection = CreateConnection();
                using SqlCommand command = CreateCommand("GetAccountValues", connection);

                command.Parameters.AddWithValue("@userId", transactions.userId);

                transactions = LoadToTransactionDetailsModel(command, transactions);

                string queryString = $"EXEC [dbo].[GetStatements] {transactions.userId}";

                transactions.Statements = LoadBatchData<StatementModel>(queryString);

                return HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                return HttpStatusCode.InternalServerError;
            }
        }
        private TransactionDetailsModel LoadToTransactionDetailsModel(
            SqlCommand command,
            TransactionDetailsModel transactions)
        {
            if (command != null)
            {

                using (command)
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection.Open();
                    using var reader = command.ExecuteReader();
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
