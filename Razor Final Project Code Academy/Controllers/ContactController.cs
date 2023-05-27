using System;
using Microsoft.AspNetCore.Mvc;

namespace Razor_Final_Project_Code_Academy.Controllers
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

