using System;
namespace Razor_Final_Project_Code_Academy.Entities
{
	public class AccessoryColor:BaseEntity
	{
		public int AccessoryId { get; set; }

		public Accessory Accessory { get; set; }

		public int ColorId { get; set; }

		public Color Color { get; set; }

        public byte Quantity { get; set; }

        public List<BasketItem>? BasketItems { get; set; }

		public AccessoryColor()
		{
			BasketItems = new();

        }
    }
}

