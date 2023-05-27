using System;
using Razor_Final_Project_Code_Academy.Entities;
using Microsoft.AspNetCore.Mvc;
using Razor_Final_Project_Code_Academy.ViewModel;
using Razor_Final_Project_Code_Academy.DAL;

namespace Razor_Final_Project_Code_Academy.Areas.RazorAdmin.Controllers
{
    [Area("RazorAdmin")]
    public class CategoryController : Controller
    {
        private readonly RazorDbContext _context;

        public CategoryController(RazorDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            IEnumerable<Category> categories = _context.Categories.AsEnumerable();

            return View(categories);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CategoryVM CreatedCategory)
        {
            if (!ModelState.IsValid)
            {

                foreach (string message in ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))
                {
                    ModelState.AddModelError("", message);
                }

                return View();
            }
            bool isDuplicated = _context.Categories.Any(c => c.Name == CreatedCategory.Name);
            if (isDuplicated)
            {
                ModelState.AddModelError("", "You cannot duplicate value");
                return View();
            }
            Category cat = new()
            {
                Name = CreatedCategory.Name
            };
            _context.Categories.Add(cat);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }


        public IActionResult Edit(int id)
        {
            if (id == 0) return NotFound();

            Category? category = _context.Categories.FirstOrDefault(c => c.Id == id);

            if (category is null)
            {
                return BadRequest();
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Category EditedCategory)
        {
            if (id == 0) return NotFound();
            Category? category = _context.Categories.FirstOrDefault(c => c.Id == id);

            if (category is null)
            {
                return BadRequest();
            }

            bool ContainsName = _context.Categories.Any(c => c.Name == EditedCategory.Name);

            if (ContainsName == true)
            {
                ModelState.AddModelError("", "You Have This category Name");

                return View();
            }

            category.Name = EditedCategory.Name;

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Detail(int id)
        {
            if (id == 0) return NotFound();

            Category? category = _context.Categories.FirstOrDefault(c => c.Id == id);

            if (category is null)
            {
                return BadRequest();
            }

            return View(category);
        }

        public IActionResult Delete(int id)
        {
            if (id == 0) return NotFound();

            Category? category = _context.Categories.FirstOrDefault(c => c.Id == id);

            _context.Remove(category);

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}

