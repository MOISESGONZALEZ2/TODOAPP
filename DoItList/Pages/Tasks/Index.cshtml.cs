using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DoItList.Data;
using TaskItem = DoItList.Data.Entities.TaskItem;

namespace DoItList.Pages.Tasks
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext context) => _context = context;

        [BindProperty]
        public TaskItem InputTask { get; set; } = new();

        public List<TaskItem> Tasks { get; set; } = [];

        public async Task OnGetAsync()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (long.TryParse(userIdClaim, out var userId))
            {
                Tasks = await _context.Tasks
                                      .Where(t => t.UserId == userId)
                                      .OrderByDescending(t => t.CreatedAt)
                                      .ToListAsync();
            }
        }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!long.TryParse(userIdClaim, out var userId))
                return RedirectToPage();

            InputTask.UserId = userId;
            InputTask.IsCompleted = false;
            InputTask.CreatedAt = DateTime.UtcNow;
            InputTask.UpdatedAt = DateTime.UtcNow;

            _context.Tasks.Add(InputTask);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(long id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task != null)
            {
                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }
    }
}
