namespace money_transfer_server_side.Models
{
    public class StatementModel
    {
        public string TransactionId { get; set; }
        public double Amount { get; set; }
        public string Recepient { get; set; }
        public string TransactionStatus { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
