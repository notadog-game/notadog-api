using Microsoft.EntityFrameworkCore;
using NotadogApi.Domain.Models;


namespace NotadogApi.Persistence.Contexts
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

            builder.Entity<User>().HasData
            (
                new User { Id = 100, Email = "UserEmail1", Name = "UserName1", Password = "UserPassword1", Score = 0 },
                new User { Id = 101, Email = "UserEmail2", Name = "UserName2", Password = "UserPassword2", Score = 0 }
            );
        }
    }
}