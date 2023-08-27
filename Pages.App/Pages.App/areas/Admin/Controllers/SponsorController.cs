using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pages.App.Context;
using Pages.App.Extentions;
using Pages.App.Helpers;
using Pages.Core.Entities;
using System.Data;
using System.Reflection.Metadata;

namespace Pages.App.areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class SponsorController : Controller
    {
        private readonly PagesDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SponsorController(PagesDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Sponsor> sponsors = await _context.Sponsors
                .Where(x => !x.IsDeleted).ToListAsync();
            return View(sponsors);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Sponsor sponsor)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }

            if(sponsor.FormFile == null)
            {
                ModelState.AddModelError("FormFile", "The filed image is required");
                return View();
            }

            if (!Helper.IsImage(sponsor.FormFile))
            {
                ModelState.AddModelError("FormFile", "The file type must be image");
                return View();
            }
            if (!Helper.IsSizeOk(sponsor.FormFile, 1))
            {
                ModelState.AddModelError("FormFile", "The file size can not than more 1 mb");
                return View();
            }

            sponsor.Image = sponsor.FormFile.CreateImage(_env.WebRootPath, "assets/img");
            sponsor.CreatedDate = DateTime.Now;
            await _context.AddAsync(sponsor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            Sponsor? sponsor = await _context.Sponsors
                .Where(x=>!x.IsDeleted).FirstOrDefaultAsync();

            if(sponsor == null)
            {
                return View();
            }

            return View(sponsor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, Sponsor sponsor)
        {

            Sponsor? updatedSponsor = await _context.Sponsors
                .Where(x => !x.IsDeleted).FirstOrDefaultAsync();

            if (sponsor == null)
            {
                return View();
            }

            if (!ModelState.IsValid)
            {
                return View(updatedSponsor);
            }

            if(sponsor.FormFile != null)
            {
                if (!Helper.IsImage(sponsor.FormFile))
                {
                    ModelState.AddModelError("FormFile", "The file type must be image");
                    return View();
                }
                if (!Helper.IsSizeOk(sponsor.FormFile, 1))
                {
                    ModelState.AddModelError("FormFile", "The file size can not than more 1 mb");
                    return View();
                }


                Helper.RemoveImage(_env.WebRootPath, "assets/img", updatedSponsor.Image);

                updatedSponsor.Image = sponsor.FormFile
                           .CreateImage(_env.WebRootPath, "assets/img");

            }

            updatedSponsor.Description = sponsor.Description;
            updatedSponsor.Title = sponsor.Title;
            updatedSponsor.UpdatedDate = DateTime.Now;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Delete(int id)
        {

            Sponsor? sponsor = await _context.Sponsors
                .Where(x => !x.IsDeleted).FirstOrDefaultAsync();

            if (sponsor == null)
            {
                return View();
            }
            sponsor.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
    }
}
