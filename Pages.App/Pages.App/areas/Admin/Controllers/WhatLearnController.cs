using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pages.App.Context;
using Pages.Core.Entities;

namespace Pages.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class WhatLearnController : Controller
    {
        private readonly PagesDbContext _context;

        public WhatLearnController(PagesDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int page=1)
        {
            int TotalCount = _context.WhatLearns.Where(x => !x.IsDeleted).Count();
            ViewBag.TotalPage = (int)Math.Ceiling((decimal)TotalCount / 8);
            ViewBag.CurrentPage = page;
            IEnumerable<WhatLearn> whatLearns = await _context.WhatLearns.Where(x => !x.IsDeleted)
                .Skip((page - 1) * 8).Take(8)
                .ToListAsync();
            return View(whatLearns);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WhatLearn whatLearn)
        {
            if (!ModelState.IsValid)
            {
                return View(whatLearn);
            }
            whatLearn.CreatedDate= DateTime.Now;
            await _context.AddAsync(whatLearn);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            WhatLearn? whatLearn = await _context.WhatLearns.Where(x => x.Id == id && !x.IsDeleted)
                .FirstOrDefaultAsync();
            if(whatLearn is null)
            {
                return NotFound();
            }
            return View(whatLearn);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async  Task<IActionResult> Update(int id, WhatLearn whatLearn)
        {
            WhatLearn? updatedwhatlearn = await _context.WhatLearns.Where(x=>x.Id == id && !x.IsDeleted)
                .FirstOrDefaultAsync();
            if(whatLearn is null)
            {
                return View(whatLearn);
            }
            if (!ModelState.IsValid)
            {
                return View(updatedwhatlearn);
            }
            updatedwhatlearn.Text = whatLearn.Text;
            updatedwhatlearn.UpdatedDate = DateTime.Now;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Remove(int id)
        {
            WhatLearn? whatLearn = await _context.WhatLearns.Where(x => x.Id == id && !x.IsDeleted)
               .FirstOrDefaultAsync();
            if (whatLearn is null)
            {
                return NotFound();
            }
            whatLearn.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
