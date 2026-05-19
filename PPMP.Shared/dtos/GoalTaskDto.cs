//namespace PPMP.Shared.DTOs;

public class GoalTaskDto
{
    public Guid Id { get; set; }
    public Guid SubGoalId { get; set; }
    public string TaskGoal { get; set; } = string.Empty;
    public bool Completed { get; set; }
}