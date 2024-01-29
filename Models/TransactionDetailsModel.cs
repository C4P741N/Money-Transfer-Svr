using System.Net;
using System.Text.Json.Serialization;

namespace money_transfer_server_side.Models
{
    public class TransactionDetailsModel : TransactionsModel, IStatusCodeModel
    {
        public double Balance { get; set; }
        public double AmountSent { get; set; }
        public double AmountReceived { get; set; }
        public List<StatementModel> Statements { get; set; }
        [JsonIgnore]
        public HttpStatusCode StatusCode { get; set; }
    }
}
