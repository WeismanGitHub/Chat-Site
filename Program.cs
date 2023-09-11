using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;

using var db = new DBContext("Chat-Site");
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<DBContext>(opt => opt.UseSqlite());
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

db.Add(new User {
    ID = Guid.NewGuid().ToString(),
    Name = "Test",
    Password = "password",
});

db.SaveChanges();

app.MapGet("/users", async (DBContext db) =>
    await db.Users.ToListAsync());

app.Run();
