
public class projectViewModel{
    public Guid ID;
    public string Name {get; set; }
    public string PrimaryGoal {get; set; }
    public string Description {get; set; }
    public string?  ClientName {get; set; }
    public StateTag state {get; set; }
    public int ProgressRate {get; set; } 
    public DateTime DateCreated {get; set; }

    public List<Subgoal> subgoals {get; set; }
}