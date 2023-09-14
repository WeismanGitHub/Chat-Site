using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity;
using API.Models;

namespace API.Database {
    public class UserContext : DbContext {

        public UserContext() : base("UserContext") {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}