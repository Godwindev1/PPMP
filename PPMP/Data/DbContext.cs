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

        protected void DefineUIstateProperties(ModelBuilder builder)
        {
            //Should Not cascade delete

            builder.Entity<Project>()
            .HasOne(x => x.State)
            .WithMany(x => x.projects)
            .HasForeignKey(x => x.CurrentStateTagID)
            .OnDelete(DeleteBehavior.Restrict);
        }
        protected void DefineCommentsProperties(ModelBuilder builder)
        {
             //Comments
            builder.Entity<Comment>().HasKey(x => x.CommentID);
            builder.Entity<Comment>()
            .HasOne(x => x.project)
            .WithMany(x => x.Comments)
            .HasForeignKey(x => x.ProjectID);
            
            builder.Entity<Comment>()
            .HasOne(x => x.Developer)
            .WithMany(x => x.Comments)
            .HasForeignKey(x => x.AuthorDeveloperID);

            builder.Entity<Comment>()
            .HasOne(x => x.client)
            .WithMany(x => x.Comments)
            .HasForeignKey(x => x.AuthorClientID);
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
            builder.Entity<Client>().Property(x => x.HasPassword).HasDefaultValue(false);

            //projects
            builder.Entity<Project>().HasKey(x => x.ID);
            builder.Entity<Project>().HasOne(x => x.client)
            .WithMany(x => x.projects)
            .HasForeignKey(x => x.ClientID);
            
            builder.Entity<Project>().HasOne(x => x.Developer)
            .WithMany(x => x.projects)
            .HasForeignKey(x => x.DeveloperID);

    

            
            //Project Modifications
            builder.Entity<ProjectModification>().HasKey(x => x.ID);
            builder.Entity<ProjectModification>()
            .HasOne(x => x.project)
            .WithMany(x => x.projectModifications)
            .HasForeignKey(x => x.ProjectID);

            builder.Entity<ProjectModification>()
            .HasOne(x => x.subgoal)
            .WithMany(x => x.modifications)
            .HasForeignKey(x => x.SubGoalAnchorID);       

            //subgoals
            builder.Entity<Subgoal>().HasKey(x => x.ID);
            builder.Entity<Subgoal>()
            .HasOne(x => x.project)
            .WithMany(x => x.subgoals)
            .HasForeignKey(x => x.ProjectID);

            builder.Entity<Subgoal>().HasMany(x => x.Tasks).WithOne(x => x.subgoal).HasForeignKey(x => x.SubGoalID);  
            builder.Entity<Task>().HasKey(x => x.ID);
            
            //Sessions 
            builder.Entity<SessionPage>().HasKey(x => x.SessionID);
            builder.Entity<SessionPage>()
            .HasOne(x => x.Developer)
            .WithMany(x => x.Sessions)
            .HasForeignKey(x => x.DeveloperID);

            builder.Entity<SessionPage>()
            .HasOne(x => x.client)
            .WithOne(x => x.Session)
            .HasForeignKey<SessionPage>(x => x.CLientID);

            DefineUIstateProperties(builder);
            //DefineCommentsProperties(builder);
            base.OnModelCreating(builder);
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<ClientRole> ClientRoles { get; set; }

        public DbSet<Project> projects {get; set;}
        public DbSet<ProjectModification> projectModifications {get; set; }
        public DbSet<Subgoal> subgoals {get; set; }
        public DbSet<StateTag> stateTags {get; set;}
        public DbSet<SessionPage> Sessions {get; set; }

        public DbSet<Task> tasks {get; set; }

        //TODO; REVISIONS
        //private DbSet<Comment> comments {get; set;}
        //private DbSet<AnchorNode> anchorNodes {get; set; }
        //private DbSet<CommentAnchor> commentAnchors {get; set; }

    }
}
