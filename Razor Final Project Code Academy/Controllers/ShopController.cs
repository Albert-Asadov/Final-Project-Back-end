using System;
using Razor_Final_Project_Code_Academy.DAL;
using Razor_Final_Project_Code_Academy.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Razor_Final_Project_Code_Academy.ViewModel;
using System.Linq;

namespace Final_Project_Razor.Controllers
{
	public class ShopController:Controller
	{
        private readonly RazorDbContext _context;

        public ShopController(RazorDbContext context)
		{
            _context = context;
        }

		public IActionResult Index(int page=1)
		{
            //ViewBag.Ram = _context.Rams.ToList();
            //ViewBag.Memory = _context.Memories.ToList();
            //ViewBag.Color = _context.Colors.ToList();
            //ViewBag.Category = _context.Categories.ToList();
            //List<Accessory> accessories = _context.Accessories.Include(x=>x.AccessoryImages).Include(x=>x.AccessoryCategories).Include(x=>x.Brand).Skip((page - 1) * 5).Take(5).AsEnumerable().ToList();
            //List<Product> products = _context.Products.Include(x => x.ProductImages).Include(x => x.productCategories).Include(x=>x.Brand).Skip((page - 1) * 5).Take(5).AsEnumerable().ToList();
            //return View(new Tuple<List<Product>, List<Accessory>>(products, accessories));

            /////

            //ViewBag.TotalPage = Math.Ceiling((double)_context.Products.Count() / 8);
            //ViewBag.TotalPage = Math.Ceiling((double)_context.Accessories.Count() / 8);

            //ViewBag.CurrentPage = page;
            //var rams = _context.Rams.ToList();
            //var memories = _context.Memories.ToList();
            //var colors = _context.Colors.ToList();
            //var categories = _context.Categories.ToList();

            //var query = _context.Accessories
            //    .Include(x => x.AccessoryImages)
            //    .Include(x => x.AccessoryCategories)
            //    .Include(x => x.Brand)
            //    .Skip((page - 1) * 8)
            //    .Take(5);

            //var accessories = query.ToList();
            //var products = query.Join(_context.Products,
            //    a => a.Id,
            //    p => p.Id,
            //    (a, p) => p)
            //    .Include(x => x.ProductImages)
            //    .Include(x => x.productCategories)
            //    .Include(x => x.Brand)
            //    .ToList();

            //var model = new Tuple<List<Product>, List<Accessory>>(products, accessories);
            //ViewBag.Ram = rams;
            //ViewBag.Memory = memories;
            //ViewBag.Color = colors;
            //ViewBag.Category = categories;

            //return View(model);

            /////
            
            var pageSize = 8;

            var totalProductCount = _context.Products.Count();
            var totalAccessoryCount = _context.Accessories.Count();

            var totalProductPages = (int)Math.Ceiling((double)totalProductCount / pageSize);
            var totalAccessoryPages = (int)Math.Ceiling((double)totalAccessoryCount / pageSize);

            ViewBag.TotalPage = Math.Max(totalProductPages, totalAccessoryPages);

            ViewBag.CurrentPage = page;
            var brands = _context.Brands.ToList();
            var rams = _context.Rams.ToList();
            var memories = _context.Memories.ToList();
            var colors = _context.Colors.ToList();
            var categories = _context.Categories.ToList();

            var query = _context.Accessories
                .Include(x => x.AccessoryImages)
                .Include(x => x.AccessoryCategories)
                .Include(x => x.Brand)
                .Skip((page - 1) * pageSize)
                .Take(5);

            var accessories = query.ToList();

            var products = query.Join(_context.Products,
                a => a.Id,
                p => p.Id,
                (a, p) => p)
                .Include(x => x.ProductImages)
                .Include(x => x.productCategories)
                .Include(x => x.Brand)
                .ToList();

            var model = new Tuple<List<Product>, List<Accessory>>(products, accessories);
            ViewBag.Ram = rams;
            ViewBag.Memory = memories;
            ViewBag.Color = colors;
            ViewBag.Category = categories;
            ViewBag.Brand = brands;

            return View(model);
        }
        public IActionResult DetailPhone(int id)
		{
            ViewBag.Ram = _context.ProductRamMemories
                             .Where(psc => psc.ProductId == id)
                             .Select(psc => psc.Ram)
                             .Distinct()
                             .Select(s => new { s.Id, s.RamName })
                             .ToList();

            ViewBag.Memory = _context.ProductRamMemories
                             .Where(psc => psc.ProductId == id)
                             .Select(psc => psc.Memory)
                             .Distinct()
                             .Select(s => new { s.Id, s.MemoryName })
                             .ToList();

            ViewBag.category = _context.Categories.ToList();

			if (id == 0) return BadRequest();
            Product? products = _context.Products.Include(x => x.ProductImages).Include(x => x.productCategories).Include(x=>x.ProductComments).Include(x => x.Brand).FirstOrDefault(x=>x.Id == id);
			if (products is null) return NotFound();
            return View(products);
		}

        public IActionResult DetailAccessory(int id)
        {
            ViewBag.Color = _context.AccessoryColors
                            .Where(psc => psc.AccessoryId == id)
                            .Select(psc => psc.Color)
                            .Distinct()
                            .Select(s => new { s.Id, s.ColorName })
                            .ToList();
            ViewBag.category = _context.Categories.ToList();
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

        [HttpPost]
        public IActionResult Index(int[] CategoryIds, int[] BrandIds, int[] RamIds, int[] MemoryIds, int[] ColorIds)
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
                products = products.Where(p => BrandIds.Any(x=>x.Equals(p.BrandId)));
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

            var productList = products.ToList();
            var accessoryList = accessories.ToList();

            return View(new Tuple<List<Product>, List<Accessory>>(productList, accessoryList));
        }


       


	}
}

