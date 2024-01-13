using Microsoft.AspNetCore.Mvc;
using money_transfer_server_side.DataServer;
using money_transfer_server_side.JsonExtractors;
using money_transfer_server_side.Models;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using static money_transfer_server_side.EnumsFactory.EnumsAtLarge;

namespace money_transfer_server_side.Controllers
{
    public class HomeController(
        IMts_AuthenticationManager authenticationManager) : Controller
    {
        private IMts_AuthenticationManager _authenticationManager = authenticationManager;

        [HttpPost]
        [Route("/[controller]/[action]")]
        public ActionResult Register([FromBody] string registrationDetails) =>
             ProcessRequest(registrationDetails, TrasactionTypes.Registration);

        [HttpPost]
        [Route("/[controller]/[action]")]
        public ActionResult Authenticate([FromBody] string auth) =>
             ProcessRequest(auth, TrasactionTypes.Authentication);        

        private ActionResult ProcessRequest(
            string jsonString,
            TrasactionTypes type)
        {
            UserDetailsModel detailsModel = JsonSerializer.Deserialize<Models.UserDetailsModel>(jsonString);

            if (detailsModel == null) return BadRequest();

            detailsModel.TrasactionType = type;

            return StatusCode((int)_authenticationManager.Begin(detailsModel));
        }
    }
}
