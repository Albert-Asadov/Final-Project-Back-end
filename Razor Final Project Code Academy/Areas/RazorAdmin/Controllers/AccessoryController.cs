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
    public class AccessoryController:Controller
	{
        private readonly RazorDbContext _context;
        private readonly IWebHostEnvironment _env;

        public AccessoryController(RazorDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page = 1)
        {
            ViewBag.TotalPage = Math.Ceiling((double)_context.Accessories.Count() / 5);
            ViewBag.CurrentPage = page;

            IEnumerable<Accessory> accessories = _context.Accessories.Include(a => a.AccessoryImages).Include(x => x.Brand)
                                                        .Include(a => a.accessoryColors).ThenInclude(a => a.Color).OrderByDescending(x=>x.Id)
                                                         .AsNoTracking().Skip((page - 1) * 5).Take(5).AsEnumerable();
            return View(accessories);
        }

        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories.AsEnumerable();
            ViewBag.Brand = _context.Brands.AsEnumerable();
            ViewBag.Colors = _context.Colors.AsEnumerable();
            return View();

        }

        [HttpPost]
        public async Task<IActionResult> Create(AccessoryVM newAccessory)
        {
            ViewBag.Categories = _context.Categories.AsEnumerable();
            ViewBag.Brand = _context.Brands.AsEnumerable();
            ViewBag.Colors = _context.Colors.AsEnumerable();

            if (!ModelState.IsValid)
            {
                return View();
            }
            if (!newAccessory.MainPhoto.IsValidFile("image/"))
            {
                ModelState.AddModelError(string.Empty, "Please choose  coorect image file");
                return View();
            }
            if (!newAccessory.MainPhoto.IsValidLength(1))
            {
                ModelState.AddModelError(string.Empty, "Please choose image which size is maximum 1MB");
                return View();
            }
            newAccessory.DiscountPrice = newAccessory.Price - (newAccessory.Price * newAccessory.Discount / 100);
            Accessory accessory = new()
            {
                Name = newAccessory.Name,
                Descr = newAccessory.Desc,
                Price = newAccessory.Price,
                DiscountPrice = (decimal)newAccessory.DiscountPrice,
                Discount = (decimal)newAccessory.Discount,
                SKU = newAccessory.SKU,
                BrandId = newAccessory.BrandId
            };

            var imagefolderPath = Path.Combine(_env.WebRootPath, "assets", "images");
            foreach (var image in newAccessory.Images)
            {
                if (!image.IsValidFile("image/") || !image.IsValidLength(1))
                {
                    return View();
                }
                AccessoryImage accessoryImage = new()
                {
                    IsMain = false,
                    Image = await image.CreateImage(imagefolderPath, "Product")
                };
                accessory.AccessoryImages.Add(accessoryImage);
            }


            AccessoryImage main = new()
            {
                IsMain = true,
                Image = await newAccessory.MainPhoto.CreateImage(imagefolderPath, "Product")
            };
            accessory.AccessoryImages.Add(main);

            foreach (int id in newAccessory.CategoryIds)
            {
                AccessoryCategory accessoryCategory = new()
                {
                    CategoryId = id
                };
                accessory.AccessoryCategories.Add(accessoryCategory);
            }


            if (newAccessory.AccessoryColor is null)
            {
                ModelState.AddModelError("", "Please Select Color and Quantity");
                return View();
            }
            else
            {
                string[] colorSizeQuantities = newAccessory.AccessoryColor.Split(',');
                foreach (string colorSizeQuantity in colorSizeQuantities)
                {
                    string[] datas = colorSizeQuantity.Split('-');
                    AccessoryColor accessoryColor = new()
                    {
                        ColorId = int.Parse(datas[0]),
                        Quantity = (byte)int.Parse(datas[1])
                    };
                    if (accessoryColor.Quantity > 0)
                    {
                        accessory.InStock = true;
                    }
                    accessory.accessoryColors.Add(accessoryColor);
                }
            }

            _context.Accessories.Add(accessory);
            _context.SaveChanges();
            return RedirectToAction("Index", "Accessory");
        }

        public IActionResult Edit(int id)
        {
            if (id == 0) return BadRequest();
            AccessoryVM? model = EditedModel(id);
            ViewBag.Categories = _context.Categories.AsEnumerable();
            ViewBag.Brand = _context.Brands.AsEnumerable();
            ViewBag.Colors = _context.Colors.AsEnumerable();
            if (model is null) return BadRequest();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, AccessoryVM edited)
        {
            ViewBag.Categories = _context.Categories.AsEnumerable();
            ViewBag.Brand = _context.Brands.AsEnumerable();
            ViewBag.Colors = _context.Colors.AsEnumerable();
            AccessoryVM? model = EditedModel(id);

            Accessory? accessory = await _context.Accessories.Include(a => a.AccessoryImages).
                Include(a => a.AccessoryCategories)
                    .Include(a => a.accessoryColors).
                        ThenInclude(a => a.Color).
                            FirstOrDefaultAsync(a => a.Id == id);

            if (accessory is null) return BadRequest();

            IEnumerable<string> removables = accessory.AccessoryImages.Where(a => !edited.ImagesId.Contains(a.Id)).Select(i => i.Image).AsEnumerable();
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
                await AdjustPlantPhoto(true, edited.MainPhoto, accessory);
            }

            accessory.AccessoryImages.RemoveAll(a => !edited.ImagesId.Contains(a.Id));
            if (edited.Images is not null)
            {
                foreach (var item in edited.Images)
                {
                    if (!item.IsValidFile("image/") || !item.IsValidLength(2))
                    {
                        TempData["NonSelect"] += item.FileName;
                        continue;
                    }
                    AccessoryImage AccessoryImage = new()
                    {
                        IsMain = false,
                        Image = await item.CreateImage(imagefolderPath, "Product")
                    };
                    accessory.AccessoryImages.Add(AccessoryImage);
                }
            }
            if (edited.CategoryIds != null)
            {
                accessory.AccessoryCategories.RemoveAll(a => !edited.CategoryIds.Contains(a.CategoryId));
                foreach (int categoryId in edited.CategoryIds)
                {
                    Category category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);
                    if (category is not null)
                    {
                        AccessoryCategory accessoryCategory = new() { Category = category };
                        accessory.AccessoryCategories.Add(accessoryCategory);
                    }
                }
            }

            if (edited.AccessoryColor is not null)
            {
                string[] RamMemoryQuantities = edited.AccessoryColor.Split(',');
                foreach (string colorSizeQuantityLoop in RamMemoryQuantities)
                {
                    string[] datas = colorSizeQuantityLoop.Split('-');
                    AccessoryColor accessoryC = new()
                    {
                        ColorId = int.Parse(datas[0]),
                        Quantity = (byte)int.Parse(datas[1])
                    };
                    if (accessoryC.Quantity <= 0)
                    {
                        accessory.InStock = false;
                    }
                    var existingItem = accessory.accessoryColors.FirstOrDefault(a => a.ColorId == accessoryC.ColorId);
                    if (existingItem != null)
                    {
                        existingItem.Quantity = accessoryC.Quantity;
                    }
                    else
                    {
                        accessory.accessoryColors.Add(accessoryC);
                    }
                }
            }
            if (edited.AccessoryColorDelete is not null)
            {
                string[] AccesoryColorDeleteId = edited.AccessoryColorDelete.Split(',');
                foreach (string rid in AccesoryColorDeleteId)
                {
                    int accessoryColorId = int.Parse(rid);
                    var itemToDelete = accessory.accessoryColors.FirstOrDefault(a => a.Id == accessoryColorId);
                    if (itemToDelete != null)
                    {
                        accessory.accessoryColors.Remove(itemToDelete);
                    }

                }
            }

            accessory.Name = edited.Name;
            accessory.Price = edited.Price;
            accessory.Descr = edited.Desc;
            accessory.SKU = edited.SKU;
            accessory.Discount = (decimal)edited.Discount;
            accessory.BrandId = edited.BrandId;
            accessory.DiscountPrice = (decimal)(edited.Price - (edited.Price * edited.Discount / 100));
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Details(int id)
        {
            if (id == 0) return BadRequest();
            AccessoryVM? modelAcc = EditedModel(id);
            ViewBag.Categories = _context.Categories.AsEnumerable();
            ViewBag.Colors = _context.Colors.AsEnumerable();
            ViewBag.Brand = _context.Brands.FirstOrDefault(x => x.Id == modelAcc.BrandId);
            if (modelAcc is null) return BadRequest();
            return View(modelAcc);
        }

        public IActionResult Delete(int id)
        {
            if (id == 0) return BadRequest();
            AccessoryVM? modelAcc = EditedModel(id);
            ViewBag.Categories = _context.Categories.AsEnumerable();
            ViewBag.Colors = _context.Colors.AsEnumerable();
            ViewBag.Brand = _context.Brands.FirstOrDefault(x => x.Id == modelAcc.BrandId);
            if (modelAcc is null) return BadRequest();
            return View(modelAcc);
        }

        [HttpPost]
        public IActionResult Delete(int id, AccessoryVM deleteAccessory)
        {
            ViewBag.Brand = _context.Brands.FirstOrDefault(x => x.Id == deleteAccessory.BrandId);
            if (id != deleteAccessory.Id) return NotFound();
            Accessory? accessory = _context.Accessories.FirstOrDefault(s => s.Id == id);
            if (accessory is null) return NotFound();
            IEnumerable<string> removables = accessory.AccessoryImages.Where(p => !deleteAccessory.ImagesId.Contains(p.Id)).Select(i => i.Image).AsEnumerable();
            var imagefolderPath = Path.Combine(_env.WebRootPath, "assets", "images");

            foreach (string removable in removables)
            {
                string path = Path.Combine(imagefolderPath, "Product", removable);
                Files.DeleteImage(path);
            }
            _context.Accessories.Remove(accessory);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private AccessoryVM? EditedModel(int id)
        {
            AccessoryVM? model = _context.Accessories.Include(a => a.AccessoryImages).
                Include(a => a.AccessoryCategories)
                    .Include(a => a.accessoryColors).
                        ThenInclude(a => a.Color)
                                            .Select(a =>
                                                new AccessoryVM
                                                {
                                                    Id = a.Id,
                                                    Name = a.Name,
                                                    SKU = a.SKU,
                                                    Desc = a.Descr,
                                                    Price = a.Price,
                                                    Discount = a.Discount,
                                                    DiscountPrice = a.DiscountPrice,
                                                    CategoryIds = a.AccessoryCategories.Select(ac => ac.CategoryId).ToList(),
                                                    BrandId = a.BrandId,
                                                    AllImages = a.AccessoryImages.Select(ai => new AccessoryImage
                                                    {
                                                        Id = ai.Id,
                                                        Image = ai.Image,
                                                        IsMain = ai.IsMain
                                                    }).ToList(),
                                                    AccessoryColors = a.accessoryColors.Select(p => new AccessoryColor
                                                    {
                                                        Id = p.Id,
                                                        Color = p.Color

                                                    }).ToList()
                                                })
                                                .FirstOrDefault(p => p.Id == id);
            return model;
        }

        private async Task AdjustPlantPhoto(bool? ismain, IFormFile image, Accessory accessory)
        {
            var imagefolderPath = Path.Combine(_env.WebRootPath, "assets", "images");
            string filepath = Path.Combine(imagefolderPath, "Product", accessory.AccessoryImages.FirstOrDefault(p => p.IsMain == ismain).Image);
            Files.DeleteImage(filepath);
            accessory.AccessoryImages.FirstOrDefault(p => p.IsMain == ismain).Image = await image.CreateImage(imagefolderPath, "Product");
        }
    }
}

