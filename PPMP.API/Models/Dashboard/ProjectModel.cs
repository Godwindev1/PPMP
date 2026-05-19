
using PPMP.API.Data;
using PPMP.API.Mapping;
using PPMP.API.Repo;
using PPMP.Shared.Models;

namespace PPMP.API.Models
{ 
    public class ProjectDashboardModel
    {
        private readonly ProjectRepo _projectRepo;
        public ProjectDashboardModel(ProjectRepo projectRepo)
        {
            _projectRepo = projectRepo;
        }


        public ProjectDashboardViewModel.ProjectDto ConvertToProjectDto(Project project)
        {
            

            return new ProjectDashboardViewModel.ProjectDto
            {
                ID = project.ID,
                Name = project.ProjectName,
                ClientName = project.client.Name,
                PrimaryGoal = project.PrimaryGoal,
                ProgressRate = project.ProgressRate,
                state = project.State.ToDto(),
                DateCreated = project.CreatedAt.LocalDateTime
            };
        }

        public projectViewModel ConvertToViewModel(Project project)
        {         
            var result = project.TotalNumberOfTasks == 0
                        ? 0
                        : (double)project.TotalCompletedTasks / project.TotalNumberOfTasks * 100;

            return new projectViewModel
            {
                ID = project.ID,
                Name = project.ProjectName,
                ClientName = project.client.Name,
                PrimaryGoal = project.PrimaryGoal,
                ProgressRate = (int)result,
                state = project.State.ToDto(),
                DateCreated = project.CreatedAt.LocalDateTime,
                Description = project.Description,
                subgoals = project.subgoals.ToDtos().ToList()
            };
        }


        public List<ProjectDashboardViewModel.ProjectDto> ConvertToProjectDto(List<Project> projects)
        {
            return projects.Select(x => ConvertToProjectDto(x)).ToList();
        }

        public async Task<projectViewModel?>GetProjectByID(string  ID)
        {
            var project = await _projectRepo.GetProjectByID(ID);

            if(project != null)
            {
                return ConvertToViewModel(project);               
            }

            return null;
        }

        public async Task<ProjectDashboardViewModel> GetDashboardData(User user)
        {
            var projects = await _projectRepo.GetProjectsByDeveloper(user);

            return new ProjectDashboardViewModel
            {
                projects = ConvertToProjectDto(projects)
            };
        }
    }
}