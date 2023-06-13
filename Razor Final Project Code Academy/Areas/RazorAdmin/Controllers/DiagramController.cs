using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Razor_Final_Project_Code_Academy.DAL;
using Razor_Final_Project_Code_Academy.Entities;
using Razor_Final_Project_Code_Academy.ViewModel;

namespace Razor_Final_Project_Code_Academy.Areas.RazorAdmin.Controllers
{
    [Area("RazorAdmin")]
    public class DiagramController:Controller
	{
        private readonly RazorDbContext _context;

        public DiagramController(RazorDbContext context)
		{
            _context = context;
        }

        public IActionResult Index()
        {
            List<Product> products = _context.Products.ToList();
            List<Accessory> accessories = _context.Accessories.ToList();

            HomeVM model = new()
            {
                products = products,
                accessories = accessories
            };
            return View(model);
        }
    }
}

