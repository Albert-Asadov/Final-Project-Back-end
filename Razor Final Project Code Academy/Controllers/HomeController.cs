using System;
using Razor_Final_Project_Code_Academy.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Razor_Final_Project_Code_Academy.Entities;
using Razor_Final_Project_Code_Academy.ViewModel;

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

            ViewBag.Comments = _context.Comments.OrderByDescending(x=>x.CreationTime).ToList();

            ViewBag.Slider = _context.Sliders.ToList();

            ViewBag.Watch = _context.Accessories.Include(x => x.AccessoryImages).Include(x => x.AccessoryCategories).Include(x => x.Brand).ToList();
            ViewBag.Phone = _context.Products.Include(x => x.ProductImages).Include(x => x.productCategories).Include(x => x.Brand).ToList();

            ViewBag.NewWatch = _context.Accessories.Include(x => x.AccessoryImages).Include(x => x.AccessoryCategories).Include(x => x.Brand).Include(x => x.Brand).OrderByDescending(x => x.Id).Take(3).ToList();
            ViewBag.NewPhone = _context.Products.Include(x => x.ProductImages).Include(x => x.productCategories).Include(x => x.Brand).OrderByDescending(x=>x.Id).Take(3).ToList();

            return View();
		}

        public IActionResult Search(string search)
        {
            var searchingProduct = _context.Products.Include(x => x.ProductImages).AsQueryable().Where(x => x.Name.Contains(search));
            var searchingAcccessory = _context.Accessories.Include(x => x.AccessoryImages).AsQueryable().Where(x => x.Name.Contains(search));

            List<Product> products = searchingProduct.OrderByDescending(x => x.Id).ToList();
            List<Accessory> accessories = searchingAcccessory.OrderByDescending(x => x.Id).ToList();

            HomeVM model = new()
            {
                accessories = accessories,

                products = products
            };


            return PartialView("_SearchPartial", model);
        }
	}
}

