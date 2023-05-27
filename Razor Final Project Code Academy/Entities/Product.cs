using System;
namespace Razor_Final_Project_Code_Academy.Entities
{
	public class Product:BaseEntity
	{
        public string Name { get; set; }

		public decimal Price { get; set; }

		public decimal Discount { get; set; }

		public decimal DiscountPrice { get; set; }

		public string SKU { get; set; }

		public int Quantity { get; set; }

		public int Count { get; set; }

		public string Descr { get; set; }

		public List<ProductImage> ProductImages { get; set; }

		public List<ProductCategory> productCategories { get; set; }

		public Brand Brand { get; set; }

		public int BrandId { get; set; }
	}
}

