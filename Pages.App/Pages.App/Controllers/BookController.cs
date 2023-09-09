﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pages.App.Context;
using Pages.App.ViewModels;
using Pages.Core.Entities;

namespace Pages.App.Controllers
{
    public class BookController : Controller
    {
        private readonly PagesDbContext _context;

        public BookController(PagesDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int? id, int page = 1, string? search = null)
        {
            int TotalCount = _context.Books.Where(x => !x.IsDeleted && x.Name.Contains(search)).Count();
            ViewBag.TotalPage = (int)Math.Ceiling((decimal)TotalCount / 4);
            ViewBag.CurrentPage = page;

            ViewBag.Language = new SelectList(_context.Languages.Where(x => x.Id == id && !x.IsDeleted).ToList(), "Id", "Name");
            ViewBag.Genre = new SelectList(_context.Genres.Where(x => x.Id == id && !x.IsDeleted).ToList(), "Id", "Name");
            ViewBag.Author = new SelectList(_context.Authors.Where(x => x.Id == id && !x.IsDeleted).ToList(), "Id", "Name");
            ViewBag.Category = new SelectList(_context.Categories.Where(x => x.Id == id && !x.IsDeleted).ToList(), "Id", "Name");

            if (id == null)
            {
                IEnumerable<Book> books = await _context.Books.Where(x => !x.IsDeleted)
                     .Include(x => x.BookAuthors)
                     .ThenInclude(x => x.Author)
                     .Include(x => x.BookLanguages)
                     .ThenInclude(x => x.Language)
                     .Include(x=>x.BookAuthors)
                     .ThenInclude(x=>x.Author)
                    .Include(x => x.Genre)
                    .Skip((page - 1) * 12)
            .Take(12).ToListAsync();
                return View(books);
            }
            else
            {
                IEnumerable<Book> books = await _context.Books.Where(x => x.Id == id && !x.IsDeleted).Skip((page - 1) * 12)
           .Take(12).ToListAsync();
                return View(books);
            }
        }
        public async Task<IActionResult> Search(string search, int page = 1)
        {
            int TotalCount = _context.Books.Where(x => !x.IsDeleted && x.Name.Trim().ToLower().Contains(search.Trim().ToLower())).Count();
            ViewBag.TotalPage = (int)Math.Ceiling((decimal)TotalCount / 3);
            ViewBag.CurrentPage = page;
            List<Book> books = await _context.Books.Where(x => !x.IsDeleted && x.Name.Trim().ToLower().Contains(search.Trim().ToLower()))
                     .Include(x => x.BookLanguages)
                     .ThenInclude(x => x.Language)
                    .Skip((page - 1) * 3).Take(3)
                .ToListAsync();
            return Json(books);
        }


        public async Task<IActionResult> Detail(int id)
        {

            ViewBag.Books = await _context.Books.Where(x => x.Id == id &&!x.IsDeleted)
                      .Include(x => x.BookAuthors)
                     .ThenInclude(x => x.Author)
                     .Include(x => x.BookLanguages)
                     .ThenInclude(x => x.Language)
                       .Include(x => x.BookAuthors)
                     .ThenInclude(x => x.Author)
                    .Include(x => x.Genre)
                     .Take(3)
                    .ToListAsync();
            Book? book = await _context.Books.Where(x => x.Id == id && !x.IsDeleted)
                   .Include(x => x.BookAuthors)
                     .ThenInclude(x => x.Author)
                     .Include(x => x.BookLanguages)
                     .ThenInclude(x => x.Language)
                       .Include(x => x.BookAuthors)
                     .ThenInclude(x => x.Author)
                    .Include(x => x.Genre)
                    .FirstOrDefaultAsync();

            if (book is null)
            {
                return NotFound();
            }
            BookVM bookVM = new BookVM
            {
                Book = book
            };

            return View(bookVM);
        }

    }
}

