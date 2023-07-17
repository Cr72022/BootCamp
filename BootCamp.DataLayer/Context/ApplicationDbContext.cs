using BootCamp.DataLayer.Entities.User;
using BootCamp.DataLayer.Entities.Wallet;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BootCamp.DataLayer.Entities.Permissions;
using BootCamp.DataLayer.Entities.Course;

namespace BootCamp.DataLayer.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<User> User { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<UserRole> UserRole { get; set; }
        #region Course
        public DbSet<Course> Course { get; set; }
        public DbSet<CourseLevel> CourseLevel { get; set; }
        public DbSet<CourseStatus> CourseStatus { get; set; }
        public DbSet<CourseEpisode> CourseEpisode { get; set; }
        public DbSet<CourseGroup> CourseGroup { get; set; }
        #endregion
        public DbSet<Wallet> Wallet { get; set; }
        public DbSet<WalletType> WalletType { get; set; }
        public DbSet<Permission> Permission { get; set; }
        public DbSet<RolePermission> RolePermission { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>()
                .Ignore(c => c.Group);
            modelBuilder.Entity<User>()
                .HasQueryFilter(u => !u.IsDelete);
            modelBuilder.Entity<Role>()
               .HasQueryFilter(r => !r.IsDelete);
            modelBuilder.Entity<CourseGroup>()
               .HasQueryFilter(g => !g.IsDelete);
            base.OnModelCreating(modelBuilder);
        }
    }
}
