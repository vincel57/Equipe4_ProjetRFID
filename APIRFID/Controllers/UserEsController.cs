using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIRFID.Data;
using APIRFID.Model;

namespace APIRFID.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserEsController : ControllerBase
    {
        private readonly APIRFIDContext _context;

        public UserEsController(APIRFIDContext context)
        {
            _context = context;
        }

        // GET: api/UserEs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserE>>> GetUserE()
        {
          if (_context.UserE == null)
          {
              return NotFound();
          }
            return await _context.UserE.ToListAsync();
        }

        // GET: api/UserEs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserE>> GetUserE(int id)
        {
          if (_context.UserE == null)
          {
              return NotFound();
          }
            var userE = await _context.UserE.FindAsync(id);

            if (userE == null)
            {
                return NotFound();
            }

            return userE;
        }

        // PUT: api/UserEs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserE(int id, UserE userE)
        {
            if (id != userE.id)
            {
                return BadRequest();
            }

            _context.Entry(userE).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserEExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/UserEs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserE>> PostUserE(UserE userE)
        {
          if (_context.UserE == null)
          {
              return Problem("Entity set 'APIRFIDContext.UserE'  is null.");
          }
            _context.UserE.Add(userE);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserE", new { id = userE.id }, userE);
        }

        // DELETE: api/UserEs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserE(int id)
        {
            if (_context.UserE == null)
            {
                return NotFound();
            }
            var userE = await _context.UserE.FindAsync(id);
            if (userE == null)
            {
                return NotFound();
            }

            _context.UserE.Remove(userE);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserEExists(int id)
        {
            return (_context.UserE?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
