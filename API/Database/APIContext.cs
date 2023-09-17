using Microsoft.EntityFrameworkCore;
using API.Models;

namespace API.Database {
    public class APIContext : DbContext {
        public APIContext(DbContextOptions<APIContext> options) : base(options) {
        }

        public DbSet<User> User { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            // Configure a unique index on the "Email" property of the "User" entity
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }

}
