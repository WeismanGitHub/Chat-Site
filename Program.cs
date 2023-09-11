using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;

using var db = new UserContext();
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<UserContext>(opt => opt.UseSqlite());
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

var user = new User {
    ID = Guid.NewGuid().ToString(),
    Name = "Test",
    Password = "password",
};

db.Add(user);
db.SaveChanges();

app.MapGet("/users", async (UserContext db) =>
    await db.Users.ToListAsync());

app.Run();

public class User {
    public string ID { get; set; }
    public string Name { get; set; }
    public string Password { get; set; } // I know I need to hash it.
}

public class UserContext : DbContext {
    public DbSet<User> Users { get; set; }

    public string DbPath { get; }

    public UserContext() {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = System.IO.Path.Join(path, "Chat-Site");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
}