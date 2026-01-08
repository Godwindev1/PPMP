using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PPMP.Models;

namespace PPMP.Controllers
{
    [Route("Dashboard")]
    public class DashboardController : Controller
    {
        public DashboardController()
        {
        }

        [Authorize(policy: "FullAccessPolicy")]
        [HttpGet("Developer")]
        public IActionResult Developer()
        {
            return View();
        }

        
        [Authorize(policy: "ClientAccessPolicy")]
        [HttpGet("Client")]
        public IActionResult Client()
        {
            return View();
        }

    }
}
