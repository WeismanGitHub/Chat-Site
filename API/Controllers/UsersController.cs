using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using API.Database;
using API.Models;
using System.Data.SqlTypes;
using System.ComponentModel.DataAnnotations;

namespace API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase {
        private readonly APIContext _context;

        public UsersController(APIContext context) {
            _context = context;
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(Guid id) {
            if (_context.User == null) {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);

            if (user == null) {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(Guid id, [FromForm] UserOptionalDTO userTDO) {
            var user = await _context.User.FindAsync(id);

            if (user == null) {
                return NotFound();
            }

            user.Name = userTDO.Name ?? user.Name;
            user.Email = userTDO.Email ?? user.Email;
            user.Password = userTDO.Password ?? user.Password;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<User>> PostUser([FromForm] UserDTO userTDO) {
            if (_context.User == null) {
                return Problem("Entity set 'APIContext.User'  is null.");
            }

            var user = new User {
                Name = userTDO.Name,
                Email = userTDO.Email,
                Password = userTDO.Password,
            };

            _context.User.Add(user);

            try {
                await _context.SaveChangesAsync();
            } catch (Exception error) {
                try {
                    await _context.User.SingleAsync(u => u.Email == user.Email);
                    return StatusCode(StatusCodes.Status409Conflict, new { message = "Email is already in database." });
                } catch (Exception) {
                    throw error;
                }
            }

            return CreatedAtAction("GetUser", new { id = user.ID }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id) {
            if (_context.User == null) {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);

            if (user == null) {
                return NotFound();
            }

            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
