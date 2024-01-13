using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using money_transfer_server_side.Models;
using money_transfer_server_side.Utils;
using MongoDB.Driver;
using System.Data.SqlClient;
using System.Net;

namespace money_transfer_server_side.DataServer
{
    public class DS_MSqlServer(
        IConfiguration config) : IDataServer
    {
        private readonly string _connectionString = config["ConnectionStrings:MSSQL"] ?? "wrong connection string assignment";
        public HttpResponseMessage Authenticate(UserLogin userDetails)
        {
            if (CheckValueExists(userDetails.user, userDetails.pwd))
            {
                return new(HttpStatusCode.Found);
            }

            return new(HttpStatusCode.NotFound);
        }

        public HttpResponseMessage Register(UserLogin userDetails)
        {
            if(CheckValueExists(userDetails.user, userDetails.pwd))
            {
                return new(HttpStatusCode.Conflict);
            }

            BeginRegistration(userDetails.user, userDetails.pwd);

            return new(HttpStatusCode.Accepted);
        }

        public HttpResponseMessage Unregister(UserLogin userDetails)
        {
            throw new NotImplementedException();
        }

        private void BeginRegistration(
            string userId,
            string pwd)
        {
            string query = "INSERT INTO [dbo].[UserCred] ([UserId] ,[UserPwd]) VALUES (@userId ,@pwd)";


            using SqlDataAdapter oAdap = new ();
            using SqlConnection oCon = new(_connectionString);
            using SqlCommand oCmd = new(query, oCon);
            oCmd.Parameters.AddWithValue("@userId", userId);
            oCmd.Parameters.AddWithValue("@pwd", pwd);

            oAdap.InsertCommand = oCmd;

            oCon.Open();
            oCmd.ExecuteNonQuery();
        }
        private bool CheckValueExists(
            string userId, 
            string pwd)
        {
            string query = "SELECT COUNT(*) FROM [dbo].[UserCred] WHERE [UserId] = @userId AND [UserPwd] = @pwd";

            using SqlConnection oCon = new(_connectionString);
            using SqlCommand oCmd = new(query, oCon);
            oCmd.Parameters.AddWithValue("@userId", userId);
            oCmd.Parameters.AddWithValue("@pwd", pwd);

            oCon.Open();

            int count = Convert.ToInt32(oCmd.ExecuteScalar());

            return count > 0;
        }

    }
}
