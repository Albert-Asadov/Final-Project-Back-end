using System;
using Razor_Final_Project_Code_Academy.Migrations;

namespace Razor_Final_Project_Code_Academy.Entities
{
	public class BasketItem:BaseEntity
	{
        public decimal UnitPrice { get; set; }
        public int SaleQuantity { get; set; }

        public int BasketId { get; set; }
        public Basket Basket { get; set; }

        public int? ProductRamMemoryId{ get; set; } 
        public ProductRamMemory ProductRamMemory { get; set; }

        public bool IsAccessuar { get; set;}

        public int? accessoryColorId { get; set; } 
        public AccessoryColor AccessoryColor { get; set; }

    }
}

