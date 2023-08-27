using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Pages.App.Context;
using Pages.App.ViewModels;
using Pages.Core.Entities;
using System.Reflection.Metadata;

namespace Pages.App.Controllers
{
    public class BlogController : Controller
    {
        private readonly PagesDbContext _context;

        public BlogController(PagesDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index( int page =1)
        {
            int TotalCount = _context.Blogs.Where(x => !x.IsDeleted).Count();
            ViewBag.TotalPage = (int)Math.Ceiling((decimal)TotalCount / 4);
            ViewBag.CurrentPage = page;

            IEnumerable<Blog> blogs = await _context.Blogs.Where(x => !x.IsDeleted).Skip((page - 1) * 12)
                .Take(12).ToListAsync();
            return View(blogs);
        }
    }
}
