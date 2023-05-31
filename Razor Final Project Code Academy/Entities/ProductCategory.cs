using System;
using Razor_Final_Project_Code_Academy.Entities;

namespace Razor_Final_Project_Code_Academy.Entities
{
	public class ProductCategory:BaseEntity
	{
		public int ProductId { get; set; }
		public Product Product { get; set; }

		public int CategoryId { get; set; }

		public Category Category { get; set; }

	}
}

