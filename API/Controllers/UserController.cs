using Microsoft.AspNetCore.Mvc;
using API.Models;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase {
    private readonly ILogger<UserController> _logger;

    public UserController(ILogger<UserController> logger) {
        _logger = logger;
    }

    [HttpPost(Name = "CreateUser")]
    public void Post() {
        Console.WriteLine("x");
    }
}
