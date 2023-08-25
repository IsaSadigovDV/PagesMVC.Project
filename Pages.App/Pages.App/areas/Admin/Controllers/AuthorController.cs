using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pages.App.Context;
using Pages.Core.Entities;

namespace Pages.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AuthorController : Controller
    {
        private readonly PagesDbContext _context;
        private readonly IWebHostEnvironment _env;

        public AuthorController(PagesDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            int TotalCount = _context.Authors.Where(x => !x.IsDeleted).Count();
            ViewBag.TotalPage = (int)Math.Ceiling((decimal)TotalCount / 8);
            ViewBag.CurrentPage = page;

            IEnumerable<Author> Authors = await _context.Authors.Where(x=>!x.IsDeleted)
                .Skip((page-1) * 5).Take(5)
                .ToListAsync();
            return View(Authors);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Author Author)
        {
            if (!ModelState.IsValid)
            {
                return View(Author);
            }
            Author.CreatedDate= DateTime.Now;
            await _context.AddAsync(Author);
            await _context.SaveChangesAsync();  
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            Author? author = await _context.Authors.Where(x => x.Id == id && !x.IsDeleted)
                .FirstOrDefaultAsync();
            if (author == null)
            {
                return NotFound();
            }
            return View(author);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, Author author)
        {
            Author? updatedauthor = await _context.Authors.Where(x => x.Id == id && !x.IsDeleted)
             .FirstOrDefaultAsync();
            if (author == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return View(updatedauthor);
            }
            updatedauthor.Name = author.Name;
            updatedauthor.UpdatedDate = DateTime.Now;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Remove(int id) 
        {

            Author? author = await _context.Authors.Where(x => x.Id == id && !x.IsDeleted)
                .FirstOrDefaultAsync();
            if (author == null)
            {
                return NotFound();
            }
            author.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
