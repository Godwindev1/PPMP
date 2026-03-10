using Microsoft.EntityFrameworkCore;
using PPMP.Data;

namespace PPMP.Repo
{
 
    public class MessageRepository
    {
        private readonly UserDBContext _context;

        public MessageRepository(UserDBContext context)
        {
            _context = context;
        }

        public async Task<Message?> GetByIdAsync(Guid messageId)
        {
            return await _context.Messages
                .Include(m => m.Room)
                .FirstOrDefaultAsync(m => m.Id == messageId);
        }

        public async Task<IEnumerable<Message>> GetMessagesForRoomAsync(Guid roomId, int limit = 50)
        {
            return await _context.Messages
                .Where(m => m.RoomId == roomId)
                .OrderByDescending(m => m.CreatedAt)
                .Take(limit)
                .ToListAsync();
        }

        public async Task<Message> CreateAsync(Message message)
        {
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
            return message;
        }

        public async Task UpdateAsync(Message message)
        {
            _context.Messages.Update(message);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(Guid messageId, Guid requestingUserId, UserType requestingUserType)
        {
            // Retrieve the message from the database
            var message = await _context.Messages.FindAsync(messageId);
            
            if (message == null)
            {
                // Message does not exist, nothing to delete
                return;
            }

            // Verify ownership
            if (message.SenderUserId != requestingUserId || message.SenderUserType != requestingUserType)
            {
                throw new UnauthorizedAccessException("You are not allowed to delete this message.");
            }

            // Remove the message and save changes
            _context.Messages.Remove(message);
            await _context.SaveChangesAsync();
        }
    }
}