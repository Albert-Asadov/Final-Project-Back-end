using System;
using Microsoft.AspNetCore.Mvc;
using Razor_Final_Project_Code_Academy.DAL;
using Razor_Final_Project_Code_Academy.Entities;

namespace Razor_Final_Project_Code_Academy.Controllers
{
	public class PageController:Controller
	{
        private readonly RazorDbContext _context;

        public PageController(RazorDbContext context)
		{
            _context = context;
        }

		public IActionResult Index()
		{
			DeliveryInformation deliveryInformation = _context.DeliveryInformations.First();

			return View(deliveryInformation);
		}
	}
}

