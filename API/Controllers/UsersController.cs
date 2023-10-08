using Microsoft.AspNetCore.Mvc;

namespace API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase {
        public UsersController() {
        }

        //// GET: api/Users/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<UserModel>> GetUser(Guid id) {
        //}

        //// PUT: api/Users/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> UpdateUser() {
        //}

        //// POST: api/Users
        //[HttpPost]
        //public async Task<ActionResult<UserModel>> CreateUser() {
        //}

        //// DELETE: api/Users/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteUser(Guid id) {
        //}
    }
}
