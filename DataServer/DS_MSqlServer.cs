using money_transfer_server_side.Models;
using MongoDB.Driver;
using System.Data.SqlClient;
using System.Net;

namespace money_transfer_server_side.DataServer
{
    public class DS_MSqlServer : IDataServer
    {
        private readonly string _connectionString;
        public DS_MSqlServer()
        {
            _connectionString = WebApplication.CreateBuilder()
                .Configuration
                .GetConnectionString("MSSQL") ?? "wrong connection string assignment";

        }

        public HttpStatusCode Authenticate(UserDetailsModel userDetails)
        {
            if (CheckValueExists(userDetails.user, userDetails.pwd))
            {
                return HttpStatusCode.OK;
            }
            return HttpStatusCode.NotFound;
        }

        public HttpStatusCode Register(UserDetailsModel userDetails)
        {
            if(CheckValueExists(userDetails.user, userDetails.pwd))
            {
                return HttpStatusCode.Conflict;
            }

            BeginRegistration(userDetails.user, userDetails.pwd);

            return HttpStatusCode.OK;
        }

        public HttpStatusCode Unregister(UserDetailsModel userDetails)
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
