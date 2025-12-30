using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace PPMP.Data
{
    public class UserDBContext : IdentityDbContext<User, Role, string>
    {
        public UserDBContext(DbContextOptions options) : base(options)
        {
        }

        protected UserDBContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Client>()
                .HasKey(x => x.Id);

            builder.Entity<Client>().HasOne(x => x.User)
                .WithMany(x => x.Clients)
                .HasForeignKey(x => x.DeveloperLinkId);

            builder.Entity<ClientRole>()
                .HasOne(x => x.client)
                .WithOne(x => x.clientRole)
                .HasForeignKey<ClientRole>(x => x.ClientID);

            builder.Entity<ClientRole>()
                .HasOne(x => x.Role)
                .WithMany(x => x.clientRoles)
                .HasForeignKey(x => x.RoleID);

            builder.Entity<ClientRole>().HasKey( x => new { x.ClientID, x.RoleID } );

            base.OnModelCreating(builder);
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<ClientRole> ClientRoles { get; set; }
    }
}
