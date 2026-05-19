

public class ProjectModificationDto
{
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }
    public Guid SubGoalAnchorId { get; set; }
    public string? Goal { get; set; }
    public string? ModDescription { get; set; }
}