using Microsoft.EntityFrameworkCore;
using PPMP.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPMP.Repo
{
    public class ChatRoomRepository
    {
        private readonly UserDBContext _context;

        public ChatRoomRepository(UserDBContext context)
        {
            _context = context;
        }

        public async Task<ChatRoom?> GetByIdAsync(Guid roomId)
        {
            // Include members and messages
            return await _context.ChatRooms
                .Include(r => r.Members)
                .Include(r => r.Messages)
                .FirstOrDefaultAsync(r => r.Id == roomId);
        }

        public async Task<IEnumerable<ChatRoom>> GetAllAsync()
        {
            return await _context.ChatRooms
                .Include(r => r.Members)
                .ToListAsync();
        }

        public async Task<ChatRoom> CreateAsync(ChatRoom room)
        {
            _context.ChatRooms.Add(room);
            await _context.SaveChangesAsync();
            return room;
        }

        public async Task AddMemberAsync(ChatRoomMember member)
        {
            _context.ChatRoomMembers.Add(member);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveMemberAsync(Guid roomId, Guid userId, UserType userType)
        {
            var member = await _context.ChatRoomMembers
                .FirstOrDefaultAsync(m => m.RoomId == roomId && m.UserId == userId && m.UserType == userType);

            if (member != null)
            {
                _context.ChatRoomMembers.Remove(member);
                await _context.SaveChangesAsync();
            }
        }
    }
}