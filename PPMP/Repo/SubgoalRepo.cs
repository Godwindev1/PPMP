using Microsoft.EntityFrameworkCore;
using PPMP.Data;

namespace PPMP.Repo
{
    public class SubgoalRepo
    {
        private readonly UserDBContext _context;

        public SubgoalRepo(UserDBContext context)
        {
            _context = context;
        }

        // Get by ID
        public async Task<Subgoal?> GetByIdAsync(Guid id)
        {
            return await _context.subgoals
                .Include(x => x.Tasks)
                .Include(x => x.modifications)
                .FirstOrDefaultAsync(x => x.ID == id);
        }

        // Get all subgoals for a project
        public async Task<List<Subgoal>> GetByProjectAsync(Guid projectId)
        {
            return await _context.subgoals
                .Where(x => x.ProjectID == projectId)
                .ToListAsync();
        }

        // Create
        public async Task<Subgoal> CreateAsync(Subgoal subgoal)
        {
            subgoal.ID = Guid.NewGuid();

            await _context.subgoals.AddAsync(subgoal);
            await _context.SaveChangesAsync();

            return subgoal;
        }

        // Update
        public async Task<bool> UpdateAsync(Subgoal subgoal)
        {
            var exists = await _context.subgoals.AnyAsync(x => x.ID == subgoal.ID);
            if (!exists) return false;

            _context.subgoals.Update(subgoal);
            await _context.SaveChangesAsync();

            return true;
        }

        // Delete
        public async Task<bool> DeleteAsync(Guid id)
        {
            var subgoal = await _context.subgoals.FirstOrDefaultAsync(x => x.ID == id);
            if (subgoal == null) return false;

            _context.subgoals.Remove(subgoal);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
