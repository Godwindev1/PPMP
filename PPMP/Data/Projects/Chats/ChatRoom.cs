public class ChatRoom
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    required public Guid CreatedByUserId { get; set; }

    required public UserType CreatedByUserType { get; set; }

    public ICollection<ChatRoomMember> Members { get; set; } = new List<ChatRoomMember>();

    public ICollection<Message> Messages { get; set; } = new List<Message>();
}