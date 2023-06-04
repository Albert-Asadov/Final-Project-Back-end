using System;
using Razor_Final_Project_Code_Academy.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Final_Project_Razor.Controllers
{
	public class HomeController:Controller
	{
        private readonly RazorDbContext _context;

        public HomeController(RazorDbContext context)
		{
            _context = context;
        }

		public IActionResult Index()
		{

            ViewBag.Slider = _context.Sliders.ToList();

            ViewBag.Watch = _context.Accessories.Include(x => x.AccessoryImages).Include(x => x.AccessoryCategories).Include(x => x.Brand).ToList();
            ViewBag.Phone = _context.Products.Include(x => x.ProductImages).Include(x => x.productCategories).Include(x => x.Brand).ToList();

            ViewBag.NewWatch = _context.Accessories.Include(x => x.AccessoryImages).Include(x => x.AccessoryCategories).Include(x => x.Brand).Include(x => x.Brand).OrderByDescending(x => x.Id).Take(3).ToList();
            ViewBag.NewPhone = _context.Products.Include(x => x.ProductImages).Include(x => x.productCategories).Include(x => x.Brand).OrderByDescending(x=>x.Id).Take(3).ToList();

            return View();
		}
	}
}

