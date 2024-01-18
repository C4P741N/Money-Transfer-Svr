using System;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using Microsoft.Extensions.Configuration;
using money_transfer_server_side.Models;

namespace money_transfer_server_side.DataServer
{
    public class MsqlDataAdapter(IConfiguration config)
    {
        private readonly string _connectionString = config["ConnectionStrings:MSSQL"] ?? throw new ArgumentException("Invalid connection string");

        private SqlConnection CreateConnection() => new(_connectionString);

        private SqlCommand CreateCommand(string query, SqlConnection connection) =>  new(query, connection);

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

        public HttpStatusCode AddUser(string userId, string pwd)
        {
            if (CheckValueExists(userId, pwd) == HttpStatusCode.NotFound)
            {
                using SqlConnection connection = CreateConnection();
                using SqlCommand command = CreateCommand("AddUser", connection);

                command.Parameters.AddWithValue("@userId", userId);
                command.Parameters.AddWithValue("@pwd", pwd);

                return ExecuteNonQuery(command) > 0 ? HttpStatusCode.Accepted : HttpStatusCode.InternalServerError;
            }

            return HttpStatusCode.Conflict;
        }
        public HttpStatusCode CheckValueExists(string userId, string pwd)
        {
            using SqlConnection connection = CreateConnection();
            using SqlCommand command = CreateCommand("CheckValueExists", connection);

            command.Parameters.AddWithValue("@userId", userId);
            command.Parameters.AddWithValue("@pwd", pwd);

            return ExecuteScalar(command) > 0 ? HttpStatusCode.Found : HttpStatusCode.NotFound;
        }
    }
}
