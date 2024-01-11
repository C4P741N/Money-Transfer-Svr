using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using static money_transfer_server_side.EnumsFactory.EnumsAtLarge;

namespace money_transfer_server_side.Controllers
{
    public class HomeController : Controller
    {
        [HttpPost]
        [Route("/[controller]/[action]")]
        public ActionResult Register([FromBody] string registrationDetails) =>
             ProcessRequest(registrationDetails, Mtss_TrasactionTypes.Registration);

        [HttpPost]
        [Route("/[controller]/[action]")]
        public ActionResult Authenticate([FromBody] string auth) =>
             ProcessRequest(auth, Mtss_TrasactionTypes.Authentication);        

        private ActionResult ProcessRequest(
            string jsonString,
            Mtss_TrasactionTypes type)
        {
            var test = JsonSerializer.Deserialize<UserDetails>(jsonString);

            return StatusCode((int)HttpStatusCode.Accepted);
        }
    }

    internal class UserDetails
    {
        public string user { get; set; }
        public string pwd { get; set; }
    }
}
