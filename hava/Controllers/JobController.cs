using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using hava.Data;
using hava.Models;

namespace hava.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public JobController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Job/client/2
        // GET all Jobs associated with Client
        [HttpGet("client/{clientId}")]
        public async Task<ActionResult<IEnumerable<Job>>> GetClientJobs(int clientId)
        {
            if (_context.Jobs == null)
            {
                return NotFound();
            }

            var jobsOfClient = await _context.Jobs
                .Where(z => z.ClientId == clientId)
                .ToListAsync();
            
            return jobsOfClient;
        }
        
        // GET: api/Job/user/2
        // GET all Jobs associated with User
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Job>>> GetUserJobs(string userId)
        {
            if (_context.Jobs == null)
            {
                return NotFound();
            }

            var jobsOfUser = await _context.UserJobs
                .Where(uj => uj.ApplicationUserId == userId)
                .Select(uj => uj.Job)
                .ToListAsync();
            
            return jobsOfUser;
        }

        // GET: api/Job/5
        // GET Job by Id
        [HttpGet("{id}")]
        public async Task<ActionResult<Job>> GetJob(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var job = await _context.Jobs.FindAsync(id);

            if (job == null)
            {
                return NotFound();
            }

            return job;
        }

        // PUT: api/Job/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutJob(JobPut jobPut)
        {
            if (jobPut == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var job = new Job()
            {
                Id = jobPut.Id,
                Name = jobPut.Name,
                ClientId = jobPut.ClientId
            };
          
            _context.Entry(job).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JobExists(job.Id))
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

        // POST: api/Job
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostJob(JobPost jobPost)
        {
            if (jobPost == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var job = new Job()
            {
                Name = jobPost.Name,
                ClientId = jobPost.ClientId
            };
              
            _context.Jobs.Add(job);
            await _context.SaveChangesAsync();
            
            return NoContent();
        }

        // DELETE: api/Job/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJob(int id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var job = await _context.Jobs.FindAsync(id);
            if (job == null)
            {
                return NotFound();
            }

            _context.Jobs.Remove(job);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool JobExists(int id)
        {
            return (_context.Jobs?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}