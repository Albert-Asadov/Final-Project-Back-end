using System;
using System.Security.Principal;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Razor_Final_Project_Code_Academy.Entities;
using Razor_Final_Project_Code_Academy.ViewModel;

namespace Razor_Final_Project_Code_Academy.Areas.RazorAdmin.Controllers
{
	public class AccountAdminController:Controller
	{
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountAdminController(UserManager<User> userManager, SignInManager<User> signInManager)
		{
           _userManager = userManager;
           _signInManager = signInManager;
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
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Login()
        {
            return View();
        }
	}
}

