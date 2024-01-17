using System.Text.Json.Serialization;

namespace money_transfer_server_side.Models
{
    public class TransactionsModel
    {
        public string userId { get; set; } = string.Empty;
        public double amount { get; set; }

        [JsonIgnore]
        public EnumsFactory.EnumsAtLarge.TransactionTypes TrasactionType { get; set; }
    }
}
