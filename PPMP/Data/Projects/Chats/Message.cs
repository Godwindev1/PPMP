public class Message
{
    public Guid Id { get; set; }

    public Guid RoomId { get; set; }

    public ChatRoom Room { get; set; } = null!;

    required public Guid SenderUserId { get; set; }

    required public UserType SenderUserType { get; set; }

    public string Content { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? EditedAt { get; set; }

    public DateTime? DeletedAt { get; set; }
}