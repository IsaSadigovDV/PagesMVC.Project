using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pages.App.Context;
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
            return View();
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