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
    //Changed Routing For Admin  controller CHeck For Linkage Errors In Pages
    [Authorize(policy: "FullAccessPolicy")]
    [Route("Admin")]
    public class AdminController : Controller
    {
        private readonly ClientRepo _clientManager;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManger;
        public AdminController(ClientRepo clientRepo,  UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _clientManager = clientRepo;
            _userManager = userManager;
            _roleManger = roleManager;
        }

        [HttpGet("Create")]
        public async Task<IActionResult> Admin()
        {
            return View("/Views/admin/admin.cshtml");
        }

        [HttpPost("AddClient")]
        public async Task<IActionResult> CreateUser([FromForm]ClientViewModel clientDto)
        {
            var Role = await _roleManger.FindByNameAsync("Client");

            if(Role == null)
            {
                await _roleManger.CreateAsync(new Role { Name = "Client", NormalizedName = "CLIENT" });

                Role = await _roleManger.FindByNameAsync("Client");

                if(Role != null)
                {
                    await _roleManger.AddClaimAsync(Role, new Claim(ClaimTypes.Role, Role.NormalizedName ?? "Client"));
                }
            }

            Client client = new Client{ 
                Name = clientDto.Name,
                StartDate = clientDto.StartDate,
                EndDate = clientDto.EndDate,
                HasPassword = false
            };

            var Claim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);
            User? Developer =  await _userManager.FindByEmailAsync(Claim.Value);

            if(Developer != null)
            {
                await _clientManager.CreateAsync(client, Developer);
                await _clientManager.AddToRoleAsync(client, "Client");
            }

            return Redirect("~/");
        }
    }
}