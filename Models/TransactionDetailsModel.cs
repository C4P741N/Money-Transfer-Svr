namespace money_transfer_server_side.Models
{
    public class TransactionDetailsModel : TransactionsModel
    {
        public double Balance { get; set; }
        public double AmountSent { get; set; }
        public double AmountReceived { get; set; }
        public List<StatementModel> Statements { get; set; }
    }
}
