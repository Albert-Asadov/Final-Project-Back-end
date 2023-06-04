using System;
namespace Razor_Final_Project_Code_Academy.Entities
{
	public class Wishlist:BaseEntity
	{

        public int? ProductId { get; set; }

		public Product Product { get; set; }

		public int? AccessoryId { get; set; }

		public Accessory Accessory { get; set; }

		public bool IsAccessory { get; set; }

		public string UserId { get; set; }

		public User User { get; set; }

	}
}

