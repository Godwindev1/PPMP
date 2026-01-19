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
                ProgressRate = project.ProgressRate
            };
        }

        public List<ProjectDashboardViewModel.ProjectDto> ConvertToProjectDto(List<Project> projects)
        {
            return projects.Select(x => ConvertToProjectDto(x)).ToList();
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