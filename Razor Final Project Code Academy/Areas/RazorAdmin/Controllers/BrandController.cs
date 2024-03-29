﻿using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Razor_Final_Project_Code_Academy.DAL;
using Razor_Final_Project_Code_Academy.Entities;
using Razor_Final_Project_Code_Academy.ViewModel;
using Razor_Final_Project_Code_Academy.ViewModel.Roles;

namespace Razor_Final_Project_Code_Academy.Areas.RazorAdmin.Controllers
{
    [Area("RazorAdmin")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class BrandController:Controller
	{
        private readonly RazorDbContext _context;

        public BrandController(RazorDbContext context)
		{
            _context = context;
        }

        public IActionResult Index(int page = 1)
        {
            ViewBag.TotalPage = Math.Ceiling((double)_context.Brands.Count() / 10);
            ViewBag.CurrentPage = page;
            IEnumerable<Brand> brands = _context.Brands.Skip((page - 1) * 10).Take(10).AsEnumerable();

            return View(brands);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(BrandVM CreatedBrands)
        {
            if (!ModelState.IsValid)
            {

                foreach (string message in ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))
                {
                    ModelState.AddModelError("", message);
                }

                return View();
            }
            bool isDuplicated = _context.Brands.Any(c => c.Name == CreatedBrands.Name);
            if (isDuplicated)
            {
                ModelState.AddModelError("", "You cannot duplicate value");
                return View();
            }
            Brand brand = new()
            {
                Name = CreatedBrands.Name
            };
            _context.Brands.Add(brand);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            if (id == 0) return NotFound();

            Brand? brand = _context.Brands.FirstOrDefault(c => c.Id == id);

            if (brand is null)
            {
                return BadRequest();
            }
            return View(brand);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Brand EditedBrand)
        {
            if (id == 0) return NotFound();
            Brand? brand = _context.Brands.FirstOrDefault(c => c.Id == id);

            if (brand is null)
            {
                return BadRequest();
            }

            bool ContainsName = _context.Brands.Any(c => c.Name == EditedBrand.Name);

            if (ContainsName == true)
            {
                ModelState.AddModelError("", "You Have This category Name");

                return View();
            }

            brand.Name = EditedBrand.Name;

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Detail(int id)
        {
            if (id == 0) return NotFound();

            Brand? brand = _context.Brands.FirstOrDefault(c => c.Id == id);

            if (brand is null)
            {
                return BadRequest();
            }

            return View(brand);
        }

        public IActionResult Delete(int id)
        {
            if (id == 0) return NotFound();

            Brand? brand = _context.Brands.FirstOrDefault(c => c.Id == id);

            _context.Remove(brand);

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}

