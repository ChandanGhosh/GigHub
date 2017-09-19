using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace GigHub.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Gig> Gigs { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Followings> Followings { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<UserNotification> UserNotifications { get; set; }


        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Attendance>().HasRequired(a => a.Gig).WithMany(g => g.Attendances).WillCascadeOnDelete(false);

            modelBuilder.Entity<ApplicationUser>().HasMany(a => a.Followees).WithRequired(a => a.Follower).WillCascadeOnDelete(false);
            modelBuilder.Entity<ApplicationUser>().HasMany(a => a.Followers).WithRequired(a => a.Followee).WillCascadeOnDelete(false);
            modelBuilder.Entity<UserNotification>().HasRequired(a => a.User).WithMany(a => a.UserNotifications).WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}