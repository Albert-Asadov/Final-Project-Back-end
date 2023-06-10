using System;
using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Razor_Final_Project_Code_Academy.DAL;
using Razor_Final_Project_Code_Academy.Entities;
using Razor_Final_Project_Code_Academy.ViewModel.Roles;

namespace Razor_Final_Project_Code_Academy.Areas.RazorAdmin.Controllers
{
    [Area("RazorAdmin")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class OrderController:Controller
	{
        private readonly RazorDbContext _context;

        public OrderController(RazorDbContext context)
		{
            _context = context;
        }

        public IActionResult Index(int page = 1)
        {

            ViewBag.TotalPage = Math.Ceiling((double)_context.Orders.Count() / 5);
            ViewBag.CurrentPage = page;

            List<Order> orders = _context.Orders.AsNoTracking().Skip((page - 1) * 5).Take(5).ToList();
            return View(orders);
        }

        public IActionResult View(int id)
        {
            Order? order = _context.Orders.
                Include(x => x.OrderItems).
                ThenInclude(x => x.ProductRamMemory.Product).
                 Include(x => x.OrderItems).
                ThenInclude(x => x.AccessoryColor.Accessory).
                FirstOrDefault(x => x.Id == id);

            if (order is null) return NotFound();
            return View(order);
        }

        public IActionResult AcceptOrdersInfo(int id)
        {
            Order order = _context.Orders.Include(x => x.OrderItems).FirstOrDefault(x => x.Id == id);

            User user = _context.Users.FirstOrDefault(x => x.Id == order.UserId);
            if (order is null) return NotFound();

            order.Status = Razor_Final_Project_Code_Academy.Utilities.Status.Accepted;

            _context.SaveChanges();

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("razor.familysites@gmail.com", "Razor Admin");
            mail.To.Add(new MailAddress(user.Email));
            mail.Subject = "Purchase Information";
            mail.Body = "Thank you for your purchase! Your order is being processed and will be sent to you shortly. Youre Orders Total price is $" + order.TotalPrice;
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

        public IActionResult RejectsOrdersInfo(int id)
        {
            Order order = _context.Orders.FirstOrDefault(x => x.Id == id);

            User user = _context.Users.FirstOrDefault(x => x.Id == order.UserId);
            if (order is null) return NotFound();

            order.Status = Razor_Final_Project_Code_Academy.Utilities.Status.Rejected;

            _context.SaveChanges();


            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("razor.familysites@gmail.com", "Razor Admin");
            mail.To.Add(new MailAddress(user.Email));
            mail.Subject = "Purchase Information";
            mail.Body = "Unfortunately, we will not be able to ship your order due to unforeseen difficulties";
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
    }
}

