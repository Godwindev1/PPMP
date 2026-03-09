public class TaskUpdateModel
{
    required public string TaskID {get; set;}
    required public bool Completed {get; set;}
    public string SubGoalID {get; set;}
}