using System.ComponentModel.DataAnnotations;

namespace PPMP.Models
{
    public class CreateProjectModel
    {
        [Required]
        public string ProjectName {get; set; }
        [Required]
        public string PrimaryGoal {get;set;}
        [Required]
        public  string Description {get; set;}
        [Required]
        public Guid CLientID {get; set; }
    }
}