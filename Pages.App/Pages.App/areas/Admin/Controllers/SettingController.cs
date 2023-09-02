using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
	public class SettingController : Controller
	{
		private readonly PagesDbContext _context;
		private readonly IWebHostEnvironment _env;

		public SettingController(PagesDbContext context, IWebHostEnvironment env)
		{
			_context = context;
			_env = env;
		}

		public async Task<IActionResult> Index(int page = 1)
		{
			int TotalCount = _context.Languages.Where(x => !x.IsDeleted).Count();
			ViewBag.TotalPage = (int)Math.Ceiling((decimal)TotalCount / 5);
			ViewBag.CurrentPage = page;

			IEnumerable<Setting> settings = await _context.Settings
				.Where(x => !x.IsDeleted).Skip((page - 1) * 5).Take(5).ToListAsync();
			return View(settings);
		}
		[HttpGet]
		public async Task<IActionResult> Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(Setting setting)
		{
			if (!ModelState.IsValid)
			{
				return View(setting);
			}
			if (setting.file is null)
			{
				ModelState.AddModelError("file", "Image must be added");
				return View(setting);
			}
			if (!Helper.IsImage(setting.file))
			{
				ModelState.AddModelError("file", "File must be image");
				return View(setting);
			}
			if (!Helper.IsSizeOk(setting.file, 1))
			{
				ModelState.AddModelError("file", "Size of Image must less than 1 mb!!!");
				return View(setting);
			}
			setting.WhatLearImage = setting.file.CreateImage(_env.WebRootPath, "assets/img");
			setting.CopyImage = setting.file.CreateImage(_env.WebRootPath, "assets/img");
			setting.Logo = setting.file.CreateImage(_env.WebRootPath, "assets/img");
			setting.CreatedDate = DateTime.Now;
			await _context.AddAsync(setting);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		[HttpGet]
		public async Task<IActionResult> Update(int id)
		{
			Setting? setting = await _context.Settings.Where(x => x.Id == id & !x.IsDeleted).FirstOrDefaultAsync();
			if(setting == null)
			{
				return NotFound();
			}
			return View(setting);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Update(int id, Setting setting)
		{
			Setting? updatedsetting = await _context.Settings.Where(x => x.Id == id & !x.IsDeleted).FirstOrDefaultAsync();
			if (setting == null)
			{
				return View(setting);
			}
			if (!ModelState.IsValid)
			{
				return View(updatedsetting);
			}
			if(setting.file is not null)
			{
				if (!Helper.IsImage(setting.file))
				{
					ModelState.AddModelError("file", "File must be image");
					return View(setting);
				}
				if (!Helper.IsSizeOk(setting.file, 1))
				{
					ModelState.AddModelError("file", "Size of Image must less than 1 mb!!!");
					return View(setting);
				}
				Helper.RemoveImage(_env.WebRootPath, "assets/img/", updatedsetting.WhatLearImage);
				updatedsetting.WhatLearImage = setting.file.CreateImage(_env.WebRootPath, "assets/img/");
				updatedsetting.CopyImage = setting.file.CreateImage(_env.WebRootPath, "assets/img/");
			}

			updatedsetting.Address = setting.Address;
			updatedsetting.Mail = setting.Mail;
			updatedsetting.Phone = setting.Phone;
			updatedsetting.CopyText = setting.CopyText;
			updatedsetting.Logo = setting.Logo;
			updatedsetting.UpdatedDate = DateTime.Now;
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		public async Task<IActionResult> Remove(int id)
		{
			Setting? setting = await _context.Settings.Where(x => x.Id == id & !x.IsDeleted).FirstOrDefaultAsync();
			if (setting == null)
			{
				return NotFound();
			}
			setting.IsDeleted = true;
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}
	}
}
