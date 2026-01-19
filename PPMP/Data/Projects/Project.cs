using System.ComponentModel.DataAnnotations;
using PPMP.Data;

public class Project
{
    [Required]
    public Guid ID { get; set; }
    [Required]
    [MaxLength(20)]
    public required string ProjectName {get; set;}
    public string PrimaryGoal { get; set; }
    [Required]
    public string DeveloperID { get; set; }
    [Required]
    public Guid ClientID { get; set; }
    public Guid CurrentStateTagID { get; set; }
    public required string Description { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public string? CreatedAtOriginalTimeZone { get; set; }
    public int ProgressRate { get; set; }

    //navigation Properties
    public User? Developer { get; set; }
    public Client? client { get; set; }
    public StateTag? State { get; set; }
    public List<Comment>? Comments { get; set; }
    public List<Subgoal>? subgoals { get; set; }
    public List<ProjectModification>? projectModifications { get; set; }

}