using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Razor_Final_Project_Code_Academy.DAL;
using Razor_Final_Project_Code_Academy.Entities;
using Razor_Final_Project_Code_Academy.ViewModel.Roles;

namespace Razor_Final_Project_Code_Academy.Areas.RazorAdmin.Controllers
{
    [Area("RazorAdmin")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class DeliveryController:Controller
	{
        private readonly RazorDbContext _context;

        public DeliveryController(RazorDbContext context)
		{
            _context = context;
        }

        public IActionResult Index()
        {
            IEnumerable<DeliveryInformation> deliveryInformation = _context.DeliveryInformations.AsEnumerable();

            return View(deliveryInformation);
        }

        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DeliveryInformation createdDelivery)
        {
            DeliveryInformation delivery = new()
            {
               AboutReturnRequest = createdDelivery.AboutReturnRequest,

               Shipping = createdDelivery.Shipping
            };

            _context.DeliveryInformations.Add(delivery);

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            if (id == 0) return NotFound();

            DeliveryInformation? deliveryInformation = _context.DeliveryInformations.First();

            if (deliveryInformation is null)
            {
                return BadRequest();
            }
            return View(deliveryInformation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, DeliveryInformation EditedDelivery)
        {
            if (id == 0) return NotFound();

            DeliveryInformation? deliveryInformation = _context.DeliveryInformations.First();

            if (deliveryInformation is null)
            {
                return BadRequest();
            }

            bool ContainsReturn = _context.DeliveryInformations.Any(c => c.AboutReturnRequest == EditedDelivery.AboutReturnRequest);
            bool ContainsShipping = _context.DeliveryInformations.Any(c => c.Shipping == EditedDelivery.Shipping);


            if (ContainsReturn == true)
            {
                ModelState.AddModelError("", "You Have This AboutReturnRequest Text");

                return View();
            }

            if (ContainsShipping == true)
            {
                ModelState.AddModelError("", "You Have This Shipping Text");

                return View();
            }

            deliveryInformation.AboutReturnRequest = EditedDelivery.AboutReturnRequest;

            deliveryInformation.Shipping = EditedDelivery.Shipping;

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }


        public IActionResult Detail(int id)
        {
            if (id == 0) return NotFound();

            DeliveryInformation? deliveryInformation = _context.DeliveryInformations.First();

            if (deliveryInformation is null)
            {
                return BadRequest();
            }

            return View(deliveryInformation);
        }

        public IActionResult Delete(int id)
        {
            if (id == 0) return NotFound();

            DeliveryInformation? deliveryInformation = _context.DeliveryInformations.First();

            _context.Remove(deliveryInformation);

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}

