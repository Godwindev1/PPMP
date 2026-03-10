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

            
            builder.Entity<Subgoal>()
            .HasOne(x => x.state)
            .WithMany(x => x.subgoals)
            .HasForeignKey(x => x.stateTagID)
            .OnDelete(DeleteBehavior.Restrict);
        }
        protected void DefineChatProperties(ModelBuilder builder)
        {
            builder.Entity<ChatRoom>().HasKey( x => x.Id);
            builder.Entity<ChatRoom>().HasMany(x => x.Messages).WithOne(x => x.Room).HasForeignKey(x => x.RoomId);

            builder.Entity<ChatRoomMember>()
            .HasKey(x => new { x.RoomId, x.UserId }); // composite PK

            builder.Entity<ChatRoomMember>()
            .HasOne(x => x.Room)
            .WithMany(x => x.Members)
            .HasForeignKey(x => x.RoomId);

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
            builder.Entity<GoalTask>().HasKey(x => x.ID);
            
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
            DefineChatProperties(builder);
            base.OnModelCreating(builder);
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<ClientRole> ClientRoles { get; set; }

        public DbSet<Project> projects {get; set;}
        public DbSet<ProjectModification> projectModifications {get; set; }
        public DbSet<Subgoal> subgoals {get; set; }
        public DbSet<StateTag> stateTags {get; set;}
        public DbSet<SessionPage> Sessions {get; set; }
        public DbSet<GoalTask> tasks {get; set; }

        public DbSet<ChatRoomMember> ChatRoomMembers {get; set;}
        public DbSet<ChatRoom> ChatRooms {get; set; }
        public DbSet<Message> Messages {get; set; }
    }
}
