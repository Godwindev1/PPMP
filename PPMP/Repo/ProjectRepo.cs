using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PPMP.Data;

namespace PPMP.Repo
{
    public class ProjectRepo
    {
        private readonly UserDBContext _context;
        private readonly HttpContextAccessor _httpContext;

        public ProjectRepo(UserDBContext Contexts, IHttpContextAccessor httpContext)
        {
            _context = Contexts;
            _httpContext = (HttpContextAccessor)httpContext;
        }

        public async Task<Project?> Create(Project project, User Developer)
        {
            if(project == null || Developer == null)
            {
                return null;
            }

            project.ID = Guid.NewGuid();
            project.DeveloperID = Developer.Id;
            //project.CurrentStateTagID 

            await _context.projects.AddAsync(project);
            await _context.SaveChangesAsync();

            return project;
        }

        public async Task<Project?> UpdateProject(Project? project)
        {
            if(project == null)
            {
                return null ;
            }

            _context.projects.Update(project);
            await _context.SaveChangesAsync();

            return project;
        }

        public struct DeleteProjectResult
        {
            public DeleteProjectResult()
            {}
            public bool ProjectIsNull = false; 
            public bool DeleteSuccess = false;
        }

        public async Task<DeleteProjectResult> DeleteProject(Project project)
        {
            if(project == null)
            {
                return new DeleteProjectResult { ProjectIsNull = true};
            }

            _context.projects.Remove( project );

            if(await _context.projects.FindAsync(project) == null)
            {
                return new DeleteProjectResult
                {
                    DeleteSuccess = true
                };
            }
            else
            {
                return new DeleteProjectResult
                {
                    DeleteSuccess = false
                };
            }
        }


        public async Task<List<Project>?> GetProjectsByDeveloper(User Developer)
        {
            if(Developer == null)
            {
                return null;
            }

            var ProjectsByDeveloper = await _context.projects
                                      .Where(x => x.DeveloperID == Developer.Id)
                                      .ToListAsync();

            return ProjectsByDeveloper;
        }

        public async Task<List<Project>?> GetProjectsByClient(Client client)
        {
            if(client == null)
            {
                return null;
            }

            var ProjectsByClient = await _context.projects
                                      .Where(x => x.ClientID == client.Id)
                                      .ToListAsync();

            return ProjectsByClient;
        }
    }
}
