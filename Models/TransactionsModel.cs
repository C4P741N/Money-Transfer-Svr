using System.Net;
using System.Text.Json.Serialization;

namespace money_transfer_server_side.Models
{
    public class TransactionsModel: IStatusCodeModel
    {
        public string userId { get; set; } = string.Empty;
        public string recepient { get; set; }
        public double amount { get; set; }
        public EnumsFactory.EnumsAtLarge.TransactionTypes trasactionType { get; set; } = 
            EnumsFactory.EnumsAtLarge.TransactionTypes.None;

        [JsonIgnore]
        public HttpStatusCode StatusCode { get; set; }
    }
}
