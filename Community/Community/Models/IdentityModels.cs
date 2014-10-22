using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Data.Entity;
using System;

namespace Community.Models
{
    /// <summary>
    /// Identity 2.0 User
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// Groups that the user is a member in
        /// </summary>
        public virtual ICollection<Group> GroupMemberships { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
        public ApplicationUser()
        {
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
        public DbSet<UserStatistics> UserStatistics { get; set; }

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