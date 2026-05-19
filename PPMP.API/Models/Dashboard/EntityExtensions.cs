using PPMP.API.Data;

using PPMP.Shared;

namespace PPMP.API.Mapping;

public static class EntityExtensions
{
    public static StateTagDto ToDto(this StateTag stateTag)
    {
        return new StateTagDto
        {
            Id = stateTag.ID,
            TagName = stateTag.TagName,
            HexColor = stateTag.HexColor
        };
    }

    public static GoalTaskDto ToDto(this GoalTask task)
    {
        return new GoalTaskDto
        {
            Id = task.ID,
            SubGoalId = task.SubGoalID,
            TaskGoal = task.TaskGoal,
            Completed = task.Completed
        };
    }

    public static ProjectModificationDto ToDto(this ProjectModification mod)
    {
        return new ProjectModificationDto
        {
            Id = mod.ID,
            ProjectId = mod.ProjectID,
            SubGoalAnchorId = mod.SubGoalAnchorID,
            Goal = mod.Goal,
            ModDescription = mod.ModDescription
        };
    }

    public static SubgoalDto ToDto(this Subgoal subgoal)
    {
        return new SubgoalDto
        {
            Id = subgoal.ID,
            ProjectId = subgoal.ProjectID,
            Goal = subgoal.Goal,
            StateTagId = subgoal.stateTagID,
            StateTagName = subgoal.state?.TagName,
            StateTagHexColor = subgoal.state?.HexColor,
            DueDate = subgoal.DueDate,
            Tasks = subgoal.Tasks?.Select(t => t.ToDto()).ToList() ?? [],
            Modifications = subgoal.modifications?.Select(m => m.ToDto()).ToList() ?? []
        };
    }

    public static IEnumerable<StateTagDto> ToDtos(this IEnumerable<StateTag> stateTags)
        => stateTags.Select(s => s.ToDto());

    public static IEnumerable<SubgoalDto> ToDtos(this IEnumerable<Subgoal> subgoals)
        => subgoals.Select(s => s.ToDto());

    public static IEnumerable<GoalTaskDto> ToDtos(this IEnumerable<GoalTask> tasks)
        => tasks.Select(t => t.ToDto());

    public static IEnumerable<ProjectModificationDto> ToDtos(this IEnumerable<ProjectModification> mods)
        => mods.Select(m => m.ToDto());
}