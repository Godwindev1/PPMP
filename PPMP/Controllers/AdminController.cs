using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PPMP.Data;
using PPMP.Models;
using PPMP.Repo;
using System.Net;
using System.Security.Claims;

namespace PPMP.Controllers
{
    [Authorize(policy: "FullAccessPolicy")]
    public class AdminController : Controller
    {
        private readonly ClientRepo _clientManager;
        private readonly UserManager<User> _userManager;
        public AdminController(ClientRepo clientRepo,  UserManager<User> userManager)
        {
            _clientManager = clientRepo;
            _userManager = userManager;
        }

        [HttpGet("Admin")]
        public async Task<IActionResult> Admin()
        {
            return View("/Views/Pages/admin.cshtml");
        }

        [HttpPost("Admin/AddClient")]
        public async Task<IActionResult> CreateUser([FromForm]ClientViewModel clientDto)
        {
            Client client = new Client{ 
                Name = clientDto.Name,
                StartDate = clientDto.StartDate,
                EndDate = clientDto.EndDate,
            };

            var Claim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);
            User? Developer =  await _userManager.FindByEmailAsync(Claim.Value);

            if(Developer != null)
            {
                await _clientManager.CreateAsync(client, Developer);
            }

            return Redirect("~/");
        }
    }
}