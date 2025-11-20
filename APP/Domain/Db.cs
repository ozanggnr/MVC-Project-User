using Microsoft.EntityFrameworkCore;

namespace APP.Domain
{
    public class Db : DbContext
    {
        public Db(DbContextOptions options) : base(options)
        {
        }

       
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }      
        public DbSet<Group> Groups { get; set; }     
        public DbSet<UserRole> UserRoles { get; set; } 

       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

           
            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);

          
            modelBuilder.Entity<User>()
                .HasOne(u => u.Group)
                .WithMany(g => g.Users)
                .HasForeignKey(u => u.GroupId);

           
            modelBuilder.Entity<User>()
                .Property(u => u.Score)
                .HasColumnType("decimal(18,2)");
        }
    }
}