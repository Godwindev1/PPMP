namespace PPMP.Data
{
   public class Client
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? PasswordHash { get; set; }
    public bool? HasPassword { get; set; }

    // Track project/session dates
    public DateTimeOffset StartDate { get; set; }
    public string StartDateOriginalTimeZone { get; set; }  

    public DateTimeOffset EndDate { get; set; }
    public string EndDateOriginalTimeZone { get; set; }   

    public DateTimeOffset? LastAccessedAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    // Relationship
    public User User { get; set; }
    public string DeveloperLinkId { get; set; }

    public ClientRole clientRole {get; set; }
}

}
