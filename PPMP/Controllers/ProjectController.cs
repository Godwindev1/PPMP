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

        [HttpPost("Delete/goal", Name ="DeleteSubgoal")]
        public async Task<IActionResult> DeleteSubGoal([FromBody]SubgoalDeleteModel deleteModel)
        {
            //Get Subgoal 
            var SubGoal = await _subgoalRepo.GetByIdAsync(deleteModel.SubgoalID);
            var TotalTasks = SubGoal.Tasks.Count;
            var CompletedTasks = SubGoal.Tasks.Select(x => x.Completed == true).Count();

            await UpdateCompletedTasks(SubGoal.ProjectID.ToString(), -1 * CompletedTasks);
            await UpdateProjectTotalTasks(SubGoal.ProjectID.ToString(), -1 * TotalTasks);

            await _subgoalRepo.DeleteAsync(SubGoal.ID);

            return new OkResult();
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
                Result.TotalNumberOfTasks =  Math.Max(0, Result.TotalNumberOfTasks);
                await _projectRepo.UpdateProject(Result);
            }
           
        }

        private async Task UpdateCompletedTasks(string projectId, int delta)
        {
            var Result =  await _projectRepo.GetProjectByIDWithoutNavigation(projectId);
           
            if(Result != null)
            {
                Result.TotalCompletedTasks += delta;
                Result.TotalCompletedTasks =  Math.Max(0, Result.TotalCompletedTasks);
                await _projectRepo.UpdateProject(Result);
            }
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

        [HttpPost("Delete/Task", Name = "DeleteTask")]
        public async Task<IActionResult> DeleteTask([FromBody]TaskDeleteModel taskDeleteModel)
        {
            var task = await _goalTaskRepo.GetByIdAsync(taskDeleteModel.TaskID); 
            var ProjectID = (await _subgoalRepo.GetByIdAsync(taskDeleteModel.SubgoalID)).ProjectID;

            await UpdateProjectTotalTasks(ProjectID.ToString(), -1);

            if(task.Completed == true)
            {
                await UpdateCompletedTasks(ProjectID.ToString(), -1);
            }

            await _goalTaskRepo.DeleteAsync(task.ID);

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
