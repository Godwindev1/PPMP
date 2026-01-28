using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PPMP.Data;
using PPMP.Models;
using PPMP.Repo;

namespace PPMP.Controllers
{
    [Route("Project")]
    public class ProjectController : Controller
    {
        private readonly ProjectDashboardModel _ProjectDashboardModel;
        public ProjectController(  ProjectRepo projectRepo)
        {
            _ProjectDashboardModel = new ProjectDashboardModel(projectRepo);
          
        }


        [Authorize(policy: "FullAccessPolicy")]
        [HttpGet("dashboard/{ID:guid}")]
        public async Task<IActionResult> Project([FromRoute]string ID)
        {
            var Project =  await _ProjectDashboardModel.GetProjectByID(ID) ; 
            return View("ProjectView", Project);
        }

    }
}
