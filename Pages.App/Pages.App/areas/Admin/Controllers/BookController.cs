using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pages.App.Context;
using Pages.App.Extentions;
using Pages.App.Helpers;
using Pages.Core.Entities;
using System.Data;

namespace Pages.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class BookController : Controller
    {

        private readonly PagesDbContext _context;
        private readonly IWebHostEnvironment _env;

        public BookController(PagesDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            int TotalCount = _context.Books.Where(x => !x.IsDeleted).Count();
            ViewBag.TotalPage = (int)Math.Ceiling((decimal)TotalCount / 8);
            ViewBag.CurrentPage = page;

            IEnumerable<Book> Books = await _context.Books.Where(x => !x.IsDeleted)
                .Include(x => x.BookAuthors)
                .ThenInclude(x => x.Author)
                .Include(x => x.BookLanguages)
                .ThenInclude(x => x.Language)
                .Include(x => x.Genre)
                .Skip((page - 1) * 5).Take(5)
                .ToListAsync();
            return View(Books);
        }


        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            Book? book = await _context.Books.Where(x => x.Id == id && !x.IsDeleted)
                .Include(x => x.BookAuthors)
                .ThenInclude(x => x.Author)
                .Include(x => x.BookLanguages)
                .ThenInclude(x => x.Language)
                .Include(x => x.Genre)
                .FirstOrDefaultAsync();
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Language = new SelectList(_context.Languages.Where(x => x.IsDeleted == null).ToList(), "Id", "Text");
            ViewBag.Genre = new SelectList(_context.Genres.Where(x => x.IsDeleted == null).ToList(), "Id", "Text");
            ViewBag.Author = new SelectList(_context.Authors.Where(x => x.IsDeleted == null).ToList(), "Id", "Text");
            ViewBag.Category = new SelectList(_context.Categories.Where(x => x.IsDeleted == null).ToList(), "Id", "Text");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book book, int[] language, int[] genre, int[] author)
        {
            if (!ModelState.IsValid)
            {
                return View(book);
            }
            if (book.FormFile == null)
            {
                ModelState.AddModelError("FormFile", "The filed image is required");
                return View();
            }

            if (!Helper.IsImage(book.FormFile))
            {
                ModelState.AddModelError("FormFile", "The file type must be image");
                return View();
            }
            if (!Helper.IsSizeOk(book.FormFile, 1))
            {
                ModelState.AddModelError("FormFile", "The file size can not than more 1 mb");
                return View();
            }
            book.BookLanguages = new List<BookLanguage>();
            if (language != null)
            {
                foreach (var expectedId in language)
                {
                    var languageItem = new BookLanguage();
                    languageItem.LanguageId = expectedId;
                    book.BookLanguages.Add(languageItem);
                }
            }
         
            book.BookAuthors = new List<BookAuthor>();
            if (author != null)
            {
                foreach (var expectedId in author)
                {
                    var authorItem = new BookAuthor();
                    authorItem.AuthorId = expectedId;
                    book.BookAuthors.Add(authorItem);
                }
            }
            book.Image = book.FormFile.CreateImage(_env.WebRootPath, "assets/img");
            book.CreatedDate = DateTime.Now.AddHours(4);

            await _context.AddAsync(book);
            await _context.SaveChangesAsync();

            ViewBag.Language = new SelectList(_context.Languages.Where(x => x.IsDeleted == null).ToList(), "Id", "Text");
            ViewBag.Genre = new SelectList(_context.Genres.Where(x => x.IsDeleted == null).ToList(), "Id", "Text");
            ViewBag.Author = new SelectList(_context.Authors.Where(x => x.IsDeleted == null).ToList(), "Id", "Text");
            ViewBag.Category = new SelectList(_context.Categories.Where(x => x.IsDeleted == null).ToList(), "Id", "Text");

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            Book? book = await _context.Books.Where(x => x.Id == id && !x.IsDeleted)
                .Include(x => x.BookAuthors)
                .ThenInclude(x => x.Author)
                .Include(x => x.BookLanguages)
                .ThenInclude(x => x.Language)
                .Include(x => x.Genre)
                .FirstOrDefaultAsync();
            if (book == null)
            {
                return NotFound();
            }

            ViewBag.Language = new SelectList(_context.Languages.Where(x => x.IsDeleted == null).ToList(), "Id", "Text");
            ViewBag.Genre = new SelectList(_context.Genres.Where(x => x.IsDeleted == null).ToList(), "Id", "Text");
            ViewBag.Author = new SelectList(_context.Authors.Where(x => x.IsDeleted == null).ToList(), "Id", "Text");
            ViewBag.Category = new SelectList(_context.Categories.Where(x => x.IsDeleted == null).ToList(), "Id", "Text");

            return View(book);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, Book book, int[] language, int[] genre, int[] author)
        {
            Book? updatedBook = await _context.Books.Where(x => x.Id == id && !x.IsDeleted)
                .Include(x => x.BookAuthors)
                .ThenInclude(x => x.Author)
                .Include(x => x.BookLanguages)
                .ThenInclude(x => x.Language)
                .Include(x => x.Genre)
             .FirstOrDefaultAsync();
            if (book == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return View(updatedBook);
            }
            if (book.FormFile != null)
            {
                if (!Helper.IsImage(book.FormFile))
                {
                    ModelState.AddModelError("FormFile", "The file type must be image");
                    return View();
                }
                if (!Helper.IsSizeOk(book.FormFile, 1))
                {
                    ModelState.AddModelError("FormFile", "The file size can not than more 1 mb");
                    return View();
                }


                Helper.RemoveImage(_env.WebRootPath, "assets/img", updatedBook.Image);

                updatedBook.Image = book.FormFile
                           .CreateImage(_env.WebRootPath, "assets/img");

            }
            if (language == null && updatedBook.BookLanguages.Any())
            {
                foreach (var languageItem in updatedBook.BookLanguages)
                {
                    _context.BookLanguages.Remove(languageItem);
                }
            }
            else if (language != null)
            {
                var expectedIds = _context.BookLanguages.Where(x => x.BookId == updatedBook.Id).Select(x => x.LanguageId).ToList()
                                    .Except(language).ToArray();

                if (expectedIds.Length > 0)
                {
                    foreach (var expectedId in expectedIds)
                    {
                        var languageItem = _context.BookLanguages.FirstOrDefault(x => x.LanguageId == expectedId
                                                     && x.BookId == updatedBook.Id);
                        if (languageItem != null)
                        {
                            _context.BookLanguages.Remove(languageItem);
                        }
                    }
                }

                var newExpectedIds = language.Except(_context.BookLanguages.Where(x => x.BookId == updatedBook.Id).Select(x => x.LanguageId).ToList())
                    .ToArray();

                if (newExpectedIds.Length > 0)
                {
                    foreach (var expectedId in newExpectedIds)
                    {
                        var languageItem = new BookLanguage();
                        languageItem.LanguageId = expectedId;
                        languageItem.BookId = updatedBook.Id;

                        await _context.BookLanguages.AddAsync(languageItem);
                    }
                }
            }

            if (author == null && updatedBook.BookAuthors.Any())
            {
                foreach (var authorItem in updatedBook.BookAuthors)
                {
                    _context.BookAuthors.Remove(authorItem);
                }
            }
            else if (author != null)
            {
                var expectedIds = _context.BookAuthors.Where(x => x.BookId == updatedBook.Id).Select(x => x.AuthorId).ToList()
                                    .Except(author).ToArray();

                if (expectedIds.Length > 0)
                {
                    foreach (var expectedId in expectedIds)
                    {
                        var authorItem = _context.BookAuthors.FirstOrDefault(x => x.AuthorId == expectedId
                                                     && x.BookId == updatedBook.Id);
                        if (authorItem != null)
                        {
                            _context.BookAuthors.Remove(authorItem);
                        }
                    }
                }

                var newExpectedIds = author.Except(_context.BookAuthors.Where(x => x.BookId == updatedBook.Id).Select(x => x.AuthorId).ToList())
                    .ToArray();

                if (newExpectedIds.Length > 0)
                {
                    foreach (var expectedId in newExpectedIds)
                    {
                        var authorItem = new BookAuthor();
                        authorItem.AuthorId = expectedId;
                        authorItem.BookId = updatedBook.Id;

                        await _context.BookAuthors.AddAsync(authorItem);
                    }
                }
            }

            updatedBook.Name = book.Name;
            updatedBook.Price = book.Price;
            updatedBook.Description = book.Description;
            updatedBook.Name = book.Name;
            updatedBook.Publisher = book.Publisher;
            updatedBook.PaperCount = book.PaperCount;
            updatedBook.Dimensions = book.Dimensions;
            updatedBook.CategoryId = book.CategoryId;
            updatedBook.GenreId = book.GenreId;
            updatedBook.UpdatedDate = DateTime.Now.AddHours(4);

            await _context.SaveChangesAsync();

            ViewBag.Language = new SelectList(_context.Languages.Where(x => !x.IsDeleted).ToList(), "Id", "Text");
            ViewBag.Genre = new SelectList(_context.Genres.Where(x => !x.IsDeleted).ToList(), "Id", "Text");
            ViewBag.Author = new SelectList(_context.Authors.Where(x => !x.IsDeleted).ToList(), "Id", "Text");
            ViewBag.Category = new SelectList(_context.Categories.Where(x => !x.IsDeleted).ToList(), "Id", "Text");

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Remove(int id)
        {

            Book? book = await _context.Books.Where(x => x.Id == id && !x.IsDeleted)
                .FirstOrDefaultAsync();

            if (book == null)
            {
                return NotFound();
            }

            var language = await _context.BookLanguages.Where(x => x.BookId == book.Id && !x.IsDeleted).ToListAsync();
            var author = await _context.BookAuthors.Where(x => x.BookId == book.Id && !x.IsDeleted).ToListAsync();

            book.IsDeleted = true;
            foreach (var item in language)
            {
                _context.BookLanguages.Remove(item);
            }
            foreach (var item in author)
            {
                _context.BookAuthors.Remove(item);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    } 
}
