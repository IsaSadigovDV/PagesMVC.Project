using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Pages.App.Controllers
{
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}