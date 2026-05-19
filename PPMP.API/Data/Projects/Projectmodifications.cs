using System.ComponentModel.DataAnnotations;

public class ProjectModification
{
    [Required]
    public Guid ID {get; set;}
     [Required]
    public Guid ProjectID {get; set;}
     [Required]
    public Guid SubGoalAnchorID {get; set;}
    public string? Goal { get; set;  }
    public string? ModDescription {get; set;}

    //NAVIGATION
    public Project? project {get; set;}
    public Subgoal? subgoal {get; set;} 
}