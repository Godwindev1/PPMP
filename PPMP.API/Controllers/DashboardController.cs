using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PPMP.Data;
using PPMP.Models;
using PPMP.Repo;

namespace PPMP.Controllers
{
    [Route("Dashboard")]
    public class DashboardController : Controller
    {
        private readonly DeveloperDashboardModel _developerModel;
        private readonly ProjectDashboardModel _ProjectDashboardModel;
        private readonly UserManager<User> _manager;
        private readonly ClientRepo _CLientManager;
        public DashboardController( ClientRepo clientRepo, ProjectRepo projectRepo, UserManager<User> manager)
        {
            _ProjectDashboardModel = new ProjectDashboardModel(projectRepo);
            _developerModel = new DeveloperDashboardModel(clientRepo, projectRepo);
            _manager = manager;
            _CLientManager = clientRepo;
        }

        [HttpGet("/")]
        public async Task<IActionResult> Dashboard()
        {
            if(User != null && User.Identity.IsAuthenticated)
            {
                if(User.IsInRole("CLIENT"))
                {
                    return LocalRedirect("/Dashboard/Client");
                }
                else if(User.IsInRole("DEVELOPER"))
                {
                    return LocalRedirect("/Dashboard/Developer");
                }
                else
                {
                    return LocalRedirect("/Account/Login");
                }

            }

            return LocalRedirect("/Account/Login");
        }

        [Authorize(policy: "FullAccessPolicy")]
        [HttpGet("Developer")]
        public async Task<IActionResult> Developer()
        {
            var user = await _manager.GetUserAsync(User);
            var mainDashboard = await _developerModel.GetDashboardData(user);
            var ProjectDashboard = await _ProjectDashboardModel.GetDashboardData(user);

            DashboardViewModel dashboardViewModel  = new DashboardViewModel
            {
                mainDashboard = mainDashboard,
                projectDashboardView = ProjectDashboard
            };

            return View("Developer/Dashboard", dashboardViewModel);
        }


        [Authorize(policy: "ClientAccessPolicy")]
        [HttpGet("Client")]
        public IActionResult Client()
        {
            var CLient = User;
            return View();
        }

    }
}
