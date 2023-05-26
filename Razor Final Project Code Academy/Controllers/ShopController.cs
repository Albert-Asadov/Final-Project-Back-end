using System;
using Final_Project_Razor.DAL;
using Final_Project_Razor.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Final_Project_Razor.Controllers
{
	public class ShopController:Controller
	{
        private readonly RazorDbContext _context;

        public ShopController(RazorDbContext context)
		{
            _context = context;
        }

		public IActionResult Index()
		{
			List<Product> products = _context.Products.Include(x => x.ProductImages).Include(x => x.productCategories).Include(x=>x.Brand).ToList();
			return View(products);
		}

		public IActionResult Detail(int id)
		{
			if (id == 0) return BadRequest();
            Product? products = _context.Products.Include(x => x.ProductImages).Include(x => x.productCategories).Include(x => x.Brand).FirstOrDefault(x=>x.Id == id);
			if (products is null) return NotFound();
            return View(products);
		}

		public IActionResult WishList()
		{
			return View();
		}


	}
}

