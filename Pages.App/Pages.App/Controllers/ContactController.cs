using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pages.App.Context;
using Pages.Core.Entities;
using System.Text.RegularExpressions;

namespace Pages.App.Controllers
{
    public class ContactController : Controller
    {
        private readonly PagesDbContext _context;

        public ContactController(PagesDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> SendEmail(Message message)
        {
            string strRegex = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";

            Regex re = new Regex(strRegex);
            if (!ModelState.IsValid)
            {
                TempData["Message"] = "Please fill all inputs";
                return RedirectToAction(nameof(Index));
            }
            if (!re.IsMatch(message.Email))
            {
                TempData["Email"] = "Please add valid email";
                return RedirectToAction("index", "home");
            }
           
            await _context.AddAsync(message);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Successfully send message";
            return RedirectToAction(nameof(Index));
        }
    }
}
