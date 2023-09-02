using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pages.App.Context;
using Pages.App.Extentions;
using Pages.App.Helpers;
using Pages.Core.Entities;
using System.Net.NetworkInformation;

namespace Pages.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SocialController : Controller
    {
        private readonly PagesDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SocialController(PagesDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            int TotalCount = _context.Socials.Where(x => !x.IsDeleted).Count();
            ViewBag.TotalPage = (int)Math.Ceiling((decimal)TotalCount / 8);
            ViewBag.CurrentPage = page;
            IEnumerable<Social> Socials = await _context.Socials.Where(x => !x.IsDeleted)
                .Include(x => x.Setting)
                .Skip((page - 1) * 8).Take(8)
                 .ToListAsync();
            return View(Socials);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Social Social)
        {
            if (!ModelState.IsValid)
            {
                return View(Social);
            }
            if(Social.file== null)
            {
                ModelState.AddModelError("file", "Image must be added");
                return View(Social);
            }
            if (!Helper.IsImage(Social.file))
            {
                ModelState.AddModelError("file", "File must be image");
                return View(Social);
            }
            if (!Helper.IsSizeOk(Social.file, 1))
            {
                ModelState.AddModelError("file", "Size of Image must less than 1 mb!!!");
                return View(Social);
            }
          
            Social.Icon = Social.file.CreateImage(_env.WebRootPath, "assets/img");
            Social.CreatedDate = DateTime.Now;
            await _context.AddAsync(Social);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update (int id)
        {
            Social? social = await _context.Socials.Where(x => x.Id == id && !x.IsDeleted)
                .FirstOrDefaultAsync();

            if (social == null)
            {
                return NotFound();
            }
            return View(social);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, Social social)
        {
            Social? updatedsocial = await _context.Socials.Where(x=>x.Id == id&& !x.IsDeleted)
                .FirstOrDefaultAsync();
            
            if(social is null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return View(updatedsocial);
            }

            if(social.file != null) 
            {
                if (!Helper.IsImage(social.file))
                {
                    ModelState.AddModelError("file", "File must be image");
                    return View(social);
                }
                if (!Helper.IsSizeOk(social.file, 1))
                {
                    ModelState.AddModelError("file", "Size of Image must less than 1 mb!!!");
                    return View(social);
                }

                Helper.RemoveImage(_env.WebRootPath, "assets/img/", updatedsocial.Icon);
                updatedsocial.Icon = social.file.CreateImage(_env.WebRootPath, "assets/img/");
            }

            updatedsocial.Name = social.Name;
            updatedsocial.UpdatedDate = DateTime.Now;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Remove(int id)
        {
            Social? social = await _context.Socials.Where(x => x.Id == id && !x.IsDeleted)
                .FirstOrDefaultAsync();

            if (social == null)
            {
                return NotFound();
            }
            social.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
