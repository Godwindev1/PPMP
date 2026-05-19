namespace PPMP.Shared.Models
{
    public class ProjectDashboardViewModel
    {
        public struct ProjectDto
        {
            public Guid ID;
            public string Name {get; set; }
            public string PrimaryGoal {get; set; }
            public string?  ClientName {get; set; }
            public StateTagDto state {get; set; }
            public int ProgressRate {get; set; }

            public DateTime DateCreated {get; set; }

        } 

        public List<ProjectDto> projects {get; set;}

    }
}