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
        public IActionResult Deposit([FromBody] TransactionsModel model) => ProcessRequest(model);

        [HttpPost("withdraw")]
        public IActionResult Withdraw([FromBody] TransactionsModel model) => ProcessRequest(model);
        
        [HttpPost("get-balance")]
        public IActionResult GetBalance([FromBody] TransactionsModel model) => ProcessRequest(model);

        [HttpPost("credit-transfer")]
        public IActionResult CreditTransfer([FromBody] TransactionsModel model) => ProcessRequest(model);

        private IActionResult ProcessRequest(TransactionsModel transactions)
        {
            try
            {
                if (transactions.trasactionType is EnumsAtLarge.TransactionTypes.None) return BadRequest();

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
