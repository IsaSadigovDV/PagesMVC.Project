using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pages.App.Context;
using Pages.App.ViewModels;
using Pages.Core.Entities;

namespace Pages.App.Controllers
{
    public class AuthorController : Controller
    {
        private readonly PagesDbContext _context;

        public AuthorController(PagesDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? id, int page = 1)
        {
            int TotalCount = _context.Authors.Where(x => !x.IsDeleted).Count();
            ViewBag.TotalPage = (int)Math.Ceiling((decimal)TotalCount / 4);
            ViewBag.CurrentPage = page;

            ViewBag.Language = new SelectList(_context.Languages.Where(x => !x.IsDeleted).ToList(), "Id", "Name");
            ViewBag.Genre = new SelectList(_context.Genres.Where(x => !x.IsDeleted).ToList(), "Id", "Name");
            ViewBag.Category = new SelectList(_context.Categories.Where(x => !x.IsDeleted).ToList(), "Id", "Name");

            if (id == null)
            {
                IEnumerable<Author> authors = await _context.Authors.Where(x => !x.IsDeleted)
                     .Include(x => x.BookAuthors)
                     .ThenInclude(x => x.Author)
                     .Include(x => x.AuthorLanguages)
                     .ThenInclude(x => x.Language)
                     .Include(x=>x.AuthorSocials)
                     .ThenInclude(x=>x.Social)
                     .Include(x=>x.AuthoreGenres)
                     .ThenInclude(x=>x.Genre)
                    .Skip((page - 1) * 12)
            .Take(12).ToListAsync();
                return View(authors);
            }
            else
            {
                IEnumerable<Author> authors = await _context.Authors.Where(x => !x.IsDeleted).Skip((page - 1) * 12)
           .Take(12).ToListAsync();
                return View(authors);
            }
        }

        public async Task<IActionResult> Detail(int? id, int page = 1)
        {
            ViewBag.Author = await _context.Authors.Where(x=>!x.IsDeleted)
                    .Include(x => x.BookAuthors)
                     .ThenInclude(x => x.Author)
                     .Include(x => x.AuthorLanguages)
                     .ThenInclude(x => x.Language)
                     .Include(x => x.AuthorSocials)
                     .ThenInclude(x => x.Social)
                     .Include(x => x.AuthoreGenres)
                     .ThenInclude(x => x.Genre)
                .Take(3)
                .ToListAsync();

            Author? author = await _context.Authors.Where(x => !x.IsDeleted)
                 .Include(x => x.BookAuthors)
                     .ThenInclude(x => x.Author)
                     .Include(x => x.AuthorLanguages)
                     .ThenInclude(x => x.Language)
                     .Include(x => x.AuthorSocials)
                     .ThenInclude(x => x.Social)
                     .Include(x => x.AuthoreGenres)
                     .ThenInclude(x => x.Genre)
                .FirstOrDefaultAsync();

            if(author is null)
            {
                return NotFound();
            }
            AuthorVM authorVM = new AuthorVM
            {
                Author = author,
            };
            return View (authorVM);
           
        }
    }
}
