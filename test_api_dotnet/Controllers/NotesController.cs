using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using test_api_dotnet;

namespace test_api_dotnet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public NotesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Notes>>> GetNotes(string orderby, string search)
        {
            var notes = from s in _context.Notes
                           select s;

            if (!String.IsNullOrEmpty(search))
            {
                notes = notes.Where(s => s.Title.Contains(search)
                                       || s.Body.Contains(search));
            }

            switch (orderby)
            {
                case "title_desc":
                    notes = notes.OrderByDescending(s => s.Title);
                    break;
                case "title":
                    notes = notes.OrderBy(s => s.Title);
                    break;
                case "date_desc":
                    notes = notes.OrderByDescending(s => s.NoteDate);
                    break;
                case "body_desc":
                    notes = notes.OrderByDescending(s => s.Body);
                    break;
                case "body":
                    notes = notes.OrderBy(s => s.Body);
                    break;
                default:
                    notes = notes.OrderBy(s => s.NoteDate);
                    break;
            }
            return await notes.AsNoTracking().ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Notes>> GetNotes(int id)
        {
            var notes = await _context.Notes.FindAsync(id);

            if (notes == null)
            {
                return NotFound();
            }

            return notes;
        }

        [HttpPost]
        public async Task<ActionResult<Notes>> PostNotes(Notes notes)
        {
            _context.Notes.Add(notes);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNotes", new { id = notes.NotesId }, notes);
        }

        [Route("edit")]
        [HttpPost]
        public async Task<IActionResult> UpdateNotes(Notes notes)
        {
            _context.Entry(notes).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NotesExists(notes.NotesId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(notes);
        }

        private bool NotesExists(int id)
        {
            return _context.Notes.Any(e => e.NotesId == id);
        }
    }
}
