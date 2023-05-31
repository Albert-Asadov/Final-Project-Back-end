using System;
using Microsoft.AspNetCore.Mvc;
using Razor_Final_Project_Code_Academy.DAL;
using Razor_Final_Project_Code_Academy.Entities;

namespace Razor_Final_Project_Code_Academy.Areas.RazorAdmin.Controllers
{
    [Area("RazorAdmin")]
    public class SettingController:Controller
	{
        private readonly RazorDbContext _context;

        public SettingController(RazorDbContext context)
		{
            _context = context;
        }

        public IActionResult Index()
        {
            IEnumerable<Setting> settings = _context.Settings.AsEnumerable();

            return View(settings);
        }

        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Setting CreatedSetting)
        {
            Setting setting = new()
            {
                Key = CreatedSetting.Key,
                Value = CreatedSetting.Value
            };

            _context.Settings.Add(setting);

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            if (id == 0) return NotFound();

            Setting setting = _context.Settings.FirstOrDefault(t => t.Id == id);

            if (setting is null) return BadRequest();

            return View(setting);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Edit(int id, Setting EditedSetting)
        {
            if (id != EditedSetting.Id) return BadRequest();

            Setting setting = _context.Settings.FirstOrDefault(t => t.Id == id);

            if (setting is null) return NotFound();

            bool duplicate = _context.Settings.Any(c => c.Key == EditedSetting.Key && c.Value == EditedSetting.Value);

            if (duplicate)
            {
                ModelState.AddModelError("", "the Tag is actually have");

                return View();
            }
            setting.Key = EditedSetting.Key;

            setting.Value = EditedSetting.Value;

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Detail(int id)
        {
            if (id == 0) return NotFound();

            Setting? setting = _context.Settings.FirstOrDefault(c => c.Id == id);

            if (setting is null)
            {
                return BadRequest();
            }

            return View(setting);
        }

        public IActionResult Delete(int id)
        {
            if (id == 0) return NotFound();

            Setting setting = _context.Settings.FirstOrDefault(t => t.Id == id);

            _context.Remove(setting);

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

    }
}

