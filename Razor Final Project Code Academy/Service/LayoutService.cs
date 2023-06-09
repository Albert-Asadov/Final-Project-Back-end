using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Razor_Final_Project_Code_Academy.DAL;
using Razor_Final_Project_Code_Academy.Entities;
using Razor_Final_Project_Code_Academy.Utilities;

namespace Razor_Final_Project_Code_Academy.Service
{
	public class LayoutService
	{
        readonly RazorDbContext _context;
        readonly IHttpContextAccessor _accessor;
        private readonly UserManager<User> _userManager;

        public LayoutService(RazorDbContext context, IHttpContextAccessor accessor, UserManager<User> userManager)
        {
            _context = context;
            _accessor = accessor;
            _userManager = userManager;
        }

        public List<Setting> GetSettings()
        {
            List<Setting> settings = _context.Settings.ToList();
            return settings;
        }

        public List<Category> AllCategories()
        {
            List<Category> categories = _context.Categories.ToList();

            return categories;
        }

        public List<Brand> AllBrands()
        {
            List<Brand> Brands = _context.Brands.ToList();

            return Brands;
        }

        public async Task <User> AllUsers()
        {
            User user = await _userManager.GetUserAsync(_accessor.HttpContext.User);

            return user;
        }

        public List<BasketItem> BaskeItem()
        {
            User user = new();

            if (_accessor.HttpContext.User.Identity.IsAuthenticated)
            {
                user = _userManager.Users.FirstOrDefault(x => x.UserName == _accessor.HttpContext.User.Identity.Name);
            }

            List<BasketItem> basket = _context.BasketItems.Include(x => x.ProductRamMemory.Product).ThenInclude(x => x.ProductImages).Include(x => x.AccessoryColor.Accessory).ThenInclude(x=>x.AccessoryImages).Include(p => p.Basket).Where(x => x.Basket.User.Id == user.Id && x.Basket.status == Status.Default).ToList();

            return basket;
        }


      

        public List<Product> Products()
        {
            List<Product> products = _context.Products.Include(p => p.ProductImages).ToList();

            return products;
        }
        public List<Accessory> Accessories()
        {
            List<Accessory> accessories = _context.Accessories.Include(p => p.AccessoryImages).ToList();

            return accessories;
        }


    }
}

