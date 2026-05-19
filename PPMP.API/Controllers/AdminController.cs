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
        private readonly StateTagRepo _stateTagRepo;
        private readonly ProjectRepo _projectRepo;
        public AdminController(ClientRepo clientRepo, UserManager<User> userManager, RoleManager<Role> roleManager, StateTagRepo stateTagRepo, ProjectRepo projectRepo)
        {
            _clientManager = clientRepo;
            _userManager = userManager;
            _roleManger = roleManager;
            _stateTagRepo = stateTagRepo;
            _projectRepo = projectRepo;
        }

        [HttpPost("CreateClient")]
        public async Task<IActionResult> CreateUser([FromForm] CreateClientModel ClientModel)
        {
            var Role = await _roleManger.FindByNameAsync("Client");

            if (Role == null)
            {
                await _roleManger.CreateAsync(new Role { Name = "Client", NormalizedName = "CLIENT" });

                Role = await _roleManger.FindByNameAsync("Client");

                if (Role != null)
                {
                    await _roleManger.AddClaimAsync(Role, new Claim(ClaimTypes.Role, Role.NormalizedName ?? "Client"));
                }
            }

            Client client = new Client
            {
                Name = ClientModel.Name,
                StartDate = ClientModel.StartDate,
                EndDate = ClientModel.EndDate,
                HasPassword = false
            };

            var Claim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);
            User? Developer = await _userManager.FindByEmailAsync(Claim.Value);

            if (Developer != null)
            {
                await _clientManager.CreateAsync(client, Developer);
                await _clientManager.AddToRoleAsync(client, "Client");
            }

            return Redirect("~/");
        }


        [HttpPost("CreateProject")]
        public async Task<IActionResult> CreateProject([FromForm] CreateProjectModel ClientModel)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);

                Project project = new Project
                {
                    ProjectName = ClientModel.ProjectName,
                    PrimaryGoal = ClientModel.PrimaryGoal,
                    Description = ClientModel.Description,
                    DeveloperID = user.Id,
                    ClientID = ClientModel.CLientID,
                    CurrentStateTagID = (await _stateTagRepo.GetStateTagByName(StateTagEnum.REGISTERED)).ID,
                    CreatedAt = DateTimeOffset.UtcNow,
                    CreatedAtOriginalTimeZone = TimeZoneInfo.Local.ToSerializedString(),
                    ProgressRate = 0
                };

                await _projectRepo.Create(project, user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return new OkResult();
        }

    }
}