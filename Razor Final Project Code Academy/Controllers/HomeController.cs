using System;
using Microsoft.AspNetCore.Mvc;

namespace Final_Project_Razor.Controllers
{
	public class HomeController:Controller
	{
		public HomeController()
		{
		}

		public IActionResult Index()
		{
			return View();
		}
	}
}

