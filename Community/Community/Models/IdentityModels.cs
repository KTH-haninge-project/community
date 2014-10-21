using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Data.Entity;

namespace Community.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<Group> GroupMemberships { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        // Navigation property
     //   public virtual ICollection<Message> SentMessages { get; set; }
        public ApplicationUser()
        {
     //       SentMessages = new List<Message>();
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Message> Messages { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<ReadEntry> ReadEntries { get; set; }

     protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
 	       modelBuilder.Entity<ApplicationUser>().HasMany<Group>(s => s.GroupMemberships).WithMany(c => c.Members).Map(c =>
        {
            c.MapLeftKey("ApplicationUser_id");
            c.MapRightKey("Group_id");
            c.ToTable("T_GROUP_USER");
        });

        base.OnModelCreating(modelBuilder);
        }
    }
}