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
    public class TransactionsController(
        IMts_TransactionManager transactionManager,
        IConfiguration config) : Controller
    {
        private readonly IConfiguration _config = config;
        private readonly IMts_TransactionManager _transactionManager = transactionManager;

        [HttpPost]
        [Route("/[controller]/[action]")]
        public IActionResult Deposit([FromBody] string deposit)
        {
            return ProcessRequest(deposit, EnumsAtLarge.TransactionTypes.Deposit);
        }
        [HttpPost]
        [Route("/[controller]/[action]")]
        public IActionResult Withdraw([FromBody] string withdraw)
        {
            return ProcessRequest(withdraw, EnumsAtLarge.TransactionTypes.Withdraw);
        }
        [HttpPost]
        [Route("/[controller]/[action]")]
        public IActionResult CheckBalance([FromBody] string checkBalance)
        {
            return ProcessRequest(checkBalance, EnumsAtLarge.TransactionTypes.CheckBalance);
        }

        private IActionResult ProcessRequest(
            string request,
            EnumsAtLarge.TransactionTypes transactionTypes)
        {
            try
            {
                if (request == null) return BadRequest();

                TransactionsModel transactions = JsonSerializer.Deserialize<TransactionsModel>(request);

                if (transactions == null) return BadRequest();

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
