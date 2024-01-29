using System.Net;

namespace money_transfer_server_side.Models
{
    public interface IStatusCodeModel
    {
        HttpStatusCode StatusCode { get; set; }
    }
}