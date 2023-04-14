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
    public class ClientController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ClientController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Jobs/client/f9fn329sp
        // GET all Jobs associated with Client
        [HttpGet("Jobs/{clientId}")]
        public async Task<ActionResult<IEnumerable<Job>>> GetJobs(int clientId)
        {
            if (_context.Clients == null)
            {
                return NotFound();
            }

            var clientJobs= await _context.Jobs.Where(h  => h.ClientId == clientId).ToListAsync();
            return clientJobs;
        }
        
        // GET: api/Client
        // GET all Clients
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Client>>> GetClient()
        {
            if (_context.Clients == null)
            {
                return NotFound();
            }
            // get all clients
            var clients = await _context.Clients.ToListAsync();

            if (clients == null)
            {
                return NotFound();
            }

            
            return clients;
        }

        // GET: api/Client/5
        // GET Client by Id
        [HttpGet("{id}")]
        public async Task<ActionResult<Client>> GetClient(int id)
        {
            if (_context.Clients == null)
            {
                return NotFound();
            }
            var client = await _context.Clients.FindAsync(id);

            if (client == null)
            {
                return NotFound();
            }

            // var clientGet = ClientConverter.ClientToClientGet(client);
            
            return client;
        }

        // PUT: api/Client
        [HttpPut]
        public async Task<IActionResult> PutClient(ClientPut clientPut)
        {
            if (clientPut == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var client = new Client()
            {
                Id = clientPut.Id,
                Name = clientPut.Name,
                ContactInformation = clientPut.ContactInformation
            };

            _context.Entry(client).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientExists(client.Id))
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

        // POST: api/client
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostClient(ClientPost client)
        {
            if (client == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ClientCreate = new Client()
            {
                Name = client.Name,
                ContactInformation = client.ContactInformation,
            };
          
            _context.Clients.Add(ClientCreate);
            await _context.SaveChangesAsync();

            
            var tempClient = new Client()
            {
                Name = ClientCreate.Name,
                ContactInformation = ClientCreate.ContactInformation
            };

            // return Ok(tempClient);
            return CreatedAtAction("GetClient", new { id = tempClient.Id }, tempClient);
        }

        // DELETE: api/Client/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            if (_context.Clients == null)
            {
                return NotFound();
            }
            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ClientExists(int id)
        {
            return (_context.Clients?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}