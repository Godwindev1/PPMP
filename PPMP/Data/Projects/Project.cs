using System.ComponentModel.DataAnnotations;
using PPMP.Data;

public class Project
{
    [Required]
    public Guid ID {get; set; }
    [Required]
    public string PrimaryGoal {get; set; }
    [Required]
    public string DeveloperID {get; set; }
    [Required]
    public Guid ClientID {get; set; }
    public Guid CurrentStateTagID {get; set; }
    public required string Description {get; set; }

    //navigation Properties
    public User? Developer {get; set; }
    public Client? client {get; set; }
    public StateTag? State {get; set;}    
    public List<Comment> ?Comments {get; set;}
    public List<Subgoal> ?subgoals {get; set; }
    public List<ProjectModification> ?projectModifications {get; set; }

}