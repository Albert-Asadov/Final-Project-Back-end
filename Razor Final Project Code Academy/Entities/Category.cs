using System;
namespace Final_Project_Razor.Entities
{
	public class Category:BaseEntity
	{
        public string Name { get; set; }

        public List<ProductCategory> productCategories { get; set; }
    }
}

