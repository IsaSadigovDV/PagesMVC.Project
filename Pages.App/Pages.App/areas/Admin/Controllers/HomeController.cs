using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pages.Core.Entities;
using System.Data;

namespace Pages.App.areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "Admin,SuperAdmin")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
