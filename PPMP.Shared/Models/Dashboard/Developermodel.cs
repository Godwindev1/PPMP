using System.Threading.Tasks;
using PPMP.Data;
using PPMP.Repo;
using PPMP.Models;

namespace PPMP.Models
    { 
    public class DeveloperDashboardModel
    {
        private readonly ClientRepo _clientRepo;
        private readonly ProjectRepo _projectRepo;
        public DeveloperDashboardModel(ClientRepo clientRepo, ProjectRepo projectRepo)
        {
            _clientRepo = clientRepo;
            _projectRepo = projectRepo;
        }

        public MainDashboardViewModel.ClientsDto ConvertToClientDto(Client client)
        {
            return new MainDashboardViewModel.ClientsDto
            {
                ID = client.Id,
                Name = client.Name,
                AmountOfProjects = client.projects != null ? client.projects.Count : 0,
                ProjectsProgressRate = client.projects != null ? (float)client.projects.Average(x => x.ProgressRate) : 0.0f,
                Created = client.CreatedAt
            };
        }

        public List<MainDashboardViewModel.ClientsDto> ConvertListToClientsDto(List<Client> clients)
        {
            return clients.Select(x => ConvertToClientDto(x)).ToList();
        }

        public async Task<MainDashboardViewModel> GetDashboardData(User user)
        {
            var clients = await _clientRepo.GetAllClientsByDeveloper(user);
            var projects = await _projectRepo.GetProjectsByDeveloper(user);
            var NoNewProjects = await _projectRepo.GetNewProjects(user);
            var NoCompletedProjects = await _projectRepo.GetCompletedProjects(user);
            var clientSatisfactionRate = _clientRepo.GetClientSatisfactionRate();

            return new MainDashboardViewModel
            {
                Clients = ConvertListToClientsDto(clients),
                NoOfClients = clients.Count,
                NoOfProjects = projects.Count,
                NoOfNewProjects = NoNewProjects.Count,
                NoOfCompletedProjects = NoCompletedProjects.Count,
                ClientSatisfactionRate = clientSatisfactionRate
            };
        }
    }
}