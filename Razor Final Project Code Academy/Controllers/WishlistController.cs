using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Razor_Final_Project_Code_Academy.DAL;
using Razor_Final_Project_Code_Academy.Entities;

namespace Razor_Final_Project_Code_Academy.Controllers
{
	public class WishlistController:Controller
	{
        private readonly RazorDbContext _context;
        private readonly UserManager<User> _userManager;

        public WishlistController(RazorDbContext context, UserManager<User> userManager)
		{
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return View(new List<Wishlist>());
            }

            var userId = _userManager.GetUserId(User);

            List<Wishlist> wishlists = _context.Wishlists
                                .Include(x => x.Accessory)
                                .Include(x => x.Accessory)
                                .ThenInclude(x => x.AccessoryImages)
                                .Include(x => x.Product)
                                .Include(x => x.Product)
                                .ThenInclude(x => x.ProductImages)
                                .Where(wli => wli.UserId == userId).ToList();

            if (wishlists.Count == 0)
            {
                return View(new List<Wishlist>());
            }

            return View(wishlists);
        }


    }
}

