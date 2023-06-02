using System;
using Razor_Final_Project_Code_Academy.Utilities;

namespace Razor_Final_Project_Code_Academy.Entities
{
	public class Basket:BaseEntity
	{
       
        public double TotalPrice { get; set; }
        public User User { get; set; }
        public Status status { get; set; }
        public List<BasketItem> BasketItems { get; set; } = null!;

        public Basket()
        {
            BasketItems = new List<BasketItem>();
        }
    }
}

