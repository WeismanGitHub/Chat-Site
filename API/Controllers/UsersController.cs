using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using API.Database;
using API.Models;
using System.Data.SqlTypes;

namespace API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase {
        private readonly APIContext _context;

        public UsersController(APIContext context) {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUser() {
            if (_context.User == null) {
                return NotFound();
            }

            return await _context.User.ToListAsync();
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
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(Guid id, User user) {
            if (id != user.ID) {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            } catch (DbUpdateConcurrencyException) {
                if (!UserExists(id)) {
                    return NotFound();
                } else {
                    throw;
                }
            }

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
            } catch (DbUpdateConcurrencyException) {
                if (UserExists(user.ID)) {
                    return StatusCode(StatusCodes.Status409Conflict, new { message = "Email is already in database." });
                } else {
                    throw;
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

        private bool UserExists(Guid id) {
            return (_context.User?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
