using System.ComponentModel.DataAnnotations;
using PPMP.Data;

public class Comment
{
    [Required]
    public Guid CommentID {get; set;}
    [Required]
    public Guid ProjectID {get; set;}
    public Guid? AuthorClientID {get; set;}
    public Guid? AuthorDeveloperID {get; set;}
    [Required]
    public string Body {get; set;}
    [Required]
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    //Navigations
    public Project? project {get; set; }
    public Client? client {get; set; }
    public User? Developer {get; set;}
}