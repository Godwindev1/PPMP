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


        public async Task<Client?> FindByIdAsync(string Id)
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

        public async Task<bool> SetPasswordAsync(Client client, string Password)
        {
            if (await _context.Clients.FirstOrDefaultAsync(x => x.Id == client.Id) == null)
            {
                return false;
            }

            var passwordHasher = new PasswordHasher<Client>();
            var PasswordHash = passwordHasher.HashPassword(client, Password);

            client.PasswordHash = PasswordHash;
            client.HasPassword = true;
            _context.Clients.Update(client);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<ClientSigninResult> SignInPasswordAsync(Client client, string Password, bool RememberMe)
        {
            if (await FindByIdAsync(client.Id.ToString()) == null)
            {
                return new ClientSigninResult
                {
                    Succeeded = false,
                    IsNotAllowed = false,
                    PasswordNotSet = false,
                    RequiresTwoFactor = false
                };
            }

            if (_httpContext.HttpContext == null)
            {
                return new ClientSigninResult
                {
                    Succeeded = false,
                    IsNotAllowed = false,
                    PasswordNotSet = false,
                    RequiresTwoFactor = false
                };
            }

            if (!client.HasPassword)
            {
                return new ClientSigninResult
                {
                    Succeeded = false,
                    IsNotAllowed = false,
                    PasswordNotSet = true,
                    RequiresTwoFactor = false
                };
            }


            var entry = _context.Entry(client);

            if (entry.State == EntityState.Detached)
            {
                _context.Attach(client);
            }

            await _context.Entry(client)
                  .Reference(x => x.clientRole)
                  .Query()
                  .Include(x => x.Role)
                  .LoadAsync();

            var passwordHasher = new PasswordHasher<Client>();
            var Result = passwordHasher.VerifyHashedPassword(client, client.PasswordHash, Password);

            if (Result == PasswordVerificationResult.Failed)
            {
                return new ClientSigninResult
                {
                    Succeeded = false,
                    IsNotAllowed = false,
                    PasswordNotSet = false,
                    RequiresTwoFactor = false
                };
            }
            else if (Result == PasswordVerificationResult.SuccessRehashNeeded)
            {
                await SetPasswordAsync(client, Password);
            }

            ClaimsPrincipal ClientAuth = new ClaimsPrincipal();
            ClaimsIdentity identity = new ClaimsIdentity(new List<Claim>(),
                                            CookieAuthenticationDefaults.AuthenticationScheme
                                            , ClaimTypes.Name, ClaimTypes.Role);

            identity.AddClaim(new Claim(identity.RoleClaimType, client.clientRole.Role.NormalizedName));
            identity.AddClaim(new Claim(identity.NameClaimType, client.Name));

            ClientAuth.AddIdentity(identity);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = RememberMe,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(30),
                AllowRefresh = true
            };

            await _httpContext.HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, ClientAuth, authProperties);

            if (_httpContext.HttpContext.User.Identity.IsAuthenticated)
            {
                return new ClientSigninResult
                {
                    Succeeded = true,
                    IsNotAllowed = false,
                    PasswordNotSet = false,
                    RequiresTwoFactor = false
                };
            }
            else
            {
                return new ClientSigninResult
                {
                    Succeeded = false,
                    IsNotAllowed = false,
                    PasswordNotSet = false,
                    RequiresTwoFactor = false
                };
            }
        }


        public async Task<bool> AddToRoleAsync(Client client, string _Role)
        {
            Role? role = _context.Roles.FirstOrDefault(x => x.NormalizedName.Equals(_Role.ToUpperInvariant()));

            if (client == null || role == null)
            {
                return false;
            }

            ClientRole clientRole = new ClientRole { ClientID = client.Id, RoleID = role.Id };

            await _context.ClientRoles.AddAsync(clientRole);
            await SaveAsync();

            return true;
        }

        public async Task<List<Client>> GetAllClientsByDeveloper(User user)
        {
            return await _context.Clients.Where(x => x.DeveloperLinkId == user.Id).ToListAsync();
        }

        public float GetClientSatisfactionRate()
        {
            return 100.0f;
        }
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
            return;
        }
    }
}
