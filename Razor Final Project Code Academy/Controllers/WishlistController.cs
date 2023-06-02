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
            return View();
        }

        //public async Task<IActionResult> AddToWishList(int productId)
        //{
        //    Product product = await _context.Products.FindAsync(productId);

        //    if (product is null)
        //    {
        //        return NotFound();
        //    }

        //    if (!User.Identity.IsAuthenticated)
        //    {
        //        return RedirectToAction("Login", "Account");
        //    }

        //    User user = await _userManager.FindByNameAsync(User.Identity.Name);

        //    WishList wishList = await _context.WishListItems
        //        .FirstOrDefaultAsync(x => x.UserId == user.Id && x.ProductId == productId);

        //    if (userWishlistItem is null)
        //    {
        //        userWishlistItem = new WishListItem
        //        {
        //            UserId = user.Id,
        //            ProductId = productId
        //        };
        //        _context.WishListItems.Add(userWishlistItem);
        //    }

        //    await _context.SaveChangesAsync();

        //    return RedirectToAction(nameof(Index));
        //}

    }
}

