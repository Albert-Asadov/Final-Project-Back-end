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
	public class OrderController:Controller
	{
        private readonly RazorDbContext _context;
        private readonly UserManager<User> _userManager;

        public OrderController(RazorDbContext context, UserManager<User> userManager)
		{
            _context = context;
           _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            CheckOutVM checkoutVM = new();
            User? user = new();

            if (User.Identity.IsAuthenticated)
            {
                user = await _userManager.FindByNameAsync(User.Identity.Name);
            }

            List<Basket> basket = _context.Baskets.Include(x => x.User).Include(x => x.BasketItems).ThenInclude(x => x.ProductRamMemory).Include(x => x.BasketItems).ThenInclude(x => x.AccessoryColor).Where(x => x.status == 0 && x.User.Id == user.Id).ToList();
            ViewBag.Product = _context.Products.Include(x => x.ProductImages).Include(x => x.Brand).ToList();
            ViewBag.Acc = _context.Accessories.Include(x => x.AccessoryImages).Include(x => x.Brand).ToList();
            checkoutVM.Email = user.Email;
            checkoutVM.FullName = user.Fullname;

            checkoutVM.BasketItems = _context.BasketItems
                .Include(x => x.ProductRamMemory.Product)
                .Where(x => x.Basket.User.Id == user.Id)
                .ToList();

            decimal totalPrice = 0;

            foreach (var item in checkoutVM.BasketItems)
            {
                totalPrice += item.SaleQuantity * item.UnitPrice;
            }

            checkoutVM.TotalPrice = totalPrice;

            return View(checkoutVM);
        }


        [HttpPost]
        public async Task<IActionResult> Index(CheckOutVM model)
        {
            ViewBag.Product = _context.Products.Include(x => x.ProductImages).Include(x => x.Brand).ToList();
            ViewBag.Acc = _context.Accessories.Include(x => x.AccessoryImages).Include(x => x.Brand).ToList();
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            User user = await _userManager.GetUserAsync(User);

            Basket basket = new Basket { User = user, status = Status.Default };
            _context.Baskets.Add(basket);

            await _context.SaveChangesAsync();

            if (model.Note is null)
            {
                ModelState.AddModelError("Note", "Please write some note.");
                return View();
            }

            if (model.Address is null)
            {
                ModelState.AddModelError("Address", "Please write address.");
                return View();
            }
            if (model.Contry is null)
            {
                ModelState.AddModelError("Address", "Please write youre Contry Name.");
                return View();
            }
            if (model.City is null)
            {
                ModelState.AddModelError("Address", "Please write youre City Name.");
                return View();
            }
            if (model.Zip is null)
            {
                ModelState.AddModelError("Address", "Please write youre Zip Code.");
                return View();
            }
            if (model.Number is null)
            {
                ModelState.AddModelError("Address", "Please write youre Phone Number.");
                return View();
            }
            Order order = new Order
            {
                FullName = model.FullName,
                Email = model.Email,
                Address = model.Address,
                Number = model.Number,
                City = model.City,
                Contry = model.Contry,
                Zip = model.Zip,
                Note = model.Note,
                CreatedTime = DateTime.Now,
                Status = Status.Pending,
                UserId = user.Id,
                BasketId = basket.Id,
                TotalPrice = 0,
                OrderItems = new List<OrderItem>()
            };


            decimal totalPrice = 0;

            if (model.BasketItems != null && model.BasketItems.Any(x => x.IsAccessuar == false))
            {
                foreach (BasketItem basketItem in model.BasketItems.Where(x => x.IsAccessuar == false))
                {
                    ProductRamMemory? productRamMemory = await _context.ProductRamMemories
                        .Include(x => x.Product)
                        .FirstOrDefaultAsync(psc => psc.Id == basketItem.ProductRamMemoryId);

                    if (productRamMemory.Quantity == 0)
                    {
                        var orderItems = _context.OrderItems.Where(o => o.ProductRamMemoryId == productRamMemory.Id).ToList();
                        _context.OrderItems.RemoveRange(orderItems);

                        var basketItems = _context.BasketItems.Where(b => b.ProductRamMemoryId == productRamMemory.Id).ToList();
                        _context.BasketItems.RemoveRange(basketItems);

                        _context.ProductRamMemories.Remove(productRamMemory);

                        _context.SaveChanges();

                        return View(model);
                    }

                    if (basketItem.SaleQuantity > productRamMemory.Quantity)
                    {
                        ModelState.AddModelError(" ", "The selected quantity is not available in stock.");
                        return View(model);
                    }

                    OrderItem orderItem = new OrderItem
                    {
                        SaleQuantity = basketItem.SaleQuantity,
                        UnitPrice = (decimal)productRamMemory.Product.DiscountPrice,
                        ProductRamMemoryId = basketItem.ProductRamMemoryId,
                        ProductRamMemory = productRamMemory,
                        IsAccessuar=false
                    };

                    order.OrderItems.Add(orderItem);

                    decimal itemTotalPrice = orderItem.UnitPrice * orderItem.SaleQuantity;
                    totalPrice += itemTotalPrice;
               
                    productRamMemory.Quantity = (byte)(productRamMemory.Quantity - basketItem.SaleQuantity);
                    productRamMemory.Product.Count++;
                   
                }
                
            }

            if (model.BasketItems != null && model.BasketItems.Any(x => x.IsAccessuar == true))
            {
                foreach (BasketItem basketItem in model.BasketItems.Where(x => x.IsAccessuar == true))
                {
                    AccessoryColor? accessoryColor = await _context.AccessoryColors
                        .Include(p => p.Accessory)
                        .FirstOrDefaultAsync(psc => psc.Id == basketItem.accessoryColorId);

                    if (accessoryColor.Quantity == 0)
                    {
                        var orderItems = _context.OrderItems.Where(o => o.AccessoryColorId == accessoryColor.Id).ToList();
                        _context.OrderItems.RemoveRange(orderItems);

                        var basketItems = _context.BasketItems.Where(b => b.accessoryColorId == accessoryColor.Id).ToList();
                        _context.BasketItems.RemoveRange(basketItems);

                        _context.AccessoryColors.Remove(accessoryColor);

                        _context.SaveChanges();

                        return View(model);
                    }

                    if (basketItem.SaleQuantity > accessoryColor.Quantity)
                    {
                        ModelState.AddModelError(" ", "The selected quantity is not available in stock.");
                        return View(model);
                    }

                    var orderItem = new OrderItem
                    {
                        SaleQuantity = basketItem.SaleQuantity,
                        UnitPrice = (decimal)accessoryColor.Accessory.DiscountPrice,
                        AccessoryColorId = basketItem.accessoryColorId,
                        AccessoryColor = accessoryColor,
                        IsAccessuar=true
                    };

                    order.OrderItems.Add(orderItem);

                    decimal itemTotalPrice = orderItem.UnitPrice * orderItem.SaleQuantity;
                    totalPrice += itemTotalPrice;
           
                    accessoryColor.Quantity = (byte)(accessoryColor.Quantity - basketItem.SaleQuantity);
                    accessoryColor.Accessory.Count++;
                }
            }

            order.TotalPrice = totalPrice;

            _context.Orders.Add(order);
            _context.BasketItems.RemoveRange(_context.BasketItems.Where(x => x.Basket.User.Id == user.Id));
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }




        public IActionResult AccountOrders()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return View(new List<Wishlist>());
            }

            var userId = _userManager.GetUserId(User);
            List<Order> orderItems = _context.Orders.Include(x => x.OrderItems)
                                .ThenInclude(x => x.AccessoryColor)
                                .ThenInclude(x => x.Accessory)
                                .ThenInclude(x => x.AccessoryImages)
                                .Include(x => x.OrderItems)
                                .ThenInclude(x => x.ProductRamMemory)
                                .ThenInclude(x => x.Product)
                                .ThenInclude(x => x.ProductImages)
                                .Where(wl => wl.UserId == userId && wl.Status==Status.Accepted).ToList();

            if(orderItems.Count == 0)
            {
                return View(new List<Order>());
            }

            return View(orderItems);
        }
    }
}

