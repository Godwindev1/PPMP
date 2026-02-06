using System.Threading.Tasks;
using PPMP.Data;
using PPMP.Repo;

namespace PPMP.Models
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
                state = project.State,
                DateCreated = project.CreatedAt.LocalDateTime
            };
        }

        public projectViewModel ConvertToViewModel(Project project)
        {
            return new projectViewModel
            {
                ID = project.ID,
                Name = project.ProjectName,
                ClientName = project.client.Name,
                PrimaryGoal = project.PrimaryGoal,
                ProgressRate = project.ProgressRate,
                state = project.State,
                DateCreated = project.CreatedAt.LocalDateTime,
                Description = project.Description,
                subgoals = project.subgoals
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