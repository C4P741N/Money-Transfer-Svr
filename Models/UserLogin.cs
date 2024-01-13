namespace money_transfer_server_side.Models
{
    public class UserLogin : TransactionTypeModel
    {
        public string user { get; set; }
        public string pwd { get; set; }
    }
}
