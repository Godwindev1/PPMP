using Microsoft.EntityFrameworkCore;
using PPMP.Data;

namespace PPMP.Repo
{
 

    public class ChatRoomMemberRepository 
    {
        private readonly UserDBContext _context;

        public ChatRoomMemberRepository(UserDBContext context)
        {
            _context = context;
        }

        public async Task<ChatRoomMember?> GetMemberAsync(Guid roomId, Guid userId, UserType userType)
        {
            return await _context.ChatRoomMembers
                .FirstOrDefaultAsync(m => m.RoomId == roomId && m.UserId == userId && m.UserType == userType);
        }

        public async Task<IEnumerable<ChatRoomMember>> GetMembersForRoomAsync(Guid roomId)
        {
            return await _context.ChatRoomMembers
                .Where(m => m.RoomId == roomId)
                .ToListAsync();
        }

        public async Task AddMemberAsync(ChatRoomMember member)
        {
            _context.ChatRoomMembers.Add(member);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveMemberAsync(Guid roomId, Guid userId, UserType userType)
        {
            var member = await GetMemberAsync(roomId, userId, userType);
            if (member != null)
            {
                _context.ChatRoomMembers.Remove(member);
                await _context.SaveChangesAsync();
            }
        }
    }
}