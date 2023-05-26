using System;
namespace Final_Project_Razor.Entities
{
	public class ProductImage:BaseEntity
	{
		public bool IsMain { get; set; }

        public string Image { get; set; }

		public Product Product { get; set; }

		public int ProductId { get; set; }

	}
}

