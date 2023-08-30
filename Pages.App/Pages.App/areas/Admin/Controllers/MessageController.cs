using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using Pages.App.Context;
using Pages.Core.Entities;
using System.Data;

namespace Pages.App.areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "Admin,SuperAdmin")]
    public class MessageController : Controller
    {
        private readonly PagesDbContext _context;
        private readonly IWebHostEnvironment _env;

        public MessageController(PagesDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index(int page =1)
        {
            int TotalCount = _context.Messages.Where(x => !x.IsDeleted).Count();
            ViewBag.TotalPage = (int)Math.Ceiling((decimal)TotalCount / 5);
            ViewBag.CurrentPage = page;

            IEnumerable<Core.Entities.Message> messages = await _context.Messages
                .Where(x => !x.IsDeleted).Skip((page - 1) * 5).Take(5).ToListAsync();
            return View(messages);
        }

        public async Task<IActionResult> Info(int id)
        {
            Core.Entities.Message? message = await _context.Messages
                .Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            if(message is null)
            {
                return NotFound();
            }
            return View(message);
        }

        public async Task<IActionResult> Remove(int id)
        {
            Core.Entities.Message? message = await _context.Messages
            .Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            if (message is null)
            {
                return NotFound();
            }
            message.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
