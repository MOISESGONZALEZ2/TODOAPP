using DoItList.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DoItList.Data.Entities;

namespace DoItList.Controllers
{
    [ApiController]
    [Authorize]
[Route("api/tasks")]
    // Todas las rutas de este controlador usarán la forma api/tasks
public class TasksController : ControllerBase
    {
        private readonly AppDbContext _db;

        public TasksController(AppDbContext db)
        {
            _db = db;
        }

        // GET api/Tasks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetAll()
        {
            var userId = int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!.Value);
            var tasks = await _db.Tasks
                .Where(t => t.UserId == userId)
                .ToListAsync();
            return Ok(tasks);
        }

        // POST api/Tasks
        [HttpPost]
        public async Task<ActionResult<TaskItem>> Create([FromBody] TaskItem input)
        {
            var userId = int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!.Value);
            input.UserId = userId;
            input.CreatedAt = DateTime.UtcNow;
            input.UpdatedAt = DateTime.UtcNow;

            _db.Tasks.Add(input);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = input.Id }, input);
        }

        // GET api/Tasks/{id}
        [HttpGet("{id:long}")]
        public async Task<ActionResult<TaskItem>> GetById(long id)
        {
            var userId = int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!.Value);
            var task = await _db.Tasks.SingleOrDefaultAsync(t => t.Id == id && t.UserId == userId);
            if (task == null) return NotFound();
            return Ok(task);
        }

        // PUT api/Tasks/{id}
        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, [FromBody] TaskItem input)
        {
            var userId = int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!.Value);
            var existing = await _db.Tasks.SingleOrDefaultAsync(t => t.Id == id && t.UserId == userId);
            if (existing == null) return NotFound();

            existing.Title = input.Title;
            existing.Description = input.Description;
            existing.Priority = input.Priority;
            existing.DueDate = input.DueDate;
            existing.Completed = input.Completed;
            existing.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return NoContent();
        }

        // DELETE api/Tasks/{id}
        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            var userId = int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!.Value);
            var existing = await _db.Tasks.SingleOrDefaultAsync(t => t.Id == id && t.UserId == userId);
            if (existing == null) return NotFound();

            _db.Tasks.Remove(existing);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
