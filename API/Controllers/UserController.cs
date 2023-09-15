using Microsoft.AspNetCore.Mvc;
using API.Database;
using API.Models;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase {
    private readonly DataContext context = new DataContext();

    [HttpPost(Name = "CreateUser")]
    public void Post(string name, string email, string password) {
        User user = new User(name, email, password);
        context.Users.Add(user);
        context.SaveChanges();
    }
}
