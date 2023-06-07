using System;
using Microsoft.AspNetCore.Mvc;
using Razor_Final_Project_Code_Academy.DAL;

namespace Razor_Final_Project_Code_Academy.Areas.RazorAdmin.Controllers
{
	public class AccountController:Controller
	{
        private readonly RazorDbContext _context;

        public AccountController(RazorDbContext context)
		{
            _context = context;
        }
	}
}

