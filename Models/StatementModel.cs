using static money_transfer_server_side.EnumsFactory.EnumsAtLarge;

namespace money_transfer_server_side.Models
{
    public class StatementModel
    {
        public int Id { get; set; }
        public string TransactionId { get; set; }
        public double Amount { get; set; }
        public string Recepient { get; set; }
        public int TransactionStatus { get; set; }
        public string TransactionStatusString {
            get
            {
                switch (TransactionStatus)
                {
                    case (int)TransactionTypes.Deposit:
                        return "Deposit";
                    case (int)TransactionTypes.CreditTransfer:
                        return "Credit Transfer";
                    case (int)TransactionTypes.Withdraw:
                        return "Withdraw";
                    default:
                        return "Invalid";
                }
            }
            set
            {
                TransactionStatus = Convert.ToInt32(value);
            }
        }
        public DateTime TimeStamp { get; set; }
        public string ReadableTimeStamp {
            get
            {
                return TimeStamp.ToString("MMMM dd, yyyy HH:mm:ss");
            }
            set
            {
                TimeStamp =  Convert.ToDateTime(value);
            } 
        }
    }
}
