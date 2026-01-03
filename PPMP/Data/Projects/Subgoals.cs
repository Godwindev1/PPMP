public class Subgoal
{
    required public Guid ID {get; set;} 
    required public Guid ProjectID {get; set;}
    required public string Goal { get; set; }

    //NAVIGATION 
    public Project? project {get; set;}
    public List<ProjectModification>? modifications { get; set; }
}