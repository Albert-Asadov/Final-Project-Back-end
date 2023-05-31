using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Razor_Final_Project_Code_Academy.DAL;
using Razor_Final_Project_Code_Academy.Entities;
using Razor_Final_Project_Code_Academy.Utilities.ExtensionMethods;
using Razor_Final_Project_Code_Academy.ViewModel;

namespace Razor_Final_Project_Code_Academy.Areas.RazorAdmin.Controllers
{
    [Area("RazorAdmin")]

    public class ProductController : Controller
    {
        private readonly RazorDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductController(RazorDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page = 1)
        {
            ViewBag.TotalPage = Math.Ceiling((double)_context.Products.Count() / 5);
            ViewBag.CurrentPage = page;

            IEnumerable<Product> products = _context.Products.Include(p => p.ProductImages).Include(x=>x.Brand)
                                                        .Include(p => p.ProductRamMemories).ThenInclude(p => p.Ram)
                                                        .Include(p => p.ProductRamMemories).ThenInclude(p => p.Memory).
                                                         OrderByDescending(x => x.Id).AsNoTracking().Skip((page - 1) * 5).Take(5).AsEnumerable();
            return View(products);
        }


        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories.AsEnumerable();
            ViewBag.Brand = _context.Brands.AsEnumerable();
            ViewBag.Rams = _context.Rams.AsEnumerable();
            ViewBag.Memories = _context.Memories.AsEnumerable();
            return View();

        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductVM newProduct)
        {
            ViewBag.Categories = _context.Categories.AsEnumerable();
            ViewBag.Brand = _context.Brands.AsEnumerable();
            ViewBag.Rams = _context.Rams.AsEnumerable();
            ViewBag.Memories = _context.Memories.AsEnumerable();
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (!newProduct.MainPhoto.IsValidFile("image/"))
            {
                ModelState.AddModelError(string.Empty, "Please choose image file");
                return View();
            }
            if (!newProduct.MainPhoto.IsValidLength(1))
            {
                ModelState.AddModelError(string.Empty, "Please choose image which size is maximum 1MB");
                return View();
            }
            newProduct.DiscountPrice = newProduct.Price - (newProduct.Price * newProduct.Discount / 100);
            Product product = new()
            {
                Name = newProduct.Name,
                Descr = newProduct.Desc,
                Price = newProduct.Price,
                DiscountPrice = (decimal)newProduct.DiscountPrice,
                Discount = (decimal)newProduct.Discount,
                SKU = newProduct.SKU,
                BrandId = newProduct.BrandId
            };

            var imagefolderPath = Path.Combine(_env.WebRootPath, "assets", "images");
            foreach (var image in newProduct.Images)
            {
                if (!image.IsValidFile("image/") || !image.IsValidLength(1))
                {
                    return View();
                }
                ProductImage productImage = new()
                {
                    IsMain = false,
                    Image = await image.CreateImage(imagefolderPath, "Product")
                };
                product.ProductImages.Add(productImage);
            }


            ProductImage main = new()
            {
                IsMain = true,
                Image = await newProduct.MainPhoto.CreateImage(imagefolderPath, "Product")
            };
            product.ProductImages.Add(main);

            foreach (int id in newProduct.CategoryIds)
            {
                ProductCategory category = new()
                {
                    CategoryId = id
                };
                product.productCategories.Add(category);
            }


            if (newProduct.ProductRamMemory is null)
            {
                ModelState.AddModelError("", "Please Select Color,Size and Quantity");
                return View();
            }
            else
            {
                string[] colorSizeQuantities = newProduct.ProductRamMemory.Split(',');
                foreach (string colorSizeQuantity in colorSizeQuantities)
                {
                    string[] datas = colorSizeQuantity.Split('-');
                    ProductRamMemory productSizeColor = new()
                    {
                        RamId = int.Parse(datas[0]),
                        MemoryId = int.Parse(datas[1]),
                        Quantity = (byte)int.Parse(datas[2])
                    };
                    if (productSizeColor.Quantity > 0)
                    {
                        product.InStock = true;
                    }
                    product.ProductRamMemories.Add(productSizeColor);
                }
            }

            _context.Products.Add(product);
            _context.SaveChanges();
            return RedirectToAction("Index", "Product");
        }



        public IActionResult Edit(int id)
        {
            if (id == 0) return BadRequest();
            ProductVM? model = EditedModel(id);
            ViewBag.Categories = _context.Categories.AsEnumerable();
            ViewBag.Brand = _context.Brands.AsEnumerable();
            ViewBag.Rams = _context.Rams.AsEnumerable();
            ViewBag.Memories = _context.Memories.AsEnumerable();

            if (model is null) return BadRequest();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, ProductVM edited)
        {
            ViewBag.Categories = _context.Categories.AsEnumerable();
            ViewBag.Brand = _context.Brands.AsEnumerable();
            ViewBag.Rams = _context.Rams.AsEnumerable();
            ViewBag.Memories = _context.Memories.AsEnumerable();
            ProductVM? model = EditedModel(id);

            Product? product = await _context.Products.Include(p => p.ProductImages).
                Include(p => p.productCategories)
                    .Include(p => p.ProductRamMemories).
                        ThenInclude(p => p.Ram).
                          Include(p => p.ProductRamMemories).
                        ThenInclude(pc => pc.Memory).
                    FirstOrDefaultAsync(p => p.Id == id);

            if (product is null) return BadRequest();

            IEnumerable<string> removables = product.ProductImages.Where(p => !edited.ImagesId.Contains(p.Id)).Select(i => i.Image).AsEnumerable();
            var imagefolderPath = Path.Combine(_env.WebRootPath, "assets", "images");

            foreach (string removable in removables)
            {
                string path = Path.Combine(imagefolderPath, "Product", removable);
                Files.DeleteImage(path);
            }

            if (edited.MainPhoto is not null)
            {
                if (!edited.MainPhoto.IsValidFile("image/"))
                {
                    ModelState.AddModelError(string.Empty, "Please choose image file");
                    return View();
                }
                if (!edited.MainPhoto.IsValidLength(2))
                {
                    ModelState.AddModelError(string.Empty, "Please choose image which size is maximum 2MB");
                    return View();
                }
                await AdjustPlantPhoto(true, edited.MainPhoto, product);
            }

            product.ProductImages.RemoveAll(p => !edited.ImagesId.Contains(p.Id));
            if (edited.Images is not null)
            {
                foreach (var item in edited.Images)
                {
                    if (!item.IsValidFile("image/") || !item.IsValidLength(2))
                    {
                        TempData["NonSelect"] += item.FileName;
                        continue;
                    }
                    ProductImage productImage = new()
                    {
                        IsMain = false,
                        Image = await item.CreateImage(imagefolderPath, "Product")
                    };
                    product.ProductImages.Add(productImage);
                }
            }
            if (edited.CategoryIds != null)
            {
                product.productCategories.RemoveAll(pt => !edited.CategoryIds.Contains(pt.CategoryId));
                foreach (int categoryId in edited.CategoryIds)
                {
                    Category category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);
                    if (category is not null)
                    {
                        ProductCategory productCategory = new() { Category = category };
                        product.productCategories.Add(productCategory);
                    }
                }
            }

            if (edited.ProductRamMemory is not null)
            {
                string[] RamMemoryQuantities = edited.ProductRamMemory.Split(',');
                foreach (string colorSizeQuantityLoop in RamMemoryQuantities)
                {
                    string[] datas = colorSizeQuantityLoop.Split('-');
                    ProductRamMemory productRamM = new()
                    {
                        RamId = int.Parse(datas[0]),
                        MemoryId = int.Parse(datas[1]),
                        Quantity = (byte)int.Parse(datas[2])
                    };
                    if (productRamM.Quantity <= 0)
                    {
                        product.InStock = false;
                    }
                    var existingItem = product.ProductRamMemories.FirstOrDefault(p => p.RamId == productRamM.RamId && p.MemoryId == productRamM.MemoryId);
                    if (existingItem != null)
                    {
                        existingItem.Quantity = productRamM.Quantity;
                    }
                    else
                    {
                        product.ProductRamMemories.Add(productRamM);
                    }
                }
            }
            if (edited.ProductRamMemoryDelete is not null)
            {
                string[] ProductRamMemoryToDeleteIds = edited.ProductRamMemoryDelete.Split(',');
                foreach (string rid in ProductRamMemoryToDeleteIds)
                {
                    int productRamMemoryId = int.Parse(rid);
                    var itemToDelete = product.ProductRamMemories.FirstOrDefault(p => p.Id == productRamMemoryId);
                    if (itemToDelete != null)
                    {
                        product.ProductRamMemories.Remove(itemToDelete);
                    }

                }
            }

            product.Name = edited.Name;
            product.Price = edited.Price;
            product.Descr = edited.Desc;
            product.SKU = edited.SKU;
            product.Discount = (decimal)edited.Discount;
            product.BrandId = edited.BrandId;
            product.DiscountPrice = (decimal)(edited.Price - (edited.Price * edited.Discount / 100));
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private ProductVM? EditedModel(int id)
        {
            ProductVM? model = _context.Products.Include(p => p.ProductImages).
                Include(p => p.productCategories)
                    .Include(p => p.ProductRamMemories).
                        ThenInclude(p => p.Ram).
                          Include(p => p.ProductRamMemories).
                        ThenInclude(pc => pc.Memory)
                                            .Select(p =>
                                                new ProductVM
                                                {
                                                    Id = p.Id,
                                                    Name = p.Name,
                                                    SKU = p.SKU,
                                                    Desc = p.Descr,
                                                    Price = p.Price,
                                                    Discount = p.Discount,
                                                  
                                                    DiscountPrice = p.DiscountPrice,
                                                    CategoryIds = p.productCategories.Select(pc => pc.CategoryId).ToList(),
                                                    BrandId = p.BrandId,
                                                    AllImages = p.ProductImages.Select(p => new ProductImage
                                                    {
                                                        Id = p.Id,
                                                        Image = p.Image,
                                                        IsMain = p.IsMain
                                                    }).ToList(),
                                                    ProductRamMemories = p.ProductRamMemories.Select(p => new ProductRamMemory
                                                    {
                                                        Id = p.Id,
                                                        Ram = p.Ram,
                                                        Memory = p.Memory
                                                    }).ToList()
                                                })
                                                .FirstOrDefault(p => p.Id == id);
            return model;
        }

        private async Task AdjustPlantPhoto(bool? ismain, IFormFile image, Product product)
        {
            var imagefolderPath = Path.Combine(_env.WebRootPath, "assets", "images");
            string filepath = Path.Combine(imagefolderPath, "Product", product.ProductImages.FirstOrDefault(p => p.IsMain == ismain).Image);
            Files.DeleteImage(filepath);
            product.ProductImages.FirstOrDefault(p => p.IsMain == ismain).Image = await image.CreateImage(imagefolderPath, "Product");
        }




        public IActionResult Details(int id)
        {
            if (id == 0) return BadRequest();
            ProductVM? model = EditedModel(id);
            ViewBag.Categories = _context.Categories.AsEnumerable();
            ViewBag.Brand = _context.Brands.AsEnumerable();
            ViewBag.Rams = _context.Rams.AsEnumerable();
            ViewBag.Memories = _context.Memories.AsEnumerable();
            if (model is null) return BadRequest();
            return View(model);
        }

        public IActionResult Delete(int id)
        {
            if (id == 0) return BadRequest();
            ProductVM? model = EditedModel(id);
            ViewBag.Categories = _context.Categories.AsEnumerable();
            ViewBag.Rams = _context.Rams.AsEnumerable();
            ViewBag.Memories = _context.Memories.AsEnumerable();
            ViewBag.Brand = _context.Brands.FirstOrDefault(x => x.Id == model.BrandId);

            if (model is null) return BadRequest();
            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(int id, ProductVM deleteProduct)
        {
            ViewBag.Brand = _context.Brands.FirstOrDefault(x => x.Id == deleteProduct.BrandId);
            if (id != deleteProduct.Id) return NotFound();
            Product? product = _context.Products.FirstOrDefault(s => s.Id == id);
            if (product is null) return NotFound();
            IEnumerable<string> removables = product.ProductImages.Where(p => !deleteProduct.ImagesId.Contains(p.Id)).Select(i => i.Image).AsEnumerable();
            var imagefolderPath = Path.Combine(_env.WebRootPath, "assets", "images");

            foreach (string removable in removables)
            {
                string path = Path.Combine(imagefolderPath, "Product", removable);
                Files.DeleteImage(path);
            }
            _context.Products.Remove(product);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}

