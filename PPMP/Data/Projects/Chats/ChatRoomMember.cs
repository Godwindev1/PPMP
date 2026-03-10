public class ChatRoomMember
{
    public Guid RoomId { get; set; }

    public ChatRoom Room { get; set; } = null!;

    required public Guid UserId { get; set; }

    required public UserType UserType { get; set; }

    public DateTime JoinedAt { get; set; }

}