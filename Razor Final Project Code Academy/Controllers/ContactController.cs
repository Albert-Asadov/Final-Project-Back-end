using System;
using Microsoft.AspNetCore.Mvc;

namespace Final_Project_Razor.Controllers
{
	public class ContactController:Controller
	{
		public ContactController()
		{
		}


		public IActionResult Index()
		{
			return View();
		}
	}
}

