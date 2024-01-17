using System;
using System.Data.SqlClient;
using System.Net;
using Microsoft.Extensions.Configuration;
using money_transfer_server_side.Models;

namespace money_transfer_server_side.DataServer
{
    public class MsqlDataAdapter
    {
        private readonly string _connectionString;

        public MsqlDataAdapter(IConfiguration config)
        {
            _connectionString = config["ConnectionStrings:MSSQL"] ?? throw new ArgumentException("Invalid connection string");
        }

        private SqlConnection CreateConnection() => new(_connectionString);

        private SqlCommand CreateCommand(string query, SqlConnection connection) =>  new(query, connection);

        private int ExecuteNonQuery(SqlCommand command)
        {
            command.Connection.Open();
            int rowsAffected = command.ExecuteNonQuery();
            command.Connection.Close();
            return rowsAffected;
        }

        private double ExecuteScalar(SqlCommand command)
        {
            command.Connection.Open();
            double result = Convert.ToDouble(command.ExecuteScalar());
            command.Connection.Close();
            return result;
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
        public HttpStatusCode AddTransaction(TransactionsModel transactions)
        {
            string query = "INSERT INTO [dbo].[Transactions] ([UserDocEntry], [Amount]) VALUES (" +
                           "(SELECT [DocEntry] FROM [Money_Transfer].[dbo].[UserCreds] WHERE [UserId] = @userId), @amount)";

            using SqlConnection connection = CreateConnection();
            using SqlCommand command = CreateCommand(query, connection);

            command.Parameters.AddWithValue("@userId", transactions.userId);
            command.Parameters.AddWithValue("@amount", transactions.amount);

            return ExecuteNonQuery(command) > 0 ? GetSumAmount(transactions) : HttpStatusCode.InternalServerError;
        }
        public HttpStatusCode GetSumAmount(TransactionsModel transactions)
        {
            string query = " SELECT SUM([Amount]) FROM [Money_Transfer].[dbo].[Transactions] " +
                "WHERE [UserDocEntry] = " +
                "(SELECT [DocEntry] FROM [Money_Transfer].[dbo].[UserCreds] WHERE [UserId] = @userId)";

            using SqlConnection connection = CreateConnection();
            using SqlCommand command = CreateCommand(query, connection);

            command.Parameters.AddWithValue("@userId", transactions.userId);

            double newAmount = ExecuteScalar(command);

            if (newAmount >= 0)
            {
                transactions.amount = newAmount;
                return HttpStatusCode.Accepted;
            }

            return HttpStatusCode.InternalServerError;
        }

        public HttpStatusCode AddUser(string userId, string pwd)
        {
            if (CheckValueExists(userId, pwd) == HttpStatusCode.NotFound)
            {
                string query = "INSERT INTO [dbo].[UserCreds] ([UserId], [UserPwd]) VALUES (@userId, @pwd)";

                using SqlConnection connection = CreateConnection();
                using SqlCommand command = CreateCommand(query, connection);

                command.Parameters.AddWithValue("@userId", userId);
                command.Parameters.AddWithValue("@pwd", pwd);

                return ExecuteNonQuery(command) > 0 ? HttpStatusCode.Accepted : HttpStatusCode.InternalServerError;
            }

            return HttpStatusCode.Conflict;
        }

        public HttpStatusCode CheckValueExists(string userId, string pwd)
        {
            string query = "SELECT COUNT(*) FROM [dbo].[UserCreds] WHERE [UserId] = @userId AND [UserPwd] = @pwd";

            using SqlConnection connection = CreateConnection();
            using SqlCommand command = CreateCommand(query, connection);

            command.Parameters.AddWithValue("@userId", userId);
            command.Parameters.AddWithValue("@pwd", pwd);

            return ExecuteScalar(command) > 0 ? HttpStatusCode.Found : HttpStatusCode.NotFound;
        }
    }
}
