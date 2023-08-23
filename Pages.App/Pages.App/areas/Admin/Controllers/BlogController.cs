using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pages.App.Context;
using Pages.App.Extentions;
using Pages.App.Helpers;
using Pages.Core.Entities;

namespace Pages.App.areas.Admin.Controllers
{
    [Area("Admin")]
    public class BlogController : Controller
    {
        private readonly PagesDbContext _context;
        private readonly IWebHostEnvironment _env;

        public BlogController(PagesDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Blog> blogs = await
                    _context.Blogs
                       .Where(x => !x.IsDeleted).ToListAsync();
            return View(blogs);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Blog blog)
        {

            if (!ModelState.IsValid)
            {
                return View();
            }

            if (blog.FormFile == null)
            {
                ModelState.AddModelError("FormFile", "The filed image is required");
                return View();
            }

            if (!Helper.IsImage(blog.FormFile))
            {
                ModelState.AddModelError("FormFile", "The file type must be image");
                return View();
            }
            if (!Helper.IsSizeOk(blog.FormFile, 1))
            {
                ModelState.AddModelError("FormFile", "The file size can not than more 1 mb");
                return View();
            }

            blog.Image = blog.FormFile.CreateImage(_env.WebRootPath, "assets/img");
            blog.CreatedDate = DateTime.Now;
            await _context.AddAsync(blog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            Blog? blog = await _context.Blogs
                           .Where(x => !x.IsDeleted && x.Id == id)
                               .FirstOrDefaultAsync();

            if (blog == null)
                return NotFound();

            return View(blog);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, Blog blog)
        {
            Blog? Updateblog = await _context.Blogs
                      .Where(x => !x.IsDeleted && x.Id == id)
                          .FirstOrDefaultAsync();

            if (blog == null)
                return NotFound();

            if (!ModelState.IsValid)
            {
                return View(Updateblog);
            }



            if (blog.FormFile != null)
            {
                if (!Helper.IsImage(blog.FormFile))
                {
                    ModelState.AddModelError("FormFile", "The file type must be image");
                    return View();
                }
                if (!Helper.IsSizeOk(blog.FormFile, 1))
                {
                    ModelState.AddModelError("FormFile", "The file size can not than more 1 mb");
                    return View();
                }

                Helper.RemoveImage(_env.WebRootPath, "assets/img", Updateblog.Image);

                Updateblog.Image = blog.FormFile
                           .CreateImage(_env.WebRootPath, "assets/img");

            }

            Updateblog.Description = blog.Description;
            Updateblog.Title = blog.Title;
            Updateblog.UpdatedDate = DateTime.Now;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Delete(int id)
        {
            Blog? blog = await _context.Blogs
                        .Where(x => !x.IsDeleted && x.Id == id)
                            .FirstOrDefaultAsync();

            if (blog == null)
                return NotFound();


            blog.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
