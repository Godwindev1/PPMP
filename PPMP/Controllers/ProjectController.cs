using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PPMP.Data;
using PPMP.Models;
using PPMP.Repo;

namespace PPMP.Controllers
{
    [Authorize(policy: "FullAccessPolicy")]
    [Route("Project")]
    public class ProjectController : Controller
    {
        private readonly ProjectDashboardModel _ProjectDashboardModel;
        private readonly SubgoalRepo _subgoalRepo;

        private readonly StateTagRepo _stateTagRepo;
        public ProjectController(  ProjectRepo projectRepo, SubgoalRepo subgoalRepo, StateTagRepo repo)
        {
            _stateTagRepo = repo;
            _subgoalRepo = subgoalRepo;
            _ProjectDashboardModel = new ProjectDashboardModel(projectRepo);
        }


        [HttpGet("dashboard/{ID:guid}")]
        public async Task<IActionResult> Project([FromRoute]string ID)
        {
            var Project =  await _ProjectDashboardModel.GetProjectByID(ID) ; 
            return View("ProjectView", Project);
        }

        [HttpPost("Add/goal", Name = "SubgoalCreate")]
        public async Task<IActionResult> AddSubGoal(SubgoalViewModel subgoalViewModel)
        {
            try
            {
                Subgoal goal = new Subgoal { 
                    Goal = subgoalViewModel.Goal,
                    ProjectID = subgoalViewModel.ProjectID,
                    ID = new Guid(),
                    stateTagID = (await _stateTagRepo.GetStateTagByName(StateTagEnum.PENDING)).ID
                };

                await _subgoalRepo.CreateAsync(goal);
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
           

            return new OkResult();

        }


    }
}
