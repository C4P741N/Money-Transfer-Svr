namespace money_transfer_server_side.Models
{
    public class ContactModel
    {
        public int ID { get; set; }
        public string Recepient { get; set; }
        public double TotalSent { get; set; }
        public double TotalReceived { get; set; }
        public DateTime EarliestDate { get; set; }
        public string ReadableTimeStamp
        {
            get
            {
                return EarliestDate.ToString("MMMM dd, yyyy HH:mm:ss");
            }
            set
            {
                EarliestDate = Convert.ToDateTime(value);
            }
        }
    }
}
