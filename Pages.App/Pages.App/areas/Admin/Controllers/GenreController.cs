using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pages.App.Context;
using Pages.Core.Entities;
using System.Data;

namespace Pages.App.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin,SuperAdmin")]
	public class GenreController : Controller
	{
		private readonly PagesDbContext _context;
		private readonly IWebHostEnvironment _env;

		public GenreController(PagesDbContext context, IWebHostEnvironment env)
		{
			_context = context;
			_env = env;
		}

		public async Task<IActionResult> Index(int page = 1)
		{
			int TotalCount = _context.Genres.Where(x => !x.IsDeleted).Count();
			ViewBag.TotalPage = (int)Math.Ceiling((decimal)TotalCount / 5);
			ViewBag.CurrentPage = page;

			IEnumerable<Genre> genres = await _context.Genres
				.Where(x => !x.IsDeleted).Skip((page - 1) * 5).Take(5).ToListAsync();
			return View(genres);
		}

		[HttpGet]
		public async Task<IActionResult> Create()
		{
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(Genre genre)
		{
			//if (!ModelState.IsValid)
			//{
			//	return View(genre);
			//}
			genre.CreatedDate = DateTime.Now.AddHours(4);
			 _context.Genres.Add(genre);
			await _context.SaveChangesAsync();

			return RedirectToAction(nameof(Index));
		}


		[HttpGet]
		public async Task<IActionResult> Update(int id)
		{
			Genre? genre = await _context.Genres
				.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();

			if (genre is null)
			{
				return NotFound();
			}
			return View(genre);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Update(int id, Genre genre)
		{
			Genre? updatedGenre = await _context.Genres
			 .Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();

			if (genre is null)
			{
				return View(genre);
			}

			if (!ModelState.IsValid)
			{
				return View(updatedGenre);
			}

			updatedGenre.Name = genre.Name;
			updatedGenre.UpdatedDate = DateTime.Now.AddHours(4);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}
		public async Task<IActionResult> Remove(int id)
		{
			Genre? genre = await _context.Genres
			   .Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();

			if (genre is null)
			{
				return NotFound();
			}
			genre.IsDeleted = true;
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));

		}
	}
}
