using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PPMP.Data;

namespace PPMP.Repo
{
    public class StateTagRepo
    {
        private readonly UserDBContext _context;
        private readonly HttpContextAccessor _httpContext;

        public StateTagRepo(UserDBContext Contexts, IHttpContextAccessor httpContext)
        {
            _context = Contexts;
            _httpContext = (HttpContextAccessor)httpContext;
        }

        public async Task<StateTag?> Create(StateTag stateTag)
        {
            try
            {
                await _context.stateTags.AddAsync(stateTag);
                await _context.SaveChangesAsync();
                return stateTag;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> Delete(Guid id)
        {
            try
            {
                var tag = await _context.stateTags.FindAsync(id);
                if (tag != null)
                {
                    _context.stateTags.Remove(tag);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task AddDefaultStateTags()
        {
            var defaults = new List<(string Name, string Color)>
            {
                (StateTagEnum.REGISTERED, "#3498db"),   // Blue
                (StateTagEnum.INPROGRESS, "#f1c40f"),   // Yellow
                (StateTagEnum.COMPLETED, "#2ecc71"),    // Green
                (StateTagEnum.DELIVERED, "#9b59b6"),    // Purple
                (StateTagEnum.UNCOMPLETED, "#e74c3c")   // Red
            };

            foreach (var (name, color) in defaults)
            {
                if (!await _context.stateTags.AnyAsync(t => t.TagName == name))
                {
                    var newTag = new StateTag
                    {
                        ID = Guid.NewGuid(),
                        TagName = name,
                        HexColor = color
                    };
                    await Create(newTag);
                }
            }
            await _context.SaveChangesAsync();
        }
        public async Task<StateTag?> GetStateTagByName(string tagName)
        {
            try
            {
                return await _context.stateTags.FirstOrDefaultAsync(t => t.TagName == tagName);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> Delete(string tagName)
        {
            try
            {
                var tag = await _context.stateTags.FirstOrDefaultAsync(t => t.TagName == tagName);
                if (tag != null)
                {
                    _context.stateTags.Remove(tag);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
