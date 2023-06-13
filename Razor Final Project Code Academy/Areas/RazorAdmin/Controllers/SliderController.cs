using System;
using Razor_Final_Project_Code_Academy.DAL;
using Microsoft.AspNetCore.Mvc;
using Razor_Final_Project_Code_Academy.Entities;
using Razor_Final_Project_Code_Academy.Utilities.ExtensionMethods;
using Microsoft.AspNetCore.Authorization;
using Razor_Final_Project_Code_Academy.ViewModel.Roles;

namespace Razor_Final_Project_Code_Academy.Areas.RazorAdmin.Controllers
{
    [Area("RazorAdmin")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class SliderController:Controller
	{
        private readonly RazorDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SliderController(RazorDbContext context, IWebHostEnvironment env)
		{
            _context = context;
            _env = env;
        }

        public IActionResult Index(int page =1)
        {
            ViewBag.TotalPage = Math.Ceiling((double)_context.Brands.Count() / 10);
            ViewBag.CurrentPage = page;
            IEnumerable<Slider> slider = _context.Sliders.Skip((page - 1) * 10).Take(10).AsEnumerable();
            return View(slider);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(Slider CreatedSlider)
        {
            if (CreatedSlider.Image is null)
            {
                ModelState.AddModelError("image", "Choose image");
                return View();
            }
            if (!CreatedSlider.Image.IsValidFile("image/"))
            {
                ModelState.AddModelError("Image", "Choose image type file");
                return View();
            }
            if (!CreatedSlider.Image.IsValidLength(1))
            {
                ModelState.AddModelError("Image", "Max Size 1MB");
                return View();
            }
            string imagesFolderPath = Path.Combine("assets", "images","SliderImages");

            CreatedSlider.Path = await CreatedSlider.Image.CreateImage(_env.WebRootPath, imagesFolderPath);

            _context.Sliders.Add(CreatedSlider);

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            if (id == 0) return NotFound();
            Slider? slider = _context.Sliders.FirstOrDefault(s => s.Id == id);
            if (slider is null)
            {
                return NotFound();
            }
            return View(slider);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Slider EditedSLider)
        {
            if (id == 0) return NotFound();
            Slider? slider = _context.Sliders.FirstOrDefault(s => s.Id == id);
            if (!ModelState.IsValid)
            {
                return View();
            }

            _context.Entry(slider).CurrentValues.SetValues(EditedSLider);

            if (EditedSLider.Image is not null)
            {
                string imagesFolderPath = Path.Combine("assets", "images", "SliderImages");

                string FullPath = Path.Combine(_env.WebRootPath, imagesFolderPath, slider.Path);

                Files.DeleteImage(FullPath);

                slider.Path = await EditedSLider.Image.CreateImage(_env.WebRootPath, imagesFolderPath);

            }
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Detail(int id)
        {
            if (id == 0) return NotFound();

            Slider? slider = _context.Sliders.FirstOrDefault(s => s.Id == id);

            if (slider is null)
            {
                return BadRequest();
            }

            return View(slider);
        }

        public async Task<IActionResult> Delete(int id, Slider edited)
        {
            if (id == 0) return NotFound();

            Slider? slider = _context.Sliders.FirstOrDefault(s => s.Id == id);

            if (edited.Image is null)
            {
                string imagesFolderPath = Path.Combine("assets", "images", "SliderImages");

                string FullPath = Path.Combine(_env.WebRootPath, imagesFolderPath, slider.Path);

                Files.DeleteImage(FullPath);
            }

            _context.Sliders.Remove(slider);

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
    
}

