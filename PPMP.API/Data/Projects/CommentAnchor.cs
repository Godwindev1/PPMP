public class CommentAnchor
{
    public Guid CommentID {get; set;}
    public Guid AnchorNodeID {get; set;}

    public AnchorNode? AnchorNode {get; set; }
}