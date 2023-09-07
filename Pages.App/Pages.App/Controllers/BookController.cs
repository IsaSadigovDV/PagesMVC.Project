using Microsoft.AspNetCore.Mvc;

namespace Pages.App.Controllers
{
    public class BookController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
