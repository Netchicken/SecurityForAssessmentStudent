using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecurityForAssessmentStudent.Data;
using SecurityForAssessmentStudent.Models;

namespace SecurityForAssessmentStudent.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CastsAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CastsAPIController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/CastsAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cast>>> GetCast()
        {
            return await _context.Cast.ToListAsync();
        }

        // GET: api/CastsAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cast>> GetCast(Guid id)
        {
            var cast = await _context.Cast.FindAsync(id);

            if (cast == null)
            {
                return NotFound();
            }

            return cast;
        }

        // PUT: api/CastsAPI/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCast(Guid id, Cast cast)
        {
            if (id != cast.Id)
            {
                return BadRequest();
            }

            _context.Entry(cast).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CastExists(id))
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

        // POST: api/CastsAPI
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Cast>> PostCast(Cast cast)
        {
            _context.Cast.Add(cast);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCast", new { id = cast.Id }, cast);
        }

        // DELETE: api/CastsAPI/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCast(Guid id)
        {
            var cast = await _context.Cast.FindAsync(id);
            if (cast == null)
            {
                return NotFound();
            }

            _context.Cast.Remove(cast);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CastExists(Guid id)
        {
            return _context.Cast.Any(e => e.Id == id);
        }
    }
}
