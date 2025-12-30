using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PPMP.Data;

namespace PPMP.Repo
{
    public class ClientRepo
    {
        private readonly UserDBContext _context;
        private readonly HttpContextAccessor _httpContext;

        public ClientRepo(UserDBContext Contexts, IHttpContextAccessor httpContext)
        {
            _context = Contexts;
            _httpContext = (HttpContextAccessor)httpContext;
        }

     
        public async Task<Client ?> FindByIdAsync(string Id)
        {
            Guid guidId;
                if (!Guid.TryParse(Id, out guidId))
                    return null; 

            var result = await _context.Clients
                                .FirstOrDefaultAsync(x => x.Id == guidId);

            return result;
        }

        public async Task<Client?> CreateAsync(Client client, User user)
        {
            if (client == null || user == null) return null;

            client.Id = Guid.NewGuid();
            client.DeveloperLinkId = user.Id;
            client.CreatedAt = DateTimeOffset.UtcNow;
            client.EndDateOriginalTimeZone = TimeZoneInfo.Local.StandardName;
            client.StartDateOriginalTimeZone = TimeZoneInfo.Local.StandardName;

            await _context.Clients.AddAsync(client);
            await _context.SaveChangesAsync();

            return client; 
        }

        public async Task<bool> SetPassword(Client client, string Password)
        {
            if(await _context.Clients.FirstOrDefaultAsync(x => x.Id == client.Id) == null)
            {
                return false;
            }

            var passwordHasher = new PasswordHasher<Client>();
            var PasswordHash = passwordHasher.HashPassword(client, Password);

            client.PasswordHash = PasswordHash;
            client.HasPassword = true;
            _context.Clients.Update(client);

            return true;
        }

        public async Task<bool> SignInPassword(Client client, string Password)
        {
            if(await _context.Clients.FirstOrDefaultAsync(x => x.Id == client.Id) == null)
            {
                return false;
            }

            if(_httpContext.HttpContext == null)
            {
                return false;
            }

            await _context.Entry(client)
                    .Reference(x => x.clientRole)
                    .Query()
                    .Include(x => x.Role)
                    .LoadAsync();

            var passwordHasher = new PasswordHasher<Client>();
            passwordHasher.VerifyHashedPassword(client, client.PasswordHash, Password);

            ClaimsPrincipal ClientAuth = new ClaimsPrincipal();
            ClaimsIdentity identity = new ClaimsIdentity(new List<Claim>(),
                                             CookieAuthenticationDefaults.AuthenticationScheme
                                            ,ClaimTypes.Name, ClaimTypes.Role);

            identity.AddClaim(new Claim(identity.RoleClaimType, client.clientRole.Role.NormalizedName));
            identity.AddClaim(new Claim(identity.NameClaimType, client.Name));

            ClientAuth.AddIdentity(identity);

            await _httpContext.HttpContext.SignInAsync(ClientAuth);

            if(_httpContext.HttpContext.User.Identity.IsAuthenticated)
            {
                return true;
            }

            return false;
        }


        public async Task<bool> AddToRoleAsync(Client client, string _Role)
        {
            Role ?role = _context.Roles.FirstOrDefault(x => x.NormalizedName.Equals(_Role.ToUpperInvariant()) );
            
            if(client == null|| role ==null )
            {
                return false;
            }

            ClientRole clientRole = new ClientRole { ClientID = client.Id, RoleID = role.Id  };

            await  _context.ClientRoles.AddAsync(clientRole);
            await SaveAsync();

            return true;
        }

 
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
            return ;
        }
    }
}
