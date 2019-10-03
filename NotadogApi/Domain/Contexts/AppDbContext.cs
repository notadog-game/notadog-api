using Microsoft.EntityFrameworkCore;
using NotadogApi.Domain.Users.Models;

namespace NotadogApi.Domain.Contexts
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>().ToTable("Users");
            builder.Entity<User>().HasKey(p => p.Id);
            builder.Entity<User>().Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Entity<User>().HasIndex(e => e.Email).IsUnique();
            builder.Entity<User>().Property(p => p.Email).IsRequired();
            builder.Entity<User>().Property(p => p.Name).IsRequired().HasMaxLength(30);
            builder.Entity<User>().Property(p => p.Password).IsRequired();
            builder.Entity<User>().Property(p => p.Score);


            builder.Entity<User>().HasData
            (
                new User { Id = 1, Email = "user1@gmail.com", Name = "user1", Password = "user1" },
                new User { Id = 2, Email = "user2@gmail.com", Name = "user2", Password = "user2" },
                new User { Id = 3, Email = "user3@gmail.com", Name = "user3", Password = "user3" },
                new User { Id = 4, Email = "user4@gmail.com", Name = "user4", Password = "user4" },
                new User { Id = 5, Email = "user5@gmail.com", Name = "user5", Password = "user5" }
            );
        }
    }
}
