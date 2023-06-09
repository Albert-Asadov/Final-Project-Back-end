using System;
using Razor_Final_Project_Code_Academy.Entities;
using Razor_Final_Project_Code_Academy.Utilities;

namespace Razor_Final_Project_Code_Academy.ViewModel
{
	public class CheckOutVM
	{
        public string FullName { get; set; }
        public string Contry { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public string Number { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Note { get; set; }
        public List<BasketItem> BasketItems { get; set; }
        public decimal TotalPrice { get; set; }
        public Status Status { get; set; }
    }
}

