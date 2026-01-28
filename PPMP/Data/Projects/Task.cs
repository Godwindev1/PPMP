public class Task
{
    public Guid ID {get; set; }

    public Guid SubGoalID { get; set; }
    public string TaskGoal {get; set; }

    public Subgoal subgoal {get; set;}
}