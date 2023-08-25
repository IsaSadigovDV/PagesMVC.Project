using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Pages.App.Context;
using Pages.Core.Entities;

namespace Pages.App.areas.Admin.Controllers
{
    [Area("Admin")]
    public class LanguageController : Controller
    {
        private readonly PagesDbContext _context;
        private readonly IWebHostEnvironment _env;

        public LanguageController(PagesDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            int TotalCount = _context.Languages.Where(x => !x.IsDeleted).Count();
            ViewBag.TotalPage = (int)Math.Ceiling((decimal)TotalCount / 5);
            ViewBag.CurrentPage = page;

            IEnumerable<Language> languages = await _context.Languages
                .Where(x => !x.IsDeleted).Skip((page - 1) * 5).Take(5).ToListAsync();
            return View(languages);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Language language)
        {
            if (!ModelState.IsValid)
            {
                return View(language);
            }
            language.CreatedDate = DateTime.Now;
            await _context.AddAsync(language);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            Language? language = await _context.Languages
                .Where(x=>!x.IsDeleted&& x.Id == id).FirstOrDefaultAsync();
            
            if (language is null) 
            {
                return NotFound();
            }
            return View(language);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, Language language)
        {
            Language? updatedlanguage = await _context.Languages
             .Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            
            if(language is null)
            {
                return View(language);
            }

            if (!ModelState.IsValid)
            {
                return View(updatedlanguage);
            }

            updatedlanguage.Name = language.Name;
            updatedlanguage.UpdatedDate = DateTime.Now;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Remove(int id)
        {
            Language? language = await _context.Languages
               .Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();

            if (language is null)
            {
                return NotFound();
            }
            language.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
