using System;
using Microsoft.AspNetCore.Mvc;

namespace Final_Project_Razor.Controllers
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

