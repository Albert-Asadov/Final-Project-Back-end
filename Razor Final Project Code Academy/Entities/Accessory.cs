using System;
using Razor_Final_Project_Code_Academy.Entities;

namespace Razor_Final_Project_Code_Academy.Entities
{
	public class Accessory:BaseEntity
	{
		public string Name { get; set; }

		public decimal Price { get; set; }

		public decimal Discount { get; set; }

		public decimal DiscountPrice { get; set; }

		public string SKU { get; set; }

		public bool InStock { get; set; }

		public int Count { get; set; }

		public string Descr { get; set; }

		public List<AccessoryImage> AccessoryImages { get; set; }

		public List<AccessoryCategory> AccessoryCategories { get; set; }

		public Brand Brand { get; set; }

		public int BrandId { get; set; }

        public List<Comment> AccessoryComments { get; set; }

        public List<AccessoryColor> accessoryColors { get; set; }

        public Accessory()
		{
			AccessoryImages = new();
            accessoryColors = new();
            AccessoryComments = new();
			AccessoryCategories = new();
        }
    }
}

