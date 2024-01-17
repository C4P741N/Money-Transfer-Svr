namespace money_transfer_server_side.Models
{
    public class UserLogin
    {
        public string user { get; set; }
        public string pwd { get; set; }
        public EnumsFactory.EnumsAtLarge.AuthTypes AuthType { get; set; }

    }
}
