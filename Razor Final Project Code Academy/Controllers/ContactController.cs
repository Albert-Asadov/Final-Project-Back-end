using System;
using Microsoft.AspNetCore.Mvc;
using Razor_Final_Project_Code_Academy.DAL;
using Razor_Final_Project_Code_Academy.Entities;
using Razor_Final_Project_Code_Academy.ViewModel;

namespace Razor_Final_Project_Code_Academy.Controllers
{
	public class ContactController:Controller
	{
        private readonly RazorDbContext _context;

        public ContactController(RazorDbContext context)
		{
            _context = context;
        }

		public IActionResult Index()
		{
   
            return View();
		}

        [HttpPost]
        public IActionResult Index(ContactUsVM contactUsVM)
        {
            ContactUs contactUs = new()
            {
                FullName = contactUsVM.FullName,
                PhoneNumber = contactUsVM.PhoneNumber,
                Email = contactUsVM.Email,
                Comments = contactUsVM.Comments
            };
            _context.contactUs.Add(contactUs);
            _context.SaveChanges();
            return Redirect(Request.Headers["Referer"].ToString());
        }



    }
}

