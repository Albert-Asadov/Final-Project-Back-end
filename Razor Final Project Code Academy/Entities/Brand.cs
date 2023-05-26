using System;
namespace Final_Project_Razor.Entities
{
	public class Brand:BaseEntity
	{
        public string Name { get; set; }

		public List<Product> Products { get; set; }
	}
}

