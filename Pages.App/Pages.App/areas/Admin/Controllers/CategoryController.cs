using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pages.App.Context;
using Pages.Core.Entities;
using System.Data;

namespace Pages.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class CategoryController : Controller
    {
        private readonly PagesDbContext _context;
        private readonly IWebHostEnvironment _env;

        public CategoryController(PagesDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            int TotalCount = _context.Categories.Where(x => !x.IsDeleted).Count();
            ViewBag.TotalPage = (int)Math.Ceiling((decimal)TotalCount / 8);
            ViewBag.CurrentPage = page;
            IEnumerable<Category> Categories = await _context.Categories.Where(x => !x.IsDeleted)
                .Skip((page - 1) * 8).Take(8)
                 .ToListAsync();
            return View(Categories);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category Category)
        {
            if (!ModelState.IsValid)
            {
                return View(Category);
            }
            Category.CreatedDate = DateTime.Now;
            await _context.AddAsync(Category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            Category? Category = await _context.Categories.Where(x => x.Id == id && !x.IsDeleted)
             .FirstOrDefaultAsync();
            if (Category is null)
            {
                return NotFound();
            }
            return View(Category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, Category Category)
        {
            Category? updatedCategory = await _context.Categories.Where(x => x.Id == id && !x.IsDeleted)
                  .FirstOrDefaultAsync();
            if (Category is null)
            {
                return View(Category);
            }
            if (!ModelState.IsValid)
            {
                return View(updatedCategory);
            }
            updatedCategory.Name = Category.Name;
            updatedCategory.UpdatedDate = DateTime.Now;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Remove(int id)
        {
            Category? Category = await _context.Categories.Where(x => x.Id == id && !x.IsDeleted)
                .FirstOrDefaultAsync();
            if (Category is null)
            {
                return NotFound();
            }
            Category.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
