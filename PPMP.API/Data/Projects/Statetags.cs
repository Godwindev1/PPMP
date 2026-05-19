using System.ComponentModel.DataAnnotations;

public class StateTag
{
    [Required]
    public Guid ID {get; set;}
    [Required]
    public string TagName {get; set;}
    [Required]
    public string? HexColor {get;set;}

    public List<Project>? projects {get; set; }
    public List<Subgoal>? subgoals {get; set; }
}