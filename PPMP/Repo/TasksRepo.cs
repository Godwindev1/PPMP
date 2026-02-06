using Microsoft.EntityFrameworkCore;
using PPMP.Data;

namespace PPMP.Repo
{
    public class GoalTaskRepo
    {
        private readonly UserDBContext _context;

        public GoalTaskRepo(UserDBContext context)
        {
            _context = context;
        }

        // Get by ID
        public async Task<GoalTask?> GetByIdAsync(Guid id)
        {
            return await _context.tasks
                .Include(x => x.subgoal)
                .FirstOrDefaultAsync(x => x.ID == id);
        }

        // Get all tasks for a subgoal
        public async Task<List<GoalTask>> GetBySubgoalAsync(Guid subgoalId)
        {
            return await _context.tasks
                .Where(x => x.SubGoalID == subgoalId)
                .ToListAsync();
        }

        // Create
        public async Task<GoalTask> CreateAsync(GoalTask task)
        {
            task.ID = Guid.NewGuid();

            await _context.tasks.AddAsync(task);
            await _context.SaveChangesAsync();

            return task;
        }

        // Update
        public async Task<bool> UpdateAsync(GoalTask task)
        {
            var exists = await _context.tasks.AnyAsync(x => x.ID == task.ID);
            if (!exists) return false;

            _context.tasks.Update(task);
            await _context.SaveChangesAsync();

            return true;
        }

        // Delete
        public async Task<bool> DeleteAsync(Guid id)
        {
            var task = await _context.tasks.FirstOrDefaultAsync(x => x.ID == id);
            if (task == null) return false;

            _context.tasks.Remove(task);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
