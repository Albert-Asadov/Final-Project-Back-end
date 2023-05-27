using System;
using Microsoft.AspNetCore.Mvc;

namespace Razor_Final_Project_Code_Academy.Controllers
{
	public class WelcomeController:Controller
	{
		public WelcomeController()
		{
		}

		public IActionResult Index()
		{
			return View();
		}
	}
}

