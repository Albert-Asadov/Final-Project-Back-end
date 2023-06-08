using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Razor_Final_Project_Code_Academy.Entities;

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


	}
}

