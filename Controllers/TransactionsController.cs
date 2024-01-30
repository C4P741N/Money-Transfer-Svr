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
        public async Task<IActionResult> Deposit([FromBody] TransactionsModel model)
        {
            return await ProcessRequest(model);
        }

        [HttpPost("withdraw")]
        public async Task<IActionResult> Withdraw([FromBody] TransactionsModel model)
        {
            return await ProcessRequest(model);
        }

        [HttpPost("populate-dashboard")]
        public async Task<IActionResult> GetDashboardValues([FromBody] TransactionsModel model)
        {
            return await ProcessRequest(model);
        }

        [HttpPost("credit-transfer")]
        public async Task<IActionResult> CreditTransfer([FromBody] TransactionsModel model)
        {
            return await ProcessRequest(model);
        }

        private async Task<IActionResult> ProcessRequest(TransactionsModel transactions)
        {
            try
            {
                if (transactions.trasactionType is EnumsAtLarge.TransactionTypes.None) return BadRequest();

                if (string.IsNullOrEmpty(transactions.recepient)) transactions.recepient = "self";

                var model = await _transactionManager.Begin(transactions, _config);

                HttpStatusCode status = model.StatusCode;

                return new ObjectResult(status)
                {
                    StatusCode = (int)status,
                    Value = model
                };
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
