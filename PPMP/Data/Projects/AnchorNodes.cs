using System.ComponentModel.DataAnnotations;

//TODO REVISE COMMENTS, CHATS AAND ANCHORS
public class AnchorNode
{
    [Required]
    public Guid AnchorNodeID {get; set;}
    public Guid? SubGoalID {get; set; }
    public Guid? CommentID {get; set; }  

    //NAVIGATION
    public Subgoal? subgoal { get; set; }
    public Comment? comment { get; set; }
}