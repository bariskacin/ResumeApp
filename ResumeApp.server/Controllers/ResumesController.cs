using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResumeApp.server.Models;

namespace ResumeApp.server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
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
            var userId = GetUserId();
            var query = _context.Resumes
                .Where(r => r.PersonId == userId)
                .OrderByDescending(r => r.UpdatedAt);

            var total = await query.CountAsync();
            var resumes = await query
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync();

            return Ok(new { total, items = resumes });
        }

        // GET: api/resumes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Resume>> GetResume(int id)
        {
            var userId = GetUserId();
            var resume = await _context.Resumes.FirstOrDefaultAsync(r => r.Id == id && r.PersonId == userId);

            if (resume == null)
            {
                return NotFound();
            }

            return Ok(resume);
        }

        // POST: api/resumes
        [HttpPost]
        public async Task<IActionResult> CreateResume([FromBody] ResumeRequest request)
        {
            var userId = GetUserId();

            var resume = new Resume
            {
                PersonId = userId,
                Headline = request.Headline,
                Summary = request.Summary,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Resumes.Add(resume);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetResume), new { id = resume.Id }, resume);
        }

        // PUT: api/resumes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateResume(int id, [FromBody] ResumeRequest request)
        {
            var userId = GetUserId();
            var resume = await _context.Resumes.FirstOrDefaultAsync(r => r.Id == id && r.PersonId == userId);
            if (resume == null)
            {
                return NotFound();
            }

            resume.Headline = request.Headline;
            resume.Summary = request.Summary;
            resume.UpdatedAt = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ResumeExists(id, userId))
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
            var userId = GetUserId();
            var resume = await _context.Resumes.FirstOrDefaultAsync(r => r.Id == id && r.PersonId == userId);
            if (resume == null)
            {
                return NotFound();
            }

            _context.Resumes.Remove(resume);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ResumeExists(int id, int personId)
        {
            return _context.Resumes.Any(e => e.Id == id && e.PersonId == personId);
        }

        private int GetUserId()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim))
            {
                throw new UnauthorizedAccessException("User identifier missing from token.");
            }

            return int.Parse(userIdClaim);
        }
    }

    public class ResumeRequest
    {
        [Required]
        [StringLength(500)]
        public string Headline { get; set; } = string.Empty;

        [StringLength(2000)]
        public string Summary { get; set; } = string.Empty;
    }
}
