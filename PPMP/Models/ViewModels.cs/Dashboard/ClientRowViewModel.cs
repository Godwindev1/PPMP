public class ClientRowViewModel
{
    public Guid Id { get; set; }
    public string ClientName { get; set; }
    public int ProjectsCount { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public string Status { get; set; }
}
