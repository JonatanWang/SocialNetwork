using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using TheSocialNetwork.Models;

namespace TheSocialNetwork.DAL
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            
        }

        public DbSet<Message> Messages { get; set; }
        public DbSet<Group> Groups { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUser>().HasMany(u => u.Messages).WithRequired(m => m.Recevier);
            modelBuilder.Entity<ApplicationUser>().HasMany(u => u.Groups).WithMany(g => g.Receviers);

            base.OnModelCreating(modelBuilder);
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}