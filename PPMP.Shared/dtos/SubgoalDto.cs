//namespace PPMP.Shared.DTOs;

public class SubgoalDto
{
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }
    public string Goal { get; set; } = string.Empty;
    public Guid StateTagId { get; set; }
    public string? StateTagName { get; set; }
    public string? StateTagHexColor { get; set; }
    public DateTimeOffset DueDate { get; set; }
    public List<GoalTaskDto> Tasks { get; set; } = [];
    public List<ProjectModificationDto> Modifications { get; set; } = [];
}