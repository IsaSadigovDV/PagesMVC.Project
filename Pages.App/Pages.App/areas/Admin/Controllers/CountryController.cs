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
    public class CountryController : Controller
    {
        private readonly PagesDbContext _context;
        private readonly IWebHostEnvironment _env;

        public CountryController(PagesDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            int TotalCount = _context.Countries.Where(x => !x.IsDeleted).Count();
            ViewBag.TotalPage = (int)Math.Ceiling((decimal)TotalCount / 8);
            ViewBag.CurrentPage = page;
            IEnumerable<Country> countries = await _context.Countries.Where(x => !x.IsDeleted)
                .Skip((page - 1) * 8).Take(8)
                 .ToListAsync();
            return View(countries);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Country Country)
        {
            if (!ModelState.IsValid)
            {
                return View(Country);
            }
            Country.CreatedDate = DateTime.Now;
            await _context.AddAsync(Country);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            Country? Country = await _context.Countries.Where(x => x.Id == id && !x.IsDeleted)
             .FirstOrDefaultAsync();
            if (Country is null)
            {
                return NotFound();
            }
            return View(Country);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, Country Country)
        {
            Country? updatedCountry = await _context.Countries.Where(x => x.Id == id && !x.IsDeleted)
                  .FirstOrDefaultAsync();
            if (Country is null)
            {
                return View(Country);
            }
            if (!ModelState.IsValid)
            {
                return View(updatedCountry);
            }
            updatedCountry.Name = Country.Name;
            updatedCountry.UpdatedDate = DateTime.Now;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Remove(int id)
        {
            Country? Country = await _context.Countries.Where(x => x.Id == id && !x.IsDeleted)
                .FirstOrDefaultAsync();
            if (Country is null)
            {
                return NotFound();
            }
            Country.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
