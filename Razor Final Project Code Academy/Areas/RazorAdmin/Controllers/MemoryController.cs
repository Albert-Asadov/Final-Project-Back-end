using System;
using System.Drawing;
using Microsoft.AspNetCore.Mvc;
using Razor_Final_Project_Code_Academy.DAL;
using Razor_Final_Project_Code_Academy.Entities;

namespace Razor_Final_Project_Code_Academy.Areas.RazorAdmin.Controllers
{
    [Area("RazorAdmin")]
    public class MemoryController : Controller
    {
        private readonly RazorDbContext _context;
        public MemoryController(RazorDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            IEnumerable<Memory> memory = _context.Memories.AsEnumerable();
            return View(memory);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Memory CreatedMemory)
        {

            if (!ModelState.IsValid)
            {
                foreach (string message in ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))
                {
                    ModelState.AddModelError("", message);
                }
                return View();
            }
            bool Isdublicate = _context.Memories.Any(c => c.MemoryName == CreatedMemory.MemoryName);

            if (Isdublicate)
            {
                ModelState.AddModelError("", "You cannot enter the same data again");
                return View();
            }
            _context.Memories.Add(CreatedMemory);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            if (id == 0) return NotFound();
            Memory? memory = _context.Memories.FirstOrDefault(s => s.Id == id);
            if (memory is null) return NotFound();
            return View(memory);
        }

        [HttpPost]
        public IActionResult Edit(int id, Memory editedMemory)
        {
            if (id != editedMemory.Id) return NotFound();
            Memory? memory = _context.Memories.FirstOrDefault(s => s.Id == id);
            if (memory is null) return NotFound();
            bool duplicate = _context.Memories.Any(s => s.MemoryName == editedMemory.MemoryName && memory.MemoryName != editedMemory.MemoryName);
            if (duplicate)
            {
                ModelState.AddModelError("Name", "This Memory name is now available");
                return View();
            }
            memory.MemoryName = editedMemory.MemoryName;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Detail(int id)
        {
            if (id == 0) return NotFound();
            Memory? memory = _context.Memories.FirstOrDefault(s => s.Id == id);
            return memory is null ? BadRequest() : View(memory);
        }



        public IActionResult Delete(int id)
        {
            if (id == 0) return NotFound();
            Memory? memory = _context.Memories.FirstOrDefault(s => s.Id == id);
            if (memory is null) return NotFound();
            _context.Memories.Remove(memory);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}

