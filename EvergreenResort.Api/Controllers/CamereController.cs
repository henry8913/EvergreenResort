using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EvergreenResort.Api.Data;
using EvergreenResort.Api.Models;

namespace EvergreenResort.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CamereController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    public CamereController(ApplicationDbContext context) => _context = context;

    // GET: api/camere
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Camera>>> GetCamere() 
        => await _context.Camere.ToListAsync();

    // GET: api/camere/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Camera>> GetCamera(int id)
    {
        var camera = await _context.Camere.FindAsync(id);
        if (camera == null) return NotFound();
        return camera;
    }

    // POST: api/camere
    [HttpPost]
    public async Task<ActionResult<Camera>> PostCamera(Camera camera)
    {
        _context.Camere.Add(camera);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetCamera), new { id = camera.Id }, camera);
    }

    // PUT: api/camere/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCamera(int id, Camera camera)
    {
        if (id != camera.Id) return BadRequest();

        _context.Entry(camera).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Camere.Any(e => e.Id == id)) return NotFound();
            throw;
        }

        return NoContent();
    }

    // DELETE: api/camere/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCamera(int id)
    {
        var camera = await _context.Camere.FindAsync(id);
        if (camera == null) return NotFound();

        _context.Camere.Remove(camera);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // POST: api/camere/cambia-stato/1 (Solo per Admin)
    [HttpPost("cambia-stato/{id}")]
    public async Task<IActionResult> CambiaStato(int id, [FromBody] string nuovoStato)
    {
        var camera = await _context.Camere.FindAsync(id);
        if (camera == null) return NotFound();

        camera.Stato = nuovoStato;
        await _context.SaveChangesAsync();
        return Ok();
    }
}