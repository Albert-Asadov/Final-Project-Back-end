using System;
namespace Razor_Final_Project_Code_Academy.Entities
{
	public class ProductImage:BaseEntity
	{
		public bool IsMain { get; set; }

        public string Image { get; set; }

		public Product Product { get; set; }

		public int ProductId { get; set; }

	}
}

