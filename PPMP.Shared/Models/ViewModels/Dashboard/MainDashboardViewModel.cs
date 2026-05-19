namespace PPMP.Models
{
    public class MainDashboardViewModel
    {
        public class ClientsDto
        {
            public Guid ID { get; set; }
            public string Name { get; set; }
            public int AmountOfProjects { get; set; }
            public float ProjectsProgressRate { get; set; }
            public DateTimeOffset Created { get; set; }
        }

        public List<ClientsDto> Clients { get; set; }
        public int NoOfClients { get; set; }
        public int NoOfProjects { get; set; }
        public int NoOfNewProjects { get; set; }
        public int NoOfCompletedProjects { get; set; }
        public float ClientSatisfactionRate { get; set; }

    }
}