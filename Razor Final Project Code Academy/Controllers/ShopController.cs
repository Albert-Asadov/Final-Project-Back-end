﻿using System;
using Razor_Final_Project_Code_Academy.DAL;
using Razor_Final_Project_Code_Academy.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Razor_Final_Project_Code_Academy.ViewModel;
using System.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Final_Project_Razor.Controllers
{
	public class ShopController:Controller
	{
        private readonly RazorDbContext _context;
        private readonly UserManager<User> _userManager;

        public ShopController(RazorDbContext context,UserManager<User> userManager)
		{
            _context = context;
            _userManager = userManager;
        }

     
        public IActionResult Index(int page = 1)
        {
            ViewBag.Ram = _context.Rams.ToList();
            ViewBag.Memory = _context.Memories.ToList();
            ViewBag.Color = _context.Colors.ToList();
            ViewBag.Brand = _context.Brands.ToList();
            ViewBag.Category = _context.Categories.ToList();

            int pageSize = 6;
            int totalProductsCount = _context.Products.Count() + _context.Accessories.Count();
            int totalPages = (int)Math.Ceiling((double)totalProductsCount / pageSize);
            ViewBag.TotalPage = totalPages;
            ViewBag.CurrentPage = page;

            List<Product> products = _context.Products
                .Include(x => x.ProductImages)
                .Include(x => x.productCategories)
                .Include(x => x.Brand)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            List<Accessory> accessories = _context.Accessories
                .Include(x => x.AccessoryImages)
                .Include(x => x.AccessoryCategories)
                .Include(x => x.Brand)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            MyCombineModelsVM model = new()
            {
                Products = products,
                Accessories = accessories
            };

            return View(model);
        }

        public async Task<IActionResult> DetailPhone(int id)
        {
            ViewBag.Ram = _context.ProductRamMemories
                             .Where(prm => prm.ProductId == id)
                             .Select(prm => prm.Ram)
                             .Distinct()
                             .Select(r => new { r.Id, r.RamName })
                             .ToList();

            ViewBag.Memory = _context.ProductRamMemories
                             .Where(prm => prm.ProductId == id)
                             .Select(prm => prm.Memory)
                             .Distinct()
                             .Select(m => new { m.Id, m.MemoryName })
                             .ToList();

            ViewBag.category = _context.Categories.ToList();
            User? user = new();

            if (User.Identity.IsAuthenticated)
            {
                user = await _userManager.FindByNameAsync(User.Identity.Name);

                ViewBag.WishList = _context.Wishlists.Include(x => x.Product).Include(x => x.User).Where(x => x.UserId == user.Id && x.IsAccessory == false).ToList();
            }
            if (id == 0) return BadRequest();
            Product? products = _context.Products.Include(x => x.ProductImages).Include(x => x.productCategories).Include(x => x.ProductComments).Include(x => x.Brand).Include(x=>x.ProductRamMemories).FirstOrDefault(x => x.Id == id);
            ViewBag.Accessory = _context.Accessories.Include(x => x.AccessoryImages).Include(x => x.AccessoryCategories).Include(x => x.Brand).ToList();
            ViewBag.Product = _context.Products.Include(x => x.ProductImages).Include(x => x.productCategories).Include(x => x.Brand).ToList();
            if (products is null) return NotFound();
            return View(products);
        }

        public async Task<IActionResult> DetailAccessory(int id)
        {
            ViewBag.Color = _context.AccessoryColors
                            .Where(ac => ac.AccessoryId == id)
                            .Select(ac => ac.Color)
                            .Distinct()
                            .Select(c => new { c.Id, c.ColorName })
                            .ToList();
            ViewBag.category = _context.Categories.ToList();

            User? user = new();

            if (User.Identity.IsAuthenticated)
            {
                user = await _userManager.FindByNameAsync(User.Identity.Name);

                ViewBag.WishList = _context.Wishlists.Include(x => x.Accessory).Include(x => x.User).Where(x => x.UserId == user.Id && x.IsAccessory == true).ToList();
            }
            if (id == 0) return BadRequest();
            Accessory? accessory = _context.Accessories.Include(x => x.AccessoryImages).Include(x => x.AccessoryCategories).Include(x => x.AccessoryComments).Include(x => x.Brand).Include(x=>x.accessoryColors).FirstOrDefault(x => x.Id == id);
            ViewBag.Accessory = _context.Accessories.Include(x => x.AccessoryImages).Include(x => x.AccessoryCategories).Include(x => x.Brand).ToList();
            ViewBag.Product = _context.Products.Include(x => x.ProductImages).Include(x => x.productCategories).Include(x => x.Brand).ToList();

            if (accessory is null) return NotFound();
            return View(accessory);
        }

        public async Task<IActionResult> AddCommentProduct(Comment comment, int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                Product? product = await _context.Products.Include(dt => dt.ProductComments).OrderByDescending(x=>x.Id).FirstOrDefaultAsync(p => p.Id == id);

                Comment newcomment = new Comment()
                {
                    Title = comment.Title,
                    Text = comment.Text,
                    CreationTime = DateTime.UtcNow,
                    Product = product,
                    Name = comment.Name,
                    Email = comment.Email

                };
                product?.ProductComments.Add(newcomment);
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
                Accessory? accessory = await _context.Accessories.Include(dt => dt.AccessoryComments).OrderByDescending(x => x.Id).FirstOrDefaultAsync(p => p.Id == id);

                Comment newcomment = new Comment()
                {
                    Title = comment.Title,
                    Text = comment.Text,
                    CreationTime = DateTime.UtcNow,
                    Accessory = accessory,
                    Name = comment.Name,
                    Email = comment.Email
                };
                accessory?.AccessoryComments.Add(newcomment);
                await _context.Comments.AddAsync(newcomment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(DetailAccessory), new { id });
            }
        }
        [HttpPost]
        public IActionResult Index(int[] CategoryIds, int[] BrandIds, int[] RamIds, int[] MemoryIds, int[] ColorIds, int page = 1)
        {
            ViewBag.Ram = _context.Rams.ToList();
            ViewBag.Memory = _context.Memories.ToList();
            ViewBag.Color = _context.Colors.ToList();
            ViewBag.Category = _context.Categories.ToList();
            ViewBag.Brand = _context.Brands.ToList();

            IQueryable<Product> products = _context.Products
                .Include(x => x.ProductImages)
                .Include(x => x.productCategories)
                .ThenInclude(x => x.Category)
                .Include(p => p.ProductRamMemories).ThenInclude(x => x.Ram)
                .Include(p => p.ProductRamMemories).ThenInclude(x => x.Memory)
                .Include(x => x.Brand);

            IQueryable<Accessory> accessories = _context.Accessories
                .Include(x => x.AccessoryImages)
                .Include(p => p.accessoryColors).ThenInclude(x => x.Color)
                .Include(x => x.AccessoryCategories)
                .Include(x => x.Brand);

            if (CategoryIds.Length > 0)
            {
                products = products.Where(p => p.productCategories.Any(pc => CategoryIds.Contains(pc.CategoryId)));
                accessories = accessories.Where(p => p.AccessoryCategories.Any(pc => CategoryIds.Contains(pc.CategoryId)));
            }
  
            if (BrandIds.Length > 0)
            {
                products = products.Where(p => BrandIds.Any(x => x.Equals(p.BrandId)));
                accessories = accessories.Where(p => BrandIds.Any(x => x.Equals(p.BrandId)));
            }
            if (RamIds.Length > 0)
            {
                products = products.Where(p => p.ProductRamMemories.Any(pc => RamIds.Contains(pc.RamId)));
            }
            if (MemoryIds.Length > 0)
            {
                products = products.Where(p => p.ProductRamMemories.Any(pc => MemoryIds.Contains(pc.MemoryId)));
            }
            if (ColorIds.Length > 0)
            {
                accessories = accessories.Where(p => p.accessoryColors.Any(pc => ColorIds.Contains(pc.ColorId)));
            }

            List<Product> filteredProducts = products.ToList();
            List<Accessory> filteredAccessories = accessories.ToList();

            MyCombineModelsVM model = new MyCombineModelsVM()
            {
                Products = filteredProducts,
                Accessories = filteredAccessories
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Sort(string sortOrder)
        {
            var products = _context.Products.ToList();
            var accessories = _context.Accessories.ToList();

          
            switch (sortOrder)
            {
                case "A-Z":
                    products = products.OrderBy(p => p.Name).ToList();
                    accessories = accessories.OrderBy(a => a.Name).ToList();
                    break;
                case "Z-A":
                    products = products.OrderByDescending(p => p.Name).ToList();
                    accessories = accessories.OrderByDescending(a => a.Name).ToList();
                    break;
                case "PriceAscending":
                    products = products.OrderBy(p => p.Price).ToList();
                    accessories = accessories.OrderBy(a => a.Price).ToList();
                    break;
                case "PriceDescending":
                    products = products.OrderByDescending(p => p.Price).ToList();
                    accessories = accessories.OrderByDescending(a => a.Price).ToList();
                    break;
                default:
                    // По умолчанию не выполняем сортировку
                    break;
            }

            var model = new Tuple<List<Product>, List<Accessory>>(products, accessories);

            return Json( model);
        }



    }
}

