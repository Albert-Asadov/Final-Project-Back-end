using System;
using Razor_Final_Project_Code_Academy.DAL;
using Razor_Final_Project_Code_Academy.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

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
            ViewBag.Ram = _context.Rams.ToList();
            ViewBag.Memory = _context.Memories.ToList();
            ViewBag.Color = _context.Colors.ToList();
            ViewBag.Category = _context.Categories.ToList();
            List<Product> products = _context.Products.Include(x => x.ProductImages).Include(x => x.productCategories).Include(x=>x.Brand).ToList();
			return View(products);
		}

		public IActionResult DetailPhone(int id)
		{
			if (id == 0) return BadRequest();
            Product? products = _context.Products.Include(x => x.ProductImages).Include(x => x.productCategories).Include(x=>x.ProductComments).Include(x => x.Brand).FirstOrDefault(x=>x.Id == id);
			if (products is null) return NotFound();
            return View(products);
		}

        public IActionResult DetailAccessory(int id)
        {
            if (id == 0) return BadRequest();
            Accessory? accessory = _context.Accessories.Include(x => x.AccessoryImages).Include(x => x.AccessoryCategories).Include(x=>x.AccessoryComments).Include(x => x.Brand).FirstOrDefault(x => x.Id == id);
            if (accessory is null) return NotFound();
            return View(accessory);
        }

        [HttpPost]
        public async Task<IActionResult> AddCommentProduct(Comment comment, int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                Product? product = await _context.Products.Include(dt => dt.ProductComments).FirstOrDefaultAsync(p => p.Id == id);

                Comment newcomment = new Comment()
                {

                    Title = comment.Title,
                    Text = comment.Text,
                    CreationTime = DateTime.UtcNow,
                    Product = product,
                    Name = comment.Name,
                    Email = comment.Email

                };
                product.ProductComments.Add(newcomment);
                await _context.Comments.AddAsync(newcomment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(DetailPhone), new { id });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddCommentAccessory(Comment comment, int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                Accessory? accessory = await _context.Accessories.Include(dt => dt.AccessoryComments).FirstOrDefaultAsync(p => p.Id == id);

                Comment newcomment = new Comment()
                {

                    Title = comment.Title,
                    Text = comment.Text,
                    CreationTime = DateTime.UtcNow,
                    Accessory = accessory,
                    Name = comment.Name,
                    Email = comment.Email

                };
                accessory.AccessoryComments.Add(newcomment);
                await _context.Comments.AddAsync(newcomment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(DetailAccessory), new { id });
            }
        }

        public IActionResult WishList()
		{
			return View();
		}


	}
}

