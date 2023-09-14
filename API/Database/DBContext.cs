using Microsoft.EntityFrameworkCore;
using API.Models;

namespace API.Database {
    public class DataContext : DbContext {
        protected readonly IConfiguration Configuration;

        public DataContext(IConfiguration configuration) {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options) {
            options.UseSqlite(Configuration.GetConnectionString("SQLiteDatabase"));
        }

        public DbSet<User> Users { get; set; }
    }
}
