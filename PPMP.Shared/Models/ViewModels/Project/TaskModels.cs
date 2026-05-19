public class TaskUpdateModel
{
    required public string TaskID {get; set;}
    required public bool Completed {get; set;}
    public string SubGoalID {get; set;}
}


public class TaskViewModel
{
    required public Guid SubgoalID {get; set;}
    required public Guid ProjectID {get; set;}
    required public string TaskGoal { get; set; }
}


public class TaskDeleteModel
{
    required public Guid TaskID {get; set;}
    required public Guid SubgoalID {get; set;}
}