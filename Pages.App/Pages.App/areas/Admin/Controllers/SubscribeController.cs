using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pages.App.Context;
using Pages.Core.Entities;
using System.Data;

namespace Pages.App.areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "Admin,SuperAdmin")]
    public class SubscribeController : Controller
    {
        private readonly PagesDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SubscribeController(PagesDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Subscribe> subscribes = await _context.Subscribes.Where(x => !x.IsDeleted).ToListAsync();
            return View(subscribes);  
        }

        public async Task<IActionResult> Remove(int id)
        {
            Subscribe? subscribe = await _context.Subscribes.Where(x => x.Id == id && !x.IsDeleted)
                .FirstOrDefaultAsync();
            if (subscribe is null) 
            {
                return NotFound();
            }
            subscribe.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
