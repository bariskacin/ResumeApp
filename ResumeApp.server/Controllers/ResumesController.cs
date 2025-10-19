using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResumeApp.server.Models;

namespace ResumeApp.server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResumesController : ControllerBase
    {
        private readonly ResumeDbContext _context;

        public ResumesController(ResumeDbContext context)
        {
            _context = context;
        }

        // GET: api/resumes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Resume>>> GetResumes([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var resumes = await _context.Resumes
                .Include(r => r.Person)
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync();

            return Ok(resumes);
        }

        // GET: api/resumes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Resume>> GetResume(int id)
        {
            var resume = await _context.Resumes.FindAsync(id);

            if (resume == null)
            {
                return NotFound();
            }

            return Ok(resume);
        }

        // POST: api/resumes
        [HttpPost]
        public async Task<IActionResult> CreateResume([FromBody] Resume resume)
        {
            _context.Resumes.Add(resume);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetResume), new { id = resume.Id }, resume);
        }

        // PUT: api/resumes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateResume(int id, [FromBody] Resume resume)
        {
            if (id != resume.Id)
            {
                return BadRequest();
            }

            _context.Entry(resume).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ResumeExists(id))
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

        // DELETE: api/resumes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteResume(int id)
        {
            var resume = await _context.Resumes.FindAsync(id);
            if (resume == null)
            {
                return NotFound();
            }

            _context.Resumes.Remove(resume);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ResumeExists(int id)
        {
            return _context.Resumes.Any(e => e.Id == id);
        }
    }
}
