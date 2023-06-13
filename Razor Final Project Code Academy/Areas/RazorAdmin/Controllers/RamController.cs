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
    public class RamController:Controller
	{
        private readonly RazorDbContext _context;
        public RamController(RazorDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int page =1)
        {
            ViewBag.TotalPage = Math.Ceiling((double)_context.Brands.Count() / 10);
            ViewBag.CurrentPage = page;
            IEnumerable<Ram> ram = _context.Rams.Skip((page - 1) * 10).Take(10).AsEnumerable();
            return View(ram);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Ram CreatedRam)
        {

            if (!ModelState.IsValid)
            {
                foreach (string message in ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))
                {
                    ModelState.AddModelError("", message);
                }
                return View();
            }
            bool Isdublicate = _context.Rams.Any(c => c.RamName == CreatedRam.RamName);

            if (Isdublicate)
            {
                ModelState.AddModelError("", "You cannot enter the same data again");
                return View();
            }
            _context.Rams.Add(CreatedRam);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            if (id == 0) return NotFound();
            Ram? ram = _context.Rams.FirstOrDefault(s => s.Id == id);
            if (ram is null) return NotFound();
            return View(ram);
        }

        [HttpPost]
        public IActionResult Edit(int id, Ram editedRam)
        {
            if (id != editedRam.Id) return NotFound();
            Ram? ram = _context.Rams.FirstOrDefault(s => s.Id == id);
            if (ram is null) return NotFound();
            bool duplicate = _context.Rams.Any(s => s.RamName == editedRam.RamName && ram.RamName != editedRam.RamName);
            if (duplicate)
            {
                ModelState.AddModelError("Name", "This Ram name is now available");
                return View();
            }
            ram.RamName = editedRam.RamName;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Detail(int id)
        {
            if (id == 0) return NotFound();
            Ram? ram = _context.Rams.FirstOrDefault(s => s.Id == id);
            return ram is null ? BadRequest() : View(ram);
        }



        public IActionResult Delete(int id)
        {
            if (id == 0) return NotFound();
            Ram? ram = _context.Rams.FirstOrDefault(s => s.Id == id);
            if (ram is null) return NotFound();
            _context.Rams.Remove(ram);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}


