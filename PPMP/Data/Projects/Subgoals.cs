public class Subgoal
{
    required public Guid ID {get; set;} 
    required public Guid ProjectID {get; set;}
    required public string Goal { get; set; }
    public Guid stateTagID {get; set; }

    //NAVIGATION 
    public StateTag state {get; set; }
    public Project? project {get; set;}
    public List<ProjectModification>? modifications { get; set; }
    public List<GoalTask>? Tasks {get; set; }
}