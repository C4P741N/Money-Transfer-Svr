using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.HttpSys;
using money_transfer_server_side.EnumsFactory;
using money_transfer_server_side.Models;
using money_transfer_server_side.Redirectors;
using money_transfer_server_side.Utils;
using MongoDB.Bson;
using System.Net;
using System.Text.Json;

namespace money_transfer_server_side.Controllers
{
    [Authorize]
    [ApiController]
    [Route("transactions")]
    public class TransactionsController(
        IMts_TransactionManager transactionManager,
        IConfiguration config) : Controller
    {
        private readonly IConfiguration _config = config;
        private readonly IMts_TransactionManager _transactionManager = transactionManager;

        [HttpPost("deposit")]
        public IActionResult Deposit([FromBody] string deposit)
        {
            return ProcessRequest(deposit, EnumsAtLarge.TransactionTypes.Deposit);
        }
        [HttpPost("withdraw")]
        public IActionResult Withdraw([FromBody] string withdraw)
        {
            return ProcessRequest(withdraw, EnumsAtLarge.TransactionTypes.Withdraw);
        }
        [HttpPost("get-balance")]
        public IActionResult GetBalance([FromBody] string getBalance)
        {
            return ProcessRequest(getBalance, EnumsAtLarge.TransactionTypes.CheckBalance);
        }

        private IActionResult ProcessRequest(
            string request,
            EnumsAtLarge.TransactionTypes transactionTypes)
        {
            try
            {
                if (string.IsNullOrEmpty(request)) return BadRequest();

                TransactionsModel transactions = JsonSerializer.Deserialize<TransactionsModel>(request);

                if (transactions is null) return BadRequest();

                transactions.TrasactionType = transactionTypes;

                HttpStatusCode status = _transactionManager.Begin(transactions, _config);

                return new ObjectResult(status)
                {
                    StatusCode = (int)status,
                    Value = transactions
                };
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
