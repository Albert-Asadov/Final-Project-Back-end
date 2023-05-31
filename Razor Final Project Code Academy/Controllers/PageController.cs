using System;
using Microsoft.AspNetCore.Mvc;

namespace Razor_Final_Project_Code_Academy.Controllers
{
	public class PageController:Controller
	{
		public PageController()
		{
		}

		public IActionResult Index()
		{
			return View();
		}
	}
}

