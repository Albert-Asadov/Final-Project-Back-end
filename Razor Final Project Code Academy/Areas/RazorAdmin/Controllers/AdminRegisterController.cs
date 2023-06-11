using System;
using System.Security.Principal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Razor_Final_Project_Code_Academy.DAL;
using Razor_Final_Project_Code_Academy.Entities;
using Razor_Final_Project_Code_Academy.ViewModel;
using Razor_Final_Project_Code_Academy.ViewModel.Roles;

namespace Razor_Final_Project_Code_Academy.Areas.RazorAdmin.Controllers
{
    [Area("RazorAdmin")]
    [Authorize(Roles = "SuperAdmin")]
    public class AdminRegisterController:Controller
	{
        private readonly RazorDbContext _context;
        private readonly UserManager<User> _userManager;

        public AdminRegisterController(RazorDbContext context, UserManager<User> userManager)
		{
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM account)
        {
            if (!ModelState.IsValid) return View();
            User user = new()
            {
                Fullname = string.Concat(account.FirstName, " ", account.LastName),
                Email = account.Email,
                UserName = account.UserName
            };
            if (_userManager.Users.Any(x => x.NormalizedEmail == account.Email.ToUpper()))
            {
                ModelState.AddModelError("Email", "Bu e-poçtda istifadəçi mövcuddur");
                return View();
            }
            IdentityResult result = await _userManager.CreateAsync(user, account.Password);
            if (!result.Succeeded)
            {
                foreach (IdentityError message in result.Errors)
                {
                    ModelState.AddModelError("", message.Description);
                }
                return View();
            }

            await _userManager.AddToRoleAsync(user, Roles.Admin.ToString());

            return RedirectToAction("Index", "Diagram");
        }
    }
}

