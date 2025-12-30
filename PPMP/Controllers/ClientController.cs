using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PPMP.Controllers
{
    public class ClientController : Controller
    {
        [Route("ClientIndex")]
        public string Index()
        {
            return "Client User Access Succesful";
        }
    }
}
