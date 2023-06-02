using System;
namespace Razor_Final_Project_Code_Academy.Entities
{
	public class OrderItem:BaseEntity
	{
        public int OrderId { get; set; }

        public Order Order { get; set; }

        public decimal UnitPrice { get; set; }

        public int SaleQuantity { get; set; }

        public int? ProductRamMemoryId { get; set; }
        public ProductRamMemory ProductRamMemory { get; set; }

        public int? AccessoryColorId { get; set; }
        public AccessoryColor AccessoryColor { get; set; }
    }
}

