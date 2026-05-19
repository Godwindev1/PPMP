using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace PPMP.Data
{
public class Client
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? PasswordHash { get; set; }
    
    [Required]
    public bool HasPassword { get; set; }

    // Track project/session dates
    public DateTimeOffset StartDate { get; set; }
    public string? StartDateOriginalTimeZone { get; set; }  

    public DateTimeOffset EndDate { get; set; }
    public string? EndDateOriginalTimeZone { get; set; }   

    public DateTimeOffset? LastAccessedAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    // Relationship
    public User User { get; set; }
    public string DeveloperLinkId { get; set; }
    public ClientRole clientRole {get; set; }

    public List<Project>? projects {get; set; }
    public List<Comment>? Comments {get; set; }
    public SessionPage? Session {get; set; }

}

}
