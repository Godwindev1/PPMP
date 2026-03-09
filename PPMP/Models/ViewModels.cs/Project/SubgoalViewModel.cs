public class SubgoalViewModel
{
    required public Guid ProjectID {get; set;}
    required public string Goal { get; set; }
    required public DateTime DueDate {get; set; }
}

public class SubgoalDeleteModel
{
    required public Guid SubgoalID {get; set; }
}