using System;
namespace Final_Project_Razor.Entities
{
	public class ProductCategory:BaseEntity
	{
        public Product Product { get; set; }

		public Category Category { get; set; }
	}
}

