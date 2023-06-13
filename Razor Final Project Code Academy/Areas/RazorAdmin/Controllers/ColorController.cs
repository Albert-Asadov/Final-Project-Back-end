using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Razor_Final_Project_Code_Academy.DAL;
using Razor_Final_Project_Code_Academy.Entities;
using Razor_Final_Project_Code_Academy.ViewModel.Roles;

namespace Razor_Final_Project_Code_Academy.Areas.RazorAdmin.Controllers
{
    [Area("RazorAdmin")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class ColorController:Controller
	{
        private readonly RazorDbContext _context;
        public ColorController(RazorDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int page =1)
        {
            ViewBag.TotalPage = Math.Ceiling((double)_context.Brands.Count() / 10);
            ViewBag.CurrentPage = page;
            IEnumerable<Color> color = _context.Colors.Skip((page - 1) * 10).Take(10).AsEnumerable();
            return View(color);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Color CreatedColor)
        {

            if (!ModelState.IsValid)
            {
                foreach (string message in ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))
                {
                    ModelState.AddModelError("", message);
                }
                return View();
            }
            bool Isdublicate = _context.Colors.Any(c => c.ColorName == CreatedColor.ColorName);

            if (Isdublicate)
            {
                ModelState.AddModelError("", "You cannot enter the same data again");
                return View();
            }
            _context.Colors.Add(CreatedColor);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Edit(int id)
        {
            if (id == 0) return NotFound();
            Color? color = _context.Colors.FirstOrDefault(s => s.Id == id);
            if (color is null) return NotFound();
            return View(color);
        }

        [HttpPost]
        public IActionResult Edit(int id, Color editedColor)
        {
            if (id != editedColor.Id) return NotFound();
            Color? color = _context.Colors.FirstOrDefault(s => s.Id == id);
            if (color is null) return NotFound();
            bool duplicate = _context.Colors.Any(s => s.ColorName == editedColor.ColorName && color.ColorName != editedColor.ColorName);
            if (duplicate)
            {
                ModelState.AddModelError("Name", "This Ram name is now available");
                return View();
            }
            color.ColorName = editedColor.ColorName;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Detail(int id)
        {
            if (id == 0) return NotFound();
            Color? color = _context.Colors.FirstOrDefault(s => s.Id == id);
            return color is null ? BadRequest() : View(color);
        }



        public IActionResult Delete(int id)
        {
            if (id == 0) return NotFound();
            Color? color = _context.Colors.FirstOrDefault(s => s.Id == id);
            if (color is null) return NotFound();
            _context.Colors.Remove(color);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}

