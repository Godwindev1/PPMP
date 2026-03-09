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

        private readonly GoalTaskRepo _goalTaskRepo;

        private readonly StateTagRepo _stateTagRepo;

        private readonly ProjectRepo _projectRepo;
        public ProjectController(ProjectRepo projectRepo, SubgoalRepo subgoalRepo, GoalTaskRepo goalTaskRepo, StateTagRepo repo)
        {
            _projectRepo = projectRepo;
            _goalTaskRepo = goalTaskRepo;
            _stateTagRepo = repo;
            _subgoalRepo = subgoalRepo;
            _ProjectDashboardModel = new ProjectDashboardModel(projectRepo);
        }


        [HttpGet("dashboard/{ID:guid}")]
        public async Task<IActionResult> Project([FromRoute] string ID)
        {
            var Project = await _ProjectDashboardModel.GetProjectByID(ID);
            return View("ProjectView", Project);
        }

        [HttpPost("Add/goal", Name = "SubgoalCreate")]
        public async Task<IActionResult> AddSubGoal(SubgoalViewModel subgoalViewModel)
        {
            try
            {
                Subgoal goal = new Subgoal
                {
                    DueDate = subgoalViewModel.DueDate,
                    Goal = subgoalViewModel.Goal,
                    ProjectID = subgoalViewModel.ProjectID,
                    ID = new Guid(),
                    stateTagID = (await _stateTagRepo.GetStateTagByName(StateTagEnum.PENDING)).ID
                };

                await _subgoalRepo.CreateAsync(goal);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }


            return new OkResult();

        }

        private async Task UpdateProjectTotalTasks(string projectId, int delta)
        {
           var Result =  await _projectRepo.GetProjectByIDWithoutNavigation(projectId);
           
            if(Result != null)
            {
                Result.TotalNumberOfTasks += delta;
                
                //REMOVE EVERYTHING CONCERNING PROGRESS RATE FROM DB (IT Will Now Be Calculated withing Project Model From both Set of totals)
               /* Result.ProgressRate =   Result.TotalNumberOfTasks == 0
                                        ? 0
                                        : Result.TotalCompletedTasks / Result.TotalNumberOfTasks * 100; */

                await _projectRepo.UpdateProject(Result);
            }
           
        }

        private async Task UpdateCompletedTasks(string projectId, int delta)
        {
            var Result =  await _projectRepo.GetProjectByIDWithoutNavigation(projectId);
           
            if(Result != null)
            {
                Result.TotalCompletedTasks += delta;
                await _projectRepo.UpdateProject(Result);
            }
        }

        [HttpPost("Complete/Task", Name = "CompleteTask")]
        public async Task CompleteTask(string TaskID, TaskViewModel taskView)
        {
            await _goalTaskRepo.UpdateAsync(new GoalTask { ID = new Guid(TaskID), Completed = true});
            await UpdateCompletedTasks(taskView.ProjectID.ToString(), +1);
        }

        [HttpPost("InComplete/Task", Name = "InCompleteTask")]
        public async Task InCompleteTask(string TaskID, TaskViewModel taskView)
        {
            await _goalTaskRepo.UpdateAsync(new GoalTask { ID = new Guid(TaskID), Completed = false});
            await UpdateCompletedTasks(taskView.ProjectID.ToString(), -1);
        }

        [HttpPost("Add/Task", Name = "AddTasksToGoal")]
        public async Task<IActionResult> AddTaskToGoal(TaskViewModel TaskView)
        {
            try
            {
                GoalTask Task = new GoalTask
                {
                    TaskGoal = TaskView.TaskGoal,
                    SubGoalID = TaskView.SubgoalID,
                    ID = Guid.CreateVersion7(),
                    Completed = false
                };

                var res = await _goalTaskRepo.CreateAsync(Task);
                
                if(res != null)
                {
                    await UpdateProjectTotalTasks(TaskView.ProjectID.ToString(), +1);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }


            return new OkResult();

        }

        [HttpPost("Update/Task", Name = "UpdateTask")]
        public async Task<IActionResult> UpdateTask([FromBody]TaskUpdateModel taskUpdate)
        {
            var result = await _goalTaskRepo.GetByIdAsync(new Guid(taskUpdate.TaskID));
            if(result.Completed == taskUpdate.Completed)
            {
                return new BadRequestObjectResult("Unecessary Call Setting Task State To it Current State"); 
            }

            result.Completed = taskUpdate.Completed;
            await _goalTaskRepo.UpdateAsync(result);

            string ProjectID = (await _subgoalRepo.GetByIdAsync(new Guid(taskUpdate.SubGoalID))).ProjectID.ToString();

            if(taskUpdate.Completed == true)
            {
                await UpdateCompletedTasks(ProjectID, +1);
            }
            else
            {
                await UpdateCompletedTasks(ProjectID, -1);
            }

            return new OkResult(); 
        }


    }
}
