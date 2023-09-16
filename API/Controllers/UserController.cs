using Microsoft.AspNetCore.Mvc;
using API.Database;
using API.Models;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase {
    private readonly DataContext context = new();

    [HttpPost(Name = "CreateUser")]
    public async Task<ActionResult<Guid>> CreateUser([FromForm] string name, [FromForm] string email, [FromForm] string password) {
        var user = new User(name, email, password);
        context.Users.Add(user);
        await context.SaveChangesAsync();

        return user.ID;
    }

    [HttpDelete("{id:guid}", Name = "DeleteUser")]
    public async Task<ActionResult<User>> DeleteUser(Guid id) {
        User user = await context.Users.FindAsync(id);

        if (user == null) {
            return NotFound();
        }

        context.Users.Remove(user);
        await context.SaveChangesAsync();

        return user;
    }
}
