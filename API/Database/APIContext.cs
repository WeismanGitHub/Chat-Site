using Microsoft.EntityFrameworkCore;
using API.Models;

namespace API.Database {
    public class APIContext : DbContext {
        public APIContext(DbContextOptions<APIContext> options) : base(options) {
        }

        public DbSet<User> User { get; set; } = default!;
    }
}
