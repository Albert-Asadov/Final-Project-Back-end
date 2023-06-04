using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Razor_Final_Project_Code_Academy.DAL;
using Razor_Final_Project_Code_Academy.Entities;
using Razor_Final_Project_Code_Academy.Utilities;
using Razor_Final_Project_Code_Academy.ViewModel;

namespace Razor_Final_Project_Code_Academy.Controllers
{
	public class BasketController:Controller
	{
        private readonly RazorDbContext _context;
        private readonly UserManager<User> _userManager;

        public BasketController(RazorDbContext context, UserManager<User> userManager)
		{
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> BasketCartAsync()
        {
            User? user = new();

            if (User.Identity.IsAuthenticated)
            {
                user = await _userManager.FindByNameAsync(User.Identity.Name);

            }
           List<Basket> basket = _context.Baskets.Include(x=>x.User).Include(x => x.BasketItems).ThenInclude(x => x.ProductRamMemory).Include(x => x.BasketItems).ThenInclude(x => x.AccessoryColor).Where(x=>x.status==0 && x.User.Id==user.Id).ToList();
            ViewBag.Product = _context.Products.Include(p => p.ProductImages).Include(x=>x.Brand).ToList();
            ViewBag.Acc = _context.Accessories.Include(p => p.AccessoryImages).Include(x => x.Brand).ToList();

            return View(basket);
        }
        public async Task<IActionResult> Addbaskets(int productId, Product? basketProduct)
        {
            User? user = new();

            if (User.Identity.IsAuthenticated)
            {
                user = await _userManager.FindByNameAsync(User.Identity.Name);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            Basket userActiveBasket = _context.Baskets
               .Include(b => b.User)
               .Include(b => b.BasketItems)
               .ThenInclude(i => i.ProductRamMemory)
               .FirstOrDefault(b => b.User.Id == user.Id && b.status == Status.Default);

            if (userActiveBasket is null)
            {
                userActiveBasket = new Basket()
                {
                    User = user,
                    BasketItems = new List<BasketItem>(),
                    status = Status.Default
                };
                _context.Baskets.Add(userActiveBasket);
            }

            if (basketProduct is not null)
            {
                ProductRamMemory? productRamMemory = _context.ProductRamMemories
                    .Include(p => p.Product)
                    .FirstOrDefault(p => p.ProductId == productId && p.RamId == basketProduct.AddCart.RamId && p.MemoryId == basketProduct.AddCart.MemoryId);

                if (productRamMemory is null) return NotFound();

                BasketItem items = userActiveBasket.BasketItems.FirstOrDefault(i => i.ProductRamMemory == productRamMemory);

                if (items is not null)
                {
                    items.SaleQuantity += basketProduct.AddCart.Quantity;
                }
                else
                {
                    items = new BasketItem
                    {
                        ProductRamMemory = productRamMemory,
                        SaleQuantity = basketProduct.AddCart.Quantity,
                        UnitPrice = (decimal)productRamMemory.Product.DiscountPrice,
                        Basket = userActiveBasket,
                        IsAccessuar = false
                    };
                    userActiveBasket.BasketItems.Add(items);
                }
            }

            userActiveBasket.TotalPrice = (double)userActiveBasket.BasketItems.Sum(p => p.SaleQuantity * p.UnitPrice);
            await _context.SaveChangesAsync();
            return Redirect(Request.Headers["Referer"].ToString());
        }


        public async Task<IActionResult> Addbasket(int productId, Accessory? basketaccessory)
        {
            User? user = new();

            if (User.Identity.IsAuthenticated)
            {
                user = await _userManager.FindByNameAsync(User.Identity.Name);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            Basket userActiveBasket = _context.Baskets
               .Include(b => b.User)
               .Include(b => b.BasketItems)
               .ThenInclude(i => i.ProductRamMemory)
               .FirstOrDefault(b => b.User.Id == user.Id && b.status == Status.Default);
            if (basketaccessory is not null)
            {
                AccessoryColor? accessorycolor = _context.AccessoryColors
                    .Include(p => p.Accessory)
                    .FirstOrDefault(p => p.AccessoryId == productId && p.ColorId == basketaccessory.Adding.ColorId);

                if (accessorycolor is null) return NotFound();

                BasketItem items = userActiveBasket.BasketItems.FirstOrDefault(i => i.AccessoryColor == accessorycolor);

                if (items is not null)
                {
                    items.SaleQuantity += basketaccessory.Adding.Quantity;
                }
                else
                {
                    items = new BasketItem
                    {
                        AccessoryColor = accessorycolor,
                        SaleQuantity = basketaccessory.Adding.Quantity,
                        UnitPrice = (decimal)accessorycolor.Accessory.DiscountPrice,
                        Basket = userActiveBasket,
                        IsAccessuar = true
                    };
                    userActiveBasket.BasketItems.Add(items);
                }
            }
            userActiveBasket.TotalPrice = (double)userActiveBasket.BasketItems.Sum(p => p.SaleQuantity * p.UnitPrice);
            await _context.SaveChangesAsync();
            return Redirect(Request.Headers["Referer"].ToString());
        }


        public async Task<IActionResult> AddToBasket(int productId, Product? basketProduct, Accessory? basketAccessory)
        {
            User? user = new();

            if (User.Identity.IsAuthenticated)
            {
                user = await _userManager.FindByNameAsync(User.Identity.Name);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

            Basket userActiveBasket = _context.Baskets
                .Include(b => b.User)
                .Include(b => b.BasketItems)
                .ThenInclude(i => i.ProductRamMemory)
                 .Include(b => b.BasketItems)
                .ThenInclude(i => i.AccessoryColor)
                .FirstOrDefault(b => b.User.Id == user.Id && b.status == Status.Default);

            if (userActiveBasket is null)
            {
                userActiveBasket = new Basket()
                {
                    User = user,
                    BasketItems = new List<BasketItem>(),
                    status = Status.Default
                };
                _context.Baskets.Add(userActiveBasket);
            }

            if (basketProduct is not null)
            {
                ProductRamMemory? productRamMemory = _context.ProductRamMemories
                    .Include(p => p.Product)
                    .FirstOrDefault(p => p.ProductId == productId && p.RamId == basketProduct.AddCart.RamId && p.MemoryId == basketProduct.AddCart.MemoryId);

                if (productRamMemory is null) return NotFound();

                BasketItem items = userActiveBasket.BasketItems.FirstOrDefault(i => i.ProductRamMemory == productRamMemory);

                if (items is not null)
                {
                    items.SaleQuantity += basketProduct.AddCart.Quantity;
                }
                else
                {
                    items = new BasketItem
                    {
                        ProductRamMemory = productRamMemory,
                        SaleQuantity = basketProduct.AddCart.Quantity,
                        UnitPrice = (decimal)productRamMemory.Product.DiscountPrice,
                        Basket = userActiveBasket
                    };
                    userActiveBasket.BasketItems.Add(items);
                }
            }

            if (basketAccessory is not null)
            {
                AccessoryColor? accessoryColor = _context.AccessoryColors
                    .Include(p => p.Accessory)
                    .FirstOrDefault(p => p.AccessoryId == productId && p.ColorId == basketAccessory.Adding.ColorId);

                if (accessoryColor is null) return NotFound();

                BasketItem items = userActiveBasket.BasketItems.FirstOrDefault(i => i.AccessoryColor == accessoryColor);

                if (items is not null)
                {
                    items.SaleQuantity += basketAccessory.Adding.Quantity;
                }
                else
                {
                    items = new BasketItem
                    {
                        AccessoryColor = accessoryColor,
                        SaleQuantity = basketAccessory.Adding.Quantity,
                        UnitPrice = (decimal)accessoryColor.Accessory.DiscountPrice,
                        Basket = userActiveBasket
                    };
                    userActiveBasket.BasketItems.Add(items);
                }
            }

            userActiveBasket.TotalPrice = (double)userActiveBasket.BasketItems.Sum(p => p.SaleQuantity * p.UnitPrice);
            await _context.SaveChangesAsync();
            return Redirect(Request.Headers["Referer"].ToString());
        }


        public async Task<IActionResult> DeletBasketItem(int basketItemId)
        {
            User? user = null; if (User.Identity.IsAuthenticated)
            {
                user = await _userManager.FindByNameAsync(User.Identity.Name);
            }
            BasketItem item = _context.BasketItems.FirstOrDefault(i => i.Id == basketItemId);

            if (item is not null)
            {
                Basket userActiveBasket = _context.Baskets
                    .Include(b => b.User)
                    .Include(b => b.BasketItems)
                    .ThenInclude(i => i.ProductRamMemory)
                    .FirstOrDefault(b => b.User.Id == user.Id && b.status == 0);

                if (userActiveBasket is not null)
                {
                    userActiveBasket.BasketItems.Remove(item);
                    userActiveBasket.TotalPrice = (double)userActiveBasket.BasketItems.Sum(p => p.SaleQuantity * p.UnitPrice);
                    await _context.SaveChangesAsync();
                }
            }
            return Redirect(Request.Headers["Referer"].ToString());
        }


    }
}

