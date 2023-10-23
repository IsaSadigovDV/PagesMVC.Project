using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pages.App.Context;
using Pages.App.ViewModels;
using Pages.Core.Entities;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Pages.App.Controllers
{
    public class HomeController : Controller
    {
        private readonly PagesDbContext _context;

        public HomeController(PagesDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            HomeVM homeVM = new HomeVM()
            {
                Sponsors = _context.Sponsors.Where(x => !x.IsDeleted).ToList(),
                Blogs = _context.Blogs.Where(x => !x.IsDeleted).ToList(),
                Books = _context.Books.Where(x => !x.IsDeleted).Include(x => x.BookLanguages).ThenInclude(x => x.Language).ToList(),
                Comments = _context.Comments.Where(x => !x.IsDeleted && x.BookId !=null )
                .OrderByDescending(c=>c.CreatedDate)
                .Include(x=>x.AppUser)
                .Include(x=>x.Blog)
                .Include(c=>c.Book)
                .Take(5)
                .ToList(),
                Authors = _context.Authors.Where(x => !x.IsDeleted)
                .OrderByDescending(c=>c.CreatedDate)
                .Take(1)
                .Include(x=>x.AuthoreGenres).ThenInclude(x=>x.Genre)
                              .Include(x => x.BookAuthors)
                     .ThenInclude(x => x.Book)
                     .ToList()
            };
            return View(homeVM);
        }

        [HttpPost]
        public async Task<IActionResult> PostSubscribe(Subscribe subscribe)
        {
            string strRegex = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";

            Regex re = new Regex(strRegex);
            if (subscribe == null)
            {
                return NotFound();
            }
            if (!re.IsMatch(subscribe.Email))
            {
                TempData["Email"] = "Please add valid email";
                return RedirectToAction("index", "home");
            }
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Invalid Email");
            }
            if( _context.Subscribes.Any(x=>x.Email == subscribe.Email))
            {
                TempData["Email"] = "This email is already registered";

            }
            await _context.AddAsync(subscribe);
            await _context.SaveChangesAsync();
            TempData["Verify"] = "Successfully added Email";
            return RedirectToAction(nameof(Index));
        }



    }
}