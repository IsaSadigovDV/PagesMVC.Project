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
    public class AuthorController : Controller
    {
        private readonly PagesDbContext _context;
        private readonly IWebHostEnvironment _env;

        public AuthorController(PagesDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            int TotalCount = _context.Authors.Where(x => !x.IsDeleted).Count();
            ViewBag.TotalPage = (int)Math.Ceiling((decimal)TotalCount / 8);
            ViewBag.CurrentPage = page;

            IEnumerable<Author> Authors = await _context.Authors.Where(x => !x.IsDeleted)
                //.Include(x => x.BookAuthors)
                //.ThenInclude(x => x.Book)
                //.Include(x => x.AuthorLanguages)
                //.ThenInclude(x => x.Language)
                //.Include(x => x.AuthoreGenres)
                //.ThenInclude(x => x.GenreId)
                //.Include(x => x.AuthorSocials)
                //.ThenInclude(x => x.Social)   
                .Skip((page - 1) * 5).Take(5)
                .ToListAsync();
            return View(Authors);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            Author? author = await _context.Authors.Where(x => x.Id == id && !x.IsDeleted)
                //.Include(x => x.BookAuthors)
                //.ThenInclude(x => x.Book)
                //.Include(x => x.AuthorLanguages)
                //.ThenInclude(x => x.Language)
                //.Include(x => x.AuthoreGenres)
                //.ThenInclude(x => x.GenreId)
                //.Include(x => x.AuthorSocials)
                //.ThenInclude(x => x.Social)
                .FirstOrDefaultAsync();
            if (author == null)
            {
                return NotFound();
            }
            return View(author);
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Language = new SelectList(_context.Languages.Where(x => !x.IsDeleted).ToList(), "Id","Name");
            ViewBag.Genre = new SelectList(_context.Genres.Where(x =>!x.IsDeleted).ToList(), "Id", "Name");
            ViewBag.Social = new SelectList(_context.Socials.Where(x => !x.IsDeleted).ToList(), "Id", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Author author, int[] language, int[] genre, int[] social)
        {
            //if (!ModelState.IsValid)
            //{
            //    return View(author);
            //}
            if (author.FormFile == null)
            {
                ModelState.AddModelError("FormFile", "The filed image is required");
                return View();
            }

            if (!Helper.IsImage(author.FormFile))
            {
                ModelState.AddModelError("FormFile", "The file type must be image");
                return View();
            }
            if (!Helper.IsSizeOk(author.FormFile, 1))
            {
                ModelState.AddModelError("FormFile", "The file size can not than more 1 mb");
                return View();
            }
            author.AuthorLanguages = new List<AuthorLanguage>();
            if (language != null)
            {
                foreach (var expectedId in language)
                {
                    var languageItem = new AuthorLanguage();
                    languageItem.LanguageId = expectedId;
                    author.AuthorLanguages.Add(languageItem);
                }
            }
            author.AuthoreGenres = new List<AuthoreGenre>();
            if (genre != null)
            {
                foreach (var expectedId in genre)
                {
                    var genreItem = new AuthoreGenre();
                    genreItem.GenreId = expectedId;
                    author.AuthoreGenres.Add(genreItem);
                }
            }
            author.AuthorSocials = new List<AuthorSocial>();
            if (social != null)
            {
                foreach (var expectedId in social)
                {
                    var socialItem = new AuthorSocial();
                    socialItem.SocialId = expectedId;
                    author.AuthorSocials.Add(socialItem);
                }
            }
            author.Image = author.FormFile.CreateImage(_env.WebRootPath, "assets/img");
            author.CreatedDate = DateTime.Now.AddHours(4);

            await _context.AddAsync(author);
            await _context.SaveChangesAsync();

            ViewBag.Language = new SelectList(_context.Languages.Where(x => !x.IsDeleted).ToList(), "Id", "Name");
            ViewBag.Genre = new SelectList(_context.Genres.Where(x => !x.IsDeleted).ToList(), "Id", "Name");
            ViewBag.Social = new SelectList(_context.Socials.Where(x => !x.IsDeleted).ToList(), "Id", "Name");

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            Author? author = await _context.Authors.Where(x => x.Id == id && !x.IsDeleted)
                .Include(x => x.BookAuthors)
                .ThenInclude(x => x.Book)
                .Include(x => x.AuthorLanguages)
                .ThenInclude(x => x.Language)
                .Include(x => x.AuthoreGenres)
                .ThenInclude(x => x.GenreId)
                .Include(x => x.AuthorSocials)
                .ThenInclude(x => x.Social)
                .FirstOrDefaultAsync();
            if (author == null)
            {
                return NotFound();
            }

            ViewBag.Language = new SelectList(_context.Languages.Where(x => !x.IsDeleted).ToList(), "Id", "Name");
            ViewBag.Genre = new SelectList(_context.Genres.Where(x => !x.IsDeleted).ToList(), "Id", "Name");
            ViewBag.Social = new SelectList(_context.Socials.Where(x => !x.IsDeleted).ToList(), "Id", "Name");

            return View(author);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, Author author, int[] language, int[] genre, int[] social)
        {
            Author? updatedAuthor = await _context.Authors.Where(x => x.Id == id && !x.IsDeleted)
                .Include(x => x.AuthorLanguages)
                .ThenInclude(x => x.Language)
                .Include(x => x.AuthoreGenres)
                .ThenInclude(x => x.GenreId)
                .Include(x => x.AuthorSocials)
                .ThenInclude(x => x.Social)
             .FirstOrDefaultAsync();
            if (author == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return View(updatedAuthor);
            }
            if (author.FormFile != null)
            {
                if (!Helper.IsImage(author.FormFile))
                {
                    ModelState.AddModelError("FormFile", "The file type must be image");
                    return View();
                }
                if (!Helper.IsSizeOk(author.FormFile, 1))
                {
                    ModelState.AddModelError("FormFile", "The file size can not than more 1 mb");
                    return View();
                }


                Helper.RemoveImage(_env.WebRootPath, "assets/img", updatedAuthor.Image);

                updatedAuthor.Image = author.FormFile
                           .CreateImage(_env.WebRootPath, "assets/img");

            }
            if (language == null && updatedAuthor.AuthorLanguages.Any())
            {
                foreach (var languageItem in updatedAuthor.AuthorLanguages)
                {
                    _context.AuthorLanguages.Remove(languageItem);
                }
            }
            else if (language != null)
            {

                var expectedIds = _context.AuthorLanguages.Where(x => x.AuthorId == updatedAuthor.Id).Select(x => x.LanguageId).ToList()
                                    .Except(language).ToArray();

                if (expectedIds.Length > 0)
                {
                    foreach (var expectedId in expectedIds)
                    {
                        var languageItem = _context.AuthorLanguages.FirstOrDefault(x => x.LanguageId == expectedId
                                                     && x.AuthorId == updatedAuthor.Id);
                        if (languageItem != null)
                        {
                            _context.AuthorLanguages.Remove(languageItem);
                        }
                    }
                }

                var newExpectedIds = language.Except(_context.AuthorLanguages.Where(x => x.AuthorId == updatedAuthor.Id).Select(x => x.LanguageId).ToList())
                    .ToArray();

                if (newExpectedIds.Length > 0)
                {
                    foreach (var expectedId in newExpectedIds)
                    {
                        var languageItem = new AuthorLanguage();
                        languageItem.LanguageId = expectedId;
                        languageItem.AuthorId = updatedAuthor.Id;

                        await _context.AuthorLanguages.AddAsync(languageItem);
                    }
                }
            }

            if (genre == null && updatedAuthor.AuthoreGenres.Any())
            {
                foreach (var genreItem in updatedAuthor.AuthoreGenres)
                {
                    _context.AuthoreGenres.Remove(genreItem);
                }
            }
            else if (genre != null)
            {
                var expectedIds = _context.AuthoreGenres.Where(x => x.AuthorId == updatedAuthor.Id).Select(x => x.GenreId).ToList()
                                    .Except(genre).ToArray();

                if (expectedIds.Length > 0)
                {
                    foreach (var expectedId in expectedIds)
                    {
                        var genreItem = _context.AuthoreGenres.FirstOrDefault(x => x.GenreId == expectedId
                                                     && x.AuthorId == updatedAuthor.Id);
                        if (genreItem != null)
                        {
                            _context.AuthoreGenres.Remove(genreItem);
                        }
                    }
                }
                var newExpectedIds = genre.Except(_context.AuthoreGenres.Where(x => x.AuthorId == updatedAuthor.Id).Select(x => x.GenreId).ToList())
                    .ToArray();

                if (newExpectedIds.Length > 0)
                {
                    foreach (var expectedId in newExpectedIds)
                    {
                        var genreItem = new AuthoreGenre();
                        genreItem.GenreId = expectedId;
                        genreItem.AuthorId = updatedAuthor.Id;

                        await _context.AuthoreGenres.AddAsync(genreItem);
                    }
                }
            }

            if (social == null && updatedAuthor.AuthorSocials.Any())
            {
                foreach (var socialItem in updatedAuthor.AuthorSocials)
                {
                    _context.AuthorSocials.Remove(socialItem);
                }
            }
            else if (social != null)
            {
                var expectedIds = _context.AuthorSocials.Where(x => x.AuthorId == updatedAuthor.Id).Select(x => x.SocialId).ToList()
                                    .Except(social).ToArray();

                if (expectedIds.Length > 0)
                {
                    foreach (var expectedId in expectedIds)
                    {
                        var socialItem = _context.AuthorSocials.FirstOrDefault(x => x.SocialId == expectedId
                                                     && x.AuthorId == updatedAuthor.Id);
                        if (socialItem != null)
                        {
                            _context.AuthorSocials.Remove(socialItem);
                        }
                    }
                }

                var newExpectedIds = social.Except(_context.AuthorSocials.Where(x => x.AuthorId == updatedAuthor.Id).Select(x => x.SocialId).ToList())
                    .ToArray();

                if (newExpectedIds.Length > 0)
                {
                    foreach (var expectedId in newExpectedIds)
                    {
                        var socialItem = new AuthorSocial();
                        socialItem.SocialId = expectedId;
                        socialItem.AuthorId = updatedAuthor.Id;

                        await _context.AuthorSocials.AddAsync(socialItem);
                    }
                }
            }

            updatedAuthor.Name = author.Name;
            updatedAuthor.Description = author.Description;
            updatedAuthor.Email = author.Email;
            updatedAuthor.PhoneNumber = author.PhoneNumber;
            updatedAuthor.UpdatedDate = DateTime.Now.AddHours(4);

            await _context.SaveChangesAsync();

            ViewBag.Language = new SelectList(_context.Languages.Where(x => !x.IsDeleted).ToList(), "Id", "Name");
            ViewBag.Genre = new SelectList(_context.Genres.Where(x => !x.IsDeleted).ToList(), "Id", "Name");
            ViewBag.Social = new SelectList(_context.Socials.Where(x => !x.IsDeleted).ToList(), "Id", "Name");

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Remove(int id)
        {

            Author? author = await _context.Authors.Where(x => x.Id == id && !x.IsDeleted)
                .FirstOrDefaultAsync();

            if (author == null)
            {
                return NotFound();
            }

            var language = await _context.AuthorLanguages.Where(x => x.AuthorId == author.Id && !x.IsDeleted).ToListAsync();
            var genre = await _context.AuthoreGenres.Where(x => x.AuthorId == author.Id && !x.IsDeleted).ToListAsync();
            var social = await _context.AuthorSocials.Where(x => x.AuthorId == author.Id && !x.IsDeleted).ToListAsync();

            author.IsDeleted = true;
            foreach (var item in language)
            {
                _context.AuthorLanguages.Remove(item);
            }
            foreach (var item in genre)
            {
                _context.AuthoreGenres.Remove(item);
            }
            foreach (var item in social)
            {
                _context.AuthorSocials.Remove(item);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
