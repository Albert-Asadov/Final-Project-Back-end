using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Razor_Final_Project_Code_Academy.DAL;
using Razor_Final_Project_Code_Academy.Entities;

namespace Razor_Final_Project_Code_Academy.Controllers
{
    public class WishlistController : Controller
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



        public async Task<IActionResult> AddWishList(int Id)
        {

            Product product = await _context.Products.FindAsync(Id);

            if (product is null)
            {
                return NotFound();
            }

            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            User user = await _userManager.FindByNameAsync(User.Identity.Name);

            Wishlist? userWishList = await _context.Wishlists
                .FirstOrDefaultAsync(x => x.UserId == user.Id && x.ProductId == Id);

            if (userWishList is null)
            {
                userWishList = new Wishlist
                {
                    UserId = user.Id,
                    ProductId = Id,
                    IsAccessory = false
                };
                _context.Wishlists.Add(userWishList);
            }

            await _context.SaveChangesAsync();

            return Redirect(Request.Headers["Referer"].ToString());
        }

        public async Task<IActionResult> AddWishListAcc(int Id)
        {

            Accessory product = await _context.Accessories.FindAsync(Id);

            if (product is null)
            {
                return NotFound();
            }

            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            User user = await _userManager.FindByNameAsync(User.Identity.Name);

            Wishlist? userWishList = await _context.Wishlists
                .FirstOrDefaultAsync(x => x.UserId == user.Id && x.AccessoryId == Id);

            if (userWishList is null)
            {
                userWishList = new Wishlist
                {
                    UserId = user.Id,
                    AccessoryId = Id,
                    IsAccessory = true
                };
                _context.Wishlists.Add(userWishList);
            }

            await _context.SaveChangesAsync();

            return Redirect(Request.Headers["Referer"].ToString());
        }

        public async Task<IActionResult> WishlistRemove(int Id)
        {
            User user = await _userManager.FindByNameAsync(User.Identity.Name);
            Wishlist? wishlist = await _context.Wishlists
                .FirstOrDefaultAsync(x => x.UserId == user.Id && (x.ProductId == Id || x.AccessoryId==Id));

            if (wishlist is null)
            {
                return NotFound();
            }

            _context.Wishlists.Remove(wishlist);

            await _context.SaveChangesAsync();

            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}

 //|| x.UserId == user.Id && x.AccessoryId == Id)