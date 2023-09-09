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

        public async Task<IActionResult> Index(int? id, int page =1)
        {
            int TotalCount = _context.Blogs.Where(x => !x.IsDeleted).Count();
            ViewBag.TotalPage = (int)Math.Ceiling((decimal)TotalCount / 4);
            ViewBag.CurrentPage = page;

            ViewBag.Tags = await _context.Tags.Where(x => !x.IsDeleted).ToListAsync();

            if (id == null)
            {
                IEnumerable<Blog> blogs = await _context.Blogs.Where(x => !x.IsDeleted)
                    .Include(x => x.Tags)
                    .Skip((page - 1) * 12)
            .Take(3).ToListAsync();
                return View(blogs);
            }
            else
            {
                IEnumerable<Blog> blogs = await _context.Blogs.Where(x => !x.IsDeleted).Skip((page - 1) * 12)
           .Take(3).ToListAsync();
                return View(blogs);
            }
        }


        public async Task<IActionResult> Detail(int id)
        {
          
            ViewBag.Blogs = await _context.Blogs.Where(x=>x.Id == id && !x.IsDeleted)
                     .Include(x => x.Tags)
                     .Take(3)
                    .ToListAsync();
            Blog? blog = await _context.Blogs.Where(x => x.Id == id && !x.IsDeleted)
                 .Include(x => x.Tags)
                    .FirstOrDefaultAsync();

            if (blog is null)
            {
                return NotFound();
            }
            BlogVM blogVM = new BlogVM
            {
                Blog = blog
            };

            return View(blogVM);
        }


    }
}
