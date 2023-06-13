using System;
using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Razor_Final_Project_Code_Academy.DAL;
using Razor_Final_Project_Code_Academy.Entities;
using Razor_Final_Project_Code_Academy.Migrations;
using Razor_Final_Project_Code_Academy.ViewModel.Roles;

namespace Razor_Final_Project_Code_Academy.Areas.RazorAdmin.Controllers
{
    [Area("RazorAdmin")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class ContactController:Controller
	{
        private readonly RazorDbContext _context;

        public ContactController(RazorDbContext context)
		{
            _context = context;
        }

        public IActionResult Index(int page =1)
        {
            ViewBag.TotalPage = Math.Ceiling((double)_context.Brands.Count() / 10);
            ViewBag.CurrentPage = page;

            IEnumerable<ContactUs> contact = _context.contactUs.Skip((page - 1) * 10).Take(10).AsEnumerable();

            return View(contact);
        }

        public IActionResult Edit(int id)
        {
            if (id == 0) return NotFound();
           ContactUs contact = _context.contactUs.FirstOrDefault(x=>x.Id == id);

            if (contact is null) return BadRequest();

            return View(contact);
        }
        [HttpPost]
        public IActionResult Edit(int id , string comment)
        {
            if (id == 0) return NotFound();
            ContactUs contact = _context.contactUs.FirstOrDefault(x => x.Id == id);

         
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("razor.familysites@gmail.com", "Razor Admin");
                mail.To.Add(new MailAddress(contact.Email));
                mail.Subject = "Contact Replace";
                mail.Body = comment;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("razor.familysites@gmail.com", "lrzxazlzxwtshywn");
                smtp.Send(mail);

           
            return RedirectToAction("index");
        }

        public IActionResult Delete(int id)
        {
            if (id == 0) return NotFound();

            ContactUs? contact = _context.contactUs.FirstOrDefault(c => c.Id == id);

            _context.Remove(contact);

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
	}
}

