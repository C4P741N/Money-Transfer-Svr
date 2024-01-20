using System.Text.Json.Serialization;

namespace money_transfer_server_side.Models
{
    public class UserLogin
    {
        public string user { get; set; } = string.Empty;
        public string pwd { get; set; } = string.Empty;
        public EnumsFactory.EnumsAtLarge.AuthTypes authType { get; set; } 
            = EnumsFactory.EnumsAtLarge.AuthTypes.None;

    }
}
