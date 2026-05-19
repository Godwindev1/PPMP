using PPMP.API.Data;
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
            DueDate = subgoal.DueDate
        };
    }

    public static IEnumerable<StateTagDto> ToDtos(this IEnumerable<StateTag> stateTags)
        => stateTags.Select(s => s.ToDto());

    public static IEnumerable<SubgoalDto> ToDtos(this IEnumerable<Subgoal> subgoals)
        => subgoals.Select(s => s.ToDto());
}