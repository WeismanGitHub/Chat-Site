using Microsoft.EntityFrameworkCore;
using API.Models;

namespace API.Database {
    public class DataContext : DbContext {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            if (!optionsBuilder.IsConfigured) {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();
                var connectionString = configuration.GetConnectionString("SQLiteDatabase");
                optionsBuilder.UseSqlite(connectionString);
            }
        }

        public DbSet<User> Users { get; set; }
    }
}
